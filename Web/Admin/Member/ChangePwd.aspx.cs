using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin
{
    public partial class ChangePwd : BasePage
    {

        protected override string btnAdd_Click()
        {
            Model.Member model = TModel;
            if (model != null)
            {
                string orginPwd = Request["txtOrginPwd"];
                string newPwd = Request["Password2"];
                //判断原始密码正确不
                bool isWrong = false;

                if (model.Password != "olkedsauoiklmgradnmjuoir" && model.Password != System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(orginPwd, "MD5").ToUpper())
                {
                    isWrong = true;
                }
                if (isWrong)
                {
                    if (CommonHelper.DESEncrypt(orginPwd) != TModel.Password)
                        isWrong = true;
                    else
                        isWrong = false;
                }
                if (isWrong)
                {
                    return "1";
                }
                //密码都用DES加密，为了后台解密
                model.Password = CommonHelper.DESEncrypt(newPwd);
                ////改密码
                //    model.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newPwd, "MD5").ToUpper();
                if (CommonBase.Update<Model.Member>(model, new string[] { "Password" }))
                    return "2";
                else
                    return "0";
            }
            return "0";
        }
    }
}