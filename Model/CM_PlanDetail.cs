using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_PlanDetail
    [EnitityMapping(TableName = "CM_PlanDetail")]
    public class CM_PlanDetail
    {


        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string PlanHeaderId { get; set; }
        public string WeekNum { get; set; }
        public DateTime PlanDate { get; set; }
        public string POSNum { get; set; }
        public decimal Rate { get; set; }
        public decimal RatePercent { get; set; }
        public decimal StoreMoney { get; set; }
        public decimal ExpenseMoney { get; set; }
        public decimal BalanceMoney { get; set; }
        public decimal TakeOffMoney { get; set; }
        public int StoreStatus { get; set; }
        public DateTime? StoreTime { get; set; }
        public string StoredBy { get; set; }
        public int ExpenseStatus { get; set; }
        public DateTime? ExpenseTime { get; set; }
        public string ExpensedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// 银行信息，所属于哪个银行
        /// </summary>
        public string Remark { get; set; }
        public int Company { get; set; }
        public string POSFirstIndustry { get; set; }
        public string POSSecondIndustry { get; set; }
        public string POSStoreName { get; set; }
        public int Sort { get; set; }
        public string QuataChangeFlag { get; set; }
        public decimal QuataChangeMoney { get; set; }

    }
}

