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
    public class GetTrainPageList : BaseHandler
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
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = " UserCode='" + SessionModel.ID + "' and  EndTime <>'' and AnswerBeginTime <>'' and AnswerEndTime <>'' ";

            if (!string.IsNullOrEmpty(nRegistBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistBeginTime) + " 00:00:00", out time);
                strWhere += " and BeginTime>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(nRegistEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistEndTime) + " 23:59:59", out time);
                strWhere += " and BeginTime<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(type))
            {
                strWhere += " and CodeType='" + type + "'";
            }

            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "Code,CodeType,BeginTime", "BeginTime DESC", "T_TrainHeader");
            dt.Tables[0].Columns.Add("CutTime");
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                string remark = string.Empty;
                string codetype = dt.Tables[0].Rows[i]["CodeType"].ToString();
                switch (codetype)
                {
                    case "1": dt.Tables[0].Rows[i]["CodeType"] = "混合词训练"; break;
                    case "2": dt.Tables[0].Rows[i]["CodeType"] = "数字训练"; break;
                    case "3": dt.Tables[0].Rows[i]["CodeType"] = "扑克牌训练"; break;
                    case "4": dt.Tables[0].Rows[i]["CodeType"] = "字母训练"; break;
                }
                dt.Tables[0].Rows[i]["CutTime"] = Convert.ToDateTime(dt.Tables[0].Rows[i]["BeginTime"]).ToString("yyyy.MM.dd HH:mm:ss"); ;

            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}