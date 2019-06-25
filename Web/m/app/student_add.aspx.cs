using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class student_add: BasePage
    {

        protected override void SetPowerZone()
        {
            txt_Company.Value = TModel.Branch;
            hid_companyId.Value = TModel.ID;
            txt_MTJMID.Value = TModel.MID;
            txt_MTJ.Value = TModel.ID;
        }

        protected string GetMtjMid()
        {
            //查看验证码是否正确
            string mtjCode = Request.Form["mtjcode"];
            string testSql = "select MID from Member where  ID='" + mtjCode + "'";
            object obj = CommonBase.GetSingle(testSql);
            if(obj != null)
            {
                return obj.ToString();
            }
            return "";
        }
        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            //保存学员信息
            Member model = new Member();
            model.ID = GetGuid;
            model.MID = Request.Form[NameHeader + txt_MID.ClientID];
            //查看手机号是否已经注册过并且已经删除
            Member isDeletedMember = CommonBase.GetList<Member>("MID='" + model.MID + "' ").FirstOrDefault();
            if(isDeletedMember != null)
            {
                return "-1*";
            }
            model.Password = MethodHelper.CommonHelper.DESEncrypt(Request.Form[NameHeader + txt_Password2.ClientID]);
            model.MName = Request.Form[NameHeader + txt_Name.ClientID];
            string teacherMId = Request.Form[NameHeader + txt_TeacherMID.ClientID];
            model.Company = TModel.ID;
            if(!string.IsNullOrEmpty(teacherMId))
            {
                Model.Member teacherMember = CommonBase.GetList<Model.Member>("MID='" + teacherMId + "'").FirstOrDefault();
                if(teacherMember != null)
                {
                    model.ParentTrade = teacherMember.ID;
                }
                else
                {
                    return "-2*";
                }
            }

            Model.Member tjModel = CommonBase.GetList<Model.Member>("MID='" + Request.Form[NameHeader + txt_MTJMID.ClientID] + "'").FirstOrDefault();
            if(tjModel == null)
            {
                return "-3*";
            }
            else
            {
                model.MTJ = tjModel.ID.ToString();
            }

            model.MState = true;
            model.RoleCode = "Member";
            model.IsClose = false;
            model.MCreateDate = DateTime.Now;
            CommonBase.Insert<Model.Member>(model, listComm);
            //机构的名额减少一个
            //TModel.LeaveTradePoints = TModel.LeaveTradePoints - 1;
            //CommonBase.Update<Model.Member>(TModel, new string[] { "LeaveTradePoints" }, listComm);

            if(CommonBase.RunListCommit(listComm))
            {
                //注册成之后，加入session
                LogService.Log(TModel, "99", TModel.MID + "开通学员" + model.MID);
                return "1*" + model.ID;
            }
            return "0*";
        }
    }
}