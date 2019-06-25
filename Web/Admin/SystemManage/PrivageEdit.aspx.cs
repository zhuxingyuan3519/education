using Bll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.SystemManage
{
    public partial class PrivageEdit : BasePage
    {
        protected List<Sys_Menu_Model> list;
        protected string mtype;
        protected override void SetPowerZone()
        {
            list = Sys_Menu_Bll.GetList("IsDeleted=0");
            string code = Request.QueryString["txtcode"], name = Request.QueryString["txtname"];
            mtype = Request.QueryString["mtype"];
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name))
                list = list.Where(c => c.Name.Contains(code) && c.Name.Contains(name) && c.ParentCode == "0").ToList();
            else if (!string.IsNullOrEmpty(code))
                list = list.Where(c => c.Name.Contains(code) && c.ParentCode == "0").ToList();
            else if (!string.IsNullOrEmpty(name))
                list = list.Where(c => c.Name.Contains(name) && c.ParentCode == "0").ToList();
        }

        protected List<Sys_Menu_Model> GetFirstLeavelDict(string code)
        {
            return list.Where(emp => emp.ParentCode == code && emp.Field1 == mtype).OrderBy(c => c.MenuIndex).ToList();
        }

        protected List<Sys_Menu_Model> GetSecondLeavelDict(string code)
        {
            return Sys_Menu_Bll.GetList("IsDeleted=0").Where(emp => emp.ParentCode == code).OrderBy(c => c.MenuIndex).ToList();
        }
    }
}