using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.custom
{
    public partial class GoodForMember : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                repList.DataSource = Service.CacheService.POSBankList.Where(c => c.Code == "4");
                repList.DataBind();
            }
        }
    }
}