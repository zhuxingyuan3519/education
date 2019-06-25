using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //Sys_Menu
    [EnitityMapping(TableName = "Sys_SQ_Answer")]
    public class Sys_SQ_Answer
    {

        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public long ID { get; set; }
        public long MID { get; set; }

        public string Code { get; set; }
        public string Answer { get; set; }
        /// <summary>
        /// Icon
        /// </summary>		

        public string CreatedBy { get; set; }
        /// <summary>
        /// URL
        /// </summary>		

        public DateTime CreatedTime { get; set; }
        public string LastUpdateBy { get; set; }
        /// <summary>
        /// URL
        /// </summary>		

        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// IsDeleted
        /// </summary>		

        public bool IsDeleted { get; set; }
        public int Status { get; set; }
        public long QId { get; set; }
        public int Company { get; set; }


    }
}

