using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// CountActiveMember 的摘要说明
    /// </summary>
    public class CountActiveMember : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string days = context.Request["days"];
            string sql = "SELECT CONVERT(varchar(100), LogDate, 23) logDate ,COUNT(1) logCount FROM dbo.DB_Log WHERE LType='1' AND DATEDIFF(dd,LogDate,GETDATE())<=" + days + " GROUP BY CONVERT(varchar(100), LogDate, 23)";
            DataTable dt = CommonBase.GetTable(sql);
            context.Response.Write(MethodHelper.JsonHelper.DataTableToJson(dt, "charData"));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}