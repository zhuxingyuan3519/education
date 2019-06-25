using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Admin;
using Model;

using System.Text;
using MethodHelper;
using Newtonsoft.Json;
using DBUtility;
using System.Data;
using Service;

namespace Web.Admin.Handler
{
    /// <summary>
    /// CourseList 的摘要说明
    /// </summary>
    public class CourseList : BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " IsDeleted=0 ";

        
            if (!string.IsNullOrEmpty(context.Request["nName"]))
            {
                strWhere += " and  t1.Name like '%" + HttpUtility.UrlDecode(context.Request["nName"]) + "%' ";
            }
        
            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, " t1.Code,t1.Name,t1.Fee,t1.Leavel", "t1.Sort ASC", "dbo.EN_Course t1", out count);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Leavel"].ToString() == "1")
                {
                    dr["Leavel"] = "是";
                }
                else
                    dr["Leavel"] = "否";
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }
    }
}