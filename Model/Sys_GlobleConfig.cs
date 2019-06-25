using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "Sys_GlobleConfig")]
    public class Sys_GlobleConfig
    {
        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string Weixin { get; set; }
        public string Contacter { get; set; }

        public string Company { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		

        public string Remark { get; set; }
        /// <summary>
        /// 缴费会员缴费金额
        /// </summary>		

        public string Field1 { get; set; }
        /// <summary>
        /// 体验会员使用期限
        /// </summary>		

        public string Field2 { get; set; }
        /// <summary>
        /// 缴费会员使用期限
        /// </summary>		

        public string Field3 { get; set; }
        /// <summary>
        /// 公司LOGO
        /// </summary>		

        public string Field4 { get; set; }
        /// <summary>
        /// 二维码中间附加的小图片
        /// </summary>		

        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }

        public decimal TXFloat { get; set; }
        public decimal MinTXMoney { get; set; }

        public decimal BaseJifen { get; set; }

    }
}
