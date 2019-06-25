using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_AgingLimit
    [EnitityMapping(TableName = "CM_AgingLimit")]
    public class CM_AgingLimit
    {
        [EnitityMapping(ColumnType = "KEY")]
        public int Id { get; set; }
        public string Code { get; set; }
        public int Category { get; set; }
        public int TotalPeriod { get; set; }
        public int Period { get; set; }
        public decimal PeriodMoney { get; set; }
        public decimal PeriodTakeOffMoney { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int Company { get; set; }

    }
}

