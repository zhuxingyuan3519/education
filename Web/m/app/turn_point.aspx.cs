using DBUtility;
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
    public partial class turn_point : BasePage
    {
        protected override void SetPowerZone()
        {
            List<Sys_Role> listRole = CacheService.RoleList.Where(c => c.Code == "VIP" || c.Code == "1F" || c.Code == "2F").OrderBy(c => c.RIndex).ToList();
            listRole = listRole.Where(c => c.RIndex < TModel.Role.RIndex).ToList();
            foreach (Sys_Role role in listRole)
            {
                ListItem li = new ListItem(role.Name, role.Code);
                li.Attributes.Add("point", role.AreaLeave);
                ddl_role.Items.Add(li);
            }
            if (TModel.RoleCode == "3F")
            {
                //如果自身是城市合伙人，在选择培训机构的时候，就要一定选择区域
                ddl_zone.DataSource = CacheService.SatandardAddressList.Where(c => c.LevelInt == 40 && c.CityCode == TModel.City);
                ddl_zone.DataTextField = "Name";
                ddl_zone.DataValueField = "AdCode";
                ddl_zone.DataBind();
            }
        }

        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            //获取到用户
            string mid = Request.Form["txt_mid"];
            Model.Member member = CommonBase.GetList<Model.Member>("MID='" + mid + "'").FirstOrDefault();
            if (member == null)
                return "-1";
            //查询到这个用户是不是在当前登录人名下
            string check = "SELECT COUNT(1) FROM dbo.FUN_CountTDMember('" + TModel.ID + "',0,99999) WHERE Code='" + member.ID + "'";
            int checkCount = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(check), 0);
            if (checkCount <= 0)
            {
                return "-4";
            }


            string rolecode = Request.Form[NameHeader + "ddl_role"];
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