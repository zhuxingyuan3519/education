using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class finance_creditmarket : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                repBankList.DataSource = Service.CacheService.CreditMarketList;
                repBankList.DataBind();
            }
        }

    }
}