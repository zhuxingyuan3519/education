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
    public class PrizeList: BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " 1=1 and t1.IsDeleted=0 and t2.ID is not null ";

            if(!string.IsNullOrEmpty(context.Request["ddlRoleCode"]))
            {
                strWhere += " and t1.PrizeType='" + context.Request["hidType"] + "'";
            }

            if(!string.IsNullOrEmpty(context.Request["ddlIsGet"]))
            {
                strWhere += " and t1.IsPrize='" + context.Request["ddlIsGet"] + "'";
            }

            if(!string.IsNullOrEmpty(context.Request["nTitle"]))
            {
                strWhere += " and t2.MID like '%" + HttpUtility.UrlDecode(context.Request["nTitle"]) + "%'";
            }
            if(!string.IsNullOrEmpty(context.Request["nName"]))
            {
                strWhere += " and t2.MName like '%" + HttpUtility.UrlDecode(context.Request["nName"]) + "%'";
            }
            //if(!string.IsNullOrEmpty(context.Request["ddlRoleCode"]))
            //{
            //    strWhere += " and t1.RoleCode='" + context.Request["ddlRoleCode"] + "' ";
            //}
            if(!string.IsNullOrEmpty(context.Request["nRegistBeginTime"]))
            {
                strWhere += " and DATEDIFF(dd,t1.PrizeTime,'" + context.Request["nRegistBeginTime"] + "')<=0";
            }
            if(!string.IsNullOrEmpty(context.Request["nRegistEndTime"]))
            {
                strWhere += " and DATEDIFF(dd,t1.PrizeTime,'" + context.Request["nRegistEndTime"] + "')>=0";
            }


            int count;
            DataTable dt = new DataTable();
            dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "t1.PrizeTime,t1.IsPrize,t1.PrizeType,t1.PrizeMoney,t2.MID,t2.MName,t2.RoleCode,'' TypeName", "t1.PrizeTime DESC", "TD_PrizeDetail t1 left join Member t2 ON t1.MCode=t2.ID", out count);
            dt.Columns.Add("IsGet", typeof(string));//是否中奖
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                //dt.Rows[i]["RoleCode"] = CacheService.RoleList.Where(c => c.Code == dt.Rows[i]["RoleCode"].ToString()).FirstOrDefault().Name;

                string bankType = dt.Rows[i]["PrizeType"].ToString();
                switch(bankType)
                {
                    case "1": dt.Rows[i]["TypeName"] = "游戏红包"; break;
                    case "2": dt.Rows[i]["TypeName"] = "代言红包"; break;
                    case "3": dt.Rows[i]["TypeName"] = "签到红包"; break;
                    case "4": dt.Rows[i]["TypeName"] = "密令答题红包"; break;
                }
                //if(dt.Rows[i]["PrizeType"] != DBNull.Value)
                //    dt.Rows[i]["PrizeType"] = dt.Rows[i]["PrizeType"].ToString() == "1" ? "游戏红包" : "代言红包";
                dt.Rows[i]["IsGet"] = MethodHelper.ConvertHelper.ToBoolean(dt.Rows[i]["IsPrize"], false) ? "中奖" : "未中";
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }
    }
}