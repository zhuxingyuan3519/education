using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "TD_TXLog")]
    public class TD_TXLog
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 1-微信，2-支付宝，3-银行卡，4-TPay，5-VPay
        /// </summary>
        public string TXBank { get; set; }
        public string TXCard { get; set; }
        public string TXName { get; set; }
        public string MID { get; set; }
        public decimal TXMoney { get; set; }
        public decimal FeeMoney { get; set; }
        public decimal RealMoney { get; set; }
        /// <summary>
        /// 会计通知出纳转账
        /// </summary>
        public DateTime ApplyTXDate { get; set; }
        /// <summary>
        /// 会计通知出纳转账
        /// </summary>
        public DateTime? TXDealTime1 { get; set; }
        /// <summary>
        /// 出纳已转账
        /// </summary>
        public DateTime? TXDealTime2 { get; set; }
        /// <summary>
        /// 通知会员已支付
        /// </summary>
        public DateTime? TXDealTime3 { get; set; }


        public bool IsDeleted { get; set; }
        /// <summary>
        /// CreatedTime
        /// </summary>		

        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		

        public string CreatedBy { get; set; }
        /// <summary>
        /// 状态; 1-提交申请；2-已转账 /// 1-提交申请，2-会计通知出纳转账；3-出纳已转账；4-通知会员已支付
        /// </summary>		

        public int Status { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		

        public string Remark { get; set; }
        /// <summary>
        /// Company
        /// </summary>		

        public string Company { get; set; }

        public string AliPay { get; set; }

        public string WeixinPay { get; set; }
        /// <summary>
        /// tpay昵称
        /// </summary>
        public string TXMCode { get; set; }

    }
}
