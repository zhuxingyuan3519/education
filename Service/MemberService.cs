using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Service
{
    public class MemberService
    {
        public static void UpdateMoney(List<CommonObject> listComm, string memberCode, string money, string moneyType, bool IsAdd)
        {
            string sql = "UPDATE Member SET " + moneyType + "=ISNULL(" + moneyType + ",0)+" + money + " WHERE ID='" + memberCode + "'";
            if(!IsAdd)
            {
                sql = "UPDATE Member SET " + moneyType + "=ISNULL(" + moneyType + ",0)-" + money + " WHERE ID='" + memberCode + "'";
            }
            listComm.Add(new CommonObject(sql, null));
        }


        /// <summary>
        /// 激活会员为VIP会员，并执行一系列的分红
        /// </summary>
        /// <param name="activeMember"></param>
        /// <param name="activeMoney"></param>
        /// <param name="listComm"></param>
        public static void ActiveMemberToVIP(Model.Member activeMember, decimal activeMoney, List<CommonObject> listComm)
        {
            activeMember.RoleCode = "VIP";
            //使用时间时长改变
            activeMember.UseBeginTime = DateTime.Now;
            activeMember.UseEndTime = DateTime.Now.AddDays(MethodHelper.ConvertHelper.ToDouble(CacheService.GlobleConfig.Field3, 365));
            //修改会员角色
            CommonBase.Update<Model.Member>(activeMember, new string[] { "RoleCode", "UseBeginTime", "UseEndTime" }, listComm);

            //执行三级分销分红
            bool idActiveAgentFH = false;
            //if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj05" || MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgjpersonaltest")
            if(false)
            {
                //善融卡管家的三级分销奖励不一样
                idActiveAgentFH = SHMoneyService.SRTJChangeMoney(activeMember, activeMoney, listComm);
            }
            else
            {
                idActiveAgentFH = SHMoneyService.TJChangeMoney(activeMember, activeMoney, listComm, "0", 0, "");
            }
            if(idActiveAgentFH) //是否执行代理商分红，（如果代理商把剩余的钱都拿走了，就执行代理商分红了）
            {
                //执行代理商分红
                if(MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00")
                {
                    //洛胜卡管家代理商分红
                    SHMoneyService.AgentChangMoneyForKgj00(activeMember, activeMoney, listComm);
                }
                else
                {
                    //执行代理商分红
                    SHMoneyService.AgentChangMoney2(activeMember, activeMoney, listComm);
                }
            }
        }


        public static bool IsInValidTime(Model.Member member)
        {
            bool result = true;
            //账号在有效期
            DateTime dtBegin = member.UseBeginTime != null ? Convert.ToDateTime(member.UseBeginTime) : DateTime.Now.AddDays(-1);
            DateTime dtEnd = member.UseEndTime != null ? Convert.ToDateTime(member.UseEndTime) : DateTime.Now.AddDays(1);
            if(member.RoleCode == "VIP")
            {
                int vipDays = ConvertHelper.ToInt32(CacheService.GlobleConfig.Field3, 365);
                DateTime validTime = dtBegin.AddDays(vipDays);
                double days = (validTime - DateTime.Now).TotalDays;
                if(days < 0)
                {
                    result = false;
                }
                double days1 = (dtEnd - DateTime.Now).TotalDays;
                if(days1 < 0)
                {
                    result = false;
                }
            }
            if(member.RoleCode == "Member")
            {
                int vipDays = ConvertHelper.ToInt32(CacheService.GlobleConfig.Field2, 1);
                DateTime validTime = dtBegin.AddDays(vipDays);
                double days = (validTime - DateTime.Now).TotalDays;
                if(days < 0)
                {
                    result = false;
                }
                double days1 = (dtEnd - DateTime.Now).TotalDays;
                if(days1 < 0)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 创建代理商的逻辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tjModel">推荐人</param>
        /// <param name="TModel">操作人</param>
        /// <param name="roleCode">要创建的角色</param>
        /// <param name="useRoleType">使用的代理类型；1-省市区县模式；2-一二三级模式</param>
        public static void CreateAgent(Model.Member model, Model.Member tjModel, Model.Member TModel, string roleCode, string useRoleType)
        {
            model.MTJ = tjModel.ID.ToString();
            model.RoleCode = roleCode;
            if(TModel.RoleCode == "Manage")
                model.UseRoleType = int.Parse(useRoleType);
            if(model.RoleCode != "Admin")
                model.UseRoleType = CacheService.RoleList.Where(c => c.Code == model.RoleCode).FirstOrDefault().RoleType;

            if(TModel.RoleCode == "Admin")
            {
                model.Company = TModel.ID;
                model.Agent = TModel.ID;
            }
            else
            {
                if(TModel.RoleCode == "Admin")
                    model.Company = TModel.ID;
                else
                {
                    //查询到O单商
                    if(model.RoleCode == "Admin")
                        model.Company = string.Empty;
                    else
                        model.Company = CommonBase.GetList<Model.Member>("RoleCode='Admin'").FirstOrDefault().ID;
                }
            }

            //代理地区AreaId
            //如果是省市区县代理商，代理地区就是下拉列表的地区
            #region 如果是省市区县代理商
            if(model.UseRoleType == 1)
            {
                Sys_Role role = CacheService.RoleList.Where(c => c.Code == model.RoleCode).FirstOrDefault();
                string areaId = model.Province;
                if(model.RoleCode != "Admin")
                    model.AreaId = Convert.ToInt32(areaId);
            }
            #endregion
            #region 以一二三级代理商模式走的
            else
            {
                //以一二三级代理商模式走的
                if(model.RoleCode != "Admin")
                    model.AreaId = Convert.ToInt32(CacheService.RoleList.Where(c => c.Code == model.RoleCode).FirstOrDefault().AreaLeave);
                else
                    model.AreaId = 0;
            }


            if(tjModel.UseRoleType == 1) //省市区县代理商模式
            {
                model.Agent = tjModel.Agent;
            }
            else if(tjModel.UseRoleType == 2) //一二三级代理商模式
            {
                if(tjModel.RoleCode == "1F" || tjModel.RoleCode == "3F" || tjModel.RoleCode == "2F" || tjModel.RoleCode == "Admin")
                    model.Agent = tjModel.ID;//归属上级代理商
                else //如果是普通会员推荐的代理商，要按推荐关系往上找，找到离自己最近的比自己级别高的那一个代理，归属在该高级别的代理名下
                {
                    //1--等级的树形结构
                    SqlParameter[] parameters = {
                                   new SqlParameter("@MID", SqlDbType.Int),
                                   new SqlParameter("@RoleCode", SqlDbType.VarChar)
                            };
                    parameters[0].Value = tjModel.ID;
                    parameters[1].Value = "";
                    DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperAgent");
                    {

                    }
                    model.Agent = TModel.ID;
                }
            }
            #endregion
        }
        /// <summary>
        /// 得到超级管理员
        /// </summary>
        /// <returns></returns>
        public static Model.Member GetCompanyAdminMember()
        {
            return CommonBase.GetList<Model.Member>("RoleCode='Manage'").FirstOrDefault();
        }
        /// <summary>
        /// 得到管理员，O单商
        /// </summary>
        /// <returns></returns>
        public static Model.Member GetAdminMember()
        {
            return CommonBase.GetList<Model.Member>("RoleCode='Admin'").FirstOrDefault();
        }

        /// <summary>
        ///判断一个会员是不是代理商，因为会员和代理商是两个不同的账号，判断是不是同一个人，就是看会员的直接推荐人是不是分销商，是的话就是同一个人
        /// </summary>
        /// <param name="checkMember"></param>
        /// <param name="agentMember"></param>
        /// <returns></returns>
        public static bool IsAgentMember(Model.Member checkMember, out Model.Member agentMember)
        {
            bool result = false;
            //查找到直接推荐人
            if(checkMember != null && checkMember.MTJ != null)
            {
                return IsAgentMember(checkMember.MTJ, out agentMember);
            }
            else
            {
                agentMember = null;
            }
            return result;
        }
        public static bool IsAgentMember(string mtj, out Model.Member agentMember)
        {
            bool result = false;
            //查找到直接推荐人
            if(mtj != null && !string.IsNullOrEmpty(mtj))
            {
                Model.Member tjModel = CommonBase.GetModel<Model.Member>(mtj);
                //判断直接推荐人是不是代理商
                if(tjModel.Role.AreaLeave == "20" || tjModel.Role.AreaLeave == "30" || tjModel.Role.AreaLeave == "40")
                {
                    result = true;
                    agentMember = tjModel;
                }
                else
                    agentMember = null;
            }
            else
            {
                agentMember = null;
            }
            return result;
        }


        public static void SetTrainPrivage(Model.Member TModel, TD_PayLog payModel, List<CommonObject> listCommon)
        {
            string trainPrivage = TModel.Learns;
            if(string.IsNullOrEmpty(trainPrivage))
                trainPrivage = "";
            //获取到培训权限
            TD_ChargeList chargeList = CommonBase.GetModel<TD_ChargeList>(payModel.ProductCode);
            List<string> listString = new List<string>();
            foreach(string trainPr in trainPrivage.Split('|'))
            {
                if(!listString.Contains(trainPr))
                {
                    listString.Add(trainPr);
                }
            }

            foreach(string newpr in chargeList.ChargeList.Split('|'))
            {
                if(!string.IsNullOrEmpty(newpr) && !listString.Contains(newpr))
                {
                    listString.Add(newpr);
                }
            }
            //组装成结果
            string resultPrivage = string.Empty;
            foreach(string li in listString)
            {
                if(!string.IsNullOrEmpty(li))
                    resultPrivage += li + "|";
            }
            TModel.Learns = resultPrivage;
            CommonBase.Update<Model.Member>(TModel, new string[] { "Learns" }, listCommon);
        }
    }
}
