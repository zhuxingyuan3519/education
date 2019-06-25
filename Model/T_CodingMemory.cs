using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_CodingMemory
    [EnitityMapping(TableName = "T_CodingMemory")]
    public class T_CodingMemory
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        public string CodeType { get; set; }
        /// <summary>
        /// 编码名称
        /// </summary>		

        public string PKCode { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>		

        public string CodeName { get; set; }
        /// <summary>
        /// 编码图片
        /// </summary>		

        public string CodeImage { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		

        public string Remark { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>		

        public bool IsDeleted { get; set; }
        /// <summary>
        /// Status
        /// </summary>		

        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 扑克数字，针对扑克牌来说
        /// </summary>
        public string PuKeNum { get; set; }

    }
}

