using DBUtility;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class tx_apply2 : BasePage
    {
        protected string GetCanTXMoney = "0", GetTXFloat = "0";
        protected decimal TotalMoney = 0;
        protected override void SetPowerZone()
        {
            //可提现金额
            decimal msh = TModel.MSH;
            TotalMoney = msh+TModel.NoActiveMoney;
            //List<Model.SH_RedBagDetailLog> listRedBagList = CommonBase.GetList<Model.SH_RedBagDetailLog>("ToUserId='" + TModel.ID + "'");
            ////未领的红包金额
            //decimal noActiveRedBagMoney = listRedBagList.Where(c => c.Status != 4).Sum(c => c.RedBagMoney);
            //TotalMoney += noActiveRedBagMoney;
            if (msh > CacheService.GlobleConfig.MinTXMoney)
            {
                decimal b = Math.Floor(msh / CacheService.GlobleConfig.BaseJifen);
                GetCanTXMoney = (msh - CacheService.GlobleConfig.MinTXMoney).ToString();
                GetTXFloat = CacheService.GlobleConfig.TXFloat + "元/笔";
            }
            ////把微信和支付宝收款二维码加载上
            //uploadImg.Value = TModel.AliPay;
            //imgappendimg.Src = uploadImg.Value;

            //uploadImgWeixin.Value = TModel.WeixinPay;
            //imgappendimgWeixin.Src = uploadImgWeixin.Value;

            ddl_Bank.DataSource = CacheService.BankList;
            ddl_Bank.DataTextField = "Remark";
            ddl_Bank.DataValueField = "Code";
            ddl_Bank.DataBind();
            ddl_Bank.Items.Insert(0, new ListItem("选择提现银行", ""));
            //加载历史收款信息
            repBankList.DataSource = CommonBase.GetList<Model.SH_MemberBank>("MID='" + TModel.ID + "'");
            repBankList.DataBind();
        }
    }
}