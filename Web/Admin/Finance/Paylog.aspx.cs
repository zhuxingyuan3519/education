using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Finance
{
    public partial class Paylog : BasePage
    {
        protected override string btnOther_Click()
        {
            string nTitle = Request["nTitle"];
            string nPayBeginTime = Request["nPayBeginTime"];
            string nPayEndTime = Request["nPayEndTime"];

            string strWhere = "'1'='1'   and t1.IsDeleted=0 ";
            if (!string.IsNullOrEmpty(nTitle))
            {
                strWhere += " and t2.MID like '%" + nTitle + "%'";
            }
            if (!string.IsNullOrEmpty(nPayBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nPayBeginTime) + " 00:00:00", out time);
                strWhere += " and t1.PayTime>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(nPayEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nPayEndTime) + " 23:59:59", out time);
                strWhere += " and t1.PayTime<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            ////代理商只能看到自己名下的会员（以后再加）
            //if (TModel.RoleCode == "Admin")
            //{
            //    strWhere += " and (PayID in (select ID FROM Member where Company=" + TModel.ID + ") or PayID='" + TModel.ID + "')";
            //}
            //if (!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
            //{
            //    strWhere += " and (PayID in (select ID FROM Member where Agent=" + TModel.ID + ") or PayID='" + TModel.ID + "')";
            //}
            string sql = "SELECT ISNULL(SUM(t1.PayMoney),0) FROM dbo.TD_PayLog t1 LEFT JOIN dbo.Member t2 ON t1.PayID=t2.ID LEFT JOIN dbo.Member t3 ON t2.MTJ=t3.ID where " + strWhere;
            object obj = CommonBase.GetSingle(sql);
            if (obj != null)
            {
                return obj.ToString();
            }
            return "0";
        }
    }
}