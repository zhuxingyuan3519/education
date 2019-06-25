using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_PlanRateConfig
    [EnitityMapping(TableName = "CM_PlanRateConfig")]
    public class CM_PlanRateConfig
    {
        [EnitityMapping(ColumnType = "KEY")]
        public int Id
        { get; set; }
        public int Company
        { get; set; }


        public string Code { get; set; }
        /// <summary>
        /// PlanHeaderId
        /// </summary>		
        public string PlanHeaderId
        { get; set; }
        public decimal Rate
        { get; set; }
        /// <summary>
        /// 比列
        /// </summary>
        public decimal RatePercent
        { get; set; }

        public int Status
        { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark
        { get; set; }
        [EnitityMapping(ColumnType = "IGNORE")]
        public decimal ExpenseMoney
        { get; set; }

    }
}

