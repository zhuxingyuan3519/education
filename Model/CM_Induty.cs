using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_Induty
    [EnitityMapping(TableName = "CM_Induty")]
    public class CM_Induty
    {
        public int Company
        { get; set; }

        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public int Id
        { get; set; }
        /// <summary>
        /// ParentCode
        /// </summary>		
        public string ParentCode
        { get; set; }
        /// <summary>
        /// Code
        /// </summary>		
        public string Code
        { get; set; }
        /// <summary>
        /// Name
        /// </summary>		
        public string Name
        { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>		
        public bool IsDeleted
        { get; set; }
        /// <summary>
        /// CreatedTime
        /// </summary>		
        public DateTime CreatedTime
        { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public string CreatedBy
        { get; set; }
        /// <summary>
        /// LastUpdateTime
        /// </summary>		
        public DateTime? LastUpdateTime
        { get; set; }
        /// <summary>
        /// LastUpdateBy
        /// </summary>		
        public string LastUpdateBy
        { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        public int Status
        { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark
        { get; set; }

    }
}

