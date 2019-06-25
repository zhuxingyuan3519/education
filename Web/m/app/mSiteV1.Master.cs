using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class mSiteV1 : System.Web.UI.MasterPage
    {
        protected string isLogin = "0";// learnIndexString = "";
        protected Member mem = null;protected int IsVip = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Member"] == null)
                {
                    isLogin = "0";
                }
                else
                {
                    mem = Session["Member"] as Member;
                    isLogin = "1";
                    IsVip = mem.RoleCode.ToUpper() == "VIP" ? 1 : 0;

                    //learnIndexString = mem.Learns;
                }
            }
        }
    }
}