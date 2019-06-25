using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "EN_Course")]
    public class EN_Course
    {
        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; }
        public string Title { get; set; }
        public decimal Fee { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 课程等级，初级-KC0001，高级-KC0002，根据这个课程等级确定具体的分红配置类型
        /// </summary>
        public string Leavel { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
