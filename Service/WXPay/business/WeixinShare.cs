using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace Service.WXPay.business
{
    public class WeixinShare
    {
        public static string APPID = "wx7358cf170bf3cbc5";
        public static string APPSecret = "6e0134dfba604bb22145de7bd206453f";
        public static WXShareModel GetKey(string str)
        {
            WXShareModel aModel = new WXShareModel();
            WXShareModel.WXToolsHelper tb = new WXShareModel.WXToolsHelper();
            string AppId = APPID;//记无忧公众号
            string secret = APPSecret;//记无忧公众号
            string access_token = tb.GetAccess_Token(AppId, secret);
            aModel.appId = AppId;
            aModel.nonceStr = tb.CreatenNonce_str();
            aModel.timestamp = tb.CreatenTimestamp();
            aModel.ticket = tb.GetTicket(access_token);
            aModel.url = str;
            aModel.MakeSign();
            return aModel;
        }
        public class WXShareModel
        {
            public string appId { get; set; }
            public string nonceStr { get; set; }
            public long timestamp { get; set; }
            public string ticket { get; set; }
            public string url { get; set; }
            public string signature { get; set; }

            public void MakeSign()
            {
                var string1Builder = new StringBuilder();
                string1Builder.Append("jsapi_ticket=").Append(ticket).Append("&")
                             .Append("noncestr=").Append(nonceStr).Append("&")
                             .Append("timestamp=").Append(timestamp).Append("&")
                             .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
                var string1 = string1Builder.ToString();

                MethodHelper.LogHelper.WriteTextLog("MakeSign", string1, DateTime.Now);
                signature = Sha1(string1, Encoding.Default);
            }
            public static string Sha1(string orgStr, Encoding encode)
            {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(orgStr);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result.ToLower();
            }
            public class WXToolsHelper
            {
                /// <summary>
                /// 获取全局的access_token,程序缓存
                /// </summary>
                /// <param name="AppId">第三方用户唯一凭证</param>
                /// <param name="AppSecret">第三方用户唯一凭证密钥，即appsecret</param>
                /// <returns>得到的全局access_token</returns>
                public string GetAccess_Token(string AppId, string AppSecret)
                {
                    try
                    {
                        //先查缓存数据
                        if(HttpContext.Current.Cache["access_token"] != null)
                        {
                            return HttpContext.Current.Cache["access_token"].ToString();
                        }
                        else
                        {
                            return GetToken(AppId, AppSecret);
                        }
                    }
                    catch
                    {
                        return GetToken(AppId, AppSecret);
                    }
                }

                /// <summary>
                /// 获取全局的access_token
                /// </summary>
                /// <param name="AppId">第三方用户唯一凭证</param>
                /// <param name="AppSecret">第三方用户唯一凭证密钥，即appsecret</param>
                /// <returns>得到的全局access_token</returns>
                public string GetToken(string AppId, string AppSecret)
                {
                    var client = new System.Net.WebClient();
                    client.Encoding = System.Text.Encoding.UTF8;
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppId, AppSecret);
                    MethodHelper.LogHelper.WriteTextLog("GetToken", url, DateTime.Now);
                    var data = client.DownloadString(url);
                    var jss = new JavaScriptSerializer();
                    var access_tokenMsg = jss.Deserialize<Dictionary<string, object>>(data);

                    MethodHelper.LogHelper.WriteTextLog("GetToken:access_token", access_tokenMsg["access_token"].ToString(), DateTime.Now);

                    //放入缓存中
                    HttpContext.Current.Cache.Insert("access_token", access_tokenMsg["access_token"], null, DateTime.Now.AddSeconds(7100), TimeSpan.Zero, CacheItemPriority.Normal, null);

                    //清除jsapi_ticket缓存
                    HttpContext.Current.Cache.Remove("ticket");

                    //获取jsapi_ticket,为了同步
                    GetTicket(access_tokenMsg["access_token"].ToString());

                    return access_tokenMsg["access_token"].ToString();
                }


                /// <summary>
                /// 获取jsapi_ticket,程序缓存
                /// </summary>
                /// <param name="access_token">全局的access_token</param>
                /// <returns>得到的jsapi_ticket</returns>
                public string GetJsapi_Ticket(string access_token)
                {
                    try
                    {
                        //先查缓存数据
                        if(HttpContext.Current.Cache["ticket"] != null)
                        {
                            return HttpContext.Current.Cache["ticket"].ToString();
                        }
                        else
                        {
                            return GetTicket(access_token);
                        }
                    }
                    catch
                    {
                        return GetTicket(access_token);
                    }
                }


                /// <summary>
                /// 获取jsapi_ticket
                /// </summary>
                /// <param name="access_token">全局的access_token</param>
                /// <returns>得到的jsapi_ticket</returns>
                public string GetTicket(string access_token)
                {
                    var client = new System.Net.WebClient();
                    client.Encoding = System.Text.Encoding.UTF8;
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", access_token);
                    MethodHelper.LogHelper.WriteTextLog("GetTicket", url, DateTime.Now);
                    var data = client.DownloadString(url);
                    var jss = new JavaScriptSerializer();
                    var ticketMsg = jss.Deserialize<Dictionary<string, object>>(data);

                    MethodHelper.LogHelper.WriteTextLog("GetTicket:ticket", ticketMsg["ticket"].ToString(), DateTime.Now);


                    try
                    {
                        //放入缓存中
                        HttpContext.Current.Cache.Insert("ticket", ticketMsg["ticket"], null, DateTime.Now.AddSeconds(7100), TimeSpan.Zero, CacheItemPriority.Normal, null);
                        return ticketMsg["ticket"].ToString();
                    }
                    catch(Exception ex)
                    {
                        return ex.Message;
                    }
                }

                /// <summary>
                /// 微信权限签名的 sha1 算法
                /// 签名用的noncestr和timestamp必须与wx.config中的nonceStr和timestamp相同
                /// </summary>
                /// <param name="jsapi_ticket">获取到的jsapi_ticket</param>
                /// <param name="noncestr">生成签名的随机串</param>
                /// <param name="timestamp">生成签名的时间戳</param>
                /// <param name="url">签名用的url必须是调用JS接口页面的完整URL</param>
                /// <returns></returns>
                public string GetShal(string jsapi_ticket, string noncestr, long timestamp, string url)
                {
                    string strSha1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, noncestr, timestamp, url);
                    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strSha1, "sha1").ToLower();
                }

                /// <summary>
                /// 微信权限签名( sha1 算法 )
                /// 签名用的noncestr和timestamp必须与wx.config中的nonceStr和timestamp相同
                /// </summary>
                /// <param name="AppId">第三方用户唯一凭证</param>
                /// /// <param name="AppSecret">第三方用户唯一凭证密钥，即appsecret</param>
                /// <param name="noncestr">生成签名的随机串</param>
                /// <param name="timestamp">生成签名的时间戳</param>
                /// <param name="url">签名用的url必须是调用JS接口页面的完整URL</param>
                /// <returns></returns>
                public string Get_Signature(string AppId, string AppSecret, string noncestr, long timestamp, string url)
                {
                    string access_token = GetAccess_Token(AppId, AppSecret); //获取全局的access_token
                    string jsapi_ticket = GetJsapi_Ticket(access_token); //获取jsapi_ticket

                    string strSha1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, noncestr, timestamp, url);
                    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strSha1, "sha1").ToLower();
                }


                /// <summary>
                /// 微信权限签名( sha1 算法 )
                /// 签名用的noncestr和timestamp必须与wx.config中的nonceStr和timestamp相同
                /// </summary>
                /// <param name="AppId">第三方用户唯一凭证</param>
                /// /// <param name="AppSecret">第三方用户唯一凭证密钥，即appsecret</param>
                /// <param name="noncestr">生成签名的随机串</param>
                /// <param name="timestamp">生成签名的时间戳</param>
                /// <param name="url">签名用的url必须是调用JS接口页面的完整URL</param>
                /// <returns></returns>
                public void signatureOut(string AppId, string AppSecret, string noncestr, long timestamp, string url, out string access_token, out string jsapi_ticket, out string signature)
                {
                    access_token = GetAccess_Token(AppId, AppSecret); //获取全局的access_token

                    jsapi_ticket = GetJsapi_Ticket(access_token); //获取jsapi_ticket

                    string strSha1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, noncestr, timestamp, url);

                    signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strSha1, "sha1").ToLower();
                }

                private string[] strs = new string[]
        {
"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
        };
                /// <summary>
                /// 创建随机字符串 
                /// </summary>
                /// <returns></returns>
                public string CreatenNonce_str()
                {
                    Random r = new Random();
                    var sb = new StringBuilder();
                    var length = strs.Length;
                    for(int i = 0; i < 15; i++)
                    {
                        sb.Append(strs[r.Next(length - 1)]);
                    }

                    MethodHelper.LogHelper.WriteTextLog("CreatenNonce_str:noncestr", sb.ToString(), DateTime.Now);
                    return sb.ToString();
                }


                /// <summary>
                /// 创建时间戳 
                /// </summary>
                /// <returns></returns>
                public long CreatenTimestamp()
                {
                    long lo = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                    MethodHelper.LogHelper.WriteTextLog("CreatenTimestamp:Timestamp", lo.ToString(), DateTime.Now);
                    return lo;
                }


            }
        }


        
    }
}
