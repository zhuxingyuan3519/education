using DBUtility;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace Web.Handler
{
    /// <summary>
    /// QRCode 的摘要说明
    /// </summary>
    public class QRCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string code = context.Request.QueryString["mid"];
            string data = GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/m/app/user_regist?code=" + code;
            CreateQRCode(data, "", context);
        }



        protected void CreateQRCode(string data, string fileName, HttpContext context)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            System.Drawing.Image image = qrCodeEncoder.Encode(data);
            if (!string.IsNullOrEmpty(Service.CacheService.GlobleConfig.Field5))
                image = CombinImage(image, context.Server.MapPath(Service.CacheService.GlobleConfig.Field5));//.Save(Server.MapPath("~/Attachment/QRCode/" + fileName + ".png"));
            else
                image = CombinImage(image, context.Server.MapPath("~/images/qcodecombine.png"));//.Save(Server.MapPath("~/Attachment/QRCode/" + fileName + ".png"));
            context.Response.Clear();
            context.Response.ContentType = "Image/JPEG";//通知浏览器发送的数据是JPEG格式的图像
            image.Save(context.Response.OutputStream, ImageFormat.Jpeg);//向浏览器发送图像数据
            context.Response.End();
        }

        /// <summary>    
        /// 调用此函数后使此两种图片合并，类似相册，有个    
        /// 背景图，中间贴自己的目标图片    
        /// </summary>    
        /// <param name="imgBack">粘贴的源图片</param>    
        /// <param name="destImg">粘贴的目标图片</param>    
        public static System.Drawing.Image CombinImage(System.Drawing.Image imgBack, string destImg)
        {
            if (MethodHelper.FileHelper.IsExistFile(destImg))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(destImg);        //照片图片      
                //if (img.Height != 40 || img.Width != 40)
                //{
                //    img = KiResizeImage(img, 40, 40, 0);
                //}
                Graphics g = Graphics.FromImage(imgBack);

                g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

                //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    
                //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    
                g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
                GC.Collect();
            }
            return imgBack;
        }
        /// <summary>    
        /// Resize图片    
        /// </summary>    
        /// <param name="bmp">原始Bitmap</param>    
        /// <param name="newW">新的宽度</param>    
        /// <param name="newH">新的高度</param>    
        /// <param name="Mode">保留着，暂时未用</param>    
        /// <returns>处理以后的图片</returns>    
        public static System.Drawing.Image KiResizeImage(System.Drawing.Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                System.Drawing.Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
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