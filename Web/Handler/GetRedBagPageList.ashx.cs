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
    public class GetRedBagPageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string type = context.Request["type"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = "ToUserId='" + SessionModel.ID + "'";
            if (type == "2")
                strWhere = "UserId='" + SessionModel.ID + "'";
            DataSet dt = new DataSet();
            if (type == "1")
                dt=CommonBase.GetPageList(currentIndex, pageSize, strWhere, "FromUserId,FromUserCode,ToUserId,ToUserCode,SendTime,RedBagMoney,RedType,IsActive,Code,FromLevelCount,Status", "SendTime desc", "SH_RedBagDetailLog");
            else if (type == "2")
                dt=CommonBase.GetPageList(currentIndex, pageSize, strWhere, "UserId,UserCode,RedBagCount,RedBagMoney,LogDate", "LogDate desc", "SH_RedBagHeaderLog");
            dt.Tables[0].Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                if (type == "1")
                    dt.Tables[0].Rows[i]["CutTime"] = Convert.ToDateTime(dt.Tables[0].Rows[i]["SendTime"]).ToString("yyyy/MM/dd HH:mm");
                else if (type == "2")
                    dt.Tables[0].Rows[i]["CutTime"] = Convert.ToDateTime(dt.Tables[0].Rows[i]["LogDate"]).ToString("yyyy/MM/dd HH:mm");
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}