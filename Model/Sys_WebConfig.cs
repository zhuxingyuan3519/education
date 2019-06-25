using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "Sys_WebConfig")]
    public class Sys_WebConfig
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        public string Value { get; set; }
        public bool Status { get; set; }
        /// <summary>
        /// 是否公共部分
        /// </summary>
        public bool IsCommonSet { get; set; }
        /// <summary>
        /// 是否加密存储
        /// </summary>
        public string Encry { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 1-上传图片按钮。2-redio单选框
        /// </summary>
        public int IsImg { get; set; }

    }
}
