using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_Template
    [EnitityMapping(TableName = "CM_Template")]
    public class CM_Template
    {

        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// Bank
        /// </summary>		
        public string Bank
        { get; set; }
        /// <summary>
        /// MinMoney
        /// </summary>		
        public decimal MinMoney
        { get; set; }
        /// <summary>
        /// MaxMoney
        /// </summary>		
        private decimal _maxmoney;
        public decimal MaxMoney
        { get; set; }
        /// <summary>
        /// CostCount
        /// </summary>		
        public int CostCount
        { get; set; }
        /// <summary>
        /// LeaveMoney
        /// </summary>		
        public decimal LeaveMoney
        { get; set; }
        /// <summary>
        /// LeavePencent
        /// </summary>		
        public decimal LeavePencent
        { get; set; }
        public decimal LeavePencent2
        { get; set; }
        /// <summary>
        /// 提额规划网络消费比例
        /// </summary>
        public decimal OnlineTake1
        { get; set; }
        /// <summary>
        /// 还款规划网络消费比例
        /// </summary>
        public decimal OnlineTake2
        { get; set; }
        /// <summary>
        /// 提额规划线下消费比例
        /// </summary>
        public decimal RealTake1
        { get; set; }
        /// <summary>
        /// 还款规划线下消费比例
        /// </summary>
        public decimal RealTake2
        { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>		
        public bool IsDeleted
        { get; set; }
    
        /// <summary>
        /// Status
        /// </summary>		
        public int Status
        { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark
        { get; set; }
        /// <summary>
        /// Company
        /// </summary>		
        public int Company
        { get; set; }
        public DateTime CreatedTime
        { get; set; }
        public string CreatedBy
        { get; set; }

    }
}

