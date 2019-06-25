using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.SystemManage
{
    public partial class MenuEdit : BasePage
    {
        protected List<Sys_Privage> list;
        protected string mtype;
        protected override void SetPowerZone()
        {
            mtype = Request.QueryString["mtype"];
            list = CommonBase.GetList<Sys_Privage>("IsDeleted=0 and PrivageType=" + mtype);
            string code = Request.QueryString["txtcode"], name = Request.QueryString["txtname"] == "undefined" ? "" : Request.QueryString["txtname"];
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name))
                list = list.Where(c => c.Name.Contains(code) && c.Name.Contains(name) && c.ParentCode == "0").ToList();
            else if (!string.IsNullOrEmpty(code))
                list = list.Where(c => c.Name.Contains(code) && c.ParentCode == "0").ToList();
            else if (!string.IsNullOrEmpty(name))
                list = list.Where(c => c.Name.Contains(name) && c.ParentCode == "0").ToList();
        }

        protected List<Sys_Privage> GetFirstLeavelDict(string code)
        {
            int mt = int.Parse(mtype);
            return list.Where(emp => emp.ParentCode == code && emp.PrivageType == mt).OrderBy(c => c.MenuIndex).ToList();
        }

        protected List<Sys_Privage> GetSecondLeavelDict(string code)
        {
            return CommonBase.GetList<Sys_Privage>("IsDeleted=0").Where(emp => emp.ParentCode == code).OrderBy(c => c.MenuIndex).ToList();
        }
    }
}