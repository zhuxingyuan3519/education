using DBUtility;
using Model;
using Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class role_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if(Session["Member"] != null)
                {
                     Model.Member loginUserModel = Session["Member"] as Model.Member;
                    if(loginUserModel!=null)
                    {
                        spRoleName.InnerHtml = loginUserModel.Role.Name;
                    }
                }

                List<Sys_Role> listRole = CommonBase.GetList<Sys_Role>("Code in ('1F','2F','3F') order by RIndex asc");

                foreach (Sys_Role role in listRole)
                {
                    //获取到一张图片
                    string remark = role.Introduce;
                    ArrayList list = MethodHelper.CommonHelper.GetImgUrl(remark);
                    string src = "/m/image/bookpicker.png";
                    if (list.Count > 0)
                    {
                        src = list[0].ToString();
                    }
                    string title = MethodHelper.CommonHelper.ReplaceHtmlTag(role.Introduce, 10);
                    role.Company = title;
                    role.Introduce = src;
                    if(role.Code == "VIP")
                        role.Status = 3514;
                    else if(role.Code == "1F")
                        role.Status = 276;
                    else if(role.Code == "2F")
                        role.Status = 48;
                    else if (role.Code == "3F")
                        role.Status = 29;
                }

                rep_list.DataSource = listRole;
                rep_list.DataBind();
            }
        }
    }
}