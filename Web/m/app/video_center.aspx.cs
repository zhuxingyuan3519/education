using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class video_center: BasePage
    {
        protected override void SetPowerZone()
        {
            //List<Model.EN_Video> listMember = CommonBase.GetList<Model.EN_Video>("Authority LIKE '%Member%'");
            string sql1 = "SELECT TOP 2 * FROM EN_Video WHERE Authority LIKE '%Member%' AND IsDeleted=0  ORDER BY CreatedTime DESC;";
            string sql2 = "SELECT TOP 4 * FROM EN_Video WHERE Authority LIKE '%VIP%' AND IsDeleted=0 ORDER BY CreatedTime DESC;";
            string sql3 = "SELECT TOP 4 * FROM EN_Video WHERE Authority LIKE '%Student%' AND IsDeleted=0 ORDER BY CreatedTime DESC;";

            DataSet ds = CommonBase.GetDataSet(sql1 + sql2 + sql3);
            DataTable dt1 = ds.Tables[0];
            foreach(DataRow row in dt1.Rows)
            {
                row["Remark"] = MethodHelper.CommonHelper.GetCutContent(row["Remark"], 18);
            }
            rep_listMember.DataSource = dt1;
            rep_listMember.DataBind();

            DataTable dt2 = ds.Tables[1];
            foreach(DataRow row in dt2.Rows)
            {
                row["Remark"] = MethodHelper.CommonHelper.GetCutContent(row["Remark"], 6);
            }
            rep_listVIP.DataSource = dt2;
            rep_listVIP.DataBind();

            DataTable dt3 = ds.Tables[2];
            foreach(DataRow row in dt3.Rows)
            {
                row["Remark"] = MethodHelper.CommonHelper.GetCutContent(row["Remark"], 6);
            }
            rep_StudentList.DataSource = dt3;
            rep_StudentList.DataBind();

        }
    }
}