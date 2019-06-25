using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //M_Rank
    [EnitityMapping(TableName = "M_Rank")]
    public class M_Rank
    {


        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }
        /// <summary>
        /// 会员Code
        /// </summary>
        public string MCode { get; set; }
        /// <summary>
        /// 会员的归属上级会员Code
        /// </summary>
        public string PMCode { get; set; }
        /// <summary>
        /// 设置点位，从左到右，1，2，3，4.。。。排列
        /// </summary>
        public int MBD { get; set; }
        /// <summary>
        /// 从跟节点开始，第几层
        /// </summary>
        public int Leavel { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RankTime { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int Sort { get; set; }

    }
}

