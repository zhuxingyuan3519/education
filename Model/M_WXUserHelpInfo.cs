using DBUtility;
using System;
namespace Model
{
    //微信助力信息表
    [EnitityMapping(TableName = "M_WXUserHelpInfo")]
    public class M_WXUserHelpInfo
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string ZLCode { get; set; }
        public string UserCode { get; set; }
        public string OpenId { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Status { get; set; }
        public string Company { get; set; }
        public int Sort { get; set; }

    }
}

