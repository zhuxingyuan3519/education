using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web.Handler
{
    /// <summary>
    /// GetPageList 的摘要说明
    /// </summary>
    public class GetTDPageList: BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            string type = context.Request["type"];
            string nRegistBeginTime = context.Request["nRegistBeginTime"];
            string nRegistEndTime = context.Request["nRegistEndTime"];
            string ddl_type = context.Request["ddl_type"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = "1=1 ";
            if(type == "tj")
            {
                if(!string.IsNullOrEmpty(nRegistBeginTime))
                {
                    DateTime time = DateTime.Now;
                    DateTime.TryParse(HttpUtility.UrlDecode(nRegistBeginTime) + " 00:00:00", out time);
                    strWhere += " and t3.CreatedTime>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
                if(!string.IsNullOrEmpty(nRegistEndTime))
                {
                    DateTime time = DateTime.Now;
                    DateTime.TryParse(HttpUtility.UrlDecode(nRegistEndTime) + " 23:59:59", out time);
                    strWhere += " and t3.CreatedTime<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
                if(string.IsNullOrEmpty(ddl_type))
                {
                    //strWhere += " and (t1.RoleCode='Member' or t1.RoleCode='VIP')";
                }
                else
                {
                    if(ddl_type == "0")
                    {
                        strWhere += " and t1.RoleCode='Member'";
                    }
                    //else if(ddl_type == "1")
                    //{
                    //    strWhere += " and t1.RoleCode='VIP'";
                    //}
                }

                DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "t1.*,t2.MID MTJMID,t2.MName MTJName,t3.CreatedTime PayTime ", "t3.CreatedTime DESC", "dbo.FUN_CountTDMember('" + SessionModel.ID + "',0,9999) t1 LEFT JOIN dbo.Member t2 ON t1.MTJ=t2.ID LEFT JOIN dbo.CM_CompanyPointCost t3 ON t1.Code=t3.ToCompany");
                dt.Tables[0].Columns.Add("CutTime");
                dt.Tables[0].Columns.Add("PayRemark");
                for(int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    string remark = string.Empty;
                    string MCode = dt.Tables[0].Rows[i]["Code"].ToString();
                    if(dt.Tables[0].Rows[i]["RoleCode"].ToString() == "Member")
                    {
                        dt.Tables[0].Rows[i]["PayTime"] = dt.Tables[0].Rows[i]["MCreateTime"];
                        dt.Tables[0].Rows[i]["PayRemark"] ="注册日期";
                        dt.Tables[0].Rows[i]["CutTime"] = Convert.ToDateTime(dt.Tables[0].Rows[i]["PayTime"]).ToString("yyyy年MM月dd日"); ;
                    }
                    else
                    {
                        dt.Tables[0].Rows[i]["PayRemark"] ="付款日期";
                        if(dt.Tables[0].Rows[i]["PayTime"] != DBNull.Value)
                        {
                            if(string.IsNullOrEmpty(dt.Tables[0].Rows[i]["PayTime"].ToString()))
                            {
                                List<TD_PayLog> listSignUp = CommonBase.GetList<Model.TD_PayLog>("PayID='" + MCode + "' order by PayTime asc");
                                if(listSignUp.Count > 0)
                                {
                                    dt.Tables[0].Rows[i]["PayTime"] = listSignUp[0].PayTime.ToString("yyyy年MM月dd日");
                                }
                            }
                            dt.Tables[0].Rows[i]["CutTime"] = Convert.ToDateTime(dt.Tables[0].Rows[i]["PayTime"]).ToString("yyyy年MM月dd日"); ;
                        }
                    }
                    var role = CacheService.RoleList.Where(c => c.Code == dt.Tables[0].Rows[i]["RoleCode"].ToString()).FirstOrDefault();
                    if(role != null)
                        dt.Tables[0].Rows[i]["RoleCode"] = role.Name;
                }
                context.Response.Write(JsonHelper.DataSetToJson(dt));
            }
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}