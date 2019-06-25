using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using Model;

namespace Web
{
    public class BaseHandler : BasePage, IHttpHandler, IRequiresSessionState
    {
        protected Member SessionModel
        {
            get
            {
                return Session["Member"] as Member;
            }
        }
    }
}