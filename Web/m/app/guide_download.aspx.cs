using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class guide_download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string globlcacheimg = GlobleConfigService.GetWebConfig("downloadappimg").Value;
                if(string.IsNullOrEmpty(globlcacheimg))
                {
                    imgDownLoadapp.Src = "/images/downloadapp.jpg";
                }
                else
                {
                       imgDownLoadapp.Src = globlcacheimg;
                }
            }
        }
    }
}