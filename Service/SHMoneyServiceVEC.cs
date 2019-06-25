using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Service
{
    /// <summary>
    /// 教育网分红系统
    /// </summary>
    public class SHMoneyServiceVEC
    {
        public static Model.Member FindUpperAgent(Model.Member agentMember, Model.Member member, decimal money, List<CommonObject> listComm, out decimal hasFHTotalMoney)
        {
            Model.Member newAgentModel = new Member();
            List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat").ToList();
            //存储过程查出来该会员的上级推荐关系信息
            SqlParameter[] parameters = {
                    new SqlParameter("@MID", SqlDbType.VarChar, 50),
                    new SqlParameter("@RoleCode", SqlDbType.VarChar, 50)
                 };
            parameters[0].Value = agentMember.ID;
            parameters[1].Value = "";
            DataTable dtUpperAgentMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperAgent");
            //DataRow[] upperArrayNoIncloudOwn = dtUpperAgentMemberTbl.Select("ID<>" + agentMember.ID, "RANK ASC");
            decimal hasFH = 0;
            foreach(DataRow dr in dtUpperAgentMemberTbl.Rows)
            {
                //查看端口是否足够
                string id = dr["ID"].ToString();
                string mid = dr["MID"].ToString();
                int treadPoint = MethodHelper.ConvertHelper.ToInt32(dr["TradePoints"], 0);
                if(treadPoint > 0)
                {
                    newAgentModel.ID = id;
                    newAgentModel.MID = mid;
                    newAgentModel.RoleCode = dr["RoleCode"].ToString();
                    newAgentModel.LeaveTradePoints = MethodHelper.ConvertHelper.ToInt32(dr["LeaveTradePoints"], 0);
                    newAgentModel.TradePoints = MethodHelper.ConvertHelper.ToInt32(dr["TradePoints"], 0);
                    break;
                }
                else
                {
                    //给他分红，分销商分红
                    string roleCode = dr["RoleCode"].ToString();
                    Model.Sys_Role roleModel = CacheService.RoleList.Where(c => c.Code == roleCode).FirstOrDefault();
                    if(roleModel != null)
                    {
                        Model.TD_SHMoney shmoney = shMoneyList.Where(c => c.TJIndex.ToString() == roleModel.AreaLeave).FirstOrDefault();
                        if(shmoney != null)
                        {
                            decimal fhMoney = shmoney.TJFloat * money;
                            if(shmoney.Field3 == "2")
                            {
                                fhMoney = shmoney.TJFloat;
                            }
                            hasFH += fhMoney;
                            #region 添加分红记录
                            TD_FHLog fhLog = new TD_FHLog();
                            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                            fhLog.Company = 0;
                            fhLog.CreatedBy = "SYSTEM";
                            fhLog.CreatedTime = DateTime.Now;
                            fhLog.FHDate = DateTime.Now;
                            fhLog.FHMoney = fhMoney;
                            fhLog.FHRoleCode = roleCode;
                            fhLog.FHType = shmoney.Id.ToString();
                            fhLog.IsDeleted = false;
                            fhLog.FHMCode = mid;
                            fhLog.MID = id.ToString();
                            fhLog.PayCode = member.ID.ToString();
                            fhLog.Status = 1;
                            fhLog.Remark = "会员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;奖励" + shmoney.TJFloat + "元";

                            if(fhLog.FHMoney > 0)
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                            //更新会员的积分
                            decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                            decimal MJB = fhLog.FHMoney - MSH;
                            //积分总额
                            //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MJB", true);
                            //现金
                            if(fhLog.FHMoney > 0)
                                MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                            #endregion

                        }
                    }
                }
            }
            hasFHTotalMoney = hasFH;
            return newAgentModel;
        }

        /// <summary>
        ///  教育网两级分销分红
        /// </summary>
        /// <param name="member">缴费会员</param>
        /// <param name="money">缴费金额</param>
        /// <param name="course">课程model</param>
        /// <param name="listComm"></param>
        public static void TJChangeMoney(Model.Member member, decimal money, Model.EN_Course course, List<CommonObject> listComm)
        {
            string sql = "SELECT * FROM dbo.FUN_CountUpperMember('" + member.ID + "',1,9999)";
            DataTable tjTbl = CommonBase.GetTable(sql);
            List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && c.Code == "KCTJ").ToList();
            if(tjTbl != null && tjTbl.Rows.Count > 0)
            {
                //只查询两级分销
                DataRow[] rankList = tjTbl.Select("MRANK<=2");
                foreach(DataRow dr in rankList)
                {
                    //查找到具体的分红配置
                    TD_SHMoney shmoney = shMoneyList.FirstOrDefault(c => c.TJIndex == Convert.ToInt16(dr["MRANK"]));
                    if(shmoney != null)
                    {
                        decimal fhMoney = shmoney.TJFloat * money;
                        if(shmoney.Field3 == "2")
                        {
                            fhMoney = shmoney.TJFloat;
                        }
                        #region 添加分红记录
                        string fh_remark = "直接推荐学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name;
                        if(shmoney.TJIndex != 1)
                            fh_remark = "间接" + (shmoney.TJIndex - 1).ToString() + "级会员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name;
                        AddFHLog(member, money, fhMoney, dr["MID"].ToString(), dr["Code"].ToString(), dr["RoleCode"].ToString(), fh_remark, shmoney.Remark, shmoney.Code, course.Code, listComm);
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        ///返还金设置
        /// </summary>
        /// <param name="member"></param>
        /// <param name="money"></param>
        /// <param name="tjTbl"></param>
        /// <param name="course"></param>
        /// <param name="listComm"></param>
        public static void ReturnChangeMoney(Model.Member member, decimal money, DataTable tjTbl, Model.EN_Course course, List<CommonObject> listComm)
        {
            if(tjTbl != null && tjTbl.Rows.Count > 0)
            {
                List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && c.Code == "Return").ToList();
                //分红的时候其他额外扣费项目
                List<TD_SHMoney> feeShMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && (c.Code == "KgjUsing" || c.Code == "ServiceUsing" || c.Code == "CompanyUsing")).ToList();
                foreach(DataRow dr in tjTbl.Rows)
                {
                    if(dr["IsFH"].ToString() == "0")
                        continue;
                    //会员角色
                    string roleCode = dr["RoleCode"].ToString();
                    //会员id
                    string id = dr["ID"].ToString();
                    TD_SHMoney shmoney = shMoneyList.FirstOrDefault(c => c.RoleCode == roleCode);
                    if(shmoney != null)
                    {
                        decimal fhMoney = shmoney.TJFloat * money;
                        if(shmoney.Field3 == "2")
                        {
                            fhMoney = shmoney.TJFloat;
                        }
                        #region 添加分红记录
                        //根据级别角色添加扣除费用项目
                        var roleFeeShMoney = feeShMoneyList.Where(c => c.RoleCode == roleCode);
                        string feeRemark = string.Empty;
                        foreach(TD_SHMoney shm in roleFeeShMoney)
                        {
                            decimal feeMoney = shm.TJFloat * fhMoney;
                            if(shm.Field3 == "2")
                                feeMoney = shm.TJFloat;
                            fhMoney -= feeMoney;
                            if(!string.IsNullOrEmpty(feeRemark)) feeRemark += "；";
                            feeRemark += "扣除" + shm.Remark + feeMoney + "元";
                            string fee_fh_remark = "学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name + "，扣除" + dr["MID"].ToString() + "的分红奖励" + feeMoney + "元：" + shm.Remark;
                            AddFHLog(member, money, feeMoney, "admin", "18", "Manage", fee_fh_remark, shm.Remark, shm.Code, course.Code, listComm);
                        }
                        string fh_remark = "学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name + "。" + feeRemark;
                        AddFHLog(member, money, fhMoney, dr["MID"].ToString(), dr["ID"].ToString(), dr["RoleCode"].ToString(), fh_remark, shmoney.Remark, shmoney.Code, course.Code, listComm);
                        #endregion
                    }
                }
            }
        }

        public static void ServiceChangeMoney(Model.Member member, decimal money, DataTable tjTbl, Model.EN_Course course, List<CommonObject> listComm)
        {
            if(tjTbl != null && tjTbl.Rows.Count > 0)
            {
                List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && c.Code == "Service").ToList();
                if(shMoneyList.Count <= 0)
                    return;
                //查找到服务中心
                //这颗树的根结点：
                string rootNode = tjTbl.Rows[tjTbl.Rows.Count - 1]["ID"].ToString();
                //查找到这课树的所有的
                string sqltd = " SELECT t2.* FROM dbo.FUN_CountTDMemberWithRank('" + rootNode + "',1,9999) t1 LEFT JOIN dbo.Member t2 ON t1.ID=t2.ID where t1.IsService='service'";
                DataTable tjTDTbl = CommonBase.GetTable(sqltd);
                if(tjTDTbl != null && tjTDTbl.Rows.Count > 0)
                {
                    DataRow dr = tjTDTbl.Rows[0];
                    //if(dr["IsFH"].ToString() == "0")
                    //    return;
                    //会员角色
                    string roleCode = dr["RoleCode"].ToString();
                    //会员id
                    string id = dr["ID"].ToString();
                    TD_SHMoney shmoney = shMoneyList[0];
                    if(shmoney != null)
                    {
                        decimal fhMoney = shmoney.TJFloat * money;
                        if(shmoney.Field3 == "2")
                        {
                            fhMoney = shmoney.TJFloat;
                        }
                        #region 添加分红记录
                        //根据级别角色添加扣除费用项目
                        string fh_remark = "学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name + "。奖励" + fhMoney;
                        AddFHLog(member, money, fhMoney, dr["MID"].ToString(), dr["ID"].ToString(), dr["RoleCode"].ToString(), fh_remark, shmoney.Remark, shmoney.Code, course.Code, listComm);
                        #endregion
                    }
                }
            }
        }


        /// <summary>
        /// 教育网培训机构分红
        /// </summary>
        /// <param name="member">缴费学员</param>
        /// <param name="money">金额</param>
        /// <param name="course">学员选择课程</param>
        /// <param name="trainingMember">培训机构</param>
        /// <param name="listComm"></param>
        public static void TrainingChangeMoney(Model.Member member, decimal money, Model.EN_Course course, Model.Member trainingMember, List<CommonObject> listComm)
        {
            if(trainingMember != null)
            {
                List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && c.Code == "KCTraining").ToList();
                //查找到具体的分红配置
                TD_SHMoney shmoney = shMoneyList.FirstOrDefault();
                if(shmoney != null)
                {
                    decimal fhMoney = shmoney.TJFloat * money;
                    if(shmoney.Field3 == "2")
                    {
                        fhMoney = shmoney.TJFloat;
                    }
                    #region 添加分红记录
                    string fh_remark = "学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name;
                    AddFHLog(member, money, fhMoney, trainingMember.MID, trainingMember.ID, trainingMember.RoleCode, fh_remark, shmoney.Remark, shmoney.Code, course.Code, listComm);
                    #endregion
                }
            }
        }
        /// <summary>
        /// 团队运营费用支出
        /// </summary>
        /// <param name="member"></param>
        /// <param name="money"></param>
        /// <param name="course"></param>
        /// <param name="trainingMember"></param>
        /// <param name="listComm"></param>
        public static void OperateChangeMoney(Model.Member member, decimal money, Model.EN_Course course, Model.Member trainingMember, List<CommonObject> listComm)
        {
            if(trainingMember != null)
            {
                List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && c.Code == "KCOperate").ToList();
                //查找到具体的分红配置
                TD_SHMoney shmoney = shMoneyList.FirstOrDefault();
                if(shmoney != null)
                {
                    decimal fhMoney = shmoney.TJFloat * money;
                    if(shmoney.Field3 == "2")
                    {
                        fhMoney = shmoney.TJFloat;
                    }
                    #region 添加分红记录
                    string fh_remark = "学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name;
                    AddFHLog(member, money, fhMoney, trainingMember.MID, trainingMember.ID, trainingMember.RoleCode, fh_remark, shmoney.Remark, shmoney.Code, course.Code, listComm);
                    #endregion
                }
            }
        }


        /// <summary>
        /// 云联惠费用支出
        /// </summary>
        /// <param name="member"></param>
        /// <param name="money"></param>
        /// <param name="course"></param>
        /// <param name="trainingMember"></param>
        /// <param name="listComm"></param>
        public static void YunlianhuiChangeMoney(Model.Member member, decimal money, Model.EN_Course course, Model.Member trainingMember, List<CommonObject> listComm)
        {
            if(trainingMember != null)
            {
                List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && c.Code == "KCYunlianhui").ToList();
                //查找到具体的分红配置
                TD_SHMoney shmoney = shMoneyList.FirstOrDefault();
                if(shmoney != null)
                {
                    decimal fhMoney = shmoney.TJFloat * money;
                    if(shmoney.Field3 == "2")
                    {
                        fhMoney = shmoney.TJFloat;
                    }
                    #region 添加分红记录
                    string fh_remark = "学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name;
                    AddFHLog(member, money, fhMoney, trainingMember.MID, trainingMember.ID, trainingMember.RoleCode, fh_remark, shmoney.Remark, shmoney.Code, course.Code, listComm);
                    #endregion
                }
            }
        }

        public static void AgentChangMoney(Model.Member member, decimal money, Model.EN_Course course, List<CommonObject> listComm)
        {
            string sql = "SELECT * FROM dbo.FUN_CountUpperMember('" + member.ID + "',1,9999) WHERE RoleCode<>'Member' and RoleCode<>'Training'";
            DataTable tjTbl = CommonBase.GetTable(sql);
            List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Field4 == course.Code && c.Code == "KCAgent").ToList();
            if(tjTbl != null && tjTbl.Rows.Count > 0)
            {
                foreach(DataRow dr in tjTbl.Rows)
                {
                    string agentRole = dr["RoleCode"].ToString();
                    #region 对创业合伙人分红
                    if(agentRole == "1F") //对创业合伙人分红
                    {
                        AgentChangeMoneyHelper(shMoneyList, agentRole, money, member, dr, course, listComm);
                    }
                    #endregion
                    #region 对区县级代理商分红
                    else if(agentRole == "2F")
                    {
                        //没有的话区县级代理商也把创业合伙人的钱拿掉
                        #region 查看这上级推荐列表中有没有创业合伙人
                        if(tjTbl.Select("RoleCode='1F'").Length == 0)
                        {
                            string fh_remark_more_money = string.Empty;
                            decimal fhMoney = 0;
                            string fhTypeId = "", shmoneyCode = "";
                            SetAgentFHMoney(shMoneyList, agentRole, money, 2, 1, out fh_remark_more_money, out fhMoney, out fhTypeId, out shmoneyCode);
                            string fh_remark = "团队学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name + "的" + fh_remark_more_money + "收益";
                            AddFHLog(member, money, fhMoney, dr["MID"].ToString(), dr["Code"].ToString(), dr["RoleCode"].ToString(), fh_remark, fhTypeId, shmoneyCode, course.Code, listComm);
                        }
                        else
                        {
                            AgentChangeMoneyHelper(shMoneyList, agentRole, money, member, dr, course, listComm);
                        }
                        #endregion
                    }
                    #endregion

                    #region 对市级级代理商分红
                    else if(agentRole == "3F")
                    {
                        //没有的话区县级代理商也把创业合伙人的钱拿掉
                        #region 查看这上级推荐列表中有没有创业合伙人和区县级代理商
                        if(tjTbl.Select("RoleCode='1F'").Length == 0 && tjTbl.Select("RoleCode='2F'").Length == 0)
                        {
                            string fh_remark_more_money = string.Empty;
                            decimal fhMoney = 0;
                            string fhTypeId = "", shmoneyCode = "";
                            SetAgentFHMoney(shMoneyList, agentRole, money, 3, 1, out fh_remark_more_money, out fhMoney, out fhTypeId, out shmoneyCode);
                            string fh_remark = "团队学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name + "的" + fh_remark_more_money + "收益";
                            AddFHLog(member, money, fhMoney, dr["MID"].ToString(), dr["Code"].ToString(), dr["RoleCode"].ToString(), fh_remark, fhTypeId, shmoneyCode, course.Code, listComm);
                        }
                        else if(tjTbl.Select("RoleCode='1F'").Length > 0 && tjTbl.Select("RoleCode='2F'").Length == 0)
                        {
                            string fh_remark_more_money = string.Empty;
                            decimal fhMoney = 0;
                            string fhTypeId = "", shmoneyCode = "";
                            SetAgentFHMoney(shMoneyList, agentRole, money, 3, 2, out fh_remark_more_money, out fhMoney, out fhTypeId, out shmoneyCode);
                            string fh_remark = "团队学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name + "的" + fh_remark_more_money + "收益";
                            AddFHLog(member, money, fhMoney, dr["MID"].ToString(), dr["Code"].ToString(), dr["RoleCode"].ToString(), fh_remark, fhTypeId, shmoneyCode, course.Code, listComm);
                        }
                        else
                        {
                            AgentChangeMoneyHelper(shMoneyList, agentRole, money, member, dr, course, listComm);
                        }
                        #endregion
                    }
                    #endregion
                    if(course.Leavel == "KC0001")
                    {
                        break;
                    }
                }
            }
        }

        public static void SetAgentFHMoney(List<TD_SHMoney> shMoneyList, string agentRole, decimal money, int maxTJIndex, int minTJIndex, out string fh_remark_more_money, out decimal fhMoney, out string fhTypeId, out string fshmoneyCode)
        {
            string fh_remark_more_money_copy = string.Empty;
            decimal fhMoney_copy = 0;
            string fhTypeId_copy = string.Empty, fshmoneyCode_copy = string.Empty;
            foreach(TD_SHMoney moreSHMoney in shMoneyList.Where(c => (c.TJIndex >= minTJIndex && c.TJIndex <= maxTJIndex)))
            {
                if(moreSHMoney.RoleCode.Contains(agentRole))
                {
                    fhTypeId_copy = moreSHMoney.Remark;
                    fshmoneyCode_copy = moreSHMoney.Code;
                }
                if(moreSHMoney.Field3 == "2")
                {
                    if(string.IsNullOrEmpty(fh_remark_more_money_copy))
                        fh_remark_more_money_copy += moreSHMoney.TJFloat;
                    else
                        fh_remark_more_money_copy += "+" + moreSHMoney.TJFloat;
                    fhMoney_copy += moreSHMoney.TJFloat;
                }
                else
                {
                    if(string.IsNullOrEmpty(fh_remark_more_money_copy))
                        fh_remark_more_money_copy += moreSHMoney.TJFloat * 100 + "%";
                    else
                        fh_remark_more_money_copy += "+" + moreSHMoney.TJFloat * 100 + "%";
                    fhMoney_copy += moreSHMoney.TJFloat * money;
                }
            }
            fh_remark_more_money = fh_remark_more_money_copy;
            fhMoney = fhMoney_copy;
            fhTypeId = fhTypeId_copy;
            fshmoneyCode = fshmoneyCode_copy;
        }

        public static void AgentChangeMoneyHelper(List<TD_SHMoney> shMoneyList, string agentRole, decimal money, Member member, DataRow dr, EN_Course course, List<CommonObject> listComm)
        {
            TD_SHMoney shmoney = shMoneyList.FirstOrDefault(c => c.RoleCode.Contains(agentRole));
            if(shmoney != null)
            {
                string fhomney_remark = string.Empty;
                decimal fhMoney = shmoney.TJFloat * money;
                fhomney_remark = shmoney.TJFloat * 100 + "%";
                if(shmoney.Field3 == "2")
                {
                    fhMoney = shmoney.TJFloat;
                    fhomney_remark = shmoney.TJFloat.ToString();
                }
                string fh_remark = "团队学员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;报名" + course.Name + "的" + fhomney_remark + "收益";
                AddFHLog(member, money, fhMoney, dr["MID"].ToString(), dr["Code"].ToString(), dr["RoleCode"].ToString(), fh_remark, shmoney.Remark, shmoney.Code, course.Code, listComm);
            }
        }

        /// <summary>
        /// 添加分红记录
        /// </summary>
        /// <param name="payMember">付款会员</param>
        /// <param name="payMoney">付款金额</param>
        /// <param name="fhMoney">分红的金额</param>
        /// <param name="fhMID">分红会员的MID</param>
        /// <param name="fhMemberID">分红会员的ID主键</param>
        /// <param name="fhRoleCode">分红会员的角色</param>
        /// <param name="fhRemark">分红说明</param>
        /// <param name="fhType">分红类型</param>
        /// <param name="listComm"></param>
        /// <param name="fhLogOut">返回分红Model实例</param>
        public static void AddFHLog(Model.Member payMember, decimal payMoney, decimal fhMoney, string fhMID, string fhMemberID, string fhRoleCode, string fhRemark, string fhType, string shMoneyCode, string productCode, List<CommonObject> listComm, out TD_FHLog fhLogOut)
        {
            //添加分红记录
            TD_FHLog fhLog = new TD_FHLog();
            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
            fhLog.Company = 0;
            fhLog.CreatedBy = "SYSTEM";
            fhLog.CreatedTime = DateTime.Now;
            fhLog.FHDate = DateTime.Now;
            fhLog.FHMoney = fhMoney;
            fhLog.FHRoleCode = fhRoleCode;
            fhLog.FHType = fhType;
            fhLog.IsDeleted = false;
            fhLog.FHMCode = fhMID;
            fhLog.MID = fhMemberID;
            fhLog.PayCode = payMember.ID.ToString();
            fhLog.Status = 1;
            fhLog.Remark = fhRemark;
            fhLog.SHMoneyCode = shMoneyCode;
            fhLog.ProductCode = productCode;
            if(fhLog.FHMoney > 0)
            {
                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
            }
            fhLogOut = fhLog;
        }
        public static void AddFHLog(Model.Member payMember, decimal payMoney, decimal fhMoney, string fhMID, string fhMemberID, string fhRoleCode, string fhRemark, string fhType, string shMoneyCode, string productCode, List<CommonObject> listComm)
        {
            //添加分红记录
            TD_FHLog fhLog = new TD_FHLog();
            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
            fhLog.Company = 0;
            fhLog.CreatedBy = "SYSTEM";
            fhLog.CreatedTime = DateTime.Now;
            fhLog.FHDate = DateTime.Now;
            fhLog.FHMoney = fhMoney;
            fhLog.FHRoleCode = fhRoleCode;
            fhLog.FHType = fhType;
            fhLog.SHMoneyCode = shMoneyCode;
            fhLog.ProductCode = productCode;
            fhLog.IsDeleted = false;
            fhLog.FHMCode = fhMID;
            fhLog.MID = fhMemberID;
            fhLog.PayCode = payMember.ID.ToString();
            fhLog.Status = 1;
            fhLog.Remark = fhRemark;
            if(fhLog.FHMoney > 0)
            {
                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                //更新会员的积分
                decimal MSH = fhLog.FHMoney * 1;
                MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
            }
        }
        /// <summary>
        /// 管理员（O单商）端口消耗（减少一个）
        /// </summary>
        /// <param name="payMember">缴费会员</param>
        /// <param name="payMoney">支付金额</param>
        /// <param name="listComm"></param>
        public static void AdminTradePointReduce(Model.Member payMember, decimal payMoney, List<CommonObject> listComm)
        {
            Model.Member OMember = DBUtility.CommonBase.GetList<Model.Member>("RoleCode='Admin'").FirstOrDefault();
            if(OMember != null)
            {
                OMember.TradePoints = OMember.TradePoints - 1;
                CommonBase.Update<Model.Member>(OMember, new string[] { "TradePoints" }, listComm);
                LogService.Log(OMember, "10", payMember.MID + "在线支付" + payMoney + "元成功申请VIP会员，消耗1个端口", listComm);
            }
        }
        /// <summary>
        /// 分销提成；代理商的奖励
        /// </summary>
        /// <param name="member"></param>
        /// <param name="money"></param>
        /// <param name="listComm"></param>
        public static void AgentChangMoney(Model.Member member, decimal money, List<CommonObject> listComm)
        {
            //根据会员所在区域，查询出省市县的代理商进行奖励
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)
                 };
            string areaId = "-1";
            if(string.IsNullOrEmpty(member.Zone))
            {
                if(string.IsNullOrEmpty(member.City))
                {
                    if(!string.IsNullOrEmpty(member.Province))
                    {
                        areaId = member.Province;
                    }
                }
                else
                {
                    areaId = member.City;
                }
            }
            else
            {
                areaId = member.Zone;
            }
            parameters[0].Value = areaId;

            //得到地址信息表
            DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_GetUpperAddress");
            //找到分红配置表中的分红配置表，找到，AgentFloat的
            //20-省级，30-市级；40-区县级；50-乡镇级
            List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat").ToList();
            foreach(TD_SHMoney shmoney in shMoneyList)
            {
                //查询到对应的区域级别
                DataRow[] rows = dtUpperAddressTbl.Select("Level='" + shmoney.TJIndex + "'");
                if(rows.Length > 0)
                {
                    //再查询到该地区的代理商
                    //省市县区的代理商角色
                    string agenRoleList = "'CityA','CityB','ProvinceA','ProvinceA','ZoneA','ZoneA'";
                    Model.Member agentMember = CommonBase.GetList<Model.Member>("IsDeleted=0 and AreaId=" + rows[0]["ID"].ToString() + " and Field5 in (" + agenRoleList + ")").FirstOrDefault();
                    if(agentMember != null)
                    {
                        decimal fhMoney = shmoney.TJFloat * money;
                        if(shmoney.Field3 == "2")
                        {
                            fhMoney = shmoney.TJFloat;
                        }
                        TD_FHLog fhLog = new TD_FHLog();
                        fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                        fhLog.Company = 0;
                        fhLog.CreatedBy = "SYSTEM";
                        fhLog.CreatedTime = DateTime.Now;
                        fhLog.FHDate = DateTime.Now;
                        fhLog.FHMoney = fhMoney;
                        fhLog.FHRoleCode = agentMember.RoleCode;
                        fhLog.FHType = shmoney.Id.ToString();
                        fhLog.IsDeleted = false;
                        fhLog.MID = agentMember.ID.ToString();
                        fhLog.PayCode = member.ID.ToString();
                        fhLog.Status = 1;
                        fhLog.Remark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元的" + shmoney.TJFloat * 100 + "%奖励";
                        CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                        //更新会员的积分
                        decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                        decimal MJB = fhLog.FHMoney - MSH;
                        ////积分总额
                        //MemberService.UpdateMoney(listComm, fhLog.MID, MJB.ToString(), "MJB", true);
                        //可提现积分总额
                        MemberService.UpdateMoney(listComm, fhLog.MID, MSH.ToString(), "MSH", true);
                    }
                }
            }
        }


        private static void AgentFHMoneyAndFHRemark(Model.Member member, decimal money, List<TD_SHMoney> shMoneyList, int leavel, out int fhTypeId, out decimal fhMoney, out string fhRemark)
        {
            TD_SHMoney shmoney = shMoneyList.Where(c => c.TJIndex == leavel).FirstOrDefault();
            if(shmoney != null)
            {
                fhTypeId = shmoney.Id;
            }
            else
                fhTypeId = 0;
            fhMoney = shMoneyList.Sum(c => c.TJFloat);
            string moneyAdd = string.Empty;
            foreach(var obj in shMoneyList)
            {
                moneyAdd += obj.TJFloat + "+";
            }
            moneyAdd = moneyAdd.TrimEnd('+');
            fhRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyAdd + "元";
        }
        private static void AgentMTJFHMoneyAndFHRemark(Model.Member member, decimal money, List<TD_SHMoney> MTJAgentMoneyList, string leavel, out int tjFHTypeID, out decimal tjFHMoney, out string tjFHRemark)
        {
            TD_SHMoney shmoney = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault();
            if(shmoney != null)
            { tjFHTypeID = shmoney.Id; }
            else
                tjFHTypeID = 0;
            tjFHMoney = MTJAgentMoneyList.Sum(c => c.TJFloat);
            string moneyMtjAdd = string.Empty;
            foreach(var obj in MTJAgentMoneyList)
            {
                moneyMtjAdd += obj.TJFloat + "+";
            }
            moneyMtjAdd = moneyMtjAdd.TrimEnd('+');
            tjFHRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyMtjAdd + "元";
        }

        /// <summary>
        /// 洛胜代理商奖励：代理商获得XX元奖励，该会员占用代理商的名额
        /// </summary>
        /// <param name="member">缴费会员</param>
        /// <param name="money">缴费金额</param>
        /// <param name="listComm"></param>
        public static void AgentChangMoneyForKgj00(Model.Member member, decimal money, List<CommonObject> listComm)
        {
            #region 2--一二三级
            //1--查询代理商等级的树形结构
            SqlParameter[] parameters = {
                                   new SqlParameter("@MID", SqlDbType.Int),
                                   new SqlParameter("@RoleCode", SqlDbType.VarChar)
                         };
            parameters[0].Value = member.Agent;
            parameters[1].Value = "";
            //得到上级以上代理商列表
            DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperAgent");
            //如果最近的代理商
            //for (int i = 0; i <= dtUpperAddressTbl.Rows.Count; i++)
            //{
            //    string agentRoleCode = dtUpperAddressTbl.Rows[i]["RoleCode"].ToString();
            //    if (i == 0 && agentRoleCode == "2F")
            //    {
            //        //把三级代理商的提成也拿了

            //    }
            //}



            //查看最近的一个代理商
            //foreach (DataRow dr in dtUpperAddressTbl.Rows)
            for(int i = 0; i < dtUpperAddressTbl.Rows.Count; i++)
            {
                DataRow dr = dtUpperAddressTbl.Rows[i];
                //查看角色
                string roleCode = dr["RoleCode"].ToString();
                string memberId = dr["ID"].ToString();
                string mid = dr["MID"].ToString();
                string IsFH = dr["IsFH"].ToString();
                if(roleCode == "1F" || roleCode == "2F" || roleCode == "3F" || roleCode == "City" || roleCode == "Province" || roleCode == "Zone")
                {
                    //收费端口数量
                    int tradePoints = MethodHelper.ConvertHelper.ToInt32(dr["TradePoints"], 0);
                    if(IsFH != "0") //分红状态为0，禁用了该代理商的分红
                    {


                        if(tradePoints >= 1)
                        {
                            //扣除代理商端口数量(跟扣除会员钱一样，只不过字段不一样)
                            MemberService.UpdateMoney(listComm, memberId, "1", "TradePoints", false);
                            //代理商获得XX元
                            //获得的奖金为总费用-直接推荐奖励
                            //直推奖
                            decimal tjMoney = money;
                            TD_SHMoney tjSHMoney = CacheService.SHMoneyList.Where(c => c.Code == "TJFloat" && c.TJIndex == 1).FirstOrDefault();
                            if(tjSHMoney != null)
                                tjMoney = tjSHMoney.TJFloat;
                            decimal leaveMoney = money - tjMoney;

                            //执行代理商分红
                            //插入代理商分红记录表
                            TD_FHLog fhLog = new TD_FHLog();
                            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                            fhLog.Company = 0;
                            fhLog.CreatedBy = "SYSTEM";
                            fhLog.CreatedTime = DateTime.Now;
                            fhLog.FHDate = DateTime.Now;
                            fhLog.FHMoney = leaveMoney;
                            fhLog.FHRoleCode = roleCode;
                            fhLog.FHMCode = mid;
                            fhLog.FHType = "代理商分红";
                            fhLog.IsDeleted = false;
                            fhLog.MID = memberId;
                            fhLog.PayCode = member.ID.ToString();
                            fhLog.Status = 1;
                            fhLog.Remark = member.MID + "升级为VIP会员，代理商分红" + fhLog.FHMoney;
                            if(fhLog.FHMoney > 0)
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                            //更新会员的账户金额
                            decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                            decimal MJB = fhLog.FHMoney - MSH;
                            //可提现积分总额
                            if(fhLog.FHMoney > 0)
                                MemberService.UpdateMoney(listComm, fhLog.MID, MSH.ToString(), "MSH", true);
                            break;
                        }
                        else //如果这个代理商端口不够，就继续往上找代理商
                        {
                            List<TD_SHMoney> listSHMoney = new List<TD_SHMoney>();
                            int areaId = Convert.ToInt16(dr["AreaId"].ToString());
                            //没有端口了，就发服务补贴
                            if(i == 0 && roleCode == "2F")
                            {
                                //把3F的提成也拿了
                                listSHMoney = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat" && c.TJIndex >= areaId).ToList();
                            }
                            //没有端口了，就发服务补贴
                            if(i == 0 && roleCode == "1F")
                            {
                                //把2F,3F的提成也拿了
                                listSHMoney = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat" && c.TJIndex >= areaId).ToList();
                            }

                            //没有端口了，就发服务补贴
                            if(i == 1 && roleCode == "1F") //中间隔了一个代理商（服务中心），那么合伙人就把服务中心的钱都拿了
                            {
                                string lastAgentRoleCode = dtUpperAddressTbl.Rows[i - 1]["RoleCode"].ToString();
                                if(lastAgentRoleCode == "3F")
                                {
                                    int lastAreaId = Convert.ToInt16(dtUpperAddressTbl.Rows[i - 1]["AreaId"].ToString());
                                    //把2F的提成也拿了
                                    listSHMoney = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat" && c.TJIndex >= areaId & c.TJIndex < lastAreaId).ToList();
                                }

                            }

                            //代理商获得XX元
                            TD_SHMoney shMoney = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat" && c.RoleCode.Contains(roleCode)).FirstOrDefault();
                            if(shMoney != null && !listSHMoney.Contains(shMoney))
                            {
                                listSHMoney.Add(shMoney);
                            }
                            //分红金额
                            decimal total_agent_fh_money = 0; string fh_money_string = string.Empty;
                            foreach(TD_SHMoney shm in listSHMoney)
                            {
                                total_agent_fh_money += shm.TJFloat;
                                if(!string.IsNullOrEmpty(fh_money_string))
                                {
                                    fh_money_string += "+" + shm.TJFloat;
                                }
                                else
                                {
                                    fh_money_string += shm.TJFloat;
                                }
                            }

                            //执行代理商分红
                            //插入代理商分红记录表
                            TD_FHLog fhLog = new TD_FHLog();
                            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                            fhLog.Company = 0;
                            fhLog.CreatedBy = "SYSTEM";
                            fhLog.CreatedTime = DateTime.Now;
                            fhLog.FHDate = DateTime.Now;
                            fhLog.FHMoney = total_agent_fh_money;
                            fhLog.FHRoleCode = roleCode;
                            fhLog.FHMCode = mid;
                            fhLog.FHType = shMoney.Id.ToString();
                            fhLog.IsDeleted = false;
                            fhLog.MID = memberId;
                            fhLog.PayCode = member.ID.ToString();
                            fhLog.Status = 1;
                            fhLog.Remark = member.MID + "升级为VIP会员，因端口消耗完发放服务补贴分红" + fh_money_string;
                            if(fhLog.FHMoney > 0)
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                            //更新会员的账户金额
                            decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                            decimal MJB = fhLog.FHMoney - MSH;
                            //可提现积分总额
                            if(fhLog.FHMoney > 0)
                                MemberService.UpdateMoney(listComm, fhLog.MID, MSH.ToString(), "MSH", true);
                            continue;
                        }
                        //    }
                        //}
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// 注册返现
        /// </summary>
        /// <param name="member"></param>
        /// <param name="money"></param>
        /// <param name="listComm"></param>
        public static void RegistChangeMoney(Model.Member member, decimal money, List<CommonObject> listComm)
        {
            //添加分红记录，update at =2016年11月16日17:20:58，注册即返现30元
            TD_SHMoney shMoney = CacheService.SHMoneyList.Where(c => c.Code == "RegistFloat").FirstOrDefault();
            if(shMoney != null)
            {
                TD_FHLog fhLog = new TD_FHLog();
                fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                fhLog.Company = 0;
                fhLog.CreatedBy = "SYSTEM";
                fhLog.CreatedTime = DateTime.Now;
                fhLog.FHDate = DateTime.Now;
                fhLog.FHMoney = shMoney.TJFloat;
                fhLog.FHRoleCode = member.RoleCode;
                fhLog.FHType = shMoney.Id.ToString();
                fhLog.IsDeleted = false;
                fhLog.MID = member.ID.ToString();
                fhLog.FHMCode = member.MID;
                fhLog.PayCode = "SYSTEM";
                fhLog.Status = 1;
                fhLog.Remark = "在" + DateTime.Now.ToString() + "注册会员的" + fhLog.FHMoney + "元返现";
                //直接给会员发30块钱，update at 2016年11月16日16:32:55
                member.MSH = fhLog.FHMoney;
                if(fhLog.FHMoney > 0)
                    CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                if(fhLog.FHMoney > 0)
                    MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
            }
        }
        /// <summary>
        /// 申请成为省市区县/一二三级代理商的会员推荐人分红提成
        /// </summary>
        /// <param name="member">得到分红的人</param>
        /// <param name="applyMember">申请人</param>
        ///  <param name="applyAgentLeavel">申请的分销商级别</param>
        /// <param name="money"></param>
        /// <param name="listComm"></param>
        public static void ApplyAgentForMTJChangeMoney(Model.Member member, Model.Member applyMember, int applyAgentLeavel, decimal money, List<CommonObject> listComm)
        {
            TD_SHMoney shMoney = CacheService.SHMoneyList.Where(c => c.Code == "ApplyAgent" && c.TJIndex == applyAgentLeavel).FirstOrDefault();
            if(shMoney != null)
            {
                bool isSH = false;
                string roleCode = shMoney.RoleCode;
                if(!string.IsNullOrEmpty(roleCode))
                {
                    foreach(string str in roleCode.Split(','))
                    {
                        if(applyMember.RoleCode == str)//由vip会员升级的没有现金奖励，只有普通会员升级的才有奖励
                        {
                            isSH = true;
                            break;
                        }
                    }
                }
                if(!isSH)
                    return;
                TD_FHLog fhLog = new TD_FHLog();
                decimal fhMoney = shMoney.TJFloat * money;
                if(shMoney.Field3 == "2")
                {
                    fhMoney = shMoney.TJFloat;
                }
                fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                fhLog.Company = 0;
                fhLog.CreatedBy = "SYSTEM";
                fhLog.CreatedTime = DateTime.Now;
                fhLog.FHDate = DateTime.Now;
                fhLog.FHMoney = fhMoney;
                fhLog.FHRoleCode = member.RoleCode;
                fhLog.FHType = shMoney.Id.ToString();
                fhLog.IsDeleted = false;
                fhLog.MID = member.ID.ToString();
                fhLog.FHMCode = member.MID;
                fhLog.PayCode = "SYSTEM";
                fhLog.Status = 1;
                fhLog.Remark = applyMember.MID + "在" + DateTime.Now.ToString() + "申请成为" + shMoney.Remark + "，做为推荐人奖励" + fhMoney;
                member.MSH = fhLog.FHMoney;
                if(fhLog.FHMoney > 0)
                    CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                if(fhLog.FHMoney > 0)
                    MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
            }
        }

        /// <summary>
        /// 发红包
        /// </summary>
        /// <param name="fromMember">发送人</param>
        /// <param name="toMember">接收人</param>
        /// <param name="money">金额</param>
        /// <param name="redType">红包类型1-公司红包，2-个人红包</param>
        /// <param name="redBagCount">红包个数</param>
        /// <param name="listComm"></param>
        public static void SendRedBag(Model.Member fromMember, Model.Member toMember, decimal money, int redType, int redBagCount, List<CommonObject> listComm)
        {
            if(redType == 2)
            {
                //SendRedBagCount只纪录最高的发送数量（发送层数）
                if(fromMember.SendRedBagCount < redBagCount)
                {
                    fromMember.SendRedBagCount = redBagCount;
                    CommonBase.Update<Member>(fromMember, new string[] { "SendRedBagCount" }, listComm);
                }
            }
            //发放红包主表插入一条数据
            SH_RedBagHeaderLog header = new SH_RedBagHeaderLog();
            header.Code = MethodHelper.CommonHelper.CreateNo();
            header.UserId = fromMember.ID.ToString();
            header.UserCode = fromMember.MID;
            header.RedType = redType;
            header.LogDate = DateTime.Now;
            header.RedBagMoney = money;
            header.RedBagCount = redBagCount;
            header.Remark = fromMember.MID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "发放" + redBagCount + "个" + money + "元的红包";
            CommonBase.Insert<SH_RedBagHeaderLog>(header, listComm);
            if(redType == 2)
            {
                //找出给谁发红包
                //只给上级发红包,调用存储过程按推荐关系往上查找
                //存储过程查出来该会员的上级推荐关心信息
                SqlParameter[] parameters = {
                    new SqlParameter("@MID", SqlDbType.VarChar, 50),
                    new SqlParameter("@RoleCode", SqlDbType.VarChar, 50),
                     new SqlParameter("@Rank", SqlDbType.Int, 4)
                 };
                parameters[0].Value = fromMember.ID;
                parameters[1].Value = "";
                parameters[2].Value = 999999;
                DataTable dtUpperTJMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperMemberAddRank");
                dtUpperTJMemberTbl.Columns.Add("RealRank", typeof(int));
                //去除掉代理商
                DataRow[] dtUpperTJMember = dtUpperTJMemberTbl.Select("RoleCode='VIP' OR RoleCode='Member'", "RANK ASC");
                for(int i = 0; i < dtUpperTJMember.Length; i++)
                {
                    dtUpperTJMember[i]["RealRank"] = i + 1;
                }

                //获取到奖金池
                var listPool = CacheService.PrizePoolList.OrderBy(c => c.Sort);
                int poolIndex = 1;
                for(int i = 0; i < redBagCount; i++)
                {
                    if(dtUpperTJMember.Length >= i + 1) //存在某一层级的会员,对会员发红包
                    {
                        DataRow dr = dtUpperTJMember[i];
                        bool isActive = false;
                        string toID = dr["ID"].ToString();
                        string toMID = dr["MID"].ToString();
                        string toMIDRoleCode = dr["RoleCode"].ToString();
                        string toMIDHasSendRedBagCount = dr["SendRedBagCount"].ToString();
                        int toLevel = MethodHelper.ConvertHelper.ToInt32(dr["RealRank"], 1);
                        //判断要发给某个会员的红包是不是可以直接激活，判断方法：
                        //发送人A距离接受红包的会员B的层级距离，
                        //例如：A发3个红包，第3层上是会员B，判断B接受红包时是不是直接激活状态，就是要判断B是不是上发过3个红包
                        isActive = CheckIsCanActive(toID);
                        if(isActive)
                        {
                            //查看A到接收人B的层级,因为要去除代理商，这里的实际层级可能不一样，所以不能用RANK
                            if(toLevel <= int.Parse(toMIDHasSendRedBagCount))
                                isActive = true;
                            else
                                isActive = false;
                        }
                        //组装红包详细记录表并插入数据
                        SH_RedBagDetailLog detailLog = GetSendRedBagDetail(fromMember, toID, toMID, money, toLevel, header, redType, isActive);
                        if(isActive) //如果是直接激活的金额，就直接存入会员的现金币
                        {
                            MemberService.UpdateMoney(listComm, toID, money.ToString(), "MSH", true);
                        }
                        else //未激活的就先存入会员的未激活金额
                        {
                            MemberService.UpdateMoney(listComm, toID, money.ToString(), "NoActiveMoney", true);
                        }
                        CommonBase.Insert<SH_RedBagDetailLog>(detailLog, listComm);
                    }
                    else  //不存在会员了，剩下的都进入奖金池
                    {
                        //进入奖金池,按顺序进入
                        SH_PrizePool pool = listPool.Where(c => c.Sort == poolIndex).FirstOrDefault();
                        if(pool != null)
                        {
                            SH_PrizePoolInLog inLog = GetPrizePoolInLog(fromMember.ID.ToString(), fromMember.MID, pool.Id, money);
                            CommonBase.Insert<SH_PrizePoolInLog>(inLog, listComm);
                            poolIndex++;
                        }
                        else
                        {  //不存在哪一个奖金池，都归属到3号奖金池
                            SH_PrizePool pool3 = listPool.Where(c => c.Sort == 3).FirstOrDefault();
                            SH_PrizePoolInLog inLog = GetPrizePoolInLog(fromMember.ID.ToString(), fromMember.MID, pool3.Id, money);
                            CommonBase.Insert<SH_PrizePoolInLog>(inLog, listComm);
                        }
                    }
                }
            }
            else
            {
                //组装红包详细记录表并插入数据
                SH_RedBagDetailLog detailLog = GetSendRedBagDetail(fromMember, toMember.ID.ToString(), toMember.MID, money, 1, header, redType, false);
                CommonBase.Insert<SH_RedBagDetailLog>(detailLog, listComm);
                MemberService.UpdateMoney(listComm, toMember.ID.ToString(), money.ToString(), "NoActiveMoney", true);
            }
        }

        private static SH_PrizePoolInLog GetPrizePoolInLog(string userId, string userCode, string poolId, decimal inMoney)
        {
            SH_PrizePoolInLog log = new SH_PrizePoolInLog();
            log.UserCode = userCode;
            log.UserId = userId;
            log.Code = MethodHelper.CommonHelper.GetGuid;
            log.InMoney = inMoney;
            log.LogDate = DateTime.Now;
            log.PoolId = poolId;
            return log;
        }


        private static SH_RedBagDetailLog GetSendRedBagDetail(Model.Member fromMember, string toMemberID, string toMID, decimal money, int level, SH_RedBagHeaderLog header, int redType, bool isActive)
        {
            SH_RedBagDetailLog detail = new SH_RedBagDetailLog();
            detail.Code = MethodHelper.CommonHelper.CreateNo() + MethodHelper.CommonHelper.GetGuid.Substring(0, 5);
            detail.FromLevelCount = level;
            detail.FromUserCode = fromMember.MID;
            detail.FromUserId = fromMember.ID.ToString();
            detail.HeaderCode = header.Code;
            detail.RedBagMoney = money;
            detail.RedType = redType;
            detail.Status = 1;
            detail.ToUserCode = toMID;
            detail.ToUserId = toMemberID;
            detail.SendTime = header.LogDate;
            detail.IsActive = isActive;
            if(isActive)
            {
                detail.Status = 3;
                detail.ActiveTime = DateTime.Now;
            }
            detail.LogDate = DateTime.Now;
            return detail;
        }

        /// <summary>
        /// 激活红包
        /// </summary>
        /// <param name="fromMember">红包发放人</param>
        /// <param name="redBagCount">红包数量</param>
        /// <param name="listComm"></param>
        public static void ActiveRedBag(string MemberID, string redBagCount, List<CommonObject> listComm)
        {
            if(string.IsNullOrEmpty(redBagCount))
            {
                //如果没有传递红包个数，就查看所有收到的红包是不是可以激活
                List<SH_RedBagDetailLog> listDetail = CommonBase.GetList<SH_RedBagDetailLog>("(Status=1 or Status=2) and ToUserId='" + MemberID + "'");
                foreach(SH_RedBagDetailLog log in listDetail)
                {
                    //看看有没有给上级发
                    if(CommonBase.GetList<SH_RedBagDetailLog>(" FromUserId='" + MemberID + "' and FromLevelCount=" + log.FromLevelCount).Count > 0)
                    {
                        decimal addsum = log.RedBagMoney;
                        log.Status = 3;
                        log.ActiveTime = DateTime.Now;
                        CommonBase.Update<SH_RedBagDetailLog>(log, new string[] { "Status", "ActiveTime" }, listComm);
                        MemberService.UpdateMoney(listComm, MemberID, addsum.ToString(), "MSH", true);
                        MemberService.UpdateMoney(listComm, MemberID, addsum.ToString(), "NoActiveMoney", false);
                    }
                }
            }
            else
            {
                bool isActive = CheckIsCanActive(MemberID);
                if(isActive)
                {
                    //往上发了两个红包，就需要激活下层2级及以内的会员发放的红包
                    string sql = "UPDATE dbo.SH_RedBagDetailLog SET Status=3  WHERE (Status=1 or Status=2) and ToUserId='" + MemberID + "' AND FromLevelCount<=" + redBagCount;
                    string sqlSum = "SELECT SUM(RedBagMoney) FROM SH_RedBagDetailLog WHERE (Status=1 or Status=2) and ToUserId='" + MemberID + "' AND FromLevelCount<=" + redBagCount;
                    decimal addsum = MethodHelper.ConvertHelper.ToDecimal(CommonBase.GetSingle(sqlSum), 0);
                    MemberService.UpdateMoney(listComm, MemberID, addsum.ToString(), "MSH", true);
                    MemberService.UpdateMoney(listComm, MemberID, addsum.ToString(), "NoActiveMoney", false);
                    listComm.Add(new CommonObject(sql, null));
                }
            }
        }

        public static void ActiveRedBag(Model.Member member, string redBagCount, List<CommonObject> listComm)
        {
            ActiveRedBag(member.ID.ToString(), redBagCount, listComm);
        }

        public static decimal ActiveRedBagMoney(string MemberID, string redBagCount, List<CommonObject> listComm)
        {
            bool isActive = CheckIsCanActive(MemberID);
            if(isActive)
            {
                //往上发了两个红包，就需要激活下层2级及以内的会员发放的红包
                string sql = "UPDATE dbo.SH_RedBagDetailLog SET Status=3  WHERE (Status=1 or Status=2) and ToUserId='" + MemberID + "' AND FromLevelCount<=" + redBagCount;
                listComm.Add(new CommonObject(sql, null));
                string sqlSum = "SELECT SUM(RedBagMoney) FROM SH_RedBagDetailLog WHERE (Status=1 or Status=2) and ToUserId='" + MemberID + "' AND FromLevelCount<=" + redBagCount;
                decimal addsum = MethodHelper.ConvertHelper.ToDecimal(CommonBase.GetSingle(sqlSum), 0);
                //现金币需要增加的金额
                //需要减少的未激活金额
                return addsum;
            }
            else
            {
                return 0;
            }
        }

        public static bool CheckIsCanActive(string MemberID)
        {
            //看是否满足推荐一个缴费会员2个注册会员，只有满足这些条件了，才能激活红包
            bool isCanActive = false;
            List<Member> listTJMemberList = CommonBase.GetList<Member>("MTJ='" + MemberID + "'");
            //注册会员
            if(CacheService.RedBagGlobleConfig.ActiveForTJCount <= listTJMemberList.Count && CacheService.RedBagGlobleConfig.ActiveForTJVIPCount <= listTJMemberList.Where(c => c.RoleCode == "VIP").Count())
            {
                isCanActive = true;
            }
            return isCanActive;
        }

        public static void SendCompanyRedBag(Model.Member member, List<CommonObject> listComm)
        {
            //缴费之后发放公司红包
            Model.Member companyMember = MemberService.GetCompanyAdminMember();
            decimal redBagMoney = CacheService.RedBagGlobleConfig.CompanyReturnMoney;
            SHMoneyService.SendRedBag(companyMember, member, redBagMoney, 1, 1, listComm);
            //查看推荐人的红包是否可以激活
            if(SHMoneyService.CheckIsCanActive(member.MTJ))
            {
                //发完红包，查看可激活的红包层级并激活之
                SHMoneyService.ActiveRedBag(member.MTJ, string.Empty, listComm);
            }
        }

        /// <summary>
        /// 奖金池日分红，针对洛胜卡管家
        /// </summary>
        public static void DayFH(DateTime fhDate, List<CommonObject> listComm)
        {
            //遍历三个奖金池
            foreach(SH_PrizePool pool in CacheService.PrizePoolList)
            {
                //查看这一天的奖金总额
                string totalMoneySql = " SELECT SUM(InMoney) FROM dbo.SH_PrizePoolInLog WHERE DATEDIFF(dd,LogDate,'" + fhDate.ToString("yyyy-MM-dd") + "')=0 AND PoolId='" + pool.Id + "'";
                decimal totalMoney = MethodHelper.ConvertHelper.ToDecimal(CommonBase.GetSingle(totalMoneySql), 0);
                if(totalMoney > 0)
                {
                    //查询出相应级别的代理商
                    List<Model.Member> listMember = CommonBase.GetList<Model.Member>("RoleCode in (" + pool.Remark + ")");
                    int memberCount = listMember.Count;
                    if(memberCount > 0)
                    {
                        //查看平均金额
                        decimal perMoney = totalMoney / memberCount;
                        //遍历代理商信息，进行分红
                        foreach(Model.Member member in listMember)
                        {
                            bool isProhibit = false;//是否禁止该奖金池分红
                                                    //查看该代理商是否具备分红资格
                            string noFHPool = member.NoFHPool;
                            if(!string.IsNullOrEmpty(noFHPool))
                            {
                                List<string> noFHPoolArray = noFHPool.Split(',').ToList();
                                if(noFHPoolArray.Contains(pool.Id))
                                {
                                    isProhibit = true;
                                }
                            }
                            //只对未禁止的会员分红
                            if(!isProhibit)
                            {
                                //分红详细表增加一条数据
                                //插入代理商分红记录表
                                TD_FHLog fhLog = new TD_FHLog();
                                fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                                fhLog.Company = 0;
                                fhLog.CreatedBy = "SYSTEM";
                                fhLog.CreatedTime = DateTime.Now;
                                fhLog.FHDate = DateTime.Now;
                                fhLog.FHMoney = perMoney;
                                fhLog.FHRoleCode = member.RoleCode;
                                fhLog.FHMCode = member.MID;
                                fhLog.FHType = "P-" + pool.Id;
                                fhLog.IsDeleted = false;
                                fhLog.MID = member.ID.ToString();
                                fhLog.PayCode = "SYSTEM";
                                fhLog.Status = 1;
                                fhLog.Remark = fhDate.ToString("yyyy年MM月dd日") + pool.PoolName + "分红";
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                                //更新代理商账户资金
                                MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                            }
                        }
                    }
                }
            }
        }

        public static void DayFH(DateTime fhDate, bool isTimeWatching)
        {
            //App.config中配置的的数据库链接字符串key值
            string ConnectionStringKeys = MethodHelper.ConfigHelper.GetAppSettings("ConnectionStringKeys_for_PrizePool");
            string[] connectionArray = ConnectionStringKeys.Split(',');
            foreach(string configName in connectionArray)
            {
                if(string.IsNullOrEmpty(configName))
                    continue;
                DbHelperSQL.connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
                List<CommonObject> listComm = new List<CommonObject>();
                try
                {
                    //遍历三个奖金池
                    List<SH_PrizePool> poolList = CommonBase.GetList<SH_PrizePool>("IsDeleted=0");
                    foreach(SH_PrizePool pool in poolList)
                    {
                        //查看这一天的奖金总额
                        string totalMoneySql = " SELECT SUM(InMoney) FROM dbo.SH_PrizePoolInLog WHERE DATEDIFF(dd,LogDate,'" + fhDate.ToString("yyyy-MM-dd") + "')=0 AND PoolId='" + pool.Id + "'";
                        decimal totalMoney = MethodHelper.ConvertHelper.ToDecimal(CommonBase.GetSingle(totalMoneySql), 0);
                        if(totalMoney > 0)
                        {
                            //查询出相应级别的代理商
                            List<Model.Member> listMember = CommonBase.GetList<Model.Member>("RoleCode in (" + pool.Remark + ")");
                            int memberCount = listMember.Count;
                            if(memberCount > 0)
                            {
                                //查看平均金额
                                decimal perMoney = totalMoney / memberCount;
                                //遍历代理商信息，进行分红
                                foreach(Model.Member member in listMember)
                                {
                                    bool isProhibit = false;//是否禁止该奖金池分红
                                                            //查看该代理商是否具备分红资格
                                    string noFHPool = member.NoFHPool;
                                    if(!string.IsNullOrEmpty(noFHPool))
                                    {
                                        List<string> noFHPoolArray = noFHPool.Split(',').ToList();
                                        if(noFHPoolArray.Contains(pool.Id))
                                        {
                                            isProhibit = true;
                                        }
                                    }
                                    //只对未禁止的会员分红
                                    if(!isProhibit)
                                    {
                                        //分红详细表增加一条数据
                                        //插入代理商分红记录表
                                        TD_FHLog fhLog = new TD_FHLog();
                                        fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                                        fhLog.Company = 0;
                                        fhLog.CreatedBy = "SYSTEM";
                                        fhLog.CreatedTime = DateTime.Now;
                                        fhLog.FHDate = DateTime.Now;
                                        fhLog.FHMoney = perMoney;
                                        fhLog.FHRoleCode = member.RoleCode;
                                        fhLog.FHMCode = member.MID;
                                        fhLog.FHType = "P-" + pool.Id;
                                        fhLog.IsDeleted = false;
                                        fhLog.MID = member.ID.ToString();
                                        fhLog.PayCode = "SYSTEM";
                                        fhLog.Status = 1;
                                        fhLog.Remark = fhDate.ToString("yyyy年MM月dd日") + pool.PoolName + "分红";
                                        CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                                        //更新代理商账户资金
                                        MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                                    }
                                }
                            }
                        }
                    }
                    if(CommonBase.RunListCommit(listComm))
                    {
                        MethodHelper.LogHelper.WriteTextLog("数据库配置key键" + configName + "中执行；DayFH()", "成功进行了汇享金日分红", fhDate);
                    }
                    else
                    {
                        MethodHelper.LogHelper.WriteTextLog("数据库配置key键" + configName + "中执行；DayFH()", "执行失败", fhDate);
                    }
                }
                catch(Exception ex)
                {
                    MethodHelper.LogHelper.WriteTextLog("数据库配置key键" + configName + "中执行；DayFH()", "执行失败，错误原因：" + ex.ToString(), fhDate);
                }
            }
        }

        /// <summary>
        /// 设置公排点位，新修改
        /// </summary>
        /// <param name="member"></param>
        /// <param name="listComm"></param>
        public static void SetRankPoint(Model.Member member, List<CommonObject> listComm, ref M_Rank rank)
        {
            rank = null;
            //如果推荐人是管理员
            if(member.MTJ == "18")
            {
                //先查看一下名下的人有没有交过费排过位的
                string sql = "SELECT t2.* FROM dbo.FUN_CountTDMember('" + member.ID + "',0,9999) t1 LEFT JOIN dbo.M_Rank t2 ON (t1.Code=t2.MCode OR t1.Code=t2.PMCode) WHERE t1.Code<>'18' AND t2.Code IS NOT NULL order BY t2.RankTime DESC";
                DataTable tjTbl = CommonBase.GetTable(sql);
                if(tjTbl.Rows.Count > 0)
                {
                    checkPoint(member, tjTbl.Rows[0]["MCode"].ToString(), listComm, ref rank);
                }
                else
                {
                    //直接推荐人是管理员，那就自己做这棵树的根结点
                    M_Rank newRank = new M_Rank();
                    newRank.Code = MethodHelper.CommonHelper.GetGuid;
                    newRank.IsDeleted = false;
                    newRank.Leavel = 1;
                    newRank.MBD = 1;
                    newRank.MCode = member.ID;
                    newRank.PMCode = "0";
                    newRank.RankTime = DateTime.Now;
                    newRank.Sort = 1;
                    newRank.Status = 1;
                    CommonBase.Insert<M_Rank>(newRank, listComm);
                    rank = newRank;
                }
            }
            //有其他推荐人的
            else
            {
                //有推荐人的话就往上查找，看看相关层级中的人哪个是在排位表中的
                string sql = "SELECT t2.* FROM dbo.FUN_CountUpperMember('" + member.ID + "',0,9999) t1 LEFT JOIN dbo.M_Rank t2 ON (t1.Code=t2.MCode OR t1.Code=t2.PMCode) WHERE t1.Code<>'18' AND t2.Code IS NOT NULL order BY t2.RankTime DESC";
                DataTable tjTbl = CommonBase.GetTable(sql);
                if(tjTbl.Rows.Count > 0)
                {
                    checkPoint(member, tjTbl.Rows[0]["MCode"].ToString(), listComm, ref rank);
                }
                else
                {
                    //自己做作为这棵树的根结点
                    M_Rank newRank = new M_Rank();
                    newRank.Code = MethodHelper.CommonHelper.GetGuid;
                    newRank.IsDeleted = false;
                    newRank.Leavel = 1;
                    newRank.MBD = 1;
                    newRank.MCode = member.ID;
                    newRank.PMCode = "0";
                    newRank.RankTime = DateTime.Now;
                    newRank.Sort = 1;
                    newRank.Status = 1;
                    CommonBase.Insert<M_Rank>(newRank, listComm);
                    rank = newRank;
                }
            }
        }

        public static void checkPoint(Model.Member member, string MCode, List<CommonObject> listComm, ref M_Rank refNewRank)
        {
            refNewRank = null;
            //获取到这棵树的跟节点
            //string sqlRank = "SELECT * FROM dbo.[FUN_CountUpperMemberWithRank]('" + tjTbl.Rows[0]["MCode"].ToString() + "',1,9999)";
            string sqlRank = "SELECT * FROM dbo.[FUN_CountUpperMemberWithRank]('" + MCode + "',1,9999)";
            DataTable tjRankTbl = CommonBase.GetTable(sqlRank);
            if(tjRankTbl.Rows.Count > 0)
            {
                //最后一行的code
                string rootRankCode = tjRankTbl.Rows[tjRankTbl.Rows.Count - 1]["Code"].ToString();
                //获取到这个Rank
                //获取到第一个根节点
                List<M_Rank> listAllRank = CommonBase.GetList<M_Rank>("");
                //跟节点
                M_Rank rank = listAllRank.FirstOrDefault(c => c.Code == rootRankCode);


                //查询到这个根节点下的有空位的序列
                List<M_Rank> treeNodes = new List<M_Rank>();
                GetSelectTreeNodes(listAllRank, rank.Code, rank.MCode, ref treeNodes, true);
                //treeNodes这集合里面有三个，都是没排满的，
                //获取到最小的leavel
                int minLeavel = treeNodes.Min(c => c.Leavel);
                treeNodes = treeNodes.Where(c => c.Leavel == minLeavel).ToList();

                //排位数量
                int rankCount = int.Parse(MethodHelper.ConfigHelper.GetAppSettings("RankCount"));//这里要从配置文件中读取
                List<M_Rank> subTreeNodes = new List<M_Rank>();
                string PMCode = string.Empty;
                int mbd = 0, leavel = 0, sort = 1;
                bool isBreak = false;
                for(int i = 1; i <= rankCount; i++)
                {
                    foreach(M_Rank node in treeNodes)
                    {
                        subTreeNodes.Clear();
                        GetSelectTreeNodes(listAllRank.Where(c => c.PMCode == node.MCode).ToList(), node.Code, node.MCode, ref subTreeNodes, false);
                        if(subTreeNodes.FirstOrDefault(c => c.MBD == i) == null)
                        {
                            mbd = i;
                            PMCode = node.MCode;
                            leavel = node.Leavel + 1;
                            isBreak = true;
                            sort = node.Sort + 1;
                            break;
                        }
                    }
                    if(isBreak)
                        break;
                }
                if(!string.IsNullOrEmpty(PMCode))
                {
                    M_Rank newRank = new M_Rank();
                    newRank.Code = MethodHelper.CommonHelper.GetGuid;
                    newRank.IsDeleted = false;
                    newRank.Leavel = leavel;
                    newRank.MBD = mbd;
                    newRank.MCode = member.ID;
                    newRank.PMCode = PMCode;
                    newRank.RankTime = DateTime.Now;
                    newRank.Sort = sort;
                    newRank.Status = 1;
                    CommonBase.Insert<M_Rank>(newRank, listComm);
                    refNewRank = newRank;
                }
            }
        }

        public static bool IsCanSJ(DataRow dr)
        {
            bool isReach = false;
            //查看下面三个人是否都达到自身等级，达到之后自身开始升级，
            string msy = "SELECT MRANK,COUNT(1) MemberCount FROM dbo.FUN_CountTDMemberWithRank('" + dr["ID"].ToString() + "',1,99999) GROUP BY MRANK";
            DataTable dtMsy = CommonBase.GetTable(msy);
            //获取到最大的一层
            DataRow[] drArray = dtMsy.Select("MRANK>0", "MRANK DESC");
            if(drArray.Length > 0)
            {
                DataRow array = drArray[0];
                if(int.Parse(array["MemberCount"].ToString()) == 1)
                {
                    isReach = true;
                }
            }
            return isReach;
        }

        private static bool IsReachSJLeavel(DataTable dtUpperRank, Sys_Role roleModel, DataRow dr)
        {
            bool isSetNoFH = false;
            //查看下面三个人是否都达到VIP，
            string msy = "SELECT * FROM dbo.FUN_CountTDMemberWithRank('" + dr["ID"].ToString() + "',0,1)";
            DataTable dtMsy = CommonBase.GetTable(msy);
            foreach(DataRow drsy in dtMsy.Rows)
            {
                DataRow[] dfgye = dtUpperRank.Select("ID='" + drsy["ID"].ToString() + "'");
                if(dfgye.Length > 0)
                {
                    drsy["RoleCode"] = dfgye[0]["RoleCode"];
                }
            }
            Sys_Role prevRoleModel = CacheService.RoleList.Where(c => c.RIndex == roleModel.RIndex - 1 && c.IsDeleted == false).FirstOrDefault();
            if(prevRoleModel != null)
            {
                //前一级别的总数已达到排位升级要求
                if(dtMsy.Select("RoleCode='" + prevRoleModel.Code + "'").Length == int.Parse(MethodHelper.ConfigHelper.GetAppSettings("RankCount")))
                {
                    isSetNoFH = true;
                }
            }
            return isSetNoFH;
        }
        /// <summary>
        /// 给会员升级
        /// </summary>
        /// <param name="roleModel"></param>
        /// <param name="dr"></param>
        /// <param name="member"></param>
        /// <param name="listComm"></param>
        public static void SetSJRole(Sys_Role roleModel, DataRow dr, Member member, List<CommonObject> listComm)
        {
            //当前级别的下一级别
            Sys_Role nextRoleModel = CacheService.RoleList.Where(c => c.RIndex == roleModel.RIndex + 1 && c.IsDeleted == false).FirstOrDefault();
            if(nextRoleModel != null)
            {
                string updateRoleSql = "UPDATE dbo.Member SET RoleCode='" + nextRoleModel.Code + "' WHERE ID='" + dr["ID"].ToString() + "'";
                listComm.Add(new CommonObject(updateRoleSql, null));
                dr["RoleCode"] = nextRoleModel.Code;
                LogService.Log(dr["ID"].ToString(), dr["MID"].ToString(), "1", member.MID + "缴费成为学员，由" + roleModel.Name + "升级为" + nextRoleModel.Name, listComm);
            }
        }

        /// <summary>
        /// 设置分红状态
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fhStatus"></param>
        /// <param name="listComm"></param>
        public static void SetFHStatus(DataRow dr, string fhStatus, List<CommonObject> listComm)
        {
            //设置可以分红
            string updateRoleSql = "UPDATE dbo.Member SET IsFH='" + fhStatus + "' WHERE ID='" + dr["ID"].ToString() + "'";
            dr["IsFH"] = fhStatus;
            listComm.Add(new CommonObject(updateRoleSql, null));
        }
        /// <summary>
        /// 查看是否达到推荐人数（可升级的推荐人数）
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool IsReachTJCount(DataRow dr, Member member, Sys_Role roleModel)
        {
            if(roleModel.RoleType <= 0)
                return true;
            string countSql = "SELECT COUNT(1) FROM dbo.Member WHERE MTJ='" + dr["ID"].ToString() + "' AND RoleCode<>'Member'";
            object obj = CommonBase.GetSingle(countSql);
            //已经存在的直接推荐人数
            int thisTJCount = MethodHelper.ConvertHelper.ToInt32(obj, 0);
            //如果缴费会员还是自己直接推荐的
            if(member.MTJ == dr["ID"].ToString())
            {
                thisTJCount = thisTJCount + 1;
            }
            if(thisTJCount >= roleModel.RoleType)
                return true;
            else
                return false;
        }
        /// <summary>
        ///  查看下面三个人是否都达到自身等级，达到之后自身开始升级，返回自身需要升级的下一等级
        /// </summary>
        /// <param name="dtUpperRank"></param>
        /// <param name="roleModel"></param>
        /// <param name="dr"></param>
        /// <param name="nextRoleModel"></param>
        /// <returns></returns>
        private static bool IsAllReachLeavel(DataTable dtUpperRank, Sys_Role roleModel, DataRow dr, ref Sys_Role nextRoleModel)
        {
            bool isReach = true;
            //查看下面三个人是否都达到自身等级，达到之后自身开始升级，
            string msy = "SELECT * FROM dbo.FUN_CountTDMemberWithRank('" + dr["ID"].ToString() + "',0,1)";
            DataTable dtMsy = CommonBase.GetTable(msy);
            if(dtMsy.Rows.Count < int.Parse(MethodHelper.ConfigHelper.GetAppSettings("RankCount")))
            {
                isReach = false;
            }
            else
            {
                foreach(DataRow drsy in dtMsy.Rows)
                {
                    DataRow[] dfgye = dtUpperRank.Select("ID='" + drsy["ID"].ToString() + "'");
                    if(dfgye.Length > 0)
                    {
                        drsy["RoleCode"] = dfgye[0]["RoleCode"];
                    }
                }
                foreach(DataRow drsy in dtMsy.Rows)
                {
                    DataRow[] dfgye = dtUpperRank.Select("RoleCode='" + dr["RoleCode"].ToString() + "'");
                    if(dfgye.Length <= 0)
                    {
                        isReach = false;
                        break;
                    }
                }
            }
            if(isReach)
            {
                nextRoleModel = CacheService.RoleList.Where(c => c.RIndex == roleModel.RIndex + 1 && c.IsDeleted == false).FirstOrDefault();
            }
            else
            {
                nextRoleModel = null;
            }
            return isReach;
        }

        //设置公排点位，老版本
        //public static void SetRankPoint(Model.Member member, List<CommonObject> listComm)
        //        {
        //            //查找到顶层
        //            string sql = "SELECT * FROM dbo.FUN_CountUpperMember('" + member.ID + "',0,9999) WHERE MID<>'admin'";
        //        DataTable tjTbl = CommonBase.GetTable(sql);
        //            //直接推荐人是管理员的
        //            if(tjTbl.Rows.Count == 0)
        //            {
        //                //查找排位表的第一个排位
        //                List<M_Rank> listAllRank = CommonBase.GetList<M_Rank>("");
        //        List<M_Rank> listRank = listAllRank.Where(c => c.PMCode == "0").OrderBy(c => c.RankTime).ToList();// CommonBase.GetList<M_Rank>("PMCode=0 ORDER BY RankTime asc");
        //                if(listRank.Count > 0)
        //                {
        //                    //获取到第一个根节点
        //                    M_Rank rank = listRank.FirstOrDefault();
        //        //查询到这个根节点下的有空位的序列
        //        List<M_Rank> treeNodes = new List<M_Rank>();
        //                    GetSelectTreeNodes(listAllRank, rank.Code, rank.MCode, ref treeNodes, true);
        //        //treeNodes这集合里面有三个，都是没排满的，
        //        //获取到最小的leavel
        //        int minLeavel = treeNodes.Min(c => c.Leavel);
        //        treeNodes = treeNodes.Where(c => c.Leavel == minLeavel).ToList();

        //        //排位数量
        //        int rankCount = int.Parse(MethodHelper.ConfigHelper.GetAppSettings("RankCount"));//这里要从数据库中读取
        //        List<M_Rank> subTreeNodes = new List<M_Rank>();
        //        string PMCode = string.Empty;
        //        int mbd = 0, leavel = 0;
        //        bool isBreak = false;
        //                    for(int i = 1; i <= rankCount; i++)
        //                    {
        //                        foreach(M_Rank node in treeNodes)
        //                        {
        //                            subTreeNodes.Clear();
        //                            GetSelectTreeNodes(listAllRank.Where(c => c.PMCode == node.MCode).ToList(), node.Code, node.MCode, ref subTreeNodes, false);
        //                            if(subTreeNodes.FirstOrDefault(c => c.MBD == i) == null)
        //                            {
        //                                mbd = i;
        //                                PMCode = node.MCode;
        //                                leavel = node.Leavel + 1;
        //                                isBreak = true;
        //                                break;
        //                            }
        //}
        //                        if(isBreak)
        //                            break;
        //                    }
        //                    if(!string.IsNullOrEmpty(PMCode))
        //                    {
        //                        M_Rank newRank = new M_Rank();
        //newRank.Code = MethodHelper.CommonHelper.GetGuid;
        //                        newRank.IsDeleted = false;
        //                        newRank.Leavel = leavel;
        //                        newRank.MBD = mbd;
        //                        newRank.MCode = member.ID;
        //                        newRank.PMCode = PMCode;
        //                        newRank.RankTime = DateTime.Now;
        //                        newRank.Sort = 1;
        //                        newRank.Status = 1;
        //                        CommonBase.Insert<M_Rank>(newRank, listComm);
        //                    }
        //                }
        //            }
        //            //有其他推荐人的
        //            else
        //            {
        //                //有推荐人的话就往上查找，看看相关层级中的人哪个是在排位表中的

        //            }
        //        }

        public static void GetSelectTreeNodes(List<M_Rank> list, string rankCode, string id, ref List<M_Rank> treeNodes, bool isFromFoot)
        {
            if(list == null)
                return;
            List<M_Rank> sublist;
            if(!string.IsNullOrWhiteSpace(id))
            {

                sublist = list.Where(t => t.PMCode == id).OrderBy(t => t.MBD).ToList();
                if(isFromFoot)
                {
                    ////名下点数不够3个的,取到第一个
                    //int rankCount = 3;//这里要从数据库中读取
                    //if(sublist.Count < rankCount)
                    //{
                    //    return;
                    //}
                }
            }
            else
            {
                sublist = list.Where(t => string.IsNullOrEmpty(t.PMCode)).ToList();
            }
            if(sublist.Count != int.Parse(MethodHelper.ConfigHelper.GetAppSettings("RankCount")))
            {
                M_Rank item = list.FirstOrDefault(c => c.Code == rankCode);
                if(item != null)
                    treeNodes.Add(new M_Rank() { MCode = item.MCode, PMCode = item.PMCode, MBD = item.MBD, Leavel = item.Leavel, RankTime = item.RankTime });
            }
            //return;
            foreach(var item in sublist)
            {
                if(sublist.Count < int.Parse(MethodHelper.ConfigHelper.GetAppSettings("RankCount")))
                    treeNodes.Add(new M_Rank() { MCode = item.MCode, PMCode = item.PMCode, MBD = item.MBD, Leavel = item.Leavel, RankTime = item.RankTime });
                GetSelectTreeNodes(list, item.Code, item.MCode, ref treeNodes, isFromFoot);
            }
        }
        /// <summary>
        /// 从根节点查询，查询子节点数量小于三个的节点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <param name="treeNodes"></param>
        public static void GetSelectTreeNodesFromFoot(List<M_Rank> list, string id, ref List<M_Rank> treeNodes)
        {
            if(list == null)
                return;
            List<M_Rank> sublist;
            if(!string.IsNullOrWhiteSpace(id))
            {
                sublist = list.Where(t => t.PMCode == id).OrderBy(t => t.MBD).ToList();

                ////名下点数不够3个的,取到第一个
                //int rankCount = 3;//这里要从数据库中读取
                //if(sublist.Count == rankCount)
                //{
                //    foreach(var item in sublist)
                //    {
                //        //treeNodes.Add(new M_Rank() { MCode = item.MCode, PMCode = item.PMCode, MBD = item.MBD, Leavel = item.Leavel, RankTime = item.RankTime });
                //        GetSelectTreeNodesFromFoot(list, item.MCode, ref treeNodes);
                //    }
                //}
            }
            else
            {
                sublist = list.Where(t => string.IsNullOrEmpty(t.PMCode)).ToList();
            }
            if(!sublist.Any())
                return;
            foreach(var item in sublist)
            {
                if(sublist.Count < 3)
                    treeNodes.Add(new M_Rank() { MCode = item.MCode, PMCode = item.PMCode, MBD = item.MBD, Leavel = item.Leavel, RankTime = item.RankTime });
                GetSelectTreeNodesFromFoot(list, item.MCode, ref treeNodes);
            }
        }

    }
}
