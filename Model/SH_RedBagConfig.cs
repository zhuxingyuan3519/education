using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "SH_RedBagConfig")]
    public class SH_RedBagConfig
    {
        /// <summary>
        /// Id
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Id { get; set; }
        /// <summary>
        /// 公司发放红包金额
        /// </summary>
        public decimal CompanyReturnMoney { get; set; }
        /// <summary>
        /// 个人红包金额
        /// </summary>
        public decimal PersonalReturnMoney { get; set; }
        /// <summary>
        /// 激活红包需要推荐的缴费会员数量
        /// </summary>
        public int ActiveForTJVIPCount { get; set; }
        /// <summary>
        /// 激活红包需要推荐的注册会员数量
        /// </summary>
        public int ActiveForTJCount { get; set; }
   
        /// <summary>
        /// Remark
        /// </summary>		

        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>		

        public string Field1 { get; set; }
        /// <summary>
        /// 
        /// </summary>		

        public string Field2 { get; set; }
        /// <summary>
        /// 
        /// </summary>		

        public string Field3 { get; set; }
        /// <summary>
        /// 
        /// </summary>		

        public string Field4 { get; set; }
        /// <summary>
        /// 
        /// </summary>		

        public string Field5 { get; set; }
    
    }
}
