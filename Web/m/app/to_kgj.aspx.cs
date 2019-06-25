using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class to_kgj: BasePage
    {
        protected string ReturnMsg = string.Empty, ReturnCode = string.Empty;
        protected override void SetPowerZone()
        {
            if(TModel.RoleCode == "Member")
            {
                if(TModel.UseEndTime.HasValue && TModel.UseEndTime.Value.Date < DateTime.Now.Date)
                {
                    ReturnCode = "1";
                    ReturnMsg = "您的卡管家使用时限已过，购买精品课程后，可继续使用卡管家系统内所有功能。";
                    return;
                }
            }
            else
            {
                if(TModel.UseEndTime.HasValue && TModel.UseEndTime.Value.Date < DateTime.Now.Date)
                {
                    ReturnCode = "1";
                    ReturnMsg = "您的卡管家使用时限已过，购买精品课程后，可继续使用卡管家系统内所有功能。";
                    return;
                }
            }


            if(TModel.UseEndTime.HasValue && TModel.UseBeginTime.Value.Date > DateTime.Now.Date)
            {
                ReturnCode = "2";
                ReturnMsg = "您可使用卡管家的时间未到，请在" + TModel.UseBeginTime.Value.Date + "日开始使用。";
                return;
            }

            string kgjUrl = MethodHelper.ConfigHelper.GetAppSettings("kgjInterfaceUrl");
            string appId = MethodHelper.ConfigHelper.GetAppSettings("appId");
            string appSecreat = MethodHelper.ConfigHelper.GetAppSettings("appSecreat");
            string mid = TModel.MID;
            //生成时间戳
            string timeSpan = MethodHelper.CommonHelper.GetTimeStamp();
            string md5string = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(appId + timeSpan + appSecreat, "MD5").ToUpper();
            //对要传入的mid(用户手机号)也要进行加密处理
            string sign = Encrypt_AES("token=" + md5string + "&mid=" + mid, appSecreat);
            string urlEncode = Server.UrlEncode(sign);
            string kgjUri = kgjUrl + "?timeSpan=" + timeSpan + "&appId=" + appId + "&sign=" + urlEncode;
            Response.Redirect(kgjUri);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">加密前明文内容</param>
        /// <param name="Key">32位密钥<</param>
        /// <returns></returns>
        public String Encrypt_AES(String str, string appSecreat)
        {
            try
            {
                Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(appSecreat);
                Byte[] toEncryptArray = System.Text.UTF8Encoding.UTF8.GetBytes(str);

                System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
                rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}