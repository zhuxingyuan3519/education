using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WxPayAPI;

namespace WXPay
{
    public partial class ResultNotifyPage: System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResultNotify resultNotify = new ResultNotify(this);
            //Model.EN_Course course = (Model.EN_Course)Session["TransCourse"];
            //if(course != null)
            //{
            //    resultNotify.ProcessNotify(course.Code);
            //}
            //else
            //{
                resultNotify.ProcessNotify(string.Empty);
            //}
            //Session["TransCourse"] = null;
        }
    }
}