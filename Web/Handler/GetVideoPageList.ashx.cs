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
    public class GetVideoPageList: BaseHandler
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

            if(type == "0")
            {
                strWhere += " and Authority LIKE '%Member%'";
            }
            else if(type == "2")
            {
                strWhere += " and Authority  LIKE '%VIP%'";
            }
            else if(type == "1")
            {
                strWhere += " and Authority  LIKE '%Student%'";
            }
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "Code,Name,[Format],Title,Remark,CoverImage,Sort,Size,CreatedTime", "CreatedTime ASC", "EN_Video");
            for(int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                dt.Tables[0].Rows[i]["Remark"] = MethodHelper.CommonHelper.GetCutContent(dt.Tables[0].Rows[i]["Remark"], 18);
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}