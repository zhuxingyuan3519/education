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
    public class SHMoneyService
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
        /// 发放推荐奖(会员奖励奖励)
        /// </summary>
        /// <param name="member">充值会员</param>
        /// <param name="money">充值金额</param>
        public static bool TJChangeMoney(Model.Member member, decimal money, List<CommonObject> listComm, string addType, int point, string isMemberFh, decimal fhBi = 0)
        {
            //获取到推荐人
            string tyjModelID = member.Company;
            //if (tyjModelID == "0" || tyjModelID == "18")
            //{
            tyjModelID = member.MTJ;
            //}
            Model.Member tjModel = CommonBase.GetModel<Model.Member>(tyjModelID);
            if(tjModel == null)
                return false;
            if(isMemberFh != "1")
            {
                if(tjModel.RoleCode.Contains("Member") || tjModel.RoleCode.Contains("VIP") || tjModel.RoleCode.Contains("Student") || tjModel.RoleCode.Contains("WordUser"))
                {
                    return false;
                }
            }

            if(tjModel.NoFHPool == "0")
                return false;

            if(tjModel.ID == "18")
                return true;
            if(addType == "1")
            {
                decimal fhMoney = fhBi / 100 * money;
                decimal fh_money = fhMoney;
                string fh_remark = member.MID + member.YunPay + "缴费" + money + "购买" + point + "名额";
                TD_FHLog fhLog = new TD_FHLog();
                AddFHLog(member, money, fh_money, tjModel.MID, tjModel.ID, tjModel.RoleCode, fh_remark, "999", listComm, out fhLog);
                //更新会员的积分
                decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                string moneyType = "MSH";
                switch(member.YunPay)
                {
                    case "Cash":
                    case "WXpay": moneyType = "MSH"; break;
                    case "Tpay": moneyType = "MJB"; break;
                    case "Vpay": moneyType = "MVB"; break;
                }
                //现金
                if(fhLog.FHMoney > 0)
                    MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), moneyType, true);
            }
            else
            {
                string memberRoleCode = member.RoleCode;

                List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "KCTJ" && c.RoleCode == member.RoleCode).ToList();
                foreach(TD_SHMoney shmoney in shMoneyList)
                {
                    decimal fhMoney = shmoney.TJFloat * money;
                    if(shmoney.Field3 == "2")
                    {
                        fhMoney = shmoney.TJFloat;
                    }
                    decimal fh_money = fhMoney;
                    string fh_remark = "推荐" + member.Role.Name + member.MID + "奖励" + fh_money + member.YunPay;
                    if(addType == "1")
                        fh_remark = member.MID + member.YunPay + "缴费" + money + "购买" + point + "名额";
                    TD_FHLog fhLog = new TD_FHLog();
                    AddFHLog(member, money, fh_money, tjModel.MID, tjModel.ID, tjModel.RoleCode, fh_remark, shmoney.Id.ToString(), listComm, out fhLog);
                    //更新会员的积分
                    decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                    string moneyType = "MSH";
                    switch(member.YunPay)
                    {
                        case "Cash":
                        case "WXpay": moneyType = "MSH"; break;
                        case "Tpay": moneyType = "MJB"; break;
                        case "Vpay": moneyType = "MVB"; break;
                    }
                    //现金
                    if(fhLog.FHMoney > 0)
                        MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), moneyType, true);
                }
            }
            return true;
        }

        /// <summary>
        /// 善融会员升级三级分销，需要去除代理商的
        /// </summary>
        /// <param name="member"></param>
        /// <param name="money"></param>
        /// <param name="listComm"></param>
        /// <returns></returns>
        public static bool SRTJChangeMoney(Model.Member member, decimal money, List<CommonObject> listComm)
        {
            bool result = true;
            //存储过程查出来该会员的上级推荐关系信息
            SqlParameter[] parameters = {
                    new SqlParameter("@MID", SqlDbType.VarChar, 50),
                    new SqlParameter("@RoleCode", SqlDbType.VarChar, 50)
                 };
            parameters[0].Value = member.ID;
            parameters[1].Value = "";
            DataTable dtUpperTJMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperMember");

            //update at 2017年7月3日21:42:56，去除代理商，只留下会员
            DataTable dtResult = dtUpperTJMemberTbl.Clone();
            //只筛查出会员级别
            foreach(DataRow dr in dtUpperTJMemberTbl.Rows)
            {
                if(dr["RoleCode"].ToString() == "VIP" || dr["RoleCode"].ToString() == "Member")
                {
                    dtResult.ImportRow(dr);
                }
            }
            //重新设置Rank
            int i = 1;
            foreach(DataRow dr in dtResult.Rows)
            {
                dr["RANK"] = i;
                i++;
            }

            dtUpperTJMemberTbl = dtResult;

            //找到分红配置表中的分红配置表，找到，TJFloat的
            List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "TJFloat").ToList();
            //update by zhuxy at2017年5月31日23:58:49，二级推荐人如果是代理商，就把剩下的钱全部给代理商
            //为了解决以下需求
            //分销商下面一般都会给自己注册一个会员，然后用这个会员资格进行推广会员，在收款的时候所推荐的会员就会在后台缴费，形成了三级分销的收益分配，但是分销商不能全额收款，有什么办法可以让这个代理商能先收到钱？
            List<string> agentRoleList = new string[] { "1F", "2F", "3F", "Province", "City", "Zone" }.ToList();
            bool isReduceAgentTreadPoint = false;
            decimal firstLeaveFHMoney = 0, agentHasFHMoney = 0, agentFHReduceMoney = 0; string agentRoleCode = string.Empty, MCode = string.Empty, mid = string.Empty; Model.Member agentMember = null;
            if(dtUpperTJMemberTbl.Rows.Count >= 2)
            {
                //查看第一个会员的推荐人是不是代理商，是的话就是代理商和会员是一个人
                if(agentRoleList.Contains(dtUpperTJMemberTbl.Rows[1]["RoleCode"].ToString()) && (dtUpperTJMemberTbl.Rows[0]["RoleCode"].ToString() == "Member" || dtUpperTJMemberTbl.Rows[0]["RoleCode"].ToString() == "VIP") && (dtUpperTJMemberTbl.Rows[0]["MTJ"].ToString() == dtUpperTJMemberTbl.Rows[1]["ID"].ToString()))
                {
                    agentRoleCode = dtUpperTJMemberTbl.Rows[1]["RoleCode"].ToString();
                    MCode = dtUpperTJMemberTbl.Rows[1]["MID"].ToString();
                    mid = dtUpperTJMemberTbl.Rows[1]["ID"].ToString();
                    agentMember = CommonBase.GetModel<Model.Member>(mid);
                    result = false;
                    if(agentMember != null && agentMember.TradePoints > 0)
                    {
                        isReduceAgentTreadPoint = true;
                    }
                    else if(agentMember.TradePoints <= 0) //只分服务奖
                    {
                        List<TD_SHMoney> shAgentMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat").ToList();
                        Model.TD_SHMoney shmoney = shAgentMoneyList.Where(c => c.TJIndex.ToString() == agentMember.Role.AreaLeave).FirstOrDefault();
                        if(shmoney != null)
                        {
                            decimal fhMoney = shmoney.TJFloat * money;
                            if(shmoney.Field3 == "2")
                            {
                                fhMoney = shmoney.TJFloat;
                            }
                            agentFHReduceMoney = fhMoney;
                            #region 添加分红记录
                            string Remark = "会员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;奖励" + fhMoney + "元";
                            TD_FHLog fhLog = new TD_FHLog();
                            AddFHLog(member, money, fhMoney, agentMember.MID, agentMember.ID.ToString(), agentMember.RoleCode, Remark, shmoney.Id.ToString(), listComm, out fhLog);
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

            foreach(TD_SHMoney shmoney in shMoneyList)
            {
                decimal fhMoney = shmoney.TJFloat * money;
                if(shmoney.Field3 == "2")
                {
                    fhMoney = shmoney.TJFloat;
                }
                if(shmoney.TJIndex == 1) firstLeaveFHMoney = fhMoney;
                DataRow[] rows = dtUpperTJMemberTbl.Select("RANK=" + shmoney.TJIndex);
                if(shmoney.TJIndex >= 2 && !result)//二级推荐人是分销商的话，剩下的钱都给分销商
                {
                    break; ;
                }
                if(rows.Length > 0)
                {
                    if(rows[0]["RoleCode"].ToString() == "Manage") continue;
                    //添加分红记录
                    decimal fh_money = fhMoney;
                    string fh_remark = "直接推荐会员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;奖励" + fhMoney + "元";
                    if(shmoney.TJIndex != 1)
                        fh_remark = "间接" + (shmoney.TJIndex - 1).ToString() + "级会员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;奖励" + fhMoney + "元";
                    TD_FHLog fhLog = new TD_FHLog();
                    AddFHLog(member, money, fh_money, rows[0]["MID"].ToString(), rows[0]["ID"].ToString(), rows[0]["RoleCode"].ToString(), fh_remark, shmoney.Id.ToString(), listComm, out fhLog);

                    //更新会员的积分
                    decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                    decimal MJB = fhLog.FHMoney - MSH;
                    //积分总额
                    //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MJB", true);
                    //现金
                    if(fhLog.FHMoney > 0)
                        MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                }
            }
            if(isReduceAgentTreadPoint)
            {
                //添加分红记录
                decimal fh_money = money - firstLeaveFHMoney - agentHasFHMoney;
                string fh_remark = "会员" + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元;其他分红之后剩余" + fh_money + "元。";
                TD_FHLog fhLog = new TD_FHLog();
                AddFHLog(member, money, fh_money, MCode, mid, agentRoleCode, fh_remark, "999999", listComm, out fhLog);
                //更新会员的积分
                decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                decimal MJB = fhLog.FHMoney - MSH;
                //积分总额
                //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MJB", true);
                //现金
                if(fhLog.FHMoney > 0)
                    MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                //消耗该代理商的一个端口
                //减去一个端口
                string updatesql = " UPDATE dbo.Member SET TradePoints=TradePoints-1 WHERE ID=" + fhLog.MID;
                listComm.Add(new CommonObject(updatesql, null));
                LogService.Log(agentMember, "10", fhLog.Remark + "扣除1个端口", listComm);
                //}
            }
            else
            {
                //正常三级分销的话扣除管理员的一个端口
                AdminTradePointReduce(member, money, listComm);
            }
            return result;
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
        public static void AddFHLog(Model.Member payMember, decimal payMoney, decimal fhMoney, string fhMID, string fhMemberID, string fhRoleCode, string fhRemark, string fhType, List<CommonObject> listComm, out TD_FHLog fhLogOut)
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
            fhLog.SHMoneyCode = fhType;
            fhLog.Remark = fhRemark;
            //fhLog.FHType = payMember.YunPay;
            if(fhLog.FHMoney > 0)
                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
            fhLogOut = fhLog;
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

        /// <summary>
        /// 代理商奖励
        /// </summary>
        /// <param name="member">缴费会员</param>
        /// <param name="money">缴费金额</param>
        /// <param name="listComm"></param>
        public static void AgentChangMoney2(Model.Member member, decimal money, List<CommonObject> listComm)
        {
            //缴费会员的直接归属代理商
            string agentId = member.Agent;
            if(string.IsNullOrEmpty(agentId))
            {
                //查询到这个代理商
                Member agentMember = CommonBase.GetModel<Member>(agentId); //直接归属代理商
                if(agentMember != null)
                {
                    //查看这个代理商使用的是哪种模式：1--省市县区；2--一二三级
                    int agentType = agentMember.UseRoleType;
                    if(agentType == 1)
                    {
                        #region 1--省市县区；
                        //1--省市县区
                        SqlParameter[] parameters = {
                                   new SqlParameter("@ID", SqlDbType.Int)
                         };
                        parameters[0].Value = agentMember.AreaId;
                        //得到地址信息表中的区域
                        DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_GetUpperAddress");
                        //查看该地区改O单商有没有代理商
                        for(int m = 0; m < dtUpperAddressTbl.Rows.Count; m++)
                        {
                            string checkIsExist = "select count(1) from Member where AreaId=" + dtUpperAddressTbl.Rows[m]["ID"].ToString() + " and Company=" + agentMember.Company;
                            object objIsExist = CommonBase.GetSingle(checkIsExist);
                            if(objIsExist == null || Convert.ToInt16(objIsExist) == 0)
                            {
                                dtUpperAddressTbl.Rows.Remove(dtUpperAddressTbl.Rows[m]);
                            }
                        }

                        //update at 2016年12月25日00:32:06
                        //对于停止发放补贴的代理商剔除出去
                        DataRow[] drSelectRows = dtUpperAddressTbl.Select("IsFH='0'");
                        foreach(DataRow drRows in drSelectRows)
                        {
                            DataRow[] temp = dtUpperAddressTbl.Select("ID=" + drRows["ID"].ToString());
                            if(temp != null && temp.Length > 0)
                            {
                                dtUpperAddressTbl.Rows.Remove(temp[0]);
                            }
                        }


                        ////找到分红配置表中的分红配置表，找到，AgentFloat的
                        ////20-省级，30-市级；40-区县级；50-乡镇级
                        List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat").ToList();
                        //省市区县的推荐人分红
                        List<TD_SHMoney> MTJAgentMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "TJAgentFloat").ToList();

                        //对这个DataTable进行排序
                        DataRow[] addressRows = dtUpperAddressTbl.Select(string.Empty, "rank asc");
                        foreach(DataRow row in addressRows)
                        {
                            //看第一个人是不是区县级
                            string leavel = row["Level"].ToString().Trim(), fhRemark = string.Empty, tjFHRemark = string.Empty;
                            int fhTypeId = 0, tjFHTypeID = 0;
                            //代理商分红金额
                            decimal fhMoney = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).Sum(c => c.TJFloat);
                            //代理商推荐人分红金额
                            decimal tjFHMoney = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).Sum(c => c.TJFloat);
                            if(addressRows.Length == 1)
                            {
                                if(leavel == "20") { }
                                //如果直接是省级代理商(addressRows只有一行)，就把省级，市级，区县级的奖励也拿了
                                var listCopy = shMoneyList.Where(c => c.TJIndex >= Convert.ToInt32(leavel));
                                fhTypeId = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                //省级得到三个分红
                                fhMoney = listCopy.Sum(c => c.TJFloat);
                                string moneyAdd = string.Empty;
                                foreach(var obj in listCopy)
                                {
                                    moneyAdd += obj.TJFloat + "+";
                                }
                                moneyAdd = moneyAdd.TrimEnd('+');
                                fhRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyAdd + "元";

                                //代理商推荐人
                                //如果直接是省级代理商(addressRows只有一行)，就把省级，市级，区县级的奖励也拿了
                                var listMTJCopy = MTJAgentMoneyList.Where(c => c.TJIndex >= Convert.ToInt32(leavel));
                                tjFHTypeID = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                tjFHMoney = listMTJCopy.Sum(c => c.TJFloat);
                                string moneyMtjAdd = string.Empty;
                                foreach(var obj in listMTJCopy)
                                {
                                    moneyMtjAdd += obj.TJFloat + "+";
                                }
                                moneyMtjAdd = moneyMtjAdd.TrimEnd('+');
                                tjFHRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyMtjAdd + "元";
                            }
                            else if(addressRows.Length == 2)
                            { //如果直接是市级代理商(addressRows只有两行)，就把市级，区县级的奖励也拿了
                                //fhMoney=shMoneyList.Where(c=>c.TJIndex>=Convert.ToInt32(leavel)).Sum(c=>c.TJFloat);
                                //20-省级；30市级；40-区县级
                                if(leavel == "30")//市级代理商；如果第一个是市级
                                {
                                    var listCopy = shMoneyList.Where(c => c.TJIndex >= Convert.ToInt32(leavel));
                                    //代理商推荐人
                                    //如果直接是省级代理商(addressRows只有一行)，就把省级，市级，区县级的奖励也拿了
                                    var listMTJCopy = MTJAgentMoneyList.Where(c => c.TJIndex >= Convert.ToInt32(leavel));
                                    bool isExistLeavel = false;
                                    foreach(DataRow selectRow in addressRows)
                                    {
                                        if(selectRow["Level"].ToString() == "40")
                                        {
                                            isExistLeavel = true;
                                            break;
                                        }
                                    }
                                    if(isExistLeavel)
                                    {
                                        listCopy = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel));
                                        listMTJCopy = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel));
                                    }
                                    fhTypeId = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                    fhMoney = listCopy.Sum(c => c.TJFloat);
                                    string moneyAdd = string.Empty;
                                    foreach(var obj in listCopy)
                                    {
                                        moneyAdd += obj.TJFloat + "+";
                                    }
                                    moneyAdd = moneyAdd.TrimEnd('+');
                                    fhRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyAdd + "元";


                                    tjFHTypeID = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                    tjFHMoney = listMTJCopy.Sum(c => c.TJFloat);
                                    string moneyMtjAdd = string.Empty;
                                    foreach(var obj in listMTJCopy)
                                    {
                                        moneyMtjAdd += obj.TJFloat + "+";
                                    }
                                    moneyMtjAdd = moneyMtjAdd.TrimEnd('+');
                                    tjFHRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyMtjAdd + "元";
                                }
                                else if(leavel == "20" || leavel == "40")//省级或区县级
                                {
                                    var listCopy = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel));
                                    var listMTJCopy = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel));
                                    //如果第二行是省级，就是说没有市级代理商，只有省级、区县级代理商，省级代理商也把市级代理商的钱拿走
                                    if(leavel == "20" && dtUpperAddressTbl.Select("Level='30'").Count() <= 0)
                                    {
                                        //没有市级代理商
                                        listCopy = shMoneyList.Where(c => c.TJIndex <= 30);
                                        listMTJCopy = MTJAgentMoneyList.Where(c => c.TJIndex <= 30);
                                    }
                                    fhTypeId = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                    fhMoney = listCopy.Sum(c => c.TJFloat);
                                    string moneyAdd = string.Empty;
                                    foreach(var obj in listCopy)
                                    {
                                        moneyAdd += obj.TJFloat + "+";
                                    }
                                    moneyAdd = moneyAdd.TrimEnd('+');
                                    fhRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyAdd + "元";

                                    //代理商推荐人
                                    //如果直接是省级代理商(addressRows只有一行)，就把省级，市级，区县级的奖励也拿了

                                    tjFHTypeID = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                    tjFHMoney = listMTJCopy.Sum(c => c.TJFloat);
                                    string moneyMtjAdd = string.Empty;
                                    foreach(var obj in listMTJCopy)
                                    {
                                        moneyMtjAdd += obj.TJFloat + "+";
                                    }
                                    moneyMtjAdd = moneyMtjAdd.TrimEnd('+');
                                    tjFHRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + moneyMtjAdd + "元";
                                }
                            }
                            else
                            {
                                //三级都有，各个代理商拿各自的
                                var listCopy = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault();
                                fhTypeId = shMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                fhMoney = listCopy.TJFloat;
                                fhRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + fhMoney + "元";

                                //代理商推荐人
                                //如果直接是省级代理商(addressRows只有一行)，就把省级，市级，区县级的奖励也拿了
                                var listMTJCopy = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault();
                                tjFHTypeID = MTJAgentMoneyList.Where(c => c.TJIndex == Convert.ToInt32(leavel)).FirstOrDefault().Id;
                                tjFHMoney = listMTJCopy.TJFloat;
                                tjFHRemark = member.MID + "在" + DateTime.Now.ToString() + "付款" + money + "元;奖励" + tjFHMoney + "元";
                            }

                            //通过代理地区找到某个代理商，给他分红
                            Model.Member addressAgent = CommonBase.GetList<Member>("AreaId=" + row["ID"].ToString() + " and Company=" + agentMember.Company).FirstOrDefault();
                            if(addressAgent != null)
                            {
                                #region 插入代理商分红记录表
                                //插入代理商分红记录表
                                TD_FHLog fhLog = new TD_FHLog();
                                fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                                fhLog.Company = 0;
                                fhLog.CreatedBy = "SYSTEM";
                                fhLog.CreatedTime = DateTime.Now;
                                fhLog.FHDate = DateTime.Now;
                                fhLog.FHMoney = fhMoney;
                                fhLog.FHRoleCode = addressAgent.RoleCode;
                                fhLog.FHMCode = addressAgent.MID;
                                fhLog.FHType = fhTypeId.ToString();
                                fhLog.IsDeleted = false;
                                fhLog.MID = addressAgent.ID.ToString();
                                fhLog.PayCode = member.ID.ToString();
                                fhLog.Status = 1;
                                fhLog.Remark = fhRemark;
                                if(fhLog.FHMoney > 0)
                                    CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                                //更新会员的积分
                                decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                                decimal MJB = fhLog.FHMoney - MSH;
                                //积分总额
                                //MemberService.UpdateMoney(listComm, fhLog.MID, MJB.ToString(), "MJB", true);
                                //可提现积分总额
                                if(fhLog.FHMoney > 0)
                                    MemberService.UpdateMoney(listComm, fhLog.MID, MSH.ToString(), "MSH", true);
                                #endregion
                                //再找到代理商的推荐人，给他分红
                                #region 插入代理商推荐人分红记录表
                                Model.Member agentTJModel = CommonBase.GetModel<Member>(addressAgent.MTJ);
                                if(agentTJModel != null)
                                {
                                    TD_FHLog TJfhLog = new TD_FHLog();
                                    TJfhLog.Code = MethodHelper.CommonHelper.GetGuid;
                                    TJfhLog.Company = 0;
                                    TJfhLog.CreatedBy = "SYSTEM";
                                    TJfhLog.CreatedTime = DateTime.Now;
                                    TJfhLog.FHDate = DateTime.Now;
                                    TJfhLog.FHMoney = tjFHMoney;
                                    TJfhLog.FHRoleCode = agentTJModel.RoleCode;
                                    TJfhLog.FHMCode = agentTJModel.MID;
                                    TJfhLog.FHType = tjFHTypeID.ToString();
                                    TJfhLog.IsDeleted = false;
                                    TJfhLog.MID = agentTJModel.ID.ToString();
                                    TJfhLog.PayCode = member.ID.ToString();
                                    TJfhLog.Status = 1;
                                    TJfhLog.Remark = tjFHRemark;
                                    if(TJfhLog.FHMoney > 0)
                                        CommonBase.Insert<TD_FHLog>(TJfhLog, listComm);
                                    //更新会员的积分
                                    decimal TJfhLogMSH = TJfhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                                    decimal TJfhLogMJB = TJfhLog.FHMoney - TJfhLogMSH;
                                    //积分总额
                                    //MemberService.UpdateMoney(listComm, fhLog.MID, MJB.ToString(), "MJB", true);
                                    //可提现积分总额
                                    if(TJfhLog.FHMoney > 0)
                                        MemberService.UpdateMoney(listComm, TJfhLog.MID, TJfhLogMSH.ToString(), "MSH", true);
                                }
                                #endregion

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 2--一二三级；

                        //1--等级的树形结构
                        SqlParameter[] parameters = {
                                   new SqlParameter("@MID", SqlDbType.Int),
                                   new SqlParameter("@RoleCode", SqlDbType.VarChar)
                         };
                        parameters[0].Value = member.ID;
                        parameters[1].Value = "";
                        DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperAgent");

                        //update at 2016年12月25日00:32:06
                        //对于停止发放补贴的代理商剔除出去
                        DataRow[] drSelectRows = dtUpperAddressTbl.Select("IsFH='0'");
                        foreach(DataRow drRows in drSelectRows)
                        {
                            DataRow[] temp = dtUpperAddressTbl.Select("ID=" + drRows["ID"].ToString());
                            if(temp != null && temp.Length > 0)
                            {
                                dtUpperAddressTbl.Rows.Remove(temp[0]);
                            }
                        }


                        ////找到分红配置表中的分红配置表，找到，AgentFloat的
                        ////20-省级/一级代理商代理提成，30-市级/二级代理商代理提成；40-区县级/三级代理商代理提成；50-乡镇级
                        List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "AgentFloat").ToList();
                        //一二三级代理商的的推荐人分红
                        List<TD_SHMoney> MTJAgentMoneyList = CacheService.SHMoneyList.Where(c => c.Code == "TJAgentFloat").ToList();

                        //对这个DataTable进行排序
                        DataRow[] addressRows = dtUpperAddressTbl.Select(string.Empty, "RANK asc");
                        //2017年6月5日22:17:26，新修改的
                        foreach(DataRow row in addressRows)
                        {
                            int leavel = Convert.ToInt16(row["AreaId"].ToString().Trim());
                            var listAgentSHMoney = shMoneyList.Where(c => c.TJIndex >= leavel).ToList();
                            for(int t = 0; t < listAgentSHMoney.Count(); t++)
                            {
                                TD_SHMoney shmoney = listAgentSHMoney[t];
                                if(shmoney.TJIndex != leavel)
                                {
                                    DataRow[] addressRowsExcept = dtUpperAddressTbl.Select("AreaId=" + shmoney.TJIndex);
                                    if(addressRowsExcept.Length > 0)
                                    {
                                        //从listAgentSHMoney删除
                                        listAgentSHMoney.Remove(shmoney);
                                    }
                                }
                            }
                            //同样分销商推荐人的提成也是一样的
                            List<TD_SHMoney> listAgentMTJSHMoneyList = new List<TD_SHMoney>();
                            foreach(TD_SHMoney shmoney in listAgentSHMoney)
                            {
                                TD_SHMoney mthShmoney = MTJAgentMoneyList.Where(c => c.TJIndex == shmoney.TJIndex).FirstOrDefault();
                                if(mthShmoney != null)
                                    listAgentMTJSHMoneyList.Add(mthShmoney);
                            }
                            //对分销商及推荐人进行分红
                            string fhRemark = string.Empty, tjFHRemark = string.Empty, agentID = row["ID"].ToString(), agentMID = row["MID"].ToString(), agentRoleCode = row["RoleCode"].ToString();
                            int fhTypeId = 0, tjFHTypeID = 0;
                            decimal fhMoney = 0, tjFHMoney = 0;

                            #region 分销商分红
                            AgentFHMoneyAndFHRemark(member, money, listAgentSHMoney, leavel, out fhTypeId, out fhMoney, out fhRemark);
                            TD_FHLog fhLog = new TD_FHLog();
                            if(fhTypeId > 0)
                                AddFHLog(member, money, fhMoney, agentMID, agentID, agentRoleCode, fhRemark, fhTypeId.ToString(), listComm, out fhLog);
                            //更新会员的积分
                            decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                            decimal MJB = fhLog.FHMoney - MSH;
                            //积分总额
                            //MemberService.UpdateMoney(listComm, fhLog.MID, MJB.ToString(), "MJB", true);
                            //可提现积分总额
                            if(fhLog.FHMoney > 0)
                                MemberService.UpdateMoney(listComm, fhLog.MID, MSH.ToString(), "MSH", true);
                            #endregion

                            #region 分销商推荐人分红
                            //代理商推荐人
                            AgentFHMoneyAndFHRemark(member, money, listAgentMTJSHMoneyList, leavel, out tjFHTypeID, out tjFHMoney, out tjFHRemark);
                            //添加分红信息
                            Model.Member agentTJModel = CommonBase.GetModel<Member>(row["MTJ"].ToString());
                            if(agentTJModel != null)
                            {
                                fhLog = new TD_FHLog();
                                if(tjFHTypeID > 0)
                                    AddFHLog(member, money, tjFHMoney, agentTJModel.MID, agentTJModel.ID.ToString(), agentTJModel.RoleCode, tjFHRemark, tjFHTypeID.ToString(), listComm, out fhLog);
                                //更新会员的积分
                                MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                                MJB = fhLog.FHMoney - MSH;
                                //积分总额
                                //MemberService.UpdateMoney(listComm, fhLog.MID, MJB.ToString(), "MJB", true);
                                //可提现积分总额
                                if(fhLog.FHMoney > 0)
                                    MemberService.UpdateMoney(listComm, fhLog.MID, MSH.ToString(), "MSH", true);
                            }
                            #endregion
                        }

                        #endregion
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

        /// <summary>
        /// 月分红
        /// </summary>
        /// <param name="fhDate"></param>
        /// <param name="listComm"></param>
        public static void MonthFH(decimal totalmoney, int count, List<CommonObject> listComm)
        {
            if(totalmoney > 0 && count > 0)
            {

                TD_SHMoney shmoney = CacheService.SHMoneyList.FirstOrDefault(c => c.Code == "MFH");
                if(shmoney == null)
                    return;
                //总收入的20%
                decimal totalFHMoney = totalmoney * shmoney.TJFloat;
                decimal everyFHMoney = totalFHMoney / count;
                //上月城市合伙人
                string sqlCount = "SELECT t1.PayType, t2.ID,t2.MID,t2.RoleCode FROM dbo.TD_PayLog t1 RIGHT JOIN dbo.Member t2 ON t1.PayID=t2.ID WHERE t2.RoleCode IN ('3F') AND MONTH(PayTime)=(CASE MONTH(GETDATE()) WHEN 1 THEN 12 ELSE MONTH(GETDATE())-1 END)-1 AND t1.Remark LIKE '%城市合伙人%'";
                DataTable dt = CommonBase.GetTable(sqlCount);
                foreach(DataRow row in dt.Rows)
                {
                    TD_FHLog fhLog = new TD_FHLog();
                    fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                    fhLog.Company = 0;
                    fhLog.CreatedBy = "SYSTEM";
                    fhLog.CreatedTime = DateTime.Now;
                    fhLog.FHDate = DateTime.Now;
                    fhLog.FHMoney = everyFHMoney;
                    fhLog.FHRoleCode = row["RoleCode"].ToString();
                    fhLog.FHMCode = row["MID"].ToString();
                    fhLog.FHType = row["PayType"].ToString();
                    fhLog.IsDeleted = false;
                    fhLog.SHMoneyCode = shmoney.Id.ToString();
                    fhLog.MID = row["ID"].ToString();
                    fhLog.PayCode = "SYSTEM";
                    fhLog.Status = 1;
                    fhLog.Remark = DateTime.Now.Month + "月分红";
                    CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                    decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                    string moneyType = "MSH";
                    switch(fhLog.FHType)
                    {
                        case "Cash":
                        case "WXpay": moneyType = "MSH"; break;
                        case "Tpay": moneyType = "MJB"; break;
                        case "Vpay": moneyType = "MVB"; break;
                    }
                    //更新代理商账户资金
                    MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), moneyType, true);
                }
            }
        }

        public static void MonthFH(decimal totalmoney, string[] arrayMemberCode, List<CommonObject> listComm)
        {
            if(totalmoney > 0 && arrayMemberCode.Length > 0)
            {
                decimal everyFHMoney = totalmoney / arrayMemberCode.Length;
                foreach(string code in arrayMemberCode)
                {
                    //上月城市合伙人
                    string sqlCount = "SELECT t1.PayType, t2.ID,t2.MID,t2.RoleCode FROM dbo.TD_PayLog t1 RIGHT JOIN dbo.Member t2 ON t1.PayID=t2.ID WHERE t2.ID='" + code + "' ORDER BY t1.PayTime DESC";
                    DataTable dt = CommonBase.GetTable(sqlCount);
                    if(dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        #region 分红表
                        TD_FHLog fhLog = new TD_FHLog();
                        fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                        fhLog.Company = 0;
                        fhLog.CreatedBy = "SYSTEM";
                        fhLog.CreatedTime = DateTime.Now;
                        fhLog.FHDate = DateTime.Now;
                        fhLog.FHMoney = everyFHMoney;
                        fhLog.FHRoleCode = row["RoleCode"].ToString();
                        fhLog.FHMCode = row["MID"].ToString();
                        fhLog.FHType = row["PayType"].ToString();
                        fhLog.IsDeleted = false;
                        fhLog.SHMoneyCode = "190";
                        fhLog.MID = row["ID"].ToString();
                        fhLog.PayCode = DateTime.Now.ToString("yyyy-MM");
                        fhLog.Status = 1;
                        fhLog.Remark = DateTime.Now.ToString("yyyy-MM") + "月分红";
                        CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                        decimal MSH = fhLog.FHMoney * 1;// CacheService.GlobleConfig.TXPart;
                        string moneyType = "MJB";
                        switch(fhLog.FHType)
                        {
                            case "Cash":
                            case "WXpay": moneyType = "MSH"; break;
                            case "Tpay": moneyType = "MJB"; break;
                            case "Vpay": moneyType = "MVB"; break;
                        }
                        //更新代理商账户资金
                        MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), moneyType, true);
                        #endregion

                        #region 分红明细
                        SH_HirePurchaseDetail hire = new SH_HirePurchaseDetail();
                        hire.Code = MethodHelper.CommonHelper.GetGuid;
                        hire.HirePurchaseId = fhLog.PayCode;
                        hire.UserId = fhLog.MID;
                        hire.UserCode = fhLog.FHMCode;
                        hire.HireTotalCount = arrayMemberCode.Length;
                        hire.HireCount = arrayMemberCode.Length;
                        hire.HireMoney = fhLog.FHMoney;
                        hire.PayDate = DateTime.Now;
                        hire.PayStatus = 1;
                        hire.RealPayDateTime = fhLog.FHDate;
                        hire.Remark = fhLog.Remark;
                        hire.TradePointCount = 0;
                        hire.LeaveTradePointCount = 0;
                        hire.CreatedBy = "SYSTEM";
                        hire.CreatedTime = DateTime.Now;
                        CommonBase.Insert<Model.SH_HirePurchaseDetail>(hire, listComm);
                        #endregion
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
        /// 申请代理商发放三级分销推荐奖()
        /// </summary>
        /// <param name="member">充值会员</param>
        /// <param name="money">充值金额</param>
        public static bool AgentTJChangeMoney(Model.Member member, decimal money, string upToAgent, List<CommonObject> listComm)
        {
            bool result = true;
            //存储过程查出来该会员的上级推荐关系信息
            SqlParameter[] parameters = {
                    new SqlParameter("@MID", SqlDbType.VarChar, 50),
                    new SqlParameter("@RoleCode", SqlDbType.VarChar, 50)
                 };
            parameters[0].Value = member.ID;
            parameters[1].Value = "";
            DataTable dtUpperTJMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperMember");
            DataTable dtResult = dtUpperTJMemberTbl.Clone();
            //只筛查出会员级别
            foreach(DataRow dr in dtUpperTJMemberTbl.Rows)
            {
                if(dr["RoleCode"].ToString() == "VIP" || dr["RoleCode"].ToString() == "Member")
                {
                    dtResult.ImportRow(dr);
                }
            }
            //重新设置Rank
            int i = 1;
            foreach(DataRow dr in dtResult.Rows)
            {
                dr["RANK"] = i;
                i++;
            }

            //找到分红配置表中的分红配置表，找到，TJFloat的
            List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == upToAgent).OrderBy(c => c.TJIndex).ToList();
            string applyName = string.Empty, tdMemberName = string.Empty;
            foreach(TD_SHMoney shmoney in shMoneyList)
            {
                bool isContinue = false;
                if(shmoney.Code == "ApplyAgentTo3F")
                    applyName = "申请成为分销商";
                else if(shmoney.Code == "ApplyAgentTo2F")
                    applyName = "申请成为服务中心";
                if(shmoney.TJIndex == 1)
                    tdMemberName = "直接推荐会员";
                else if(shmoney.TJIndex == 2)
                    tdMemberName = "间接一级会员";
                else if(shmoney.TJIndex == 3)
                    tdMemberName = "间接二级会员";
                DataRow[] drSelect = dtResult.Select("RANK>=" + shmoney.TJIndex + " AND RoleCode in ('VIP','Member')", "RANK ASC");
                if(drSelect.Length > 0)
                {
                    //财臣卡管家如果是低级别推荐高级别的，只拿一层的700元
                    //善融的是不分级别，只要推荐的人交钱上面的人都有三级分销的钱分
                    if(MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj02") // || MethodHelper.ConfigHelper.GetAppSettings("SystemID") != "kgjpersonaltest")//如果是财臣系统
                    {
                        //查看直接推荐人的级别
                        string firstTJMember = drSelect[0]["MTJ"].ToString();
                        Model.Member upMember = null;
                        bool tjMemberIsAgent = MemberService.IsAgentMember(firstTJMember, out upMember);
                        if(tjMemberIsAgent) //缴费会员的推荐人是分销商
                        {
                            //查看分销商的级别，如果是大于要缴费会员要升级的，就往下走，如果是小于要缴费会员的申请级别，就则就拿一层，需要跳出循环
                            if(shmoney.Code == "ApplyAgentTo2F")
                            {
                                if(upMember.Role.AreaLeave == "40")
                                {
                                    isContinue = true;
                                }
                            }
                        }
                        else
                        {
                            isContinue = true;
                        }
                        if(isContinue && shmoney.TJIndex == 1)
                        {
                            isContinue = false;
                        }
                    }

                    if(isContinue)
                        continue;

                    DataRow drFHMember = drSelect[0];
                    #region 添加分红记录
                    decimal fhMoney = shmoney.TJFloat * money;
                    if(shmoney.Field3 == "2")
                    {
                        fhMoney = shmoney.TJFloat;
                    }
                    string Remark = tdMemberName + member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元" + applyName + ";奖励" + fhMoney + "元";
                    TD_FHLog fhLog = new TD_FHLog();
                    AddFHLog(member, money, fhMoney, drFHMember["MID"].ToString(), drFHMember["ID"].ToString(), drFHMember["RoleCode"].ToString(), Remark, shmoney.Id.ToString(), listComm, out fhLog);
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
            return result;
        }

        public static bool TrainPayChangeMoney(TD_PayLog payModel, Model.Member member, decimal money, string upToAgent, List<CommonObject> listComm)
        {
            bool result = true;
            //存储过程查出来该会员的上级推荐关系信息
            SqlParameter[] parameters = {
                    new SqlParameter("@MID", SqlDbType.VarChar, 50),
                    new SqlParameter("@RoleCode", SqlDbType.VarChar, 50)
                 };
            parameters[0].Value = member.ID;
            parameters[1].Value = "";
            DataTable dtUpperTJMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperMember");
            //找到分红配置表中的分红配置表，找到，TJFloat的
            List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.Code == payModel.ProductCode).OrderBy(c => c.TJIndex).ToList();
            string applyName = string.Empty, tdMemberName = string.Empty;
            foreach(TD_SHMoney shmoney in shMoneyList)
            {
                if(shmoney.RoleCode == "Teacher")
                {
                    //教师分红
                    //获取到教师
                    Model.Member teacherMember = CommonBase.GetModel<Model.Member>(member.ParentTrade);
                    if(teacherMember != null)
                    {
                        #region 添加分红记录
                        decimal fhMoney = shmoney.TJFloat * money;
                        if(shmoney.Field3 == "2")
                        {
                            fhMoney = shmoney.TJFloat;
                        }
                        string Remark = member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元开通记忆训练；奖励" + fhMoney + "元";
                        TD_FHLog fhLog = new TD_FHLog();
                        AddFHLog(member, money, fhMoney, teacherMember.MID, teacherMember.ID, teacherMember.RoleCode, Remark, shmoney.Id.ToString(), listComm, out fhLog);
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
                else
                {
                    DataRow[] drSelect = dtUpperTJMemberTbl.Select("RoleCode='" + shmoney.RoleCode + "'");
                    if(drSelect.Length <= 0)
                        continue;
                    DataRow drFHMember = drSelect[0];
                    #region 添加分红记录
                    decimal fhMoney = shmoney.TJFloat * money;
                    if(shmoney.Field3 == "2")
                    {
                        fhMoney = shmoney.TJFloat;
                    }
                    string Remark = member.MID + "在" + DateTime.Now.ToString() + "缴费" + money + "元开通记忆训练；奖励" + fhMoney + "元";
                    TD_FHLog fhLog = new TD_FHLog();
                    AddFHLog(member, money, fhMoney, drFHMember["MID"].ToString(), drFHMember["ID"].ToString(), drFHMember["RoleCode"].ToString(), Remark, shmoney.Id.ToString(), listComm, out fhLog);
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
            return result;
        }




        public static void ServicePayChangeMoney(Model.Member payMember, decimal money, List<CommonObject> listComm, out List<TD_FHLog> listFHLog)
        {
            List<TD_FHLog> fhListLog = new List<TD_FHLog>();
            //查询到上级服务中心
            string upSql = "select * from dbo.FUN_CountUpperMember('" + payMember.ID + "',0,9999) where RoleCode in('1F', '2F','3F')";
            DataTable dtUpperTJMemberTbl = CommonBase.GetTable(upSql);
            if(dtUpperTJMemberTbl != null && dtUpperTJMemberTbl.Rows.Count > 0)
            {
                //查找到这个服务中心
                string serviceRowCode = dtUpperTJMemberTbl.Rows[0]["Code"].ToString();
                //查找到这个服务中心的奖励配置
                List<TD_SHMoney> shMoneyList = CacheService.SHMoneyList.Where(c => c.RoleCode == serviceRowCode).ToList();
                if (shMoneyList.Count == 0)
                {
                    #region 服务中心剩余分红
                    if (money > 0)
                    {
                        string Remark = payMember.MID + "在" + DateTime.Now.ToString() + "升级成学员，发放服务中心奖励";
                        TD_FHLog fhLog = new TD_FHLog();
                        fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                        fhLog.Company = 0;
                        fhLog.CreatedBy = "SYSTEM";
                        fhLog.CreatedTime = DateTime.Now;
                        fhLog.FHDate = DateTime.Now;
                        fhLog.FHMoney = money;
                        fhLog.FHRoleCode = "";
                        fhLog.FHType = "KCTJ";
                        fhLog.IsDeleted = false;
                        fhLog.FHMCode = dtUpperTJMemberTbl.Rows[0]["MID"].ToString();
                        fhLog.MID = serviceRowCode;
                        fhLog.PayCode = payMember.ID;
                        fhLog.Status = 2;
                        fhLog.Remark = Remark;
                        fhLog.SHMoneyCode = "KCTJ";
                        fhLog.ProductCode = "1";
                        if (fhLog.FHMoney > 0)
                        {
                            fhListLog.Add(fhLog);
                            CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                            //账户余额增加
                            //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                        }
                    }
                    #endregion
                }
                foreach (TD_SHMoney shmoney in shMoneyList)
                {
                    if(shmoney.Code == "GuDong")
                    {
                        #region 股东分红
                        decimal totalFHMoney = money * shmoney.TJFloat;
                        decimal totalLeavelMoney = 0, totalAlreadyMoney = 0;
                        //再查询出来所有要分钱的用户信息
                        List<SH_HirePurchaseDetail> purchDetailList = CommonBase.GetList<SH_HirePurchaseDetail>("HirePurchaseId='" + serviceRowCode + "' and PayStatus=1");
                        foreach(SH_HirePurchaseDetail detail in purchDetailList)
                        {
                            decimal fhMoney = detail.HireMoney * totalFHMoney;
                            totalAlreadyMoney += fhMoney;
                            #region 发放推荐奖励
                            string Remark = payMember.MID + "在" + DateTime.Now.ToString() + "升级成学员，服务中心发放奖励发放" + fhMoney + "元代言红包";
                            TD_FHLog fhLog = new TD_FHLog();
                            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                            fhLog.Company = 0;
                            fhLog.CreatedBy = "SYSTEM";
                            fhLog.CreatedTime = DateTime.Now;
                            fhLog.FHDate = DateTime.Now;
                            fhLog.FHMoney = fhMoney;
                            fhLog.FHRoleCode = "";
                            fhLog.FHType = shmoney.Code;
                            fhLog.IsDeleted = false;
                            fhLog.FHMCode = detail.UserCode;
                            fhLog.MID = detail.UserId;
                            fhLog.PayCode = payMember.ID;
                            fhLog.Status = 2;
                            fhLog.Remark = Remark;
                            fhLog.SHMoneyCode = shmoney.Code;
                            fhLog.ProductCode = "1";
                            if(fhLog.FHMoney > 0)
                            {
                                fhListLog.Add(fhLog);
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                                //账户余额增加
                                //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                            }
                            #endregion
                        }
                        //剩余金额，剩余金额=总分红金额-已分红金额
                        totalLeavelMoney = totalFHMoney - totalAlreadyMoney;
                        #region 服务中心剩余分红
                        if(totalLeavelMoney > 0)
                        {
                            string Remark = payMember.MID + "在" + DateTime.Now.ToString() + "升级成学员，发放股东奖励之后剩余";
                            TD_FHLog fhLog = new TD_FHLog();
                            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                            fhLog.Company = 0;
                            fhLog.CreatedBy = "SYSTEM";
                            fhLog.CreatedTime = DateTime.Now;
                            fhLog.FHDate = DateTime.Now;
                            fhLog.FHMoney = totalLeavelMoney;
                            fhLog.FHRoleCode = "";
                            fhLog.FHType = shmoney.Code;
                            fhLog.IsDeleted = false;
                            fhLog.FHMCode = dtUpperTJMemberTbl.Rows[0]["MID"].ToString();
                            fhLog.MID = serviceRowCode;
                            fhLog.PayCode = payMember.ID;
                            fhLog.Status = 2;
                            fhLog.Remark = Remark;
                            fhLog.SHMoneyCode = shmoney.Code;
                            fhLog.ProductCode = "1";
                            if(fhLog.FHMoney > 0)
                            {
                                fhListLog.Add(fhLog);
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                                //账户余额增加
                                //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                            }
                        }
                        #endregion
                        #endregion
                    }
                    else if(shmoney.Code == "LaoShi")
                    {
                        #region 老师分红
                        decimal totalFHMoney = money * shmoney.TJFloat;
                        decimal totalLeavelMoney = 0, totalAlreadyMoney = 0;
                        //再查询出来所有要分钱的用户信息
                        List<SH_HirePurchaseDetail> purchDetailList = CommonBase.GetList<SH_HirePurchaseDetail>("HirePurchaseId='" + serviceRowCode + "' and PayStatus=2");
                        foreach(SH_HirePurchaseDetail detail in purchDetailList)
                        {
                            decimal fhMoney = detail.HireMoney * totalFHMoney;
                            totalAlreadyMoney += fhMoney;
                            #region 发放推荐奖励
                            string Remark = payMember.MID + "在" + DateTime.Now.ToString() + "升级成学员，服务中心发放奖励发放" + fhMoney + "元代言红包";
                            TD_FHLog fhLog = new TD_FHLog();
                            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                            fhLog.Company = 0;
                            fhLog.CreatedBy = "SYSTEM";
                            fhLog.CreatedTime = DateTime.Now;
                            fhLog.FHDate = DateTime.Now;
                            fhLog.FHMoney = fhMoney;
                            fhLog.FHRoleCode = "";
                            fhLog.FHType = shmoney.Code;
                            fhLog.IsDeleted = false;
                            fhLog.FHMCode = detail.UserCode;
                            fhLog.MID = detail.UserId;
                            fhLog.PayCode = payMember.ID;
                            fhLog.Status = 2;
                            fhLog.Remark = Remark;
                            fhLog.SHMoneyCode = shmoney.Code;
                            fhLog.ProductCode = "1";
                            if(fhLog.FHMoney > 0)
                            {
                                fhListLog.Add(fhLog);
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                                //账户余额增加
                                //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                            }
                            #endregion
                        }
                        //剩余金额，剩余金额=总分红金额-已分红金额
                        totalLeavelMoney = totalFHMoney - totalAlreadyMoney;
                        #region 服务中心剩余分红
                        if(totalLeavelMoney > 0)
                        {
                            string Remark = payMember.MID + "在" + DateTime.Now.ToString() + "升级成学员，发放老师奖励之后剩余";
                            TD_FHLog fhLog = new TD_FHLog();
                            fhLog.Code = MethodHelper.CommonHelper.GetGuid;
                            fhLog.Company = 0;
                            fhLog.CreatedBy = "SYSTEM";
                            fhLog.CreatedTime = DateTime.Now;
                            fhLog.FHDate = DateTime.Now;
                            fhLog.FHMoney = totalLeavelMoney;
                            fhLog.FHRoleCode = "";
                            fhLog.FHType = shmoney.Code;
                            fhLog.IsDeleted = false;
                            fhLog.FHMCode = dtUpperTJMemberTbl.Rows[0]["MID"].ToString();
                            fhLog.MID = serviceRowCode;
                            fhLog.PayCode = payMember.ID;
                            fhLog.Status = 2;
                            fhLog.Remark = Remark;
                            fhLog.SHMoneyCode = shmoney.Code;
                            fhLog.ProductCode = "1";
                            if(fhLog.FHMoney > 0)
                            {
                                fhListLog.Add(fhLog);
                                CommonBase.Insert<TD_FHLog>(fhLog, listComm);
                                //账户余额增加
                                //MemberService.UpdateMoney(listComm, fhLog.MID, fhLog.FHMoney.ToString(), "MSH", true);
                            }
                        }
                        #endregion
                        #endregion
                    }
                }

            }
            listFHLog = fhListLog;
        }


        public static bool sendTotalPrizeMoney(decimal prizeMoney, string date)
        {
            var url = "http://jwy.u1200.com/api/v1/user/prize/total";
            string bodyJson = "{ \"prize_type\": 1, \"prize_money\": " + (int)(prizeMoney * 100) + ",  \"prize_date\": \"" + date + "\", \"at_time\": " + new Random().Next(100000, 999999) + " }";
            //string bodyJson = "{ \"prize_type\": 1, \"prize_money\": \"wuyew11\",  \"prize_date\": \"" + date + "\", \"at_time\": " + new Random().Next(100000, 999999) + " }";
            string result = WebRemoteRequest.RequestWebApi(url, bodyJson);
            RemoteReturnMsg obj = Newtonsoft.Json.JsonConvert.DeserializeObject<RemoteReturnMsg>(result);
            if(obj != null && obj.result == "1")
            {
                MethodHelper.LogHelper.WriteTextLog("sendTotalPrizeMoney", "bodyJson:" + bodyJson + "\r\n" + date + "总红包金额：" + prizeMoney, DateTime.Now);
                return true;
            }
            else
            {
                return false;
            }
        }

        public class RemoteReturnMsg
        {
            public string result { get; set; }
            public string msg { get; set; }
            public string at_time { get; set; }
        }

    }
}
