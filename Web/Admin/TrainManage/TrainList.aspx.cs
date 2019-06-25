
using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Web.Admin.TrainManage
{
    public partial class TrainList: BasePage
    {
        protected override void SetPowerZone()
        {
            string sql = "SELECT * FROM (SELECT  CodeType,COUNT(1) CodeCount,'' CodeName FROM T_CodingMemory  GROUP BY CodeType) t1 RIGHT JOIN (SELECT value FROM dbo.F_Split('1,2,3,4',',')) t2 ON t1.CodeType=t2.value ";
            DataTable dt = CommonBase.GetTable(sql);
            foreach(DataRow row in dt.Rows)
            {
                row["CodeType"] = row["value"];
                string coName = "数字编码";
                switch(row["CodeType"].ToString())
                {
                    case "1": coName = "数字编码"; break;
                    case "2": coName = "字母编码"; break;
                    case "3": coName = "扑克编码"; break;
                    case "4": coName = "超级密码"; break;
                }
                row["CodeName"] = coName;
                if(string.IsNullOrEmpty(row["CodeCount"].ToString()))
                {
                    row["CodeCount"] = 0;
                }
            }
            rep_list.DataSource = dt;
            rep_list.DataBind();
        }
    }
}