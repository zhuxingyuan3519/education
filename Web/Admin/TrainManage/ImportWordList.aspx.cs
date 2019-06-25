
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Service;

namespace Web.Admin.TrainManage
{
    public partial class ImportWordList: BasePage
    {
        protected override void SetPowerZone()
        {
            GetDictBindDDL("BookVersion", ddl_Version);
            ddl_Version.Items.Insert(0, new ListItem("--版本--", ""));

            GetDictBindDDL("Grade", ddl_Grade);
            ddl_Grade.Items.Insert(0, new ListItem("--年级--", ""));

            GetDictBindDDL("Leavel", ddl_Leavel);
            ddl_Leavel.Items.Insert(0, new ListItem("--学期--", ""));

            GetDictBindDDL("Unit", ddl_Unit);
            ddl_Unit.Items.Insert(0, new ListItem("--章节--", ""));
        }
    }
}