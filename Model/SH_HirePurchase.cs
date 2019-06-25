using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //SH_HirePurchase
    [EnitityMapping(TableName = "SH_HirePurchase")]
    public class SH_HirePurchase
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Id { get; set; }
        /// <summary>
        /// 申请支付方式
        /// </summary>
        public string HireId { get; set; }
        public int HireCount { get; set; }
        public decimal EveryHireMoney { get; set; }
        public int PayDate { get; set; }
        /// <summary>
        /// 申请支付方式，1-微信支付，2-tpay,3-vpay
        /// </summary>
        public int HireType { get; set; }
        public int TradePointCount { get; set; }
        public int LeaveTradePointCount { get; set; }
        /// <summary>
        /// 申请级别
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 申请地区编号
        /// </summary>
        public string Remark { get; set; }
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
    }
}

