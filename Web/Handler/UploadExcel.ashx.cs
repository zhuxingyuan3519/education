using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Data.OleDb;
using System.Data;


namespace Web.Handler
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    public class UploadExcel: IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            //*******************************************
            //文件上传并保存
            HttpPostedFile file = context.Request.Files["Filedata"];
            string name = string.Empty;
            string uploadPath = string.Empty;
            if(file != null)
            {
                string cate = context.Request["cate"];
                string uptype = context.Request.Form["uptype"];
                uploadPath = HttpContext.Current.Server.MapPath("../Attachment/");
                if(uptype == "video")
                {
                    //如果是视频
                    uploadPath = HttpContext.Current.Server.MapPath("../Attachment/Video/");
                }
                else if(uptype == "train")
                {
                    //如果是视频
                    uploadPath = HttpContext.Current.Server.MapPath("../Attachment/Train/");
                }
                //string newfilename = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                name = file.FileName;
                //如果上传的是图片，就是需要保存到logo文件夹中的
                string type = name.Substring(name.LastIndexOf('.')).ToLower();
                string filename = name.Substring(0, name.LastIndexOf('.'));
                filename = GetGuid; //DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString();

                //string typeXls = context.Request.QueryString["t"];
                //if (!string.IsNullOrEmpty(typeXls) && typeXls == "xls")//上传的Excel文件
                //{
                //    uploadPath = HttpContext.Current.Server.MapPath("../Attachment/ImportExcel/");
                //}
                //else
                //{
                //if (type == ".jpg" || type == ".jpeg" || type == ".gif" || type == ".png" || type == ".swf" || type == ".ico")
                if(cate == "logo")
                {
                    uploadPath += "logo/";
                }
                //uploadPath = Bll.Configuration.Model.CommonURL;
                //else
                //    uploadPath = HttpContext.Current.Server.MapPath("../Attachment/download/");
                //}


                if(!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string sourceFile = uploadPath + filename + type;
                file.SaveAs(sourceFile);
                if(uptype == "train")
                {
                    //压缩图片
                    string desinceFile = HttpContext.Current.Server.MapPath("../Attachment/Train_Thumb/") + filename + type;
                    MethodHelper.ImageHelper.GetPicThumbnail(sourceFile, desinceFile, 280, 260, 55);
                }
                context.Response.Write(filename + type);
            }
            else
            {
                context.Response.Write("0");
            }
            //***************************************
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        protected string GetGuid
        {
            get { return Guid.NewGuid().ToString().Replace("-", "").Replace(" ", ""); }
        }
    }
}
