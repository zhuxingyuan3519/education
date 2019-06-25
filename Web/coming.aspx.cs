using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class coming : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                M_WXUserInfo wxUser = CommonBase.GetModel<M_WXUserInfo>("oef_-0r3kgk85qEtLEVo4RplRk8A");
                if (wxUser != null)
                {
                    Session["WXMember"] = wxUser;
                }
            }
        }
    }
}