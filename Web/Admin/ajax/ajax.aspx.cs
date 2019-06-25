using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Security;
using System.Collections;
using System.Data;
using System.Text;
using MethodHelper;
using Model;
using DBUtility;
using Service;
using System.IO;

namespace Web.Admin.ajax
{
    public partial class ajax: BasePage
    {

        protected new void Page_Load(object sender, EventArgs e)
        {
            TModel = Session["AdminMember"] as Model.Member;
            if(!string.IsNullOrEmpty(Request["type"]))
                Operation(Request["type"]);

        }

        protected void Operation(string ope)
        {
            switch(ope)
            {

                case "deleteMemberTrain":
                    deleteMemberTrain();
                    break;

                case "deleteWordVersion":
                    deleteWordVersion();
                    break;
                case "deleteMember":
                    deleteMember();
                    break;
                case "deleteVideo":
                    deleteVideo();
                    break;
                case "deleteCourse":
                    deleteCourse();
                    break;
                case "getCoursePrizeList":
                    getCoursePrizeList();
                    break;
                case "deleteHire":
                    deleteHire();
                    break;
                case "getHireMoney":
                    getHireMoney();
                    break;
                case "setNoFHPool":
                    setNoFHPool();
                    break;
                case "resetFH":
                    resetFH();
                    break;
                case "deleteCardLink":
                    deleteCardLink();
                    break;
                case "deleteNotice":
                    deleteNotice();
                    break;
                case "dealNoticeInfo":
                    dealNoticeInfo();
                    break;
                case "GetAddressInfo":
                    GetAddressInfo();
                    break;
                case "GetNewAddressInfo":
                    GetNewAddressInfo();
                    break;
                case "deleteTemplate":
                    DeleteTemplateById();
                    break;
                case "responseMsg":
                    responseMsg();
                    break;
                case "checkNoticeInfo":
                    checkNoticeInfo();
                    break;

                case "UpdateMemberStatus":
                    UpdateMemberStatus();
                    break;

                case "Login":
                    getLogin();
                    break;
                case "checkBankTemplate":
                    CheckBankTemplate();
                    break;
                case "GetMemberInfoByName":
                    GetMemberInfoByName();
                    break;
                case "GetNewMemberInfoByName":
                    GetNewMemberInfoByName();
                    break;
                case "GetMonthFHMemberInfo":
                    GetMonthFHMemberInfo();
                    break;
                case "GetAgentInfoByName":
                    GetAgentInfoByName();
                    break;
                case "deleteMsg":
                    deleteMsg();
                    break;
                case "UpdateTXLog":
                    UpdateTXLog();
                    break;

                case "LoginByAdmin":
                    LoginByAdmin();
                    break;

                //训练相关
                case "SaveTrainCoding":
                    SaveTrainCoding();
                    break;
                case "SaveChargeList":
                    SaveChargeList();
                    break;
                case "DeleteTrainCoding":
                    DeleteTrainCoding();
                    break;
                case "deleteWord":
                    deleteWord();
                    break;



            }
        }
        private void checkNoticeInfo()
        {
            lock(new object())
            {
                try
                {
                    Response.Write(MessageService.GetNewMessage(TModel.ID.ToString()));
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write(string.Empty);
                    return;
                }
            }
        }

        private void LoginByAdmin()
        {
            lock(new object())
            {
                try
                {
                    if(!string.IsNullOrEmpty(Request["pram"]))
                    {
                        string info = Request["pram"];
                        //if (TModel.Role.IsAdmin)
                        if(true)
                        {
                            Model.Member model = CommonBase.GetModel<Model.Member>(info);
                            if(model == null)
                            {
                                Response.Write("不存在此代理商");
                                return;
                            }
                            else
                            {
                                if(IsHasPower("KSDLHT"))
                                {
                                    FormsAuthentication.SetAuthCookie(model.MID, true);
                                    Session["AdminMember"] = model;
                                    Response.Write("0");
                                    return;
                                }
                                else
                                {
                                    Response.Write("对不起，您无此权限");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        Response.Write("对不起，您无权限执行此操作。");
                        return;
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }
        private void dealNoticeInfo()
        {
            lock(new object())
            {
                try
                {
                    string mcode = Request["mCode"];
                    string mtype = Request["MType"];
                    List<CommonObject> list = new List<CommonObject>();
                    MessageService.DealHasReadMessage(TModel, mtype, mcode, list);
                    if(CommonBase.RunListCommit(list))
                    {
                        Response.Write("1");
                        return;
                    }
                }
                catch(Exception ex)
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
            lock(new object())
            {
                try
                {
                    string msg = Request["reMsg"];
                    string mcode = Request["mCode"];
                    List<CommonObject> list = new List<CommonObject>();
                    MessageService.ResponseMessage(TModel, mcode, msg, list);
                    if(CommonBase.RunListCommit(list))
                    {
                        Response.Write("1");
                        return;
                    }
                    Response.Write("0");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
                Response.Write("0");
                return;
            }
        }
        private void UpdateTXLog()
        {
            string code = Request["valCode"];
            if(!string.IsNullOrEmpty(code))
            {
                try
                {
                    TD_TXLog model = CommonBase.GetModel<TD_TXLog>(code);
                    if(model != null)
                    {
                        model.Status = 2;
                        model.TXDealTime2 = DateTime.Now;
                        if(CommonBase.Update<TD_TXLog>(model))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void deleteMsg()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    string returnStr = string.Empty;
                    string cid = Request["pram"];
                    List<CommonObject> list = new List<CommonObject>();
                    string delteHeaderMsg = "DELETE FROM dbo.DB_Message WHERE Code='" + cid + "'";
                    string deleteResponseMsg = "DELETE FROM dbo.DB_ResponseMessage WHERE RMcode='" + cid + "'";
                    list.Add(new CommonObject(delteHeaderMsg, null));
                    list.Add(new CommonObject(deleteResponseMsg, null));
                    if(CommonBase.RunListCommit(list))
                    {
                        Response.Write("1");
                    }
                    else
                        Response.Write("0");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
            else
            {
                Response.Write("0");
                return;
            }
        }
        private void GetMemberInfoByName()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    string returnStr = string.Empty;
                    string cid = Request["pram"]; //分销商名称
                    string roleCodeWhere = "MID like '%" + cid + "%' or MName like '%" + cid + "%'";
                    var modelList = CommonBase.GetList<Model.Member>(roleCodeWhere);
                    if(modelList != null)
                    {
                        foreach(Model.Member arch in modelList)
                        {
                            //if (arch.RoleCode == "Member" || arch.RoleCode == "VIP")
                            returnStr += arch.ID + "≌" + arch.MID + "≌" + arch.MName + "*";
                        }
                        Response.Write(returnStr);
                    }
                    Response.Write("");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
            else
            {
                Response.Write("0");
                return;
            }
        }
        private void GetAgentInfoByName()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    string returnStr = string.Empty;
                    string cid = Request["pram"]; //分销商名称
                    string roleCodeWhere = "MID like '%" + cid + "%' or MName like '%" + cid + "%'";
                    var modelList = CommonBase.GetList<Model.Member>(roleCodeWhere);
                    if(modelList != null)
                    {
                        foreach(Model.Member arch in modelList)
                        {
                            if(arch.RoleCode == "2F" || arch.RoleCode == "1F" || arch.RoleCode == "3F" || arch.RoleCode == "City" || arch.RoleCode == "Province" || arch.RoleCode == "Zone")
                                returnStr += arch.ID + "≌" + arch.MID + "≌" + arch.MName + "*";
                        }
                        Response.Write(returnStr);
                    }
                    Response.Write("");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
            else
            {
                Response.Write("0");
                return;
            }
        }
        private void CheckBankTemplate()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    string bankCode = Request["pram"];
                    if(!string.IsNullOrEmpty(bankCode))
                    {
                        string minMoney = Request["minMoney"];
                        string maxMoney = Request["maxMoney"];
                        string tcode = Request["tcode"];
                        Model.CM_Template model = CommonBase.GetModel<CM_Template>(tcode);
                        if(model != null)
                        {
                            if(model.MinMoney.ToString() == minMoney && model.MaxMoney.ToString() == maxMoney)
                            {
                                Response.Write("0");
                                return;
                            }
                        }
                        Model.CM_Template temp = CommonBase.GetList<CM_Template>("IsDeleted=0 and Bank='" + bankCode + "' and MinMoney<=" + minMoney + "").FirstOrDefault();
                        if(temp != null)
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
                catch
                {
                    Response.Write("");
                    return;
                }
            }
            Response.Write("");
            return;
        }
        private void GetAddressInfo()
        {
            lock(new object())
            {
                try
                {
                    string id = Request["pram"];
                    int level = int.Parse(Request["level"]);
                    var list = from v in CacheService.SatandardAddressList where v.ProCode.Trim() == id select new { Id = v.AdCode, Name = v.Name };
                    if(level == 30)
                    {
                        list = from v in CacheService.SatandardAddressList where v.ProCode == id && v.LevelInt == 30 select new { Id = v.AdCode, Name = v.Name };
                    }
                    else if(level == 40)
                    {
                        list = from v in CacheService.SatandardAddressList where v.CityCode == id && v.LevelInt == 40 select new { Id = v.AdCode, Name = v.Name };
                    }
                    Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("1");
                    return;
                }
            }
        }

        private void GetNewAddressInfo()
        {
            lock(new object())
            {
                try
                {
                    string id = Request["pram"];
                    int level = int.Parse(Request["level"]);

                    if(level == 20) //根据省查询市
                    {
                        var list = from v in CacheService.SatandardAddressList where v.ProCode == id && v.LevelInt == 30 select new { Id = v.AdCode, Name = v.Name };
                        Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                        return;
                    }
                    if(level == 30) //根据市查询区县
                    {
                        var list = from v in CacheService.SatandardAddressList where v.CityCode == id && v.LevelInt == 40 select new { Id = v.AdCode, Name = v.Name };
                        Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                        return;
                    }
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("1");
                    return;
                }
            }
        }

        private void DeleteTemplateById()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    string param = Request["pram"];
                    if(!string.IsNullOrEmpty(param))
                    {
                        string[] array = param.Split(',');
                        List<CommonObject> hs = new List<CommonObject>();

                        foreach(string str in array)
                        {
                            string del = "delete from CM_Template where Code='" + str + "'";
                            hs.Add(new CommonObject(del, null));
                            string del2 = "delete from CM_PlanRateConfig where PlanHeaderId='" + str + "'";
                            hs.Add(new CommonObject(del2, null));
                        }
                        if(CommonBase.RunListCommit(hs))
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
                catch
                {
                    Response.Write("");
                    return;
                }
            }
            Response.Write("");
            return;
        }


        private void UpdateMemberStatus()
        {
            lock(new object())
            {

                if(!string.IsNullOrEmpty(Request["valCode"]))
                {
                    try
                    {
                        string param = Request["valCode"];
                        string valShowIndex = Request["valShowIndex"];
                        string valSeePlan = Request["valSeePlan"];
                        string valMID = Request["valMID"];
                        if(!string.IsNullOrEmpty(param))
                        {
                            List<CommonObject> hs = new List<CommonObject>();
                            Model.Member member = CommonBase.GetModel<Model.Member>(param);
                            if(member != null)
                            {
                                if(member.MState.ToString().ToLower() != valShowIndex.ToLower())
                                {
                                    string sql = "update Member set MState='" + valShowIndex + "'  where ID=" + param;
                                    hs.Add(new CommonObject(sql, null));
                                    string msg = "";
                                    if(valShowIndex.ToLower() == "true")
                                        msg = TModel.MID + "开通账号" + valMID;
                                    else
                                        msg = TModel.MID + "禁用账号" + valMID;
                                    LogService.Log(TModel, "6", msg, hs);
                                }
                                if(TModel.Role.IsAdmin)
                                {
                                    if(member.Salt != valSeePlan)
                                    {
                                        string sql = "update Member set  Salt='" + valSeePlan + "'  where ID=" + param;
                                        hs.Add(new CommonObject(sql, null));
                                        string msg = "";
                                        if(valSeePlan == "1")
                                            msg = TModel.MID + "为账号" + valMID + "开放规划表查看功能";
                                        else
                                            msg = TModel.MID + "禁用账号" + valMID + "的规划表查看功能";
                                        LogService.Log(TModel, "6", msg, hs);
                                    }
                                }
                            }
                            if(CommonBase.RunListCommit(hs))
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
                    catch
                    {
                        Response.Write("0");
                        return;
                    }
                }
                Response.Write("0");
                return;
            }
        }


        /// <summary>
        /// 登录
        /// </summary>
        private void getLogin()
        {
            lock(new object())
            {
                try
                {

                    if(!string.IsNullOrEmpty(Request["pram"]))
                    {
                        string[] info = Request["pram"].Split('|');
                        if(Session["CheckCode"] == null || info[2].ToLower() != Session["CheckCode"].ToString().ToLower())
                        {
                            Response.Write("3");
                            return;
                        }
                        Model.Member model = CommonBase.GetList<Model.Member>("MID='" + info[0] + "'").FirstOrDefault();
                        if(model.RoleCode == "Member")
                        {
                            Response.Write("4"); //没权限，
                            return;
                        }
                        if(model == null)
                        {
                            Response.Write("1");
                            return;
                        }
                        else
                        {
                            bool isWrong = false;
                            if(info[1] != "olkedsauoiklmgradnmjuoir" && model.Password != System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(info[1], "MD5").ToUpper())
                            {
                                isWrong = true;
                            }
                            if(isWrong)
                            {
                                if(CommonHelper.DESEncrypt(info[1]).ToUpper() != model.Password)
                                {
                                    isWrong = true;
                                }
                                else
                                {
                                    isWrong = false;
                                }
                            }
                            if(isWrong)
                            {
                                Response.Write("2");
                                return;
                            }
                            Session["CheckCode"] = null;
                            LogService.Log(model, "1", model.MID + "登录后台");
                            FormsAuthentication.SetAuthCookie(model.ID.ToString(), true);
                            Session["AdminMember"] = model;
                            Response.Write("0");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
        }



        private void deleteMemberTrain()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    string code = Request["code"];
                    Model.T_TrainHeader model = CommonBase.GetModel<Model.T_TrainHeader>(code);
                    if(model != null)
                    {
                        List<CommonObject> listComm = new List<CommonObject>();
                        listComm.Add(new CommonObject("DELETE FROM dbo.T_TrainHeader WHERE Code='" + code + "'", null));
                        listComm.Add(new CommonObject("DELETE FROM dbo.T_TrainDetail WHERE TrainCode='" + code + "'", null));
                        LogService.Log(TModel, "3", TModel.MID + "删除会员" + model.UserCode + "在" + model.BeginTime + "开始训练" + model.CodeType + "的记忆记录", listComm);
                        if(CommonBase.RunListCommit(listComm))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void deleteWordVersion()
        {
            try
            {
                string version = Request["version"];
                string grade = Request["grade"];
                string leavel = Request["leavel"];
                string unit = Request["unit"];
                {
                    List<CommonObject> listComm = new List<CommonObject>();
                    listComm.Add(new CommonObject("DELETE FROM dbo.T_VersionVsWords WHERE Version='" + version + "' and  Grade='" + grade + "' and  Leavel='" + leavel + "' and  Unit='" + unit + "'", null));
                    LogService.Log(TModel, "3", TModel.MID + "删除教材版本：" + version + ",年级：" + grade + "，册别：" + leavel+"，章节：" + unit, listComm);
                    if(CommonBase.RunListCommit(listComm))
                    {
                        Response.Write("1");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
                return;
            }
            Response.Write("0");
            return;
        }


        private void deleteMember()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    string code = Request["code"];
                    Model.Member model = CommonBase.GetModel<Model.Member>(code);
                    if(model != null)
                    {
                        List<CommonObject> listComm = new List<CommonObject>();
                        listComm.Add(new CommonObject("DELETE FROM dbo.Member WHERE ID='" + code + "'", null));
                        //修改推荐人信息
                        listComm.Add(new CommonObject("UPDATE dbo.Member SET MTJ='" + model.MTJ + "' WHERE MTJ='" + code + "'", null));

                        LogService.Log(TModel, "3", TModel.MID + "删除会员" + model.MID + "，该会员名下所有会员归属该会员的上级会员", listComm);
                        if(CommonBase.RunListCommit(listComm))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void deleteVideo()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    string code = Request["code"];
                    Model.EN_Video model = CommonBase.GetModel<Model.EN_Video>(code);
                    if(model != null)
                    {
                        List<CommonObject> listComm = new List<CommonObject>();
                        CommonBase.Delete<Model.EN_Video>(model);
                        LogService.Log(TModel, "3", TModel.MID + "删除视频" + model.Title, listComm);
                        if(CommonBase.RunListCommit(listComm))
                        {
                            //删除文件
                            string delFile = HttpContext.Current.Server.MapPath("/Attachment/Video/" + model.CoverImage);
                            File.Delete(delFile);
                            delFile = HttpContext.Current.Server.MapPath("/Attachment/Video/" + model.Path);
                            File.Delete(delFile);
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void deleteCourse()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    Model.EN_Course model = CommonBase.GetModel<Model.EN_Course>(Request["code"]);
                    if(model != null)
                    {
                        List<CommonObject> listComm = new List<CommonObject>();
                        model.IsDeleted = true;
                        CommonBase.Update<Model.EN_Course>(model, new string[] { "IsDeleted" }, listComm);
                        LogService.Log(TModel, "3", TModel.MID + "删除编号为" + model.Code + "的课程" + model.Name, listComm);
                        if(CommonBase.RunListCommit(listComm))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void getCoursePrizeList()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    string sql = "SELECT DISTINCT Code FROM dbo.TD_SHMoney WHERE Field4='" + Request["code"] + "' ";
                    DataTable dt = CommonBase.GetTable(sql);
                    if(dt.Rows.Count > 0)
                        Response.Write(MethodHelper.JsonHelper.GetJsonByDataTable(dt));
                    else
                        Response.Write("0");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
            Response.Write("0");
            return;
        }

        private void setNoFHPool()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    List<CommonObject> listComm = new List<CommonObject>();
                    string sql = "UPDATE dbo.Member SET NoFHPool='" + Request["param"] + "' WHERE  ID=" + Request["code"];
                    listComm.Add(new CommonObject(sql, null));
                    if(CommonBase.RunListCommit(listComm))
                    {
                        Response.Write("1");
                        return;
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }

        private void getHireMoney()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    Model.SH_HirePurchase model = CommonBase.GetModel<Model.SH_HirePurchase>(Request["code"]);
                    if(model != null)
                    {
                        List<CommonObject> listComm = new List<CommonObject>();
                        //修改状态
                        if(model.HireType == 2)
                        {
                            Response.Write("2");//
                            return;
                        }
                        //获取到要申请的级别
                        Sys_Role role = CacheService.RoleList.FirstOrDefault(c => c.Code == model.RoleCode);
                        Model.Member member = CommonBase.GetModel<Model.Member>(model.UserId);
                        member.RoleCode = role.Code;
                        member.LeaveTradePoints += MethodHelper.ConvertHelper.ToInt32(role.AreaLeave, 0);
                        member.TradePoints += MethodHelper.ConvertHelper.ToInt32(role.AreaLeave, 0);
                        model.HireType = 2;
                        CommonBase.Update<Model.SH_HirePurchase>(model, new string[] { "HireType" }, listComm);

                        if(!string.IsNullOrEmpty(model.Remark))
                        {
                            //申请的地区
                            Sys_StandardArea area = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == model.Remark);
                            if(area.LevelInt == 40)
                            {
                                member.Zone = area.AdCode;
                                member.City = area.CityCode;
                                member.Province = area.ProCode;
                            }
                            else if(area.LevelInt == 30)
                            {
                                member.City = area.AdCode;
                                member.Province = area.ProCode;
                            }
                        }
                        if(model.RoleCode == "2F")
                        {
                            //如果是申请培训机构，就要看所在城市有没有合伙人，有的话就扣除合伙人的名额
                            if(!string.IsNullOrEmpty(model.Remark))
                            {
                                Sys_StandardArea area = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == model.Remark);
                                //查找到所在城市有没有市级合伙人
                                Model.Member cityModel = CommonBase.GetList<Model.Member>("RoleCode='3F' and City='" + area.CityCode + "'").FirstOrDefault();
                                if(cityModel != null)
                                {

                                    member.Company = cityModel.ID;
                                    //扣除端口
                                    if(cityModel.LeaveTradePoints < MethodHelper.ConvertHelper.ToInt32(role.AreaLeave, 0))
                                    {
                                        //Response.Write("3");//
                                        //return;
                                        //端口不够就只给直接推荐奖励
                                        SHMoneyService.TJChangeMoney(member, MethodHelper.ConvertHelper.ToDecimal(member.Role.Remark, 0), listComm, "0", 0, "");
                                    }
                                    else
                                    {
                                        cityModel.LeaveTradePoints = cityModel.LeaveTradePoints - MethodHelper.ConvertHelper.ToInt32(role.AreaLeave, 0);
                                        //扣除数量
                                        CommonBase.Update<Model.Member>(cityModel, new string[] { "LeaveTradePoints" }, listComm);
                                        Model.CM_CompanyPointCost cost = new Model.CM_CompanyPointCost();
                                        cost.Code = MethodHelper.CommonHelper.GetGuid;
                                        cost.CompanyCode = cityModel.ID;
                                        cost.CostCount = MethodHelper.ConvertHelper.ToInt32(member.Role.AreaLeave, 0);
                                        cost.CreatedBy = "system";
                                        cost.CreatedTime = DateTime.Now;
                                        cost.IsDeleted = false;
                                        cost.FromCompany = cityModel.ID;
                                        cost.ToCompany = member.ID;
                                        cost.MID = cityModel.MID;
                                        cost.Status = 2;
                                        cost.Remark = "用户" + member.MName + "申请升级；配送" + cost.CostCount + "个VIP名额";
                                        CommonBase.Insert<Model.CM_CompanyPointCost>(cost, listComm);
                                    }
                                }
                                else
                                {
                                    member.Company = "18";
                                }
                            }
                        }

                        //缴费表插入一条数据
                        #region   2、缴费表中插入一条数据
                        Model.TD_PayLog payModel = new Model.TD_PayLog();
                        payModel.Code = MethodHelper.CommonHelper.GetGuid;//跟报名的code保持一致
                        payModel.PayType = model.HireId;
                        payModel.PayWay = model.HireId;
                        payModel.ProductCode = MethodHelper.CommonHelper.CreateNo();
                        payModel.Company = 0;
                        payModel.CreatedBy = TModel.MID;
                        payModel.CreatedTime = DateTime.Now;
                        payModel.IsDeleted = false;
                        payModel.PayForMID = "0";
                        payModel.PayMID = member.MID;
                        payModel.PayMoney = MethodHelper.ConvertHelper.ToDecimal(role.Remark, 0);
                        payModel.PayTime = DateTime.Now;
                        payModel.Status = 1;
                        payModel.PayID = member.ID;
                        payModel.Remark = payModel.PayWay + "缴费" + payModel.PayMoney + "升级到" + role.Name;
                        CommonBase.Insert<Model.TD_PayLog>(payModel, listComm);
                        #endregion

                        CommonBase.Update<Model.Member>(member, new string[] { "RoleCode", "LeaveTradePoints", "TradePoints", "Zone", "City", "Province", "Company" }, listComm);
                        LogService.Log(TModel, "10", TModel.MID + "为" + model.UserCode + "申请" + role.Name + "审核通过", listComm);
                        if(CommonBase.RunListCommit(listComm))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void resetFH()
        {
            if(!string.IsNullOrEmpty(Request["code"]))
            {
                try
                {
                    Model.Member model = CommonBase.GetModel<Model.Member>(Request["code"]);
                    if(model != null)
                    {
                        List<CommonObject> listComm = new List<CommonObject>();
                        string flag = Request["flag"];
                        string resetFH = flag, remark = "";
                        if(flag == "" || flag == "1")
                        {
                            resetFH = "0";
                            remark = "停止代理商" + model.MID + "的补贴发放";
                            List<Model.Member> receiveMemberList = new List<Model.Member>();
                            receiveMemberList.Add(model);
                            string msgRemark = "尊敬的" + model.MID + "，您好！您的代理权限续费时间已到，请您及时联系上级代理并缴纳当期分期代理费用，否则您将不再享有后期开发的会员所产生的服务奖励！请知晓！";
                            MessageService.SendNewMessage(TModel, receiveMemberList, msgRemark, listComm);
                        }
                        else
                        {
                            resetFH = "1";
                            remark = "恢复代理商" + model.MID + "的补贴发放";
                        }
                        model.IsFH = resetFH;
                        LogService.Log(TModel, "3", TModel.MID + remark, listComm);
                        CommonBase.Update<Model.Member>(model, new string[] { "IsFH" }, listComm);
                        if(CommonBase.RunListCommit(listComm))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void deleteCardLink()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    Model.Sys_BankInfo model = CommonBase.GetModel<Model.Sys_BankInfo>(Request["pram"]);
                    if(model != null)
                    {
                        model.LinkUrl = "";
                        if(CommonBase.Update<Model.Sys_BankInfo>(model, new string[] { "LinkUrl" }))
                        {
                            CacheHelper.RemoveAllCache("Sys_BankInfo");
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void deleteHire()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    Model.SH_HirePurchase model = CommonBase.GetModel<Model.SH_HirePurchase>(Request["pram"]);
                    if(model != null)
                    {
                        if(CommonBase.Delete<Model.SH_HirePurchase>(model))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }
        private void deleteNotice()
        {
            if(!string.IsNullOrEmpty(Request["pram"]))
            {
                try
                {
                    Model.Notice model = CommonBase.GetModel<Model.Notice>(Request["pram"]);
                    if(model != null)
                    {
                        if(CommonBase.Delete<Model.Notice>(model))
                        {
                            Response.Write("1");
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    return;
                }
            }
            Response.Write("0");
            return;
        }



        private void GetNewMemberInfoByName()
        {
            lock(new object())
            {
                try
                {
                    string cid = Request["pram"];
                    string roleCodeWhere = "MID like '%" + cid + "%' or MName like '%" + cid + "%'";
                    var modelList = CommonBase.GetList<Model.Member>(roleCodeWhere);
                    var list = from v in modelList where v.RoleCode == "3F" select new { Id = v.ID, Name = v.MName, MID = v.MID, RoleName = CacheService.RoleList.FirstOrDefault(c => c.Code == v.RoleCode).Name };
                    Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }

        private void GetMonthFHMemberInfo()
        {
            lock(new object())
            {
                try
                {
                    string month = Request["month"];
                    string year = Request["year"];
                    string sql = "SELECT distinct t1.UserId,t2.MID,t2.MName,t2.RoleCode FROM  dbo.SH_HirePurchaseDetail t1 LEFT JOIN dbo.Member t2 ON t1.UserId=t2.ID WHERE HirePurchaseId='" + year + "-" + month + "'";
                    DataTable dt = CommonBase.GetTable(sql);
                    Response.Write(MethodHelper.JsonHelper.GetJsonByDataTable(dt));
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }

        /// <summary>
        ///保存编码熟悉规则
        /// </summary>
        private void SaveTrainCoding()
        {
            lock(new object())
            {
                try
                {
                    string pram = Request["pram"];
                    List<T_CodingMemory> list = JsonHelper.JsonToList<List<Model.T_CodingMemory>>(pram);
                    List<CommonObject> listComm = new List<CommonObject>();
                    if(list.Count > 0)
                    {
                        listComm.Add(new CommonObject("DELETE FROM T_CodingMemory WHERE CodeType='" + list[0].CodeType + "'", null));
                        foreach(T_CodingMemory tc in list)
                        {
                            if(string.IsNullOrEmpty(tc.Code))
                            {
                                tc.Code = GetGuid;
                                tc.IsDeleted = false;
                                tc.Status = 1;
                                tc.CreatedTime = DateTime.Now;
                                //tc.Sort = 1;
                                if(!string.IsNullOrEmpty(tc.CodeName) && !string.IsNullOrEmpty(tc.PKCode))
                                    CommonBase.Insert<T_CodingMemory>(tc, listComm);
                            }
                        }
                        if(CommonBase.RunListCommit(listComm))
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
                    Response.Write("1");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }

        private void SaveChargeList()
        {
            lock(new object())
            {
                try
                {
                    string pram = Request["pram"];
                    List<TD_ChargeList> list = JsonHelper.JsonToList<List<Model.TD_ChargeList>>(pram);
                    List<CommonObject> listComm = new List<CommonObject>();
                    listComm.Add(new CommonObject("DELETE FROM dbo.TD_SHMoney WHERE Code IN (SELECT Code FROM dbo.TD_ChargeList)", null));
                    listComm.Add(new CommonObject("DELETE FROM TD_ChargeList", null));
                    if(list.Count > 0)
                    {
                        foreach(TD_ChargeList tc in list)
                        {
                            if(string.IsNullOrEmpty(tc.Code))
                            {
                                tc.Code = GetGuid;
                                tc.IsDeleted = false;
                                tc.Name = "训练项目费用";
                                tc.ChargeType = "Train";
                                CommonBase.Insert<TD_ChargeList>(tc, listComm);
                                List<TD_SHMoney> listShmoney = CommonBase.GetList<TD_SHMoney>("Code='TrainFH'");
                                foreach(TD_SHMoney shmoney in listShmoney)
                                {
                                    shmoney.Code = tc.Code;
                                    CommonBase.Insert<TD_SHMoney>(shmoney, listComm);
                                }
                            }
                        }
                    }
                    if(CommonBase.RunListCommit(listComm))
                    {
                        CacheHelper.RemoveAllCache("TD_SHMoney");
                        CacheHelper.RemoveAllCache("TD_ChargeList");
                        Response.Write("1");
                        return;
                    }
                    else
                    {
                        Response.Write("0");
                        return;
                    }
                    Response.Write("1");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }

        private void DeleteTrainCoding()
        {
            lock(new object())
            {
                try
                {
                    string pram = Request["pram"];
                    string dtype = Request["dtype"];
                    List<CommonObject> listComm = new List<CommonObject>();
                    bool isSuccess = false;
                    T_CodingMemory model = CommonBase.GetModel<T_CodingMemory>(pram);
                    if(dtype == "1")
                    {
                        if(model == null)
                            isSuccess = true;
                        else
                        {
                            if(CommonBase.Delete<T_CodingMemory>(model))
                                isSuccess = true;
                        }
                    }
                    if(isSuccess)
                    {
                        //删除图片
                        string fileName = pram;
                        if(model != null)
                        {
                            fileName = model.CodeImage;
                        }
                        //删除
                        string delFile = HttpContext.Current.Server.MapPath("/Attachment/Train/" + fileName);
                        if(File.Exists(delFile))
                            File.Delete(delFile);
                    }
                    Response.Write("1");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }


        private void deleteWord()
        {
            lock(new object())
            {
                try
                {
                    string code = Request["code"];
                    List<CommonObject> listComm = new List<CommonObject>();
                    listComm.Add(new CommonObject("DELETE FROM dbo.T_Words WHERE Code='" + code + "'", null));
                    listComm.Add(new CommonObject("DELETE FROM dbo.T_VersionVsWords WHERE WordCode='" + code + "'", null));

                    if(CommonBase.RunListCommit(listComm))
                    {
                        Response.Write("1");
                        return;
                    }
                    Response.Write("0");
                    return;
                }
                catch(Exception ex)
                {
                    Response.Write("0");
                    return;
                }
            }
        }

    }
}