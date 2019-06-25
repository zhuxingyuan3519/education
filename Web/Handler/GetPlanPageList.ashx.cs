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
    public class GetPlanPageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = "IsDeleted=0 and DATEDIFF(dd,PlanEndDate,GETDATE())<=0 and Company=" + SessionModel.ID;
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "Bank,CardID,Years,Months,Code,PlanBeginDate,PlanEndDate", "Years desc,Months desc", "CM_PlanHeader");
            dt.Tables[0].Columns.Add("PlanBeginDateString",typeof(string));
            dt.Tables[0].Columns.Add("PlanEndDateString", typeof(string));
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                dt.Tables[0].Rows[i]["PlanBeginDateString"] = MethodHelper.ConvertHelper.ToDateTime(dt.Tables[0].Rows[i]["PlanBeginDate"], DateTime.Now).ToString("MM-dd");
                dt.Tables[0].Rows[i]["PlanEndDateString"] = MethodHelper.ConvertHelper.ToDateTime(dt.Tables[0].Rows[i]["PlanEndDate"], DateTime.Now).ToString("MM-dd"); 
                string bank = dt.Tables[0].Rows[i]["Bank"].ToString();
                dt.Tables[0].Rows[i]["Bank"] = "Attachment/" + CacheService.BankList.Where(c => c.Code == bank).FirstOrDefault().PicUrl;
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

        public string GetDictValue(string code, string parentCode)
        {
            return DictService.GetDictValue(code, parentCode);
        }
        protected string GetLabelValue(object labels)
        {
            string result = string.Empty;
            string label = labels.ToString();
            if (!string.IsNullOrEmpty(label))
            {
                string[] array = label.Split(',');
                foreach (string str in array)
                {
                    if (!string.IsNullOrEmpty(str))
                        result += GetDictValue(str, "Label") + "/";
                }
                if (!string.IsNullOrEmpty(result))
                    return result.Substring(0, result.LastIndexOf('/'));
            }
            return string.Empty;
        }
    }
}