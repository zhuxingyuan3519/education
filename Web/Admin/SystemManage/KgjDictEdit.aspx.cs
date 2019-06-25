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
    public partial class KgjDictEdit : BasePage
    {
        protected List<Model.CM_Dict> list;
        protected override void SetPowerZone()
        {
            list = CommonBase.GetList<CM_Dict>("IsDeleted=0");
            string code = Request.QueryString["txtcode"], name = Request.QueryString["txtname"];
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name))
                list = list.Where(c => c.Code.ToUpper().Contains(code.ToUpper()) && c.Name.ToUpper().Contains(name.ToUpper()) && c.ParentCode == "0").ToList();
            //list = YG_DictBll.GetList("Code like '%" + code + "%' and Name like '%" + name + "%' and ParentCode='0' and IsDeleted=0");
            else if (!string.IsNullOrEmpty(code))
                list = list.Where(c => c.Code.ToUpper().Contains(code.ToUpper()) && c.ParentCode == "0").ToList();
            //list = YG_DictBll.GetList(" Code like '%" + code + "%' and ParentCode='0' and IsDeleted=0");
            else if (!string.IsNullOrEmpty(name))
                list = list.Where(c => c.Name.ToUpper().Contains(name.ToUpper()) && c.ParentCode == "0").ToList();
            //list = YG_DictBll.GetList(" Name like '%" + name + "%' and ParentCode='0' and IsDeleted=0");
            //else
            //    list = YG_DictBll.GetList("IsDeleted=0");
        }

        protected List<Model.CM_Dict> GetFirstLeavelDict(string code)
        {
            return list.Where(emp => emp.ParentCode == code).ToList();
        }

        protected List<Model.CM_Dict> GetSecondLeavelDict(string code)
        {
            return CommonBase.GetList<CM_Dict>("IsDeleted=0").Where(emp => emp.ParentCode == code).ToList();
        }
        //protected override string btnAdd_Click()
        //{
        //    string code = Request.Form["txtcode"], name = Request.Form["txtname"];
        //    if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name))
        //        list = YG_DictBll.GetList("Code like '%" + code + "%' and Name like '%" + name + "%' and ParentCode='0' and IsDeleted=0");
        //    else if (!string.IsNullOrEmpty(code))
        //        list = YG_DictBll.GetList(" Code like '%" + code + "%' and ParentCode='0' and IsDeleted=0");
        //    else if (!string.IsNullOrEmpty(name))
        //        list = YG_DictBll.GetList(" Name like '%" + name + "%' and ParentCode='0' and IsDeleted=0");
        //    return "";
        //}
    }
}