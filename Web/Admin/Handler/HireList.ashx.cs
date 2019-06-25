using DBUtility;
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
    /// FHLogList 的摘要说明
    /// </summary>
    public class HireList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "1=1 ";
            string hireType = context.Request["hiretype"];
            if (!string.IsNullOrEmpty(context.Request["txtKey"]))
            {
                strWhere += " and t1.RoleCode='" + context.Request["txtKey"] + "'";
            }
            if (!string.IsNullOrEmpty(hireType))
            {
                strWhere += " and HireType=" + hireType;
            }
            if (!string.IsNullOrEmpty(context.Request["nMID"]))
            {
                strWhere += " and UserCode like '%" + context.Request["nMID"] + "%'";
            }
            int count;
            string fields = "t1.*";
            System.Data.DataTable dtTbl = new DataTable();
            if (hireType == "0")
                dtTbl = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, "t1.RoleCode asc", "dbo.SH_HirePurchase t1", out count);
            else
                dtTbl = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, "t1.CreatedTime asc", "dbo.SH_HirePurchase t1 inner join Member t2 on  t1.UserId=t2.ID", out count);
            dtTbl.Columns.Add("CutTime", typeof(string));
            dtTbl.Columns.Add("IsAllPay", typeof(string));
            dtTbl.Columns.Add("LeaveHireCount", typeof(string));
            for (int i = 0; i < dtTbl.Rows.Count; i++)
            {
                DataRow dr = dtTbl.Rows[i];
                dr["CutTime"] = Convert.ToDateTime(dr["CreatedTime"]).ToString("yyyy/MM/dd HH:mm");
                dr["RoleCode"] = CacheService.RoleList.FirstOrDefault(c => c.Code == dr["RoleCode"].ToString()).Name;
                if (hireType == "1")
                {
                    //查看是否结清
                    var listDetail = CommonBase.GetList<SH_HirePurchaseDetail>("HirePurchaseId='" + dr["Id"].ToString() + "'");
                    var noPay = listDetail.Where(c => c.PayStatus == 0);
                    if (noPay != null && noPay.Count() > 0)
                    {
                        dr["IsAllPay"] = "未结清";
                    }
                    else
                    {
                        dr["IsAllPay"] = "已结清";
                    }
                    //剩余期数
                    dr["LeaveHireCount"] = noPay.Count();
                }
            }
            context.Response.Write(MethodHelper.JsonHelper.GetAdminDataTableToJson(dtTbl, count));

        }
    }
}