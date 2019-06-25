
using DBUtility;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// MessageList 的摘要说明
    /// </summary>
    public class MessageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "'1'='1' and IsDeleted=0 and SendCode='" + SessionModel.ID + "'";
            if (!string.IsNullOrEmpty(context.Request["remark"]))
            {
                strWhere = "'1'='1' and IsDeleted=0  and Remark='" + context.Request["remark"] + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["nTitle"]))
            {
                strWhere += " and SendName like '%" + HttpUtility.UrlDecode(context.Request["nTitle"] + "%'");
            }
         
            if (!string.IsNullOrEmpty(context.Request["nName"]))
            {
                strWhere += " and ReceiveName like '%" + HttpUtility.UrlDecode(context.Request["nName"] + "%'");
            }
            int count;
            string fields = "*";
            System.Data.DataTable dtTbl = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, "CreatedTime desc", "dbo.DB_Message", out count);
            dtTbl.Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dtTbl.Rows.Count; i++)
            {
                dtTbl.Rows[i]["CutTime"] = Convert.ToDateTime(dtTbl.Rows[i]["CreatedTime"]).ToString("yyyy-MM-dd HH:mm");
                dtTbl.Rows[i]["Message"] = MethodHelper.CommonHelper.GetCutContent(dtTbl.Rows[i]["Message"].ToString(), 20);
            }
            context.Response.Write(MethodHelper.JsonHelper.GetAdminDataTableToJson(dtTbl, count));

            //List<DB_Message_Model> List = CommonBase.GetPageList<DB_Message_Model>(strWhere, pageIndex, pageSize, out count);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < List.Count; i++)
            //{
            //    sb.Append(MethodHelper.CommonHelper.GetCutContent(List[i].Message, 20) + "~");
            //    sb.Append(List[i].CreatedTime.ToString() + "~");
            //    sb.Append(List[i].SendName + "~");
            //    sb.Append(List[i].ReceiveName + "~");
            //    sb.Append("<a href='javascript:seeNotice(\"" + List[i].Code + "\")'  class='btn btn-info tablecontrol'  role='button'>查看</a>&nbsp;<a href='javascript:deleteNotice(\"" + List[i].Code + "\")'  class='btn btn-danger tablecontrol'  role='button'>删除</a>");
            //    sb.Append("≌");
            //}
            //var info = new { PageData = Traditionalized(sb), TotalCount = count };
            //context.Response.Write(JsonConvert.SerializeObject(info));
        }
    }
}