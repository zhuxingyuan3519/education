﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class credit_apply : System.Web.UI.Page
    {
        protected Member TModel;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Member"] != null)
                {
                    TModel = Session["Member"] as Member;
                }
            }
        }
    }
}