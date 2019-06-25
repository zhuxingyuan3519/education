using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class talk : BasePage
    {
        protected override void SetPowerZone()
        {
            DataTable dtMsg = MessageService.GetNoReadMessage(TModel.ID.ToString());
            repMsgList.DataSource = dtMsg;
            repMsgList.DataBind();
        }
    }
}