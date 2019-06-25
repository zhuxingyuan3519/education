using DBUtility;
using MethodHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// TempList 的摘要说明
    /// </summary>
    public class TempList : BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            Model.Member memberModel = SessionModel;
            base.ProcessRequest(context);
            string strWhere = " IsDeleted=0";// and Company=" + memberModel.Company;

            if (!string.IsNullOrEmpty(context.Request["txtKey"]))
            {
                strWhere += " and Bank='" + context.Request["txtKey"] + "'";
            }
            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "*", "CreatedTime DESC", "CM_Template", out count);
            dt.Columns.Add("CutTime", typeof(string));
            dt.Columns.Add("MoneyToMoney", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Bank"] =GetBankName(dt.Rows[i]["Bank"].ToString());
                dt.Rows[i]["MoneyToMoney"] = dt.Rows[i]["MinMoney"] + "~" + dt.Rows[i]["MaxMoney"];
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));
            //List<Model.CM_Template> ListNotice = CommonBase.GetPageList<Model.CM_Template>(strWhere, pageIndex, pageSize, out count);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < ListNotice.Count; i++)
            //{
            //    sb.Append((i + 1) + (pageIndex - 1) * pageSize + "~");
            //    sb.Append(GetBankName(ListNotice[i].Bank) + "~");
            //    sb.Append(ListNotice[i].MinMoney + "—" + ListNotice[i].MaxMoney + "~");
            //    sb.Append(ListNotice[i].CostCount + "~");
            //    sb.Append(ListNotice[i].LeavePencent + "~");
            //    sb.Append("<input type='button' class='btn btn-sm btn-success' value='查看详细' onclick=showDetail(this,'" + ListNotice[i].Code + "') />&nbsp;");
            //    //if (!memberModel.Role.IsReadOnly)
            //    sb.Append("<input type='button' class='btn btn-sm btn-danger' value='删除' onclick=deleteArchive(this,'" + ListNotice[i].Code + "') /> ");
            //    sb.Append("≌");
            //}
            //var info = new { PageData = Traditionalized(sb), TotalCount = count };
            ////var json = new { PageData = sb.ToString(), TotalCount = count };匿名类
            //context.Response.Write(JsonConvert.SerializeObject(info));
        }
    }
}