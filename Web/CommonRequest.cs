using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public class CommonRequest: IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            Uri uri = context.Request.UrlReferrer;
            if(uri == null)
            {
                context.Response.Write("来源地址不合法");
                context.Response.End();
            }
            string host = uri.Host.ToString().ToLower();
            bool result = host.IndexOf("localhost") > -1;
            if(result)
                context.Response.WriteFile(context.Request.PhysicalPath);
            else
                context.Response.Write("来源地址不合法");
        }
    }
}