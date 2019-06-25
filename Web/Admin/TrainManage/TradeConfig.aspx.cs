using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.TrainManage
{
    public partial class TradeConfig: BasePage
    {
        protected override void SetPowerZone()
        {
            BindRep();
        }

        protected void rptypelist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TD_ChargeList chargeModel = e.Item.DataItem as TD_ChargeList;

                Repeater rep_ShoneyList = e.Item.FindControl("rep_ShoneyList") as Repeater;
                if(rep_ShoneyList != null)
                {
                    List<TD_SHMoney> listShmoney = CommonBase.GetList<TD_SHMoney>("Code='" + chargeModel.Code + "'  order by TJIndex");
                    if(listShmoney.Count == 0)
                        listShmoney = CommonBase.GetList<TD_SHMoney>("Code='TrainFH' order by TJIndex");
                    rep_ShoneyList.DataSource = listShmoney;
                    rep_ShoneyList.DataBind();
                }
            }
        }

        protected void BindRep()
        {
            List<TD_ChargeList> list = CommonBase.GetList<TD_ChargeList>("");
            foreach(TD_ChargeList shoney in list)
            {
                string chargeList = shoney.ChargeList;
                string trainName = string.Empty;
                foreach(string str in chargeList.Split('|'))
                {
                    switch(str)
                    {
                        case "1": trainName += "混合词训练，"; break;
                        case "2": trainName += "数字训练，"; break;
                        case "3": trainName += "扑克牌训练，"; break;
                        case "4": trainName += "字母训练，"; break;
                    }
                    shoney.ChargeList = trainName;
                }
            }
            repSHMoneyTypeList.DataSource = list;
            repSHMoneyTypeList.DataBind();

        }
        protected override string btnAdd_Click()
        {
            string pram = Request.Form["param"];
            List<TD_SHMoney> list = JsonHelper.JsonToList<List<Model.TD_SHMoney>>(pram);

            List<CommonObject> listComm = new List<CommonObject>();
            foreach(TD_SHMoney shomoney in list)
            {
                string deleteSQL = "delete from TD_SHMoney where Code='" + shomoney.Code + "'";
                listComm.Add(new CommonObject(deleteSQL, null));
            }

            foreach(TD_SHMoney shomoney in list)
            {
                CommonBase.Insert<TD_SHMoney>(shomoney, listComm);
            }

            if(CommonBase.RunListCommit(listComm))
            {
                CacheHelper.RemoveAllCache("TD_SHMoney");
                //CacheHelper.RemoveAllCache("Sys_Role");
                return "操作成功";
            }
            else
                return "操作失败";
        }

        protected void GetSaveData(string[] allkeys, TD_SHMoney shMoney, string startWith)
        {
            foreach(string str in allkeys)
            {
                if(str.StartsWith(startWith + "_"))
                {
                    string code = str.Split('_')[0];
                    string id = str.Split('_')[1];
                    string field = str.Split('_')[2];
                    string val = Request.Form[str];
                    shMoney.Code = code;
                    if(field == "TJFloat")
                        shMoney.TJFloat = decimal.Parse(val);
                    if(field == "RoleCode")
                        shMoney.RoleCode = val;
                    if(field == "Remark")
                        shMoney.Remark = val;
                    if(field == "Field3")
                    {
                        shMoney.Field3 = (val == "1" ? "1" : "2");
                    }
                }
            }
        }

        protected void GetSaveRoleData(string[] allkeys, List<CommonObject> listComm)
        {
            string roleCode = Request.Form["RoleCode"];
            if(!string.IsNullOrEmpty(roleCode))
            {
                for(int i = 0; i < roleCode.Split(',').Length; i++)
                {
                    string role = roleCode.Split(',')[i];
                    Sys_Role roleModel = Service.CacheService.RoleList.FirstOrDefault(c => c.Code == role);
                    if(roleModel != null)
                    {
                        roleModel.Remark = Request.Form["Remark_" + i];
                        roleModel.AreaLeave = Request.Form["AreaLeave_" + i];
                        CommonBase.Update<Sys_Role>(roleModel, new string[] { "Remark", "AreaLeave" }, listComm);
                    }
                }
            }
        }
    }
}