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
    /// FHLogList 的摘要说明
    /// </summary>
    public class FHLogList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "'1'='1' and t1.IsDeleted=0 ";

            if (!string.IsNullOrEmpty(context.Request["nPayBeginTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayBeginTime"]) + " 00:00:00", out time);
                strWhere += " and t1.FHDate>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["nPayEndTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nPayEndTime"]) + " 23:59:59", out time);
                strWhere += " and t1.FHDate<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(context.Request["type"])) //是否是查询奖金池分红
            {
                strWhere += " and  charindex('P-',t1.FHType)=1";
                if (TModel.RoleCode != "Manage" && TModel.RoleCode != "Admin")
                    strWhere += " and t1.MID=" + TModel.ID;
            }
            if (!string.IsNullOrEmpty(context.Request["nMID"])) //会员昵称
            {
                strWhere += " and t1.FHMCode like '%" + context.Request["nMID"] + "%'";
            }

            //if(TModel.RoleCode == "Admin")
            //{
            //    strWhere += " and (t1.MID in (select ID FROM Member where Company=" + SessionModel.ID + ") or t1.MID='" + SessionModel.ID + "')";
            //}
            //if(!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
            //{
            //    strWhere += " and (t1.MID in (select ID FROM Member where Agent=" + SessionModel.ID + ") or t1.MID='" + SessionModel.ID + "')";
            //}
            int count;

            string export = context.Request["export"];
            if (export == "1")
            {
                pageIndex = 1;
                pageSize = int.MaxValue;
            }
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "t1.*,t2.MName,t3.MName as PayName", "t1.FHDate DESC", "TD_FHLog t1 left join Member t2 on t1.MID=t2.ID  left join Member t3 on t1.PayCode=t3.ID", out count);
            dt.Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["FHDate"]).ToString("yyyy/MM/dd HH:mm");
                //dt.Rows[i]["FHRoleCode"] = CacheService.RoleList.Where(c => c.Code == dt.Rows[i]["FHRoleCode"].ToString()).FirstOrDefault().Name;
            }
            //是否导出excel
            if (export == "1")
            {
                Dictionary<string, string> listFields = new Dictionary<string, string>();
                listFields.Add("RowNumber", "序号");
                listFields.Add("FHMCode", "账号");
                listFields.Add("MName", "姓名");
                listFields.Add("FHMoney", "奖励金额");
                listFields.Add("CutTime", "奖励时间");
                listFields.Add("FHType", "支付方式");
                listFields.Add("Remark", "奖励事由");
                ExportExcel(context, "用户奖励列表", dt, listFields);
            }
            else
                context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }


    }
}