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
    public class GetInDetailPageList : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            context.Response.ContentType = "text/plain";
            string pageIndex = context.Request["pageIndex"];
            string paSize = context.Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string type = context.Request["type"];
            string nRegistBeginTime = context.Request["nRegistBeginTime"];
            string nRegistEndTime = context.Request["nRegistEndTime"];
            string ddl_type = context.Request["ddl_type"];
            string strWhere = "t1.IsDeleted=0 and t1.Status=1 and t1.MID='" + SessionModel.ID + "' ";
            if (SessionModel.RoleCode == "Member" || SessionModel.RoleCode == "VIP")
            {
                if (SessionModel.NoFHPool == "0" || string.IsNullOrEmpty(SessionModel.NoFHPool))
                    strWhere += " and 1>2 ";
            }
            if (!string.IsNullOrEmpty(nRegistBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistBeginTime) + " 00:00:00", out time);
                strWhere += " and t1.FHDate>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(nRegistEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistEndTime) + " 23:59:59", out time);
                strWhere += " and t1.FHDate<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (!string.IsNullOrEmpty(ddl_type))
            {
                strWhere += " and t1.SHMoneyCode='" + ddl_type + "'";
            }

            //if(type == "wh") //维护奖励查询
            //{
            //    strWhere += " and (t1.SHMoneyCode='Return' or t1.SHMoneyCode='KgjUsing' or t1.SHMoneyCode='ServiceUsing'  or t1.SHMoneyCode='CompanyUsing')";
            //}
            //else if(type == "fw")//服务奖励查询
            //{
            //    strWhere += " and (t1.SHMoneyCode='Service')";
            //}
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, "t1.FHType,t1.PayCode,t1.FHDate,t1.FHMoney,t1.FHRoleCode,t1.Remark,t2.MID as PayMID,t1.SHMoneyCode", "t1.FHDate desc", "TD_FHLog t1 left join Member t2 on t1.PayCode=t2.ID");
            dt.Tables[0].Columns.Add("CutTime", typeof(string));
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                string bank = dt.Tables[0].Rows[i]["FHRoleCode"].ToString();
                var role = CacheService.RoleList.Where(c => c.Code == bank).FirstOrDefault();
                if (role != null)
                    dt.Tables[0].Rows[i]["FHRoleCode"] = role.Name;

                int bankType = MethodHelper.ConvertHelper.ToInt32(dt.Tables[0].Rows[i]["SHMoneyCode"].ToString(), 0);
                if(bankType == 999)
                {
                    dt.Tables[0].Rows[i]["FHType"] = "被推荐人增购名额";
                }
                else
                {
                    TD_SHMoney shmoney = CacheService.SHMoneyList.Where(c => c.Id == bankType).FirstOrDefault();
                    if(shmoney != null)
                        dt.Tables[0].Rows[i]["FHType"] = shmoney.Remark;
                }
                dt.Tables[0].Rows[i]["CutTime"] = MethodHelper.ConvertHelper.ToDateTime(dt.Tables[0].Rows[i]["FHDate"], DateTime.Now).ToString("yyyy-MM-dd HH:mm");
            }
            context.Response.Write(JsonHelper.DataSetToJson(dt));
        }

        protected string ReplaceHtml(string content)
        {
            return content.Replace("\"", "'").Replace("{", "|").Replace("{", "|").Replace(" ", "").Replace("\n", "");
        }

    }
}