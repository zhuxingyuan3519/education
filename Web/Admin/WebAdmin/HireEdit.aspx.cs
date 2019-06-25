using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.WebAdmin
{
    public partial class HireEdit : BasePage
    {
        protected override void SetPowerZone()
        {
            ddlRole.DataSource = CacheService.RoleList;
            ddlRole.DataTextField = "Name";
            ddlRole.DataValueField = "Code";
            ddlRole.DataBind();
        }

        protected override void SetValue(string id)
        {
            hidId.Value = id;
            SH_HirePurchase model = CommonBase.GetModel<SH_HirePurchase>(id);
            ddlRole.Value = model.RoleCode;
            txt_EveryHireMoney.Value = model.EveryHireMoney.ToString();
            txt_HireCount.Value = model.HireCount.ToString();
            txt_LeaveTradePointCount.Value = model.LeaveTradePointCount.ToString();
            txt_PayDate.Value = model.PayDate.ToString();
            txt_TradePointCount.Value = model.TradePointCount.ToString();
        }
        protected Model.SH_HirePurchase GetModel(Model.SH_HirePurchase model)
        {
            model.RoleCode = Request.Form["ddlRole"];
            model.HireCount = int.Parse(Request.Form["txt_HireCount"]);
            model.PayDate = int.Parse(Request.Form["txt_PayDate"]);
            model.EveryHireMoney = int.Parse(Request.Form["txt_EveryHireMoney"]);
            model.TradePointCount = int.Parse(Request.Form["txt_TradePointCount"]);
            model.LeaveTradePointCount = int.Parse(Request.Form["txt_LeaveTradePointCount"]);
            model.HireType = 0;
            model.UserId = "";
            model.UserCode = "";
            return model;
        }

        protected override string btnAdd_Click()
        {
            if (!string.IsNullOrEmpty(Request["hidId"]))
            {
                Model.SH_HirePurchase model = CommonBase.GetModel<Model.SH_HirePurchase>(Request["hidId"]);
                if (model != null)
                {
                    model = GetModel(model);
                    if (CommonBase.Update<Model.SH_HirePurchase>(model))
                    {
                        return "1";
                    }
                }
            }
            else
            {
                Model.SH_HirePurchase model = new SH_HirePurchase();
                model = GetModel(model);
                model.Id = GetGuid;
                model.CreatedBy = TModel.MID;
                model.CreatedTime = DateTime.Now;
                if (CommonBase.Insert<Model.SH_HirePurchase>(model))
                {
                    return "1";
                }
            }

            return "0";
        }
    }
}