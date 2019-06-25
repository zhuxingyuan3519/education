using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    [EnitityMapping(TableName = "SH_HirePurchaseDetail")]
    public class SH_HirePurchaseDetail
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string HirePurchaseId { get; set; }
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public int HireTotalCount { get; set; }
        public decimal HireMoney { get; set; }
        public int HireCount { get; set; }
        public DateTime PayDate { get; set; }
        /// <summary>
        /// 付款状态，0-未付款，1-已付款
        /// </summary>
        public int PayStatus { get; set; }
        public int TradePointCount { get; set; }
        public int LeaveTradePointCount { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? RealPayDateTime { get; set; }
        public string CreatedBy { get; set; }
    }
}

