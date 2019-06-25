using DBUtility;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Web.m.app
{

    public partial class no_rank_detail: BasePage
    {
        protected override void SetPowerZone()
        {
            ////查找体验学员:直接推荐的体验会员
            //rep_member.DataSource = CommonBase.GetList<Model.Member>("MTJ='" + TModel.ID + "' AND RoleCode='Member'");
            //rep_member.DataBind();

        }
    }
}