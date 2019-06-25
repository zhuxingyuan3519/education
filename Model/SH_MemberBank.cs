using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "SH_MemberBank")]
    public class SH_MemberBank
    {
        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 银行编码
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankNum { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public string ReceiveName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string MID { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string MCode { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark { get; set; }
        public int Status { get; set; }
    }
}
