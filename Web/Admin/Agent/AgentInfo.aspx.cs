
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
    public partial class AgentInfo : BasePage
    {
        protected string HireStatusString = string.Empty;
        protected bool IsShowHire = false;
        protected override void SetPowerZone()
        {
            spAgentLeavel.InnerHtml = TModel.Role.Name;

            System.Web.UI.HtmlControls.HtmlSelect selectControl = ddl_Province;// this.Page.FindControl(NameHeader + ddl_Province.ClientID) as System.Web.UI.HtmlControls.HtmlSelect;
            if (selectControl != null)
            {
                selectControl.DataSource = CacheService.SatandardAddressList.Where(c => c.LevelInt == 20);// CacheService.AddressList.Where(c => c.Level == 20);
                selectControl.DataTextField = "Name";
                selectControl.DataValueField = "AdCode";
                selectControl.DataBind();
                selectControl.Items.Insert(0, new ListItem("--请选择--", ""));
            }

            //如果是修改/查看

            string checkMTJ = "select MID from Member where ID=" + TModel.MTJ;
            object obj = CommonBase.GetSingle(checkMTJ);
            if (obj != null)
            {
                txt_MTJ.Value = obj.ToString();
                txt_MTJ.Attributes.Add("readonly", "readonly");
            }
            txt_Email.Value = TModel.Email;
            txt_Name.Value = TModel.MName;
            txtPhone.Value = TModel.Tel;
            txt_QQ.Value = TModel.QQ;
            txt_WeiXin.Value = TModel.Weichat;
            //ddlStatus.Value = model.Status.ToString();
            hidId.Value = TModel.ID.ToString();
            txt_MID.Value = TModel.MID;
            txt_pwd.Value = CommonHelper.DESDecrypt(TModel.Password);

            ddl_Province.Value = TModel.Province;


            //绑定市
            ddl_City.DataSource = CacheService.SatandardAddressList.Where(c => c.ProCode== TModel.Province);
            ddl_City.DataTextField = "Name";
            ddl_City.DataValueField = "AdCode";
            ddl_City.DataBind();
            ddl_City.Items.Insert(0, new ListItem("--请选择--", ""));
            ddl_City.Value = TModel.City;


            //绑定区县
            ddl_Zone.DataSource = CacheService.SatandardAddressList.Where(c => c.CityCode.Trim() == TModel.City);
            ddl_Zone.DataTextField = "Name";
            ddl_Zone.DataValueField = "AdCode";
            ddl_Zone.DataBind();
            ddl_Zone.Items.Insert(0, new ListItem("--请选择--", ""));
            ddl_Zone.Value = TModel.Zone;
        }
        protected Model.Member GetModel(Model.Member model)
        {
            model.Email = Request.Form["txt_Email"];
            model.MName = Request.Form["txt_Name"];
            model.Tel = Request.Form["txtPhone"];
            model.Email = Request.Form["txt_Email"];
            model.Weichat = Request.Form["txt_WeiXin"];
            model.Province = Request.Form["ddl_Province"];
            model.City = Request.Form["ddl_City"];
            model.Zone = Request.Form["ddl_Zone"];
            model.QQ = Request.Form["txt_QQ"];
            model.Password = CommonHelper.DESEncrypt(Request.Form["txt_pwd"]);
            return model;
        }
        protected override string btnAdd_Click()
        {
            try
            {
                TModel = GetModel(TModel);
                if (CommonBase.Update<Model.Member>(TModel))
                {
                    return "1";
                }
            }
            catch (Exception e)
            {
                return "操作失败：" + e.Message;
            }
            return "操作失败，请重试";
        }
    }
}
