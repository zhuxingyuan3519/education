using DBUtility;
using Model;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// FHLogList 的摘要说明
    /// </summary>
    public class CardList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "'1'='1' and IsDeleted=0 and (LinkUrl<>'' and LinkUrl is not NULL )";
            if (!string.IsNullOrEmpty(context.Request["txtKey"]))
            {
                strWhere += " and ID=" + context.Request["txtKey"];
            }
            int count;
            string fields = "Remark,LinkUrl,Id";
            System.Data.DataTable dtTbl = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, "Code asc", "dbo.Sys_BankInfo", out count);
            context.Response.Write(MethodHelper.JsonHelper.GetAdminDataTableToJson(dtTbl, count));


            //List<Sys_BankInfo> list = CommonBase.GetPageList<Sys_BankInfo>(strWhere, pageIndex, pageSize, out count);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    sb.Append(list[i].Remark + "~");
            //    sb.Append(list[i].LinkUrl + "~");

            //    sb.Append("<input type='button' class='btn btn-info' value='修改' onclick='seeDetail(" + list[i].Id + ")'/>&nbsp;<input type='button' class='btn btn-danger' value='删除' onclick='deleteArchive(" + list[i].Id + ",this)'/>");
            //    sb.Append("≌");
            //}
            //var info = new { PageData = Traditionalized(sb), TotalCount = count };

            //context.Response.Write(JsonConvert.SerializeObject(info));
        }
    }
}