using DBUtility;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;
using Model;
using System.Data;

namespace Web.m.app
{
    public partial class code_train: BasePage
    {
        protected string CheckMsg = string.Empty;
        protected override void SetPowerZone()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ChargeCode");
            dt.Columns.Add("CodeType");
            dt.Columns.Add("IsHave");
            dt.Columns.Add("HaveMsg");
            //获取到收费项目及个人权限
            string chaegeList = TModel.Learns;
            //校验是否从数据库中读取最新的
            //if(!string.IsNullOrEmpty(chaegeList))
            //{
            //if(!chaegeList.Contains("1") || !chaegeList.Contains("2") || !chaegeList.Contains("3") || !chaegeList.Contains("4"))
            //{
            Model.Member currentModel = CommonBase.GetModel<Model.Member>(TModel.ID);
            Session["Member"] = currentModel;
            chaegeList = currentModel.Learns;
            //MethodHelper.LogHelper.WriteTextLog(this.GetType().ToString(), "chaegeList:" + chaegeList, DateTime.Now);
            //    }
            //}

            List<TD_ChargeList> listChargeList = CacheService.ChargeList;
            //if(!string.IsNullOrEmpty(chaegeList))
            //{
            //string[] array = chaegeList.Split('|');
            for(int i = 1; i <= 5; i++)
            {
                TD_ChargeList charge = listChargeList.FirstOrDefault(c => c.ChargeList.Contains(i.ToString()));
                if(charge != null)
                {
                    DataRow row = dt.NewRow();
                    row["CodeType"] = i.ToString();
                    row["ChargeCode"] = charge.Code;
                    //是否拥有这个权限
                    if(!string.IsNullOrEmpty(chaegeList) && chaegeList.Contains(i.ToString()))
                        row["IsHave"] = "1";
                    else
                        row["IsHave"] = "0";
                    string kaiTongItem = string.Empty;
                    foreach(string item in charge.ChargeList.Split('|'))
                    {
                        switch(item)
                        {
                            case "1": if(string.IsNullOrEmpty(kaiTongItem)) kaiTongItem += "初级混合词训练"; else kaiTongItem += "、初级混合词训练"; break;
                            case "2": if(string.IsNullOrEmpty(kaiTongItem)) kaiTongItem += "数字训练"; else kaiTongItem += "、数字训练"; break;
                            case "3": if(string.IsNullOrEmpty(kaiTongItem)) kaiTongItem += "扑克牌训练"; else kaiTongItem += "、扑克牌训练"; break;
                            case "4": if(string.IsNullOrEmpty(kaiTongItem)) kaiTongItem += "字母训练"; else kaiTongItem += "、字母训练"; break;
                            case "5": if(string.IsNullOrEmpty(kaiTongItem)) kaiTongItem += "高级混合词训练"; else kaiTongItem += "、高级混合词训练"; break;
                        }
                    }
                    row["HaveMsg"] = "支付" + charge.ChargeMoney + "元可开通" + kaiTongItem;
                    dt.Rows.Add(row);
                }
                //}
            }
            CheckMsg = MethodHelper.JsonHelper.DataTableToJson(dt, "CodeNames");
        }
    }
}