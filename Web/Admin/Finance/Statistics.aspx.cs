
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
    public partial class Statistics : BasePage
    {
        protected override void SetPowerZone()
        {
            nPayBeginTime.Value = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            nPayEndTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
            //未体现金额合计，不按时间段计算，显示的数据是从系统一开始到当天未体现金额合计。
            //统计会员表所有的MSH
            string sql = "SELECT SUM(MSH) FROM dbo.Member ";
            if (TModel.RoleCode == "Manage")
            {
                //sql += "  where Company=0 and ID not in (select ID from Member where RoleCode='Admin')";
            }
            else if (TModel.RoleCode == "Admin")
            {
                sql += "  where Company=" + TModel.ID.ToString();
            }
            else
            {
                sql += " where ID=" + TModel.ID.ToString();
            }

            object obj = CommonBase.GetSingle(sql);
            decimal a = 0;
            if (obj != null)
            {
                a = Convert.ToDecimal(obj);
            }
            spNoTXTotalMoney.InnerHtml = a.ToString("F2");

        }

        protected override string btnAdd_Click()
        {
            decimal memberTotalPayMoney = 0, applyTXByWeixin = 0, applyTXByAliPay = 0, applyTXByTPay = 0, applyTXByVPay = 0, hasTXByWeixin = 0, hasTXByAliPay = 0, hasTXByTPay = 0, hasTXByVPay = 0, hasTXByBankCard = 0, CompanyInMoney = 0, HasTXNoTranTotalMoney = 0;
            //注册会员返现金额合计/三级分销总金额/管理员或者代理商分红合计
            decimal registFHTotalMoney = 0, ThreeLeavelFHMoney = 0, AgentThreeLeavelFHMoney = 0, AgentFHMoney = 0, AgentTJFHMoney = 0, NoTxTotalMoney = 0;
            string beginTime = Request.Form["nPayBeginTime"];
            string endTime = Request.Form["nPayEndTime"];
            //2、会员付款总金额=所有付费会员缴费金额合计
            string payLogWhere = "IsDeleted=0 and Status=1 AND DATEDIFF(dd,'" + beginTime + "',PayTime)>=0   AND DATEDIFF(dd,'" + endTime + "',PayTime)<=0";
            if (TModel.RoleCode == "Admin")
            {
                payLogWhere += " and (PayID in (select ID from Member where Company=" + TModel.ID + ") or PayID='" + TModel.ID + "')";
            }
            List<TD_PayLog> listPayLog = CommonBase.GetList<TD_PayLog>(payLogWhere);

            memberTotalPayMoney = listPayLog.Sum(c => c.PayMoney);
            //3、提现总金额=所有会员提现金额合计
            //注：需要能查询阶段性收入，需要将提现方式归类：微信、支付宝
            string txLogWhere = "IsDeleted=0 AND DATEDIFF(dd,'" + beginTime + "',ApplyTXDate)>=0   AND DATEDIFF(dd,'" + endTime + "',ApplyTXDate)<=0";
            if (TModel.RoleCode == "Admin")
            {
                txLogWhere += " and (Company in (select ID from Member where Company=" + TModel.ID + ") or Company='" + TModel.ID + "')";
            }
            List<TD_TXLog> listTXLog = CommonBase.GetList<TD_TXLog>(txLogWhere);
            var applyTXList = listTXLog.Where(c => c.Status == 1);//提交申请的
            HasTXNoTranTotalMoney = applyTXList.Sum(c => c.TXMoney);
            applyTXByWeixin = applyTXList.Where(c => c.TXBank == "2").Sum(c => c.TXMoney);
            applyTXByAliPay = applyTXList.Where(c => c.TXBank == "1").Sum(c => c.TXMoney);

            applyTXByTPay = applyTXList.Where(c => c.TXBank == "4").Sum(c => c.TXMoney);

            applyTXByVPay = applyTXList.Where(c => c.TXBank == "5").Sum(c => c.TXMoney);



            var hasTXList = listTXLog.Where(c => c.Status == 2);//已经转账的
            decimal TXFeeTotalMoney = hasTXList.Sum(c => c.FeeMoney);
            hasTXByWeixin = hasTXList.Where(c => c.TXBank == "2").Sum(c => c.TXMoney);
            hasTXByAliPay = hasTXList.Where(c => c.TXBank == "1").Sum(c => c.TXMoney);
            hasTXByTPay = hasTXList.Where(c => c.TXBank == "4").Sum(c => c.TXMoney);
            hasTXByVPay = hasTXList.Where(c => c.TXBank == "5").Sum(c => c.TXMoney);
            hasTXByBankCard = hasTXList.Where(c => c.TXBank == "3").Sum(c => c.TXMoney);

            //公司收入=会员付款总金额—提现总金额
            CompanyInMoney = memberTotalPayMoney - hasTXByWeixin - hasTXByAliPay - hasTXByBankCard-hasTXByTPay-hasTXByVPay;

            string fhLogWhere = "IsDeleted=0 AND DATEDIFF(dd,'" + beginTime + "',FHDate)>=0   AND DATEDIFF(dd,'" + endTime + "',FHDate)<=0";

            List<TD_FHLog> listFHLog = CommonBase.GetList<TD_FHLog>(fhLogWhere);
            //未提现合计

            //三级分销总金额
            var fhTJList = listFHLog.Where(c => c.FHType == "14" || c.FHType == "16" || c.FHType == "17" || c.FHType == "95");
            ThreeLeavelFHMoney = fhTJList.Sum(c => c.FHMoney);
            AgentThreeLeavelFHMoney = fhTJList.Where(c => c.FHRoleCode == "2F" || c.FHRoleCode == "1F" || c.FHRoleCode == "3F").Sum(c => c.FHMoney);

            //返回值
            string returnJson = "{";
            returnJson += "\"FHTotalMoney\":\"" + listFHLog.Sum(c => c.FHMoney).ToString("F2") + "\",";//奖金指出合计
            returnJson += "\"ThreeLeavelTotalMoney1F\":\"" + listFHLog.Where(c => c.FHType == "17").Sum(c => c.FHMoney).ToString("F2") + "\",";//奖金指出合计
            returnJson += "\"ThreeLeavelTotalMoney2F\":\"" + listFHLog.Where(c => c.FHType == "95").Sum(c => c.FHMoney).ToString("F2") + "\",";//奖金指出合计
            returnJson += "\"ThreeLeavelTotalMoney\":\"" + listFHLog.Where(c => c.FHType == "16").Sum(c => c.FHMoney).ToString("F2") + "\",";//学员两级分销支出合计
            returnJson += "\"MemberTotalPayMoney\":\"" + memberTotalPayMoney + "\","; //会员付款总金额
            returnJson += "\"AgentThreeLeavelFHMoney\":\"" + AgentThreeLeavelFHMoney + "\","; //代理商分红
            AgentFHMoney = listFHLog.Where(c => c.SHMoneyCode == "Return").Sum(c => c.FHMoney);

            returnJson += "\"KCTrainingMoney\":\"" + listFHLog.Where(c => c.SHMoneyCode == "KgjUsing").Sum(c => c.FHMoney).ToString("F2") + "\",";//奖金指出合计

            returnJson += "\"KCOperateMoney\":\"" + listFHLog.Where(c => c.SHMoneyCode == "ServiceUsing").Sum(c => c.FHMoney).ToString("F2") + "\",";//奖金指出合计
            returnJson += "\"KCYunlianhuiMoney\":\"" + listFHLog.Where(c => c.SHMoneyCode == "CompanyUsing").Sum(c => c.FHMoney).ToString("F2") + "\",";//

            decimal AliPayFeeTotalMoney = memberTotalPayMoney * 0;// 0.0055M;
            returnJson += "\"AliPayFeeTotalMoney\":\"" + AliPayFeeTotalMoney.ToString("F2") + "\","; //支付宝手续费合计:手续费计算公式：会员付款总金额*0.55%
            //公司实际收入合计
            decimal citm = memberTotalPayMoney - AliPayFeeTotalMoney;
            returnJson += "\"CompanyIncomeTotalMoney\":\"" + citm.ToString("F2") + "\","; //公司实际收入合计:公司实际收入=会员付款总金额-支付宝手续费合计

            returnJson += "\"WeixinTXTotalMoney\":\"" + hasTXByWeixin + "\","; //微信提现合计（已提现）
            returnJson += "\"AliPayTXTotalMoney\":\"" + hasTXByAliPay + "\","; //支付宝提现合计（已提现）
            returnJson += "\"TPayTXTotalMoney\":\"" + hasTXByTPay + "\","; //支付宝提现合计（已提现）
            returnJson += "\"VPayTXTotalMoney\":\"" + hasTXByVPay + "\","; //支付宝提现合计（已提现）
            returnJson += "\"BankCardTXTotalMoney\":\"" + hasTXByBankCard + "\","; //支付宝提现合计（已提现）
            returnJson += "\"TXFeeTotalMoney\":\"" + TXFeeTotalMoney + "\","; //提现手续费合计:
            returnJson += "\"HasTXNoTranTotalMoney\":\"" + HasTXNoTranTotalMoney + "\","; //已提现未转账金额合计:
            returnJson += "\"TXTotalMoney\":\"" + (hasTXByWeixin + hasTXByAliPay + hasTXByBankCard) + "\","; //提现合计:
            //公司实际支出合计
            decimal cotm = hasTXByAliPay + hasTXByWeixin + hasTXByBankCard - TXFeeTotalMoney;
            returnJson += "\"CompanyOutTotalMoney\":\"" + cotm.ToString("F2") + "\","; //公司实际支出合计:实际支出合计=提现合计-提现手续费合计
            returnJson += "\"NoTXTotalMoney\":\"" + NoTxTotalMoney + "\","; //未提现总金额合计:系统所有会员和代理未提现部分金额合计（贵州管理员+admin推荐的代理）
            decimal CompanyUseMoney = (memberTotalPayMoney - AliPayFeeTotalMoney) - (hasTXByAliPay + hasTXByWeixin + hasTXByBankCard - TXFeeTotalMoney) - NoTxTotalMoney;
            //公司可支配金额=所查时间段实际总收入-(所查时间段的总分红)
            CompanyUseMoney = citm - listFHLog.Sum(c => c.FHMoney);
            returnJson += "\"CompanyUseMoney\":\"" + CompanyUseMoney.ToString("F2") + "\","; //公司可支配金额合计:公司可支配金额合计=公司实际收入合计-公司实际支出合计-未提现总金额合计

            //参考统计数据
            returnJson += "\"ThreeLeavelIncomeTotalMoney\":\"" + ThreeLeavelFHMoney + "\","; //三级分销收益合计:(只统计个人的)
            returnJson += "\"AgentFHTotalMoney\":\"" + AgentFHMoney + "\","; //代理商服务补贴合计:
            returnJson += "\"AgentTJTotalMoney\":\"" + AgentTJFHMoney + "\","; //代理商推荐人推荐奖励合计:

            returnJson += "\"1\":\"0\"}";
            return returnJson;
        }
    }
}