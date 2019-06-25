using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Finance
{
    public partial class PrizePool: BasePage
    {
        protected override void SetPowerZone()
        {
            //加载分红金额
            string sql = "SELECT SUM(PayMoney) FROM dbo.TD_PayLog t1 LEFT JOIN dbo.Member t2 ON t1.PayID=t2.ID WHERE t2.RoleCode IN ('1F','2F','3F') AND MONTH(PayTime)=(CASE MONTH(GETDATE()) WHEN 1 THEN 12 ELSE MONTH(GETDATE())-1 END)";
            txt_fmMoney.Value = MethodHelper.ConvertHelper.ToDecimal(CommonBase.GetSingle(sql), 0).ToString();
            //上月成为城市合伙人的数量
            string sqlCount = "SELECT COUNT(1) FROM dbo.TD_PayLog t1 RIGHT JOIN dbo.Member t2 ON t1.PayID=t2.ID WHERE   MONTH(PayTime)=(CASE MONTH(GETDATE()) WHEN 1 THEN 12 ELSE MONTH(GETDATE())-1 END)-1 AND t1.Remark LIKE '%城市合伙人%'";
            txt_count.Value = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(sqlCount), 0).ToString();



        }

        protected override string btnAdd_Click()
        {
            DateTime fhDate = DateTime.Now;
            List<CommonObject> listComm = new List<CommonObject>();
            decimal totaloney = MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_fmMoney"], 0);
            int fhCount = MethodHelper.ConvertHelper.ToInt32(Request.Form["txt_count"], 0);

            string memberList = Request.Form["chkMemberCode"];
            string[] array = memberList.Split(',');
            SHMoneyService.MonthFH(totaloney, array, listComm);
            if(CommonBase.RunListCommit(listComm))
            {
                return "1";
            }
            else
                return "0";
        }

        protected override string btnModify_Click()
        {
            decimal totaloney = MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_allPrizeMoney"], 0);
            string date = Request.Form["txt_lingquDate"];
            bool flag = false;
            //只尝试5次
            for(int i = 0; i <= 5; i++)
            {
                flag = SHMoneyService.sendTotalPrizeMoney(totaloney, date);
                if(flag)
                    break;
            }

            if(flag)
            {
                return "1";
            }
            else
                return "0";
        }

        class PoolMoneyList
        {
            public string PoolId { get; set; }
            public string PoolName { get; set; }
            public decimal PoolTotalMoney { get; set; }
            public int TotalMemberCount { get; set; }
            public decimal MyFHMoney { get; set; }
        }
    }
}