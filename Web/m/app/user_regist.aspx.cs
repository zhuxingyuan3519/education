using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class user_regist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                M_WXUserInfo wxUser = Session["WXMember"] as M_WXUserInfo;
                if (wxUser != null)
                {
                    img_coverImg.Src = wxUser.HeadImgUrl;
                    sp_wxName.InnerHtml = wxUser.NickName;
                }

                //HtmlTextArea textArea = (HtmlTextArea)this.Page.FindControl("txtProtrol");
                //if (textArea != null)
                //{

                //}

                if (!string.IsNullOrEmpty(Request.QueryString["Action"]))
                {
                    if (Request.QueryString["Action"].ToUpper() == "ADD")
                    {
                        Response.Write(btnAdd_Click());
                        Response.End();
                    }
                    else if (Request.QueryString["Action"].ToUpper() == "GET")
                    {
                        Response.Write(GetMtjMid());
                        Response.End();
                    }
                }

                if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    //ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>window.localStorage.setItem('mtjcode', " + Request.QueryString["code"] + ");</script>");
                    //Response.Redirect("/website/index.html");
                    ////查询到推荐人信息
                    //string sql = "SELECT ID,MID FROM dbo.Member WHERE ID='" + Request.QueryString["code"] + "'";
                    //DataTable obj = CommonBase.GetTable(sql);
                    //if (obj != null && obj.Rows.Count > 0)
                    //{
                    //    txt_MTJ.Value = obj.Rows[0][0].ToString();
                    //    txt_MTJMID.Value = obj.Rows[0][1].ToString();

                    //    //把这个code存入cookie中
                    //    string cookName = ConfigurationManager.AppSettings["SystemID"] + "_MTJ";
                    //    if (HttpContext.Current.Request.Cookies[cookName] == null)
                    //    {
                    //        //do something
                    //        HttpCookie cookie = new HttpCookie(cookName);
                    //        cookie.Value = obj.Rows[0][0].ToString();
                    //        HttpContext.Current.Response.Cookies.Add(cookie);
                    //    }
                    //}
                    //Response.Redirect("index");
                }
                //else
                //{
                //    //读取cookie,
                //    string cookName = ConfigurationManager.AppSettings["SystemID"] + "_MTJ";
                //    if (Request.Cookies[cookName] != null)
                //    {
                //        string cookieCode = Server.HtmlEncode(Request.Cookies[cookName].Value);
                //        //查询到推荐人信息
                //        string sql = "SELECT ID,MID FROM dbo.Member WHERE ID='" + cookieCode + "'";
                //        DataTable obj = CommonBase.GetTable(sql);
                //        if (obj != null && obj.Rows.Count > 0)
                //        {
                //            txt_MTJ.Value = obj.Rows[0][0].ToString();
                //            txt_MTJMID.Value = obj.Rows[0][1].ToString();
                //        }
                //    }
                //}
            }
        }

        public static string NameHeader = "ctl00$ContentPlaceHolder1$";
        protected string GetMtjMid()
        {
            //查看验证码是否正确
            string mtjCode = Request.Form["mtjcode"];
            string testSql = "select MID from Member where  ID='" + mtjCode + "'";
            object obj = CommonBase.GetSingle(testSql);
            if (obj != null)
            {
                return obj.ToString();
            }
            return "";
        }
        protected string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            //查看验证码是否正确
            string validCode = Request.Form[NameHeader + VerificationCode.ClientID];
            string telePhone = Request.Form[NameHeader + txt_MID.ClientID];

            if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") != "kgjpersonaltest")
            {
                if (!TelephoneCodeService.CheckValidCode(telePhone, validCode, "用户注册"))
                {
                    return "-3";
                }
            }
            //验证成功，更新验证码信息
            listComm.Add(new CommonObject("update Sys_SendCode set IsUsed=1,ValidTime=GETDATE() where Telephone='" + telePhone + "' and SendCode='" + validCode + "' and IsUsed=0", null));
            Member model = new Member();
            model.MID = telePhone;

            bool isExest = false;
            //查看手机号是否已经注册过并且已经删除
            //Member isDeletedMember = CommonBase.GetList<Member>("MID='" + model.MID + "' and IsClose=1").FirstOrDefault();
            //if (isDeletedMember != null)
            //{
            //    isExest = true;
            //    model = isDeletedMember;
            //}


            string mtj = Request.Form[NameHeader + txt_MTJ.ClientID];
            string mtjMID = Request.Form[NameHeader + txt_MTJMID.ClientID];
            Model.Member tjModel = null;// 
            //如果没有推荐人，就把推荐人设为管理员
            if (string.IsNullOrEmpty(mtjMID))
            {
                Model.Member OMember = CommonBase.GetList<Model.Member>("RoleCode='Admin'").FirstOrDefault();
                tjModel = OMember;
                if (OMember != null)
                {
                    //查询到这个推荐人的Model，update at 2016年11月16日17:09:47
                    model.AreaId = OMember.AreaId;
                    model.Agent = OMember.ID;
                    model.UseRoleType = OMember.UseRoleType;
                    model.Company = OMember.ID;
                    model.MTJ = OMember.ID.ToString();
                }
            }
            else
            {
                tjModel = CommonBase.GetList<Model.Member>("MID='" + mtjMID + "'").FirstOrDefault();
                if (tjModel == null)
                {
                    return "2";
                }


                model.MTJ = tjModel.ID.ToString();
                List<string> agentRoleCodeList = new string[] { "1F", "2F", "3F", "City", "Province", "Zone" }.ToList();
                if (tjModel.RoleCode == "Admin")
                {
                    model.AreaId = tjModel.AreaId;
                    model.Agent = tjModel.ID;
                    model.UseRoleType = tjModel.UseRoleType;
                    model.Company = tjModel.ID;
                    model.MTJ = tjModel.ID.ToString();
                }
                else if (agentRoleCodeList.Contains(tjModel.RoleCode))//如果推荐人是代理商
                {
                    model.AreaId = tjModel.AreaId;
                    model.Agent = tjModel.ID;
                    model.UseRoleType = tjModel.UseRoleType;
                    model.Company = tjModel.Company;
                    model.MTJ = tjModel.ID.ToString();
                }
                else
                {
                    //查询到这个推荐人的Model，update at 2016年11月16日17:09:47
                    //Model.Member tjModel = CommonBase.GetModel<Member>(model.MTJ);
                    model.AreaId = tjModel.AreaId;
                    model.Agent = tjModel.Agent;
                    model.UseRoleType = tjModel.UseRoleType;
                    model.Company = tjModel.Company;
                }
            }

            Model.Member teacherMember = CommonBase.GetList<Model.Member>("MID='" + Request.Form[NameHeader + txt_TeacherMID.ClientID] + "'").FirstOrDefault();
            if (teacherMember != null)
            {
                model.ParentTrade = teacherMember.ID;
            }



            //密码都用DES加密，为了后台解密
            model.Password = CommonHelper.DESEncrypt(Request.Form[NameHeader + txt_Password2.ClientID]);
            //model.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Form[NameHeader + txt_Password2.ClientID], "MD5").ToUpper();
            model.MState = true;
            model.RoleCode = "Member";
            model.IsClose = false;
            model.Tel = model.MID;
            model.MCreateDate = DateTime.Now;
            model.UseBeginTime = DateTime.Now.AddDays(-1);
            model.UseEndTime = DateTime.Now.AddDays(ConvertHelper.ToInt32(CacheService.GlobleConfig.Field2, 30) - 1);
            model.MName = Request.Form[NameHeader + txt_Name.ClientID];// model.MID;

            //根据自动定位获取到的省市区县等地址信息，保存到数据库中
            //update by zhuxy at 2017年3月15日13:43:36
            string adcode = Request.Form["hid_location_adcode"];
            Sys_StandardArea areaModel = AddressService.GetAddressByAdCode(adcode);
            if (areaModel.LevelInt == 40)
            {
                model.Zone = areaModel.AdCode;
                //得到省
                model.Province = areaModel.ProCode;
                //找到市
                model.City = areaModel.CityCode;
            }

            //model.Province = Request.Form["hid_location_province"];
            //model.City = Request.Form["hid_location_city"];
            //model.Zone = Request.Form["hid_location_zone"];
            model.Town = Request.Form["hid_location_town"];
            model.RegistPointer = Request.Form["hid_location_pointer"];
            if (!isExest)
            {
                model.ID = MethodHelper.CommonHelper.GetGuid;

                //校验用户名是否存在
                string testSql = "select count(1) from Member where  MID='" + model.MID + "'";
                if (Convert.ToInt16(CommonBase.GetSingle(testSql)) > 0)
                    return "3";
                CommonBase.Insert<Member>(model, listComm);
                //微信与用户信息表插入一条数据
                M_WXUserInfo wxUser = Session["WXMember"] as M_WXUserInfo;
                if (wxUser != null)
                {
                    //return "1";
                    //M_WX_VS_User mwvu = new M_WX_VS_User();
                    //mwvu.Company = "0";
                    //mwvu.CreatedTime = DateTime.Now;
                    //mwvu.IsDeleted = false;
                    //mwvu.OpenId = wxUser.OpenId;
                    //mwvu.Sort = 1;
                    //mwvu.Status = 1;
                    //mwvu.UserCode = model.ID;
                    //CommonBase.Insert<M_WX_VS_User>(mwvu, listComm);
                }
            }
            else
            {
                CommonBase.Update<Member>(model, listComm);
            }

            if (CommonBase.RunListCommit(listComm))
            {
                //注册成之后，加入session
                LogService.Log(model, "7", model.MID + "注册账号");
                FormsAuthentication.SetAuthCookie(model.MID, true);
                Session["Member"] = model;
                return "1";
            }
            return "0";
        }
    }
}