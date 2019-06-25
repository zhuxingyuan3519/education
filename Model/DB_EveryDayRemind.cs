using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //DB_Message
    [EnitityMapping(TableName = "DB_EveryDayRemind")]
    public class DB_EveryDayRemind
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        /// <summary>
        /// 提醒类型：1、今日出账单请规划;2、今日是最后还款日，还需还款xx元钱;3、临时额度    3天后/2天后/今天   到期；4、不占额度分期今日到期；5、占额度分期今日到期
        /// 6-最新口子提醒项目
        /// </summary>		
        public string RType { get; set; }
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
        /// 规划表中需要去规划的档案Code
        /// </summary>		
        public string Field1 { get; set; }
        /// <summary>
        /// 最新口子提示的用户的口子编号ID
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
        /// IsDeleted
        /// </summary>		
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// CreatedTime
        /// </summary>		
        public DateTime CreatedTime { get; set; }
        public DateTime? ReadTime { get; set; }
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

