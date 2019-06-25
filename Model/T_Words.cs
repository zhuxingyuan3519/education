using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    /// <summary>
    /// 单词词库表
    /// </summary>
    [EnitityMapping(TableName = "T_Words")]
    public class T_Words
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        //[EnitityMapping(ColumnType = "IGNORE")]
        /// <summary>
        /// 1-是基础词库，0-非基础词库
        /// </summary>
        public string Version { get; set; }
        [EnitityMapping(ColumnType = "IGNORE")]
        public string Grade { get; set; }
        [EnitityMapping(ColumnType = "IGNORE")]
        public string Leavel { get; set; }
        [EnitityMapping(ColumnType = "IGNORE")]
        public string Unit { get; set; }
        [EnitityMapping(ColumnType = "IGNORE")]
        public string WIndex { get; set; }
        /// <summary>
        /// 是否是更新
        /// </summary>
        [EnitityMapping(ColumnType = "IGNORE")]
        public bool IsUpdate { get; set; }
        /// <summary>
        /// 英文单词，中间含有‘号的，保存入数据库中替换为@，前台再转换
        /// </summary>
        public string English { get; set; }
        /// <summary>
        /// 熟词相关
        /// </summary>
        public string HotWord { get; set; }
        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic { get; set; }
        /// <summary>
        /// 中文意思
        /// </summary>
        public string Chinese { get; set; }
        /// <summary>
        /// 模块拆分1
        /// </summary>
        public string Module1 { get; set; }
        /// <summary>
        /// 情景联想1
        /// </summary>
        public string Association1 { get; set; }
        public string Module2 { get; set; }
        public string Association2 { get; set; }

        public string Module3 { get; set; }
        public string Association3 { get; set; }


        public string Module4 { get; set; }
        public string Association4 { get; set; }

        public string Remark { get; set; }
        public bool IsDeleted { get; set; }

        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}

