
using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Member
{
    public partial class SignUpList : BasePage
    {
        protected override void SetPowerZone()
        {
            //绑定课程
            ddlCourse.DataSource = CommonBase.GetList<Model.EN_Course>("IsDeleted=0");
            ddlCourse.DataTextField = "Name";
            ddlCourse.DataValueField = "Code";
            ddlCourse.DataBind();
            ddlCourse.Items.Insert(0, new ListItem("选择课程", ""));

            //绑定培训机构
            ddlTraing.DataSource = CommonBase.GetList<Model.Member>("IsClose=0 and RoleCode ='Training'");
            ddlTraing.DataTextField = "MName";
            ddlTraing.DataValueField = "ID";
            ddlTraing.DataBind();
            ddlTraing.Items.Insert(0, new ListItem("选择代理", ""));
        }
    }
}