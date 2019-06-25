using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web.Handler
{
    /// <summary>
    /// GetPageList 的摘要说明
    /// </summary>
    public class GetMtTjPageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = " MTJ='" + SessionModel.ID+"'";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "ID,MName,MID,MCreateDate,'' MCount", "MCreateDate desc", "Member");
            dt.Tables[0].Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                //再计算每一个被推荐人的推荐人数
                string sqlCount = "SELECT COUNT(1) FROM dbo.Member WHERE MTJ='" + dt.Tables[0].Rows[i]["ID"] + "'";
                dt.Tables[0].Rows[i]["MCount"] = CommonBase.GetSingle(sqlCount).ToString();
                string bank = dt.Tables[0].Rows[i]["MCreateDate"].ToString();
                dt.Tables[0].Rows[i]["CutTime"] = Convert.ToDateTime(bank).ToString("MM/dd");
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}