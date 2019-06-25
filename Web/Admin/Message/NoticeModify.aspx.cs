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
    public partial class NoticeModify : BasePage
    {
        protected string NType = "";
        protected string txtNContent = "";
        protected override void SetValue(string id)
        {
            Model.Notice notice = null;
            notice = CommonBase.GetModel<Model.Notice>(id);
            NoticeModel = CommonBase.GetModel<Model.Notice>(id);
            if (notice != null)
            {
                NType = notice.NType.ToString();
                lbID.Value = id;
                ddlBank.DataSource = CacheService.BankList;// Bll.Sys_BankInfoBLL.Model;
                ddlBank.DataTextField = "Remark";
                ddlBank.DataValueField = "Code";
                ddlBank.DataBind();
                string bank = Request.QueryString["bank"];
                if (!string.IsNullOrEmpty(bank))
                {
                    ddlBank.Value = bank;
                }
                else
                    trBank.Style.Add("display", "none");
                ddlBank.Value = notice.Remark;
            }
        }

        private Model.Notice NoticeModel
        {
            get
            {
                Model.Notice model = CommonBase.GetModel<Model.Notice>(Request.Form["lbID"]);
                model.NTitle = Request.Form["txtNTitle"];
                model.NContent = HttpUtility.UrlDecode(Request.Form["hdContent"]);
                model.ID = int.Parse(Request.Form["lbID"]);
                model.Remark = Request.Form["ddlBank"];
                model.NState = true;
                if (!string.IsNullOrEmpty(Request.Form["hdchkFixed"]))
                    model.IsFixed = bool.Parse(Request.Form["hdchkFixed"]);
                else
                    model.IsFixed = false;
                if (model.NType == 4)
                {
                    model.NContent = HttpUtility.UrlDecode(Request.Form["txt_content"]);
                }
                return model;
            }
            set
            {
                lbID.Value = value.ID.ToString();
                txtNTitle.Value = value.NTitle;
                txtNContent = value.NContent;
                chkFixed.Checked = value.IsFixed;
            }
        }

        protected override string btnModify_Click()
        {
            Model.Notice notice = NoticeModel;
            if (CommonBase.Update<Model.Notice>(notice))
            {
                if (notice.NType == 1)
                {
                    //更新发送消息表
                    string sql = "UPDATE DB_Message SET STATUS=1,Message=N'" + NoticeModel.NContent + "' WHERE Field5='" + Request.Form["lbID"] + "'";
                    CommonBase.RunSql(sql);
                }
                else if (notice.NType == 7 || notice.NType == 8 || notice.NType == 9 || notice.NType == 11 || notice.NType == 12 || notice.NType == 13)
                {
                    //更新发送消息表
                    string sql = "UPDATE DB_EveryDayRemind SET IsRead=0,ReadTime=NULL WHERE RType='6' and Field2='" + Request.Form["lbID"] + "'";
                    CommonBase.RunSql(sql);
                }
                return "操作成功";
            }
            return "操作失败";
        }
    }
}