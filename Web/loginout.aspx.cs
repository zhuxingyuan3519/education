using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class loginout : BasePage
    {
        protected override void SetPowerZone()
        {
            Session["Member"] = null;
            TModel = null;
            //Server.Transfer("index.aspx");
            string v = Request.QueryString["v"];
            if (!string.IsNullOrEmpty(v))
                Response.Redirect("/m/app/main_mine");
            else
                Response.Redirect("index");
        }
    }
}