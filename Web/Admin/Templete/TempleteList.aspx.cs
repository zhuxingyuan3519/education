using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Templete
{
    public partial class TempleteList : BasePage
    {
        protected override void SetPowerZone()
        {
            BindDDLFromDict("TempType", ddlType);
            ddlType.Items.Insert(0, new ListItem("--模板类型--", ""));
        }
    }
}