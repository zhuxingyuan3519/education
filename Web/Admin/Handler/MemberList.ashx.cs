using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Admin;
using Model;

using System.Text;
using MethodHelper;
using Newtonsoft.Json;
using DBUtility;
using System.Data;
using Service;

namespace Web.Admin.Handler
{
    /// <summary>
    /// MemberList 的摘要说明
    /// </summary>
    public class MemberList: BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " 1=1 and t1.IsClose=0 ";

            if(!string.IsNullOrEmpty(context.Request["hidType"]))
            {
                strWhere += " and t1.RoleCode='" + context.Request["hidType"] + "'";
            }
            else
            {
                strWhere += " and  (t1.RoleCode ='Member' OR t1.RoleCode ='Teacher' OR t1.RoleCode ='VIP' OR t1.RoleCode='Student' OR t1.RoleCode='WordMember')";
            }

            if(!string.IsNullOrEmpty(context.Request["nTitle"]))
            {
                strWhere += " and t1.MID like '%" + HttpUtility.UrlDecode(context.Request["nTitle"]) + "%'";
            }
            if(!string.IsNullOrEmpty(context.Request["nName"]))
            {
                strWhere += " and t1.MName like '%" + HttpUtility.UrlDecode(context.Request["nName"]) + "%'";
            }
            if(!string.IsNullOrEmpty(context.Request["ddlRoleCode"]))
            {

                strWhere += " and t1.RoleCode='" + context.Request["ddlRoleCode"] + "' ";

            }
            if(!string.IsNullOrEmpty(context.Request["nRegistBeginTime"]))
            {
                strWhere += " and DATEDIFF(dd,t1.MCreateDate,'" + context.Request["nRegistBeginTime"] + "')<=0";
            }
            if(!string.IsNullOrEmpty(context.Request["nRegistEndTime"]))
            {
                strWhere += " and DATEDIFF(dd,t1.MCreateDate,'" + context.Request["nRegistEndTime"] + "')>=0";
            }
            if(!string.IsNullOrEmpty(context.Request["nMTJTitle"]))
            {
                strWhere += " and t2.MID like '%" + HttpUtility.UrlDecode(context.Request["nMTJTitle"]) + "%'";
            }
            if(!string.IsNullOrEmpty(context.Request["nMTJName"]))
            {
                strWhere += " and t2.MName like '%" + HttpUtility.UrlDecode(context.Request["nMTJName"]) + "%'";
            }
            bool isQueryService = false;
            string RootNodeMCode = string.Empty;
            if(!string.IsNullOrEmpty(context.Request["nServiceMID"])) //查询某个服务中心名下的所有会员
            {
                string serviceMID = "";
                string sqlq = "SELECT ID FROM dbo.Member WHERE MID='" + context.Request["nServiceMID"] + "' AND Learns='service'";
                object obj = CommonBase.GetSingle(sqlq);
                if(obj != null)
                {
                    serviceMID = obj.ToString();
                    strWhere += " and t0.ID<>'" + serviceMID + "'";
                }
                string queryRootSql = @"DECLARE @RootNodeMCode VARCHAR(50);
            SELECT @RootNodeMCode= ID FROM( SELECT TOP 1 ID FROM dbo.[FUN_CountUpperMemberWithRank]('" + serviceMID + "',1,9999) ORDER BY RankTime ASC, LEAVEL ASC) T1;SELECT @RootNodeMCode";
                object objRootNodeCode = CommonBase.GetSingle(queryRootSql);
                if(objRootNodeCode != null)
                    RootNodeMCode = objRootNodeCode.ToString();
                isQueryService = true;
            }

            int count;
            DataTable dt = new DataTable();
            if(isQueryService)
                dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "t1.*", "t1.MCreateDate DESC", " dbo.FUN_CountTDMemberWithRank('" + RootNodeMCode + "',0,9999) t0 left join Member t1 ON t0.ID=t1.ID  LEFT JOIN dbo.Member t2 ON t1.MTJ=t2.ID", out count);
            else
                dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "t1.*", "t1.MCreateDate DESC", "Member t1 LEFT JOIN dbo.Member t2 ON t1.MTJ=t2.ID", out count);
            dt.Columns.Add("CutTime", typeof(string));
            dt.Columns.Add("MTJName", typeof(string));
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["RoleCode"] = CacheService.RoleList.Where(c => c.Code == dt.Rows[i]["RoleCode"].ToString()).FirstOrDefault().Name;
                dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["MCreateDate"]).ToString("yyyy/MM/dd");
                Model.Member tjModel = CommonBase.GetModel<Model.Member>(dt.Rows[i]["MTJ"].ToString());
                if(tjModel != null)
                {
                    dt.Rows[i]["MTJ"] = tjModel.MID;
                    dt.Rows[i]["MTJName"] = tjModel.MName;
                }
                if(!IsHasPower("CKHYMM"))
                    dt.Rows[i]["Password"] = "";
                else
                {
                    //解密密码

                    if(dt.Rows[i]["Password"].ToString() == CommonHelper.DESDecrypt(dt.Rows[i]["Password"].ToString()))
                    {
                        dt.Rows[i]["Password"] = "不可逆MD5";
                    }
                    else
                    {
                        dt.Rows[i]["Password"] = CommonHelper.DESDecrypt(dt.Rows[i]["Password"].ToString());
                    }
                }
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }
    }
}