
using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Member
{
    public partial class MemberChange : BasePage
    {
        protected override void SetPowerZone()
        {
        }

        protected override string btnModify_Click()
        {
            if (!string.IsNullOrEmpty(Request["txt_MID"]))
            {
                Model.Member model = CommonBase.GetList<Model.Member>("MID='" + Request["txt_MID"] + "'").FirstOrDefault();
                if (model == null)
                {
                    return "不存在此会员";
                }
                else
                {
                    if (model.RoleCode == "VIP" || model.RoleCode == "Member")
                    {
                        return model.ID + "~" + model.MID + "~" + GetMID(model.MTJ) + "~" + model.MTJ + "~" + GetMID(model.Company) + "~" + model.Company + "~" + GetMID(model.Agent) + "~" + model.Agent + "~" + model.Role.Name;
                    }
                    else
                    {
                        //代理商
                        return model.ID + "~" + model.MID + "~" + GetMID(model.MTJ) + "~" + model.MTJ + "~" + GetMID(model.Company) + "~" + model.Company + "~" + GetMID(model.Agent) + "~" + model.Agent + "~" + model.Role.Name;
                    }
                }
            }
            return "操作成功";
        }

        protected string GetMID(object id)
        {
            string result = string.Empty;
            Model.Member mem = CommonBase.GetModel<Model.Member>(id);
            if (mem != null)
            {
                result = mem.MID;
            }
            else
            {
                result = "admin";
            }
            return result;
        }

        protected override string btnAdd_Click()
        {
            try
            {
                List<CommonObject> listComm = new List<CommonObject>();
                if (!string.IsNullOrEmpty(Request["hidId"]))
                {
                    Model.Member model = CommonBase.GetModel<Model.Member>(Request["hidId"]);
                    if (model != null)
                    {
                        //如果推荐人是会员
                        if (model.RoleCode == "Member" || model.RoleCode == "VIP")
                        {
                            string newMTJ = Request["txt_MTJ"];
                            //查询新的推荐人是否存在？
                            Model.Member MTJ = CommonBase.GetList<Model.Member>("MID='" + Request["txt_MTJ"] + "'").FirstOrDefault();
                            if (MTJ == null)
                            {
                                return "不存在该推荐人";
                            }
                            //主要处理逻辑
                            //1、把该会员的推荐人
                            //如果是会员，Member表字段Agent，就是归属于哪个代理商 ;Company字段就是归属于哪个O单商 
                            model.MTJ = MTJ.ID.ToString();
                            if (MTJ.Role.AreaLeave != null && int.Parse(MTJ.Role.AreaLeave) >= 10)
                            {
                                model.Agent = MTJ.ID;
                                if (int.Parse(MTJ.Role.AreaLeave) == 10)
                                    model.Company = MTJ.ID;
                                else
                                    model.Company = MTJ.Company;
                                model.UseRoleType = MTJ.UseRoleType;
                                model.AreaId = MTJ.AreaId;
                            }
                            else if (MTJ.Role.IsAdmin)
                            {
                                model.Agent = MTJ.ID;
                                model.Company = string.Empty;
                                model.UseRoleType = MTJ.UseRoleType;
                            }
                            else
                            {
                                model.Agent = MTJ.Agent;
                                model.Company = MTJ.Company;
                                model.UseRoleType = MTJ.UseRoleType;
                            }

                            CommonBase.Update<Model.Member>(model, new string[] { "MTJ", "Agent", "Company", "UseRoleType", "AreaId" }, listComm);
                            //修改该会员名下的所有会员
                            //存储过程查出来该会员的上级推荐关心信息
                            SqlParameter[] parameters = {
                    new SqlParameter("@MID", SqlDbType.VarChar, 50),
                    new SqlParameter("@RoleCode", SqlDbType.VarChar, 50)
                 };
                            parameters[0].Value = model.ID;
                            parameters[1].Value = "";
                            DataTable dtUpperMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountTDCount");
                            foreach (DataRow dr in dtUpperMemberTbl.Rows)
                            {
                                string mid = dr["ID"].ToString();
                                if (dr["RoleCode"].ToString() == "Member" || dr["RoleCode"].ToString() == "VIP")
                                {
                                    string sql = "UPDATE dbo.Member SET Agent=" + model.Agent + ",Company=" + model.Company + ",UseRoleType=" + model.UseRoleType + ",AreaId=" + model.AreaId + " WHERE ID=" + mid;
                                    listComm.Add(new CommonObject(sql, null));
                                }
                            }
                            //操作之前备份数据库
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                            CommonBase.BackUpDataBase("ZBkgj", MethodHelper.ConfigHelper.GetAppSettings("DataBaseBackUpURL"), Service.GlobleConfigService.GetWebConfig("SystemID").Value + dateStr);
                            LogService.Log(TModel, "20", TModel.MID + "在" + dateStr + "时间修改会员" + model.MID + "的推荐人为" + MTJ.MID + "，改之前备份了数据库", listComm);
                            if (CommonBase.RunListCommit(listComm))
                            {
                                return "操作成功";
                            }
                            else
                            {
                                return "操作失败，请重试。";
                            }
                        }
                        else
                        {
                            //修改代理商的推荐人
                            //查询新的推荐人是否存在？
                            Model.Member MTJ = CommonBase.GetList<Model.Member>("MID='" + Request["txt_MTJ"] + "'").FirstOrDefault();
                            if (MTJ == null)
                            {
                                return "不存在该推荐人";
                            }
                            //修改推荐人
                            model.MTJ = MTJ.ID.ToString();
                            ////归属地区
                            ////代理地区AreaId
                            ////如果是省市区县代理商，代理地区就是下拉列表的地区
                            //#region 如果是省市区县代理商
                            //if (model.UseRoleType == 1)
                            //{
                            //    Sys_Role role = CacheService.RoleList.Where(c => c.Code == model.RoleCode).FirstOrDefault();
                            //    string areaId = model.Province;
                            //    //查看代理地区与所选级别是否匹配
                            //    bool isMatch = true;
                            //    if (role.AreaLeave == "20")
                            //    {
                            //        if (!string.IsNullOrEmpty(model.City))
                            //            isMatch = false;
                            //        if (!string.IsNullOrEmpty(model.Zone))
                            //            isMatch = false;
                            //        if (string.IsNullOrEmpty(model.Province))
                            //            isMatch = false;
                            //    }
                            //    if (role.AreaLeave == "30")
                            //    {
                            //        if (string.IsNullOrEmpty(model.City))
                            //            isMatch = false;
                            //        if (!string.IsNullOrEmpty(model.Zone))
                            //            isMatch = false;
                            //        areaId = model.City;
                            //    }
                              
                            //    if (role.AreaLeave == "40")
                            //    {
                            //        if (string.IsNullOrEmpty(model.Zone))
                            //            isMatch = false;
                            //        areaId = model.Zone;
                            //    }
                            //    if (!isMatch)
                            //    {
                            //        return "添加失败，代理商级别与代理地区不匹配！";
                            //    }
                            //    else
                            //    {
                            //        if (model.RoleCode != "Admin")
                            //            model.AreaId = Convert.ToInt32(areaId);
                            //    }
                            //}
                            //#endregion
                            //else
                            //{
                            //    //以一二三级代理商模式走的
                            //    if (model.RoleCode != "Admin")
                            //        model.AreaId = Convert.ToInt32(CacheService.RoleList.Where(c => c.Code == model.RoleCode).FirstOrDefault().AreaLeave);
                            //    else
                            //        model.AreaId = 0;
                            //    //model.Company = TModel.ID;
                            //}


                            ////修改所属O单商
                            //if (MTJ.Company == 0)
                            //{
                            //    //查询到超级管理员
                            //    Model.Member superAdminModel = Service.MemberService.GetCompanyAdminMember();
                            //    model.Company = superAdminModel.ID;
                            //}
                            //else
                            //{
                            //    model.Company = MTJ.Company;
                            //}
                            ////修改归属代理商
                            //if (MTJ.UseRoleType == 1) //省市区县代理商模式
                            //{
                            //    model.Agent = MTJ.Agent;
                            //}
                            //else if (MTJ.UseRoleType == 2) //一二三级代理商模式
                            //{
                            //    Model.Member AdminModel = Service.MemberService.GetAdminMember();
                            //    int guishuAgentId = 0;
                            //    if (MTJ.RoleCode == "1F" || MTJ.RoleCode == "3F" || MTJ.RoleCode == "2F" || MTJ.RoleCode == "Admin")
                            //    {
                            //        guishuAgentId = MTJ.ID;//归属上级代理商
                            //        int areaLeavel = Convert.ToInt32(CacheService.RoleList.Where(c => c.Code == model.RoleCode).FirstOrDefault().AreaLeave);
                            //        if (areaLeavel < Convert.ToInt32(MTJ.Role.AreaLeave))
                            //        {
                            //            //1--等级的树形结构
                            //            SqlParameter[] parameters = {
                            //           new SqlParameter("@MID", SqlDbType.Int),
                            //           new SqlParameter("@RoleCode", SqlDbType.VarChar)
                            //             };
                            //            parameters[0].Value = MTJ.ID;
                            //            parameters[1].Value = "";
                            //            DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperAgent");
                            //            bool isMatch = false;
                            //            foreach (DataRow dr in dtUpperAddressTbl.Rows)
                            //            {
                            //                string roleCode = dr["RoleCode"].ToString();
                            //                string areaId = dr["AreaId"].ToString();
                            //                if (roleCode == "1F" || roleCode == "2F" || roleCode == "3F")
                            //                {
                            //                    if (model.AreaId > Convert.ToInt16(areaId))
                            //                    {
                            //                        guishuAgentId = Convert.ToInt32(dr["ID"].ToString());
                            //                        isMatch = true;
                            //                        break;
                            //                    }
                            //                }
                            //            }
                            //            if (!isMatch)
                            //            {
                            //                if (AdminModel == null)
                            //                    guishuAgentId = 0;
                            //                else
                            //                    guishuAgentId = AdminModel.ID;
                            //            }
                            //        }
                            //        if (areaLeavel == Convert.ToInt32(MTJ.Role.AreaLeave))
                            //        {
                            //            guishuAgentId = MTJ.Agent;//归属上级代理商
                            //        }
                            //    }
                            //    else //如果是普通会员推荐的代理商，要按推荐关系往上找，找到离自己最近的比自己级别高的那一个代理，归属在该高级别的代理名下
                            //    {
                            //        //1--等级的树形结构
                            //        SqlParameter[] parameters = {
                            //       new SqlParameter("@MID", SqlDbType.Int),
                            //       new SqlParameter("@RoleCode", SqlDbType.VarChar)
                            //};
                            //        parameters[0].Value = MTJ.ID;
                            //        parameters[1].Value = "";
                            //        DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperAgent");

                            //        foreach (DataRow dr in dtUpperAddressTbl.Rows)
                            //        {
                            //            string roleCode = dr["RoleCode"].ToString();
                            //            string areaId = dr["AreaId"].ToString();
                            //            if (roleCode == "1F" || roleCode == "2F" || roleCode == "3F")
                            //            {
                            //                if (model.AreaId > Convert.ToInt16(areaId))
                            //                {
                            //                    guishuAgentId = Convert.ToInt32(dr["ID"].ToString());
                            //                    break;
                            //                }
                            //            }
                            //        }
                            //        if (guishuAgentId == 0)
                            //            if (AdminModel == null)
                            //                guishuAgentId = 0;
                            //            else
                            //                guishuAgentId = AdminModel.ID;
                            //    }
                            //    model.Agent = guishuAgentId;
                            //}


                            return "操作失败，只能修改会员角色。";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return "操作失败：" + e.Message;
            }
            return "操作失败，请重试";
        }

    }
}