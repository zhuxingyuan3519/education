using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Message
{
    public partial class NoticeAdd : BasePage
    {
        protected string NType = "";
        protected override void SetValue(string id)
        {
            NType = id;
            hidType.Value = id;
            hidCType.Value = Request.QueryString["type"];
            string bank = Request.QueryString["bank"];
            ddlBank.DataSource = CacheService.BankList;
            ddlBank.DataTextField = "Remark";
            ddlBank.DataValueField = "Code";
            ddlBank.DataBind();
            if (!string.IsNullOrEmpty(bank))
            {

                //ddlBank.Items.Add(new ListItem("其他银行", "other"));
                ddlBank.Value = bank;
            }
            else
                trBank.Style.Add("display", "none");
        }
        private Model.Notice NoticeModel
        {
            get
            {
                Model.Notice model = new Model.Notice();
                model.NTitle = Request.Form["txtNTitle"];
                model.NContent = HttpUtility.UrlDecode(Request.Form["hdContent"]);
                model.NType = int.Parse(Request.Form["hidType"]);
                model.Remark = Request.Form["ddlBank"];
                if (model.NType == 2)
                {
                    model.Company = int.Parse(Request.Form["hidCType"]);
                }
                if (model.NType == 4) //信用贷款
                {
                    model.NContent = Request.Form["txt_content"];
                }

                if (!string.IsNullOrEmpty(Request.Form["isFaxed"]))
                    model.IsFixed = bool.Parse(Request.Form["isFaxed"]);
                else
                    model.IsFixed = false;
                model.NCreateTime = DateTime.Now;
                model.NState = true;
                return model;
            }
        }

        protected override string btnAdd_Click()
        {
            Model.Notice notice = NoticeModel;
            if (CommonBase.Insert<Model.Notice>(notice))
            {
                //取到最新的Id
                string getId = "SELECT MAX(ID) FROM dbo.Notice";
                object obj = CommonBase.GetSingle(getId);
                if (notice.NType == 1)
                {
                    List<CommonObject> list = new List<CommonObject>();
                    //全体发送
                    List<Model.Member> listAll = CommonBase.GetList<Model.Member>("RoleCode<>'Manage' ");
                    foreach (Model.Member mcode in listAll)
                    {
                        DB_Message_Model model = new Model.DB_Message_Model();
                        model.Code = GetGuid;
                        model.CreatedBy = TModel.MID;
                        model.CreatedTime = DateTime.Now;
                        model.IsDeleted = false;
                        model.MType = "1";
                        model.ReceiveCode = mcode.ID.ToString();
                        model.SendCode = TModel.ID.ToString();
                        model.Status = 1;
                        model.Field5 = obj.ToString();
                        model.Message = NoticeModel.NContent;
                        CommonBase.Insert<DB_Message_Model>(model, list);
                    }
                    CommonBase.RunListCommit(list);
                }
                else if (notice.NType == 7 || notice.NType == 8 || notice.NType == 9 || notice.NType == 11 || notice.NType == 12 || notice.NType == 13)
                {
                    //添加重要提醒项目
                    List<CommonObject> list = new List<CommonObject>();
                    //全体发送
                    List<Model.Member> listAll = CommonBase.GetList<Model.Member>("RoleCode='VIP' OR  RoleCode='Member' ");
                    foreach (Model.Member mcode in listAll)
                    {
                        DB_EveryDayRemind remind = new DB_EveryDayRemind();
                        remind.Code = GetGuid;
                        remind.RType = "6";
                        remind.Status = 1;
                        remind.CreatedBy = TModel.MID;
                        remind.CreatedTime = DateTime.Now;
                        remind.IsDeleted = false;
                        remind.IsRead = false;
                        remind.LogDate = DateTime.Now;
                        //remind.ReadTime
                        remind.MCode = mcode.MID;
                        remind.MID = mcode.ID;
                        remind.Field2 = obj.ToString();
                        string remark="管理员后台有最新口子发布，请及时查阅";
                        switch(notice.NType)
                        {
                            case 7: remark = "有最新快卡口子发布，请及时查阅"; break;
                            case 8: remark = "有最新提额口子发布，请及时查阅"; break;
                            case 9: remark = "有最新贷款口子发布，请及时查阅"; break;
                            case 11: remark = "分销商后台有最新快卡口子发布，请及时查阅"; break;
                            case 12: remark = "分销商后台有最新提额口子发布，请及时查阅"; break;
                            case 13: remark = "分销商后台有最新贷款口子发布，请及时查阅"; break;
                        }

                        remind.Remark = remark;
                        CommonBase.Insert<DB_EveryDayRemind>(remind, list);
                    }
                    CommonBase.RunListCommit(list);
                }
                return "1";
            }
            return "0";
        }
    }
}