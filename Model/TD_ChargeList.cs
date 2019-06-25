using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    [EnitityMapping(TableName = "TD_ChargeList")]
    public class TD_ChargeList
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string ChargeType { get; set; }
        public string ChargeMoney { get; set; }
        public string ChargeList { get; set; }
        public bool IsDeleted { get; set; }
        public string Remark { get; set; }
        public int Sort { get; set; }

    }
}

