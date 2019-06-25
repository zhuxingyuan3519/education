using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class credit_materialcontent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["ID"];
                if (!string.IsNullOrEmpty(id))
                {
                    Model.Notice model = DBUtility.CommonBase.GetModel<Model.Notice>(id);
                    if (model != null)
                    {
                        switch (model.NType)
                        {
                            case 1: spLable.InnerHtml = "系统公告"; break;
                            case 2: spLable.InnerHtml = "学习资料"; break;
                            case 3: spLable.InnerHtml = "相关新闻"; break;
                            case 4: spLable.InnerHtml = "信用贷款"; break;
                            case 5: spLable.InnerHtml = "新手指南"; break;
                            case 6: spLable.InnerHtml = "信贷口子"; break;
                        }
                        divTitle.InnerHtml = model.NTitle;
                        divContent.InnerHtml = model.NContent;
                        model.NClicks += 1;
                        DBUtility.CommonBase.Update<Model.Notice>(model, new string[] { "NClicks" });
                    }
                }
            }
        }
    }
}