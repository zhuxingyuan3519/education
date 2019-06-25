using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //TD_FHLog
    [EnitityMapping(TableName = "TD_FHLog")]
    public class TD_FHLog
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 分红类型：单个数字就是TD_SHMoney中的ID，如果是以P-开头的，就是奖金池中(SH_PrizePool)的Id
        /// </summary>		

        public string FHType { get; set; }
        /// <summary>
        /// PayCode
        /// </summary>		

        public string PayCode { get; set; }
        /// <summary>
        /// FHDate
        /// </summary>		

        public DateTime FHDate { get; set; }
        /// <summary>
        /// FHMoney
        /// </summary>		

        public decimal FHMoney { get; set; }
        /// <summary>
        /// MID
        /// </summary>		

        public string MID { get; set; }
        /// <summary>
        /// FHRoleCode
        /// </summary>		

        public string FHRoleCode { get; set; }
        public string FHMCode { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>		

        public bool IsDeleted { get; set; }
        /// <summary>
        /// CreatedTime
        /// </summary>		

        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		

        public string CreatedBy { get; set; }
        /// <summary>
        /// 红包及分红状态，1-可显示，2-不显示
        /// </summary>		

        public int Status { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		

        public string Remark { get; set; }
        /// <summary>
        /// Company
        /// </summary>		

        public int Company { get; set; }

        public string SHMoneyCode { get; set; }
        public string ProductCode { get; set; }

    }
}

