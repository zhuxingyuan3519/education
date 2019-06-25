using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace Service
{
    public class PlanService
    {
        private static string GetDate(int year, int month, int day)
        {
            return year.ToString() + "-" + month.ToString() + "-" + day.ToString();
        }
        public static List<string> GetBillDateAndPlanDate(int billDay, int replayDay)
        {
            string billDate = DateTime.Now.ToString("yyyy-MM-dd");
            string replayDate = DateTime.Now.ToString("yyyy-MM-dd");
            string planBeginbillDate = DateTime.Now.ToString("yyyy-MM-dd");
            string planEndDate = DateTime.Now.ToString("yyyy-MM-dd");
            int year = DateTime.Now.Year, month = DateTime.Now.Month, day = DateTime.Now.Day;
            List<string> array = new List<string>();
            int today = DateTime.Now.Day;
            #region 如果账单日<还款日
            if (billDay < replayDay)
            {
                if (today >= billDay && today < replayDay)
                {
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    billDate = GetDate(year, month, billDay);
                    replayDate = GetDate(year, month, replayDay);
                    planBeginbillDate = GetDate(year, month, today);
                    planEndDate = GetDate(year, month, replayDay);
                }
                else if (today < billDay)
                {
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    billDate = GetDate(year, month, billDay);
                    replayDate = GetDate(year, month, replayDay);
                    planBeginbillDate = GetDate(year, month, billDay);
                    planEndDate = GetDate(year, month, replayDay);
                }
                else if (today >= replayDay)
                {
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    if (month == 12)
                    {
                        year += 1;
                        month = 1;
                    }
                    billDate = GetDate(year, month, billDay);
                    replayDate = GetDate(year, month, replayDay);
                    planBeginbillDate = GetDate(year, month, billDay);
                    planEndDate = GetDate(year, month, replayDay);
                }
            }
            #endregion
            #region 如果账单日>还款日
            if (billDay > replayDay)
            {
                int billMonth = month;
                if (today >= replayDay && today < billDay) //如果今日在账单日和还款日之间
                {
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    int replayMonth = month, replayYear = year;
                    if (month == 12)
                    {
                        replayMonth = 1;
                        replayYear += 1;
                    }
                    else
                    {
                        replayMonth += 1;
                    }
                    billDate = GetDate(year, month, billDay);
                    replayDate = GetDate(replayYear, replayMonth, replayDay);
                    planBeginbillDate = GetDate(year, month, billDay);
                    planEndDate = GetDate(replayYear, replayMonth, replayDay);
                }
                //else if (today >= replayDay)
                //{
                //    year = DateTime.Now.Year;
                //    month = DateTime.Now.Month;
                //    billDate = GetDate(year, month, billDay);
                //    replayDate = GetDate(year, month, replayDay);
                //    planBeginbillDate = GetDate(year, month, billDay);
                //    planEndDate = GetDate(year, month, replayDay);
                //}
                else if (today <= billDay && today > replayDay)//如果今日小于账单日
                {
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    int replayMonth = month, replayYear = year;
                    if (month == 12)
                    {
                        replayYear += 1;
                        replayMonth = 1;
                    }
                    else
                    {
                        replayMonth += 1;
                    }
                    billDate = GetDate(year, month, billDay);
                    replayDate = GetDate(replayYear, replayMonth, replayDay);
                    planBeginbillDate = GetDate(year, month, billDay);
                    planEndDate = GetDate(replayYear, replayMonth, replayDay);
                }
                else if (today >= billDay && today <= 31)//如果今日小于还款日，大于账单日
                {
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    int replayMonth = month, replayYear = year;
                    if (month == 12)
                    {
                        replayYear += 1;
                        replayMonth = 1;
                    }
                    else
                    {
                        replayMonth += 1;
                    }
                    billDate = GetDate(year, month, billDay);
                    replayDate = GetDate(replayYear, replayMonth, replayDay);
                    planBeginbillDate = GetDate(year, month, today);
                    planEndDate = GetDate(replayYear, replayMonth, replayDay);
                }
                else if (today > 0 && today <= replayDay)//
                {
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    int billMonth2 = month, billYear = year;
                    if (month == 1)
                    {
                        billYear -= 1;
                        billMonth2 = 12;
                    }
                    else
                    {
                        billMonth2 -= 1;
                    }
                    billDate = GetDate(billYear, billMonth2, billDay);
                    replayDate = GetDate(year, month, replayDay);
                    planBeginbillDate = GetDate(year, month, today);
                    planEndDate = GetDate(year, month, replayDay);
                }
            }
            #endregion
            array.Add(billDate);
            array.Add(replayDate);
            array.Add(planBeginbillDate);
            array.Add(planEndDate);
            return array;
        }


        /// <summary>
        /// 规划，返回规划表详细
        /// </summary>
        /// <returns></returns>
        public static List<CM_PlanDetail> GetPlanDetail(CM_PlanHeader planHeader)
        {
            List<CM_PlanRateConfig> rateList = planHeader.PlanRateConfigList;
            //账单金额，每个大的行业要刷的金额
            foreach (CM_PlanRateConfig prc in rateList)
            {
                prc.ExpenseMoney = prc.RatePercent * planHeader.BillMoney;
            }



            return null;
        }
        /// <summary>
        /// 规划，生成规划表主表
        /// </summary>
        /// <returns></returns>
        public static CM_PlanHeader GetPlanHeader(string archiveId, string billMoney, string billDate, string repayDate, string useModel)
        {
            //根据唯一的档案号取到该卡片信息
            CM_Archives archive = CommonBase.GetModel<CM_Archives>(archiveId);
            if (archive != null)
            {
                Model.CM_PlanHeader model = null;
                model = new Model.CM_PlanHeader();
                model.Code = CommonHelper.GetGuid;
                model.IsDeleted = false;
                model.Status = 1;
                model.CreatedBy = "system";
                model.CreatedTime = DateTime.Now;
                model.Company = 0;
                DateTime billdate = DateTime.Now;
                DateTime.TryParse(billDate, out billdate);
                model.Years = billdate.Year;
                model.Months = billdate.Month;
                model.ArchiveId = archive.ArchiveId;
                model.Name = archive.Name;
                model.Bank = archive.Bank;
                model.CardID = archive.CardID;
                model.FixedQuota = archive.FixedQuota;
                //计算账单日
                var thisYear = DateTime.Now.Year;
                var thisBillDateYear = thisYear;
                var thisBillDateMonth = model.Months;
                var thisPayDateYear = thisYear;
                var thisPayDateMonth = model.Months;

                if (archive.RepayDate <= archive.BillDate)
                {
                    if (model.Months == 12)
                        thisPayDateYear = model.Years + 1;
                    thisPayDateMonth = model.Months + 1;
                    if (thisPayDateMonth > 12)
                        thisPayDateMonth = 1;
                }
                var endYear = model.Years;
                if (model.Months == 12)
                    endYear = model.Years + 1;
                model.BillDate = DateTime.Parse(model.Years + "-" + thisBillDateMonth + "-" + billDate);
                billDate = (archive.BillDate + 1).ToString();
                model.PlanBeginDate = DateTime.Parse(model.Years + "-" + thisBillDateMonth + "-" + billDate);
                model.PlanEndDate = DateTime.Parse(endYear + "-" + thisBillDateMonth + "-" + model.RepayDate);
                model.RepayDate = DateTime.Parse(endYear + "-" + thisBillDateMonth + "-" + model.RepayDate);
                model.BillMoney = decimal.Parse(billMoney);
                model.CountModel = 1;
                CM_Template tempModel = CommonBase.GetList<CM_Template>("Bank='" + model.Bank + "' and MinMoney<=" + model.FixedQuota + " and MaxMoney>=" + model.FixedQuota).FirstOrDefault();
                if (tempModel != null)
                {
                    model.TempCode = tempModel.Code;
                    model.PlanRateConfigList = CommonBase.GetList<CM_PlanRateConfig>("PlanHeaderId='" + tempModel.Code + "'");
                    if (useModel == "1") //使用提额规划模板
                    {
                        //根据固定额度选择出需要使用的模板
                        model.LeavePencent = tempModel.LeavePencent;
                        model.LeaveMoney = model.FixedQuota * tempModel.LeavePencent;
                        model.ExpenseCount = tempModel.CostCount;
                    }
                    else //没有还款规划模板 ，默认只剩5%的额度
                    {
                        model.LeavePencent = 0.05M;
                        model.LeaveMoney = model.FixedQuota * model.LeavePencent;
                        model.ExpenseCount = tempModel.CostCount;
                    }
                }
                return model;
            }
            else
                return null;
        }
        /// <summary>
        /// 调用存储过程重新计算余额
        /// </summary>
        /// <param name="planheaderId"></param>
        public static void RecountBalanceMoney(string planheaderId)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@PlanHeaderCode", SqlDbType.VarChar, 50)
                 };
            parameters[0].Value = planheaderId;
            CommonBase.RunProcedureNoReturn("Proc_RecountBanlnceMoney", parameters);
        }
        /// <summary>
        /// 删除某次的规划，该次的规划任务就分配到这次之后的规划中
        /// </summary>
        /// <param name="code"></param>
        /// <param name="comm"></param>
        public static void DeletePlanDetail(List<CM_PlanDetail> ListModel, List<CommonObject> comm, CM_PlanHeader header)
        {
            //查询到这条规划信息
            if (ListModel.Count > 0)
            {
                decimal totalStoreMoney = 0;// ListModel.Sum(c => c.StoreMoney);
                decimal totalExpenseMoney = 0;// ListModel.Sum(c => c.ExpenseMoney);
                List<CommonObject> tempcomm = new List<CommonObject>();
                decimal hasStoreMoney = 0;
                foreach (CM_PlanDetail plandetailModel in ListModel)
                {
                    if (plandetailModel.StoreStatus == 2 && plandetailModel.ExpenseStatus == 1)
                    {
                        //已经存入，但是没刷出，这时候不要删除，把刷出重置为0
                        totalExpenseMoney += plandetailModel.ExpenseMoney;
                        plandetailModel.ExpenseMoney = 0;
                        plandetailModel.ExpenseStatus = 2;
                        plandetailModel.TakeOffMoney = 0;
                        plandetailModel.POSNum = "";
                        hasStoreMoney += plandetailModel.StoreMoney;
                        CommonBase.Update<CM_PlanDetail>(plandetailModel, new string[] { "ExpenseMoney", "ExpenseStatus", "POSNum", "TakeOffMoney" }, tempcomm);
                    }
                    else if (plandetailModel.StoreStatus == 1 && plandetailModel.ExpenseStatus == 2)
                    {
                        //已经刷出，但是没存入，这时候不要删除，把存入重置为0
                        totalStoreMoney += plandetailModel.StoreMoney;
                        plandetailModel.StoreMoney = 0;
                        plandetailModel.StoreStatus = 2;
                        CommonBase.Update<CM_PlanDetail>(plandetailModel, new string[] { "StoreMoney", "StoreStatus" }, tempcomm);
                    }
                    else
                    {
                        hasStoreMoney += plandetailModel.StoreMoney;
                        totalExpenseMoney += plandetailModel.ExpenseMoney;
                        totalStoreMoney += plandetailModel.StoreMoney;

                        plandetailModel.TakeOffMoney = 0;
                        plandetailModel.POSNum = "";
                        plandetailModel.StoreMoney = 0;
                        plandetailModel.StoreStatus = 2;
                        plandetailModel.ExpenseMoney = 0;
                        plandetailModel.ExpenseStatus = 2;
                        CommonBase.Update<CM_PlanDetail>(plandetailModel, tempcomm);
                    }
                }

                //取到最后一条规划详细
                CM_PlanDetail model = ListModel.OrderByDescending(c => c.Sort).FirstOrDefault();

                //该条规划之后的规划信息
                List<CM_PlanDetail> planList = CommonBase.GetList<CM_PlanDetail>("PlanHeaderId='" + model.PlanHeaderId + "' AND Sort>" + model.Sort + " and DATEDIFF(dd,PlanDate,'"+header.PlanEndDate+"')>=0 order by Sort");
                if (planList.Count > 0)
                {

                    ////把需要删除的规划平均分配到规划还款日之前
                    ////把日期distinct一下
                    List<DateAddMoreMoney> existDate = new List<DateAddMoreMoney>();
                    foreach (CM_PlanDetail cpd in planList)
                    {
                        if (existDate.Find(c => c.Date == cpd.PlanDate.ToString("yyyy-MM-dd")) == null)
                        {
                            DateAddMoreMoney dam = new DateAddMoreMoney();
                            dam.Date = cpd.PlanDate.ToString("yyyy-MM-dd");
                            dam.AddExpenseMoney = 0;
                            dam.AddStoreMoney = 0;
                            dam.PosRate = cpd.Rate;
                            dam.AddTakeOffMoney = 0;
                            existDate.Add(dam);
                        }
                    }
                    if (existDate.Count > 0)
                    {
                        CycleDateStoreMoney(existDate, totalStoreMoney);
                        //按日期分配
                        List<CM_PlanDetail> listForUpdate = new List<CM_PlanDetail>();
                        foreach (DateAddMoreMoney add in existDate)
                        {
                            //查询到当天的
                            CM_PlanDetail plan = planList.Where(c => c.PlanDate.ToString("yyyy-MM-dd") == add.Date && c.Code != model.Code).FirstOrDefault();
                            if (plan != null)
                            {
                                plan.StoreMoney += add.AddStoreMoney;
                                listForUpdate.Add(plan);
                            }
                        }
                        if (header != null && listForUpdate.Count > 0)
                        {
                            //更新规划主表中的Remark字段，标识是不是规划表重新修改了
                            header.Remark = "True";
                            CommonBase.Update<CM_PlanHeader>(header, new string[] { "Remark" }, tempcomm);
                        }
                        //组建更新操作
                        foreach (CM_PlanDetail cm in listForUpdate)
                        {
                            CommonBase.Update<CM_PlanDetail>(cm, tempcomm);
                        }
                    }
                    if (CommonBase.RunListCommit(tempcomm))
                    {
                        //重新计算余额
                        RecountBalanceMoney(header.Code);
                    }

                    #region 重置刷出金额

                    decimal lastBalance = 0;
                    CM_PlanDetail last = CommonBase.GetList<CM_PlanDetail>("IsDeleted=0 and PlanHeaderId='" + header.Code + "' and Sort<=" + model.Sort).OrderByDescending(c => c.Sort).FirstOrDefault();
                    if (last != null)
                        lastBalance = last.BalanceMoney;
                    totalStoreMoney = planList.Sum(c => c.StoreMoney);
                    totalExpenseMoney = planList.Sum(c => c.ExpenseMoney);
                    //刷出金额=存入金额+lastBalance-卡内应剩余金额
                    totalExpenseMoney = totalStoreMoney + lastBalance - header.LeaveMoney;
                    {
                        //平均每天要刷出的金额
                        decimal everydatPerExpenseMoney = totalExpenseMoney / planList.Count;
                        //设定一个系数范围：50%-150%
                        decimal minExpense = 0;
                        decimal maxExpense = 0;
                        for (int i = 0; i < planList.Count; i++)
                        {
                            CM_PlanDetail toUpdateModel = planList[i];
                            //根据余额，获取一个随机的刷出金额
                            decimal bananceFor0ToExpense = toUpdateModel.StoreMoney + lastBalance;
                            minExpense = bananceFor0ToExpense * 0.2M;
                            maxExpense = bananceFor0ToExpense * 0.95M;
                            decimal expense = 0;
                            if (maxExpense >= minExpense)
                                expense = GetExpenseMoney(minExpense, maxExpense);
                            decimal balanceMoney = toUpdateModel.StoreMoney + lastBalance - expense;
                            if (i == planList.Count - 1) //最后一天的规划，余额应该等于规划初始的余额
                            {
                                if (balanceMoney > header.LeaveMoney)
                                {
                                    //多余的刷出金额
                                    decimal moreExpense = balanceMoney - header.LeaveMoney;
                                    expense += moreExpense;
                                    balanceMoney = toUpdateModel.StoreMoney + lastBalance - expense;
                                }
                                else
                                {
                                    //需要扣除的刷出金额
                                    //多余的刷出金额
                                    decimal moreExpense = header.LeaveMoney - balanceMoney;
                                    if (expense > moreExpense)
                                        expense -= moreExpense;
                                    balanceMoney = toUpdateModel.StoreMoney + lastBalance - expense;
                                }
                            }
                            toUpdateModel.ExpenseMoney = expense;
                            toUpdateModel.TakeOffMoney = toUpdateModel.ExpenseMoney * toUpdateModel.Rate; ;
                            toUpdateModel.BalanceMoney = balanceMoney;
                            lastBalance = balanceMoney;
                            CommonBase.Update<CM_PlanDetail>(toUpdateModel, comm);
                        }
                    #endregion


                        //List<CM_PlanDetail> listForUpdate = RePlan(header, totalStoreMoney, totalExpenseMoney, planList, false, lastBalance);
                        //for (int i = 0; i < planList.Count; i++)
                        //{
                        //    CM_PlanDetail toDeteleModel = planList[i];
                        //    CM_PlanDetail toCopy = listForUpdate[i];
                        //    toCopy.Sort = toDeteleModel.Sort;
                        //    toCopy.Company = toDeteleModel.Company;
                        //    toCopy.CreatedBy = toDeteleModel.CreatedBy;
                        //    CommonBase.Insert<CM_PlanDetail>(toCopy, comm);
                        //    CommonBase.Delete<CM_PlanDetail>(toDeteleModel, comm);
                        //}
                    }
                }
            }
        }
        protected static decimal GetExpenseMoney(decimal minExpenseMoney, decimal maxExpenseMoney)
        {
            //随机取出一个刷出金额
            int oneExpenseMoney = new RandomHelper().GetRandomInt((int)minExpenseMoney, (int)maxExpenseMoney);
            return oneExpenseMoney;
        }

        protected static decimal GetExpenseMoney(decimal perMoney, decimal minExpenseMoney, decimal maxExpenseMoney, decimal storeMoney, decimal lastBalanceMoney)
        {
            //随机取出一个刷出金额
            int oneExpenseMoney = new RandomHelper().GetRandomInt((int)minExpenseMoney, (int)maxExpenseMoney);
            decimal balanceMoney = lastBalanceMoney + storeMoney - oneExpenseMoney;
            if (balanceMoney > 0)
            {
            }
            if (balanceMoney < 0)
            {
                GetExpenseMoney(perMoney, minExpenseMoney, maxExpenseMoney, storeMoney, lastBalanceMoney);
            }
            //tag1:
            return oneExpenseMoney;
        }

        /// <summary>
        /// 对于删除的规划重新分配后期的的存入金额
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="totalStoreMoney"></param>
        protected static void CycleDateStoreMoney(List<DateAddMoreMoney> dtList, decimal totalStoreMoney)
        {
            if (totalStoreMoney > 0)
            {
                foreach (DateAddMoreMoney dt in dtList)
                {
                    dt.AddStoreMoney += 100;
                    totalStoreMoney -= 100;
                    if (totalStoreMoney <= 0)
                        break;
                }
                CycleDateStoreMoney(dtList, totalStoreMoney);
            }
        }
        /// <summary>
        /// 对于删除的规划，根据存入金额的分配，重新分配刷出金额
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="totalExpenseMoney"></param>
        protected static void CycleDateExpenseMoney(List<DateAddMoreMoney> dtList, decimal totalExpenseMoney)
        {
            if (totalExpenseMoney > 0)
            {
                int preExpenseMoney = (int)(totalExpenseMoney / dtList.Count); //每天的存入金额
                if (preExpenseMoney == 0 && totalExpenseMoney > 0)
                    preExpenseMoney = (int)totalExpenseMoney;
                foreach (DateAddMoreMoney dt in dtList)
                {
                    //看看今天存入金额多出了多少，刷出金额也要多出多少
                    decimal todayAddStoreMoney = dt.AddStoreMoney;
                    if (todayAddStoreMoney > 0)  //如果分配的存入金额大于0，也就是说删除的那一个规划详细信息没有存入，只有刷出
                    {
                        if (totalExpenseMoney >= todayAddStoreMoney)
                        {
                            dt.AddExpenseMoney += todayAddStoreMoney;
                            dt.AddTakeOffMoney += (todayAddStoreMoney * dt.PosRate);
                            totalExpenseMoney -= todayAddStoreMoney;
                        }
                        else //如果总刷出金额小于今天要存入的金额
                        {
                            dt.AddExpenseMoney += totalExpenseMoney;
                            dt.AddTakeOffMoney += (totalExpenseMoney * dt.PosRate);
                            totalExpenseMoney = 0;
                        }
                        if (totalExpenseMoney <= 0)
                            break;
                    }
                    else
                    {
                        dt.AddExpenseMoney += preExpenseMoney;
                        dt.AddTakeOffMoney += (preExpenseMoney * dt.PosRate);
                        totalExpenseMoney -= preExpenseMoney;
                        if (totalExpenseMoney <= 0)
                            break;
                    }
                }
                CycleDateExpenseMoney(dtList, totalExpenseMoney);
            }
        }



        public class DateAddMoreMoney
        {
            public string Date { get; set; }
            public decimal AddStoreMoney { get; set; }
            public decimal AddExpenseMoney { get; set; }
            public decimal PosRate { get; set; }
            public decimal AddTakeOffMoney { get; set; }
        }

        /// <summary>
        /// 删除昨天之前的规划信息
        /// </summary>
        public static void DeleteYesterdayPlanDetail()
        {
            //App.config中配置的的数据库链接字符串key值
            string ConnectionStringKeys = MethodHelper.ConfigHelper.GetAppSettings("ConnectionStringKeys");
            string[] connectionArray = ConnectionStringKeys.Split(',');
            foreach (string configName in connectionArray)
            {
                if (string.IsNullOrEmpty(configName))
                    continue;
                DbHelperSQL.connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
                if (configName == "KgjPCVersion")
                {
                    //卡管家PC版的临时额度到期自动去掉减少余额
                    CheckEveryDayPlanScript("TempQuataEndChange");
                    //continue;
                }
                if (configName != "KgjPCVersion")
                {
                    //执行每日需要执行的计划事件
                    CheckEveryDayPlanScript("CheckEveryDayRemind");
                }
                //取到昨天还在规划中的规划主表
                List<CM_PlanHeader> listPlanHeader = CommonBase.GetList<CM_PlanHeader>("IsDeleted=0 AND DATEDIFF(dd,PlanEndDate,DATEADD(DAY,-1,GETDATE()))<=0");
                //一个一个的循环执行
                foreach (CM_PlanHeader header in listPlanHeader)
                {
                    List<CommonObject> listComm = new List<CommonObject>();
                    //取到这个规划中的规划详细：昨天之前的，包含昨天的
                    List<CM_PlanDetail> listPlanDetail = CommonBase.GetList<CM_PlanDetail>("IsDeleted=0  AND PlanHeaderId='" + header.Code + "' AND DATEDIFF(dd,PlanDate,DATEADD(DAY,-1,GETDATE()))>=0 AND (StoreStatus=1 OR ExpenseStatus=1)");
                    PlanService.DeletePlanDetail(listPlanDetail, listComm, header);
                    try
                    {
                        if (CommonBase.RunListCommit(listComm))
                        {
                            //重新计算余额
                            ReCountPlanBananceMoney(header, configName);
                            MethodHelper.LogHelper.WriteTextLog("数据库配置key键" + configName + "中执行；DeleteYesterdayPlanDetail()", "规划主表：" + header.Code + "；成功删除昨天之前的的规划详细", DateTime.Now);
                        }
                    }
                    catch (Exception ex)
                    {
                        MethodHelper.LogHelper.WriteTextLog("数据库配置key键" + configName + "中执行；DeleteYesterdayPlanDetail()", "规划主表：" + header.Code + "；删除昨天之前的的规划详细时失败，错误信息：" + ex.ToString(), DateTime.Now);
                    }
                }
            }
        }



        public static void ReCountPlanBananceMoney(CM_PlanHeader planHeader, string connKey)
        {
            try
            {
                SqlParameter[] parameters = {
                                   new SqlParameter("@PlanHeaderCode", SqlDbType.VarChar,50)
                         };
                parameters[0].Value = planHeader.Code;
                //得到地址信息表中的区域
                CommonBase.GetProduceTable(parameters, "Proc_RecountBanlnceMoney");
            }
            catch (Exception ex)
            {
                MethodHelper.LogHelper.WriteTextLog("数据库配置key键" + connKey + "中执行；Proc_RecountBanlnceMoney", "规划主表：" + planHeader.Code + "；重新计算余额出错", DateTime.Now);
            }
        }

        /// <summary>
        /// 处理重新计算余额之后余额有负数的情况
        /// </summary>
        /// <param name="planHeader"></param>
        public static void DealLess0PlanDetail(CM_PlanHeader planHeader)
        {
            List<CM_PlanDetail> listPlanDetail = CommonBase.GetList<CM_PlanDetail>("IsDeleted=0  AND PlanHeaderId='" + planHeader.Code + "' order by Sort");
            foreach (CM_PlanDetail cpd in listPlanDetail)
            {
                decimal balanceMoney = cpd.BalanceMoney;
                if (balanceMoney < 0)
                {
                    //如果余额为负数，就把刷出金额减少为相应的金额
                    decimal expenseMoney = cpd.ExpenseMoney;
                    //expenseMoney=expenseMoney-
                }
            }
        }


        protected static List<Model.CM_PlanRateConfig> GetPlanRateConfigList(Model.CM_PlanHeader planHeader)
        {
            //得到模板表
            List<Model.CM_PlanRateConfig> listRate = CommonBase.GetList<CM_PlanRateConfig>("PlanHeaderId='" + planHeader.TempCode + "' and RatePercent>0 and Remark<>''");
            return listRate;
        }

        /// <summary>
        /// 重新规划剩余工作
        /// </summary>
        /// <param name="planHeader"></param>
        public static List<CM_PlanDetail> RePlan(CM_PlanHeader planHeader, decimal storeMoney, decimal expenseMoney, List<CM_PlanDetail> toRePlanDetailList, bool isUpdate, decimal lastBalanceMoney)
        {
            planHeader.PlanRateConfigList = GetPlanRateConfigList(planHeader);
            Model.CM_Template tempModel = CommonBase.GetList<CM_Template>("IsDeleted=0 and Bank='" + planHeader.Bank + "' and MinMoney<=" + planHeader.FixedQuota + " and MaxMoney>" + planHeader.FixedQuota).FirstOrDefault();
            planHeader.PlanTemplete = tempModel;
            DateTime workFromDate = toRePlanDetailList.FirstOrDefault().PlanDate;
            //规划的次数
            int replanCount = toRePlanDetailList.Count;
            planHeader.ExpenseCount = replanCount;
            List<CM_PlanDetail> listResult = new List<CM_PlanDetail>();
            //存入金额
            List<double> listDoublePayIn = new List<double>();
            foreach (CM_PlanDetail temp in toRePlanDetailList)
            {
                if (temp.StoreMoney > 0)
                {
                    listDoublePayIn.Add((double)temp.StoreMoney);
                }
            }

            double total = listDoublePayIn.Sum();
            //List<Model.CM_PlanRateConfig> list = GetPlanRateConfigList(planHeader);

            //得到消费笔数并分配到每台POS机上的金额
            List<PosNumAndExpenceMoney> listPNAM = CreatePlanDetailNew(expenseMoney, planHeader, planHeader.PlanRateConfigList, isUpdate, 0);
            listPNAM = listPNAM.Select(a => new { a, newID = Guid.NewGuid() }).OrderBy(b => b.newID).Select(c => c.a).ToList();
            decimal totalExpense = listPNAM.Sum(c => c.Money);
            //规划日期小于账单日期的，默认为从账单日到还款日规划；规划日期大于账单日期的，默认为从规划当日开始规划。
            DateTime currentPlanBeginDate = toRePlanDetailList.FirstOrDefault().PlanDate;
            DateTime truePlanBeginDate = currentPlanBeginDate;
            List<string> listMoreDate = new List<string>();
            //从账单日和还款日中间随机取出来几天，这几天是要多刷的
            int totalExpenseCount = replanCount;
            for (int i = 0; i < totalExpenseCount - listDoublePayIn.Count; i++)
            {
                string date = GetRandomTime(truePlanBeginDate, planHeader.PlanEndDate).ToString("yyyy-MM-dd");
                listMoreDate.Add(date);
            }
            //添加账单日到还款日的规划信息
            for (DateTime dt = truePlanBeginDate; dt <= planHeader.PlanEndDate; dt = dt.AddDays(1))
            {
                Model.CM_PlanDetail pdl = new Model.CM_PlanDetail();
                pdl.Code = CommonHelper.GetGuid;
                pdl.PlanHeaderId = planHeader.Code;
                pdl.WeekNum = Week(dt);
                pdl.PlanDate = dt;
                pdl.Company = 0;
                pdl.CreatedBy = "";
                pdl.CreatedTime = DateTime.Now;
                pdl.ExpenseStatus = 1;
                pdl.StoreStatus = 1;
                pdl.IsDeleted = false;
                pdl.Status = 1;
                //pdl.Sort = listResult.Max(c => c.Sort) + 1;
                //刷出金额得找到计算之后余额不为零的
                //昨天余额：
                decimal lastBalance = lastBalanceMoney;
                CM_PlanDetail opad = listResult.Where(c => c.Sort == (pdl.Sort - 1)).FirstOrDefault();
                if (opad != null)
                    lastBalance = opad.BalanceMoney;
                //先选择一个今天存入金额
                decimal todayStore = decimal.Parse(listDoublePayIn[0].ToString());
                listDoublePayIn.RemoveAt(0);
                //再查看刷出金额需要多少才能达到余额不是负数
                decimal reach0 = lastBalance + todayStore;
                List<PosNumAndExpenceMoney> listRandom = listPNAM.Where(c => c.Money < reach0).ToList();
                if (listRandom.Count <= 0)
                {
                    pdl.StoreMoney = todayStore;
                    pdl.ExpenseMoney = 0;
                    pdl.BalanceMoney = lastBalance + pdl.StoreMoney - pdl.ExpenseMoney;

                    pdl.TakeOffMoney = pdl.ExpenseMoney * 0.006M;
                    pdl.POSFirstIndustry = "";
                    pdl.POSSecondIndustry = "";
                    pdl.POSStoreName = "";
                }
                else
                {
                    int index = GetRandom().Next(0, listRandom.Count);
                    PosNumAndExpenceMoney PNAM = listRandom[index];
                    listPNAM.Remove(PNAM);
                    pdl.StoreMoney = todayStore;
                    pdl.ExpenseMoney = PNAM.Money;
                    pdl.BalanceMoney = lastBalance + pdl.StoreMoney - pdl.ExpenseMoney;

                    pdl.POSFirstIndustry = PNAM.POSFirstIndustry;
                    pdl.POSSecondIndustry = PNAM.POSSecondIndustry;
                    pdl.POSStoreName = PNAM.POSStoreName;

                    if (pdl.POSFirstIndustry == "first" || pdl.POSFirstIndustry == "second" || pdl.POSFirstIndustry == "third")
                        pdl.Rate = 0.006M;
                    else if (pdl.POSFirstIndustry == "other")
                        pdl.Rate = 0.0035M;
                    pdl.TakeOffMoney = pdl.ExpenseMoney * pdl.Rate;

                }
                pdl.Remark = planHeader.Bank;
                listResult.Add(pdl);
                //如果某一天有多个刷出，就再添加几条刷出
                int count = listMoreDate.Where(c => c == dt.ToString("yyyy-MM-dd")).Count();
                for (int j = 0; j < count; j++)
                {
                    Model.CM_PlanDetail moreDatPdl = new Model.CM_PlanDetail();
                    moreDatPdl.Code = CommonHelper.GetGuid;
                    moreDatPdl.PlanHeaderId = planHeader.Code;
                    moreDatPdl.WeekNum = Week(dt);
                    moreDatPdl.PlanDate = dt;
                    moreDatPdl.Company = 0;
                    moreDatPdl.CreatedBy = "";
                    moreDatPdl.CreatedTime = DateTime.Now;
                    moreDatPdl.ExpenseStatus = 1;
                    moreDatPdl.IsDeleted = false;
                    moreDatPdl.Status = 1;
                    moreDatPdl.StoreStatus = 1;
                    //moreDatPdl.Sort = listResult.Max(c => c.Sort) + 1;
                    //刷出金额得找到计算之后余额不为零的
                    //昨天余额：
                    decimal lastBalance1 = lastBalanceMoney;
                    CM_PlanDetail tenu = listResult.Where(c => c.Sort == (moreDatPdl.Sort - 1)).FirstOrDefault();
                    if (tenu != null)
                        lastBalance1 = tenu.BalanceMoney;
                    //先选择一个今天存入金额
                    decimal todayStore1 = 0;//decimal.Parse(listDoublePayIn[0].ToString());
                    //再查看刷出金额需要多少才能达到余额不是负数
                    decimal reach01 = lastBalance1 + todayStore1;
                    List<PosNumAndExpenceMoney> listRandomMore = listPNAM.Where(c => c.Money < reach01).ToList();
                    int indexMore = GetRandom().Next(0, listRandomMore.Count);
                    PosNumAndExpenceMoney PNAMoreDay = listRandomMore[indexMore];
                    listPNAM.Remove(PNAMoreDay);
                    moreDatPdl.ExpenseMoney = PNAMoreDay.Money;
                    moreDatPdl.BalanceMoney = lastBalance1 + moreDatPdl.StoreMoney - moreDatPdl.ExpenseMoney;

                    moreDatPdl.POSFirstIndustry = PNAMoreDay.POSFirstIndustry;
                    moreDatPdl.POSSecondIndustry = PNAMoreDay.POSSecondIndustry;
                    moreDatPdl.POSStoreName = PNAMoreDay.POSStoreName;
                    moreDatPdl.Remark = planHeader.Bank;

                    if (pdl.POSFirstIndustry == "first" || pdl.POSFirstIndustry == "second" || pdl.POSFirstIndustry == "third")
                        pdl.Rate = 0.006M;
                    else if (pdl.POSFirstIndustry == "other")
                        pdl.Rate = 0.0035M;
                    pdl.TakeOffMoney = pdl.ExpenseMoney * pdl.Rate;

                    listResult.Add(moreDatPdl);
                }
            }
            return listResult;
        }

        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = GetRandom();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();
            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);
            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;
            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }
            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }
            int maxValue = iTotalSecontds;
            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;
            int i = random.Next(System.Math.Abs(maxValue));
            DateTime result = minTime.AddSeconds(i);
            return result;
        }


        protected static List<PosNumAndExpenceMoney> CreatePlanDetailNew(decimal shuKaMoney, Model.CM_PlanHeader planHeader, List<Model.CM_PlanRateConfig> listPlanRateConfig, bool isUpdate, int hasExpenseCount, int addOutDate = 0)
        {
            //2016年10月9日20:52:44增加网络消费和线下消费比例
            decimal onlineTakePencent = planHeader.PlanTemplete.OnlineTake1;
            decimal realTakePencent = planHeader.PlanTemplete.RealTake1;
            if (planHeader.IsPayPlan)
            {
                onlineTakePencent = planHeader.PlanTemplete.OnlineTake2;
                realTakePencent = planHeader.PlanTemplete.RealTake2;
            }
            decimal realTakeMoney = shuKaMoney * (realTakePencent / 100);
            decimal onlineTakeMoney = shuKaMoney * (onlineTakePencent / 100);
            //再计算网络消费的笔数，和线下消费的笔数之和=总的消费笔数
            int onlineTakeCount = (int)Math.Ceiling(onlineTakePencent / 100 * (planHeader.ExpenseCount - hasExpenseCount));
            int realTakeCount = 0;
            if (onlineTakeCount < planHeader.ExpenseCount - hasExpenseCount)
                realTakeCount = planHeader.ExpenseCount - hasExpenseCount - onlineTakeCount;

            List<Model.CM_POSDevices> tempPosDevices = CommonBase.GetList<CM_POSDevices>("IsDeleted=0 and Status=1");
            List<rateCount> rateCountList = new List<rateCount>();
            foreach (Model.CM_PlanRateConfig rateconfig in listPlanRateConfig)
            {
                rateCountList.Add(new rateCount(rateconfig.RatePercent / 10 * realTakeMoney, rateconfig.Rate, rateconfig.RatePercent));
            }
            //取到网络消费的POS机
            Model.CM_POSDevices onlinePOS = tempPosDevices.FirstOrDefault(c => c.FirstIndustry == "other" && c.SecondIndustry == "100");
            //if (onlinePOS != null)
            //{
            //    rateCountList.Add(new rateCount(onlineTakeMoney, onlinePOS.Rate, onlinePOS.RatePercent));
            //}
            List<PosNumAndExpenceMoney> listPosNumAndMoney = new List<PosNumAndExpenceMoney>();

            //如果公司的pos机数量大于消费笔数，就从这些pos机中按百分比排序取到前planHeader.ExpenseCount - addOutDate个

            foreach (rateCount rc in rateCountList)
            {
                //查询到该费率下的所有占比大于0的POS机集合
                var listPos = tempPosDevices.Where(c => c.Rate == rc.rate && c.RatePercent > 0);
                int maxSort = 0;
                foreach (Model.CM_POSDevices pos in listPos)
                {
                    //这里面都是该费率占比大于0的，就是说都需要使用的POS机
                    try
                    {
                        maxSort = listPosNumAndMoney.Max(c => c.Sort);
                    }
                    catch
                    {
                        maxSort = 0;
                    }
                    listPosNumAndMoney.Add(new PosNumAndExpenceMoney(Math.Round(rc.totolMoney * pos.RatePercent / 1000, MidpointRounding.AwayFromZero), pos.Rate, pos.RatePercent, pos.Id, false, maxSort + 1, pos.FirstIndustry, pos.SecondIndustry, pos.StoreName));
                }
            }

            //再对整个需要刷出的pos机集合进行分析，比较总共的数量是否与规划主表设置的刷卡次数相等会小于大于；
            //如果相等，那就最好了，如果大于刷卡次数（这种情况很少见，但也要考虑），就把最小的pos机概率去掉，不用它刷
            int PosNumAndMoneyCount = listPosNumAndMoney.Count;
            if (PosNumAndMoneyCount > realTakeCount - addOutDate)
            {
                //排序，排序之后只选从大到小排列的前n条数据
                //decimal textCou = listPosNumAndMoney.Sum(c => c.Money);
                List<PosNumAndExpenceMoney> listFirstCountList = listPosNumAndMoney.OrderByDescending(c => c.PosPencent).Take(realTakeCount - addOutDate).ToList();
                //textCou = listFirstCountList.Sum(c=>c.Money);
                //取到差集
                List<PosNumAndExpenceMoney> listExcept = listPosNumAndMoney.Except(listFirstCountList).ToList();
                //把这差集，就是多出来的POS机及刷出金额，按照pos机费率分给listFirstCountList;
                foreach (PosNumAndExpenceMoney pa in listExcept)
                {
                    List<PosNumAndExpenceMoney> listMatchList = listFirstCountList.Where(c => c.Rate == pa.Rate).ToList();
                    if (listMatchList.Count > 0)
                    {
                        int mco = GetRandom().Next(0, listMatchList.Count - 1);
                        PosNumAndExpenceMoney nem = listMatchList[mco];
                        listFirstCountList.Remove(nem);
                        nem.Money += pa.Money;
                        listFirstCountList.Add(nem);
                    }
                }
                listPosNumAndMoney = listFirstCountList;
                //textCou = listFirstCountList.Sum(c => c.Money);
                //listPosNumAndMoney = listPosNumAndMoney.OrderByDescending(c => c.PosPencent).Take(planHeader.ExpenseCount - addOutDate).ToList();
            }
            else if (PosNumAndMoneyCount < realTakeCount - addOutDate)
            {
                List<PosNumAndExpenceMoney> li = new List<PosNumAndExpenceMoney>();
                if (planHeader.CountModel == 1)
                    resultListPosNumAndMoneyNew(realTakeCount - addOutDate, PosNumAndMoneyCount, listPosNumAndMoney, ref li);
                else
                    resultListPosNumAndMoneyNew(realTakeCount - addOutDate, PosNumAndMoneyCount, listPosNumAndMoney, ref li);
                listPosNumAndMoney = li;
            }
            //2016年10月9日21:20:19上面这是线下消费的集合，下面再添加网络消费的集合
            //网络消费总笔数，网络消费总金额，这里又要牵扯到随机算法了。。。。。
            //暂时使用平均每笔消费，以后再改
            if (onlineTakeCount >= 1)
            {
                int lastTakeCount = onlineTakeCount;
                decimal perOnlineTakeMoney = onlineTakeMoney / onlineTakeCount; //每笔平均消费金额
                if (onlineTakeCount % 2 == 0) //是2的倍数
                {
                    lastTakeCount = lastTakeCount / 2;
                }
                else if (onlineTakeCount % 2 != 0 && onlineTakeCount > 2) //不是2的倍数并且消费笔数大于2
                {
                    lastTakeCount = (onlineTakeCount - 1) / 2;
                }
                if (onlineTakeCount == 1)
                {
                    PosNumAndExpenceMoney pn = new PosNumAndExpenceMoney(Math.Floor(perOnlineTakeMoney), onlinePOS.Rate, onlinePOS.RatePercent, onlinePOS.Id, true, 0, onlinePOS.FirstIndustry, onlinePOS.SecondIndustry, onlinePOS.StoreName);
                    listPosNumAndMoney.Add(pn);
                }
                else
                {
                    for (int i = 0; i < lastTakeCount; i++)
                    {
                        Random rand = GetRandom();
                        int d = rand.Next(60, 140);
                        decimal result = Math.Ceiling((perOnlineTakeMoney / 2) * d / 100);

                        PosNumAndExpenceMoney pn1 = new PosNumAndExpenceMoney(Math.Floor(result), onlinePOS.Rate, onlinePOS.RatePercent, onlinePOS.Id, true, 0, onlinePOS.FirstIndustry, onlinePOS.SecondIndustry, onlinePOS.StoreName);
                        listPosNumAndMoney.Add(pn1);
                        PosNumAndExpenceMoney pn2 = new PosNumAndExpenceMoney(Math.Floor(perOnlineTakeMoney - result), onlinePOS.Rate, onlinePOS.RatePercent, onlinePOS.Id, true, 0, onlinePOS.FirstIndustry, onlinePOS.SecondIndustry, onlinePOS.StoreName);
                        listPosNumAndMoney.Add(pn2);
                    }
                }
                if (onlineTakeCount % 2 != 0 && onlineTakeCount > 2) //如果是奇数，就单独再来一项
                {
                    PosNumAndExpenceMoney pn = new PosNumAndExpenceMoney(Math.Floor(perOnlineTakeMoney), onlinePOS.Rate, onlinePOS.RatePercent, onlinePOS.Id, true, 0, onlinePOS.FirstIndustry, onlinePOS.SecondIndustry, onlinePOS.StoreName);
                    listPosNumAndMoney.Add(pn);
                }
            }

            //再看刷卡金额，刷卡金额
            decimal lessMoney = shuKaMoney - listPosNumAndMoney.Sum(c => c.Money);
            if (lessMoney > 0)
            {
                //重新分配刷卡金额
                ResetShuaKaMoney(listPosNumAndMoney, lessMoney);
            }
            return listPosNumAndMoney;
        }

        //这个方法需要重新修改//2015-10-8日深夜
        protected static void resultListPosNumAndMoneyNew(int ExpenseCount, int PosNumAndMoneyCount, List<PosNumAndExpenceMoney> listPosNumAndMoney, ref List<PosNumAndExpenceMoney> li)
        {
            List<PosNumAndExpenceMoney> newlistPosNumAndMoney = new List<PosNumAndExpenceMoney>();
            //小于的时候，就是需要某些POS机刷两次或多次，但这个多次的总和是固定的
            //计算少几次
            int lessCount = ExpenseCount - PosNumAndMoneyCount;
            listPosNumAndMoney = listPosNumAndMoney.OrderByDescending(c => c.Money).ToList();
            newlistPosNumAndMoney = listPosNumAndMoney;
            int maxSort = 0;
            //把最大的金额一份为二
            PosNumAndExpenceMoney removeObj = listPosNumAndMoney[0];
            newlistPosNumAndMoney.Remove(removeObj);
            try
            {
                maxSort = listPosNumAndMoney.Max(c => c.Sort);
            }
            catch
            {
                maxSort = 0;
            }
            //分成两个，一个60%；一个40%;这里需要随机，从35到65之间随机
            Random random = GetRandom();
            random.NextDouble();
            decimal orginMoney = removeObj.Money;
            decimal orgionPencent = removeObj.PosPencent;

            Random ran = GetRandom();
            int RandKey = ran.Next(35, 60);
            double orh = RandKey / 100.00;
            double yus = 1 - orh;
            decimal firstMoney = Convert.ToDecimal(orh) * orginMoney;
            decimal firstPercent = orgionPencent * Convert.ToDecimal(orh);
            decimal secondMoney = orginMoney * Convert.ToDecimal(yus);
            decimal secondPercent = orgionPencent * Convert.ToDecimal(yus);

            newlistPosNumAndMoney.Add(new PosNumAndExpenceMoney(Math.Round(firstMoney, MidpointRounding.AwayFromZero), removeObj.Rate, firstPercent, removeObj.PosNum, false, maxSort + 1, removeObj.POSFirstIndustry, removeObj.POSSecondIndustry, removeObj.POSStoreName));
            newlistPosNumAndMoney.Add(new PosNumAndExpenceMoney(Math.Round(secondMoney, MidpointRounding.AwayFromZero), removeObj.Rate, secondPercent, removeObj.PosNum, false, maxSort + 2, removeObj.POSFirstIndustry, removeObj.POSSecondIndustry, removeObj.POSStoreName));
            PosNumAndMoneyCount = newlistPosNumAndMoney.Count;
            if (PosNumAndMoneyCount < ExpenseCount)
            {
                resultListPosNumAndMoneyNew(ExpenseCount, PosNumAndMoneyCount, newlistPosNumAndMoney, ref li);
            }
            if (PosNumAndMoneyCount == ExpenseCount)
            {
                li = newlistPosNumAndMoney;
            }
        }


        public static Random GetRandom()
        {
            Thread.Sleep(1);
            long tick = DateTime.Now.Ticks;//一个以0.1纳秒为单位的时间戳，18位
            int seed = int.Parse(tick.ToString().Substring(9)); //  int类型的最大值:  2147483647
            //或者使用unchecked((int)tick)也可
            return new Random(seed);
        }

        protected static void ResetShuaKaMoney(List<PosNumAndExpenceMoney> listPNAE, decimal lessMoney)
        {
            if (lessMoney > 0)
            {
                foreach (PosNumAndExpenceMoney po in listPNAE)
                {
                    if (lessMoney > 100)
                    {
                        po.Money += 100;
                        lessMoney -= 100;
                    }
                    else
                    {
                        po.Money += lessMoney;
                        lessMoney = 0;
                    }
                    if (lessMoney <= 0)
                    {
                        break;
                    }
                }
                ResetShuaKaMoney(listPNAE, lessMoney);
            }
        }



        #region 自定义类
        public class rateCount
        {
            public rateCount(decimal _count, decimal _rate, decimal _ratePencent)
            {
                this.totolMoney = _count;
                this.rate = _rate;
                this.ratePencent = _ratePencent;
            }
            /// <summary>
            /// 总金额
            /// </summary>
            public decimal totolMoney { get; set; }
            /// <summary>
            /// 费率
            /// </summary>
            public decimal rate { get; set; }
            /// <summary>
            /// 费率所占百分比
            /// </summary>
            public decimal ratePencent { get; set; }
        }
        public class outDateCount
        {
            public outDateCount(int _count, string _date)
            {
                this.count = _count;
                this.date = _date;
            }
            public int count { get; set; }
            public string date { get; set; }
        }
        public class RateToValue
        {
            public RateToValue(string rate, string rateVal)
            {
                this.Rate = rate;
                this.RateVal = rateVal;
            }
            public string Rate { get; set; }
            public string RateVal { get; set; }
        }

        //********************************************************
        //另外一种生成随机规划的方法
        //********************************************************
        //自定义一个类，这个类存取所有需要刷出的POS的编号和金额
        public class PosNumAndExpenceMoney
        {
            public PosNumAndExpenceMoney(decimal _money, decimal _rate, decimal _posPencent, int _posNum, bool _isRandom, int _sort, string _POSFirstIndustry, string _POSSecondIndustry, string _POSStoreName)
            {
                this.Money = _money;
                this.Rate = _rate;
                this.PosPencent = _posPencent;
                this.PosNum = _posNum;
                this.IsRandom = _isRandom;
                this.Sort = _sort;
                this.POSFirstIndustry = _POSFirstIndustry;
                this.POSSecondIndustry = _POSSecondIndustry;
                this.POSStoreName = _POSStoreName;
            }
            /// <summary>
            /// 刷出金额
            /// </summary>
            public decimal Money { get; set; }
            /// <summary>
            /// Pos费率
            /// </summary>
            public decimal Rate { get; set; }
            /// <summary>
            /// 该Pos机所占百分比
            /// </summary>
            public decimal PosPencent { get; set; }
            /// <summary>
            /// POS机编号
            /// </summary>
            public int PosNum { get; set; }
            public bool IsRandom { get; set; }
            public int Sort { get; set; }
            public string POSFirstIndustry { get; set; }
            public string POSSecondIndustry { get; set; }
            public string POSStoreName { get; set; }
        }
        #endregion

        #region 随机相关

        protected static decimal GetStoreMoney(List<double> listDoublePayIn, decimal todayTotalStoreMoney, decimal dayMaxStoreMoney)
        {
            decimal retuenMoney = Math.Round((decimal)listDoublePayIn[0], 0);
            return retuenMoney;
        }

        List<double> lisDouNewPlus = new List<double>();
        public static string Week(DateTime dt)
        {
            string[] weekdays = { "日", "一", "二", "三", "四", "五", "六" };
            string week = weekdays[Convert.ToInt32(dt.DayOfWeek)];
            return week;
        }

        protected static Model.CM_PlanDetail SetExpenseMoney(Model.CM_PlanHeader planHeader, Model.CM_PlanDetail lastPlanDetail, Model.CM_PlanDetail pdl, List<PosNumAndExpenceMoney> listPNAM, decimal todayTotalExpenseMoney, decimal dayTotalExpenseMoney, int avoidCount)
        {
            //2015-10-7：第二种修改的刷出金额begin
            decimal perExpenseMOney = planHeader.BillMoney / (planHeader.ExpenseCount - avoidCount);
            decimal maxExpense = CacheService.ConfigurationModel.OutMaxFloat * perExpenseMOney;
            decimal minExpense = CacheService.ConfigurationModel.OutMinFloat * perExpenseMOney;
            var listRandom = listPNAM.OrderByDescending(c => c.Money).ToList();
            int count = listRandom.Count();
            PosNumAndExpenceMoney PNAM = null;
            if (count > 0)
            {
                int index = GetRandom().Next(0, count);
                PNAM = listRandom.ToList()[index];
                if (PNAM != null)
                {
                    listPNAM.Remove(PNAM);
                    pdl.ExpenseMoney = PNAM.Money;
                    pdl.POSNum = PNAM.PosNum.ToString();
                    pdl.Rate = PNAM.Rate;
                    pdl.RatePercent = PNAM.PosPencent;
                    pdl.TakeOffMoney = pdl.ExpenseMoney * pdl.Rate;
                    pdl.POSFirstIndustry = PNAM.POSFirstIndustry;
                    pdl.POSSecondIndustry = PNAM.POSSecondIndustry;
                    pdl.POSStoreName = PNAM.POSStoreName;
                }
                else
                {
                    pdl.POSNum = "0";
                    pdl.Rate = 0;
                    pdl.RatePercent = 0;
                }
            }
            //2015-10-7：第二种修改的刷出金额end
            return pdl;
        }

        #endregion

        /// <summary>
        /// 执行每日计划执行的脚本
        /// </summary>
        protected static void CheckEveryDayPlanScript(string key)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(AppDomain.CurrentDomain.BaseDirectory + "SQLScript.xml");
                //读取节点
                XmlNodeList snXmlNode = xml.SelectSingleNode("functionroot").SelectNodes("function");
                foreach (XmlNode node in snXmlNode)
                {
                    if (node.Attributes["key"].Value == key)
                    {
                        string innerSql = node.FirstChild.InnerText;
                        CommonBase.RunSql(innerSql);
                        break;
                    }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
