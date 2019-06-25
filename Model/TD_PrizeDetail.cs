using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    [EnitityMapping(TableName = "TD_PrizeDetail")]
    public class TD_PrizeDetail
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public DateTime PrizeTime { get; set; }
        public string MCode { get; set; }
        public string MID { get; set; }
        public bool IsPrize { get; set; }

        public decimal PrizeMoney { get; set; }
        /// <summary>
        /// 红包类型，1-游戏红包，2-代言红包，3-签到红包，4-密令答题红包
        /// </summary>
        public string PrizeType { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public int Sort { get; set; }

    }
}

