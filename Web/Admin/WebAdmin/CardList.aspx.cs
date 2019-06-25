using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.WebAdmin
{
    public partial class CardList : BasePage
    {
        protected override void SetPowerZone()
        {
            txtKey.DataSource = CacheService.BankList;
            txtKey.DataTextField = "Remark";
            txtKey.DataValueField = "ID";
            txtKey.DataBind();
            txtKey.Items.Insert(0, new ListItem("--请选择--", ""));
        }
    }
}