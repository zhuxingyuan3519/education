using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Message
{
    public partial class NoticeDetail : BasePage
    {
        protected Model.Notice NoticeModel;
        protected override void SetValue(string id)
        {
            Model.Notice notice = CommonBase.GetModel<Model.Notice>(id);
            if (notice != null)
                NoticeModel = notice;
            else
                NoticeModel = new Model.Notice();
        }
    }
}