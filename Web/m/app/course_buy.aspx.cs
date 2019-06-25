using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using Model;

namespace Web.m.app
{
    public partial class course_buy: BasePage
    {
        protected EN_Course course;
        protected override void SetPowerZone()
        {
            Session["TransCourse"] = null;
            course = CommonBase.GetModel<EN_Course>(Request.QueryString["course"]);
            if(course == null)
                course = new EN_Course();
            else
            {
                ArrayList list = MethodHelper.CommonHelper.GetImgUrl(course.Remark);
                string src = "/m/image/bookpicker.png";
                if(list.Count > 0)
                {
                    src = list[0].ToString();
                }
                course.Remark = src;
            }
        }
    }
}