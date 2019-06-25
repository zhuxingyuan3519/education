
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Member
{
    public partial class AgentList : BasePage
    {
        protected override void SetPowerZone()
        {
            ddlRoleCode.DataSource = CacheService.RoleList.Where(c => c.IsDeleted == false && (c.Code == "1F" || c.Code == "2F" || c.Code == "3F"));
            ddlRoleCode.DataTextField = "Name";
            ddlRoleCode.DataValueField = "Code";
            ddlRoleCode.DataBind();
            ListItem li = new ListItem("--选择级别--", "");
            ddlRoleCode.Items.Insert(0, li);
        }
    }
}