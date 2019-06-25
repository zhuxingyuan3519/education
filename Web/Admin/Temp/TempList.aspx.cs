using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Temp
{
    public partial class TempList : BasePage
    {
        protected override void SetPowerZone()
        {
            txtKey.DataSource = CacheService.BankList;
            txtKey.DataTextField = "Remark";
            txtKey.DataValueField = "Code";
            txtKey.DataBind();
            txtKey.Items.Insert(0, new ListItem("--请选择--", ""));
        }
    }
}