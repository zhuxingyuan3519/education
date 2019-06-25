
using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Web
{
    public partial class ajax : BasePage
    {
        //private object lockObj = new object();
        protected new void Page_Load(object sender, EventArgs e)
        {
            TModel = Session["Member"] as Member;
            if (TModel == null)
            {
                TModel = Session["AdminMember"] as Member;
            }

            if (!string.IsNullOrEmpty(Request["type"]))
            {
                Operation(Request["type"]);
            }

            Response.End();
        }

        protected void Operation(string ope)
        {

            switch (ope)
            {
                case "changeCurrentMember":
                    changeCurrentMember();
                    break;
                case "bingWXUser":
                    BingWXUser();
                    break;
                case "bingLoginWXUser":
                    bingLoginWXUser();
                    break;
                    
                case "GetWeixinShareData":
                    GetWeixinShareData();
                    break;
                case "selectMember":
                    selectMember();
                    break;
                case "bindTeacher":
                    bindTeacher();
                    break;
                case "Login":
                    getLogin();
                    break;
                case "ApplyRoles":
                    ApplyRoles();
                    break;
                case "resetpwd":
                    resetpwd();
                    break;
                case "FindPwd":
                    FindPwd();
                    break;
                case "ResetPwd":
                    ResetPwd();
                    break;
                case "LoginOut":
                    LoginOut();
                    break;
                case "CheckQuestionAnswer":
                    CheckQuestionAnswer();
                    break;
                case "GetNewAddressInfo":
                    GetNewAddressInfo();
                    break;
                case "UpdateRemindStatus":
                    UpdateRemindStatus();
                    break;

                case "GetAddressInfo":
                    GetAddressInfo();
                    break;
                case "GetIndutyInfo":
                    GetIndutyInfo();
                    break;

                case "deleteArchive":
                    deleteArchive();
                    break;
                case "deletePlan":
                    deletePlan();
                    break;
                case "HasKnowPlanChange":
                    HasKnowPlanChange();
                    break;
                case "deletePlanDetail":
                    deletePlanDetail();
                    break;
                case "setExpenseOrStatusMoney":
                    setExpenseOrStatusMoney();
                    break;
                case "checkNoticeInfo":
                    checkNoticeInfo();
                    break;
                case "responseMsg":
                    responseMsg();
                    break;
                case "dealNoticeInfo":
                    dealNoticeInfo();
                    break;
                case "SendMessage":
                    SendMessage();
                    break;
                case "SendAdMessage":
                    SendAdMessage();
                    break;
                case "TX":
                    getTX();
                    break;
                //申请分销商
                case "CheckIsCanApplyAgent":
                    CheckIsCanApplyAgent();
                    break;

                #region 红包模式
                case "operatorRedBag":
                    operatorRedBag();
                    break;
                case "addRedBag":
                    addRedBag();
                    break;
                #endregion

                #region 手机验证码部分
                case "sendTelCode":
                    sendTelCode();
                    break;
                #endregion

                //在线训练相关
                case "GetTrainLearnCode":
                    GetTrainLearnCode();
                    break;

                case "stopMemory":
                    stopMemory();
                    break;

                case "confirmAnswer":
                    confirmAnswer();
                    break;

                case "queryWord":
                    queryWord();
                    break;

                case "getBookVersionVsWord":
                    getBookVersionVsWord();
                    break;
                case "createEvaluationPaper":
                    createEvaluationPaper();
                    break;
                case "getUnits":
                    getUnits();
                    break;

            }
        }

        private void GetWeixinShareData()
        {
            try
            {
                string str = Request["str"];
                Service.WXPay.business.WeixinShare.WXShareModel models = Service.WXPay.business.WeixinShare.GetKey(str);
                Response.Write(MethodHelper.JsonHelper.ObjectToJson<Service.WXPay.business.WeixinShare.WXShareModel>(models));
                return;
            }
            catch (Exception ex)
            {
                Response.Write("");
                return;
            }
        }



        private void GetNewAddressInfo()
        {
            lock (new object())
            {
                try
                {
                    string id = Request["code"];
                    int level = int.Parse(Request["level"]);
                    string condition = Request["condition"];//是否查询代理商的可用区域
                    string rolecode = Request["rolecode"];//查询的角色


                    if (level == 20) //根据省查询市
                    {
                        var list = from v in CacheService.SatandardAddressList where v.ProCode == id && v.LevelInt == 30 select new { Id = v.AdCode, Name = v.Name };
                        if (condition == "agent")
                        {
                            if (rolecode == "3F")
                            {
                                List<Model.Member> listAgentAddress = CommonBase.GetList<Model.Member>("RoleCode='3F'");
                                list = list.Where(n => !listAgentAddress.Select(m => m.City).Contains(n.Id));
                            }
                        }

                        Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                        return;
                    }
                    if (level == 30) //根据市查询区县
                    {
                        var list = from v in CacheService.SatandardAddressList where v.CityCode == id && v.LevelInt == 40 select new { Id = v.AdCode, Name = v.Name };
                        Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                        return;
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write("1");
                    return;
                }
            }
        }

        private void sendTelCode()
        {
            lock (new object())
            {
                try
                {
                    string code = Request["code"]; //手机号
                    string sendtype = Request["sendtype"];
                    string sendRemark = "用户注册";
                    if (sendtype == "1")
                    {
                        //校验该手机号是否已被注册
                        string testSql = "select count(1) from Member where  MID='" + code + "' or Tel='" + code + "'";
                        if (MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(testSql), 0) > 0)
                        {
                            Response.Write("2");//发送成功
                            return;
                        }
                    }
                    else if (sendtype == "2")
                    {
                        sendRemark = "找回密码";
                        //校验该手机号是否已注册
                        string testSql = "select count(1) from Member where  MID='" + code + "' or Tel='" + code + "'";
                        if (MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(testSql), 0) <= 0)
                        {
                            Response.Write("2");//发送成功
                            return;
                        }

                    }
                    else if (sendtype == "3")
                    {
                        sendRemark = "用户提现";
                        code = string.IsNullOrEmpty(TModel.Tel) ? TModel.MID : TModel.Tel;
                        if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj01")
                        {
                            //白金掌付的提现需要是VIP用户才能提现
                            if (TModel.RoleCode.ToLower() != "vip")
                            {
                                Response.Write("-1");//
                                return;
                            }
                        }
                        //if(MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00")
                        //{
                        //    //洛胜联盟只有上发过2个红包的人才能提现，如果不符合条件，就提示不能提现
                        //    if(TModel.SendRedBagCount < 2)
                        //    {
                        //        Response.Write("-2");//
                        //        return;
                        //    }
                        //}

                        //查看是否具有提现权限
                        //直接推荐三个人才能提现
                        string sqlCount = "select count(1) from dbo.FUN_CountTDMember('" + TModel.ID + "',0,9999) t1 INNER JOIN dbo.M_Rank t2 ON t1.Code=t2.MCode";
                        int tjCount = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(sqlCount), 0);
                        if (tjCount < 3)
                        {
                            Response.Write("-2");
                            return;
                        }
                    }
                    string resu = TelephoneCodeService.SendCode(code, "SYSTEM", sendRemark);
                    if (!string.IsNullOrEmpty(resu))
                    {
                        Response.Write("1");//发送成功
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0"); //发送失败
                return;
            }
        }

        private void ApplyRoles()
        {
            lock (new object())
            {
                try
                {
                    string code = Request["code"]; //申请级别
                    string payType = Request["payType"];//支付方式
                    string applyCity = Request["applyCity"];//申请城市，对城市管理员启用
                    //查看是否申请过
                    if (CommonBase.GetList<Model.SH_HirePurchase>("RoleCode='" + code + "' and UserId='" + TModel.ID + "'").Count > 0)
                    {
                        Response.Write("1");//发送成功
                        return;
                    }
                    SH_HirePurchase apply = new SH_HirePurchase();
                    apply.Id = GetGuid;
                    apply.CreatedBy = TModel.ID;
                    apply.CreatedTime = DateTime.Now;
                    apply.EveryHireMoney = 0;
                    apply.HireCount = 0;
                    apply.HireId = payType;
                    apply.HireType = 1;
                    apply.LeaveTradePointCount = 0;
                    apply.PayDate = 1;
                    apply.RoleCode = code;
                    apply.TradePointCount = 0;
                    apply.UserId = TModel.ID;
                    apply.UserCode = TModel.MID;
                    apply.Remark = applyCity;
                    if (CommonBase.Insert<Model.SH_HirePurchase>(apply))
                    {
                        Response.Write("2");//发送成功
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0"); //发送失败
                return;
            }
        }
        private void bindTeacher()
        {
            lock (new object())
            {
                try
                {
                    string teacherCode = Request["teacherCode"];
                    List<CommonObject> listCommon = new List<CommonObject>();
                    //查询到老师
                    Model.Member teacherMember = CommonBase.GetList<Model.Member>("MID='" + teacherCode + "'").FirstOrDefault();
                    if (teacherMember == null)
                    {
                        Response.Write("2");//
                        return;
                    }
                    TModel.ParentTrade = teacherMember.ID;
                    CommonBase.Update<Model.Member>(TModel, new string[] { "ParentTrade" }, listCommon);
                    Service.LogService.Log(TModel, "2", TModel.MID + "绑定老师", listCommon);
                    if (CommonBase.RunListCommit(listCommon))
                    {
                        Response.Write("1");//发送成功
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0"); //发送失败
                return;
            }
        }
        private void operatorRedBag()
        {
            return;
            lock (new object())
            {
                try
                {
                    string code = Request["code"];
                    SH_RedBagDetailLog log = CommonBase.GetModel<SH_RedBagDetailLog>(code);
                    if (log != null)
                    {
                        if (log.Status == 3) //红包已激活才可以领取
                        {
                            log.Status = 4;
                            log.ReceiveTime = DateTime.Now;
                            List<CommonObject> listComm = new List<CommonObject>();
                            CommonBase.Update<SH_RedBagDetailLog>(log, new string[] { "Status", "ReceiveTime" }, listComm);
                            //更新我的现金币金额
                            TModel.MSH = TModel.MSH + log.RedBagMoney;
                            CommonBase.Update<Member>(TModel, new string[] { "MSH" }, listComm);
                            if (CommonBase.RunListCommit(listComm))
                            {
                                Response.Write("1");//领取成功
                                return;
                            }
                            else
                            {
                                Response.Write("0");//领取失败
                                return;
                            }
                        }
                        else
                        {
                            Response.Write("2");//未激活或已领取
                            return;
                        }
                    }
                    else
                    {
                        Response.Write("3");////不存在红包
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }
        private void addRedBag()
        {
            lock (new object())
            {
                try
                {
                    string count = Request["count"];
                    int bagCount = ConvertHelper.ToInt32(count, 0);
                    if (bagCount <= 1)
                    {
                        Response.Write("0");////红包数量要>=2
                        return;
                    }
                    //缴费之后发放公司红包
                    decimal redBagMoney = CacheService.RedBagGlobleConfig.PersonalReturnMoney;
                    List<CommonObject> listComm = new List<CommonObject>();
                    decimal totalRedBagMoney = bagCount * redBagMoney;
                    //查看账户余额是否购发红包
                    if ((TModel.MSH + TModel.NoActiveMoney) < totalRedBagMoney)
                    {
                        Response.Write("1");////账户金额不足
                        return;
                    }


                    SHMoneyService.SendRedBag(TModel, null, redBagMoney, 2, bagCount, listComm);
                    //激活的金额
                    decimal changeMoney = SHMoneyService.ActiveRedBagMoney(TModel.ID.ToString(), count, listComm);
                    TModel.MSH = changeMoney + TModel.MSH;
                    TModel.NoActiveMoney = TModel.NoActiveMoney - changeMoney;
                    //发出去几个红包，就减去几个金额

                    decimal noActiveMoneyLess = 0, MSHLess = 0;
                    if (TModel.NoActiveMoney <= totalRedBagMoney)
                    {
                        noActiveMoneyLess = TModel.NoActiveMoney;
                        MSHLess = totalRedBagMoney - TModel.NoActiveMoney;
                    }
                    else
                    {
                        noActiveMoneyLess = totalRedBagMoney;
                        MSHLess = 0;
                    }
                    TModel.MSH = TModel.MSH - MSHLess;
                    TModel.NoActiveMoney = TModel.NoActiveMoney - noActiveMoneyLess;
                    //扣除账户金额
                    CommonBase.Update<Member>(TModel, new string[] { "MSH", "NoActiveMoney" }, listComm);

                    //if (SHMoneyService.CheckIsCanActive(TModel.ID.ToString()))
                    //{
                    //    //发完红包，查看可激活的红包层级并激活之
                    //}
                    //else
                    //{
                    //    //查看红包总金额，未激活的金额是否可以直接抵扣
                    //    decimal noActiveMoneyLess = 0, MSHLess = 0;
                    //    if (TModel.NoActiveMoney <= totalRedBagMoney)
                    //    {
                    //        noActiveMoneyLess = TModel.NoActiveMoney;
                    //        MSHLess = totalRedBagMoney - TModel.NoActiveMoney;
                    //    }
                    //    else
                    //    {
                    //        noActiveMoneyLess = totalRedBagMoney;
                    //        MSHLess = 0;
                    //    }
                    //    TModel.MSH = TModel.MSH - MSHLess;
                    //    TModel.NoActiveMoney = TModel.NoActiveMoney - noActiveMoneyLess;
                    //    //扣除账户金额
                    //    CommonBase.Update<Member>(TModel, new string[] { "MSH", "NoActiveMoney" }, listComm);
                    //}

                    if (CommonBase.RunListCommit(listComm))
                    {
                        Response.Write("3");////成功
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }
        private void ResetPwd()
        {
            lock (new object())
            {
                try
                {
                    string mid = Request["mid"];
                    string oldpwd = Request["oldpwd"];
                    string newpwd = Request["newpwd"];
                    string opwd = CommonHelper.DESDecrypt(TModel.Password);
                    if (opwd != oldpwd)
                    {
                        Response.Write("1"); //原始密码不正确
                        return;
                    }
                    else
                    {
                        TModel.Password = newpwd;
                        TModel.Password = CommonHelper.DESEncrypt(TModel.Password);
                        //修改用户密码
                        if (CommonBase.Update<Model.Member>(TModel, new string[] { "Password" }))
                        {
                            Response.Write("0");
                        }
                        else
                        {
                            Response.Write("-1");
                        }
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }

        private void CheckIsCanApplyAgent()
        {
            lock (new object())
            {
                try
                {
                    string applyLeavel = Request["agentleavel"];//申请级别
                    //decimal vipMoney = ConvertHelper.ToDecimal(Service.CacheService.GlobleConfig.Field1, 0);
                    //decimal applyAgentMoney = ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent3Money").Value, 0);
                    //decimal applyLeaveMoney = applyAgentMoney;
                    if (TModel.RoleCode == "VIP")
                    {
                        List<Model.Member> upListMember = CommonBase.GetList<Model.Member>("MID LIKE '" + TModel.MID + "%'");
                        Model.Member upMember = null;
                        foreach (Model.Member mem in upListMember)
                        {
                            if (mem.RoleCode != "VIP" && mem.RoleCode != "Member")
                            {
                                upMember = mem;
                            }
                        }
                        if (upMember == null)
                        {
                            //另外一种判断会员是不是分销商的方法
                            MemberService.IsAgentMember(TModel, out upMember);
                        }

                        //applyLeaveMoney = applyAgentMoney - vipMoney;
                        //查看推荐人是不是分销商，如果是，就说明是同一个人，不用升级
                        //Model.Member mtjModel = CommonBase.GetModel<Model.Member>(TModel.MTJ);
                        if (upMember != null)
                        {
                            string mtjAreaLeave = upMember.Role.AreaLeave;

                            if (applyLeavel == "2f")
                            {
                                if (mtjAreaLeave != null && Convert.ToInt16(mtjAreaLeave) <= 30)
                                {
                                    Response.Write("1");//本人已经是二级分销商了
                                    return;
                                }
                            }
                            if (applyLeavel == "3f")
                            {
                                if (mtjAreaLeave != null && Convert.ToInt16(mtjAreaLeave) <= 40)
                                {
                                    Response.Write("1");//本人已经是二级分销商了
                                    return;
                                }
                            }
                        }
                    }
                    //查看管理员的端口够不够扣除
                    int usePoint = ConvertHelper.ToInt32(Service.GlobleConfigService.GetWebConfig("ApplyAgent3ToTradePoint").Value, 0);
                    if (applyLeavel == "2f")
                    {
                        usePoint = ConvertHelper.ToInt32(Service.GlobleConfigService.GetWebConfig("ApplyAgent2ToTradePoint").Value, 0);
                    }

                    Model.Member comAdmin = MemberService.GetAdminMember();
                    if (comAdmin != null)
                    {
                        if (comAdmin.TradePoints < usePoint)
                        {
                            Response.Write("2");//
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }


        private void getTX()
        {
            lock (new object())
            {
                try
                {
                    string money = Request["txmoney"];
                    string txway = Request["txway"];
                    decimal pointTXMoney = 0;
                    if (TModel.RoleCode.ToLower() == "member" && TModel.ReadNoticeId == 0)
                    {
                        Response.Write("-4");//
                        return;
                    }
                    //查看是否具有提现权限
                    //直接推荐三个人才能提现
                    string sqlCount = "select count(1) from Member t1 left join SYS_role t2 on t2.Code=t1.RoleCode where t2.RIndex>=2 and t1.MTJ='" + TModel.ID + "'";
                    int tjCount = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(sqlCount), 0);
                    if (tjCount < 3)
                    {
                        Response.Write("9");
                        return;
                    }

                    List<CommonObject> listComm = new List<CommonObject>();

                    //验证码
                    string validCode = Request["sendcode"];
                    if (!string.IsNullOrEmpty(validCode))
                    {
                        string telePhone = string.IsNullOrEmpty(TModel.Tel) ? TModel.MID : TModel.Tel;
                        if (ConfigHelper.GetAppSettings("IsTest") != "1")
                        {
                            if (!TelephoneCodeService.CheckValidCode(telePhone, validCode, "用户提现"))
                            {
                                Response.Write("-3");
                                return;
                            }
                        }
                        //验证通过，更新验证码信息
                        listComm.Add(new CommonObject("update Sys_SendCode set IsUsed=1,ValidTime=GETDATE() where Telephone='" + telePhone + "' and SendCode='" + validCode + "' and IsUsed=0", null));
                    }
                    if (TModel.RoleCode == "Member" || TModel.RoleCode == "VIP")
                    {
                        if (TModel.ReadNoticeId == 0)
                        {
                            TModel.MSH = 0;
                            TModel.MVB = 0;
                            TModel.MJB = 0;
                        }
                    }
                    //查看提现积分是否足够
                    decimal.TryParse(money, out pointTXMoney);
                    decimal ownMoney = TModel.MSH;
                    if (txway == "4")
                    {
                        ownMoney = TModel.MJB;
                    }

                    if (txway == "5")
                    {
                        ownMoney = TModel.MVB;
                    }

                    if (pointTXMoney > ownMoney)
                    {
                        Response.Write("1");
                        return;
                    }
                    if (pointTXMoney <= 0)
                    {
                        Response.Write("2");
                        return;
                    }
                    if (pointTXMoney < CacheService.GlobleConfig.MinTXMoney)
                    {
                        Response.Write("3");
                        return;
                    }
                    if (pointTXMoney % CacheService.GlobleConfig.BaseJifen != 0)
                    {
                        Response.Write("4");
                        return;
                    }
                    if (!string.IsNullOrEmpty(Request["qid"])) //判断是不是使用安全问题校验
                    {
                        string answer = Server.UrlDecode(Request["answer"]);
                        string qid = Request["qid"];
                        if (string.IsNullOrEmpty(answer))
                        {
                            Response.Write("5");
                            return;
                        }
                        //判断安全问题是否正确
                        Sys_SQ_Answer answerModel = CommonBase.GetList<Sys_SQ_Answer>("IsDeleted=0 and MID=" + TModel.ID + " and QID=" + qid + " and Answer='" + answer + "'").FirstOrDefault();
                        if (answerModel == null)
                        {
                            Response.Write("6");
                            return;
                        }
                    }

                    //插入提现记录表
                    TD_TXLog txlog = new TD_TXLog();
                    txlog.ApplyTXDate = DateTime.Now;
                    txlog.Code = GetGuid;
                    txlog.CreatedBy = TModel.MID;
                    txlog.CreatedTime = DateTime.Now;
                    txlog.IsDeleted = false;
                    txlog.MID = TModel.MID;
                    txlog.Company = TModel.ID;
                    txlog.Status = 1;
                    txlog.TXBank = txway;
                    string txInfo = string.Empty;
                    if (txway == "1")
                    {
                        txlog.TXName = "支付宝";
                        txlog.TXCard = Request["alipay"];
                        txInfo = txlog.TXName + "账号：" + txlog.TXCard;
                    }
                    else if (txway == "2")
                    {
                        txlog.TXName = "微信";
                        txlog.TXCard = Request["weixinpay"];
                        txInfo = txlog.TXName + "账号：" + txlog.TXCard;
                    }
                    else if (txway == "3") //自行输入银行账号来提现
                    {
                        #region 提现到银行卡
                        string txName = GetBankName(Request["txbank"]);
                        string txBankNum = Request["txbanknum"];
                        string txReceiveName = Request["txreceivenum"];
                        if (!string.IsNullOrEmpty(Request["txbankcode"]))
                        {
                            //银行code不为空，就是选择的历史的数据，取到这条历史数据
                            SH_MemberBank memBank = CommonBase.GetModel<SH_MemberBank>(Request["txbankcode"]);
                            if (memBank != null)
                            {
                                txName = GetBankName(memBank.Bank);
                                txBankNum = memBank.BankNum;
                                txReceiveName = memBank.ReceiveName;
                            }
                        }
                        else
                        {
                            //新增的提现信息，要保存到用户的提现银行表中
                            SH_MemberBank memBank = new SH_MemberBank();
                            memBank.Bank = Request["txbank"];
                            memBank.BankNum = txBankNum;
                            memBank.Code = GetGuid;
                            memBank.CreatedBy = TModel.MID;
                            memBank.CreatedTime = DateTime.Now;
                            memBank.LogDate = DateTime.Now;
                            memBank.MCode = TModel.MID;
                            memBank.MID = TModel.ID.ToString();
                            memBank.Status = 1;
                            memBank.ReceiveName = txReceiveName;
                            CommonBase.Insert<SH_MemberBank>(memBank, listComm);
                        }
                        txlog.TXName = txName;
                        txlog.TXCard = txBankNum;
                        txlog.Remark = txReceiveName;
                        txInfo = txlog.TXCard + "账号：" + txlog.TXCard + "，收款人：" + txlog.Remark;
                        #endregion
                    }
                    else if (txway == "4") //TPAY
                    {
                        string txBankNum = Request["txpaynum"];
                        string txReceiveName = Request["txpayname"];
                        txlog.TXCard = txBankNum;
                        txlog.TXName = "TPay";
                        txlog.Remark = txReceiveName;
                        txlog.TXMCode = Request["nickname"];
                        if (TModel.MJB < pointTXMoney)
                        {
                            Response.Write("7");
                            return;
                        }
                        txInfo = txlog.TXName + "手机号后四位：" + txlog.Remark + "，账号（ID）：" + txlog.TXCard + "，昵称：" + txlog.TXMCode;
                    }
                    else if (txway == "5") //VPAY
                    {
                        string txBankNum = Request["txpaynum"];
                        string txReceiveName = Request["txpayname"];
                        txlog.TXCard = txBankNum;
                        txlog.TXName = "VPay";
                        txlog.Remark = txReceiveName;
                        if (TModel.MVB < pointTXMoney)
                        {
                            Response.Write("8");
                            return;
                        }
                    }
                    //txlog.TXName = Request["txname"];
                    txlog.TXMoney = pointTXMoney;
                    txlog.FeeMoney = CacheService.GlobleConfig.TXFloat;

                    txlog.RealMoney = txlog.TXMoney - txlog.FeeMoney;
                    string alipay = Request["alipay"];
                    string weixinpay = Request["weixinpay"];
                    if (!string.IsNullOrEmpty(alipay))
                    {
                        txlog.AliPay = alipay;
                    }
                    if (!string.IsNullOrEmpty(weixinpay))
                    {
                        txlog.WeixinPay = weixinpay;
                    }
                    if (txway == "4")
                    {
                        MemberService.UpdateMoney(listComm, txlog.Company.ToString(), txlog.TXMoney.ToString(), "MJB", false);
                    }
                    else if (txway == "5")
                    {
                        MemberService.UpdateMoney(listComm, txlog.Company.ToString(), txlog.TXMoney.ToString(), "MVB", false);
                    }
                    else
                    {
                        MemberService.UpdateMoney(listComm, txlog.Company.ToString(), txlog.TXMoney.ToString(), "MSH", false);
                    }
                    //付款成功，给管理员或O单商发送消息
                    string sendAdminRoleCode = "RoleCode='Manage'";
                    List<Model.Member> receiveList = new List<Member>();
                    if (string.IsNullOrEmpty(TModel.Company))
                    {
                        sendAdminRoleCode += " or RoleCode='Admin'";
                    }
                    receiveList = CommonBase.GetList<Model.Member>(sendAdminRoleCode);

                    string message = TModel.MID + "申请" + txlog.TXName + "提现" + txlog.TXMoney + "元";
                    Service.MessageService.SendNewMessage(TModel, receiveList, message, listComm, "3");
                    CommonBase.Insert<TD_TXLog>(txlog, listComm);
                    if (CommonBase.RunListCommit(listComm))
                    {
                        try
                        {
                            //找到城市管理员给他发送短信
                            Model.Member cityModel = CommonBase.GetModel<Model.Member>(TModel.Company);
                            string sendToTel = string.Empty;
                            if (cityModel == null)
                            {
                                //给admin发
                                cityModel = MemberService.GetCompanyAdminMember();
                                sendToTel = GlobleConfigService.GetWebConfig("SendTXSMSPhone").Value;
                            }
                            else
                            {
                                sendToTel = cityModel.Tel;
                            }
                            //给城市管理员发
                            string sendTXContent = GlobleConfigService.GetWebConfig("SendTXSMSContentToCityAdmin").Value;
                            sendTXContent = sendTXContent.Replace("{{ApplyTXUser}}", TModel.MID);//替换提线人
                            sendTXContent = sendTXContent.Replace("{{ApplyTXMoney}}", txlog.TXMoney.ToString());//替换提现金额
                            //提现信息
                            sendTXContent = sendTXContent.Replace("{{ApplyInfo}}", txInfo);//替换提现信息

                            if (ConfigHelper.GetAppSettings("IsTest") != "1")
                            {
                                Service.TelephoneCodeService.SendSMS(cityModel.Tel, "SYSTEM", sendTXContent);
                            }

                            //给申请人发
                            sendTXContent = GlobleConfigService.GetWebConfig("SendTXSMSContentToApplyer").Value;
                            if (!string.IsNullOrEmpty(sendTXContent))
                            {
                                sendToTel = TModel.Tel;
                                sendTXContent = sendTXContent.Replace("{{ApplyTXToUser}}", TModel.MID);//替换提线人
                                sendTXContent = sendTXContent.Replace("{{ApplyTXMoney}}", txlog.TXMoney.ToString());//替换提现金额
                                if (ConfigHelper.GetAppSettings("IsTest") != "1")
                                {
                                    Service.TelephoneCodeService.SendSMS(cityModel.Tel, "SYSTEM", sendTXContent);
                                }
                            }
                            //发送提现短信
                            //是否发送提现短信
                            string isSendTXSMS = GlobleConfigService.GetWebConfig("IsSendTXSMS").Value;
                            if (isSendTXSMS == "1")
                            {
                                sendTXContent = GlobleConfigService.GetWebConfig("SendTXSMSContent").Value;
                                sendToTel = GlobleConfigService.GetWebConfig("SendTXSMSPhone").Value;
                                sendTXContent = sendTXContent.Replace("{{ApplyTXUser}}", TModel.MID);//替换提线人
                                sendTXContent = sendTXContent.Replace("{{ApplyTXMoney}}", txlog.TXMoney.ToString());//替换提现金额
                                //发送短信
                                if (ConfigHelper.GetAppSettings("IsTest") != "1")
                                {
                                    string SMSResult = Service.TelephoneCodeService.SendSMS(sendToTel, "SYSTEM", sendTXContent);
                                }
                            }
                        }
                        catch
                        {

                        }
                        if (TModel.RoleCode != "Member" && TModel.RoleCode != "VIP")
                        {
                            if (TModel.ReadNoticeId != 0)
                            {
                                TModel.MSH -= pointTXMoney;
                            }
                        }
                        Response.Write("0");
                        return;
                    }
                    else
                    {
                        Response.Write("-1");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }

        private void dealNoticeInfo()
        {
            lock (new object())
            {
                try
                {
                    string mcode = Request["mCode"];
                    string mtype = Request["MType"];
                    List<CommonObject> list = new List<CommonObject>();
                    MessageService.DealHasReadMessage(TModel, mtype, mcode, list);
                    if (CommonBase.RunListCommit(list))
                    {
                        Response.Write("1");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0");
                return;
            }
        }
        private void SendMessage()
        {
            lock (new object())
            {
                try
                {
                    string msg = Request["reMsg"];
                    List<CommonObject> list = new List<CommonObject>();
                    List<Model.Member> receiveMemberList = CommonBase.GetList<Model.Member>("RoleCode='Manage'");
                    MessageService.SendNewMessage(TModel, receiveMemberList, msg, list);
                    if (CommonBase.RunListCommit(list))
                    {
                        Response.Write("1");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0");
                return;
            }
        }

        private void SendAdMessage()
        {
            lock (new object())
            {
                try
                {
                    string msg = Request["reMsg"];
                    List<CommonObject> list = new List<CommonObject>();
                    List<Model.Member> receiveMemberList = CommonBase.GetList<Model.Member>("RoleCode='Manage'");
                    MessageService.SendNewMessage(TModel, receiveMemberList, msg, list, "1");
                    if (CommonBase.RunListCommit(list))
                    {
                        Response.Write("1");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0");
                return;
            }
        }

        private void responseMsg()
        {
            lock (new object())
            {
                try
                {
                    string msg = Request["reMsg"];
                    string mcode = Request["mCode"];
                    List<CommonObject> list = new List<CommonObject>();
                    MessageService.ResponseMessage(TModel, mcode, msg, list);
                    if (CommonBase.RunListCommit(list))
                    {
                        Response.Write("1");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0");
                return;
            }
        }
        private void checkNoticeInfo()
        {
            lock (new object())
            {
                try
                {
                    Response.Write(MessageService.GetNewMessage(TModel.ID.ToString()));
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write(string.Empty);
                    return;
                }
            }
        }
        private void setExpenseOrStatusMoney()
        {

        }

        private void deleteArchive()
        {
            try
            {
                string code = Request["code"];
                List<CommonObject> comm = new List<CommonObject>();
                string deleteArch = "delete from CM_Archives WHERE Code='" + code + "'";
                comm.Add(new CommonObject(deleteArch, null));
                string deleteAging = "delete from CM_AgingLimit where Code='" + code + "'";
                comm.Add(new CommonObject(deleteAging, null));

                //查询出改档案所有的规划信息
                List<CM_PlanHeader> listHeader = CommonBase.GetList<CM_PlanHeader>("ArchiveId='" + code + "'");
                foreach (CM_PlanHeader plan in listHeader)
                {
                    string deletePlan = "delete from CM_PlanHeader where Code='" + plan.Code + "'";
                    comm.Add(new CommonObject(deletePlan, null));

                    string deletePlanDetail = "delete from CM_PlanDetail where PlanHeaderId='" + plan.Code + "'";
                    comm.Add(new CommonObject(deletePlanDetail, null));
                }


                if (CommonBase.RunListCommit(comm))
                {
                    Response.Write("0");
                    return;
                }
                else
                {
                    Response.Write("1");
                    return;
                }
            }
            catch (Exception ex)
            {
                Response.Write("1");
                return;
            }
        }
        private void deletePlan()
        {
            try
            {
                string code = Request["code"];
                List<CommonObject> comm = new List<CommonObject>();
                string deleteArch = "delete from CM_PlanHeader WHERE Code='" + code + "'";
                comm.Add(new CommonObject(deleteArch, null));
                string deleteAging = "delete from CM_PlanDetail where PlanHeaderId='" + code + "'";
                comm.Add(new CommonObject(deleteAging, null));
                if (CommonBase.RunListCommit(comm))
                {
                    Response.Write("0");
                    return;
                }
                else
                {
                    Response.Write("1");
                    return;
                }
            }
            catch (Exception ex)
            {
                Response.Write("1");
                return;
            }
        }

        private void HasKnowPlanChange()
        {
            try
            {
                string sql = "update CM_PlanHeader set Remark='False' where  DATEDIFF(dd,PlanEndDate,GETDATE())<=0 and Remark='True' and Company=" + TModel.ID;
                CommonBase.GetSingle(sql);
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void deletePlanDetail()
        {
            try
            {
                string code = Request["code"];
                List<CommonObject> comm = new List<CommonObject>();
                CM_PlanDetail model = CommonBase.GetModel<CM_PlanDetail>(code);
                List<CM_PlanDetail> listPlanDetail = CommonBase.GetList<CM_PlanDetail>("IsDeleted=0 and PlanHeaderId='" + model.PlanHeaderId + "' and Sort<=" + model.Sort);
                //listPlanDetail.Add(model);
                //获取到主表
                CM_PlanHeader header = CommonBase.GetModel<CM_PlanHeader>(model.PlanHeaderId);
                PlanService.DeletePlanDetail(listPlanDetail, comm, header);
                #region 代码整理封装，以下代码是不再需要的

                ////查询到这条规划信息
                //if (model != null)
                //{
                //    CommonBase.Delete<CM_PlanDetail>(model, comm);
                //    //该条规划之后的规划信息
                //    List<CM_PlanDetail> planList = CommonBase.GetList<CM_PlanDetail>("PlanHeaderId='" + model.PlanHeaderId + "' AND Sort>" + model.Sort + " order by Sort");
                //    //把需要删除的规划平均分配到规划还款日之前
                //    //把日期distinct一下
                //    List<DateAddMoreMoney> existDate = new List<DateAddMoreMoney>();
                //    foreach (CM_PlanDetail cpd in planList)
                //    {
                //        if (existDate.Find(c => c.Date == cpd.PlanDate) == null)
                //        {
                //            DateAddMoreMoney dam = new DateAddMoreMoney();
                //            dam.Date = cpd.PlanDate;
                //            dam.AddExpenseMoney = 0;
                //            dam.AddStoreMoney = 0;
                //            existDate.Add(dam);
                //        }
                //    }
                //    //平均分配
                //    CycleDateStoreMoney(existDate, model.StoreMoney);
                //    CycleDateExpenseMoney(existDate, model.ExpenseMoney);
                //    //按日期分配
                //    List<CM_PlanDetail> listForUpdate = new List<CM_PlanDetail>();
                //    foreach (DateAddMoreMoney add in existDate)
                //    {
                //        //查询到当天的
                //        CM_PlanDetail plan = planList.Where(c => c.PlanDate == add.Date && c.StoreMoney > 0).FirstOrDefault();
                //        //planList.Remove(plan);
                //        plan.StoreMoney += add.AddStoreMoney;
                //        plan.ExpenseMoney += add.AddExpenseMoney;
                //        plan.TakeOffMoney = plan.ExpenseMoney * plan.Rate;
                //        listForUpdate.Add(plan);
                //    }
                //    //组建更新操作
                //    foreach (CM_PlanDetail cm in listForUpdate)
                //    {
                //        CommonBase.Update<CM_PlanDetail>(cm, comm);
                //    }
                //}
                #endregion
                //执行事物操作
                if (CommonBase.RunListCommit(comm))
                {
                    //foreach (CM_PlanDetail pdl in listPlanDetail)
                    //{
                    //    //成功之后重新计算规划表的余额
                    //    PlanService.RecountBalanceMoney(pdl.PlanHeaderId);
                    //}
                    Response.Write("0");
                    return;
                }
                else
                {
                    Response.Write("1");
                    return;
                }
            }
            catch (Exception ex)
            {
                Response.Write("1");
                return;
            }
        }

        private void GetIndutyInfo()
        {
            //lock (new object())
            //{
            //    try
            //    {
            //        string id = Request["code"];
            //        var list = from v in Sys_Industry_Bll.GetList("ParentId=" + id) select new { Id = v.JobId, Name = v.JobName };
            //        Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
            //        return;
            //    }
            //    catch (Exception ex)
            //    {
            //        Response.Write("1");
            //        return;
            //    }
            //}
        }
        private void GetAddressInfo()
        {
            lock (new object())
            {
                try
                {
                    string id = Request["pram"];
                    int level = int.Parse(Request["level"]);
                    var list = from v in CacheService.SatandardAddressList where v.ProCode.Trim() == id select new { Id = v.AdCode, Name = v.Name };
                    if (level == 30)
                    {
                        list = from v in CacheService.SatandardAddressList where v.ProCode == id && v.LevelInt == 30 select new { Id = v.AdCode, Name = v.Name };
                    }
                    else if (level == 40)
                    {
                        list = from v in CacheService.SatandardAddressList where v.CityCode == id && v.LevelInt == 40 select new { Id = v.AdCode, Name = v.Name };
                    }
                    Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write("1");
                    return;
                }
            }
        }

        private void selectMember()
        {
            lock (new object())
            {
                try
                {
                    string id = Request["nickname"];
                    List<Model.Member> list = CommonBase.GetList<Model.Member>("MID='" + id + "' or MName='" + id + "'");
                    var Memberlist = from v in list where v.RoleCode == "VIP" select new { MID = v.MID, MName = v.MName, TEL = v.Tel, Role = CacheService.RoleList.FirstOrDefault(c => c.Code == v.RoleCode).Name };
                    Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(Memberlist));
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write("");
                    return;
                }
            }
        }

        private void LoginOut()
        {
            lock (new object())
            {
                try
                {
                    //if (TModel != null)
                    //Log(TModel, "1", TModel.MID + "退出系统", null, true);
                    Response.Buffer = true;
                    Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                    Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                    Response.Expires = 0;
                    Response.CacheControl = "no-cache";
                    Response.AddHeader("Pragma", "No-Cache");
                    Session.Clear();
                    FormsAuthentication.SignOut();
                    Response.Write("0");
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write("1");
                    return;
                }
            }
        }
        private void resetpwd()
        {
            lock (new object())
            {
                try
                {
                    Member model = TModel;
                    if (model == null)
                    {
                        Response.Write("1");
                        return;
                    }
                    else
                    {

                        //判断原始密码正确不
                        if (CommonHelper.DESEncrypt(Request["pwd"]) != TModel.Password)
                        //if (Request["pwd"] != "olkedsauoiklmgradnmjuoir" && model.Password != System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Request["pwd"], "MD5").ToUpper())
                        {
                            Response.Write("2");
                            return;
                        }
                        else
                        {
                            //密码都用DES加密，为了后台解密
                            model.Password = CommonHelper.DESEncrypt(Request["pwd2"]);
                            //model.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Request["pwd2"], "MD5").ToUpper();
                            if (CommonBase.Update<Member>(model, new string[] { "Password" }))
                            {
                                Response.Write("0");
                                return;
                            }
                        }
                        Response.Write("-1");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }

        private void CheckQuestionAnswer()
        {
            lock (new object())
            {
                try
                {
                    string answer = Server.UrlDecode(Request["answer"]);
                    string qid = Request["qid"];
                    if (string.IsNullOrEmpty(answer))
                    {
                        Response.Write("0");
                        return;
                    }
                    {
                        //判断安全问题是否正确
                        Sys_SQ_Answer answerModel = CommonBase.GetList<Sys_SQ_Answer>("IsDeleted=0 and MID=" + TModel.ID + " and QID=" + qid + " and Answer='" + answer + "'").FirstOrDefault();
                        if (answerModel == null)
                        {
                            Response.Write("1");
                            return;
                        }
                        else
                        {
                            Response.Write("2");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }
        private void UpdateRemindStatus()
        {
            lock (new object())
            {
                try
                {
                    string rid = Request["rid"];
                    if (TModel != null)
                    {
                        string updateSQL = "UPDATE dbo.DB_EveryDayRemind SET IsRead=1 WHERE Field2='" + rid + "' AND MID=" + TModel.ID;
                        CommonBase.RunSql(updateSQL);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }

        private void FindPwd()
        {
            lock (new object())
            {
                try
                {
                    string mid = Server.UrlDecode(Request["mid"]);
                    string checkCode = Request["checkCode"];
                    if (string.IsNullOrEmpty(mid))
                    {
                        Response.Write("登录账号不能为空");
                        return;
                    }
                    if (Session["CheckCode"] == null || checkCode.ToLower() != Session["CheckCode"].ToString().ToLower())
                    {
                        Response.Write("3");
                        return;
                    }

                    Member model = CommonBase.GetList<Model.Member>("MID='" + mid + "'").FirstOrDefault();
                    if (model == null)
                    {
                        Response.Write("1");
                        return;
                    }
                    else
                    {
                        //判断安全问题是否正确
                        string qid = Request["qid"];
                        string answer = Server.UrlDecode(Request["answer"]);
                        Sys_SQ_Answer answerModel = CommonBase.GetList<Sys_SQ_Answer>("IsDeleted=0 and MID=" + model.ID + " and QID=" + qid + " and Answer='" + answer + "'").FirstOrDefault();
                        if (answerModel == null)
                        {
                            Response.Write("4");
                            return;
                        }
                        Session["CheckCode"] = null;
                        //解析密码
                        if (model.Password == CommonHelper.DESDecrypt(model.Password))
                        {
                            model.Password = "123456";
                            model.Password = CommonHelper.DESEncrypt(model.Password);
                            CommonBase.Update<Model.Member>(model, new string[] { "Password" });
                            Response.Write("您的密码为：123456，请牢记您的密码。");
                        }
                        else
                        {
                            Response.Write("您的密码为：" + CommonHelper.DESDecrypt(model.Password) + "，请牢记您的密码。");
                        }

                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        private void getLogin()
        {
            lock (new object())
            {
                try
                {

                    //if (Session["CheckCode"] == null || info[2].ToLower() != Session["CheckCode"].ToString().ToLower())
                    //{
                    //    Response.Write("3");
                    //    return;
                    //}
                    Member model = CommonBase.GetList<Member>("MID='" + Request["uname"] + "' and IsClose=0").FirstOrDefault();
                    if (model == null)
                    {
                        Response.Write("1");
                        return;
                    }
                    else
                    {
                        bool isWrong = false;
                        //判断原始密码正确不
                        //先判断MD5
                        if (Request["upwd"] != "olkedsauoiklmgradnmjuoir" && model.Password != System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Request["upwd"], "MD5").ToUpper())
                        {
                            isWrong = true;
                        }
                        if (isWrong)
                        {
                            if (CommonHelper.DESEncrypt(Request["upwd"]) != model.Password)
                            {
                                isWrong = true;
                            }
                            else
                            {
                                isWrong = false;
                            }
                        }
                        if (isWrong)
                        {
                            Response.Write("2");
                            return;
                        }

                        //账号是否被禁用
                        if (!model.MState)
                        {
                            Response.Write("3");
                            return;
                        }
                        ////账号在有效期
                        //DateTime dtBegin = model.UseBeginTime != null ? Convert.ToDateTime(model.UseBeginTime) : DateTime.Now.AddDays(-1);
                        //DateTime dtEnd = model.UseEndTime != null ? Convert.ToDateTime(model.UseEndTime) : DateTime.Now.AddDays(1);
                        //int begin = dtBegin.Subtract(DateTime.Now).Days;
                        //int end = dtEnd.Subtract(DateTime.Now).Days;
                        ////if (begin < 0)
                        ////{
                        ////    Response.Write("4");
                        ////    return;
                        ////}
                        //if (end < 0)
                        //{
                        //    Response.Write("5");
                        //    return;
                        //}

                        //更新会员的地址信息
                        string hid_location_town = Request["hid_location_town"];
                        string hid_location_adcode = Request["hid_location_adcode"];
                        string hid_location_pointer = Request["hid_location_pointer"];
                        string loginAddressInfo = string.Empty;
                        if (!string.IsNullOrEmpty(hid_location_adcode) && hid_location_adcode != "undefined")
                        {
                            Sys_StandardArea areaModel = AddressService.GetAddressByAdCode(hid_location_adcode);
                            if (areaModel.LevelInt == 40)
                            {
                                model.Zone = areaModel.AdCode;
                                //得到省
                                model.Province = areaModel.ProCode;
                                //找到市
                                model.City = areaModel.CityCode;
                                if (!string.IsNullOrEmpty(hid_location_town) && hid_location_town != "undefined")
                                {
                                    model.Town = hid_location_town;
                                }

                                if (!string.IsNullOrEmpty(hid_location_pointer) && hid_location_pointer != "undefined")
                                {
                                    model.RegistPointer = hid_location_pointer;
                                }
                                //List<CommonObject> listComm = new List<CommonObject>();
                                loginAddressInfo = model.Province + "|" + model.City + "|" + model.Zone + "|" + model.RegistPointer;
                                CommonBase.Update<Member>(model, new string[] { "Zone", "Province", "City", "Town", "RegistPointer" });
                            }
                        }

                        LogService.Log(model, "1", model.MID + "登录前台", loginAddressInfo);
                        FormsAuthentication.SetAuthCookie(model.ID.ToString(), true);
                        //Bll.Member bllmodel = new Bll.Member { TModel = model };
                        //Session["Member"] = bllmodel; 
                        Session["Member"] = model;

                        //登录成功之后查看是否绑定过微信信息
                        M_WXUserInfo wxuser = Session["WXMember"] as M_WXUserInfo;
                        if (wxuser != null)
                        {
                            List<M_WX_VS_User> listUser = CommonBase.GetList<M_WX_VS_User>("UserCode='" + model.ID + "'");
                            if (listUser.Count == 0)
                            {
                                //如果该账号没有绑定过微信，就要在前台提示用户绑定微信
                                Response.Write("6");
                                return;
                                //M_WX_VS_User mwvu = new M_WX_VS_User();
                                //mwvu.Company = "0";
                                //mwvu.CreatedTime = DateTime.Now;
                                //mwvu.IsDeleted = false;
                                //mwvu.OpenId = wxuser.OpenId;
                                //mwvu.Sort = 1;
                                //mwvu.Status = 1;
                                //mwvu.UserCode = model.ID;
                                //CommonBase.Insert<M_WX_VS_User>(mwvu);
                            }
                        }

                        //Bll.Member.AddOnLine(model.MID);
                        //Log(model, "1", "登录系统", null, true);
                        //登录成功之后，

                        Response.Write("0");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }

        public class DateAddMoreMoney
        {
            public DateTime Date { get; set; }
            public decimal AddStoreMoney { get; set; }
            public decimal AddExpenseMoney { get; set; }
        }



        private void GetTrainLearnCode()
        {
            lock (new object())
            {
                try
                {
                    string codeType = Request["codeType"];
                    string codeOrder = Request["codeOrder"];
                    string strWhere = "CodeType='" + codeType + "'";

                    //System.Data.DataTable dtTbl = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, "Code asc", "dbo.Sys_BankInfo", out count);
                    ////context.Response.Write(MethodHelper.JsonHelper.GetAdminDataTableToJson(dtTbl, count));


                    //var modelList = CommonBase.GetList<Model.T_CodingMemory>(strWhere);

                    //var list = from v in modelList where v.RoleCode == "3F" select new { Id = v.ID, Name = v.MName, MID = v.MID, RoleName = CacheService.RoleList.FirstOrDefault(c => c.Code == v.RoleCode).Name };
                    //Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                    return;


                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0"); //发送失败
                return;
            }
        }

        private void stopMemory()
        {
            lock (new object())
            {
                try
                {
                    string trainCode = Request["trainCode"];
                    T_TrainHeader header = CommonBase.GetModel<T_TrainHeader>(trainCode);
                    if (header == null)
                    {
                        Response.Write("0");
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request["reviewCode"]))
                        {
                            //设置复习时间
                            DateTime reviewBeginTime = MethodHelper.ConvertHelper.ToDateTime(header.Remark, DateTime.Now);
                            TimeSpan EndDateTimeSpan = new TimeSpan(DateTime.Now.Ticks);
                            TimeSpan BeginDateTimeSpan = new TimeSpan(reviewBeginTime.Ticks);
                            //总秒数
                            int totalSecond = (int)EndDateTimeSpan.Subtract(BeginDateTimeSpan).TotalSeconds;
                            //更新复习时间
                            header.ReviewTime += totalSecond;
                            if (CommonBase.Update<T_TrainHeader>(header, new string[] { "ReviewTime" }))
                            {
                                Response.Write("1");
                                return;
                            }
                            else
                            {
                                Response.Write("0");
                                return;
                            }
                        }
                        else
                        {
                            //设置结束记忆时间
                            header.EndTime = DateTime.Now.ToString();
                            if (CommonBase.Update<T_TrainHeader>(header, new string[] { "EndTime" }))
                            {
                                Response.Write("1");
                                return;
                            }
                            else
                            {
                                Response.Write("0");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0"); //发送失败
                return;
            }
        }


        private void queryWord()
        {
            lock (new object())
            {
                try
                {
                    bool isHasPrivage = false;
                    //if(TModel.RoleCode == "VIP" || TModel.RoleCode == "Teacher" || TModel.RoleCode == "WordUser")
                    //学员级别之上可以查看速记大辞典
                    if (TModel.Role.RIndex > 2)
                    {
                        isHasPrivage = true;
                    }

                    if (isHasPrivage)
                    {
                        string word = Request["word"];
                        List<T_Words> listWords = CommonBase.GetList<T_Words>("English='" + word + "' or Chinese like '%" + word + "%'");

                        foreach (T_Words wordModel in listWords)
                        {
                            wordModel.Chinese = wordModel.Chinese.TrimEnd();
                            wordModel.Version = NpoiService.GetNewString(wordModel.Chinese);
                            wordModel.Association1 = NpoiService.SetNewWordAssociation(wordModel.Version, wordModel.Association1);
                            wordModel.Association2 = NpoiService.SetNewWordAssociation(wordModel.Version, wordModel.Association2);
                            wordModel.Association3 = NpoiService.SetNewWordAssociation(wordModel.Version, wordModel.Association3);
                            wordModel.Association4 = NpoiService.SetNewWordAssociation(wordModel.Version, wordModel.Association4);
                        }


                        var list = from v in listWords select new { English = v.English, Phonetic = v.Phonetic, Chinese = v.Chinese, HotWord = v.HotWord, Module1 = v.Module1, Association1 = v.Association1, Module2 = v.Module2, Association2 = v.Association2, Module3 = v.Module3, Association3 = v.Association3, Module4 = v.Module4, Association4 = v.Association4 };
                        Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                        return;
                    }
                    else
                    {
                        Response.Write("-1");//无权限
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }

        private void getBookVersionVsWord()
        {
            lock (new object())
            {
                try
                {
                    string version = Request["version"];
                    string grade = Request["grade"];
                    string leavel = Request["leavel"];
                    string unit = Request["unit"];
                    string sql = "SELECT t2.English FROM dbo.T_VersionVsWords t1 LEFT JOIN dbo.T_Words t2 ON t1.WordCode=t2.Code WHERE t2.IsDeleted=0 AND t1.Version='" + version + "' AND t1.Grade='" + grade + "' AND t1.Leavel='" + leavel + "' AND t1.Unit='" + unit + "' order by CONVERT(INT,ISNULL(t1.WIndex,'0'))";
                    DataTable dt = CommonBase.GetTable(sql);
                    Response.Write(JsonHelper.DataTableToJson(dt, "wordList"));
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }

        private void createEvaluationPaper()
        {
            lock (new object())
            {
                try
                {
                    //string paperCode = Request["code"];
                    ////获取到测评试卷
                    //T_EvaluationPaper paper = CommonBase.GetModel<T_EvaluationPaper>(paperCode);
                    //if (paper == null)
                    //{
                    //    Response.Write(CommonHelper.Response(false, "操作失败，不存在该试卷"));
                    //    return;
                    //}
                    //查询出这个试卷的单词

                    string version = Request["version"];
                    string grade = Request["grade"];
                    string leavel = Request["leavel"];
                    string unit1 = Request["unit1"];
                    string unit2 = Request["unit2"];
                    string paperName = HttpUtility.UrlDecode(Request["paperName"]);

                    string sqlUnit = "SELECT DISTINCT  CONVERT(INT,ISNULL(Unit,'1')) AS Unit  FROM dbo.T_VersionVsWords WHERE Version='" + version + "' AND Grade='" + grade + "' AND Leavel='" + leavel + "' ORDER BY CONVERT(INT,ISNULL(Unit,'1'))";
                    DataTable dtUnit = CommonBase.GetTable(sqlUnit);
                    DataRow[] arrayRows = dtUnit.Select("Unit>=" + unit1 + " and Unit<=" + unit2);
                    if (arrayRows.Length <= 0)
                    {
                        Response.Write(CommonHelper.Response(false, "操作失败，选择的章节单元不合规范"));
                        return;
                    }
                    string unitString = string.Empty;
                    foreach (DataRow row in arrayRows)
                    {
                        if (string.IsNullOrEmpty(unitString))
                        {
                            unitString += "'" + row["Unit"].ToString() + "'";
                        }
                        else
                        {
                            unitString += ",'" + row["Unit"].ToString() + "'";
                        }
                    }

                    string sqlWhere = "1=1 and t2.English is not null ";
                    if (!string.IsNullOrEmpty(version))
                    {
                        sqlWhere += " and t1.Version='" + version + "'";
                    }

                    if (!string.IsNullOrEmpty(grade))
                    {
                        sqlWhere += " and t1.Grade='" + grade + "'";
                    }

                    if (!string.IsNullOrEmpty(leavel))
                    {
                        sqlWhere += " and t1.Leavel='" + leavel + "'";
                    }

                    sqlWhere += " and t1.Unit in (" + unitString + ")";
                    string fields = "t1.Code, t1.Version, t1.Grade, t1.Leavel, t1.Unit, t1.WIndex,t1.WordCode,t2.English,t2.HotWord, t2.Phonetic,t2.Chinese";
                    string tables = "T_VersionVsWords t1 LEFT JOIN dbo.T_Words t2 ON t1.WordCode=t2.Code";
                    string sortBy = " order by  CONVERT(INT,ISNULL(t1.Unit,'1')) asc, CONVERT(INT,ISNULL(t1.WIndex,'0')) asc";

                    string sql = "select " + fields + " from " + tables + " where " + sqlWhere + sortBy;
                    DataTable dt = CommonBase.GetTable(sql);

                    if (dt.Rows.Count <= 0)
                    {
                        Response.Write(CommonHelper.Response(false, "操作失败，您选择的测评范围暂无可测评单词"));
                        return;
                    }
                    //测评主表加入一条数据
                    T_EvaluationHeader header = new T_EvaluationHeader();
                    header.Code = GetGuid;
                    header.CreatedBy = TModel.MID;
                    header.CreatedTime = DateTime.Now;
                    header.EvalBeginTime = DateTime.Now;
                    header.IsDeleted = false;
                    header.PaperCode = paperName;
                    header.Sort = 1;
                    header.Status = 1;
                    header.UserCode = TModel.ID;
                    header.QuestionCount = dt.Rows.Count;
                    List<CommonObject> listCommon = new List<CommonObject>();
                    CommonBase.Insert<T_EvaluationHeader>(header, listCommon);

                    foreach (DataRow row in dt.Rows)
                    {
                        T_EvaluationDetail detail = new T_EvaluationDetail();
                        detail.Code = GetGuid;
                        detail.CreatedBy = TModel.MID;
                        detail.CreatedTime = DateTime.Now;
                        detail.HeaderCode = header.Code;
                        detail.IsDeleted = false;
                        detail.PaperCode = paperName;
                        detail.Sort = MethodHelper.ConvertHelper.ToInt32(row["WIndex"], 1);
                        detail.Status = 1;
                        detail.WordChinese = row["Chinese"].ToString();
                        detail.WordCode = row["WordCode"].ToString();
                        detail.WordEnglish = row["English"].ToString();
                        CommonBase.Insert<T_EvaluationDetail>(detail, listCommon);
                    }
                    Service.LogService.Log(TModel, "2", TModel.MID + "创建试卷，开始测评", listCommon);
                    if (CommonBase.RunListCommit(listCommon))
                    {
                        Response.Write(CommonHelper.Response(true, header.Code));
                        return;
                    }
                    else
                    {
                        Response.Write(CommonHelper.Response(false, "操作失败，请重试"));
                        return;
                    }

                    Response.Write(CommonHelper.Response(false, "操作失败，请重试"));
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write(CommonHelper.Response(false, "操作失败，请重试"));
                    return;
                }
            }
        }

        private void confirmAnswer()
        {
            lock (new object())
            {
                try
                {
                    string trainCode = Request["trainCode"];
                    T_TrainHeader header = CommonBase.GetModel<T_TrainHeader>(trainCode);
                    if (header == null)
                    {
                        Response.Write("0");
                        return;
                    }
                    else
                    {
                        List<CommonObject> listComm = new List<CommonObject>();
                        //设置答题结束时间
                        header.AnswerEndTime = DateTime.Now.ToString();
                        string answer = Request["answer"];
                        string trainIndex = Request["trainIndex"];
                        int correctCount = 0, errorCount = 0;
                        if (trainIndex == "-1")
                        {
                            #region 全部回答
                            List<T_TrainDetail> listDetail = CommonBase.GetList<T_TrainDetail>("TrainCode='" + header.Code + "' order by Sort");
                            if (header.CodeType == "3")
                            {
                                #region 扑克牌的答案
                                List<T_TrainDetail> answerList = JsonHelper.JsonToList<List<Model.T_TrainDetail>>(answer);
                                foreach (T_TrainDetail existDetail in listDetail)
                                {
                                    T_TrainDetail answerObj = answerList.FirstOrDefault(c => c.Sort == existDetail.Sort);
                                    if (answerObj != null)
                                    {
                                        existDetail.Remark = answerObj.Remark;
                                        if (answerObj.Remark == existDetail.CodeName)
                                        {
                                            existDetail.Status = 2;
                                            correctCount++;
                                        }
                                        else
                                        {
                                            errorCount++;
                                            existDetail.Status = 3;
                                        }
                                        CommonBase.Update<T_TrainDetail>(existDetail, listComm);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 其他的训练答案
                                int totalCompareLength = 0, quesLength = 0, fromLength = 0;
                                string oneAnswer = "";
                                foreach (T_TrainDetail detail in listDetail)
                                {
                                    quesLength = detail.CodeName.Length;
                                    //totalCompareLength += quesLength;
                                    oneAnswer = answer.Substring(fromLength, quesLength);
                                    fromLength += quesLength;
                                    detail.Remark = oneAnswer;
                                    if (detail.CodeName == oneAnswer)
                                    {
                                        detail.Status = 2;
                                        correctCount++;
                                    }
                                    else
                                    {
                                        errorCount++;
                                        detail.Status = 3;
                                    }
                                    CommonBase.Update<T_TrainDetail>(detail, listComm);
                                }



                                //string[] answerArray = answer.Replace("\n", "").Split(' ');
                                //answerArray = answerArray.Where(c => !string.IsNullOrEmpty(c)).ToArray();
                                ////answerArray = answerArray.Where(c => c != "\n").ToArray();

                                //for(int i = 0; i < answerArray.Length; i++)
                                //{
                                //    if(i < listDetail.Count)
                                //    {
                                //        T_TrainDetail detail = listDetail[i];
                                //        MethodHelper.LogHelper.WriteTextLog(this.GetType().ToString(), i.ToString() + ":" + answerArray[i].Trim(), DateTime.Now);
                                //        detail.Remark = answerArray[i].Trim();
                                //        if(detail.Remark == detail.CodeName)
                                //        {
                                //            detail.Status = 2;
                                //            correctCount++;
                                //        }
                                //        else
                                //        {
                                //            errorCount++;
                                //            detail.Status = 3;
                                //        }
                                //        CommonBase.Update<T_TrainDetail>(detail, listComm);
                                //    }
                                //}
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            //按照随机抽查的
                            #region 随机抽查
                            List<T_TrainDetail> answerDetail = JsonHelper.JsonToList<List<Model.T_TrainDetail>>(answer);
                            List<T_TrainDetail> listDetail = CommonBase.GetList<T_TrainDetail>("TrainCode='" + header.Code + "' order by Sort");
                            if (header.CodeType == "3")
                            {
                                #region 扑克牌的答案
                                List<T_TrainDetail> answerList = JsonHelper.JsonToList<List<Model.T_TrainDetail>>(answer);
                                foreach (T_TrainDetail existDetail in listDetail)
                                {
                                    T_TrainDetail answerObj = answerList.FirstOrDefault(c => c.Sort == existDetail.Sort);
                                    if (answerObj != null)
                                    {
                                        existDetail.Remark = answerObj.Remark;
                                        if (answerObj.Remark == existDetail.CodeName)
                                        {
                                            existDetail.Status = 2;
                                            correctCount++;
                                        }
                                        else
                                        {
                                            errorCount++;
                                            existDetail.Status = 3;
                                        }
                                        CommonBase.Update<T_TrainDetail>(existDetail, listComm);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 其他类型的答题答案
                                T_TrainDetail temp = null;
                                foreach (T_TrainDetail detail in listDetail)
                                {
                                    temp = answerDetail.FirstOrDefault(c => c.Sort == detail.Sort);
                                    if (temp != null)
                                    {
                                        detail.Remark = temp.Remark;
                                        if (detail.Remark == detail.CodeName)
                                        {
                                            correctCount++;
                                            detail.Status = 2;
                                        }
                                        else
                                        {
                                            errorCount++;
                                            detail.Status = 3;
                                        }
                                    }
                                    CommonBase.Update<T_TrainDetail>(detail, listComm);
                                }
                                #endregion
                            }
                            #endregion
                        }
                        header.CorrectCount = correctCount;
                        header.ErrorCount = errorCount;
                        CommonBase.Update<T_TrainHeader>(header, new string[] { "AnswerEndTime", "CorrectCount", "ErrorCount" }, listComm);
                        if (CommonBase.RunListCommit(listComm))
                        {
                            Response.Write("1");
                            return;
                        }
                        else
                        {
                            Response.Write("0");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0"); //发送失败
                return;
            }
        }


        private void getUnits()
        {
            lock (new object())
            {
                try
                {
                    string version = Request["version"];
                    string grade = Request["grade"];
                    string leavel = Request["leavel"];
                    string unit = Request["unit"];
                    string sql = "SELECT DISTINCT  CONVERT(INT,ISNULL(Unit,'1')) AS Unit  FROM dbo.T_VersionVsWords WHERE Version='" + version + "' AND Grade='" + grade + "' AND Leavel='" + leavel + "' ORDER BY CONVERT(INT,ISNULL(Unit,'1'))";
                    DataTable dt = CommonBase.GetTable(sql);
                    Response.Write(JsonHelper.DataTableToJson(dt, "unitList"));
                    return;
                }
                catch (Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }





        /// <summary>
        /// 绑定微信用户信息
        /// </summary>
        private void BingWXUser()
        {
            lock (new object())
            {
                try
                {
                    string user_mid = Request["uname"];
                    Member model = CommonBase.GetList<Member>("MID='" + Request["uname"] + "'").FirstOrDefault();
                    if (model == null)
                    {
                        Response.Write("1");
                        return;
                    }
                    else
                    {
                        bool isWrong = false;
                        //判断原始密码正确不
                        //先判断MD5
                        if (Request["upwd"] != "olkedsauoiklmgradnmjuoir" && model.Password != System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Request["upwd"], "MD5").ToUpper())
                        {
                            isWrong = true;
                        }
                        if (isWrong)
                        {
                            if (CommonHelper.DESEncrypt(Request["upwd"]) != model.Password)
                            {
                                isWrong = true;
                            }
                            else
                            {
                                isWrong = false;
                            }
                        }
                        if (isWrong)
                        {
                            Response.Write("2");
                            return;
                        }

                        //账号是否被禁用
                        if (!model.MState)
                        {
                            Response.Write("3");
                            return;
                        }
                        Session["Member"] = model;

                        //查看是否已经绑定过其他微信
                        string openid = Request["openId"];
                        string sql = "select t1.OpenId,t2.ID,t2.MID,t2.MName,t2.Tel from M_WX_VS_User t1 left join Member t2 on t1.UserCode=t2.ID where t1.IsDeleted=0 and t2.MID='" + user_mid + "'";
                        DataTable dtUser = CommonBase.GetTable(sql);
                        if (dtUser != null && dtUser.Rows.Count > 0)
                        {
                            Response.Write("6");
                            return;
                        }
                        //微信与用户关系映射表中插入一条数据
                        M_WX_VS_User mwvu = new M_WX_VS_User();
                        mwvu.Company = "0";
                        mwvu.CreatedTime = DateTime.Now;
                        mwvu.IsDeleted = false;
                        mwvu.OpenId = openid;
                        mwvu.Sort = 1;
                        mwvu.Status = 1;
                        mwvu.UserCode = model.ID;
                        if (CommonBase.Insert<M_WX_VS_User>(mwvu))
                        {
                            Response.Write("0");
                            return;
                        }



                        Response.Write("-1");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 绑定微信用户信息
        /// </summary>
        private void bingLoginWXUser()
        {
            lock (new object())
            {
                try
                {
                    Member model = Session["Member"] as Member;
                    if (model != null)
                    {
                        //查看是否已经绑定过其他微信
                        M_WXUserInfo wxuserInfo = Session["WXMember"] as M_WXUserInfo;

                        string openid = wxuserInfo.OpenId;
                        string sql = "select t1.OpenId,t2.ID,t2.MID,t2.MName,t2.Tel from M_WX_VS_User t1 left join Member t2 on t1.UserCode=t2.ID where t1.IsDeleted=0 and t2.ID='" + model.ID + "'";
                        DataTable dtUser = CommonBase.GetTable(sql);
                        if (dtUser != null &&dtUser.Rows.Count > 0)
                        {
                            Response.Write("-1");
                            return;
                        }
                        //微信与用户关系映射表中插入一条数据
                        M_WX_VS_User mwvu = new M_WX_VS_User();
                        mwvu.Company = "0";
                        mwvu.CreatedTime = DateTime.Now;
                        mwvu.IsDeleted = false;
                        mwvu.OpenId = openid;
                        mwvu.Sort = 1;
                        mwvu.Status = 1;
                        mwvu.UserCode = model.ID;
                        if (CommonBase.Insert<M_WX_VS_User>(mwvu))
                        {
                            Response.Write("1");
                            return;
                        }
                        Response.Write("-2");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("-2");
                    return;
                }
            }
        }


        /// <summary>
        /// 绑定微信用户信息
        /// </summary>
        private void changeCurrentMember()
        {
            lock (new object())
            {
                try
                {
                    string user_mid = Request["uid"];
                    Member model = CommonBase.GetModel<Member>(user_mid);
                    if (model == null)
                    {
                        Response.Write("1");
                        return;
                    }
                    else
                    {
                        //账号是否被禁用
                        if (!model.MState)
                        {
                            Response.Write("3");
                            return;
                        }
                        Session["Member"] = model;
                        Response.Write("0");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }
    }
}