
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Service;

namespace Web.Admin.Member
{
    public partial class MemberList: BasePage
    {
        protected override void SetPowerZone()
        {
            ddlRoleCode.DataSource = CacheService.RoleList.Where(c => c.IsDeleted == false && (c.Code == "Member" || c.Code == "Teacher" || c.Code == "VIP"||c.Code=="Student"));
            ddlRoleCode.DataTextField = "Name";
            ddlRoleCode.DataValueField = "Code";
            ddlRoleCode.DataBind();
            ListItem li = new ListItem("--选择级别--", "");
            ddlRoleCode.Items.Insert(0,li);

            //string type = Request.QueryString["type"];
            //hidType.Value = type;
            //if (!string.IsNullOrEmpty(type))
            //{
            //    if (type == "Training")
            //    {
            //        ddlRoleCode.Items.Clear();
            //        ListItem li = new ListItem("培训机构", "Training");
            //        ddlRoleCode.Items.Add(li);
            //        ddlRoleCode.Style.Add("display", "none");
            //    }
            //    else if (type == "Agent")
            //    {
            //        ddlRoleCode.Items.Clear();
            //        ListItem li = new ListItem("", "");
            //        ddlRoleCode.Items.Add(li);
            //        li = new ListItem("创业合伙人", "1F");
            //        ddlRoleCode.Items.Add(li);
            //        li = new ListItem("区县级代理商", "2F");
            //        ddlRoleCode.Items.Add(li);
            //        li = new ListItem("市级代理商", "3F");
            //        ddlRoleCode.Items.Add(li);
            //    }
            //}
        }
    }
}