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
    public partial class reward_user_setting : BasePage
    {
        public string typeName = "";
        protected override void SetPowerZone()
        {
            string type = Request.QueryString["type"];
            if (!string.IsNullOrEmpty(type))
            {
                hid_type.Value = type;
                List<SH_HirePurchaseDetail> list = CommonBase.GetList<SH_HirePurchaseDetail>("HirePurchaseId='" + TModel.ID + "' and PayStatus=1");
                if (type == "1")
                    typeName = "股东人员及分配比例";
                else if (type == "2")
                    typeName = "老师人员及分配比例";
                //查询出明细的人
                string sql = " select UserCode,HireMoney*100 HireMoney from SH_HirePurchaseDetail where HirePurchaseId='" + TModel.ID + "' and PayStatus=" + type;
                rep_list.DataSource = CommonBase.GetTable(sql);
                rep_list.DataBind();
            }

            //TD_SHMoney shmoneyGudong = CommonBase.GetList<TD_SHMoney>("Code='GuDong' and RoleCode='" + TModel.ID + "'").FirstOrDefault();
            //if (shmoneyGudong != null)
            //{
            //    txt_gudong.Value = (shmoneyGudong.TJFloat * 100).ToString();
            //}

            ////获取到股东比例
            //TD_SHMoney shmoneyLaoshi = CommonBase.GetList<TD_SHMoney>("Code='LaoShi' and RoleCode='" + TModel.ID + "'").FirstOrDefault();
            //if (shmoneyLaoshi != null)
            //{
            //    txt_laoshi.Value = (shmoneyLaoshi.TJFloat * 100).ToString();
            //}
        }

        protected override string btnAdd_Click()
        {
            IList<SH_HirePurchaseDetail> list = GetDetailModelList();
            List<CommonObject> listComm = new List<CommonObject>();
            string hidType = Request.Form[NameHeader + "hid_type"];
            //先删除所有的
            listComm.Add(new CommonObject("delete from SH_HirePurchaseDetail where HirePurchaseId='" + TModel.ID + "' and PayStatus=" + hidType, null));
            foreach (SH_HirePurchaseDetail objNew in list)
            {
                objNew.CreatedBy = TModel.MID;
                objNew.CreatedTime = DateTime.Now;
                objNew.HireMoney = objNew.HireMoney / 100;
                objNew.PayDate = DateTime.Now;
                objNew.PayStatus = hidType == "1" ? 1 : 2;
                //objNew.UserId
                //查询到会员是否存在
                Model.Member model = CommonBase.GetList<Model.Member>("MID='" + objNew.UserCode + "'").FirstOrDefault();
                if (model == null)
                {
                    return objNew.UserCode + "不存在";
                    break;
                }
                objNew.UserId = model.ID;
                CommonBase.Insert<SH_HirePurchaseDetail>(objNew, listComm);
            }


            if (CommonBase.RunListCommit(listComm))
            {
                return "1";
            }
            return "0";
        }


        public string[] EditTableKeysForTrain { get { return new string[] { "Code", "Rate" }; } }

        protected IList<SH_HirePurchaseDetail> GetDetailModelList()
        {
            string[] arrRequest = Request.Form.AllKeys;
            IList<string> newDetailListTrain = new List<string>();
            IList<SH_HirePurchaseDetail> listSafe = new List<SH_HirePurchaseDetail>();
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
        protected IList<SH_HirePurchaseDetail> AddToTotalModel(IList<SH_HirePurchaseDetail> toAdd, IList<SH_HirePurchaseDetail> origin)
        {
            foreach (SH_HirePurchaseDetail obj in origin)
            {
                SH_HirePurchaseDetail newObj = obj;
                toAdd.Add(newObj);
            }
            return toAdd;
        }


        protected IList<SH_HirePurchaseDetail> AddListModel(IList<string> strFlags, string[] EditTableKeys)
        {
            IList<SH_HirePurchaseDetail> list = new List<SH_HirePurchaseDetail>();
            object id = null; string code = "", name = "";
            foreach (string str in strFlags)
            {
                foreach (string sin in EditTableKeys)
                {
                    switch (sin)
                    {
                        case "Code":
                            code = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                code = Request.Form[sin + "_" + str];
                            break;
                        case "Rate":
                            name = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                name = Request.Form[sin + "_" + str];
                            break;
                    }
                }
                list.Add(NewEntity(code, name));
            }
            return list;
        }
        private SH_HirePurchaseDetail NewEntity(string code, string name)
        {
            SH_HirePurchaseDetail obj = null;
            if (!string.IsNullOrEmpty(name))
            {
                obj = new SH_HirePurchaseDetail { HirePurchaseId = TModel.ID, Code = GetGuid, UserCode = code, HireMoney = MethodHelper.ConvertHelper.ToDecimal(name, 0) };
            }
            return obj;
        }



    }
}