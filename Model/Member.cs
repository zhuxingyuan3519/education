using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
using MethodHelper;
namespace Model
{
    //Member
    [EnitityMapping(TableName = "Member")]
    public class Member
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string ID { get; set; }
        public string MID { get; set; }
        public string MName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Zone { get; set; }
        public string Town { get; set; }
        /// <summary>
        /// 学员注册时候的所在年级
        /// </summary>
        public string RegistPointer { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Bank { get; set; }
        /// <summary>
        /// 作为机构的机构代码
        /// </summary>
        public string Branch { get; set; }
        public string BankNumber { get; set; }
        public string BankCardName { get; set; }
        public string Password { get; set; }
        public string SecPsd { get; set; }
        public string MTJ { get; set; }
        public DateTime MCreateDate { get; set; }
        public DateTime? MDate { get; set; }
        public bool MState { get; set; }
        public bool IsClose { get; set; }
        public bool IsClock { get; set; }
        /// <summary>
        /// 用户角色，角色之间用|隔开，一个人可以有多个角色
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 会员是否有查看规划表的权限，有：1。无：0
        /// </summary>
        public string Salt { get; set; }
        public string ThrPsd { get; set; }
        public string NumID { get; set; }
        public string QQ { get; set; }
        public string Weichat { get; set; }
        /// <summary>
        /// 代理商归属与哪个O单商，（Member表Id，默认是公司0）
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 游客权限是否具有体现功能
        /// </summary>
        public int ReadNoticeId { get; set; }
        /// <summary>
        /// 代理商的代理地区；如果是省市区县这种地域模式，就用地址表中的Id，如果一二三级这种模式，就是20，30，40这种固定的数字
        /// 目的是为了跟省市区县这种地域模式保持一致
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 绑定的老师
        /// </summary>
        public string ParentTrade { get; set; }
        /// <summary>
        /// 0-停止发放代言红包。1-开启发放代言红包
        /// </summary>
        public string IsFH { get; set; }
        /// <summary>
        /// 学员总数量（针对机构）
        /// </summary>
        public int TradePoints { get; set; }
        /// <summary>
        /// 学员剩余数量（针对机构）
        /// </summary>
        public int LeaveTradePoints { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string YunPay { get; set; }
        public string AliPay { get; set; }
        public string WeixinPay { get; set; }
        /// <summary>
        /// 归属与哪个代理商
        /// </summary>
        public string Agent { get; set; }
        /// <summary>
        /// 作为机构的缴费金额
        /// </summary>
        public decimal PointMoney { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal MSH { get; set; }
        /// <summary>
        /// TPay
        /// </summary>
        public decimal MJB { get; set; }
        /// <summary>
        /// VPay
        /// </summary>
        public decimal MVB { get; set; }
        /// <summary>
        /// 用户积分
        /// </summary>

        public decimal MOV { get; set; }
        /// <summary>
        /// 代理商使用的角色类型，分为1、2；
        /// 1就是省市县代理商模式，2就是一级二级三级代理商模式
        /// </summary>
        public int UseRoleType { get; set; }
        /// <summary>
        /// 服务中心的名字
        /// </summary>
        public string WelfareID { get; set; }
        /// <summary>
        ///训练权限
        /// </summary>
        public string Learns { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Father { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string FatherTel { get; set; }
        /// <summary>
        /// 信息是否都已完善，1-是
        /// </summary>
        public string Mather { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MatherTel { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string OtherTel { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 使用开始日
        /// </summary>
        public DateTime? UseBeginTime { get; set; }
        /// <summary>
        /// 使用到期日
        /// </summary>
        public DateTime? UseEndTime { get; set; }


        [EnitityMapping(ColumnType = "IGNORE")]
        public Sys_Role Role
        {
            get
            {
                List<Sys_Role> list = CacheHelper.Get<List<Sys_Role>>("Sys_Role");
                if(list == null)
                {
                    list = CommonBase.GetList<Sys_Role>("");
                    CacheHelper.Insert("Sys_Role", list);
                }
                return list.Find(c => c.Code == this.RoleCode);
            }
        }

        [EnitityMapping(ColumnType = "IGNORE")]
        public List<Sys_RolePower> PowerList
        {
            get
            {
                List<Sys_RolePower> list = null;
                string sqlWhere = "RoleCode='" + this.RoleCode + "'";
                if(this.RoleCode != "Manage")
                    sqlWhere += " and MID='" + this.ID + "'";
                list = CommonBase.GetList<Sys_RolePower>(sqlWhere);
                return list;
            }
        }
        /// <summary>
        /// 测评次数，一次提交就累加一次
        /// </summary>
        public int SendRedBagCount { get; set; }
        /// <summary>
        /// 冻结金额(未激活金额)
        /// </summary>
        public decimal NoActiveMoney { get; set; }
        /// <summary>
        /// 是否发放推荐奖励
        /// </summary>
        public string NoFHPool { get; set; }

    }
}

