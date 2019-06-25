using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MethodHelper;

namespace Web.m.app
{
    public partial class student_list: BasePage
    {

        protected override string btnQuery_Click()
        {
            string pageIndex = Request["pageIndex"];
            string paSize = Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = "t1.Company='" + TModel.ID + "'";

            string nRegistBeginTime = Request["nRegistBeginTime"];
            string nRegistEndTime = Request["nRegistEndTime"];
            if(!string.IsNullOrEmpty(nRegistBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistBeginTime) + " 00:00:00", out time);
                strWhere += " and t1.MCreateDate>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if(!string.IsNullOrEmpty(nRegistEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistEndTime) + " 23:59:59", out time);
                strWhere += " and t1.MCreateDate<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }


            string tables = "Member t1 left join Member t2 on t1.MTJ=t2.ID";
            string fields = "t1.ID,t1.MID,t1.MName,t1.Tel,t2.ID MTJID,t2.MID MTJMID,t2.MName MTJMName";
            string orderBy = "t1.MCreateDate DESC";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, fields, orderBy, tables);
            return JsonHelper.DataSetToJson(dt);
        }
    }

}