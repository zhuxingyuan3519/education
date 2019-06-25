using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.SystemManage
{
    public partial class Authority : BasePage
    {
        protected List<Sys_Privage> listAdmin, listOperator;
        protected string mtype;
        protected override void SetPowerZone()
        {
            ddl_Roles.DataSource = CommonBase.GetList<Sys_Role>("1=1 AND IsDeleted=0 ORDER BY  RIndex ASC");
            ddl_Roles.DataTextField = "Name";
            ddl_Roles.DataValueField = "Code";
            ddl_Roles.DataBind();
            ddl_Roles.Items.Insert(0, new ListItem("--请选择角色--", ""));
            listAdmin = CommonBase.GetList<Sys_Privage>("  PrivageType=1 and ParentCode='0' Order by MenuIndex");
            listOperator = CommonBase.GetList<Sys_Privage>("PrivageType=2");
        }

        protected List<Sys_Privage> GetSecondLeavelDict(string code)
        {
            return CommonBase.GetList<Sys_Privage>("").Where(emp => emp.ParentCode == code).OrderBy(c => c.MenuIndex).ToList();
        }
        protected override string btnAdd_Click()
        {
            string menuCode = Request.Form["menuCode"];
            if (!string.IsNullOrEmpty(menuCode))
            {
                List<CommonObject> listSave = new List<CommonObject>();
                string roleCode = Request.Form["ddl_Roles"];
                //先删除
                string deleteSql = "delete from Sys_RolePower where (MID=0 or MID IS NULL OR MID='')  AND RoleCode='" + roleCode + "'";
                listSave.Add(new CommonObject(deleteSql, null));
                string[] array = menuCode.Split(',');
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
                    model.RoleCode = roleCode;
                    model.Status = 1;
                    CommonBase.Insert<Sys_RolePower>(model, listSave);
                }
                if (CommonBase.RunListCommit(listSave))
                {
                    //刷新缓存
                    CacheHelper.RemoveAllCache("Sys_RolePower");
                    return "1";
                }
            }
            return "0";
        }
        protected override string btnModify_Click()
        {
            string roleId = Request.Form["roleId"];
            string result = string.Empty;
            List<Sys_RolePower> listRoleMenu = CommonBase.GetList<Sys_RolePower>("IsDeleted=0 and (MID=0 or MID IS NULL OR MID='')  and RoleCode='" + roleId + "'");
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