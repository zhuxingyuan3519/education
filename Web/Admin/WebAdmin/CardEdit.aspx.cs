using MethodHelper;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.WebAdmin
{
    public partial class CardEdit : BasePage
    {
        protected override void SetPowerZone()
        {
            ddlBank.DataSource = CacheService.BankList;
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                //新增的时候
                ddlBank.DataSource = CacheService.BankList.Where(c => string.IsNullOrEmpty(c.LinkUrl));
            }
            ddlBank.DataTextField = "Remark";
            ddlBank.DataValueField = "ID";
            ddlBank.DataBind();
        }

        protected override void SetValue(string id)
        {
            Model.Sys_BankInfo bank = CacheService.BankList.FirstOrDefault(c => c.Id == int.Parse(id));
            ddlBank.Value = id;
            txt_content.Value = bank.LinkUrl;
        }

        protected override string btnAdd_Click()
        {
            Model.Sys_BankInfo bank = CacheService.BankList.FirstOrDefault(c => c.Id == int.Parse(Request.Form["ddlBank"]));
            bank.LinkUrl = Request["txt_content"];
            if (DBUtility.CommonBase.Update<Model.Sys_BankInfo>(bank, new string[] { "LinkUrl" }))
            {
                //更新缓存
                CacheHelper.RemoveAllCache("Sys_BankInfo");
                return "1";
            }
            return "0";
        }
    }
}