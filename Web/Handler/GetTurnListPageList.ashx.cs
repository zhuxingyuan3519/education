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
    public class GetTurnListPageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string type = context.Request["type"];
            string strWhere = "t1.IsDeleted=0 and t1.FromCompany='" + SessionModel.ID + "' ";

            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "t1.CreatedTime,t1.CostCount,t1.Remark,t2.MID,t2.MName", "t1.CreatedTime desc", "CM_CompanyPointCost t1 left join Member t2 on t1.ToCompany=t2.ID");
            dt.Tables[0].Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {

                dt.Tables[0].Rows[i]["CutTime"] = MethodHelper.ConvertHelper.ToDateTime(dt.Tables[0].Rows[i]["CreatedTime"], DateTime.Now).ToString("yyyy-MM-dd HH:mm");
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}