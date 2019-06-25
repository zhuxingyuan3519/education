using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_Company
    [EnitityMapping(TableName = "CM_Company")]
    public class CM_Company
    {


        [EnitityMapping(ColumnType = "KEY")]
        public int Id { get; set; }
        public string ParentCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Zone { get; set; }
        public string Tel { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public string CName { get; set; }
        public int MaxCardCount { get; set; }
        public bool IsPersonal { get; set; }
        public int AddCompanyCount { get; set; }
        public int AreaId { get; set; }
        public string ParentTrade { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}

