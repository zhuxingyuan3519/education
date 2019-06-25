
using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Agent
{
    public partial class AgentEdit : BasePage
    {
        protected string HireStatusString = string.Empty;
        protected bool IsShowHire = false;
        protected override void SetPowerZone()
        {
            txt_MTJ.Value = TModel.MID;

            ddlRoles.Items.Clear();
            ddlRoles.DataSource = CacheService.RoleList.Where(c => c.IsDeleted == false && (c.Code == "1F" || c.Code == "2F" || c.Code == "3F")).OrderBy(c => c.RIndex);
            ddlRoles.DataTextField = "Name";
            ddlRoles.DataValueField = "Code";
            ddlRoles.DataBind();

            System.Web.UI.HtmlControls.HtmlSelect selectControl = ddl_Province;// this.Page.FindControl(NameHeader + ddl_Province.ClientID) as System.Web.UI.HtmlControls.HtmlSelect;
            if (selectControl != null)
            {
                selectControl.DataSource = CacheService.SatandardAddressList.Where(c => c.LevelInt == 20);// CacheService.AddressList.Where(c => c.Level == 20);
                selectControl.DataTextField = "Name";
                selectControl.DataValueField = "AdCode";
                selectControl.DataBind();
                selectControl.Items.Insert(0, new ListItem("--请选择--", ""));
            }
            string code = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(code))
            {
                //如果是修改/查看

                Model.Member model = CommonBase.GetModel<Model.Member>(code);
                if (model != null)
                {
                    string checkMTJ = "select MID from Member where ID='" + model.MTJ + "'";
                    object obj = CommonBase.GetSingle(checkMTJ);
                    if (obj != null)
                    {
                        txt_MTJ.Value = obj.ToString();
                        txt_MTJ.Attributes.Add("readonly", "readonly");
                    }
                    txt_Branch.Value = model.Branch;
                    txt_Name.Value = model.MName;
                    txt_PointMoney.Value = model.PointMoney.ToString();
                    txtPhone.Value = model.Tel;
                    txt_QQ.Value = model.QQ;
                    txtPwd.Value = MethodHelper.CommonHelper.DESDecrypt(model.Password);
                    txtPwd2.Value = txtPwd.Value;
                    txt_WeiXin.Value = model.Weichat;
                    //ddlStatus.Value = model.Status.ToString();
                    hidId.Value = model.ID.ToString();
                    txt_MID.Value = model.MID;
                    ddl_Province.Value = model.Province;
                    txt_PointMoney.Value = model.PointMoney.ToString();
                    txt_LeaveTradePoints.Value = model.LeaveTradePoints.ToString();
                    ddlRoles.Value = model.RoleCode;
                    txt_address.Value = model.Address;
                    if (!string.IsNullOrEmpty(model.City))
                    {
                        //绑定市
                        ddl_City.DataSource = CacheService.SatandardAddressList.Where(c => c.ProCode.Trim() == model.Province);
                        ddl_City.DataTextField = "Name";
                        ddl_City.DataValueField = "AdCode";
                        ddl_City.DataBind();
                        ddl_City.Items.Insert(0, new ListItem("--请选择--", ""));
                        ddl_City.Value = model.City;
                    }

                    if (!string.IsNullOrEmpty(model.Zone))
                    {
                        //绑定区县
                        ddl_Zone.DataSource = CacheService.SatandardAddressList.Where(c => c.CityCode.Trim() == model.City);
                        ddl_Zone.DataTextField = "Name";
                        ddl_Zone.DataValueField = "AdCode";
                        ddl_Zone.DataBind();
                        ddl_Zone.Items.Insert(0, new ListItem("--请选择--", ""));
                        ddl_Zone.Value = model.Zone;
                    }
                }
            }
        }
        protected Model.Member GetModel(Model.Member model)
        {
            model.Branch = Request.Form[txt_Branch.ClientID];
            model.MName = Request.Form["txt_Name"];
            model.Tel = Request.Form["txtPhone"];
            model.Weichat = Request.Form["txt_WeiXin"];
            model.UseRoleType = 1;
            model.Province = Request.Form["ddl_Province"];
            model.Address = Request.Form["txt_address"];
            model.City = Request.Form["ddl_City"];
            model.Zone = Request.Form["ddl_Zone"];
            model.MID = Request.Form["txt_MID"];
            model.QQ = Request.Form["txt_QQ"];
            model.PointMoney = MethodHelper.ConvertHelper.ToDecimal(Request.Form[txt_PointMoney.ClientID], 0);
            //密码都用DES加密，为了后台解密
            model.Password = CommonHelper.DESEncrypt(Request.Form["txtPwd2"]);
            ////查询推荐人是不是存在
            string checkMTJ = "select ID from Member where MID='" + Request.Form["txt_MTJ"] + "'";
            object obj = CommonBase.GetSingle(checkMTJ);
            if (obj == null)
            {
                model.MTJ = "";
            }
            else
            {
                model.MTJ = obj.ToString();
            }
            return model;
        }
        protected override string btnAdd_Click()
        {
            try
            {
                List<CommonObject> listComm = new List<CommonObject>();
                Model.Member model = new Model.Member();
                if (!string.IsNullOrEmpty(Request["hidId"]))
                {  //修改
                    #region 修改操作
                    model = CommonBase.GetModel<Model.Member>(Request["hidId"]);
                    if (model != null)
                    {
                        int oldTradePoints = model.TradePoints;

                        int oldLeaveTradePoints = model.LeaveTradePoints;
                        model.LeaveTradePoints = int.Parse(Request.Form[txt_LeaveTradePoints.ClientID]);

                        model = GetModel(model);
                        if (string.IsNullOrEmpty(model.MTJ))
                            return CommonHelper.Response(false, "修改失败，不存在推荐人！");
                        //上级代理商
                        bool isLessTradePoints = false, isAddTradePoints = false;
                        int LessTradePoints = 0, AddTradePoints = 0;
                        if (oldLeaveTradePoints > model.LeaveTradePoints)
                        {
                            //收费端口减少
                            isLessTradePoints = true;
                            LessTradePoints = oldLeaveTradePoints - model.LeaveTradePoints;
                            model.TradePoints = model.TradePoints - LessTradePoints;

                        }
                        else if (oldLeaveTradePoints < model.LeaveTradePoints)
                        {
                            //增加
                            isAddTradePoints = true;
                            AddTradePoints = model.LeaveTradePoints - oldLeaveTradePoints;
                            model.TradePoints = model.TradePoints + AddTradePoints;
                        }
                        if (isAddTradePoints)
                        {
                            Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                            cost.Code = MethodHelper.CommonHelper.GetGuid;
                            cost.CompanyCode = model.ID;
                            cost.CostCount = AddTradePoints;
                            cost.CreatedBy = "system";
                            cost.CreatedTime = DateTime.Now;
                            cost.IsDeleted = false;
                            cost.FromCompany = "18";
                            cost.ToCompany = model.ID;
                            cost.MID = model.MID;
                            cost.Status = 2;
                            cost.Remark = "管理员调整VIP名额数量；增加配送" + cost.CostCount + "个VIP名额";
                            CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                            LogService.Log(TModel, "10", TModel.MID + "操作" + model.MID + "收费端口增加" + AddTradePoints + "，现有端口数量：" + model.LeaveTradePoints, listComm);
                        }

                        if (isLessTradePoints)
                        {
                            Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                            cost.Code = MethodHelper.CommonHelper.GetGuid;
                            cost.CompanyCode = model.ID;
                            cost.CostCount = LessTradePoints;
                            cost.CreatedBy = "system";
                            cost.CreatedTime = DateTime.Now;
                            cost.IsDeleted = false;
                            cost.FromCompany = model.ID;
                            cost.ToCompany = "18";
                            cost.MID = model.MID;
                            cost.Status = 2;
                            cost.Remark = "管理员调整VIP名额数量；减少配送" + cost.CostCount + "个VIP名额";
                            CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                            LogService.Log(TModel, "10", TModel.MID + "操作" + model.MID + "收费端口减少" + LessTradePoints + "，现有端口数量" + model.LeaveTradePoints, listComm);
                        }

                        string checkSql = "select count(1) from Member where  (MID='" + model.MID + "' OR Branch='" + model.Branch + "') and ID<>'" + model.ID + "'";
                        int resu = Convert.ToInt16(CommonBase.GetSingle(checkSql));
                        if (resu > 0)
                            return CommonHelper.Response(false, "修改失败，已存在该登录账号或机构代码！");
                        CommonBase.Update<Model.Member>(model, listComm);
                    }
                    #endregion
                }
                else
                {
                    #region 新增操作
                    model = new Model.Member();
                    model = GetModel(model);
                    model.LeaveTradePoints = int.Parse(Request.Form[txt_LeaveTradePoints.ClientID]);
                    model.TradePoints = model.LeaveTradePoints;
                    model.ID = GetGuid;

                    //查询推荐人是不是存在
                    if (string.IsNullOrEmpty(model.MTJ))
                        return CommonHelper.Response(false, "添加失败，不存在该推荐人！");
                    string checkSql = "select count(1) from Member where MID='" + model.MID + "' OR Branch='" + model.Branch + "' where RoleCode in ('1F','2F','3F','Manage')";
                    int resu = Convert.ToInt16(CommonBase.GetSingle(checkSql));
                    if (resu > 0)
                        return CommonHelper.Response(false, "添加失败，已存在该登录账号或机构代码！");
                    model.RoleCode = Request.Form["ddlRoles"];
                    model.MCreateDate = DateTime.Now;
                    model.Company = "18";
                    model.MState = true;

                    Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                    cost.Code = MethodHelper.CommonHelper.GetGuid;
                    cost.CompanyCode = model.ID;
                    cost.CostCount = model.LeaveTradePoints;
                    cost.CreatedBy = "system";
                    cost.CreatedTime = DateTime.Now;
                    cost.IsDeleted = false;
                    cost.FromCompany = "18";
                    cost.ToCompany = model.ID;
                    cost.MID = model.MID;
                    cost.Status = 2;
                    cost.Remark = "管理员创建用户" + model.MID + "，配送" + cost.CostCount + "个VIP名额";
                    CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                    //查看管理员使用的分销类型
                    //model.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("123456", "MD5").ToUpper();
                    CommonBase.Insert<Model.Member>(model, listComm);
                    LogService.Log(TModel, "4", TModel.MID + "创建角色为" + CacheService.RoleList.Where(c => c.Code == model.RoleCode).FirstOrDefault().Name + "的账号" + model.MID + ",推荐人为:" + Request.Form["txt_MTJ"], listComm);
                    #endregion
                }
                if (CommonBase.RunListCommit(listComm))
                {
                    return CommonHelper.Response(true, model.ID);
                }
                else
                {
                    return CommonHelper.Response(false, "操作失败，请重试！");
                }
            }
            catch (Exception e)
            {
                return CommonHelper.Response(false, "操作失败：" + e.Message);
            }
            return CommonHelper.Response(false, "操作失败，请重试！");
        }
    }
}
