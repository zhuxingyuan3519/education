using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class ext_remark : BasePage
    {
        protected override void SetPowerZone()
        {
            string globlcacheimg = GlobleConfigService.GetWebConfig("extremark").Value;
            if (string.IsNullOrEmpty(globlcacheimg))
            {
                imgextremark.Src = "/images/tjshow.jpg";
            }
            else
            {
                imgextremark.Src = globlcacheimg;
            }
        }
    }
}