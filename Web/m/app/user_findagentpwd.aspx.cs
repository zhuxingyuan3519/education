using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class user_findagentpwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Action"]))
                {
                    if (Request.QueryString["Action"].ToUpper() == "ADD")
                    {
                        Response.Write(btnAdd_Click());
                        Response.End();
                    }
                }
            }
        }

        public static string NameHeader = "ctl00$ContentPlaceHolder1$";

        protected string btnAdd_Click()
        {
            //查看验证码是否正确
            string validCode = Request.Form[NameHeader + VerificationCode.ClientID];
            string telePhone = Request.Form[NameHeader + txt_MID.ClientID];
            if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") != "kgjpersonaltest")
            {
                if (!TelephoneCodeService.CheckValidCode(telePhone, validCode, "找回密码"))
                {
                    return "-3";
                }
            }
            Member model = CommonBase.GetList<Model.Member>("Tel='" + telePhone + "' and RoleCode in ('1F','2F','3F','Admin','City','Province','Zone')").FirstOrDefault();
            if (model == null)
            {
                return "2";
            }
            //密码都用DES加密，为了后台解密
            model.Password = CommonHelper.DESEncrypt(Request.Form[NameHeader + txt_Password2.ClientID]);
            List<CommonObject> listComm = new List<CommonObject>();
            CommonBase.Update<Member>(model, new string[] { "Password" }, listComm);
            LogService.Log(model, "7", model.MID + "修改密码", listComm);
            if (CommonBase.RunListCommit(listComm))
            {
                return "1";
            }
            return "0";
        }
    }
}