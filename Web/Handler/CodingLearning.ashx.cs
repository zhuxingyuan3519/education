using DBUtility;
using MethodHelper;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Handler
{
    /// <summary>
    /// GetGoodList 的摘要说明
    /// </summary>
    public class CodingLearning : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string codeType = context.Request["codeType"];
            string showPart = context.Request["showPart"];//显示范围
            string codeOrder = context.Request["codeOrder"];
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);

            //提现 记录表TD_TXLog
            string strWhere = "IsDeleted=0  and CodeType='" + codeType + "'";
            if (!string.IsNullOrEmpty(showPart))
            {
                if (codeType == "3")
                {
                    strWhere += " and Remark= '" + showPart + "'";
                }
                else
                {
                    strWhere += " and PKCode like '" + showPart + "%'";
                }
            }
            int count;
            string orderBy = "Sort asc";
            if (codeOrder == "2")
            { orderBy = "NEWID()"; }
             else if (codeOrder == "3")//倒叙显示
            { orderBy = "Sort desc"; }
            DataTable dt = CommonBase.GetPageDataTable(currentIndex, pageSize, strWhere, "PKCode,CodeName,CodeImage,PuKeNum", orderBy, "T_CodingMemory", out count);
            //for(int i = 0; i < dt.Rows.Count; i++)
            //{
            //    switch(dt.Rows[i]["Status"].ToString())
            //    {
            //        case "1": dt.Rows[i]["Remark"] = "提交申请"; break;
            //        case "2": dt.Rows[i]["Remark"] = "已转账"; break;
            //        case "3": dt.Rows[i]["Remark"] = "已转账"; break;
            //    }
            //    dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["ApplyTXDate"]).ToString("yy/MM/dd");
            //}
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));
        }
    }
}