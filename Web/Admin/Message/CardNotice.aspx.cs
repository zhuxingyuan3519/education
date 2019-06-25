using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Message
{
    public partial class CardNotice : BasePage
    {
        protected string type = string.Empty;
        protected override void SetPowerZone()
        {
            type = Request.QueryString["type"];

            repBankList.DataSource = Service.CacheService.BankList;
            repBankList.DataBind();
        }
    }
}