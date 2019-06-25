using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "EN_Video")]
    public class EN_Video
    {
        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string Name { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string CoverImage { get; set; }
        public decimal Size { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 视频格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 视频观看权限
        /// </summary>
        public string Authority { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
