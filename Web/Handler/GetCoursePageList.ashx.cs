using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;

namespace Web.Handler
{
    /// <summary>
    /// GetPageList 的摘要说明
    /// </summary>
    public class GetCoursePageList: BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = "IsDeleted=0";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "Code,Name,Title,Remark,Fee", "Code desc", "EN_Course");
            foreach(DataRow dr in dt.Tables[0].Rows)
            {
                //获取到一张图片
                string remark = dr["Remark"].ToString();
                ArrayList list = MethodHelper.CommonHelper.GetImgUrl(remark);
                string src = "/m/image/bookpicker.png";
                if(list.Count > 0)
                {
                    src = list[0].ToString();
                }
                dr["Remark"] = src;
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }



        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}