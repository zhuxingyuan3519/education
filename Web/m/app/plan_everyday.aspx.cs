using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class plan_everyday : BasePage
    {
        protected override void SetPowerZone()
        {
            List<CM_Archives> list = DBUtility.CommonBase.GetList<CM_Archives>("IsDeleted=0 and CustomId=" + TModel.ID);

            List<string> bankList = new List<string>();
            foreach (CM_Archives ca in list)
            {
                if (!bankList.Contains(ca.Bank))
                    bankList.Add(ca.Bank);
            }

            foreach (string bank in bankList)
            {
                ListItem li = new ListItem();
                li.Value = bank;
                Sys_BankInfo sb = CacheService.BankList.Where(c => c.Code == bank).FirstOrDefault();
                if (sb != null)
                {
                    li.Text = sb.Remark;
                    ddl_Bank.Items.Add(li);
                }
            }
            //ddl_Bank.DataSource = CacheService.BankList;
            //ddl_Bank.DataTextField = "Remark";
            //ddl_Bank.DataValueField = "Code";
            //ddl_Bank.DataBind();
            ddl_Bank.Items.Insert(0, new ListItem("--选择银行--", ""));
        }

    }
}