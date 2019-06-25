using LitJson;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.UI;

namespace Service.WXPay.business
{
    public class WeixinUser
    {
        /// <summary>
        /// 保存页面对象，因为要在类的方法中使用Page的Request对象
        /// </summary>
        private Page page { get; set; }
        private static string APPID { get { return "wx7358cf170bf3cbc5"; } }

        private static string APPSecret { get { return "6e0134dfba604bb22145de7bd206453f"; } }

        /// <summary>
        /// openid用于调用统一下单接口
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// access_token用于获取收货地址js函数入口参数
        /// </summary>
        public string access_token { get; set; }

        public WeixinUser(Page page)
        {
            this.page = page;
        }

        public OAuthUser WXUserInfo { get; set; }

        /**
        * 
        * 网页授权获取用户基本信息的全部过程
        * 详情请参看网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * 第一步：利用url跳转获取code
        * 第二步：利用code去获取openid和access_token
        * 
        */
        public void GetWXUserInfo()
        {
            if (!string.IsNullOrEmpty(page.Request.QueryString["code"]))
            {
                //获取code码，以获取openid和access_token
                string code = page.Request.QueryString["code"];
                //Log.Debug(this.GetType().ToString(), "Get code : " + code);
                //2、使用code换取access_token
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WeixinUser.APPID;
                url += "&secret=" + WeixinUser.APPSecret;
                url += "&code=" + code;
                url += "&grant_type=authorization_code";
                string result = WxPayAPI.HttpService.Get(url);
                //MethodHelper.LogHelper.WriteTextLog("Test", "使用code换取access_token返回结果：" + result, DateTime.Now);

                JsonData jd = JsonMapper.ToObject(result);
                //获取access_token
                this.access_token = (string)jd["access_token"];
                //获取用户openid
                this.openid = (string)jd["openid"];

                //3、使用access_token获取用户信息
                url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token;
                url += "&openid=" + openid;
                url += "&lang=zh_CN";
                string resultInfo = WxPayAPI.HttpService.Get(url);
                //MethodHelper.LogHelper.WriteTextLog("Test", "使用access_token获取用户信息：" + resultInfo, DateTime.Now);
                OAuthUser user = JsonConvert.DeserializeObject<OAuthUser>(resultInfo);
                WXUserInfo = user;
            }
            else
            {
                //构造网页授权获取code的URL
                string host = page.Request.Url.Host;
                string path = page.Request.Path;
                string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);

                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?";
                url += "appid=" + Service.WXPay.business.WeixinShare.APPID;
                url += "&redirect_uri=" + redirect_uri;
                url += "&response_type=code";
                url += "&scope=snsapi_userinfo";
                url += "&state=STATE#wechat_redirect";
                //Log.Debug(this.GetType().ToString(), "Will Redirect to URL : " + url);
                try
                {
                    //触发微信返回code码         
                    page.Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                }
            }
        }


    }
}
