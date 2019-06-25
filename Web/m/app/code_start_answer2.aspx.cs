using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace Web.m.app
{
    /// <summary>
    /// 开始答题页面
    /// </summary>
    public partial class code_start_answer2 : BasePage
    {
        protected string ErrMsg = string.Empty;
        protected override void SetPowerZone()
        {
            //获取到traincode
            string traincode = Request.QueryString["traincode"];
            T_TrainHeader header = CommonBase.GetModel<T_TrainHeader>(traincode);
            if (header != null)
            {
                //if (header.CodeType == "1")
                //    sp_tip.InnerHtml = "词语";
                //else if (header.CodeType == "2")
                //    sp_tip.InnerHtml = "数字";
                //else if (header.CodeType == "4")
                //    sp_tip.InnerHtml = "字母";
                header.AnswerBeginTime = DateTime.Now.ToString();
                hid_trainCode.Value = header.Code;
                CommonBase.Update<T_TrainHeader>(header, new string[] { "AnswerBeginTime" });
                int count;
                DataTable dt = CommonBase.GetPageDataTable(1, 10000, "TrainCode='" + header.Code + "'", "CodeName,Sort", "Sort asc", "T_TrainDetail", out count);
                hid_codevalue.Value = MethodHelper.JsonHelper.DataTableToJson(dt, "CodeNames");

            }
            else
            {
                ErrMsg = "初始化记忆结果失败";
            }
        }
    }
}