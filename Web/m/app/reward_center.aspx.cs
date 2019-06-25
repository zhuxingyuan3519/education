using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class reward_center : BasePage
    {
        protected override void SetPowerZone()
        {
            
        }

        protected string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            //获取到用户
            string mid = Request.Form["txt_mid"];
            Model.Member member = CommonBase.GetList<Model.Member>("MID='" + mid + "'").FirstOrDefault();
            if (member == null)
                return "-1";
            string rolecode = Request.Form["ddl_role"];
            int trunCount = MethodHelper.ConvertHelper.ToInt32(Request.Form["txt_count"], 0);
            Sys_Role role = CacheService.RoleList.FirstOrDefault(c => c.Code == rolecode);
            if (member.Role.RIndex < role.RIndex)
            {
                member.RoleCode = role.Code;
            }
            if (trunCount > TModel.LeaveTradePoints)
                return "-2";
            member.TradePoints += trunCount;
            member.LeaveTradePoints += trunCount;
            CommonBase.Update<Model.Member>(member, new string[] { "RoleCode", "TradePoints", "LeaveTradePoints" }, listComm);


            TModel.LeaveTradePoints -= trunCount;
            CommonBase.Update<Model.Member>(TModel, new string[] { "LeaveTradePoints" }, listComm);

            Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
            cost.Code = MethodHelper.CommonHelper.GetGuid;
            cost.CompanyCode = TModel.ID;
            cost.CostCount = trunCount;
            cost.CreatedBy = "system";
            cost.CreatedTime = DateTime.Now;
            cost.IsDeleted = false;
            cost.FromCompany = TModel.ID;
            cost.ToCompany = member.ID;
            cost.MID = TModel.MID;
            cost.Status = 2;
            cost.Remark = "转让给" + member.MName + cost.CostCount + "个VIP名额";
            CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);

            if (CommonBase.RunListCommit(listComm))
            {
                return "1";
            }
            return "0";
        }
    }
}