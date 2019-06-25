using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Web.Admin.Message
{
    public partial class NoticeList : BasePage
    {
        public string NType="0",bank="",type="";
        protected override void SetValue(string id)
        {
            NType = id;
            string bankQuery = Request.QueryString["bank"];
            bank = bankQuery;
            type= Request.QueryString["type"];
        }
    }
}