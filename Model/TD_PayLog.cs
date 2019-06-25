using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //TD_PayLog
    [EnitityMapping(TableName = "TD_PayLog")]
    public class TD_PayLog
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// PayMID
        /// </summary>		

        public string PayMID { get; set; }
        /// <summary>
        /// PayWay
        /// </summary>		

        public string PayWay { get; set; }
        /// <summary>
        /// PayID
        /// </summary>		

        public string PayID { get; set; }
        /// <summary>
        /// PayForMID
        /// </summary>		

        public string PayForMID { get; set; }
        /// <summary>
        /// PayTime
        /// </summary>		

        public DateTime PayTime { get; set; }
        /// <summary>
        /// PayMoney
        /// </summary>		

        public decimal PayMoney { get; set; }
        /// <summary>
        /// ProductCode
        /// </summary>		

        public string ProductCode { get; set; }
        /// <summary>
        /// ProductCount
        /// </summary>		

        public int ProductCount { get; set; }
        /// <summary>
        /// 买家支付宝账号
        /// </summary>		

        public string PayPic { get; set; }
        /// <summary>
        /// PayPic2
        /// </summary>		

        public string PayPic2 { get; set; }
        /// <summary>
        /// PayPic3
        /// </summary>		

        public string PayPic3 { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>		

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
        /// 订单状态，0-提交未支付，1-支付成功，2-支付失败
        /// </summary>		

        public int Status { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		

        public string Remark { get; set; }
        /// <summary>
        /// Company
        /// </summary>		

        public int Company { get; set; }
        /// <summary>
        /// ApplyStudent-申请成为学员
        /// </summary>

        public string PayType { get; set; }

    }
}

