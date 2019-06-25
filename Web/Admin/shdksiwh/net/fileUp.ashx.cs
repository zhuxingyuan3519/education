using System;
using System.Web;
using System.IO;
using System.Collections;

namespace Web.Ueditor
{
    public class fileUp : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //上传配置
            String pathbase = "upload/";                                      //保存路径
            string[] filetype = { ".rar", ".doc", ".docx", ".zip", ".pdf", ".txt", ".swf", ".wmv" };    //文件允许格式
            int size = 100;   //文件大小限制,单位MB,同时在web.config里配置环境默认为100MB


            //上传文件
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();
            info = up.upFile(context, pathbase, filetype, size); //获取上传状态

            context.Response.Write("{'state':'" + info["state"] + "','url':'" + info["url"] + "','fileType':'" + info["currentType"] + "','original':'" + info["originalName"] + "'}"); //向浏览器返回数据json数据
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