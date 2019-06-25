using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //Sys_Menu
    [EnitityMapping(TableName = "Sys_SecurityQuestion")]
    public class Sys_SecurityQuestion
    {

        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public long ID { get; set; }
        /// <summary>
        /// ParentCode
        /// </summary>		

        public string Code { get; set; }
        /// <summary>
        /// Name
        /// </summary>		

        public string Question { get; set; }
        /// <summary>
        /// Icon
        /// </summary>		

        public string CreatedBy { get; set; }
        /// <summary>
        /// URL
        /// </summary>		

        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// IsDeleted
        /// </summary>		

        public bool IsDeleted { get; set; }
        public int Status { get; set; }


    }
}

