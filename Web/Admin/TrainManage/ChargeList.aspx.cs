using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.TrainManage
{
    public partial class ChargeList: BasePage
    {
        protected override void SetPowerZone()
        {
            var list = CommonBase.GetList<TD_ChargeList>("IsDeleted=0 Order  by Sort");
            foreach(TD_ChargeList ch in list)
            {
                string remark = string.Empty;
                string[] array = ch.ChargeList.Split('|');
                foreach(string str in array)
                {
                    string code = string.Empty;
                    switch(str)
                    {
                        case "1": code = "初级混合词训练"; break;
                        case "2": code = "数字训练"; break;
                        case "3": code = "扑克牌训练"; break;
                        case "4": code = "字母训练"; break;
                        case "5": code = "高级混合词训练"; break;
                    }
                    remark += "<input type='hidden' value='" + str + "'/>" + code + "&emsp;";
                }
                ch.Remark = remark;
            }
            rep_list.DataSource = list;
            rep_list.DataBind();

        }
    }
}