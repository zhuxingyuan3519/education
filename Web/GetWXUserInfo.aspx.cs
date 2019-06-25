using DBUtility;
using LitJson;
using Model;
using Newtonsoft.Json;
using System;

namespace Web
{
    public partial class GetWXUserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string redirect = Request.QueryString["redirect"];
                MethodHelper.LogHelper.WriteTextLog("Test", "获取到参数redirect:" + redirect, DateTime.Now);
                //1、获取到code
                string code = Request.QueryString["code"];
                string desParam = string.Empty;

                ////作为测试begin
                //redirect = "20D4C8B7C700A48A98004ED62BFAC32A3DE203EA5F5FE50A4CBF538F2DF5AA49CDADF817CF4AA3966F1EDC3C410EB94AACBD09DA5FBA8AA56A5C73AAB62A8892BAE804441514C10181EBD4F1A28B3860377AB15E62AA570141DAB10F9E8F48DB";
                //string test = MethodHelper.CommonHelper.DESEncrypt("http://jwyapi.0755wgwh.com/project_readme.html?mid=002f16a3e40f454b8f706d0973bb46dd&share=1");
                //desParam = "{\"nickname\":\"" + "兴源端口" + "\",\"openid\":\"" + "oef_-0r3kgk85qEtLEVo4RplRk8A" + "\",\"headimgurl\":\"" + "http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTLU8b47j5DHfUqpic3hkLYItJicpdI1WBibTSic9ooWW6HUKtCB7FyZAzX3lPWYFaPYHYiaibXBUgX1cpNg/132" + "\",\"sex\":\"" + "1" + "\"}";
                ////对这个参数进行DES加密
                //desParam = MethodHelper.CommonHelper.DESEncrypt(desParam);
                ////作为测试end

                if (!string.IsNullOrEmpty(code))
                {
                    //MethodHelper.LogHelper.WriteTextLog("Test", "获取到微信传递的code:" + code, DateTime.Now);
                    //2、使用code换取access_token
                    string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Service.WXPay.business.WeixinShare.APPID;
                    url += "&secret=" + Service.WXPay.business.WeixinShare.APPSecret;
                    url += "&code=" + code;
                    url += "&grant_type=authorization_code";
                    string result = WxPayAPI.HttpService.Get(url);
                    //MethodHelper.LogHelper.WriteTextLog("Test", "使用code换取access_token返回结果：" + result, DateTime.Now);

                    JsonData jd = JsonMapper.ToObject(result);
                    //获取access_token
                    string access_token = (string)jd["access_token"];
                    //获取用户openid
                    string openid = (string)jd["openid"];

                    MethodHelper.LogHelper.WriteTextLog("Test", "获取到access_token：" + access_token + "，openid：" + openid, DateTime.Now);
                    //3、使用access_token获取用户信息
                    url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token;
                    url += "&openid=" + openid;
                    url += "&lang=zh_CN";
                    string resultInfo = WxPayAPI.HttpService.Get(url);
                    Service.WXPay.business.OAuthUser user = JsonConvert.DeserializeObject<Service.WXPay.business.OAuthUser>(resultInfo);

                    string orginParam = "{\"status\":\"-1\",\"msg\":\"获取失败\"}";
                    if (user != null)
                    { 
                        //查看系统中是否存在此微信用户
                        M_WXUserInfo wxUser = CommonBase.GetModel<M_WXUserInfo>(user.openid);
                        if (wxUser == null)
                        {
                            //不存在就新插入一条数据
                            wxUser = new M_WXUserInfo();
                            wxUser.City = user.city;
                            wxUser.Country = user.country;
                            wxUser.CreatedTime = DateTime.Now;
                            wxUser.HeadImgUrl = user.headimgurl;
                            wxUser.IsDeleted = false;
                            wxUser.NickName = user.nickname;
                            wxUser.OpenId = user.openid;
                            wxUser.Province = user.province;
                            wxUser.Sex = user.sex.ToString();
                            wxUser.Sort = 1;
                            wxUser.Status = 1;
                            if (CommonBase.Insert<M_WXUserInfo>(wxUser))
                            {
                                orginParam = "{\"nickname\":\"" + user.nickname + "\",\"openid\":\"" + user.openid + "\",\"headimgurl\":\"" + user.headimgurl + "\",\"sex\":\"" + user.sex + "\"}";
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            orginParam = "{\"nickname\":\"" + user.nickname + "\",\"openid\":\"" + user.openid + "\",\"headimgurl\":\"" + user.headimgurl + "\",\"sex\":\"" + user.sex + "\"}";
                        }
                    }
                    else
                    {

                    }

                    //divTip.InnerHtml = "微信昵称：" + user.nickname + "，openid:" + user.openid + "。<br/>用户头像：<img src=\"" + user.headimgurl + "\"/>";
                    MethodHelper.LogHelper.WriteTextLog("Test", "使用access_token获取用户信息：" + resultInfo, DateTime.Now);
                    
                    //对这个参数进行DES加密
                    desParam = MethodHelper.CommonHelper.DESEncrypt(orginParam);
                }
                //对该参数进行DES解密
                string toUrl = MethodHelper.CommonHelper.DESDecrypt(redirect);
                if (toUrl.IndexOf("?") > 0)
                {
                    toUrl += "&desuser=" + desParam;
                }
                else
                {
                    toUrl += "?desuser=" + desParam;
                }

                MethodHelper.LogHelper.WriteTextLog("Test", "获取用户信息之后跳转的地址：" + toUrl, DateTime.Now);
                Response.Redirect(toUrl);
            }
        }
    }
}