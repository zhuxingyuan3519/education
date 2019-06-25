using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //Sys_BankInfo
    [EnitityMapping(TableName = "Sys_CreditMarket")]
    public class Sys_CreditMarket
    {
        [EnitityMapping(ColumnType = "KEY")]
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }
        public string LinkUrl { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool Status { get; set; }
     

    }
}

