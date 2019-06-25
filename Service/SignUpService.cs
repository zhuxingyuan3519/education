using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Service
{
    public class SignUpService
    {
        /// <summary>
        /// 缴费排位
        /// </summary>
        /// <param name="payModel"></param>
        /// <param name="applyForRole"></param>
        /// <param name="member"></param>
        /// <param name="TModel"></param>
        /// <param name="payType"></param>
        /// <returns></returns>
        public static bool SignUp(Model.TD_PayLog payModel, Sys_Role applyForRole, Member member, Member TModel, string payType)
        {
            //如果这个会员已经在排位表中存在了，就不再进行排位 
            string isExistInRank = " SELECT COUNT(1) FROM dbo.M_Rank WHERE MCode='" + member.ID + "'";
            if (MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(isExistInRank), 0) > 0)
            {
                return true;
            }

            #region 3、设置公排点位
            List<CommonObject> listTemp = new List<CommonObject>();
            M_Rank rank = null;
            Service.SHMoneyServiceVRank.SetRankPoint(member, listTemp, ref rank);



            #endregion
            if (CommonBase.RunListCommit(listTemp))
            {
                List<CommonObject> listComm = new List<CommonObject>();
                #region 4 判断并设置升级及分红状态
                //设置完临时排位之后，先保存到数据库了，然后再从数据库查询
                string sql = "  SELECT * FROM dbo.FUN_CountUpperMemberWithRank('" + member.ID + "',0,9999)";
                DataTable dtUpperRankTbl = CommonBase.GetTable(sql);
                decimal money = MethodHelper.ConvertHelper.ToDecimal(applyForRole.Remark, 0);
                //上级分红包
                List<TD_FHLog> fhLogList = new List<TD_FHLog>();
                SHMoneyServiceVRank.UpFHChangeMoney(member, money, dtUpperRankTbl, listComm, out fhLogList);
                #endregion

                payModel.Status = 1;
                payModel.PayTime = DateTime.Now;
                CommonBase.Update<TD_PayLog>(payModel, new string[] { "Status", "PayTime" }, listComm);

                #region  6、该会员升级VIP
                //该会员升级VIP
                //member.RoleCode = applyForRole.Code;
                //重新设置角色，增加学员角色
                if (member.Role.RIndex < applyForRole.RIndex)
                //if (!member.RoleCode.Contains(applyForRole.Code))
                {
                    member.RoleCode = applyForRole.Code;
                }
                member.UseBeginTime = DateTime.Now;
                member.UseEndTime = DateTime.Now.AddYears(1);
                CommonBase.Update<Model.Member>(member, new string[] { "RoleCode", "UseBeginTime", "UseEndTime" }, listComm);
                #endregion

                #region 7、记录日志
                LogService.Log(TModel, "2", TModel.MID + "为学员" + member.MID + "报名", listComm);
                #endregion

                #region 8  服务中心名下“学员”团队推荐两人服务中心账户和对应城市合伙人账户可各赠送一个VIP会员名额，城市合伙人自己团队推荐可得到两个名额
                //查看上级会员
                string upperMemberSql = "SELECT * FROM dbo.FUN_CountUpperMember('" + member.ID + "',1,9999)";
                DataTable upperMemberTbl = CommonBase.GetTable(upperMemberSql);
                //找到服务中心
                string serverMemberCode = string.Empty, cityMemberCode = string.Empty;
                //查询到第一个服务中心
                foreach (DataRow row in upperMemberTbl.Rows)
                {
                    if (row["RoleCode"].ToString() == "2F")
                    {
                        serverMemberCode = row["Code"].ToString();
                        break;
                    }
                }
                //查询到第一个城市合伙人
                foreach (DataRow row in upperMemberTbl.Rows)
                {
                    if (row["RoleCode"].ToString() == "3F")
                    {
                        cityMemberCode = row["Code"].ToString();
                        break;
                    }
                }

                //string checkMember = string.Empty;
                //if()

                if (!string.IsNullOrEmpty(serverMemberCode))
                {
                    //查看名下学员数量
                    string tdStudentCount = "SELECT COUNT(1) FROM dbo.FUN_CountTDMember('" + serverMemberCode + "',0,9999) t1 LEFT JOIN dbo.M_Rank t2 ON t1.Code=t2.MCode WHERE t2.Code IS NOT NULL";
                    int tdCount = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(tdStudentCount), 0);
                    if (tdCount > 0 && tdCount % 2 == 0)//两个为一组
                    {
                        //服务中心赠送一个VIP名额
                        Model.Member serverMember = CommonBase.GetModel<Model.Member>(serverMemberCode);
                        if (serverMember != null)
                        {
                            #region 增加名额
                            serverMember.TradePoints += 1;
                            serverMember.LeaveTradePoints += 1;
                            CommonBase.Update<Model.Member>(serverMember, new string[] { "TradePoints", "LeaveTradePoints" }, listComm);

                            Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                            cost.Code = MethodHelper.CommonHelper.GetGuid;
                            cost.CompanyCode = "0";
                            cost.CostCount = 1;
                            cost.CreatedBy = "system";
                            cost.CreatedTime = DateTime.Now;
                            cost.IsDeleted = false;
                            cost.FromCompany = "18";
                            cost.ToCompany = serverMember.ID;
                            cost.MID = serverMember.MID;
                            cost.Status = 1;
                            cost.Remark = "名下学员推荐满足条件赠送VIP名额";
                            CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                            #endregion
                        }

                        if (!string.IsNullOrEmpty(cityMemberCode))
                        {
                            //服务中心赠送一个VIP名额
                            Model.Member cityMember = CommonBase.GetModel<Model.Member>(cityMemberCode);
                            if (cityMember != null)
                            {
                                #region 增加名额
                                cityMember.TradePoints += 1;
                                cityMember.LeaveTradePoints += 1;
                                CommonBase.Update<Model.Member>(cityMember, new string[] { "TradePoints", "LeaveTradePoints" }, listComm);

                                Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                                cost.Code = MethodHelper.CommonHelper.GetGuid;
                                cost.CompanyCode = "0";
                                cost.CostCount = 1;
                                cost.CreatedBy = "system";
                                cost.CreatedTime = DateTime.Now;
                                cost.IsDeleted = false;
                                cost.FromCompany = "18";
                                cost.ToCompany = cityMember.ID;
                                cost.MID = cityMember.MID;
                                cost.Status = 1;
                                cost.Remark = "名下学员推荐满足条件赠送VIP名额";
                                CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                                #endregion
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(cityMemberCode)) //如果城市合伙人直推的
                {
                    //查看名下学员数量
                    string tdStudentCount = "SELECT COUNT(1) FROM dbo.FUN_CountTDMember('" + cityMemberCode + "',0,9999) t1 LEFT JOIN dbo.M_Rank t2 ON t1.Code=t2.MCode WHERE t2.Code IS NOT NULL";
                    int tdCount = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(tdStudentCount), 0);
                    if (tdCount > 0 && tdCount % 2 == 0)//两个为一组
                    {
                        //服务中心赠送一个VIP名额
                        Model.Member cityMember = CommonBase.GetModel<Model.Member>(cityMemberCode);
                        if (cityMember != null)
                        {
                            #region 增加2名额
                            cityMember.TradePoints += 2;
                            cityMember.LeaveTradePoints += 2;
                            CommonBase.Update<Model.Member>(cityMember, new string[] { "TradePoints", "LeaveTradePoints" }, listComm);

                            Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                            cost.Code = MethodHelper.CommonHelper.GetGuid;
                            cost.CompanyCode = "0";
                            cost.CostCount = 2;
                            cost.CreatedBy = "system";
                            cost.CreatedTime = DateTime.Now;
                            cost.IsDeleted = false;
                            cost.FromCompany = "18";
                            cost.ToCompany = cityMember.ID;
                            cost.MID = cityMember.MID;
                            cost.Status = 1;
                            cost.Remark = "名下学员推荐满足条件赠送VIP名额";
                            CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                            #endregion
                        }
                    }
                }



                #endregion

                if (CommonBase.RunListCommit(listComm))
                {
                    listComm.Clear();
                    #region 发送系统消息和短信消息
                    //发送直接推荐成功消息
                    //查询推荐人总共推荐了几个学员
                    if (member.MTJ != "18")
                    {
                        Member tjMember = CommonBase.GetModel<Member>(member.MTJ);
                        if (tjMember != null)
                        {
                            string sqlCount = "select t1.* from dbo.FUN_CountTDMember('" + member.MTJ + "',0,9999) t1 INNER JOIN dbo.M_Rank t2 ON t1.Code=t2.MCode";
                            DataTable dtMTJTbl = CommonBase.GetTable(sqlCount);
                            if (dtMTJTbl.Rows.Count <= 3)
                            {
                                int totalTJCount = 3;
                                string msg = "";
                                int lessTjCount = totalTJCount - dtMTJTbl.Rows.Count;
                                if (lessTjCount == 0)
                                {
                                    msg = "恭喜您成功推荐" + totalTJCount + "名学员，您已获得代言奖励资格。";
                                    //推荐够三人送“单词会员”角色
                                    if (!tjMember.RoleCode.Contains("WordUser"))
                                    {
                                        tjMember.RoleCode = tjMember.RoleCode + ",WordUser";
                                    }
                                    //设置为可得到代言红包
                                    if (tjMember.IsFH != "1")
                                    {
                                        tjMember.IsFH = "1";
                                    }
                                    //开通速记训练系统-初级词语训练
                                    if (!tjMember.Learns.Contains("1"))
                                    {
                                        tjMember.Learns = tjMember.Learns + "|1";
                                    }
                                    CommonBase.Update<Model.Member>(tjMember, new string[] { "IsFH", "RoleCode", "Learns" }, listComm);

                                }
                                else
                                {
                                    msg = "恭喜您成功推荐" + totalTJCount + "名学员，您再推荐" + lessTjCount + "名学员将有机会获得代言奖励。";
                                }
                                #region 发送系统消息
                                DB_Message_Model model = new DB_Message_Model();
                                model.Code = MethodHelper.CommonHelper.GetGuid;
                                model.CreatedBy = "system";
                                model.CreatedTime = DateTime.Now;
                                model.IsDeleted = false;
                                model.MType = "1";
                                model.ReceiveCode = member.MTJ;
                                model.SendCode = "18";
                                model.Status = 1;
                                model.Message = msg;
                                model.SendName = "admin";
                                model.Field1 = "管理员";
                                model.ReceiveName = tjMember.MID;
                                model.Field2 = tjMember.MID;
                                model.Remark = "5";
                                CommonBase.Insert<DB_Message_Model>(model, listComm);
                                #endregion
                            }
                        }
                    }

                    //发送分红代言红包消息
                    foreach (TD_FHLog fhlog in fhLogList)
                    {
                        #region 发送系统消息

                        var contentType = "application/json; charset=utf-8";
                        string bodyJson = "{ \"user_id\": \"" + fhlog.MID + "\", \"phone\": \"" + fhlog.FHMCode + "\", \"prize_type\": 2, \"prize_money\": " + (int)(fhlog.FHMoney * 100) + ", \"at_time\": " + new Random().Next(100000, 999999) + " }";
                        //由于接口需要格林威治时间故将当前时间转为格林威治时间再将格林威治时间字符串转成日期类型传入头部
                        var nowDateStr = DateTime.Now.ToString("r");
                        var nowTimeGMT = DateTime.Parse(nowDateStr);
                        var utf8 = Encoding.UTF8;
                        var headerAuthorization = "Bearer Udhekishe7763gdheu77h8j";
                        var url = "http://jwy.u1200.com/api/v1/user/prize/rate";

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                        req.Method = "post";
                        req.Headers.Add("Authorization", headerAuthorization);
                        req.ContentType = contentType;
                        req.Date = nowTimeGMT;

                        byte[] bytes = utf8.GetBytes(bodyJson);
                        req.ContentLength = bytes.Length;
                        Stream reqstream = req.GetRequestStream();
                        reqstream.Write(bytes, 0, bytes.Length);

                        string result = string.Empty;
                        try
                        {
                            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), utf8))
                            {
                                result = sr.ReadToEnd();
                            }
                        }
                        catch (Exception ex)
                        {


                        }

                        DB_Message_Model model = new DB_Message_Model();
                        model.Code = MethodHelper.CommonHelper.GetGuid;
                        model.CreatedBy = "system";
                        model.CreatedTime = DateTime.Now;
                        model.IsDeleted = false;
                        model.MType = "1";
                        model.ReceiveCode = fhlog.MID;
                        model.SendCode = "18";
                        model.Status = 1;
                        model.Message = "恭喜您获得" + fhlog.FHMoney + "元代言红包，感谢您为记无忧代言。";
                        model.SendName = "admin";
                        model.Field1 = "管理员";
                        model.ReceiveName = fhlog.FHMCode;
                        model.Field2 = fhlog.FHMCode;
                        model.Remark = "4";
                        CommonBase.Insert<DB_Message_Model>(model, listComm);
                        #endregion

                        //发送短信
                        if (ConfigHelper.GetAppSettings("IsTest") != "1")
                        {
                            TelephoneCodeService.SendSMS(fhlog.FHMCode, "system", "【记无忧】" + model.Message);
                        }
                    }
                    if (!CommonBase.RunListCommit(listComm))
                    {
                        LogService.Log(member, "2", member.MID + "缴费成为学员成功,但是发送相关系统消息和短信消息失败", listComm);
                    }
                    #endregion

                    return true;
                }
                else
                {
                    //删除刚才新增加的排位信息
                    if (rank != null)
                    {
                        CommonBase.Delete<M_Rank>(rank);
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payModel">付款信息的model</param>
        /// <param name="applyForRole">申请到的级别</param>
        /// <param name="member">付款人（要申请的人）</param>
        /// <param name="TModel">操作人，是谁操作支付的，一般与要申请的人是一个人，为别人代缴费情况除外</param>
        /// <param name="payType"></param>
        /// <returns></returns>
        public static bool SignUpNoMRank(Model.TD_PayLog payModel, Sys_Role applyForRole, Member member, Member TModel, string payType)
        {
            List<CommonObject> listComm = new List<CommonObject>();
            payModel.Status = 1;
            payModel.PayTime = DateTime.Now;
            CommonBase.Update<TD_PayLog>(payModel, new string[] { "Status", "PayTime" }, listComm);
            List<TD_FHLog> fhLogList = new List<TD_FHLog>();
            //服务中心奖励
            //暂时写死100
            TD_SHMoney fuwuzhongxinMoney = CacheService.SHMoneyList.FirstOrDefault(c => c.IsDeleted == false && c.Code == "KCTJ" && c.RoleCode == "fuwuzhongxin");
            if (fuwuzhongxinMoney != null)
            {
                SHMoneyService.ServicePayChangeMoney(member, fuwuzhongxinMoney.TJFloat, listComm, out fhLogList);
            }

            #region  6、该会员升级VIP
            //该会员升级VIP
            //member.RoleCode = applyForRole.Code;
            //重新设置角色，增加学员角色
            if (member.Role.RIndex < applyForRole.RIndex)
            {
                member.RoleCode = applyForRole.Code;
            }
            member.UseBeginTime = DateTime.Now;
            member.UseEndTime = DateTime.Now.AddYears(10);
            CommonBase.Update<Model.Member>(member, new string[] { "RoleCode", "UseBeginTime", "UseEndTime" }, listComm);
            #endregion

            #region 7、记录日志
            LogService.Log(TModel, "2", TModel.MID + "为学员" + member.MID + "报名", listComm);
            #endregion

            #region 8  服务中心名下“学员”团队推荐两人服务中心账户和对应城市合伙人账户可各赠送一个VIP会员名额，城市合伙人自己团队推荐可得到两个名额
            //查看上级会员
            string upperMemberSql = "SELECT * FROM dbo.FUN_CountUpperMember('" + member.ID + "',1,9999)";
            DataTable upperMemberTbl = CommonBase.GetTable(upperMemberSql);
            //找到服务中心
            string serverMemberCode = string.Empty, cityMemberCode = string.Empty;
            //查询到第一个服务中心
            foreach (DataRow row in upperMemberTbl.Rows)
            {
                if (row["RoleCode"].ToString() == "2F")
                {
                    serverMemberCode = row["Code"].ToString();
                    break;
                }
            }
            //查询到第一个城市合伙人
            foreach (DataRow row in upperMemberTbl.Rows)
            {
                if (row["RoleCode"].ToString() == "3F")
                {
                    cityMemberCode = row["Code"].ToString();
                    break;
                }
            }


            if (!string.IsNullOrEmpty(serverMemberCode))
            {
                //查看名下学员数量
                string tdStudentCount = "SELECT COUNT(1) FROM dbo.FUN_CountTDMember('" + serverMemberCode + "',0,9999) t1 left join SYS_role t2 on t2.Code=t1.RoleCode where t2.RIndex>=2 ";
                int tdCount = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(tdStudentCount), 0);
                if (tdCount > 0 && tdCount % 2 == 0)//两个为一组
                {
                    //服务中心赠送一个VIP名额
                    Model.Member serverMember = CommonBase.GetModel<Model.Member>(serverMemberCode);
                    if (serverMember != null)
                    {
                        #region 增加名额
                        serverMember.TradePoints += 1;
                        serverMember.LeaveTradePoints += 1;
                        CommonBase.Update<Model.Member>(serverMember, new string[] { "TradePoints", "LeaveTradePoints" }, listComm);

                        Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                        cost.Code = MethodHelper.CommonHelper.GetGuid;
                        cost.CompanyCode = "0";
                        cost.CostCount = 1;
                        cost.CreatedBy = "system";
                        cost.CreatedTime = DateTime.Now;
                        cost.IsDeleted = false;
                        cost.FromCompany = "18";
                        cost.ToCompany = serverMember.ID;
                        cost.MID = serverMember.MID;
                        cost.Status = 1;
                        cost.Remark = "名下学员推荐满足条件赠送VIP名额";
                        CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                        #endregion
                    }

                    if (!string.IsNullOrEmpty(cityMemberCode))
                    {
                        //服务中心赠送一个VIP名额
                        Model.Member cityMember = CommonBase.GetModel<Model.Member>(cityMemberCode);
                        if (cityMember != null)
                        {
                            #region 增加名额
                            cityMember.TradePoints += 1;
                            cityMember.LeaveTradePoints += 1;
                            CommonBase.Update<Model.Member>(cityMember, new string[] { "TradePoints", "LeaveTradePoints" }, listComm);

                            Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                            cost.Code = MethodHelper.CommonHelper.GetGuid;
                            cost.CompanyCode = "0";
                            cost.CostCount = 1;
                            cost.CreatedBy = "system";
                            cost.CreatedTime = DateTime.Now;
                            cost.IsDeleted = false;
                            cost.FromCompany = "18";
                            cost.ToCompany = cityMember.ID;
                            cost.MID = cityMember.MID;
                            cost.Status = 1;
                            cost.Remark = "名下学员推荐满足条件赠送VIP名额";
                            CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                            #endregion
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(cityMemberCode)) //如果城市合伙人直推的
            {
                //查看名下学员数量
                string tdStudentCount = "SELECT COUNT(1) FROM dbo.FUN_CountTDMember('" + cityMemberCode + "',0,9999) t1 left join SYS_role t2 on t2.Code=t1.RoleCode where t2.RIndex>=2 ";
                int tdCount = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(tdStudentCount), 0);
                if (tdCount > 0 && tdCount % 2 == 0)//两个为一组
                {
                    //服务中心赠送一个VIP名额
                    Model.Member cityMember = CommonBase.GetModel<Model.Member>(cityMemberCode);
                    if (cityMember != null)
                    {
                        #region 增加2名额
                        cityMember.TradePoints += 2;
                        cityMember.LeaveTradePoints += 2;
                        CommonBase.Update<Model.Member>(cityMember, new string[] { "TradePoints", "LeaveTradePoints" }, listComm);

                        Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                        cost.Code = MethodHelper.CommonHelper.GetGuid;
                        cost.CompanyCode = "0";
                        cost.CostCount = 2;
                        cost.CreatedBy = "system";
                        cost.CreatedTime = DateTime.Now;
                        cost.IsDeleted = false;
                        cost.FromCompany = "18";
                        cost.ToCompany = cityMember.ID;
                        cost.MID = cityMember.MID;
                        cost.Status = 1;
                        cost.Remark = "名下学员推荐满足条件赠送VIP名额";
                        CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                        #endregion
                    }
                }
            }
            #endregion


            //看看需要多少是进入红包奖金池的
            decimal toPrizeMoney = applyForRole.ToPrizeMoney;
            if (toPrizeMoney > 0)
            {
                SH_PrizePoolInLog inlog = new SH_PrizePoolInLog();
                inlog.Code = MethodHelper.CommonHelper.GetGuid;
                inlog.InMoney = toPrizeMoney;
                inlog.LogDate = DateTime.Now;
                inlog.PoolId = "1";
                inlog.UserCode = member.MID;
                inlog.UserId = member.ID;
                CommonBase.Insert<SH_PrizePoolInLog>(inlog, listComm);
            }

            if (CommonBase.RunListCommit(listComm))
            {
                listComm.Clear();
                //return true;
                #region 发送系统消息和短信消息
                //发送直接推荐成功消息
                //查询推荐人总共推荐了几个学员
                if (member.MTJ != "18")
                {
                    Member tjMember = CommonBase.GetModel<Member>(member.MTJ);
                    if (tjMember != null)
                    {
                        string sqlCount = "select t1.* from dbo.FUN_CountTDMember('" + member.MTJ + "',0,9999) t1 left join SYS_role t2 on t2.Code=t1.RoleCode where t2.RIndex>=2";
                        DataTable dtMTJTbl = CommonBase.GetTable(sqlCount);
                        if (dtMTJTbl.Rows.Count <= 3)
                        {
                            int totalTJCount = 3;
                            string msg = "";
                            int lessTjCount = totalTJCount - dtMTJTbl.Rows.Count;
                            if (lessTjCount == 0)
                            {
                                msg = "恭喜您成功推荐" + totalTJCount + "名学员，您已获得代言奖励资格。";
                                //推荐够三人送“单词会员”角色
                                //if(!tjMember.RoleCode.Contains("WordUser"))
                                //{
                                //    tjMember.RoleCode = tjMember.RoleCode + ",WordUser";
                                //}
                                //设置为可得到代言红包
                                if (tjMember.IsFH != "1")
                                {
                                    tjMember.IsFH = "1";
                                }
                                //开通速记训练系统-初级词语训练
                                if (!tjMember.Learns.Contains("1"))
                                {
                                    tjMember.Learns = tjMember.Learns + "|1";
                                }
                                CommonBase.Update<Model.Member>(tjMember, new string[] { "IsFH", "Learns" }, listComm);

                            }
                            else
                            {
                                msg = "恭喜您成功推荐" + totalTJCount + "名学员，您再推荐" + lessTjCount + "名学员将有机会获得代言奖励。";
                            }
                            #region 发送系统消息
                            DB_Message_Model model = new DB_Message_Model();
                            model.Code = MethodHelper.CommonHelper.GetGuid;
                            model.CreatedBy = "system";
                            model.CreatedTime = DateTime.Now;
                            model.IsDeleted = false;
                            model.MType = "1";
                            model.ReceiveCode = member.MTJ;
                            model.SendCode = "18";
                            model.Status = 1;
                            model.Message = msg;
                            model.SendName = "admin";
                            model.Field1 = "管理员";
                            model.ReceiveName = tjMember.MID;
                            model.Field2 = tjMember.MID;
                            model.Remark = "5";
                            CommonBase.Insert<DB_Message_Model>(model, listComm);
                            #endregion
                        }
                    }
                }

                //发送分红代言红包消息
                //List<TD_FHLog> fhLogList = new List<TD_FHLog>();
                foreach (TD_FHLog fhlog in fhLogList)
                {
                    #region 发送系统消息
                    var url = "http://jwy.u1200.com/api/v1/user/prize/rate";
                    string bodyJson = "{ \"user_id\": \"" + fhlog.MID + "\", \"phone\": \"" + fhlog.FHMCode + "\", \"prize_type\": 2, \"prize_money\": " + (int)(fhlog.FHMoney * 100) + ", \"at_time\": " + new Random().Next(100000, 999999) + " }";
                    WebRemoteRequest.RequestWebApi(url, bodyJson);

                    DB_Message_Model model = new DB_Message_Model();
                    model.Code = MethodHelper.CommonHelper.GetGuid;
                    model.CreatedBy = "system";
                    model.CreatedTime = DateTime.Now;
                    model.IsDeleted = false;
                    model.MType = "1";
                    model.ReceiveCode = fhlog.MID;
                    model.SendCode = "18";
                    model.Status = 1;
                    model.Message = "恭喜您获得" + fhlog.FHMoney + "元代言红包，感谢您为记无忧代言。";
                    model.SendName = "admin";
                    model.Field1 = "管理员";
                    model.ReceiveName = fhlog.FHMCode;
                    model.Field2 = fhlog.FHMCode;
                    model.Remark = "4";
                    CommonBase.Insert<DB_Message_Model>(model, listComm);
                    #endregion

                    //发送短信
                    if (ConfigHelper.GetAppSettings("IsTest") != "1")
                    {
                        TelephoneCodeService.SendSMS(fhlog.FHMCode, "system", "【记无忧】" + model.Message);
                    }
                }
                if (!CommonBase.RunListCommit(listComm))
                {
                    LogService.Log(member, "2", member.MID + "缴费成为学员成功,但是发送相关系统消息和短信消息失败", listComm);
                }
                #endregion

                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

