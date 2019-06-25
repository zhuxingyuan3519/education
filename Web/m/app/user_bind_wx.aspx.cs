using DBUtility;
using Model;
using System;

namespace Web.m.app
{
    public partial class user_bind_wx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["WXMember"] == null)
                {
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
                                //isShowBindWX = "1";
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
                            int hasBindCount = CommonBase.GetList<M_WX_VS_User>("OpenId='" + wxUser.OpenId + "'").Count;
                            if (hasBindCount == 0)
                            {
                                //isShowBindWX = "1";
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
                        //ScriptAlert("获取微信用户信息失败");
                    }

                }

                //获取到这个微信登录的信息
                M_WXUserInfo wx_user = Session["WXMember"] as M_WXUserInfo;
                hid_openid.Value = wx_user.OpenId;
                img_wx_headimg.Src = wx_user.HeadImgUrl.Replace("\\","");
                sp_wxName.InnerHtml = wx_user.NickName;

                //绑定已有账号
                string sql = "select t1.OpenId,t2.ID,t2.MID,t2.MName,t2.Tel from M_WX_VS_User t1 left join Member t2 on t1.UserCode=t2.ID where t1.IsDeleted=0 and t1.OpenId='" + wx_user.OpenId + "'";
                rep_bindList.DataSource = CommonBase.GetTable(sql);
                rep_bindList.DataBind();

            }
        }
    }
}