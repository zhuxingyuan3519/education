using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class main_application : System.Web.UI.Page
    {
        protected Member TModel;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Member"] != null)
                {
                    TModel = Session["Member"] as Member;
                    if (TModel != null)
                    {
                        if (!string.IsNullOrEmpty(TModel.Image))
                        {
                            img_coverImg.Src = TModel.Image;
                        }
                        else
                        {
                            M_WXUserInfo wxUser = Session["WXMember"] as M_WXUserInfo;
                            if (wxUser != null)
                            {
                                img_coverImg.Src = wxUser.HeadImgUrl;
                            }
                        }
                    }
                }
                else
                {
                    M_WXUserInfo wxUser = Session["WXMember"] as M_WXUserInfo;
                    if (wxUser != null)
                    {
                        img_coverImg.Src = wxUser.HeadImgUrl;
                    }
                }
            }
        }
    }
}