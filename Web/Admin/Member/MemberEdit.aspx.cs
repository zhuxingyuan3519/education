
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

namespace Web.Admin.Member
{
    public partial class MemberEdit: BasePage
    {
        protected bool isService = false;

        protected string GetTrainCheckStatus(string code)
        {
            string result = "";
            Model.Member member = (Model.Member)ViewState["TempTModel"];
            if(member != null)
            {
                if(!string.IsNullOrEmpty(member.Learns) && member.Learns.Contains(code))
                {
                    result = " checked='checked'";
                }
            }
            return result;
        }
        protected override void SetPowerZone()
        {

            BindAddress();//code为空的话表示新建，初始化地址

            ddlType.Items.Clear();
            ddlType.DataSource = CacheService.RoleList.Where(c => c.IsDeleted == false && (c.Code == "Member" || c.Code == "Teacher" || c.Code == "VIP")).OrderBy(c => c.RIndex);
            ddlType.DataTextField = "Name";
            ddlType.DataValueField = "Code";
            ddlType.DataBind();

        }
        protected void BindAddress()
        {
            System.Web.UI.HtmlControls.HtmlSelect selectControl = ddl_Province;// this.Page.FindControl(NameHeader + ddl_Province.ClientID) as System.Web.UI.HtmlControls.HtmlSelect;
            if(selectControl != null)
            {
                selectControl.DataSource = CacheService.SatandardAddressList.Where(c => c.LevelInt == 20);// CacheService.AddressList.Where(c => c.Level == 20);
                selectControl.DataTextField = "Name";
                selectControl.DataValueField = "AdCode";
                selectControl.DataBind();
                selectControl.Items.Insert(0, new ListItem("--请选择--", ""));
            }

        }

        protected override void SetValue(string code)
        {
            txt_MTJ.Value = TModel.MID;
            //txt_UseBeginTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
            //txt_UseEndTime.Value = DateTime.Now.AddDays(MethodHelper.ConvertHelper.ToInt32(CacheService.GlobleConfig.Field2, 1)).ToString("yyyy-MM-dd");

            Model.Member model = CommonBase.GetModel<Model.Member>(code);
            if(model != null)
            {
                ViewState["TempTModel"] = model;
                isService = (model.Learns == "service");
                txt_WelfareID.Value = model.WelfareID;
                if(IsHasPower("CKHYMM"))
                {
                    txtPwd.Value = CommonHelper.DESDecrypt(model.Password);
                }
                txt_Email.Value = model.Email;
                txt_Name.Value = model.MName;
                txt_Tel.Value = model.Tel;
                txt_QQ.Value = model.QQ;
                txt_MID.Value = model.MID;
                txt_WeiXin.Value = model.Weichat;
                txt_Address.Value = model.Address;
                hidId.Value = model.ID.ToString();
                ddlType.Value = model.RoleCode;
                txt_UseBeginTime.Value = model.UseBeginTime == null ? "" : Convert.ToDateTime(model.UseBeginTime).ToString("yyyy-MM-dd");
                txt_UseEndTime.Value = model.UseEndTime == null ? "" : Convert.ToDateTime(model.UseEndTime).ToString("yyyy-MM-dd");
                ddlStatus.Value = model.MState ? "1" : "0";
                txtPoint.Value = model.TradePoints.ToString();
                ddlPayType.Value = model.YunPay;

                ddl_ReadNoticeId.Value = model.ReadNoticeId.ToString();
                if(string.IsNullOrEmpty(model.NoFHPool) || model.NoFHPool == "1")
                    ddl_tjStatus.Value = "1";
                //查询推荐人是不是存在
                string checkMTJ = "select MID from Member where ID='" + model.MTJ + "'";
                object obj = CommonBase.GetSingle(checkMTJ);
                if(obj == null)
                {
                }
                else
                {
                    txt_MTJ.Value = obj.ToString();
                }

                checkMTJ = "select MID from Member where ID='" + model.ParentTrade + "'";
                obj = CommonBase.GetSingle(checkMTJ);
                if(obj == null)
                {
                }
                else
                {
                    txt_TeacherMID.Value = obj.ToString();
                }



                string checkCompany = "select MID from Member where ID='" + model.Company + "'";
                object objCompany = CommonBase.GetSingle(checkCompany);
                if(objCompany == null)
                {
                }
                else
                {
                    txt_Company.Value = objCompany.ToString();
                }

                //省、市、县三级初始化
                if(!string.IsNullOrEmpty(model.Zone))
                {
                    ddl_Province.Value = model.Province;
                    ddl_City.DataSource = CacheService.SatandardAddressList.Where(c => c.ProCode.Trim() == model.Province);
                    ddl_City.DataTextField = "Name";
                    ddl_City.DataValueField = "AdCode";
                    ddl_City.DataBind();
                    ddl_City.Value = model.City;

                    ddl_Zone.DataSource = CacheService.SatandardAddressList.Where(c => c.CityCode.Trim() == model.City);
                    ddl_Zone.DataTextField = "Name";
                    ddl_Zone.DataValueField = "AdCode";
                    ddl_Zone.DataBind();
                    ddl_Zone.Items.Insert(0, new ListItem("--请选择--", ""));
                    ddl_Zone.Value = model.Zone;
                }
                else if(!string.IsNullOrEmpty(model.City))
                {
                    ddl_Province.Value = model.Province;
                    ddl_City.DataSource = CacheService.SatandardAddressList.Where(c => c.ProCode.Trim() == model.Province && c.LevelInt == 30);
                    ddl_City.DataTextField = "Name";
                    ddl_City.DataValueField = "AdCode";
                    ddl_City.DataBind();
                    ddl_City.Items.Insert(0, new ListItem("--请选择--", ""));
                    ddl_City.Value = model.City;
                }
                else if(!string.IsNullOrEmpty(model.Province))
                {
                    ddl_Province.Value = model.Province;
                }
            }

        }

        protected Model.Member GetModel(Model.Member model)
        {
            model.Email = Request.Form["txt_Email"];
            model.MName = Request.Form["txt_Name"];
            model.Tel = Request.Form["txt_Tel"];
            model.Weichat = Request.Form["txt_WeiXin"];
            model.MState = Request.Form["ddlStatus"] == "1";
            model.MID = Request.Form["txt_MID"];
            model.QQ = Request.Form["txt_QQ"];
            //model.UseBeginTime = Convert.ToDateTime(Request.Form["txt_UseBeginTime"]);
            //model.UseEndTime = Convert.ToDateTime(Request.Form["txt_UseEndTime"]);
            model.Address = Request.Form["txt_Address"];
            model.Province = Request.Form["ddl_Province"];
            model.City = Request.Form["ddl_City"];
            model.Zone = Request.Form["ddl_Zone"];
            model.NoFHPool = Request.Form["ddl_tjStatus"];
            model.ReadNoticeId = MethodHelper.ConvertHelper.ToInt32(Request.Form["ddl_tjStatus"], 0);
            //查询推荐人是不是存在
            string tuijianren = Request.Form["txt_MTJ"];
            Model.Member tjModel = CommonBase.GetList<Model.Member>("MID='" + tuijianren + "'").FirstOrDefault();
            if(tjModel == null)
                model.MTJ = "";
            else
                model.MTJ = tjModel.ID.ToString();


            string teacherMid = Request.Form["txt_TeacherMID"];
            Model.Member teacherMember = CommonBase.GetList<Model.Member>("MID='" + teacherMid + "' AND RoleCode='Teacher'").FirstOrDefault();
            if(teacherMember == null)
                model.ParentTrade = "";
            else
                model.ParentTrade = teacherMember.ID.ToString();


            //归属机构
            Model.Member companyModel = CommonBase.GetList<Model.Member>("(MID='" + Request.Form["txt_Company"] + "' OR Branch='" + Request.Form["txt_Company"] + "') and (RoleCode='1F' OR RoleCode='2F' OR RoleCode='3F' OR RoleCode='Manage')  ").FirstOrDefault();
            if(companyModel == null)
                model.Company = "";
            else
                model.Company = companyModel.ID.ToString();

            model.YunPay = Request.Form["ddlPayType"];
            if(!string.IsNullOrEmpty(Request.Form["chkTrainCode"]))
                model.Learns = Request.Form["chkTrainCode"].Replace(",", "|");
            else
                model.Learns = "";
            return model;
        }
        protected override string btnAdd_Click()
        {
            try
            {
                List<CommonObject> listComm = new List<CommonObject>();
                Model.Member model = new Model.Member();
                if(!string.IsNullOrEmpty(Request["hidId"]))
                { //修改
                    #region 修改操作
                    model = CommonBase.GetModel<Model.Member>(Request.Form["hidId"]);
                    if(model != null)
                    {
                        model = GetModel(model);
                        if(string.IsNullOrEmpty(model.MTJ))
                            return CommonHelper.Response(false, "操作失败，不存在该推荐人！");
                        //判断是不是存在此MID
                        string checkSql = "select count(1) from Member where  MID='" + model.MID + "' and ID<>'" + model.ID + "'";
                        int resu = Convert.ToInt16(CommonBase.GetSingle(checkSql));
                        if(resu > 0)
                            return CommonHelper.Response(false, "修改失败，已存在该登录账号！");

                        if(string.IsNullOrEmpty(model.Company))
                            return CommonHelper.Response(false, "操作失败，不存在该机构！");

                        string remsg = string.Empty;
                        if(model.RoleCode == "3F")
                        {
                            if(CheckCity(model, model.City, out remsg))
                            {
                                return CommonHelper.Response(false, "操作失败，" + remsg);
                            }
                        }
                        CommonBase.Update<Model.Member>(model, listComm);
                        LogService.Log(TModel, "2", TModel.MID + "修改会员" + model.MID, listComm);

                    }
                    #endregion
                }
                else
                {//添加会员
                    #region 添加操作
                    model = new Model.Member();
                    model = GetModel(model);
                    model.RoleCode = Request.Form["ddlType"];
                    model.UseBeginTime = DateTime.Now.AddDays(-1);
                    model.UseEndTime = DateTime.Now.AddDays(ConvertHelper.ToInt32(CacheService.GlobleConfig.Field2, 30) - 1);
                    model.TradePoints = MethodHelper.ConvertHelper.ToInt32(Request.Form["txtPoint"], 0);
                    model.LeaveTradePoints = MethodHelper.ConvertHelper.ToInt32(Request.Form["txtPoint"], 0);
                    //查询推荐人是不是存在
                    if(string.IsNullOrEmpty(model.MTJ))
                        return CommonHelper.Response(false, "操作失败，不存在该推荐人！");

                    if(string.IsNullOrEmpty(model.Company))
                        return CommonHelper.Response(false, "操作失败，不存在该机构！");
                    model.MCreateDate = DateTime.Now;
                    string checkSql = "select count(1) from Member where MID='" + model.MID + "'";
                    int resu = Convert.ToInt16(CommonBase.GetSingle(checkSql));
                    if(resu > 0)
                        return CommonHelper.Response(false, "添加失败，已存在该登录账号");
                    //密码都用DES加密，为了后台解密
                    model.Password = Request.Form["txtPwd"];
                    model.Password = CommonHelper.DESEncrypt(model.Password);
                    model.ID = GetGuid;
                    CommonBase.Insert<Model.Member>(model, listComm);
                    string remsg = string.Empty;
                    if(model.RoleCode == "3F")
                    {
                        if(CheckCity(model, model.City, out remsg))
                            return CommonHelper.Response(false, "操作失败，" + remsg);
                    }

                    //如果是缴费级别，就要执行分红操作
                    if(model.RoleCode != "Member")
                    {
                        //获取到归属机构
                        Model.Member companyModel = CommonBase.GetModel<Model.Member>(model.Company);
                        if(companyModel != null)
                        {
                            //扣除相应的名额数量
                            if(companyModel.LeaveTradePoints >= MethodHelper.ConvertHelper.ToInt32(model.Role.AreaLeave, 0))
                            {
                                companyModel.LeaveTradePoints = companyModel.LeaveTradePoints - MethodHelper.ConvertHelper.ToInt32(model.Role.AreaLeave, 0);
                                CommonBase.Update<Model.Member>(companyModel, new string[] { "LeaveTradePoints" }, listComm);
                                Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                                cost.Code = MethodHelper.CommonHelper.GetGuid;
                                cost.CompanyCode = companyModel.ID;
                                cost.CostCount = MethodHelper.ConvertHelper.ToInt32(model.Role.AreaLeave, 0);
                                cost.CreatedBy = "system";
                                cost.CreatedTime = DateTime.Now;
                                cost.IsDeleted = false;
                                cost.FromCompany = companyModel.ID;
                                cost.ToCompany = model.ID;
                                cost.MID = companyModel.MID;
                                cost.Status = 2;
                                cost.Remark = "后台创建用户" + model.MName + "；配送" + cost.CostCount + "个VIP名额";
                                CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                            }
                            else
                            {
                                //名额用完了就只给直接推荐奖励
                                SHMoneyService.TJChangeMoney(model, MethodHelper.ConvertHelper.ToDecimal(model.Role.Remark, 0), listComm, "0", 0, "");
                            }
                        }
                    }
                    LogService.Log(TModel, "2", TModel.MID + "创建会员" + model.MID, listComm);

                    #endregion
                }
                if(CommonBase.RunListCommit(listComm))
                    return CommonHelper.Response(true, "操作成功！");
                else
                    return CommonHelper.Response(true, "操作失败，请重试！");
            }
            catch(Exception e)
            {
                return CommonHelper.Response(false, "操作失败，请联系管理员");
            }
            return CommonHelper.Response(false, "操作失败，请联系管理员");
        }

        //如果是城市合伙人，就要判断代理的城市是否已经存在城市合伙人
        protected bool CheckCity(Model.Member model, string cityCode, out string retMsg)
        {
            bool result = false;
            if(CommonBase.GetList<Model.Member>("RoleCode='3F' AND City='" + cityCode + "' and ID<>'" + model.ID + "'").Count > 0)
            {
                result = true;
                retMsg = "该城市已存在合伙人，请选择其他城市！";
            }
            else
            {
                retMsg = string.Empty;
            }
            return result;
        }

    }
}