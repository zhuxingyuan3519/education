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
    public class HirePurchaseDetail : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(context.Request["id"]))
            {
                strWhere += " and HirePurchaseId = '" + context.Request["id"] + "'";
            }

            int count;
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "*", "CreatedTime asc", "SH_HirePurchase", out count);
            dt.Columns.Add("CutTime", typeof(string));
            dt.Columns.Add("RealCutTime", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["CreatedTime"]).ToString("yyyy/MM/dd");
                //if (!string.IsNullOrEmpty(dt.Rows[i]["HireType"].ToString()))
                //{

                dt.Rows[i]["RoleCode"] = CacheService.RoleList.FirstOrDefault(c => c.Code == dt.Rows[i]["RoleCode"].ToString()).Name;
                //}
                if (!string.IsNullOrEmpty(dt.Rows[i]["Remark"].ToString()))
                {
                    //获取到城市
                    Model.Sys_StandardArea address = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == dt.Rows[i]["Remark"].ToString());
                    //if (address.LevelInt == 40)
                    dt.Rows[i]["Remark"] = GetAddressName(address.ProCode) + GetAddressName(address.CityCode) + GetAddressName(address.AdCode);
                }

            }
            context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }
    }
}