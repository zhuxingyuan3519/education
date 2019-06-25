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
    public class GetMateralsPageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string bank = context.Request["bank"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = "NType=2 and Remark='"+bank+"'";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "ID,NTitle", "NCreateTime desc", "Notice");
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

      }
}