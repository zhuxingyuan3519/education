using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "SH_RedBagHeaderLog")]
    public class SH_RedBagHeaderLog
    {
        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 发放会员的iD
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 发放会员的cide
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 红包类型1-公司红包，2-个人红包
        /// </summary>
        public int RedType { get; set; }
        /// <summary>
        /// 红包个数
        /// </summary>
        public int RedBagCount { get; set; }
        public decimal RedBagMoney { get; set; }
        public DateTime LogDate { get; set; }
   
        /// <summary>
        /// Remark
        /// </summary>		

        public string Remark { get; set; }
    
    }
}
