using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Message
{
    public partial class MessageDetail : BasePage
    {
        protected override void SetPowerZone()
        {
            string mcode = Request.QueryString["mcode"];
            if (!string.IsNullOrEmpty(mcode))
            {
                DB_Message_Model modelMessage = CommonBase.GetModel<DB_Message_Model>(mcode);
                if (modelMessage != null)
                {
                    divSender.InnerHtml = modelMessage.SendName + "：" + modelMessage.CreatedTime.ToString();
                    divMContent.InnerHtml = modelMessage.Message;
                }
                //绑定回复列表
                rep_responseMsgList.DataSource = CommonBase.GetList<DB_ResponseMessage_Model>("IsDeleted=0 and Message<>'' and RMcode='" + mcode + "'  Order by CreatedTime asc");
                rep_responseMsgList.DataBind();
            }
        }
    }
}