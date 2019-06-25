using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class app_redbag : BasePage
    {
        protected decimal totalMoney = 0, canTXMoney = 0, redBagCount = 0, redBagMoney = 0, noReceiveRedBagCount = 0, noActiveRedBagMoney = 0;
        protected override void SetPowerZone()
        {
            //1)	账户余额xx元（公司奖励和所有下级会员上发的红包）
            //查询红包纪录
            totalMoney += TModel.MSH;
            //List<Model.SH_RedBagDetailLog> listRedBagList= CommonBase.GetList<Model.SH_RedBagDetailLog>("ToUserId='" + TModel.ID + "'");
            //未领的红包金额
            noActiveRedBagMoney = TModel.NoActiveMoney;// listRedBagList.Where(c => c.Status != 4).Sum(c => c.RedBagMoney);
            totalMoney += TModel.NoActiveMoney;// noActiveRedBagMoney;
            //未领取的红包数量
            //noReceiveRedBagCount = listRedBagList.Where(c => c.Status == 1 || c.Status == 2 || c.Status == 3).Count();
            //红包总数量
            //redBagCount = listRedBagList.Count;
            //可提现金额
            canTXMoney = TModel.MSH;
        }
    }
}