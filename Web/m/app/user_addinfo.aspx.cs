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
    public partial class user_addinfo : BasePage
    {
        protected string isShowChoiceRole = "0";
        protected override void SetPowerZone()
        {
            div_org_info.Visible = false;
            div_member_info.Visible = false;
            if (TModel.RoleCode == "1F" || TModel.RoleCode == "2F" || TModel.RoleCode == "3F")
            {
                div_member_info.Visible = false;
                div_org_info.Visible = true;
            }
            else //if (TModel.RoleCode == "Member" || TModel.RoleCode == "VIP")
            {
                div_member_info.Visible = true;
                div_org_info.Visible = false;
            }

            System.Web.UI.HtmlControls.HtmlSelect selectControl = ddl_Province;// this.Page.FindControl(NameHeader + ddl_Province.ClientID) as System.Web.UI.HtmlControls.HtmlSelect;
            if (selectControl != null)
            {
                selectControl.DataSource = CacheService.SatandardAddressList.Where(c => c.LevelInt == 20);// CacheService.AddressList.Where(c => c.Level == 20);
                selectControl.DataTextField = "Name";
                selectControl.DataValueField = "AdCode";
                selectControl.DataBind();
                selectControl.Items.Insert(0, new ListItem("--省--", ""));
            }

            GetDictBindDDL("Grade", ddl_gread);
            ddl_gread.Items.Insert(0, new ListItem("--所在年级--", ""));


            //TModel.RegistPointer;
            ddl_gread.Value = TModel.RegistPointer;
            if (!string.IsNullOrEmpty(TModel.Image))
            {
                uploadImg.Value = TModel.Image;
                img_pic.Src = TModel.Image;
            }
            txt_father.Value = TModel.Father;
            txt_fatherTel.Value = TModel.FatherTel;
            txt_Mname.Value = TModel.MName;
            //txt_mather.Value = TModel.Mather;
            //txt_matherTel.Value = TModel.MatherTel;
            //txt_other.Value = TModel.Other;
            //txt_otherTel.Value = TModel.OtherTel;

            ddl_Province.Value = TModel.Province;
            txt_address.Value = TModel.Address;
            if (!string.IsNullOrEmpty(TModel.City))
            {
                //绑定市
                ddl_City.DataSource = CacheService.SatandardAddressList.Where(c => c.ProCode.Trim() == TModel.Province);
                ddl_City.DataTextField = "Name";
                ddl_City.DataValueField = "AdCode";
                ddl_City.DataBind();
                ddl_City.Items.Insert(0, new ListItem("--市--", ""));
                ddl_City.Value = TModel.City;
            }

            if (!string.IsNullOrEmpty(TModel.Zone))
            {
                //绑定区县
                ddl_Zone.DataSource = CacheService.SatandardAddressList.Where(c => c.CityCode.Trim() == TModel.City);
                ddl_Zone.DataTextField = "Name";
                ddl_Zone.DataValueField = "AdCode";
                ddl_Zone.DataBind();
                ddl_Zone.Items.Insert(0, new ListItem("--区县--", ""));
                ddl_Zone.Value = TModel.Zone;
            }


            //if (TModel.RoleCode == "Oragin")
            {
                txt_orgMName.Value = TModel.MName;
                txt_OrgTel.Value = TModel.Tel;
                txt_orgMID.Value = TModel.MID;
                txt_orgBranch.Value = TModel.Branch;
            }



        }


        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            TModel.Province = Request.Form[NameHeader + ddl_Province.ClientID];
            TModel.City = Request.Form[NameHeader + ddl_City.ClientID];
            TModel.Zone = Request.Form[NameHeader + ddl_Zone.ClientID];
            TModel.Address = Request.Form[NameHeader + txt_address.ClientID];
            TModel.RegistPointer = Request.Form[NameHeader + ddl_gread.ClientID];
            TModel.Father = Request.Form[NameHeader + txt_father.ClientID];
            TModel.FatherTel = Request.Form[NameHeader + txt_fatherTel.ClientID];
            //TModel.Mather = Request.Form[NameHeader + txt_mather.ClientID];
            //TModel.Other = Request.Form[NameHeader + txt_other.ClientID];
            //TModel.MatherTel = Request.Form[NameHeader + txt_matherTel.ClientID];
            //TModel.OtherTel = Request.Form[NameHeader + txt_otherTel.ClientID];
            TModel.Image = Request.Form[NameHeader + uploadImg.ClientID];
            TModel.MName = Request.Form[NameHeader + txt_Mname.ClientID];

           if (TModel.RoleCode == "1F"||TModel.RoleCode == "2F"||TModel.RoleCode == "3F")
            {
                TModel.MName = Request.Form[NameHeader + txt_orgMName.ClientID];
                TModel.Tel = Request.Form[NameHeader + txt_OrgTel.ClientID];
                //TModel.MID = Request.Form[NameHeader + txt_orgMID.ClientID];
                //TModel.Branch = Request.Form[NameHeader + txt_orgBranch.ClientID];
            }

            //if (!string.IsNullOrEmpty(TModel.MName) && !string.IsNullOrEmpty(TModel.RegistPointer) && !string.IsNullOrEmpty(TModel.Father) && !string.IsNullOrEmpty(TModel.FatherTel))
            {
                TModel.Mather = "1";
            }

            CommonBase.Update<Model.Member>(TModel, listComm);
            if (CommonBase.RunListCommit(listComm))
            {
                return "1";
            }
            return "0";
        }

    }
}