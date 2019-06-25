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

    public partial class prize_indetail : BasePage
    {
        //protected string GetCanTXMoney = "0", GetTXFloat = "0";
        //protected decimal TotalMoney = 0;
        protected override void SetPowerZone()
        {
            //可提现金额
            //decimal msh = TModel.MSH;
            //TotalMoney = msh;
            //List<Model.SH_RedBagDetailLog> listRedBagList = CommonBase.GetList<Model.SH_RedBagDetailLog>("ToUserId='" + TModel.ID + "'");
            ////未领的红包金额
            //decimal noActiveRedBagMoney = listRedBagList.Where(c => c.Status != 4).Sum(c => c.RedBagMoney);
            //TotalMoney += noActiveRedBagMoney;
            //if (msh > CacheService.GlobleConfig.MinTXMoney)
            //{
            //    decimal b = Math.Floor(msh / CacheService.GlobleConfig.BaseJifen);
            //    GetCanTXMoney = (msh - CacheService.GlobleConfig.MinTXMoney).ToString();
            //    GetTXFloat = CacheService.GlobleConfig.TXFloat + "元/笔";
            //}
        }
    }
}