using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //TD_SHMoney
    [EnitityMapping(TableName = "TD_SHMoney")]
    public class TD_SHMoney
    {

        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public int Id { get; set; }
        /// <summary>
        /// Code
        /// </summary>		

        public string Code { get; set; }
        /// <summary>
        /// RoleCode
        /// </summary>		

        public string RoleCode { get; set; }
        /// <summary>
        /// TJIndex
        /// </summary>		

        public int TJIndex { get; set; }
        /// <summary>
        /// TJFloat
        /// </summary>		

        public decimal TJFloat { get; set; }
        /// <summary>
        /// FHFloat
        /// </summary>		

        public decimal FHFloat { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>		

        public bool IsDeleted { get; set; }
     /// <summary>
        /// Status
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
        /// <summary>
        /// 分红会员的Code，这个是动态添加的
        /// </summary>
        public string Field1 { get; set; }
        /// <summary>
        /// 分红时会员的角色，这个也是动态添加的
        /// </summary>
        public string Field2 { get; set; }
        /// <summary>
        /// 使用的分红金额方式，1-是缴费金额的百分比（小数位）。2-真实的分红金额，不打折
        /// </summary>
        public string Field3 { get; set; }
        /// <summary>
        /// 课程等级，初级-KC0001，高级-KC0002，根据这个课程等级确定具体的分红配置类型
        /// </summary>
        public string Field4 { get; set; }

    }
}

