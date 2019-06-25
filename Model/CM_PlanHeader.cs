using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_PlanHeader
    [EnitityMapping(TableName = "CM_PlanHeader")]
    public class CM_PlanHeader
    {

        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string ArchiveId { get; set; }
        public string Name { get; set; }
        public string Bank { get; set; }
        public string CardID { get; set; }
        public decimal FixedQuota { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime RepayDate { get; set; }
        public decimal BillMoney { get; set; }
        public decimal ServiceMoney { get; set; }
        public decimal ReferBillMoney { get; set; }
        public decimal AgingOutMoney { get; set; }
        public decimal AgingInMoney { get; set; }
        public decimal AgingOutTakeOffMoney { get; set; }
        public decimal AgingInTakeOffMoney { get; set; }
        /// <summary>
        /// 使用模式
        /// </summary>
        public int CountModel { get; set; }
        public string AvoidDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public int ExpenseCount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime?  LastUpdateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int Company { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalStoreMoney { get; set; }
        public decimal TotalExpenseMoney { get; set; }
        public decimal BalanceMoney { get; set; }
        public DateTime PlanBeginDate { get; set; }
        public decimal LeavePencent { get; set; }
        public decimal LeaveMoney { get; set; }
        public string TempCode { get; set; }
        /// <summary>
        /// 规划时卡内余额
        /// </summary>
        public decimal PlanBalanceMoney { get; set; }


        [EnitityMapping(ColumnType = "IGNORE")]
        public List<CM_PlanRateConfig> PlanRateConfigList { get; set; }

        [EnitityMapping(ColumnType = "IGNORE")]
        public CM_Template PlanTemplete { get; set; }

        /// <summary>
        /// 是否是还款规划
        /// </summary>
        //[EnitityMapping(ColumnType = "IGNORE")]
        public bool IsPayPlan { get; set; }

    }
}

