using DBUtility;
using Model;
using System;
using System.Data;

namespace Web.m.app
{
    public partial class user_change : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //获取到这个微信登录的信息
                M_WXUserInfo wx_user = Session["WXMember"] as M_WXUserInfo;
                
                //绑定已有账号
                string sql = "select t1.OpenId,t2.ID,t2.MID,t2.MName,t2.Tel from M_WX_VS_User t1 left join Member t2 on t1.UserCode=t2.ID where t1.IsDeleted=0 and t1.OpenId='" + wx_user.OpenId + "'";
                DataTable dt = CommonBase.GetTable(sql);
                dt.Columns.Add("CurrentStatus");
                if (Session["Member"] != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["ID"].ToString() == TModel.ID)
                        {
                            row["CurrentStatus"] = "√";
                        }
                    }
                }

                rep_bindList.DataSource =dt;
                rep_bindList.DataBind();

            }
        }
    }
}