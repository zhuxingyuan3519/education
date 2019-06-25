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
    public class PayService
    {
        /// <summary>
        /// 在线支付成功
        /// </summary>
        /// <param name="trade_no">订单号</param>
        /// <param name="out_trade_no">交易号</param>
        /// <param name="TModel">会员Model</param>
        /// <param name="total_fee">交易金额</param>
        /// <param name="notify_time">交易时间</param>
        /// <param name="body">返回body内容</param>
        /// <param name="payType">支付方式：1-支付宝。2-微信支付</param>
        /// <param name="buyer_logon_id">付款账号</param>
        /// <param name="payForUse">付款用途：recharge：充值。applyagent3f：申请分销商。applyagent2f：申请服务中心。applyvip：会员升级</param>
        /// <param name="listComm"></param>
        public static void PaySuccess(string trade_no, string out_trade_no, Member TModel, string total_fee, string notify_time, string body, int payType, string buyer_logon_id, string payForUse, List<CommonObject> listComm)
        {
            //判断该笔订单是否在商户网站中已经做过处理
            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //如果有做过处理，不执行商户的业务程序
            //Core.LogResult("支付成功" + Request.QueryString["trade_status"]);
            //插入付款纪录表TD_PayLog
            string payTypeString = payType == 1 ? "支付宝" : "微信支付";
            TD_PayLog payModel = new TD_PayLog();
            payModel.Code = trade_no;//支付宝交易号
            //查看是否存在此交易号
            if (CommonBase.GetModel<TD_PayLog>(trade_no) != null)
            {
                payModel.Code = trade_no + "-" + new Random().Next(0, 100).ToString(); ;
            }
            payModel.PayType = payType.ToString();
            payModel.PayWay = payTypeString;//支付宝
            payModel.ProductCode = out_trade_no;//支付宝交易号
            payModel.Company = 0;
            payModel.CreatedBy = TModel.MID;
            payModel.CreatedTime = DateTime.Now;
            payModel.IsDeleted = false;
            payModel.PayForMID = "0";
            //payModel.PayPic = Request.QueryString["buyer_logon_id"];//买家支付宝付款账号
            payModel.PayMID = TModel.MID; //会员ID
            payModel.PayMoney = MethodHelper.CommonHelper.GetDecimal(total_fee);
            payModel.PayTime = MethodHelper.CommonHelper.GetDateTime(notify_time);
            payModel.Status = 1;
            payModel.PayID = TModel.ID.ToString();
            payModel.Remark = TModel.MID + payTypeString + "在线成功支付" + payModel.PayMoney + "元成为缴费会员";
            //if (body.IndexOf("在线充值") > 0)//在线充值
            if (payForUse == "recharge")
            {
                payModel.Remark = TModel.MID + payTypeString + "在线充值" + payModel.PayMoney + "元";
                //给会员充钱
                TModel.MSH += payModel.PayMoney;
                MemberService.UpdateMoney(listComm, TModel.ID.ToString(), payModel.PayMoney.ToString(), "MSH", true);
            }
            //else if (body.IndexOf("申请分销商") > 0)  //申请分销商
            else if (payForUse == "applyagent3f" || payForUse == "applyagent2f")
            {
                #region 会员没有申请过分销商，需要创建三级分销商
                bool isCreatNewAgent = true;
                Model.Member upAgentMember = null;
                payModel.Remark = TModel.MID + payTypeString + "在线缴费" + payModel.PayMoney + "元申请成为分销商";
                if (payForUse == "applyagent2f")
                {
                    payModel.Remark = TModel.MID + payTypeString + "在线缴费" + payModel.PayMoney + "元申请成为服务中心";
                    List<Model.Member> upListMember = CommonBase.GetList<Model.Member>("MID LIKE '" + TModel.MID + "%'");
                    foreach (Model.Member mem in upListMember)
                    {
                        if (mem.RoleCode == "3F" || mem.RoleCode == "Zone")
                        {
                            isCreatNewAgent = false;
                            upAgentMember = mem;
                            break;
                        }
                    }
                }
                Model.Member newAgent = null;
                if (isCreatNewAgent)
                {
                    //申请分销商成功之后的处理逻辑
                    //创建一个新的分销商，名字为***分销商，密码123456，推荐人为会员的推荐人，缴费端口为后台配置的获得的名额数量-1；该会员推荐的名下n层会员都归属到该分销商。触发发送短信。
                    //创建一个新的分销商
                    newAgent = new Member();
                }
                else
                {
                    newAgent = upAgentMember;
                }
                newAgent.MName = TModel.MName;
                newAgent.Tel = TModel.MID;
                newAgent.Province = TModel.Province;
                newAgent.City = TModel.City;
                newAgent.Zone = TModel.Zone;
                newAgent.Company = TModel.Company;
                newAgent.MID = TModel.MID + "分销商";

                if (payForUse == "applyagent2f")
                    newAgent.MID = TModel.MID + "服务中心";

                newAgent.NoFHPool = string.Empty;
                if (isCreatNewAgent)
                {
                    newAgent.Password = CommonHelper.DESEncrypt("123456");
                    newAgent.IsFH = "1";
                    newAgent.LeaveTradePoints = 0;
                    newAgent.MCreateDate = DateTime.Now;
                    newAgent.MJB = 0;
                    newAgent.MSH = 0;
                    newAgent.NoActiveMoney = 0;

                    newAgent.TradePoints = ConvertHelper.ToInt32(Service.GlobleConfigService.GetWebConfig("ApplyAgent3ToTradePoint").Value, 0);
                    if (payForUse == "applyagent2f")
                        newAgent.TradePoints = ConvertHelper.ToInt32(Service.GlobleConfigService.GetWebConfig("ApplyAgent2ToTradePoint").Value, 0);

                    if (newAgent.TradePoints > 0)
                        newAgent.TradePoints -= 1;
                    newAgent.UseBeginTime = DateTime.Now;
                }
                else
                {
                    if (payForUse == "applyagent2f")
                        newAgent.TradePoints = ConvertHelper.ToInt32(Service.GlobleConfigService.GetWebConfig("ApplyAgent2ToTradePoint").Value, 0);

                }

                newAgent.PointMoney = 0;
                newAgent.MTJ = TModel.MTJ;
                newAgent.UseRoleType = TModel.UseRoleType;

                if (TModel.UseRoleType == 1)
                    newAgent.RoleCode = "Zone";
                else
                    newAgent.RoleCode = "3F";
                if (payForUse == "applyagent2f")
                {
                    if (TModel.UseRoleType == 1)
                        newAgent.RoleCode = "City";
                    else
                        newAgent.RoleCode = "2F";
                }

                SqlParameter[] parameters = {
                                   new SqlParameter("@MID", SqlDbType.Int),
                                   new SqlParameter("@RoleCode", SqlDbType.VarChar)
                         };
                parameters[0].Value = TModel.ID;
                parameters[1].Value = "";
                //得到上级以上代理商列表
                DataTable dtUpperAddressTbl = CommonBase.GetProduceTable(parameters, "Proc_CountUpperMember");
                string firstAgent = MemberService.GetCompanyAdminMember().ID;
                List<string> agentRoleCodeList = new string[] { "2F", "1F", "Admin", "Manage", "Province", "City" }.ToList();
                if (payForUse == "applyagent2f")
                    agentRoleCodeList = new string[] { "1F", "Admin", "Manage", "Province" }.ToList();
                if (dtUpperAddressTbl != null && dtUpperAddressTbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUpperAddressTbl.Rows)
                    {
                        string roleCode = dr["RoleCode"].ToString();
                        if (agentRoleCodeList.Contains(roleCode))
                        {
                            firstAgent =dr["ID"].ToString();
                            break;
                        }
                    }
                }
                newAgent.Agent = firstAgent;
                newAgent.AreaId = ConvertHelper.ToInt32(CacheService.RoleList.Where(c => c.Code == newAgent.RoleCode).FirstOrDefault().AreaLeave, 40);
                //查看该会员账号是否已存在
                if (CommonBase.GetList<Model.Member>("MID='" + newAgent.MID + "'").FirstOrDefault() != null)
                {
                    newAgent.MID = TModel.MID + "分销商管理员";
                    if (payForUse == "applyagent2f")
                        newAgent.MID = TModel.MID + "服务中心管理员";
                }
                //新增插入数据
                if (CommonBase.Insert<Model.Member>(newAgent))
                {
                    if (payForUse == "applyagent2f")
                        payForUse = "ApplyAgentTo2F";

                    if (payForUse == "applyagent3f")
                        payForUse = "ApplyAgentTo3F";
                    //会员先成为VIP学员之后补交费用成为分销商的，补交费用的要不要参与三级分销
                    //也就是说只有从体验会员直接升级代理商的猜进行成为代理商的相关分红
                    if (TModel.RoleCode != "VIP")
                    {
                        SHMoneyService.AgentTJChangeMoney(TModel, payModel.PayMoney, payForUse, listComm);
                    }
                    ////推荐人分红
                    ////得到推荐人信息
                    //Model.Member mtjModel = CommonBase.GetModel<Model.Member>(TModel.MTJ);
                    //if (mtjModel != null)
                    //{
                    //    SHMoneyService.ApplyAgentForMTJChangeMoney(mtjModel, TModel, newAgent.AreaId, MethodHelper.CommonHelper.GetDecimal(total_fee), listComm);
                    //}
                    //发送短信
                    if (ConfigHelper.GetAppSettings("IsTest") != "1")//不是测试才发送短信
                    {
                        string sendMsgContent = Service.GlobleConfigService.GetWebConfig("ApplyAgent3ToSendSMS").Value;
                        if (payForUse == "applyagent2f")
                            sendMsgContent = Service.GlobleConfigService.GetWebConfig("ApplyAgent2ToSendSMS").Value;
                        sendMsgContent = sendMsgContent.Replace("{{AgentLoginName}}", newAgent.MID);
                        Service.TelephoneCodeService.SendSMS(newAgent.Tel, "system", sendMsgContent);
                    }

                    //再查出来这个分销商
                    newAgent = CommonBase.GetList<Model.Member>("MID='" + newAgent.MID + "'").FirstOrDefault();
                    if (newAgent != null)
                    {
                        //先删除这个人的所有权限、
                        string deleteSQL = "SELECT * FROM dbo.Sys_RolePower WHERE  MID=" + newAgent.ID.ToString();
                        listComm.Add(new CommonObject(deleteSQL, null));
                        //插入权限数据
                        List<Sys_RolePower> rolePowerList = CommonBase.GetList<Sys_RolePower>("IsDeleted=0 and RoleCode='" + newAgent.RoleCode + "' and MID=0");
                        foreach (Sys_RolePower power in rolePowerList)
                        {
                            Sys_RolePower newPower = new Sys_RolePower();
                            newPower.Id = CommonHelper.GetGuid;
                            newPower.IsDeleted = false;
                            newPower.MID = newAgent.ID;
                            newPower.PrivageId = power.PrivageId;
                            newPower.PrivageType = power.PrivageType;
                            newPower.RoleCode = power.RoleCode;
                            newPower.Status = power.Status;
                            CommonBase.Insert<Sys_RolePower>(newPower, listComm);
                        }
                    }
                    //查询出该会员推荐的名下的会员，名下会员的归属代理商为刚创建的分销商
                    //同时该会员的推荐人修改为该创建的代理商
                    if (isCreatNewAgent)
                    {
                        TModel.MTJ = newAgent.ID.ToString();
                        //如果原来是体验会员，那么申请分销商之后自动变为缴费会员，同时减少该分销商自己的1个名额
                        if (TModel.RoleCode == "Member")
                        {
                            TModel.RoleCode = "VIP";
                            //会员的使用时间边长
                            TModel.UseBeginTime = DateTime.Now;
                            TModel.UseEndTime = DateTime.Now.AddDays(MethodHelper.ConvertHelper.ToDouble(CacheService.GlobleConfig.Field3, 365));
                            CommonBase.Update<Model.Member>(TModel, new string[] { "MTJ", "RoleCode", "UseBeginTime", "UseEndTime" }, listComm);
                        }
                        else
                        {
                            CommonBase.Update<Model.Member>(TModel, new string[] { "MTJ", "RoleCode" }, listComm);
                        }
                    }
                    //扣除管理员的端口
                    Model.Member comAdmin = MemberService.GetAdminMember();
                    if (comAdmin != null)
                    {
                        comAdmin.TradePoints -= newAgent.TradePoints;
                        comAdmin.LeaveTradePoints -= newAgent.LeaveTradePoints;
                        CommonBase.Update<Model.Member>(comAdmin, new string[] { "TradePoints", "LeaveTradePoints" }, listComm);
                        //插入端口消费记录
                        LogService.Log(comAdmin, "10", TModel.MID + "前台升级为" + CacheService.RoleList.Where(c => c.Code == newAgent.RoleCode).FirstOrDefault().Name + "，分配" + newAgent.LeaveTradePoints + "体验端口和" + newAgent.TradePoints + "收费端口", listComm);
                    }

                    //更新名下会员的归属分销商为新建的分销商
                    if (isCreatNewAgent)
                    {
                        string updateSQL = "UPDATE dbo.Member SET Agent=" + newAgent.ID + " WHERE ID IN (SELECT Code FROM dbo.FUN_CountTDMember(" + TModel.ID + ",1,99999))";
                        listComm.Add(new CommonObject(updateSQL, null));
                    }
                }
                #endregion
            }
            else if (payForUse == "applyvip")  //会员升级
            {
                #region 会员升级
                //三级分销及代理商分红核心业务逻辑
                MemberService.ActiveMemberToVIP(TModel, payModel.PayMoney, listComm);

                //付款成功，给管理员和O单商发送消息
                string sendAdminRoleCode = "RoleCode='Manage'";
                List<Model.Member> receiveList = new List<Member>();
                if (string.IsNullOrEmpty( TModel.Company))
                {
                    sendAdminRoleCode += " or RoleCode='Admin'";
                }
                receiveList = CommonBase.GetList<Model.Member>(sendAdminRoleCode);
                string message = payModel.Remark;
                Service.MessageService.SendNewMessage(TModel, receiveList, message, listComm, "2");
                //洛胜卡管家加入红包模式
                if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00")
                {
                    //缴费之后发放公司红包
                    SHMoneyService.SendCompanyRedBag(TModel, listComm);
                }
                #endregion
            }
            //插入表中付款记录表
            CommonBase.Insert<TD_PayLog>(payModel, listComm);
            LogService.Log(TModel, "8", TModel.MID + payTypeString + "成功支付" + payModel.PayMoney + "元", listComm);
        }
        /// <summary>
        /// 在线支付失败
        /// </summary>
        /// <param name="trade_no">订单号</param>
        /// <param name="out_trade_no">交易号</param>
        /// <param name="TModel">会员Model</param>
        /// <param name="total_fee">交易金额</param>
        /// <param name="notify_time">交易时间</param>
        /// <param name="body">返回body内容</param>
        /// <param name="payType">支付方式：1-支付宝。2-微信支付</param>
        /// <param name="buyer_logon_id">付款账号</param>
        /// <param name="listComm"></param>
        public static void PayFail(string trade_no, string out_trade_no, Member TModel, string total_fee, string notify_time, string body, int payType, string buyer_logon_id, List<CommonObject> listComm)
        {
            string payTypeString = payType == 1 ? "支付宝" : "微信支付";
            TD_PayLog payModel = new TD_PayLog();
            payModel.Code = out_trade_no;
            payModel.PayType = payType.ToString();
            payModel.PayWay = payTypeString;//支付宝
            payModel.ProductCode = trade_no;//支付宝交易号
            payModel.Company = 0;
            payModel.CreatedBy = TModel.MID;
            payModel.CreatedTime = DateTime.Now;
            payModel.IsDeleted = false;
            payModel.PayForMID = "0";
            payModel.PayPic = buyer_logon_id;
            payModel.PayMID = TModel.MID; //会员ID
            payModel.PayMoney = MethodHelper.CommonHelper.GetDecimal(total_fee);
            payModel.PayTime = MethodHelper.CommonHelper.GetDateTime(notify_time);
            payModel.Remark = TModel.MID + payTypeString + "在线支付" + payModel.PayMoney + "元，但" + payTypeString + "返回失败。";
            payModel.Status = 2;
            payModel.PayID = TModel.ID.ToString();
            //插入表中
            CommonBase.Insert<TD_PayLog>(payModel, listComm);
            LogService.Log(TModel, "8", TModel.MID + payTypeString + "支付" + payModel.PayMoney + "失败");
        }

    }
}
