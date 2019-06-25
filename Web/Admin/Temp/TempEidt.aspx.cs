using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Temp
{
    public partial class TempEidt : BasePage
    {
        protected string planheaderid = string.Empty;
        protected override void SetPowerZone()
        {
            ddlBank.DataSource = CacheService.BankList;
            ddlBank.DataTextField = "Remark";
            ddlBank.DataValueField = "Code";
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, new ListItem("--请选择--", ""));

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                string mid = HttpUtility.UrlDecode(Request["id"].Trim());
                hidId.Value = mid;
                TemplateModel = CommonBase.GetModel<CM_Template>(mid);
            }

            rep_rates.DataSource = CommonBase.GetList<CM_Dict>("").Where(c => c.ParentCode == "PosRate" && c.IsDeleted == false);
            rep_rates.DataBind();

        }
        protected override void SetValue(string id)
        {
            //string mid = HttpUtility.UrlDecode(Request["id"].Trim());
            //hidId.Value = mid;
            //TemplateModel = Bll.CM_Template.GetModel(mid);
        }

        public Model.CM_Template TemplateModel
        {
            get
            {
                Model.CM_Template model = null;
                if (string.IsNullOrEmpty(Request.Form["hidId"]))
                {
                    model = new Model.CM_Template();
                    model.Code = GetGuid;
                    model.IsDeleted = false;
                }
                else
                {
                    model = CommonBase.GetModel<CM_Template>(Request.Form["hidId"].Trim());
                }
                model.Bank = Request.Form["ddlBank"];
                model.CostCount = int.Parse(Request.Form["txtCostCount"]);
                model.LeaveMoney = decimal.Parse(Request.Form["txtLeaveMoney"]);
                model.LeavePencent = decimal.Parse(Request.Form["txtLeavePencent"]);
                model.MaxMoney = decimal.Parse(Request.Form["txtMaxMoney"]);
                model.MinMoney = decimal.Parse(Request.Form["txtMinMoney"]);
                model.LeavePencent2 = decimal.Parse(Request.Form["txtLeavePencent2"]);
                model.OnlineTake1 = decimal.Parse(Request.Form["txtOnlineTake1"]);
                model.OnlineTake2 = decimal.Parse(Request.Form["txtOnlineTake2"]);
                model.RealTake1 = decimal.Parse(Request.Form["txtRealTake1"]);
                model.RealTake2 = decimal.Parse(Request.Form["txtRealTake2"]);
                return model;
            }
            set
            {
                if (value != null)
                {
                    txtMinMoney.Value = value.MinMoney.ToString();
                    txtMaxMoney.Value = value.MaxMoney.ToString();
                    txtLeavePencent.Value = value.LeavePencent.ToString();
                    txtLeaveMoney.Value = value.LeaveMoney.ToString();
                    txtCostCount.Value = value.CostCount.ToString();
                    ddlBank.Value = value.Bank;
                    planheaderid = value.Code;
                    txtLeavePencent2.Value = value.LeavePencent2.ToString();
                    txtOnlineTake1.Value = value.OnlineTake1.ToString();
                    txtOnlineTake2.Value = value.OnlineTake2.ToString();
                    txtRealTake1.Value = value.RealTake1.ToString();
                    txtRealTake2.Value = value.RealTake2.ToString();
                    //ViewState["valcode"] = value.Code;
                }
            }
        }

        protected string GetBandValue(object code)
        {
            string ret = string.Empty;
            string valcode = planheaderid;// ViewState["valcode"].ToString();
            Model.CM_PlanRateConfig config = CommonBase.GetList<CM_PlanRateConfig>("Rate=" + code.ToString() + " and PlanHeaderId='" + valcode + "'").FirstOrDefault();
            if (config != null)
                ret = config.RatePercent.ToString();
            return ret;
        }

        /// <summary>
        /// 更新基本资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override string btnAdd_Click()
        {
            List<CommonObject> hs = new List<CommonObject>();
            if (string.IsNullOrEmpty(Request.Form["hidId"])) //新增
            {
                Model.CM_Template comModel = TemplateModel;
                comModel.CreatedBy = TModel.MID;
                comModel.CreatedTime = DateTime.Now;
                CommonBase.Insert<CM_Template>(comModel, hs);
                //保存费率配置表
                IList<Model.CM_PlanRateConfig> rateConfigList = GetDetailModelList();
                foreach (Model.CM_PlanRateConfig co in rateConfigList)
                {
                    co.Code = GetGuid;
                    co.Company = 0;
                    co.PlanHeaderId = comModel.Code;
                    co.Status = 1;
                    CommonBase.Insert<CM_PlanRateConfig>(co, hs);
                }
                if (CommonBase.RunListCommit(hs))
                    return "1";
                return "操作失败";
            }
            else  //修改
            {
                Model.CM_Template comModel = TemplateModel;
                CommonBase.Update<CM_Template>(comModel, hs);
                IList<Model.CM_PlanRateConfig> rateConfigList = GetDetailModelList();
                //先删除
                string dele = "DELETE FROM CM_PlanRateConfig where PlanHeaderId='"+comModel.Code+"'";
                foreach (CM_PlanRateConfig config in rateConfigList)
                {
                    config.PlanHeaderId = comModel.Code;
                    config.Code = GetGuid;
                    config.Company = 0;
                    config.Status = 1;
                    CommonBase.Insert<CM_PlanRateConfig>(config, hs);
                }

                //foreach (Model.CM_PlanRateConfig co in rateConfigList)
                //{
                //    Model.CM_PlanRateConfig coModel = CommonBase.GetList<CM_PlanRateConfig>("IsDeleted=0  and Rate=" + co.Rate + " and PlanHeaderId='" + comModel.Code + "'").FirstOrDefault();
                //    if (comModel != null)
                //    {
                //        coModel.RatePercent = co.RatePercent;
                //        CommonBase.Update<CM_PlanRateConfig>(coModel, hs);
                //    }
                //    else
                //    {
                //        co.Code = GetGuid;
                //        co.Company = 0;
                //        co.PlanHeaderId = comModel.Code;
                //        co.Status = 1;
                //        CommonBase.Insert<CM_PlanRateConfig>(co, hs);
                //    }
                //}
                if (CommonBase.RunListCommit(hs))
                    return "1";
                return "操作失败";
            }
        }


        public string[] EditTableKeysForTrain { get { return new string[] { "rate", "ratePencent" }; } }

        protected IList<Model.CM_PlanRateConfig> GetDetailModelList()
        {
            string[] arrRequest = Request.Form.AllKeys;
            IList<string> newDetailListTrain = new List<string>();
            IList<Model.CM_PlanRateConfig> listSafe = new List<Model.CM_PlanRateConfig>();
            foreach (string str in arrRequest)
            {
                var key = str.Split('_')[0];
                if (EditTableNeedSaveKeys(key, EditTableKeysForTrain))
                {
                    newDetailListTrain.Add(str);
                }

            }
            IList<string> strFlagsTrain = GetSetedGuid(newDetailListTrain);

            listSafe = AddToTotalModel(listSafe, AddListModel(strFlagsTrain, EditTableKeysForTrain));

            return listSafe;
        }
        protected IList<Model.CM_PlanRateConfig> AddToTotalModel(IList<Model.CM_PlanRateConfig> toAdd, IList<Model.CM_PlanRateConfig> origin)
        {
            foreach (Model.CM_PlanRateConfig obj in origin)
            {
                Model.CM_PlanRateConfig newObj = obj;
                toAdd.Add(newObj);
            }
            return toAdd;
        }


        protected IList<Model.CM_PlanRateConfig> AddListModel(IList<string> strFlags, string[] EditTableKeys)
        {
            IList<Model.CM_PlanRateConfig> list = new List<Model.CM_PlanRateConfig>();

            foreach (string str in strFlags)
            {
                object id = null; string code = "", pencent = "";

                foreach (string sin in EditTableKeys)
                {
                    switch (sin)
                    {
                        case "hidId": id = Request.Form[sin + "_" + str]; break;
                        case "rate":
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                            {
                                code = Request.Form[sin + "_" + str];
                            }
                            break;
                        case "ratePencent":
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                            {
                                pencent = Request.Form[sin + "_" + str];
                            }
                            break;
                    }
                }
                list.Add(NewEntity(id, code, pencent));

            }
            return list;
        }
        private Model.CM_PlanRateConfig NewEntity(object id, string code, string pencent)
        {
            Model.CM_PlanRateConfig obj = null;
            if (!string.IsNullOrEmpty(code))
            {
                obj = new Model.CM_PlanRateConfig { Id = ToNullInt(id), Rate = ToNullDecimal(code), RatePercent = ToNullDecimal(pencent) };
            }
            return obj;
        }
    }
}