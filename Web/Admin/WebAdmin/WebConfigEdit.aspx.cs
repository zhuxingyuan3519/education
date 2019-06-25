using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.WebAdmin
{
    public partial class WebConfigEdit : BasePage
    {

        protected override void SetPowerZone()
        {
            //组件html
            List<Sys_WebConfig> listConfig = CommonBase.GetList<Sys_WebConfig>("IsCommonSet=1 and Status=1 order by Sort");
            StringBuilder sb = new StringBuilder();
            foreach (Sys_WebConfig config in listConfig)
            {
                sb.Append("<div class=\"listContent\">");
                sb.Append(config.Remark + "：");
                if (config.IsImg == 1) //img标签
                {
                    sb.Append("<input type=\"button\" data-field=\"" + config.Code + "\" value=\"上传图片\"  class=\"btn btn-info confiTbtn\" title=\"btnUpload\" onclick=\"showUpload(this,'" + config.Code + "');\"/>");
                    sb.Append("<input type=\"hidden\"  value=\"" + config.Value + "\" class=\"hidimg-" + config.Code + "\" name=\"configNode_" + config.Code + "\" />");
                    sb.Append("<br/><img src=\"" + config.Value + "\"  style=\"" + (string.IsNullOrEmpty(config.Value) ? "" : "height:250px") + "\"  class=\"uploadedimg img-" + config.Code + "\"/>");
                }
                else if (config.IsImg == 2) //单选按钮
                {
                    sb.Append("<br/><input type=\"radio\" name=\"configNode_" + config.Code + "\" value=\"1\"   " + returnCheckString("1", config.Value) + " />是&emsp;<input type=\"radio\" name=\"configNode_" + config.Code + "\" value=\"0\"   " + returnCheckString("0", config.Value) + "/>否 ");
                }
                else if (config.IsImg == 3) //文本输入框
                {
                    sb.Append("<br/><input type=\"text\" name=\"configNode_" + config.Code + "\" value=\"" + config.Value + "\"  />");
                }
                else //文本域（多行输入）
                    sb.Append("<br/><textarea  name=\"configNode_" + config.Code + "\" class=\"confiTextarea\">" + config.Value + "</textarea>");
                sb.Append("</div>");
            }
            contentDiv.InnerHtml = sb.ToString();
        }
        protected string returnCheckString(string radioVal, string configVal)
        {
            if (radioVal.Trim() == configVal.Trim())
                return "checked=\"checked\"";
            else
                return "";
        }

        protected override string btnAdd_Click()
        {
            //Model.Sys_WebConfig config = null;
            List<CommonObject> listComm = new List<CommonObject>();

            List<Sys_WebConfig> listConfig = CommonBase.GetList<Sys_WebConfig>("IsCommonSet=1 and Status=1");
            foreach (Sys_WebConfig config in listConfig)
            {
                string contentValue = Request["configNode_" + config.Code];
                if (!string.IsNullOrEmpty(contentValue))
                {
                    config.Value = contentValue;
                    CommonBase.Update<Sys_WebConfig>(config, new string[] { "Value" }, listComm);
                }
            }

            if (CommonBase.RunListCommit(listComm))
            {
                //更新缓村
                CacheHelper.RemoveAllCache("Sys_WebConfig");
                return "1";
            }
            else
                return "0";
        }
    }
}