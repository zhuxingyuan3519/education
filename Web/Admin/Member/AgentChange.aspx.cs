
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
    public partial class AgentChange : BasePage
    {
        protected override void SetPowerZone()
        {
        }

        protected override string btnModify_Click()
        {
            if (!string.IsNullOrEmpty(Request["txt_MID"]))
            {
                Model.Member model = CommonBase.GetList<Model.Member>("MID='" + Request["txt_MID"] + "' and RoleCode NOT IN ('Member','VIP')").FirstOrDefault();
                if (model == null)
                {
                    return "0";
                }
                else
                {

                    string returnJson = "{";
                    returnJson += "\"mid\":\"" + model.MID + "\",";
                    returnJson += "\"id\":\"" + model.ID + "\",";
                    returnJson += "\"mname\":\"" + model.MName + "\",";
                    returnJson += "\"rolename\":\"" + model.Role.Name + "\",";
                    returnJson += "\"currentMTJ\":\"" + GetMID(model.MTJ) + "\",";
                    returnJson += "\"currentAgent\":\"" + GetMID(model.Agent) + "\",";
                    returnJson += "\"currentCompany\":\"" + GetMID(model.Company) + "\",";
                    returnJson += "\"1\":\"0\"}";
                    return returnJson;
                }
            }
            return "-1";
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
        //修改代理商推荐人的逻辑
        //
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
                        if (model.RoleCode == "Member" || model.RoleCode == "VIP")
                        {
                            return "不存在该分销商人";
                        }

                        string newMTJ = Request["txt_MTJ"];
                        //查询新的推荐人是否存在？
                        Model.Member MTJ = CommonBase.GetList<Model.Member>("MID='" + Request["txt_MTJ"] + "'").FirstOrDefault();
                        if (MTJ == null)
                        {
                            return "不存在该推荐人";
                        }

                        //修改分销商推荐人
                        model.MTJ = MTJ.ID.ToString();
                        //查询到推荐人的往上查找，查找上级代理商
                        //1--等级的树形结构
                        SqlParameter[] parameters = {
                                   new SqlParameter("@MID", SqlDbType.Int),
                                   new SqlParameter("@RoleCode", SqlDbType.VarChar)
                                     };
                        parameters[0].Value = model.MTJ;
                        parameters[1].Value = "";
                        DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperMember");
                        string updateAgent = string.Empty;
                        string updateCompany = string.Empty;
                        string updateUserType = string.Empty;
                        foreach (DataRow dr in dtUpperAddressTbl.Rows)
                        {
                            string roleCode = dr["RoleCode"].ToString();
                            string areaId = dr["AreaId"].ToString();
                            string useRoleType = dr["UseRoleType"].ToString();
                            if (roleCode == "1F" || roleCode == "2F" || roleCode == "3F")
                            {
                                if (model.AreaId>Convert.ToInt16(areaId))
                                {
                                    updateAgent = dr["ID"].ToString();
                                    updateCompany = dr["Company"].ToString();
                                    updateUserType = useRoleType;
                                    break;
                                }
                            }
                        }
                        model.Agent = updateAgent;
                        model.Company =updateCompany;
                        model.UseRoleType = MethodHelper.ConvertHelper.ToInt32(updateUserType, 2); ;
                        CommonBase.Update<Model.Member>(model, new string[] { "MTJ", "Company", "Agent", "UseRoleType" }, listComm);
                        //更新名下会员的Company,UseRoleType
                        string updateSQL = "update Member set Company=" + model.Company + " where ID IN (SELECT Code FROM dbo.FUN_CountTDMember('" + model.MTJ + "',0,99999))";
                        listComm.Add(new CommonObject(updateSQL, null));

                        if (CommonBase.RunListCommit(listComm))
                            return "操作成功，重新查询该代理商信息以检查信息是否正确。";
                        return "操作失败";
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