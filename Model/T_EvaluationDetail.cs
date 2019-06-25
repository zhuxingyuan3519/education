using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    /// <summary>
    /// 学员测评明细表
    /// </summary>
    [EnitityMapping(TableName = "T_EvaluationDetail")]
    public class T_EvaluationDetail
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string HeaderCode { get; set; }
         public string PaperCode { get; set; }
         public string WordCode { get; set; }
         public string WordEnglish { get; set; }
        public string WordChinese { get; set; }
        public string Answer { get; set; }

        public DateTime? AnswerTime { get; set; }
     
        public string CreatedBy { get; set; }
        public string Remark { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 答题状态【0】未答，【1】正确，【2】错误
        /// </summary>
        public int Status { get; set; }
        public int Sort { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}

