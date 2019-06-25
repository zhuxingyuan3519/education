using DBUtility;
using System;
namespace Model
{
    //微信用户信息表
    [EnitityMapping(TableName = "M_WX_VS_User")]
    public class M_WX_VS_User
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string UserCode { get; set; }
        public string OpenId { get; set; }

       
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Status { get; set; }
        public string Company { get; set; }
        public int Sort { get; set; }

    }
}

