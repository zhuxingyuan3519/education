using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "SH_PrizePoolInLog")]
    public class SH_PrizePoolInLog
    {
        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 发放人Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 发放人账号
        /// </summary>		
        public string UserCode { get; set; }
        public string PoolId { get; set; }
        public decimal InMoney { get; set; }
        public DateTime LogDate { get; set; }
        public string Remark { get; set; }
    }
}
