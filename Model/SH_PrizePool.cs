using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "SH_PrizePool")]
    public class SH_PrizePool
    {
        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Id { get; set; }
        /// <summary>
        /// 奖金池名称
        /// </summary>
        public string PoolName { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark { get; set; }
        public int Sort { get; set; }
        public bool IsDeleted { get; set; }
    }
}
