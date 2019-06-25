using DBUtility;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Web.m.app
{

    public partial class rank_detail: BasePage
    {
        protected string MTJName = string.Empty, MTJTel = string.Empty;
        protected override void SetPowerZone()
        {
            //查找推荐人
            var mtjModel = CommonBase.GetModel<Model.Member>(TModel.MTJ);
            if(mtjModel != null)
            {
                MTJName = mtjModel.MName;
                MTJTel = mtjModel.MID;
            }
            //查找导师:往上查询两级
            string sql1 = "SELECT * FROM dbo.FUN_CountUpperMemberWithRank('" + TModel.ID + "',0,2)";
            rep_my_teacher.DataSource = CommonBase.GetTable(sql1);
            rep_my_teacher.DataBind();

            //查找帮扶学员:往下查询两级
            string sql2 = "SELECT * FROM dbo.FUN_CountTDMemberWithRank('" + TModel.ID + "',0,2)";
            DataTable dt = CommonBase.GetTable(sql2);
            DataTable result = dt.Clone();
            DataRow[] drArray = dt.Select("MRANK=1", "MBD ASC");
            foreach(DataRow dr in drArray)
            {
                result.ImportRow(dr);
            }
            foreach(DataRow dr in drArray)
            {
                DataRow[] drArraTd = dt.Select("MRANK=2 and P_ID='" + dr["ID"].ToString() + "'", "MBD ASC");
                foreach(DataRow row in drArraTd)
                {
                    result.ImportRow(row);
                }
            }
            rep_td_member.DataSource = result;
            rep_td_member.DataBind();

            ////查找体验学员:直接推荐的体验会员
            //rep_member.DataSource = CommonBase.GetList<Model.Member>("MTJ='" + TModel.ID + "' AND RoleCode='Member'");
            //rep_member.DataBind();

        }
    }
}