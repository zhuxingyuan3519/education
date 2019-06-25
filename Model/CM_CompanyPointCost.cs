using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
  /// <summary>
  /// VIP学员数量流转表
  /// </summary>
    [EnitityMapping(TableName = "CM_CompanyPointCost")]
    public class CM_CompanyPointCost
    {

        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string CompanyCode { get; set; }
        public int CostCount { get; set; }
        public string MID { get; set; }
        public string ToMID { get; set; }
        public string MType { get; set; }
        public string FromCompany { get; set; }
        public string ToCompany { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        /// <summary>
        /// 1-充值端口，2-消耗端口
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }

    }
}

