using DBUtility;
using MethodHelper;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// PayLogList 的摘要说明
    /// </summary>
    public class PayLogList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "'1'='1'   and t1.IsDeleted=0 and t1.Status=1 ";
            if (!string.IsNullOrEmpty(context.Request["nTitle"]))
            {
                strWhere += " and t2.MID like '%" + context.Request["nTitle"] + "%'";
            }
            if (!string.IsNullOrEmpty(context.Request["nPayBeginTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayBeginTime"]) + " 00:00:00", out time);
                strWhere += " and t1.PayTime>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["nPayEndTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayEndTime"]) + " 23:59:59", out time);
                strWhere += " and t1.PayTime<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            //代理商只能看到自己名下的会员（以后再加）
            //if (TModel.RoleCode == "Admin")
            //{
            //    strWhere += " and (PayID in (select ID FROM Member where Company=" + SessionModel.ID + ") or PayID='" + SessionModel.ID + "')";
            //}
            //if (!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
            //{
            //    strWhere += " and (PayID in (select ID FROM Member where Agent=" + SessionModel.ID + ") or PayID='" + SessionModel.ID + "')";
            //}

            int count;

            string export = context.Request["export"];
            if (export == "1")
            {
                pageIndex = 1;
                pageSize = int.MaxValue;
            }
            string tables = "dbo.TD_PayLog t1 LEFT JOIN dbo.Member t2 ON t1.PayID=t2.ID LEFT JOIN dbo.Member t3 ON t2.MTJ=t3.ID";
            string fields = "t1.Remark,t1.PayID,t1.PayType,t1.PayMoney,t1.PayTime,t2.MID,t2.MName,t3.MID AS MTJMID ,t3.MName AS MTJMName";
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, "PayTime DESC", tables, out count);
            dt.Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["PayTime"]).ToString("yyyy/MM/dd HH:mm");
            }
            //是否导出excel
            if (export == "1")
            {
                Dictionary<string, string> listFields = new Dictionary<string, string>();
                listFields.Add("RowNumber", "序号");
                listFields.Add("MID", "会员账号");
                listFields.Add("MName", "姓名");
                listFields.Add("PayMoney", "支付金额");
                listFields.Add("PayType", "支付方式");
                listFields.Add("CutTime", "支付时间");
                listFields.Add("MTJMID", "推荐人账户");
                listFields.Add("MTJMName", "推荐人姓名");
                listFields.Add("Remark", "交易备注");
                ExportExcel(context, "用户付款列表", dt, listFields);
            }
            else
                context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));
        }
    }
}