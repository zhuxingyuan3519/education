using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.SessionState;
using Newtonsoft.Json;
using DBUtility;

namespace Web.Admin.Handler
{
    /// <summary>
    /// MemberList 的摘要说明
    /// </summary>
    public class NoticeList : BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "'1'='1' ";
            if (!string.IsNullOrEmpty(context.Request["nType"]))
            {
                strWhere += " and NType=" + context.Request["nType"] + "";
            }
            if (!string.IsNullOrEmpty(context.Request["nBank"]))
            {
                strWhere += " and Remark='" + context.Request["nBank"] + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["nCType"]))
            {
                strWhere += " and Company=" + context.Request["nCType"];
            }
            if (!string.IsNullOrEmpty(context.Request["tState"]))
            {
                strWhere += " and NState='" + context.Request["tState"] + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["nTitle"]))
            {
                strWhere += " and NTitle like '%" + HttpUtility.UrlDecode(context.Request["nTitle"]) + "%'";
            }
            int count;

            string fields = "NTitle,NCreateTime,NClicks,ID";
            System.Data.DataTable dtTbl = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, "NCreateTime desc", "dbo.Notice", out count);
            dtTbl.Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dtTbl.Rows.Count; i++)
            {
                dtTbl.Rows[i]["CutTime"] = Convert.ToDateTime(dtTbl.Rows[i]["NCreateTime"]).ToString("yyyy-MM-dd HH:mm");
            }
            context.Response.Write(MethodHelper.JsonHelper.GetAdminDataTableToJson(dtTbl, count));

            //List<Model.Notice> ListNotice = CommonBase.GetPageList<Model.Notice>(strWhere, pageIndex, pageSize, out count);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < ListNotice.Count; i++)
            //{
            //    sb.Append((i + 1) + (pageIndex - 1) * pageSize + "~");
            //    sb.Append(ListNotice[i].NTitle + "~");
            //    sb.Append(ListNotice[i].NCreateTime.ToString("yyyy-MM-dd HH:mm") + "~");
            //    sb.Append(ListNotice[i].NClicks + "~");
            //    sb.Append("<input type='button' class='btn btn-info' value='查看' onclick='toSeeDetail(" + ListNotice[i].ID + ")'/>&nbsp;<input type='button' class='btn btn-danger' value='删除' onclick='toDelete(" + ListNotice[i].ID + ")'/>" );
            //    sb.Append("≌");
            //}
            //var info = new { PageData = Traditionalized(sb), TotalCount = count };

            ////var json = new { PageData = sb.ToString(), TotalCount = count };匿名类
            //context.Response.Write(JsonConvert.SerializeObject(info));
        }
    }
}