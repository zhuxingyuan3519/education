using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //Notice
    [EnitityMapping(TableName = "Notice")]
    public class Notice
    {


        [EnitityMapping(ColumnType = "KEY")]
        public int ID { get; set; }
        public string NTitle { get; set; }
        public string NContent { get; set; }
        public DateTime NCreateTime { get; set; }
        public int NClicks { get; set; }
        public bool NState { get; set; }
        public bool IsFixed { get; set; }
        public int Company { get; set; }
        /// <summary>
        /// 1-公告；2-信用卡常识/提额技术；3-相关新闻；4-信用贷款；5-新手指南；
        /// 6-前台信贷口子，7-前台快卡口子，8-前台提额口子，9-前台贷款口子
        //10-前台信贷口子，11-前台快卡口子，12-前台提额口子，13-前台贷款口子
        /// </summary>
        public int NType { get; set; }
        public string Remark { get; set; }
    }
}

