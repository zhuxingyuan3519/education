using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

namespace Web.m.app
{
    public partial class role_detail : BasePage
    {
        protected Sys_Role course;
        protected string img_name = string.Empty;
        protected override void SetPowerZone()
        {
            string code = Request.QueryString["code"];
            if (code == "VIP")
            {
                img_name = "sqjm_vip";
                div_province.Style.Add("display", "none");
                div_city.Style.Add("display", "none");
                div_zone.Style.Add("display", "none");
            }
            else if (code == "1F")
            {
                img_name = "sqjm_yxgs";
                div_province.Style.Add("display", "none");
                div_city.Style.Add("display", "none");
                div_zone.Style.Add("display", "none");
            }
            else if (code == "2F")
            {
                img_name = "sqjm_fwzx";
                //div_province.Style.Add("display", "none");
                //div_city.Style.Add("display", "none");
                //div_zone.Style.Add("display", "none");
            }
            else if (code == "3F")
            {
                img_name = "sqjm_pxjg";
                //div_province.Style.Add("display", "none");
                //div_city.Style.Add("display", "none");
                div_zone.Style.Add("display", "none");
            }
            course = CommonBase.GetModel<Sys_Role>(code);
            if (course == null)
                course = new Sys_Role();

            //申请城市合伙人的时候，需要选择城市，直辖市不能申请-

            ddl_province.DataSource = CacheService.SatandardAddressList.Where(c => c.LevelInt == 20 && string.IsNullOrEmpty(c.CityTelCode));
            ddl_province.DataTextField = "Name";
            ddl_province.DataValueField = "AdCode";
            ddl_province.DataBind();
            ddl_province.Items.Insert(0, new ListItem("--请选择代理省份--", ""));
        }
    }
}