using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_CodingWord
    [EnitityMapping(TableName = "T_CodingWord")]
    public class T_CodingWord
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        public string CodeWord { get; set; }
       
        public bool IsDeleted { get; set; }
     
        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}

