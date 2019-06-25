using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;

namespace Web.Admin.Member
{
    public partial class SignUp: BasePage
    {
        protected override void SetPowerZone()
        {
            //绑定课程
            List<Model.Sys_Role> list = CacheService.RoleList.Where(c => c.IsDeleted == false && c.IsAdmin == false).OrderBy(c => c.RIndex).ToList();// CommonBase.GetList<Model.Sys_Role>("IsDeleted=0 and IsAdmin=0 order by RIndex asc");
            foreach(Model.Sys_Role course in list)
            {
                ListItem li = new ListItem();
                li.Text = course.Name + "（" + course.Remark + "元，配送" + course.AreaLeave + "个名额）";
                li.Value = course.Code;
                li.Attributes.Add("point", course.AreaLeave);
                ddlCourse.Items.Add(li);
            }
        }
        protected override string btnAdd_Click()
        {
            try
            {
                Model.Member member = CommonBase.GetModel<Model.Member>(Request.Form["chkMemberCode"]);
                if(member == null || member.IsClose)
                {
                    return MethodHelper.CommonHelper.Response(false, "缴费会员不存在");
                }
                string addType = Request.QueryString["addtype"];
                string isMemberFh = Request.Form["chk_memberFH"];
                decimal addmoney = MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_money"], 0);
                string payRemark = string.Empty;
                int point = MethodHelper.ConvertHelper.ToInt32(Request.Form["txtPoint"], 0);

                if(addType == "0")
                {
                    #region 用户升级
                    //获取到升级的级别
                    Sys_Role role = CacheService.RoleList.FirstOrDefault(c => c.Code == Request.Form["ddlCourse"]);
                    if(role == null)
                        return MethodHelper.CommonHelper.Response(false, "要升的等级不存在");
                    if(role.Code == "Student")  //如果是学员，就单独处理
                    //if(false)
                    {
                        #region   2、缴费表中插入一条数据
                        Model.TD_PayLog payModelTemp = new Model.TD_PayLog();
                        payModelTemp.Code = MethodHelper.CommonHelper.GetGuid;
                        payModelTemp.PayType = Request.Form["ddlPayType"];
                        payModelTemp.PayWay = Request.Form["ddlPayType"];
                        payModelTemp.ProductCode = MethodHelper.CommonHelper.CreateNo();
                        payModelTemp.Company = 0;
                        payModelTemp.CreatedBy = member.MID;
                        payModelTemp.CreatedTime = DateTime.Now;
                        payModelTemp.IsDeleted = false;
                        payModelTemp.PayForMID = "admin";
                        payModelTemp.PayMID = member.MID;
                        payModelTemp.PayMoney = MethodHelper.ConvertHelper.ToDecimal(role.Remark, 0);
                        payModelTemp.PayTime = DateTime.Now;
                        payModelTemp.Status = 0;
                        payModelTemp.PayID = member.ID;
                        payModelTemp.Remark = "缴费成为" + role.Name;
                        #endregion
                        if(CommonBase.Insert<Model.TD_PayLog>(payModelTemp))
                        {
                            if(SignUpService.SignUpNoMRank(payModelTemp, role, member, TModel, "后台缴费升级"))
                            {
                                return MethodHelper.CommonHelper.Response(true, "缴费升级成功");
                            }
                            return MethodHelper.CommonHelper.Response(false, "缴费升级失败，请重试");
                        }
                        else
                        {
                            return MethodHelper.CommonHelper.Response(false, "缴费升级失败，请重试");
                        }
                    }
                    else
                    {
                        //配送名额数量
                        member.RoleCode = role.Code;
                        addmoney = MethodHelper.ConvertHelper.ToDecimal(role.Remark, 0);
                        payRemark = Request.Form["ddlPayType"] + "缴费" + addmoney + "升级到" + role.Name;
                    }
                    #endregion
                }
                else
                {
                    payRemark = Request.Form["ddlPayType"] + "缴费" + addmoney + "购买" + point + "名额";
                }

                List<CommonObject> listComm = new List<CommonObject>();
                //会员级别改变
                member.TradePoints += point;
                member.LeaveTradePoints += point;
                member.YunPay = Request.Form["ddlPayType"];
                CommonBase.Update<Model.Member>(member, new string[] { "RoleCode", "TradePoints", "LeaveTradePoints", "YunPay" }, listComm);
                //SHMoneyService.TJChangeMoney(member, MethodHelper.ConvertHelper.ToDecimal(role.Remark, 0), listComm);


                #region   2、缴费表中插入一条数据
                Model.TD_PayLog payModel = new Model.TD_PayLog();
                payModel.Code = MethodHelper.CommonHelper.GetGuid;//跟报名的code保持一致
                payModel.PayType = member.YunPay;
                payModel.PayWay = member.YunPay;
                payModel.ProductCode = MethodHelper.CommonHelper.CreateNo();
                payModel.Company = 0;
                payModel.CreatedBy = TModel.MID;
                payModel.CreatedTime = DateTime.Now;
                payModel.IsDeleted = false;
                payModel.PayForMID = "0";
                payModel.PayMID = member.MID;
                payModel.PayMoney = addmoney;
                payModel.PayTime = DateTime.Now;
                payModel.Status = 1;
                payModel.PayID = member.ID;
                payModel.Remark = payRemark;
                CommonBase.Insert<Model.TD_PayLog>(payModel, listComm);
                #endregion

                //获取到归属机构
                Model.Member companyModel = CommonBase.GetModel<Model.Member>(member.Company);
                if(companyModel != null && !companyModel.Role.IsAdmin)
                {
                    //扣除相应的名额数量
                    if(companyModel.LeaveTradePoints >= MethodHelper.ConvertHelper.ToInt32(member.Role.AreaLeave, 0))
                    {
                        #region 扣除相应名额

                        companyModel.LeaveTradePoints = companyModel.LeaveTradePoints - MethodHelper.ConvertHelper.ToInt32(member.Role.AreaLeave, 0);
                        CommonBase.Update<Model.Member>(companyModel, new string[] { "LeaveTradePoints" }, listComm);

                        Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                        cost.Code = MethodHelper.CommonHelper.GetGuid;
                        cost.CompanyCode = companyModel.ID;
                        cost.CostCount = MethodHelper.ConvertHelper.ToInt32(member.Role.AreaLeave, 0);
                        cost.CreatedBy = "system";
                        cost.CreatedTime = DateTime.Now;
                        cost.IsDeleted = false;
                        cost.FromCompany = companyModel.ID;
                        cost.ToCompany = member.ID;
                        cost.MID = companyModel.MID;
                        cost.Status = 2;
                        if(addType == "0")
                        {
                            cost.Remark = "后台为用户" + member.MName + "缴费升级；配送" + cost.CostCount + "个VIP名额";
                        }
                        else
                        {
                            cost.Remark = "后台为用户" + member.MName + "缴费，购买" + cost.CostCount + "个VIP名额";
                        }
                        CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                        #endregion
                    }
                }
                else
                {
                    //名额用完了就只给直接推荐奖励
                    decimal fhBili = MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_bili"], 0);
                    SHMoneyService.TJChangeMoney(member, addmoney, listComm, addType, point, isMemberFh, fhBili);
                }
                if(CommonBase.RunListCommit(listComm))
                {
                    return MethodHelper.CommonHelper.Response(true, "缴费成功");
                }
                return MethodHelper.CommonHelper.Response(false, "缴费失败，请重试");
            }
            catch(Exception ex)
            {
                return MethodHelper.CommonHelper.Response(false, "报名失败：" + ex.ToString());
            }
        }



    }
}