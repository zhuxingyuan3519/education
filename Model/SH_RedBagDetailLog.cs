using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "SH_RedBagDetailLog")]
    public class SH_RedBagDetailLog
    {
        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string HeaderCode { get; set; }
        public string FromUserId { get; set; }
        public string FromUserCode { get; set; }
        public string ToUserId { get; set; }
        public string ToUserCode { get; set; }
        public int RedType { get; set; }
        public bool IsActive { get; set; }

        public decimal RedBagMoney { get; set; }
        /// <summary>
        /// 红包状态，1-已发出，2-红包冻结，3-红包已激活，4-红包已领取
        /// </summary>
        public int Status { get; set; }
        public int FromLevelCount { get; set; }
        public string Remark { get; set; }
        public string Field1 { get; set; }

        public string Field2 { get; set; }
        public string Field3 { get; set; }

        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public DateTime? ReceiveTime { get; set; }
        public DateTime? ActiveTime { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime LogDate { get; set; }

    }
}
