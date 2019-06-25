using DBUtility;
using System;
namespace Model
{
    //微信用户信息表
    [EnitityMapping(TableName = "M_WXUserInfo")]
    public class M_WXUserInfo
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string OpenId { get; set; }

        public string NickName { get; set; }

        public string Sex { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }
        public string HeadImgUrl { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Status { get; set; }
        public string Company { get; set; }
        public int Sort { get; set; }

    }
}

