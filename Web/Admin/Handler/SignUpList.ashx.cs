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
    /// SignUpList 的摘要说明
    /// </summary>
    public class SignUpList : BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " 1=1 ";

            //if (!string.IsNullOrEmpty(context.Request["nTitle"]))
            //{
            //    strWhere += " and t1.MID like '%" + HttpUtility.UrlDecode(context.Request["nTitle"]) + "%'";
            //}
            if (!string.IsNullOrEmpty(context.Request["nName"]))
            {
                strWhere += " and ( t1.MName like '%" + HttpUtility.UrlDecode(context.Request["nName"]) + "%' or  t1.MID like '%" + HttpUtility.UrlDecode(context.Request["nName"]) + "%' )";
            }
            if (!string.IsNullOrEmpty(context.Request["ddlTraing"]))
            {
                strWhere += " and t1.TrainingCode='" + context.Request["ddlTraing"] + "' ";
            }
            if (!string.IsNullOrEmpty(context.Request["ddlCourse"]))
            {
                strWhere += " and t1.CourseCode='" + context.Request["ddlCourse"] + "' ";
            }
            if (!string.IsNullOrEmpty(context.Request["nRegistBeginTime"]))
            {
                strWhere += " and DATEDIFF(dd,t1.SignDate,'" + context.Request["nRegistBeginTime"] + "')<=0";
            }
            if (!string.IsNullOrEmpty(context.Request["nRegistEndTime"]))
            {
                strWhere += " and DATEDIFF(dd,t1.SignDate,'" + context.Request["nRegistEndTime"] + "')>=0";
            }
            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, " t1.Code,t1.MCode,t2.MID,t1.Fee,t2.MName,t2.Tel,t3.Name as  CourseName, t1.SignDate,t1.CourseCode,t1.TrainingCode,t4.MID AS TrainMID,t4.MName AS TrainName", "t1.SignDate DESC", "dbo.EN_SignUp t1 LEFT JOIN dbo.Member t2 ON t1.MCode=t2.ID LEFT JOIN dbo.EN_Course t3 ON t1.CourseCode=t3.Code LEFT JOIN dbo.Member t4 ON t1.TrainingCode=t4.ID", out count);
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }
    }
}