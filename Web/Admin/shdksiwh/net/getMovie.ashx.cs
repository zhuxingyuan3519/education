using System;
using System.Web;
using System.Net;
using System.Text;

namespace Web.Ueditor
{
    public class getMovie : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string key = context.Server.HtmlEncode(context.Request.Form["searchKey"]);
            string type = context.Server.HtmlEncode(context.Request.Form["videoType"]);

            Uri httpURL = new Uri("http://api.tudou.com/v3/gw?method=item.search&appKey=myKey&format=json&kw=" + key + "&pageNo=1&pageSize=20&channelId=" + type + "&inDays=7&media=v&sort=s");
            WebClient MyWebClient = new WebClient();


            MyWebClient.Credentials = CredentialCache.DefaultCredentials;           //获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData(httpURL);

            context.Response.Write(Encoding.UTF8.GetString(pageData));
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