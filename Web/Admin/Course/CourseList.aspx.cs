
using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Course
{
    public partial class CourseList : BasePage
    {
        protected override void SetPowerZone()
        {
            rep_shmoneyList.DataSource = CommonBase.GetList<TD_SHMoney_Dict>("IsDeleted=0");
            rep_shmoneyList.DataBind();
        }
    }
}