using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_TrainHeader
    [EnitityMapping(TableName = "T_TrainHeader")]
    public class T_TrainHeader
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 1-速记基础训练，2-数字训练，3-扑克牌训练，4-字母训练，5-速记高手训练
        /// </summary>
        public string CodeType { get; set; }

        public int TrainCount { get; set; }

        public int ShowTime { get; set; }
        /// <summary>
        /// 记忆开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 记忆结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 答题开始时间
        /// </summary>
        public string AnswerBeginTime { get; set; }
        /// <summary>
        /// 答题结束时间
        /// </summary>
        public string AnswerEndTime { get; set; }
        /// <summary>
        /// 复习时间，单位秒
        /// </summary>
        public int ReviewTime { get; set; }
        public int CorrectCount { get; set; }
        public int ErrorCount { get; set; }

        /// <summary>
        /// 每次复习的开始时间
        /// </summary>
        public string Remark { get; set; }
        public string UserCode { get; set; }

        public bool IsDeleted { get; set; }

        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}

