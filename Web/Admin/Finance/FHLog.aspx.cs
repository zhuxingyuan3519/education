using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Finance
{
    public partial class FHLog : BasePage
    {
        protected override string btnOther_Click()
        {
            string mid = Request["nMID"];
            string type = Request["type"];
            string nPayBeginTime = Request["nPayBeginTime"];
            string nPayEndTime = Request["nPayEndTime"];

            string strWhere = "'1'='1' and IsDeleted=0 ";

            if (!string.IsNullOrEmpty(nPayBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nPayBeginTime) + " 00:00:00", out time);
                strWhere += " and FHDate>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(nPayEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nPayEndTime) + " 23:59:59", out time);
                strWhere += " and FHDate<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(type)) //是否是查询奖金池分红
            {
                strWhere += " and  charindex('P-',FHType)=1";
                if (TModel.RoleCode != "Manage" && TModel.RoleCode != "Admin")
                    strWhere += " and MID=" + TModel.ID;
            }
            if (!string.IsNullOrEmpty(mid)) //会员昵称
            {
                strWhere += " and FHMCode like '%" + mid + "%'";
            }

            if (TModel.RoleCode == "Admin")
            {
                strWhere += " and (MID in (select ID FROM Member where Company=" + TModel.ID + ") or MID='" + TModel.ID + "')";
            }
            if (!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
            {
                strWhere += " and (MID in (select ID FROM Member where Agent=" + TModel.ID + ") or MID='" + TModel.ID + "')";
            }

            string sql = "SELECT ISNULL(SUM(FHMoney),0) FROM dbo.TD_FHLog where " + strWhere;
            object obj = CommonBase.GetSingle(sql);
            if (obj != null)
            {
                return obj.ToString();
            }
            return "0";
        }
    }
}