using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.WebAdmin
{
    public partial class WebConfig : BasePage
    {
        protected string txtNContent = "";
        protected override void SetValue(string id)
        {
            hidCode.Value = id;
            string edittype = Request.QueryString["edittype"];
            hidType.Value = edittype;
            if (edittype == "role")
            {
                Sys_Role role = CommonBase.GetModel<Sys_Role>(id);
                if (role != null)
                {
                    //txtNContent = role.Introduce;
                     editor1.Value =  role.Introduce;
                }
            }
            else
            {
                string remark = Request.QueryString["remark"];
                hidRemark.Value = remark;
                Sys_WebConfig config = CacheService.WebConfigList.Where(c => c.Code == id).FirstOrDefault();
                if (config != null)
                {
                    //txtNContent = config.Value;
                      editor1.Value =config.Value;
                }
            }
        }
        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            if (Request.Form["hidType"] == "role")
            {
                Sys_Role role = CommonBase.GetModel<Sys_Role>(Request.Form["hidCode"]);
                role.Introduce = HttpUtility.UrlDecode(Request.Form["editor1"]);
                CommonBase.Update<Sys_Role>(role, listComm);
            }
            else
            {
                Model.Sys_WebConfig config = null;
                if (!string.IsNullOrEmpty(Request.Form["hidCode"]))
                {
                    config = CacheService.WebConfigList.Where(c => c.Code == Request.Form["hidCode"]).FirstOrDefault();
                    if (config != null)
                    {
                        config.Value = HttpUtility.UrlDecode(Request.Form["editor1"]);
                        config.Remark = HttpUtility.UrlDecode(Request.Form["hidRemark"]);
                        CommonBase.Update<Sys_WebConfig>(config, listComm);
                    }
                    else
                    {
                        config = new Sys_WebConfig();
                        config.Code = Request.Form["hidCode"];
                        config.Value = HttpUtility.UrlDecode(Request.Form["editor1"]);
                        config.Remark = HttpUtility.UrlDecode(Request.Form["hidRemark"]);
                        config.Status = true;
                        CommonBase.Insert<Sys_WebConfig>(config, listComm);
                    }
                }
                else
                {
                    config = new Sys_WebConfig();
                    config.Code = Request.Form["hidCode"];
                    config.Value = HttpUtility.UrlDecode(Request.Form["editor1"]);
                    config.Remark = HttpUtility.UrlDecode(Request.Form["hidRemark"]);
                    config.Status = true;
                    CommonBase.Insert<Sys_WebConfig>(config, listComm);
                }
            }

            if (CommonBase.RunListCommit(listComm))
            {
                if (Request.Form["hidType"] == "role")
                    CacheHelper.RemoveAllCache("Sys_Role");
                else
                {//更新魂村
                    CacheHelper.RemoveAllCache("Sys_WebConfig");
                }
                return "1";
            }
            else
                return "0";
        }
    }
}