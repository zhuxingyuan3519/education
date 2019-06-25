using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class share : System.Web.UI.Page
    {
        protected  void Page_Load(object sender, EventArgs e)
        {
            string code = Request.QueryString["code"];
            string registcode = Request.QueryString["registcode"];
            if (!string.IsNullOrEmpty(code))
            {
                Model.Member member = DBUtility.CommonBase.GetList<Model.Member>("Branch='" + code + "'").FirstOrDefault();
                if (member != null)
                {
                    //imgQCode.Src = "~/Attachment/QRCode/" + member.Branch + ".png";
                    spTel.InnerHtml = member.Tel;
                    sName.InnerHtml = member.MName;
                    sMID.InnerHtml = member.MID;
                }
            }
            if (!string.IsNullOrEmpty(registcode))
            {
                //DES解密
                string memberId = MethodHelper.CommonHelper.DESDecrypt(registcode);
                Model.Member member = DBUtility.CommonBase.GetModel<Model.Member>(memberId);
                if (member != null)
                {
                    spTel.InnerHtml += member.MID;
                    sName.InnerHtml = member.MName;
                    sMID.InnerHtml = member.MID;
                }
            }
        }
    }
}