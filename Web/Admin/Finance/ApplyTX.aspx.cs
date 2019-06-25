
using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Finance
{
    public partial class ApplyTX : BasePage
    {
        protected string GetCanTXMoney = "0", GetTXFloat = "0";
        protected override void SetPowerZone()
        {
            //可提现金额
            decimal msh = TModel.MSH;
            if (msh > CacheService.GlobleConfig.MinTXMoney)
            {
                decimal b = Math.Floor(msh / CacheService.GlobleConfig.BaseJifen);
                GetCanTXMoney = b.ToString();//(b * CacheService.GlobleConfig.MinTXMoney).ToString();
                GetTXFloat = CacheService.GlobleConfig.TXFloat.ToString();
            }
            //把微信和支付宝收款二维码加载上
            uploadImg.Value = TModel.AliPay;
            imgappendimg.Src = uploadImg.Value;

            uploadImgWeixin.Value = TModel.WeixinPay;
            imgappendimgWeixin.Src = uploadImgWeixin.Value;

            ddl_Bank.DataSource = CacheService.BankList;
            ddl_Bank.DataTextField = "Remark";
            ddl_Bank.DataValueField = "Code";
            ddl_Bank.DataBind();
            ddl_Bank.Items.Insert(0, new ListItem("选择提现银行", ""));
            //加载历史收款信息
            repBankList.DataSource = CommonBase.GetList<Model.SH_MemberBank>("MID='" + TModel.ID + "'");
            repBankList.DataBind();
        }
        protected override void SetValue(string id)
        {

        }
        protected override string btnAdd_Click()
        {
            if (MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_TXMoney"], 0) < CacheService.GlobleConfig.MinTXMoney)
                return "最低提现金额为" + CacheService.GlobleConfig.MinTXMoney;
            decimal pointTXMoney = 0;
            string money = Request.Form["txt_TXMoney"];
            string txway = Request.Form["txbank"];
            //查看提现积分是否足够
            decimal.TryParse(money, out pointTXMoney);
            if (pointTXMoney > TModel.MSH)
            {
                return "您的现金余额不足";
            }
            if (pointTXMoney <= 0)
            {
                return "提现金额应大于0";
            }

            List<CommonObject> listComm = new List<CommonObject>();
            //插入提现记录表
            TD_TXLog txlog = new TD_TXLog();
            txlog.ApplyTXDate = DateTime.Now;
            txlog.Code = GetGuid;
            txlog.Company = "0";
            txlog.CreatedBy = TModel.ID.ToString();
            txlog.CreatedTime = DateTime.Now;
            txlog.IsDeleted = false;
            txlog.MID = TModel.MID;
            txlog.Company = TModel.ID;
            txlog.Status = 1;

            txlog.TXBank = txway;
            if (txway == "1")
            {
                txlog.TXCard = Request.Form["uploadImg"];
                txlog.TXName = "支付宝";
            }
            else if (txway == "2")
            {
                txlog.TXName = "微信";
                txlog.TXCard = Request.Form["uploadImgWeixin"];
            }
            else if (txway == "3") //自行输入银行账号来提现
            {
                string txName = GetBankName(Request.Form["hidSelBank"]);
                string txBankNum = Request.Form["hidBankNum"];
                string txReceiveName = Request.Form["hidReceiveName"];
                if (!string.IsNullOrEmpty(Request.Form["hidSelBankCode"]))
                {
                    //银行code不为空，就是选择的历史的数据，取到这条历史数据
                    SH_MemberBank memBank = CommonBase.GetModel<SH_MemberBank>(Request.Form["hidSelBankCode"]);
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
                    memBank.Bank = Request.Form["hidSelBank"];
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
            }
            txlog.TXMoney = MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_TXMoney"], 0);
            txlog.FeeMoney = CacheService.GlobleConfig.TXFloat;
            txlog.RealMoney = txlog.TXMoney - txlog.FeeMoney;

            CommonBase.Insert<TD_TXLog>(txlog, listComm);

            //付款成功，给管理员或O单商发送消息
            string sendAdminRoleCode = "RoleCode='Manage'";
            List<Model.Member> receiveList = new List<Model.Member>();
            if (string.IsNullOrEmpty( TModel.Company ))
            {
                sendAdminRoleCode += " or RoleCode='Admin'";
            }
            receiveList = CommonBase.GetList<Model.Member>(sendAdminRoleCode);

            string message = "分销商：" + TModel.MID + "申请" + txlog.TXName + "提现" + txlog.TXMoney + "元";
            Service.MessageService.SendNewMessage(TModel, receiveList, message, listComm, "3");

            //扣除积分
            MemberService.UpdateMoney(listComm, txlog.Company.ToString(), txlog.TXMoney.ToString(), "MSH", false);
            if (CommonBase.RunListCommit(listComm))
            {
                TModel.MSH -= txlog.TXMoney;
                //发送提现短信
                //是否发送提现短信
                string isSendTXSMS = GlobleConfigService.GetWebConfig("IsSendTXSMS").Value;
                if (isSendTXSMS == "1")
                {
                    try
                    {
                        string sendTXContent = GlobleConfigService.GetWebConfig("SendTXSMSContent").Value;
                        string sendToTel = GlobleConfigService.GetWebConfig("SendTXSMSPhone").Value;
                        sendTXContent = sendTXContent.Replace("{{ApplyTXUser}}", "分销商：" + TModel.MID);//替换提线人
                        sendTXContent = sendTXContent.Replace("{{ApplyTXMoney}}", txlog.TXMoney.ToString());//替换提现金额
                        //发送短信
                        if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") != "kgjpersonaltest")
                        {
                            string SMSResult = Service.TelephoneCodeService.SendSMS(sendToTel, "SYSTEM", sendTXContent);
                        }
                    }
                    catch{

                    }
                }
                return "1";
            }
            return "0";
        }

        public string GetBankName(object bankCode)
        {
            string result = string.Empty;
            if (bankCode != null)
            {
                Sys_BankInfo bank = CacheService.BankList.FirstOrDefault(c => c.Code == bankCode.ToString());
                if (bank != null)
                    result = bank.Remark;
            }
            return result;
        }
    }
}