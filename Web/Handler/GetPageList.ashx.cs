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
    public class GetPageList : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string type = context.Request["type"];//1-首页显示全部;0-查询页显出
            switch (type)
            {
                case "news": result = getIndexList(context); break;
                case "active": result = getActiveList(context); break;
                case "list": result = getListList(context); break;
                case "commentList": result = getCommentList(context); break;
            }
            context.Response.Write(result);
        }
        protected string getCommentList(HttpContext context)
        {
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string cid = context.Request["cid"];//类型的Id
            string strWhere = "IsDeleted=0 and ParentCode='"+cid+"'";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "*", "CreatedTime desc", "W_Commit");
            return JsonHelper.DataSetToJson(dt);
        }

        protected string getListList(HttpContext context)
        {
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string cid = context.Request["cid"];//类型的Id
            string label = context.Request["label"];
            string keys = context.Request["keys"];
            string strWhere = "IsDeleted=0 ";
            if (!string.IsNullOrEmpty(label))
            {
                strWhere += " and Label like '%" + HttpUtility.UrlDecode(label) + "%'";
            }
            else if (!string.IsNullOrEmpty(keys))
            {
                strWhere += " and Keys like '%" + HttpUtility.UrlDecode(keys) + "%'";
            }
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "*", "CreatedTime desc", "W_Content");
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                string content = CommonHelper.GetCutContent(dt.Tables[0].Rows[i]["Contents"].ToString(), 50);
                dt.Tables[0].Rows[i]["Contents"] = ReplaceHtml(content);
                dt.Tables[0].Rows[i]["Label"] = GetLabelValue(dt.Tables[0].Rows[i]["Label"]);
            }
            return JsonHelper.DataSetToJson(dt);
        }

        protected string getActiveList(HttpContext context)
        {
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string cid = context.Request["cid"];//类型的Id
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, "IsDeleted=0 and Title<>''", "Id,Title,CreatedTime", "CreatedTime desc", "W_ShopActive");
            //for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            //{
            //    //dt.Tables[0].Rows[i]["CreatedTime"] = Convert.ToDateTime(dt.Tables[0].Rows[i]["CreatedTime"]).ToString("yyyy-MM-dd");
            //}
            return JsonHelper.DataSetToJson(dt);
        }
        protected string getIndexList(HttpContext context)
        {
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string cid = context.Request["cid"];//类型的Id
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, "IsDeleted=0 and Title<>''", "Id,Title,CreatedTime", "CreatedTime desc", "W_Content");
            //for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            //{
                //dt.Tables[0].Rows[i]["CreatedTime"] = Convert.ToDateTime(dt.Tables[0].Rows[i]["CreatedTime"]).ToString("yyyy-MM-dd");
            //}
            return JsonHelper.DataSetToJson(dt);
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}