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
    public class VideoList: BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " IsDeleted=0 ";


            if(!string.IsNullOrEmpty(context.Request["nName"]))
            {
                strWhere += " and  t1.Title like '%" + HttpUtility.UrlDecode(context.Request["nName"]) + "%' ";
            }

            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, " t1.Code,t1.Name,t1.[Format],t1.Title,t1.Authority,t1.Size,t1.CreatedTime", "t1.CreatedTime DESC", "dbo.EN_Video t1", out count);
            dt.Columns.Add("SizeString");
            dt.Columns.Add("CutTime");

            foreach(DataRow dr in dt.Rows)
            {
                  dr["CutTime"] = Convert.ToDateTime(dr["CreatedTime"]).ToString("yyyy/MM/dd HH:mm");
               
                if(!string.IsNullOrEmpty(dr["Size"].ToString()))
                {
                    decimal dsiz = MethodHelper.ConvertHelper.ToDecimal(dr["Size"].ToString(), 0);
                    dr["SizeString"] = (dsiz / 1024 / 1024).ToString("F2") + "M";
                }

                if(!string.IsNullOrEmpty(dr["Authority"].ToString()))
                {
                    string privage = dr["Authority"].ToString();
                    string result = string.Empty;
                    string[] array = privage.Split(',');
                    foreach(string str in array)
                    {
                        Sys_Role role = CacheService.RoleList.FirstOrDefault(c => c.Code == str);
                        if(role != null)
                        {
                            result += role.Name + "；";
                        }
                    }
                    dr["Authority"] = result;
                }
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));
        }
    }
}