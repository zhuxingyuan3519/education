using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    [EnitityMapping(TableName = "TD_SHMoney_Dict")]
    public class TD_SHMoney_Dict
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
          [EnitityMapping(ColumnType = "IGNORE")]
        public string Remark { get; set; }

    }
}

