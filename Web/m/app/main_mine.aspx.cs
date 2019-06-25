using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class main_mine : BasePage
    {
        protected Model.Member loginUserModel = null;
        protected decimal applyRoleMoney = 0;
        protected bool isShowSJ = false;
        protected string isSignUp = "0";
        protected string isShowBindWX = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 获取微信用户信息
                if (Session["WXMember"] == null)
                {
                    #region 获取到微信用户信息
                    Service.WXPay.business.WeixinUser wxModel = new Service.WXPay.business.WeixinUser(this);
                    wxModel.GetWXUserInfo();
                    Service.WXPay.business.OAuthUser user = wxModel.WXUserInfo;
                    //对获取到的微信用户信息进行操作
                    bool isGetWXinfo = true;
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
                                Session["WXMember"] = wxUser;
                                isShowBindWX = "1";
                            }
                            else
                            {
                                isGetWXinfo = false;
                            }
                        }
                        else
                        {
                            //如果系统中已经存在该微信用户
                            //再查看该微信用户是否已经绑定了手机号
                            if (Session["Member"] == null)
                            {

                                List<M_WX_VS_User> userList = CommonBase.GetList<M_WX_VS_User>("OpenId='" + wxUser.OpenId + "'");
                                if (userList.Count == 0)
                                {
                                    isShowBindWX = "1";
                                }
                                else
                                {
                                    ////获取到一个绑定的用户
                                    //M_WX_VS_User wxBindUser = userList.OrderByDescending(c => c.CreatedTime).FirstOrDefault();
                                    ////获取到用户
                                    //Model.Member member = CommonBase.GetModel<Model.Member>(wxBindUser.UserCode);
                                    //Session["Member"] = member;
                                }
                            }

                            Session["WXMember"] = wxUser;
                        }
                    }
                    else
                    {
                        isGetWXinfo = false;
                    }
                    if (!isGetWXinfo)//如果获取失败
                    {
                        ScriptAlert("获取微信用户信息失败");
                    }
                    #endregion

                    #region 以下代码已经封装，不再这里重复写了

                    //if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                    //{
                    //    //获取code码，以获取openid和access_token
                    //    string code = Request.QueryString["code"];
                    //    //Log.Debug(this.GetType().ToString(), "Get code : " + code);
                    //    //2、使用code换取access_token
                    //    string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Service.WXPay.business.WeixinShare.APPID;
                    //    url += "&secret=" + Service.WXPay.business.WeixinShare.APPSecret;
                    //    url += "&code=" + code;
                    //    url += "&grant_type=authorization_code";
                    //    string result = WxPayAPI.HttpService.Get(url);
                    //    //MethodHelper.LogHelper.WriteTextLog("Test", "使用code换取access_token返回结果：" + result, DateTime.Now);

                    //    JsonData jd = JsonMapper.ToObject(result);
                    //    //获取access_token
                    //    string access_token = (string)jd["access_token"];
                    //    //获取用户openid
                    //    string openid = (string)jd["openid"];
                    //    //3、使用access_token获取用户信息
                    //    url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token;
                    //    url += "&openid=" + openid;
                    //    url += "&lang=zh_CN";
                    //    string resultInfo = WxPayAPI.HttpService.Get(url);
                    //    Service.WXPay.business.WeixinShare.OAuthUser user = JsonConvert.DeserializeObject<Service.WXPay.business.WeixinShare.OAuthUser>(resultInfo);
                    //    bool isGetWXinfo = true;
                    //    if (user != null)
                    //    {
                    //        //查看系统中是否存在此微信用户
                    //        M_WXUserInfo wxUser = CommonBase.GetModel<M_WXUserInfo>(user.openid);
                    //        if (wxUser == null)
                    //        {
                    //            //不存在就新插入一条数据
                    //            wxUser = new M_WXUserInfo();
                    //            wxUser.City = user.city;
                    //            wxUser.Country = user.country;
                    //            wxUser.CreatedTime = DateTime.Now;
                    //            wxUser.HeadImgUrl = user.headimgurl;
                    //            wxUser.IsDeleted = false;
                    //            wxUser.NickName = user.nickname;
                    //            wxUser.OpenId = user.openid;
                    //            wxUser.Province = user.province;
                    //            wxUser.Sex = user.sex.ToString();
                    //            wxUser.Sort = 1;
                    //            wxUser.Status = 1;
                    //            if (CommonBase.Insert<M_WXUserInfo>(wxUser))
                    //            {
                    //                Session["WXMember"] = wxUser;
                    //                isShowBindWX = "1";
                    //            }
                    //            else
                    //            {
                    //                isGetWXinfo = false;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            //如果系统中已经存在该微信用户
                    //            //再查看该微信用户是否已经绑定了手机号
                    //            int hasBindCount= CommonBase.GetList<M_WX_VS_User>("OpenId='" + wxUser.OpenId + "'").Count;
                    //            if (hasBindCount == 0)
                    //            {
                    //                isShowBindWX = "1";
                    //            }
                    //            Session["WXMember"] = wxUser;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        isGetWXinfo = false;
                    //    }
                    //    if (!isGetWXinfo)//如果获取失败
                    //    {
                    //        ScriptAlert("获取微信用户信息失败");
                    //    }
                    //}
                    //else
                    //{
                    //    //构造网页授权获取code的URL
                    //    string host = Request.Url.Host;
                    //    string path = Request.Path;
                    //    string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);

                    //    string url = "https://open.weixin.qq.com/connect/oauth2/authorize?";
                    //    url += "appid=" + Service.WXPay.business.WeixinShare.APPID;
                    //    url += "&redirect_uri=" + redirect_uri;
                    //    url += "&response_type=code";
                    //    url += "&scope=snsapi_base";
                    //    url += "&state=STATE#wechat_redirect";

                    //    try
                    //    {
                    //        //触发微信返回code码         
                    //        Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
                    //    }
                    //    catch (System.Threading.ThreadAbortException ex)
                    //    {
                    //    }
                    //}
                    #endregion

                }
                else
                {
                    //如果系统中已经存在该微信用户
                    //再查看该微信用户是否已经绑定了手机号
                    M_WXUserInfo wx_user = Session["WXMember"] as M_WXUserInfo;
                    if (Session["Member"] == null)
                    {
                        List<M_WX_VS_User> userList = CommonBase.GetList<M_WX_VS_User>("OpenId='" + wx_user.OpenId + "'");
                        if (userList.Count == 0)
                        {
                            isShowBindWX = "1";
                        }
                        else
                        {
                            ////获取到一个绑定的用户
                            //M_WX_VS_User wxBindUser = userList.OrderByDescending(c => c.CreatedTime).FirstOrDefault();
                            ////获取到用户
                            //Model.Member member = CommonBase.GetModel<Model.Member>(wxBindUser.UserCode);
                            //Session["Member"] = member;
                        }
                    }
                }

                #endregion



                //JsApiPay jsApiPay = new JsApiPay(this);
                //jsApiPay.GetOpenidAndAccessToken();

                //Log.Debug(this.GetType().ToString(), "Get openid : " + jsApiPay.openid);

                Repeater repBannerList = (Repeater)this.Master.FindControl("ContentPlaceHolder1").FindControl("repBannerList");
                if (repBannerList != null)
                {
                    var bannerList = Service.CacheService.POSBankList.Where(c => c.Code == "2");
                    repBannerList.DataSource = bannerList;
                    repBannerList.DataBind();
                }


                if (Session["Member"] != null)
                {
                    loginUserModel = Session["Member"] as Model.Member;
                    string checkSql = "SELECT COUNT(1) FROM dbo.EN_SignUp WHERE DATEDIFF(dd,SignDate,GETDATE())=0  AND CourseCode='1' AND MCode='" + loginUserModel.ID + "'";
                    object obj = CommonBase.GetSingle(checkSql);
                    if (MethodHelper.ConvertHelper.ToInt32(obj, 0) > 0)
                    {
                        isSignUp = "1";
                    }
                    if (loginUserModel.RoleCode == "Member")
                    {
                        isShowSJ = true;
                    }
                    if (loginUserModel.RoleCode == "VIP")
                    {
                        List<Model.Member> upListMember = CommonBase.GetList<Model.Member>("MID LIKE '" + loginUserModel.MID + "%'");
                        Model.Member upMember = null;
                        foreach (Model.Member mem in upListMember)
                        {
                            if (mem.RoleCode != "VIP" && mem.RoleCode != "Member")
                            {
                                upMember = mem;
                            }
                        }
                        if (upMember != null)
                        {
                            string mtjAreaLeave = upMember.Role.AreaLeave;

                            if (mtjAreaLeave != null && Convert.ToInt16(mtjAreaLeave) <= 30)
                            {
                                //本人已经是二级分销商了
                                applyRoleMoney = 0;
                            }

                            if (mtjAreaLeave != null && Convert.ToInt16(mtjAreaLeave) <= 40)
                            {
                                //本人已经是三级分销商了，需要申请二级
                                decimal app3money = MethodHelper.ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent3Money").Value, 0);
                                decimal app2money = MethodHelper.ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent2Money").Value, 0);
                                applyRoleMoney = app2money - app3money;
                            }
                        }
                    }
                    else
                    {
                        applyRoleMoney = MethodHelper.ConvertHelper.ToDecimal(Service.CacheService.GlobleConfig.Field1, 0);
                    }
                }
            }
        }
    }
}