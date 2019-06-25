using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web.Handler
{
    /// <summary>
    /// GetPageList 的摘要说明
    /// </summary>
    public class GetEveryDayPlanPageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);

            string fromDate = context.Request["fromDate"];
            string toDate = context.Request["toDate"];
            string bank = context.Request["bank"];
            string diffDate = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(toDate))
            {
                diffDate = toDate;
            }

            if (string.IsNullOrEmpty(toDate) && !string.IsNullOrEmpty(fromDate))
            {
                diffDate = "2218-10-31";
            }
            string strWhere = "IsDeleted=0 and (ExpenseMoney>0 or StoreMoney>0)  and DATEDIFF(DAY,PlanDate,'" + diffDate + "')=0  and Company=" + SessionModel.ID;
            if (!string.IsNullOrEmpty(bank))
            {
                strWhere += " and Remark='" + bank + "'";
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                fromDate += " 00:00:00";
                strWhere += " and PlanDate>='" + fromDate + "'";
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                toDate += " 23:59:59";
                strWhere += " and PlanDate<='" + toDate + "'";
            }
            string sql = "SELECT SUM(StoreMoney) TotalStoreMoney,SUM(ExpenseMoney) TotalExpenseMoney,SUM(TakeOffMoney) TotalTakeOffMoney,COUNT(1) TotalCount FROM dbo.CM_PlanDetail where ";
            sql += strWhere;
            DataTable dtSum = CommonBase.GetTable(sql);
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, " '' Bank,'' CardID,PlanHeaderId,PlanDate,StoreMoney,ExpenseMoney,TakeOffMoney,StoreStatus,ExpenseStatus,POSFirstIndustry,Code,Sort", "Sort asc", "CM_PlanDetail");
            //总存入，总消费，银行贡献值，总笔数
            dt.Tables[0].Columns.Add("BillMoney", typeof(string));
            dt.Tables[0].Columns.Add("HasStoredMoney", typeof(string));
            dt.Tables[0].Columns.Add("HasExpenseMoney", typeof(string));
            dt.Tables[0].Columns.Add("LeaveStoreMoney", typeof(string));
            dt.Tables[0].Columns.Add("TotalExpenseMoney", typeof(string));
            dt.Tables[0].Columns.Add("CurrentBalanceMoney", typeof(string)); //可用余额
            dt.Tables[0].Columns.Add("IsAllDo", typeof(string));
            dt.Tables[0].Columns.Add("BankImg", typeof(string));
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                if (dt.Tables[0].Rows[i]["StoreStatus"].ToString() == "2" && dt.Tables[0].Rows[i]["ExpenseStatus"].ToString() == "2")
                {
                    dt.Tables[0].Rows[i]["IsAllDo"] = "1";
                }
                else
                {
                    dt.Tables[0].Rows[i]["IsAllDo"] = "0";
                }
                //计算某张卡某一日之前的消费和存入总额
                int sort = 0;
                int.TryParse(dt.Tables[0].Rows[i]["Sort"].ToString(), out sort);

                string PlanHeaderId = dt.Tables[0].Rows[i]["PlanHeaderId"].ToString();
                CM_PlanHeader header = CommonBase.GetModel<CM_PlanHeader>(PlanHeaderId);
                if (header != null)
                {
                    string ownBank = header.Bank;
                    string cardid = header.CardID;
                    var bankModel = CacheService.BankList.Where(c => c.Code == ownBank).FirstOrDefault();
                    dt.Tables[0].Rows[i]["Bank"] = ownBank;
                    dt.Tables[0].Rows[i]["BankImg"] = "Attachment/" + bankModel.PicUrl;
                    dt.Tables[0].Rows[i]["CardID"] = cardid;
                    dt.Tables[0].Rows[i]["BillMoney"] = header.BillMoney.ToString("F0");
                    //未存入合计
                    string countTotalNoStoreMoney = "SELECT SUM(StoreMoney) FROM dbo.CM_PlanDetail WHERE PlanHeaderId='" + header.Code + "'  AND IsDeleted=0  AND StoreStatus=1 AND StoreMoney>0;";
                    //countTotalNoStoreMoney += "SELECT SUM(StoreMoney) TotalHasStoreMoney,  SUM(ExpenseMoney) TotalHasExpenseMoney  FROM dbo.CM_PlanDetail WHERE PlanHeaderId='" + header.Code + "'  AND IsDeleted=0  AND Sort<=" + sort + ";";
                    //已存入合计
                    countTotalNoStoreMoney += "SELECT SUM(StoreMoney) TotalHasStoreMoney  FROM dbo.CM_PlanDetail WHERE PlanHeaderId='" + header.Code + "'  AND IsDeleted=0  AND StoreStatus=2 ;";
                    //已消费合计
                    countTotalNoStoreMoney += "SELECT SUM(ExpenseMoney) TotalHasExpenseMoney  FROM dbo.CM_PlanDetail WHERE PlanHeaderId='" + header.Code + "'  AND IsDeleted=0  AND ExpenseStatus=2;";
                    //总存入，总消费
                    countTotalNoStoreMoney += "SELECT SUM(StoreMoney) TotalStoreMoney,  SUM(ExpenseMoney) TotalExpenseMoney  FROM dbo.CM_PlanDetail WHERE PlanHeaderId='" + header.Code + "'  AND IsDeleted=0 ;";

                    countTotalNoStoreMoney += "SELECT top 1 BalanceMoney FROM dbo.CM_PlanDetail WHERE PlanHeaderId='" + header.Code + "'  AND IsDeleted=0 ORDER BY Sort;";

                    DataSet objResult = CommonBase.GetDataSet(countTotalNoStoreMoney);
                    if (objResult != null)
                    {
                        //剩余未存入金额
                        dt.Tables[0].Rows[i]["LeaveStoreMoney"] = ConvertHelper.ToDecimal(objResult.Tables[0].Rows[0][0], 0).ToString("F0");
                        //已存入
                        dt.Tables[0].Rows[i]["HasStoredMoney"] = ConvertHelper.ToDecimal(objResult.Tables[1].Rows[0]["TotalHasStoreMoney"], 0).ToString("F0");
                        //已消费
                        dt.Tables[0].Rows[i]["HasExpenseMoney"] = ConvertHelper.ToDecimal(objResult.Tables[2].Rows[0]["TotalHasExpenseMoney"], 0).ToString("F0");
                        //总消费
                        dt.Tables[0].Rows[i]["TotalExpenseMoney"] = ConvertHelper.ToDecimal(objResult.Tables[3].Rows[0]["TotalExpenseMoney"], 0).ToString("F0");
                        //当前余额
                        decimal planBeginMoney = ConvertHelper.ToDecimal(objResult.Tables[4].Rows[0]["BalanceMoney"], 0);
                        decimal currentMoney = planBeginMoney + ConvertHelper.ToDecimal(objResult.Tables[1].Rows[0]["TotalHasStoreMoney"], 0) - ConvertHelper.ToDecimal(objResult.Tables[2].Rows[0]["TotalHasExpenseMoney"], 0);
                        dt.Tables[0].Rows[i]["CurrentBalanceMoney"] = currentMoney.ToString("F0");
                    }

                    string expenseInduty = "消费";
                    if (header.IsPayPlan)
                    {
                        switch (dt.Tables[0].Rows[i]["POSFirstIndustry"].ToString())
                        {
                            case "first":
                            case "second":
                            case "third": expenseInduty = "线下消费"; break;
                            case "other": expenseInduty = "网络消费"; break;
                            default: expenseInduty = "线下消费"; break;
                        }
                    }
                    else
                    {
                        switch (dt.Tables[0].Rows[i]["POSFirstIndustry"].ToString())
                        {
                            case "first": expenseInduty = "民生类"; break;
                            case "other": expenseInduty = "网络消费"; break;
                            case "second": expenseInduty = "一般类"; break;
                            case "third": expenseInduty = "餐娱类"; break;
                        }
                    }
                    dt.Tables[0].Rows[i]["POSFirstIndustry"] = expenseInduty;

                    decimal storeMoney = MethodHelper.ConvertHelper.ToDecimal(dt.Tables[0].Rows[i]["StoreMoney"], 0);
                    //totalStoreMoney += storeMoney;
                    dt.Tables[0].Rows[i]["StoreMoney"] = storeMoney.ToString("F0");
                    decimal expenseMoney = MethodHelper.ConvertHelper.ToDecimal(dt.Tables[0].Rows[i]["ExpenseMoney"], 0);
                    //totalExpenseMoney += expenseMoney;

                    decimal takeOffMoney = MethodHelper.ConvertHelper.ToDecimal(dt.Tables[0].Rows[i]["TakeOffMoney"], 0);
                    //totalCostMoney += takeOffMoney;

                    dt.Tables[0].Rows[i]["ExpenseMoney"] = expenseMoney.ToString("F0");
                    dt.Tables[0].Rows[i]["PlanDate"] = MethodHelper.ConvertHelper.ToDateTime(dt.Tables[0].Rows[i]["PlanDate"], DateTime.Now).ToString("yyyy/MM/dd");
                }
            }
            //下面计算汇总
            dt.Tables[1].Columns.Add("totalStoreMoney");
            dt.Tables[1].Columns.Add("totalExpenseMoney");
            dt.Tables[1].Columns.Add("totalCostMoney");
            dt.Tables[1].Columns.Add("totalExpenseCount");
            DataRow dr = dt.Tables[1].Rows[0];
            string Total = dr["Total"].ToString();
            dt.Tables[1].Clear();
            DataRow drNew = dt.Tables[1].NewRow();
            drNew["Total"] = Total;
            drNew["totalStoreMoney"] = dtSum.Rows[0]["TotalStoreMoney"];
            drNew["totalExpenseMoney"] = dtSum.Rows[0]["TotalExpenseMoney"]; ;
            drNew["totalCostMoney"] = dtSum.Rows[0]["TotalTakeOffMoney"]; ;
            drNew["totalExpenseCount"] = dtSum.Rows[0]["TotalCount"]; ;
            dt.Tables[1].Rows.Add(drNew);

            context.Response.Write(JsonHelper.DataSetToJson(dt, true));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }
    }
}