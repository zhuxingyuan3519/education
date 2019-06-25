using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "Sys_Privage")]
    public class Sys_Privage
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Id { get; set; }
        public string ParentCode { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public int MenuIndex { get; set; }

        public int PrivageType { get; set; }
        public string Company { get; set; }
        public bool IsDeleted { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public string Field1 { get; set; }
        /// <summary>
        /// Field2
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
    }
}
