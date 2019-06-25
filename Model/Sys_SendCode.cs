using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_Dict
    [EnitityMapping(TableName = "Sys_SendCode")]
    public class Sys_SendCode
    {
        [EnitityMapping(ColumnType = "KEY")]
        public int Id { get; set; }
        public string Telephone { get; set; }
        public string SendCode { get; set; }
        public bool IsUsed { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime? ValidTime { get; set; }

        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public string Remark { get; set; }
    }
}

