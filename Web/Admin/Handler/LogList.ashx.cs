using DBUtility;
using MethodHelper;
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
    /// PayLogList 的摘要说明
    /// </summary>
    public class LogList: BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "IsDeleted=0 ";
            if(!string.IsNullOrEmpty(context.Request["nMID"]))
            {
                strWhere += " and MCode like '%" + context.Request["nMID"] + "%'";
            }
            if(!string.IsNullOrEmpty(context.Request["ddlLogType"]))
            {
                strWhere += " and LType='" + context.Request["ddlLogType"] + "'";
            }
            if(!string.IsNullOrEmpty(context.Request["nPayBeginTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayBeginTime"]) + " 00:00:00", out time);
                strWhere += " and LogDate>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if(!string.IsNullOrEmpty(context.Request["nPayEndTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayEndTime"]) + " 23:59:59", out time);
                strWhere += " and LogDate<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            //代理商只能看到自己名下的会员（以后再加）
            if(TModel.RoleCode == "Admin")
            {
                strWhere += " and (MID in (select ID FROM Member where Company=" + SessionModel.ID + ") or MID=" + SessionModel.ID + ")";
            }
            if(!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
            {
                strWhere += " and (MID in (select ID FROM Member where Agent=" + SessionModel.ID + ") or MID=" + SessionModel.ID + ")";
            }

            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "*", "LogDate DESC", "DB_Log", out count);
            dt.Columns.Add("CutTime", typeof(string));
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["LogDate"]).ToString("yyyy/MM/dd HH:mm");
                Sys_Role role = CacheService.RoleList.Where(c => c.Code == dt.Rows[i]["OperatorRole"].ToString()).FirstOrDefault();
                if(role != null)
                {
                    dt.Rows[i]["OperatorRole"] = role.Name;
                }
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));
        }
    }
}