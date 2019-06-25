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
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected string isLogin = "0";
        protected Member mem = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Member"] == null)
                {
                    afterlogininfo.Visible = false;
                    isLogin = "0";
                }
                else
                {
                    mem = Session["Member"] as Member;
                    afterlogininfo.Items[0].Text = "欢迎：" + mem.MName;
                    logininfo.Visible = false;
                    logininfo2.Visible = false;
                    isLogin = "1";
                }
            }
        }
    }
}