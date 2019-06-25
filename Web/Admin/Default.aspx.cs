using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;

namespace Web.Admin
{
    public partial class Default : BasePage
    {
        protected override void SetPowerZone()
        {
            if (TModel == null)
            {
                Response.Redirect("/Admin/Login.aspx");
                return;
            }
            Server.Transfer("Index.aspx");
        }

        protected override void SetValue(string id)
        {
            //Model.Member model = BllModel.GetModel(id);
            //if (model != null)
            //{
            //    if (TModel.MID == model.FMID || TModel.FMID == model.MID || TModel.Role.IsAdmin)
            //    {
            //        BllModel.TModel = model;
            //        FormsAuthentication.SetAuthCookie(model.MID, true);
            //    }
            //}
            Response.Redirect("Default.aspx");
        }
    }
}
