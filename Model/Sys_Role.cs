using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 已废弃，不用
    /// </summary>
    [EnitityMapping(TableName = "Sys_Role")]
    public class Sys_Role
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        public string Name { get; set; }
        public int Status { get; set; }
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 缴费金额
        /// </summary>
        public string Remark { get; set; }
        public int RIndex { get; set; }
        /// <summary>
        ///配送VIP名额数量
        /// </summary>
        public string AreaLeave { get; set; }
        /// <summary>
        /// O单商ID，以后可能会用到
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 直接推荐几个人可以享有相关晋升及相应奖励权益（注意，必须是直接推荐）
        /// </summary>
        public int RoleType { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 图文介绍
        /// </summary>
        public string Introduce { get; set; }
        /// <summary>
        /// 进入红包池的金额
        /// </summary>
        public decimal ToPrizeMoney { get; set; }
    }
}
