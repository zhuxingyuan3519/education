using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class credit_material : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string bankcode = Request.QueryString["bank"];
                if (!string.IsNullOrEmpty(bankcode))
                {
                    Model.Sys_BankInfo bank= CacheService.BankList.Where(c => c.Code == bankcode).FirstOrDefault();
                    if(bank!=null)
                    {
                        h4_bankName.InnerHtml = bank.Remark;
                    }
                }
            }
        }
    }
}