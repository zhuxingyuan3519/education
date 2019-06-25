using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Web.Handler
{
    /// <summary>
    /// DeleteFile 的摘要说明
    /// </summary>
    public class DeleteUPFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (!string.IsNullOrEmpty(context.Request.QueryString["picName"]))
            {
                string delFile = "";// HttpContext.Current.Server.MapPath("/Attachment/") + context.Request.QueryString["picName"];
                //delFile = HttpContext.Current.Server.MapPath(context.Request.QueryString["picName"]);
                string[] fileInfo = context.Request.QueryString["picName"].Split('*');
                foreach (string str in fileInfo)
                {
                    delFile += str + "/";
                }
                delFile = delFile.Substring(0, delFile.LastIndexOf('/'));
               delFile= HttpContext.Current.Server.MapPath("/"+delFile);
                if (File.Exists(delFile))
                {
                    try
                    {
                        File.Delete(delFile);
                        context.Response.Write("1");//删除成功
                    }
                    catch
                    {
                        context.Response.Write("0");//删除失败
                    }
                }
                else
                {
                    context.Response.Write("1");//不存在
                }
            }
            else
                context.Response.Write("0");//参数错误
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