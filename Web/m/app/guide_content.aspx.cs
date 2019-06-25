using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class guide_content : BasePage
    {
        protected Sys_WebConfig webConfig;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                webConfig = GlobleConfigService.GetWebConfig(Request.QueryString["code"]);
            }
        }
    }
}