using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

namespace Web.m.app
{
    public partial class course_detail: System.Web.UI.Page
    {
        protected EN_Course course;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                course = CommonBase.GetModel<EN_Course>(Request.QueryString["code"]);
                if(course == null)
                    course = new EN_Course();
            }
        }
    }
}