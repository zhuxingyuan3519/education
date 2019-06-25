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
    public partial class select_member : BasePage
    {
        //protected string TJCount = "0", TDCount1 = "0", TDCount2 = "0";
        protected override void SetPowerZone()
        {
         
        }

        class memberLeavel
        {
            public int Rank { get; set; }
            public int RankCount { get; set; }
            public int RedBagCount { get; set; }
            public string RandRemark { get; set; }
            public bool IsShowRedBag { get; set; }
        }
    }
}