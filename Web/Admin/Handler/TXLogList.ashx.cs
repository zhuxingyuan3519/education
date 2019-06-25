using DBUtility;
using MethodHelper;
using Model;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// TXLogList 的摘要说明
    /// </summary>
    public class TXLogList : BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " IsDeleted=0 ";
            if (!string.IsNullOrEmpty(context.Request["nMID"]))
            {
                strWhere += " and MID LIKE '%" + context.Request["nMID"] + "%'";
            }
            if (!string.IsNullOrEmpty(context.Request["nBank"]))
            {
                strWhere += " and TXBank = '" + context.Request["nBank"] + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["nPayBeginTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayBeginTime"]) + " 00:00:00", out time);
                strWhere += " and ApplyTXDate>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["nPayEndTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayEndTime"]) + " 23:59:59", out time);
                strWhere += " and ApplyTXDate<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (TModel.RoleCode == "Admin")
            {
                strWhere += " and Company in (select Id FROM Member where Company=" + SessionModel.ID + ")";
            }
            else if (!TModel.Role.IsAdmin)
            {
                //只查询自己申请的提现记录
                strWhere += " and Company=" + SessionModel.ID;
            }
            //if (!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
            //{
            //    strWhere += " and Company in (select MID FROM Member where Agent=" + SessionModel.ID + ")";
            //}
            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "*", "ApplyTXDate DESC", "TD_TXLog", out count);
            dt.Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["ApplyTXDate"]).ToString("yyyy/MM/dd HH:mm");
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }
    }
}