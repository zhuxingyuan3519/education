using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    /// <summary>
    /// 单词词库表
    /// </summary>
    [EnitityMapping(TableName = "T_EvaluationPaper")]
    public class T_EvaluationPaper
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Grade { get; set; }
        public string Leavel { get; set; }
        public string Unit { get; set; }

        public string CreatedBy { get; set; }

        public string Remark { get; set; }
        public bool IsDeleted { get; set; }

        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}

