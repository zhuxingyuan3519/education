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
    public partial class guide_aboutus : System.Web.UI.Page
    {
        protected string txtNContent = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack){
                Sys_WebConfig config = CacheService.WebConfigList.Where(c => c.Code == "aboutus").FirstOrDefault();
                if (config != null)
                {
                    txtNContent = config.Value;
                }
            }
        }
    }
}