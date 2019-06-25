
using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.WebAdmin
{
    public partial class GlobleConfig : BasePage
    {
        protected override void SetPowerZone()
        {
            Sys_GlobleConfig model = CommonBase.GetList<Sys_GlobleConfig>("").FirstOrDefault();
            if (model != null)
            {
                txt_Address.Value = model.Address;
                txt_Contacter.Value = model.Contacter;
                txt_Email.Value = model.Email;
                txt_Phone.Value = model.Phone;
                txt_QQ.Value = model.QQ;
                txt_MinTXMoney.Value = model.MinTXMoney.ToString();
                txt_Weixin.Value = model.Weixin;
                txt_TXFloat.Value = model.TXFloat.ToString();
                txt_BaseJifen.Value = model.BaseJifen.ToString();

                txt_Field1.Value = model.Field1;
                txt_Field2.Value = model.Field2;
                txt_Field3.Value = model.Field3;
                hid_YunPayQCord.Value = model.Field4;
                if (!string.IsNullOrEmpty(model.Field4))
                    img_Yunpay.Src = ".."+model.Field4;

                hidQRCodeCombine.Value = model.Field5;
                if (!string.IsNullOrEmpty(model.Field5))
                    imgQRCodeCombine.Src = model.Field5.Replace("~", "..");

            }
        }
        protected override void SetValue(string id)
        {

        }
        protected override string btnAdd_Click()
        {
            Sys_GlobleConfig model = CommonBase.GetList<Sys_GlobleConfig>("").FirstOrDefault();
            string content = Request.Form["editor"];
            model.Address = Request.Form["txt_Address"];
            model.Contacter = Request.Form["txt_Contacter"];
            model.Email = Request.Form["txt_Email"];
            model.Phone = Request.Form["txt_Phone"];
            model.Weixin = Request.Form["txt_Weixin"];
            model.Field1 = Request.Form["txt_Field1"];
            model.Field2 = Request.Form["txt_Field2"];
            model.Field3 = Request.Form["txt_Field3"];
            model.Field4 = Request.Form["hid_YunPayQCord"];
            model.Field5 = Request.Form["hidQRCodeCombine"];
            model.BaseJifen = string.IsNullOrEmpty(Request.Form["txt_BaseJifen"]) ? 0 : decimal.Parse(Request.Form["txt_BaseJifen"]);
            model.QQ = Request.Form["txt_QQ"];
            model.TXFloat = string.IsNullOrEmpty(Request.Form["txt_TXFloat"]) ? 0 : decimal.Parse(Request.Form["txt_TXFloat"]);
            model.MinTXMoney = string.IsNullOrEmpty(Request.Form["txt_MinTXMoney"]) ? 0 : decimal.Parse(Request.Form["txt_MinTXMoney"]);
            if (CommonBase.Update<Sys_GlobleConfig>(model))
            {
                //刷新缓存
                CacheHelper.RemoveAllCache("Sys_GlobleConfig");
                return "1";
            }
            //}
            return "0";
        }
    }
}