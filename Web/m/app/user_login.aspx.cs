using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class user_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //以这种方式才能取到子页面中的控件
            //HtmlInputText hid = (HtmlInputText)this.Master.FindControl("ContentPlaceHolder1").FindControl("hidhid");
            //if (hid != null)
            //{

            //}
        }
    }
}