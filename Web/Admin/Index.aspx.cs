using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MethodHelper;
using Model;
using System.IO;
using System.Text.RegularExpressions;
using Service;
using DBUtility;

namespace Web.Admin
{
    public partial class Index : BasePage
    {
        public List<Sys_Privage> listFirstPowers = new List<Sys_Privage>();
        List<Sys_RolePower> listMenuPowers = new List<Sys_RolePower>();

        protected string IsShowBaseInfo = "0";
        protected override void SetPowerZone()
        {

            if (TModel == null)
            {
                Response.Redirect("/Admin/Login.aspx");
                return;
            }
            //MethodHelper.LogHelper.WriteTextLog(typeof(Index).ToString(), TModel.MID+"登录系统", DateTime.Now);
            //登陆过就不再显示
            //List<DB_Log> listlog = CommonBase.GetList<DB_Log>("IsDeleted=0 and LType='1' and  MID=" + TModel.ID.ToString());
            //if (listlog != null)
            //{
            //    if (listlog.Count == 1)
            //    {
            //        DB_Log log = listlog.FirstOrDefault();
            //        if (log != null && log.Status == 0)
            //            IsShowBaseInfo = "1";
            //    }
            //}
            //listMenuPowers = CommonBase.GetList<Sys_RolePower>("PrivageType=1 and RoleCode='" + TModel.RoleCode + "'");// CacheService.RolePowerList.Where(c => c.RoleCode == TModel.RoleCode && c.PrivageType == 1).ToList();
            listMenuPowers = TModel.PowerList.Where(emp => !emp.IsDeleted && emp.PrivageType == 1).ToList();

            foreach (Sys_RolePower rp in listMenuPowers)
            {
                Sys_Privage obj = CacheService.PrivageList.Where(c => c.Id == rp.PrivageId && c.ParentCode == "0" && c.IsDeleted == false).FirstOrDefault();
                if (obj != null)
                    listFirstPowers.Add(obj);
            }
            listFirstPowers = listFirstPowers.OrderBy(c => c.MenuIndex).ToList();
        }


        protected IEnumerable<Model.Sys_Privage> GetPowers(string cfid)
        {
            //List<Sys_Privage> listSecond = new List<Sys_Privage>();
            var list = from menu in CacheService.PrivageList
                       join srp in listMenuPowers on menu.Id equals srp.PrivageId
                       where menu.ParentCode == cfid
                       orderby menu.MenuIndex
                       select menu;
            return list;
        }

        protected void SetDefaultVal()
        {

        }

        protected string RemoveContent(string orginContent)
        {
            string temp = NoHTML(orginContent);
            return temp;
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Htmlstring.Replace("\r\n", "");  //去除换行
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            //去除script标签
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //去除style标签
            Htmlstring = Regex.Replace(Htmlstring, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);
            //string pararn = @"<ul class='nav' id='nav'>]*?>.*?</ul>";
            //Htmlstring = Regex.Replace(Htmlstring, pararn, "", RegexOptions.IgnoreCase);
            ////删除HTML
            //Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            //Htmlstring.Replace("<", "");
            //Htmlstring.Replace(">", "");
            //Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        /// <summary>
        /// 取文本中间到List集合
        /// </summary>
        /// <param name="str">文本字符串</param>
        /// <param name="leftstr">左边文本</param>
        /// <param name="rightstr">右边文本</param>
        /// <returns>List集合</returns>
        public List<string> BetweenArr(string str, string leftstr, string rightstr)
        {
            List<string> list = new List<string>();
            int leftIndex = str.IndexOf(leftstr);//左文本起始位置
            int leftlength = leftstr.Length;//左文本长度
            int rightIndex = 0;
            string temp = "";
            while (leftIndex != -1)
            {
                rightIndex = str.IndexOf(rightstr, leftIndex + leftlength);
                if (rightIndex == -1)
                {
                    break;
                }
                temp = str.Substring(leftIndex + leftlength, rightIndex - leftIndex - leftlength);
                temp = temp.Replace("\b", "").Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("\r\n", "");
                if (!string.IsNullOrEmpty(temp.Trim()))
                    list.Add(temp);
                leftIndex = str.IndexOf(leftstr, rightIndex + 1);
            }
            return list;
        }


        protected override string btnAdd_Click()
        {
            string type = Request.Form["type"];
            if (type == "1") //缴费测试分红
            {
                #region 手动给会员缴费分红
                List<CommonObject> listComm = new List<CommonObject>();
                string mid = Request.Form["mid"];
                Model.Member member = CommonBase.GetList<Model.Member>("MID='" + mid + "'").FirstOrDefault();
                if (member == null)
                {
                    return "不存在该缴费会员";
                }
                string trade_no = MethodHelper.CommonHelper.CreateNo(), out_trade_no = MethodHelper.CommonHelper.CreateNo(), total_fee = Service.CacheService.GlobleConfig.Field1;
                string noticeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                PayService.PaySuccess(trade_no, out_trade_no, member, total_fee, noticeTime, "admin手动操作缴费升级", 1, member.ID.ToString(), "applyvip", listComm);
                decimal money = decimal.Parse(CacheService.GlobleConfig.Field1);
                //备份数据库
                string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                string sysId = ConfigHelper.GetAppSettings("SystemID");
                //CommonBase.BackUpDataBase(sysId, MethodHelper.ConfigHelper.GetAppSettings("DataBaseBackUpURL"), sysId + dateStr);
                LogService.Log(TModel, "20", TModel.MID + "在" + dateStr + "手动操作" + member.MID + "缴费" + money + "元，进行手动分红，分红之前备份了数据库", listComm);
                if (CommonBase.RunListCommit(listComm))
                {
                    return "1";
                }
                #endregion
            }
            else if (type == "4") //会员升级分销商
            {
                string addOutNoString = Request.Form["ddlagentLeavl"];
                string mid = Request.Form["mid"];
                decimal paymoney = 0;

                #region 校验是否能升级分销商并确定缴费金额
                Model.Member member;

                //判断会员是不是已经是分销商
                //判断是不是已经交过费，是不是已经是缴费会员了
                List<Model.Member> upListMember = CommonBase.GetList<Model.Member>("MID LIKE '" + mid + "%'");
                member = upListMember.FirstOrDefault(c => c.MID == mid);
                if (member == null)
                {
                    return "不存在该会员";
                }
                Model.Member upMember = null;
                foreach (Model.Member mem in upListMember)
                {
                    if (mem.RoleCode != "VIP" && mem.RoleCode != "Member")
                    {
                        upMember = mem;
                    }
                }
                if (upMember != null)
                {
                    if (addOutNoString == "applyagent2f")
                    {
                        decimal vipMoney = ConvertHelper.ToDecimal(Service.CacheService.GlobleConfig.Field1, 0);
                        decimal applyAgentMoney = ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent2Money").Value, 0);
                        decimal applyLeaveMoney = applyAgentMoney;
                        if (member.RoleCode == "VIP")
                        {
                            paymoney = applyAgentMoney - vipMoney;
                        }
                    }
                    //如果是从分销商升级成服务中心，，查询出要不教多少钱
                    if (addOutNoString == "applyagent2f" && ConvertHelper.ToInt32(upMember.Role.AreaLeave, 0) == 40)
                    {
                        decimal applyAgent3Money = ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent3Money").Value, 0);
                        decimal applyAgent2Money = ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent2Money").Value, 0);
                        if (applyAgent2Money >= applyAgent3Money)
                            paymoney = applyAgent2Money - applyAgent3Money;
                    }
                    if (addOutNoString == "applyagent3f" && ConvertHelper.ToInt32(upMember.Role.AreaLeave, 0) <= 30) //已经是二级分销商，不允许再申请三级分销商
                    {
                        return "该会员已是服务中心，请勿重复缴费。";
                    }
                    else
                    {
                        if (addOutNoString == "applyagent2f" && ConvertHelper.ToInt32(upMember.Role.AreaLeave, 0) <= 20) //已经是一级分销商，不允许再申请二级级分销商
                        {
                            return "'该会员已是一级分销商，请勿重复缴费。";
                        }
                    }

                }
                else
                {
                    if (addOutNoString == "applyagent2f")
                    {
                        decimal vipMoney = ConvertHelper.ToDecimal(Service.CacheService.GlobleConfig.Field1, 0);
                        decimal applyAgentMoney = ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent2Money").Value, 0);
                        decimal applyLeaveMoney = applyAgentMoney;
                        if (member.RoleCode == "VIP")
                        {
                            paymoney = applyAgentMoney - vipMoney;
                        }
                        else
                        {
                            paymoney = applyAgentMoney;
                        }
                    }
                    else if (addOutNoString == "applyagent3f")
                    {
                        decimal vipMoney = ConvertHelper.ToDecimal(Service.CacheService.GlobleConfig.Field1, 0);
                        decimal applyAgentMoney = ConvertHelper.ToDecimal(Service.GlobleConfigService.GetWebConfig("ApplyAgent3Money").Value, 0);
                        decimal applyLeaveMoney = applyAgentMoney;
                        if (member.RoleCode == "VIP")
                        {
                            paymoney = applyAgentMoney - vipMoney;
                        }
                        else
                        {
                            paymoney = applyAgentMoney;
                        }
                    }
                }
                #endregion

                List<CommonObject> listComm = new List<CommonObject>();

                string trade_no = MethodHelper.CommonHelper.CreateNo(), out_trade_no = MethodHelper.CommonHelper.CreateNo(), total_fee = paymoney.ToString();
                string noticeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                PayService.PaySuccess(trade_no, out_trade_no, member, total_fee, noticeTime, "admin手动操作升级分销商", 1, member.ID.ToString(), addOutNoString, listComm);
                decimal money = paymoney;
                //备份数据库
                string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                string sysId = ConfigHelper.GetAppSettings("SystemID");// Service.GlobleConfigService.GetWebConfig("SystemID").Value;
                //CommonBase.BackUpDataBase(sysId, MethodHelper.ConfigHelper.GetAppSettings("DataBaseBackUpURL"), sysId + dateStr);
                LogService.Log(TModel, "20", TModel.MID + "在" + dateStr + "手动操作" + member.MID + "缴费" + money + "元，升级分销商，升级之前备份了数据库", listComm);
                if (CommonBase.RunListCommit(listComm))
                {
                    return "1";
                }
            }
            return "0";
        }
    }
}