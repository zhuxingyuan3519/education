using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_TrainDetail
    [EnitityMapping(TableName = "T_TrainDetail")]
    public class T_TrainDetail
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        public string TrainCode { get; set; }

        public string CodeId { get; set; }

        public string CodeName { get; set; }

        public string Remark { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 状态,是否回答正确，1-未回答，2-正确，3-错误
        /// </summary>
        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}

