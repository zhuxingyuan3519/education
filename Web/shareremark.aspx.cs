using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class shareremark : BasePage
    {
        protected override void SetPowerZone()
        {
            //string code = Request.QueryString["code"];
            //if (!string.IsNullOrEmpty(code))
            //{
            //    Model.Member member = DBUtility.CommonBase.GetList<Model.Member>("Branch='" + code + "'").FirstOrDefault();
            //    if (member != null)
            //    {
            //        imgQCode.Src = "~/Attachment/QRCode/" + member.Branch + ".png";
            //        spTel.InnerHtml = member.Tel;
            //        sName.InnerHtml = member.MName;
            //    }
            //}
        }
    }
}