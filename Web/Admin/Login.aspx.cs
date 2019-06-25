using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin
{
    public partial class Login : BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            //Session["Member"] = Bll.Member.ManageMember;
            if (TModel != null)
                Response.Write("<script>window.top.location.href='/Admin/Index'</script>");
        }
    }
}