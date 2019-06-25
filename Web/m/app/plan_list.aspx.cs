using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class plan_list : BasePage
    {
        protected override void SetPowerZone()
        {
            //绑定自己的卡信息
            List<CM_Archives> listBank = CommonBase.GetList<CM_Archives>("IsDeleted=0 and CustomId=" + TModel.ID);
            foreach (CM_Archives arch in listBank)
            {
                ddlArchiveBank.Items.Add(new ListItem(GetBankName(arch.Bank) + arch.CardID, arch.Code));
            }
        }
    }
}