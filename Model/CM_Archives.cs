using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //CM_Archives
    [EnitityMapping(TableName = "CM_Archives")]
    public class CM_Archives
    {



        //public int Id { get; set; }
        public string CustomId { get; set; }
        public string ArchiveId { get; set; }
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public DateTime InStoreTime { get; set; }
        public string Tel { get; set; }
        public decimal InStoreMoney { get; set; }
        public decimal InitPlanMoney { get; set; }
        public string Bank { get; set; }
        public string CardID { get; set; }
        public decimal FixedQuota { get; set; }
        public int BillDate { get; set; }
        public int RepayDate { get; set; }
        public DateTime? ValidDate { get; set; }
        public string QueryPwd { get; set; }
        public string TradePwd { get; set; }
        public int HaveCount { get; set; }
        public string CardColor { get; set; }
        public string EBankUserCode { get; set; }
        public string EBankPwd { get; set; }
        public string Education { get; set; }
        public string NumID { get; set; }
        public DateTime? Birthday { get; set; }
        public string Zodiac { get; set; }
        public string HomeAddress { get; set; }
        public string RegisterAddress { get; set; }
        public string HomeTel { get; set; }
        public string Email { get; set; }
        public string RelativeName { get; set; }
        public string RelationShip { get; set; }
        public string RelationTel { get; set; }
        public string OtherRelativeName { get; set; }
        public string OtherRelationShip { get; set; }
        public string OtherRelationTel { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPosition { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyDept { get; set; }
        public bool IsTemporary { get; set; }
        public decimal TempMoney { get; set; }
        public DateTime? TempEndDate { get; set; }
        public bool IsAgingOutLimit { get; set; }
        public bool IsAgingInLimit { get; set; }
        public string SaleMan { get; set; }
        public DateTime? ServiceEndDate { get; set; }
        public decimal ServiceMoney { get; set; }
        public int ServiceCycle { get; set; }
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

