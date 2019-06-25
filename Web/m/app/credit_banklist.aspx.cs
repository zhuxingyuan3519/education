using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class credit_banklist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                repBankList.DataSource = Service.CacheService.BankList;
                if (!string.IsNullOrEmpty(Request.QueryString["code"]) && Request.QueryString["code"] == "1")
                {
                    repBankList.DataSource = Service.CacheService.BankList.Where(c => !string.IsNullOrEmpty(c.LinkUrl));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["code"]) && Request.QueryString["code"] == "3")//学习资料没有的银行就不添加
                {
                    string sql = "SELECT DISTINCT Remark FROM dbo.Notice WHERE NType=2 AND Remark IS NOT NULL";
                    DataTable dt = DBUtility.CommonBase.GetTable(sql);
                    List<Model.Sys_BankInfo> list = new List<Model.Sys_BankInfo>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(Service.CacheService.BankList.FirstOrDefault(c => c.Code == dr["Remark"].ToString()));
                    }
                    repBankList.DataSource = list;
                }

                repBankList.DataBind();
            }
        }

    }
}