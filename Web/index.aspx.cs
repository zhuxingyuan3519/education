using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class index : BasePage
    {
        protected string IsChangePlan = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (IsMobile())
                //{
                    Response.Redirect("/m/app/main_mine.aspx");
                //}
                //else
                //{
                //    Response.Redirect("/pc/main_mine.aspx");
                //}
            }
        }
    }
}