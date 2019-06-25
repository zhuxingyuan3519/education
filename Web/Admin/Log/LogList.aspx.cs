using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Log
{
    public partial class LogList : BasePage
    {
        protected override void SetPowerZone()
        {
            string mid=Request.QueryString["mid"];
            nMID.Value = mid;
        }
    }
}