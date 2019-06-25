using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.WebAdmin
{
    public partial class TradeConfig: BasePage
    {
        protected override void SetPowerZone()
        {
            BindRep();
        }
        protected void BindRep()
        {
            List<TD_SHMoney> list = CommonBase.GetList<TD_SHMoney>("");
            foreach(TD_SHMoney shoney in list)
            {
                if(shoney.Field3 == "1")
                {
                    shoney.Remark = shoney.Remark + "（配置比例）";
                }
                else if(shoney.Field3 == "2")
                {
                    shoney.Remark = shoney.Remark + "（具体金额）";
                }
            }
            rep_KCTJList.DataSource = list.Where(c => c.Code == "KCTJ");
            rep_KCTJList.DataBind();

            var listRole = CommonBase.GetList<Sys_Role>("IsDeleted=0 and IsAdmin=0 order by RIndex asc");
            repRoleConfig.DataSource = listRole;
            repRoleConfig.DataBind();

            //rep_KCTraining.DataSource = list.Where(c => c.Code == "KCTraining");
            //rep_KCTraining.DataBind();


            //rep_KCAgent.DataSource = list.Where(c => c.Code == "KCAgent");
            //rep_KCAgent.DataBind();

            //rep_KCOperate.DataSource = list.Where(c => c.Code == "KCOperate");
            //rep_KCOperate.DataBind();

            //rep_KCYunlianhui.DataSource = list.Where(c => c.Code == "KCYunlianhui");
            //rep_KCYunlianhui.DataBind();

        }
        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            string[] allkeys = Request.Form.AllKeys;
            GetSaveData(allkeys, listComm, "KCTJ");
            GetSaveRoleData(allkeys, listComm);
            //GetSaveData(allkeys, listComm, "KCTraining");
            //GetSaveData(allkeys, listComm, "KCOperate");
            //GetSaveData(allkeys, listComm, "KCYunlianhui");
            //GetSaveData(allkeys, listComm, "KCAgent");
            //if (str.StartsWith("TJFloat_"))
            //{
            //    string id = str.Split('_')[1];
            //    string val = Request.Form[str];
            //    //更新
            //    TD_SHMoney shMoney = CommonBase.GetModel<TD_SHMoney>(id);
            //    shMoney.TJFloat = decimal.Parse(val);
            //    CommonBase.Update<TD_SHMoney>(shMoney, new string[] { "TJFloat" }, listComm);
            //}
            if(CommonBase.RunListCommit(listComm))
            {
                CacheHelper.RemoveAllCache("TD_SHMoney");
                CacheHelper.RemoveAllCache("Sys_Role");
                return "操作成功";
            }
            else
                return "操作失败";
        }

        protected void GetSaveData(string[] allkeys, List<CommonObject> listComm, string startWith)
        {
            foreach(string str in allkeys)
            {
                if(str.StartsWith(startWith + "_"))
                {
                    string id = str.Split('_')[1];
                    string val = Request.Form[str];
                    //更新
                    TD_SHMoney shMoney = CommonBase.GetModel<TD_SHMoney>(id);
                    shMoney.TJFloat = decimal.Parse(val);
                    CommonBase.Update<TD_SHMoney>(shMoney, new string[] { "TJFloat" }, listComm);
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
                        roleModel.ToPrizeMoney = MethodHelper.ConvertHelper.ToDecimal(Request.Form["ToPrizeMoney_" + i], 0);
                        CommonBase.Update<Sys_Role>(roleModel, new string[] { "Remark", "AreaLeave","ToPrizeMoney" }, listComm);
                    }
                }
            }
        }
    }
}