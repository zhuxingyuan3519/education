using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //DB_Message
    [EnitityMapping(TableName = "DB_Log")]
    public class DB_Log
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        /// <summary>
        /// 1-登录系统；2-创建会员；3-删除会员；4-创建管理员；5-删除管理员，6-开通或禁用系统，7-会员自主注册，8-支付相关
        /// </summary>		
        public string LType { get; set; }
        /// <summary>
        /// SendName
        /// </summary>		
        public string MID { get; set; }
        /// <summary>
        /// ReceiveCode
        /// </summary>		
        public string MCode { get; set; }
        /// <summary>
        /// ReceiveName
        /// </summary>		
        public DateTime LogDate { get; set; }
        /// <summary>
        /// Message
        /// </summary>		
        public string OperatorRole { get; set; }
        /// <summary>
        /// 针对登录日志，当天最后一次的登录时间
        /// </summary>		
        public string Field1 { get; set; }
        /// <summary>
        /// 登录所在省
        /// </summary>		
        public string Field2 { get; set; }
        /// <summary>
        /// 登录所在市
        /// </summary>		
        public string Field3 { get; set; }
        /// <summary>
        /// 登录所在区县
        /// </summary>		
        public string Field4 { get; set; }
        /// <summary>
        /// 登录所在经纬度
        /// </summary>		
        public string Field5 { get; set; }

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
        /// 状态，1-未读，2-已读
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark { get; set; }

    }
}

