
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
    public partial class RedBagGlobleConfig : BasePage
    {
        protected override void SetPowerZone()
        {
            SH_RedBagConfig model = CommonBase.GetList<SH_RedBagConfig>("").FirstOrDefault();
            if (model != null)
            {
                txt_ActiveForTJCount.Value = model.ActiveForTJCount.ToString();
                txt_ActiveForTJVIPCount.Value = model.ActiveForTJVIPCount.ToString();
                txt_CompanyReturnMoney.Value = model.CompanyReturnMoney.ToString();
                txt_PersonalReturnMoney.Value = model.PersonalReturnMoney.ToString();
            }
        }
        protected override void SetValue(string id)
        {

        }
        protected override string btnAdd_Click()
        {
            SH_RedBagConfig model = CommonBase.GetList<SH_RedBagConfig>("").FirstOrDefault();
            model.PersonalReturnMoney =MethodHelper.ConvertHelper.ToDecimal( Request.Form["txt_PersonalReturnMoney"],0);
            model.CompanyReturnMoney = MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_PersonalReturnMoney"], 0);
            model.ActiveForTJCount = MethodHelper.ConvertHelper.ToInt32(Request.Form["txt_ActiveForTJCount"], 0);
            model.ActiveForTJVIPCount = MethodHelper.ConvertHelper.ToInt32(Request.Form["txt_ActiveForTJVIPCount"], 0);
            if (CommonBase.Update<SH_RedBagConfig>(model))
            {
                //刷新缓存
                CacheHelper.RemoveAllCache("SH_RedBagConfig");
                return "1";
            }
            //}
            return "0";
        }
    }
}