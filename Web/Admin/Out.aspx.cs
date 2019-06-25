using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Web.Admin
{
    public partial class Out : BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (BllModel == null || !TModel.Role.IsAdmin)
                //{
                //    Response.Write(RetUrlStr("/Login.aspx", "/Error.aspx"));
                //}
                //else
                //{
                //    Response.Write(RetUrlStr("/Login.aspx", "/Login.aspx"));
                //}
                //if (TModel != null)
                //    Log(TModel, "1", TModel.MID + "退出系统", null, true);
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.AddHeader("Pragma", "No-Cache");
                Session["AdminMember"] = null;
                FormsAuthentication.SignOut();
                Response.Redirect("/Admin/Login.aspx");
                //RetUrlStr("/Admin/Login.aspx", "/Admin/Login.aspx");
                Response.End();
            }
        }

        private string RetUrlStr(string OutUrl, string ErrorUrl)
        {
            //if (!WebModel.WebState || !TestCloseTime())
            //{
            //    return "<script>window.top.location.href='" + ErrorUrl + "'</script>";
            //}
            //else
            {
                return "<script>window.top.location.href='" + OutUrl + "'</script>";
            }
        }

        private bool TestCloseTime()
        {
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd ");
            //foreach (string time in WebModel.OpenTimeList)
            //{
            //    string[] times = time.Split('-');
            //    if (DateTime.Parse(nowDate + times[0]) < DateTime.Now && DateTime.Parse(nowDate + times[1]) > DateTime.Now)
            //        return true;
            //}
            return false;
        }
    }
}