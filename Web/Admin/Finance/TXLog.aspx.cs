using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Finance
{
    public partial class TXLog : BasePage
    {
        protected override string btnOther_Click()
        {

            string mid = Request["nMID"];
            string nBank = Request["nBank"];
            string nPayBeginTime = Request["nPayBeginTime"];
            string nPayEndTime = Request["nPayEndTime"];

            string strWhere = " IsDeleted=0 ";
            if (!string.IsNullOrEmpty(mid))
            {
                strWhere += " and MID LIKE '%" + mid + "%'";
            }
            if (!string.IsNullOrEmpty(nBank))
            {
                strWhere += " and TXBank = '" + nBank + "'";
            }
            if (!string.IsNullOrEmpty(nPayBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nPayBeginTime) + " 00:00:00", out time);
                strWhere += " and ApplyTXDate>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(nPayEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nPayEndTime) + " 23:59:59", out time);
                strWhere += " and ApplyTXDate<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (TModel.RoleCode == "Admin")
            {
                strWhere += " and Company in (select Id FROM Member where Company=" + TModel.ID + ")";
            }
            else if (!TModel.Role.IsAdmin)
            {
                //只查询自己申请的提现记录
                strWhere += " and Company=" + TModel.ID;
            }
            //if (!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
            //{
            //    strWhere += " and Company in (select MID FROM Member where Agent=" + SessionModel.ID + ")";
            //}

            string sql = "SELECT ISNULL(SUM(TXMoney),0),ISNULL(SUM(FeeMoney),0),ISNULL(SUM(RealMoney),0) FROM dbo.TD_TXLog  where " + strWhere;
            DataTable obj = CommonBase.GetTable(sql);
            if (obj != null && obj.Rows.Count > 0)
            {
                return obj.Rows[0][0].ToString() + "*" + obj.Rows[0][1].ToString() + "*" + obj.Rows[0][2].ToString();
            }
            return "0*0*";
        }
    }
}