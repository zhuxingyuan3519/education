
using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.SystemManage
{
    public partial class RoleEdit : BasePage
    {
        protected List<Sys_Role> list;
        protected override void SetPowerZone()
        {
            list = CommonBase.GetList<Sys_Role>("IsDeleted=0");
        }
    }
}