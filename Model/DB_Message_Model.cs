using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //DB_Message
    [EnitityMapping(TableName = "DB_Message")]
    public class DB_Message_Model
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 1-发送
        /// </summary>		
        public string MType { get; set; }
        /// <summary>
        /// SendCode
        /// </summary>		
        public string SendCode { get; set; }
        /// <summary>
        /// SendName
        /// </summary>		
        public string SendName { get; set; }
        /// <summary>
        /// ReceiveCode
        /// </summary>		
        public string ReceiveCode { get; set; }
        /// <summary>
        /// ReceiveName
        /// </summary>		
        public string ReceiveName { get; set; }
        /// <summary>
        /// Message
        /// </summary>		
        public string Message { get; set; }
        /// <summary>
        /// 发送人姓名
        /// </summary>		
        public string Field1 { get; set; }
        /// <summary>
        /// 收信人的姓名
        /// </summary>		
        public string Field2 { get; set; }
        /// <summary>
        /// Field3
        /// </summary>		
        public string Field3 { get; set; }
        /// <summary>
        /// Field4
        /// </summary>		
        public string Field4 { get; set; }
        /// <summary>
        /// Field5
        /// </summary>		
        public string Field5 { get; set; }
        /// <summary>
        /// Field6
        /// </summary>		
        public string Field6 { get; set; }
        /// <summary>
        /// Field7
        /// </summary>		
        public string Field7 { get; set; }
        /// <summary>
        /// Field8
        /// </summary>		
        public string Field8 { get; set; }
        /// <summary>
        /// Field9
        /// </summary>		
        public string Field9 { get; set; }
        /// <summary>
        /// Field10
        /// </summary>		
        public string Field10 { get; set; }
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
        /// LastUpdateTime
        /// </summary>		
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// LastUpdateBy
        /// </summary>		
        public string LastUpdateBy { get; set; }
        /// <summary>
        /// 状态，1-未读，2-已读
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 1-意见反馈，2-支付信息，3提现信息，空或null为普通消息，4-红包发放信息
        /// </summary>		
        public string Remark { get; set; }

    }
}

