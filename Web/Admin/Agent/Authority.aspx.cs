using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Agent
{
    public partial class Authority : BasePage
    {
        protected List<Sys_Privage> listAdmin, listOperator;
        List<Sys_RolePower> listMenuPowers = new List<Sys_RolePower>();
        protected string mtype;
        protected override void SetPowerZone()
        {
            listMenuPowers = TModel.PowerList.Where(emp => !emp.IsDeleted).ToList();
            //只加载代理商自己拥有的权限，包括菜单权限和操作权限。
            var list = from menu in CacheService.PrivageList
                       join srp in listMenuPowers on menu.Id equals srp.PrivageId
                       where menu.ParentCode == "0" && menu.IsDeleted == false
                       orderby menu.MenuIndex
                       select menu;
            listAdmin = list.Where(c => c.PrivageType == 1).ToList();
            listOperator = list.Where(c => c.PrivageType == 2).ToList();
            //listAdmin = CommonBase.GetList<Sys_Privage>("  PrivageType=1 and ParentCode='0' Order by MenuIndex");
            //listOperator = CommonBase.GetList<Sys_Privage>("PrivageType=2");
        }
        protected override void SetValue(string id)
        {
            hidCode.Value = id;
            Model.Member member = CommonBase.GetModel<Model.Member>(id);
            if (member != null)
            {
                spAgent.InnerHtml = member.Role.Name;
                spMID.InnerHtml = member.MID;
                spName.InnerHtml = member.MName;
                spAgentAddress.InnerHtml = GetAgentAddress(member.AreaId);
            }
        }

        protected IEnumerable<Sys_Privage> GetSecondLeavelDict(string cfid)
        {
            var list = from menu in CacheService.PrivageList
                       join srp in listMenuPowers on menu.Id equals srp.PrivageId
                       where menu.ParentCode == cfid
                       orderby menu.MenuIndex
                       select menu;
            return list;
            //return CommonBase.GetList<Sys_Privage>("").Where(emp => emp.ParentCode == code).OrderBy(c => c.MenuIndex).ToList();
        }
        protected override string btnAdd_Click()
        {
            string menuCode = Request.Form["menuCode"];
            if (!string.IsNullOrEmpty(menuCode))
            {
                List<CommonObject> listSave = new List<CommonObject>();
                string roleCode = Request.Form["hidCode"];
                //先删除
                string deleteSql = "delete from Sys_RolePower where MID='" + roleCode + "'";
                listSave.Add(new CommonObject(deleteSql, null));
                //查询代理商的级别
                Model.Member member = CommonBase.GetModel<Model.Member>(roleCode);
                string role = member.Role.Code;
                string[] array = menuCode.Split(',');
                List<string> privIdList = new List<string>();
                foreach (string str in array)
                {
                    Sys_RolePower model = new Sys_RolePower();
                    model.Id = GetGuid;
                    model.IsDeleted = false;
                    if (str.StartsWith("^"))
                    {
                        model.PrivageType = 2;
                        model.PrivageId = str.Substring(1);
                    }
                    else
                    {
                        model.PrivageType = 1;
                        model.PrivageId = str;
                    }
                    model.RoleCode = role;
                    model.MID = roleCode;
                    model.Status = 1;
                    privIdList.Add(model.PrivageId);
                    CommonBase.Insert<Sys_RolePower>(model, listSave);
                }
                //再遍历代理商名下的所有代理商，对没有的权限也把其删除掉
                string strWhere = "1=1 "; bool isHasChild = false;
                if (member.Role.Remark == "Agent")
                {
                    string areaId = member.AreaId.ToString();
                    string areaLeavl = member.Role.AreaLeave;
                    if (areaLeavl == "20")
                    {
                        isHasChild = true;
                        //省级代理商，需要查询到该省 下的所有市级和区县级代理商
                        strWhere += " and AreaId in (select ID from Sys_Address where Version=" + areaId + ")";
                    }
                    else if (areaLeavl == "30")
                    {
                        isHasChild = true;
                        strWhere += " and AreaId in (select ID from Sys_Address where ParentId=" + areaId + ")";
                    }
                    if (isHasChild)
                    {
                        //查询出所有的下级代理商
                        List<Model.Member> listNext = CommonBase.GetList<Model.Member>(strWhere);
                        foreach (Model.Member mem in listNext)
                        {
                            //查询出该代理商的权限
                            List<Sys_RolePower> rolePowerList = CommonBase.GetList<Sys_RolePower>("MID='" + mem.ID + "'");
                            //List<Sys_RolePower> rolePowerListForDelete = new List<Sys_RolePower>();
                            foreach (Sys_RolePower srp in rolePowerList)
                            {
                                if (privIdList.Contains(srp.PrivageId))
                                    continue;
                                else
                                {
                                    //删除不存在的
                                    CommonBase.Delete<Sys_RolePower>(srp, listSave);
                                }
                            }
                        }
                    }
                }
                if (CommonBase.RunListCommit(listSave))
                    return "1";
            }
            else
            {
                List<CommonObject> listSave = new List<CommonObject>();
                string roleCode = Request.Form["hidCode"];
                //先删除
                string deleteSql = "delete from Sys_RolePower where MID=" + roleCode;
                listSave.Add(new CommonObject(deleteSql, null));
                //查询代理商的级别
                Model.Member member = CommonBase.GetModel<Model.Member>(roleCode);
                //再遍历代理商名下的所有代理商，对没有的权限也把其删除掉
                string strWhere = "1=1 "; bool isHasChild = false;
                if (member.Role.Remark == "Agent")
                {
                    string areaId = member.AreaId.ToString();
                    string areaLeavl = member.Role.AreaLeave;
                    if (areaLeavl == "20")
                    {
                        isHasChild = true;
                        //省级代理商，需要查询到该省 下的所有市级和区县级代理商
                        strWhere += " and AreaId in (select ID from Sys_Address where Version=" + areaId + ")";
                    }
                    else if (areaLeavl == "30")
                    {
                        isHasChild = true;
                        strWhere += " and AreaId in (select ID from Sys_Address where ParentId=" + areaId + ")";
                    }
                    if (isHasChild)
                    {
                        //查询出所有的下级代理商
                        List<Model.Member> listNext = CommonBase.GetList<Model.Member>(strWhere);
                        foreach (Model.Member mem in listNext)
                        {
                            string delete = "delete from Sys_RolePower where MID='" + mem.ID + "'";
                            listSave.Add(new CommonObject(delete, null));
                            //查询出该代理商的权限
                            //List<Sys_RolePower> rolePowerList = CommonBase.GetList<Sys_RolePower>("MID='" + mem.Code + "'");
                            //foreach (Sys_RolePower srp in rolePowerList)
                            //{
                            //    //删除不存在的
                            //    CommonBase.Delete<Sys_RolePower>(srp, listSave);
                            //}
                        }
                    }
                }
                if (CommonBase.RunListCommit(listSave))
                    return "1";
            }
            return "0";
        }
        protected override string btnModify_Click()
        {
            string roleId = Request.Form["hidCode"];
            string result = string.Empty;
            List<Sys_RolePower> listRoleMenu = CommonBase.GetList<Sys_RolePower>("IsDeleted=0 and MID=" + roleId);
            foreach (Sys_RolePower model in listRoleMenu)
            {
                if (model.PrivageType == 2)
                    result += "^" + model.PrivageId + "*";
                else
                    result += model.PrivageId + "*";
            }
            return result;
        }
    }
}