using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //DB_ResponseMessage
    [EnitityMapping(TableName = "DB_ResponseMessage")]
    public class DB_ResponseMessage_Model
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// MType
        /// </summary>		
        public string MType { get; set; }
        /// <summary>
        /// RMcode
        /// </summary>		
        public string RMcode { get; set; }
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
        /// 接收人姓名
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
        /// Status
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark { get; set; }

    }
}

