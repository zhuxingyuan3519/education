using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "Sys_RolePower")]
    public class Sys_RolePower
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Id { get; set; }
        public string PrivageId { get; set; }
        public int PrivageType { get; set; }
        public string RoleCode { get; set; }
        public string MID { get; set; }
        public string Company { get; set; }
        public bool IsDeleted { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
    }
}
