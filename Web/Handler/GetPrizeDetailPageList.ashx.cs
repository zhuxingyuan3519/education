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
    public class GetPrizeDetailPageList: BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string type = context.Request["type"];
            string nRegistBeginTime = context.Request["nRegistBeginTime"];
            string nRegistEndTime = context.Request["nRegistEndTime"];
            string ddl_type = context.Request["ddl_type"];
            string strWhere = "t1.IsDeleted=0 and t1.IsPrize=1  and t1.MCode='" + SessionModel.ID + "' ";

            if(!string.IsNullOrEmpty(nRegistBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistBeginTime) + " 00:00:00", out time);
                strWhere += " and t1.PrizeTime>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if(!string.IsNullOrEmpty(nRegistEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistEndTime) + " 23:59:59", out time);
                strWhere += " and t1.PrizeTime<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if(!string.IsNullOrEmpty(ddl_type))
            {
                strWhere += " and t1.PrizeType='" + ddl_type + "'";
            }

            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "PrizeTime,PrizeType,PrizeMoney,'' TypeName", "t1.PrizeTime desc", "TD_PrizeDetail t1");
            dt.Tables[0].Columns.Add("CutTime", typeof(string));
            for(int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                string bankType = dt.Tables[0].Rows[i]["PrizeType"].ToString();
                switch(bankType)
                {
                    case "1": dt.Tables[0].Rows[i]["TypeName"] = "游戏红包"; break;
                    case "2": dt.Tables[0].Rows[i]["TypeName"] = "代言红包"; break;
                    case "3": dt.Tables[0].Rows[i]["TypeName"] = "签到红包"; break;
                    case "4": dt.Tables[0].Rows[i]["TypeName"] = "密令答题红包"; break;
                }
                dt.Tables[0].Rows[i]["CutTime"] = MethodHelper.ConvertHelper.ToDateTime(dt.Tables[0].Rows[i]["PrizeTime"], DateTime.Now).ToString("yyyy-MM-dd HH:mm");
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}