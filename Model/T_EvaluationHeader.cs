using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    /// <summary>
    /// 学员测评主表
    /// </summary>
    [EnitityMapping(TableName = "T_EvaluationHeader")]
    public class T_EvaluationHeader
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string UserCode { get; set; }

        public DateTime EvalBeginTime { get; set; }
        public DateTime? EvalEndTime { get; set; }
        /// <summary>
        /// 试卷名称
        /// </summary>
        public string PaperCode { get; set; }
        public int QuestionCount { get; set; }
        public int CorrectCount { get; set; }
        public int ErrorCount { get; set; }
        public int AnswerCount { get; set; }

        public string CreatedBy { get; set; }

        public string Remark { get; set; }
        public bool IsDeleted { get; set; }

        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}

