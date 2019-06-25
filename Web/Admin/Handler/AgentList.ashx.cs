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
    /// MemberList 的摘要说明
    /// </summary>
    public class AgentList: BaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " 1=1  and (RoleCode ='1F' OR RoleCode ='2F' OR RoleCode ='3F')";
            if(!string.IsNullOrEmpty(context.Request["nMID"]))
            {
                strWhere += " and MID LIKE '%" + context.Request["nMID"] + "%' ";
            }

            if(!string.IsNullOrEmpty(context.Request["nMName"])) //查看某个管理员名下的管理员 ，穿值为某个管理员的Id
            {
                strWhere += " and MName LIKE '%" + context.Request["nMName"] + "%' ";
            }

            if(!string.IsNullOrEmpty(context.Request["nTitle"]))
            {
                strWhere += " and Branch = '" + context.Request["nTitle"] + "'";
            }

            if (!string.IsNullOrEmpty(context.Request["ddlRoleCode"]))
            {
                strWhere += " and RoleCode = '" + context.Request["ddlRoleCode"] + "'";
            }

            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "*", "MCreateDate DESC", "Member", out count);
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Province"] = Service.AddressService.GetAddressByAdCode(dt.Rows[i]["Province"].ToString()).Name + Service.AddressService.GetAddressByAdCode(dt.Rows[i]["City"].ToString()).Name + Service.AddressService.GetAddressByAdCode(dt.Rows[i]["Zone"].ToString()).Name+ dt.Rows[i]["Address"].ToString();
                dt.Rows[i]["Learns"] = Convert.ToDateTime(dt.Rows[i]["MCreateDate"]).ToString("yyyy/MM/dd");
                dt.Rows[i]["RoleCode"] = CacheService.RoleList.FirstOrDefault(c => c.Code == dt.Rows[i]["RoleCode"].ToString()).Name;
            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }
    }
}