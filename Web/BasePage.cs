
using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace Web
{
    public class BasePage: System.Web.UI.Page
    {
        public bool IsMobile()
        {
            string u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if(!(b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        //protected Model.WebSetInfo WebModel = Bll.WebSetInfo.Model;
        public string MID = string.Empty;
        public Model.Member TModel
        {
            get
            {
                if(Session["Member"] == null)
                {
                    //if (!string.IsNullOrEmpty(User.Identity.Name))
                    //{
                    //    int indenity = 0;
                    //    int.TryParse(User.Identity.Name, out indenity);
                    //    if (indenity > 0)
                    //    {
                    //        Model.Member model = CommonBase.GetModel<Model.Member>(User.Identity.Name);
                    //        if (model != null)
                    //        {
                    //            Session["Member"] = model;
                    //        }
                    //    }
                    //}
                }
                return Session["Member"] as Model.Member;
            }
            set { }
        }

        public static string NameHeader = "ctl00$ContentPlaceHolder1$";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            ////z作为测试
            //if (Session["Member"] == null)
            //{
            //    Model.Member model = new Model.Member();
            //    model = CommonBase.GetModel<Model.Member>(4158);
            //    Session["Member"] = model;
            //}
            ////z作为测试

            //TModel = Session["Member"] as Member;
            if(TModel == null)
            {
                string url = Request.RawUrl;
                string cookName = ConfigurationManager.AppSettings["SystemID"];
                HttpCookie cookie = new HttpCookie(cookName);
                cookie.Value = url;
                Response.AppendCookie(cookie);
                //if (MethodHelper.ConfigHelper.GetAppSettings("version") == "2.0")
                //{
                //    Response.Write("<script>window.location='/m/app/user_login'</script>");
                //}
                //else
                //{
                //    Response.Write("<script>window.location='/login'</script>");
                //}
                Response.Write("<script>window.location='/m/app/user_change'</script>");
                Response.End();
            }
            if(TModel != null)
                MID = TModel.MID;

            if(!IsPostBack)
            {
                VerifyPower();
            }
        }
        private void VerifyPower()
        {
            SetPowerZone();
            if(!string.IsNullOrEmpty(Request.QueryString["Action"]))
            {
                try
                {
                    if(Request.QueryString["Action"].ToUpper() == "ADD")
                    {
                        Response.Write(btnAdd_Click());
                    }
                    else if(Request.QueryString["Action"].ToUpper() == "MODIFY")
                    {
                        Response.Write(btnModify_Click());
                    }
                    else if(Request.QueryString["Action"].ToUpper() == "OTHER")
                    {
                        Response.Write(btnOther_Click());
                    }
                    else if(Request.QueryString["Action"].ToUpper() == "QUERY")
                    {
                        Response.Write(btnQuery_Click());
                    }
                    else
                    {
                        Response.Write("未提供该操作的函数");
                    }
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    Response.End();
                }
                return;
            }
            else if(!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                SetValue(Request.QueryString["ID"]);
            }
            else
            {
                SetValue();
            }
        }

        protected virtual string btnAdd_Click()
        {
            return "未重载";
        }

        protected virtual string btnModify_Click()
        {
            return "未重载";
        }

        protected virtual string btnOther_Click()
        {
            return "未重载";
        }
        protected virtual string btnQuery_Click()
        {
            return "未重载";
        }

        protected virtual void SetValue(string id)
        {

        }

        protected virtual void SetValue()
        {

        }

        protected virtual void SetPowerZone()
        {

        }

        protected virtual bool NoPower()
        {
            return true;
        }

        protected string GetGuid
        {
            get { return Guid.NewGuid().ToString().Replace("-", "").Replace(" ", ""); }
        }
        protected string GetPwdType
        {
            get { return "DES"; }
        }
        public Random GetRandom()
        {
            Thread.Sleep(1);
            long tick = DateTime.Now.Ticks;//一个以0.1纳秒为单位的时间戳，18位
            int seed = int.Parse(tick.ToString().Substring(9)); //  int类型的最大值:  2147483647
            //或者使用unchecked((int)tick)也可
            return new Random(seed);
        }
        public int GetRandNum(int min, int max)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }
        public string GetDictValue(string code, string parentCode)
        {
            return DictService.GetDictValue(code, parentCode);
        }

        public bool EditTableNeedSaveKeys(string key, string[] EditTableKeys)
        {
            if(EditTableKeys != null && EditTableKeys.Length > 0)
            {
                foreach(string o in EditTableKeys)
                {
                    if(o.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        protected IList<string> GetSetedGuid(IList<string> newDetailListTrain)
        {
            List<string> strFlagsTrain = new List<string>();
            foreach(string str in newDetailListTrain)
            {
                string keyFlags = string.Empty;
                if(str.Split('_').Count() >= 2)
                {
                    keyFlags = str.Split('_')[1];
                    if(!strFlagsTrain.Contains(keyFlags))
                    {
                        strFlagsTrain.Add(keyFlags);
                    }
                }
            }
            return strFlagsTrain;
        }
        public int ToNullInt(object l)
        {
            int ret;
            try
            {
                if(int.TryParse(l.ToString(), out ret))
                {
                    return ret;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public long ToNullLong(object l)
        {
            long ret;
            try
            {
                if(long.TryParse(l.ToString(), out ret))
                {
                    return ret;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public decimal ToNullDecimal(object l)
        {
            decimal ret;
            try
            {
                if(decimal.TryParse(l.ToString(), out ret))
                {
                    return ret;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        protected string GetLabelValue(object labels)
        {
            string result = string.Empty;
            string label = labels.ToString();
            if(!string.IsNullOrEmpty(label))
            {
                string[] array = label.Split(',');
                foreach(string str in array)
                {
                    if(!string.IsNullOrEmpty(str))
                        result += GetDictValue(str, "Label") + "/";
                }
                if(!string.IsNullOrEmpty(result))
                    return result.Substring(0, result.LastIndexOf('/'));
            }
            return string.Empty;
        }

        public string GetUserIP()
        {
            string _userIP;
            if(HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null)//未使用代理
            {
                _userIP = HttpContext.Current.Request.UserHostAddress;
            }
            else//使用代理服务器
            {
                _userIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            return _userIP;
        }



        public void SetControlValue<T>(T model)
        {
            try
            {
                Type type = typeof(T);
                PropertyInfo[] propertyArray = type.GetProperties(); //获取所有的公有属性
                foreach(PropertyInfo property in propertyArray)
                {
                    string lastName = property.Name;
                    string controlId = NameHeader + "txt_" + lastName;
                    System.Web.UI.HtmlControls.HtmlInputText inputControl = Master.FindControl(controlId) as System.Web.UI.HtmlControls.HtmlInputText;
                    if(inputControl != null)
                    {
                        object value = property.GetValue(model, null);
                        if(value != null)
                            inputControl.Value = value.ToString();
                    }
                    controlId = NameHeader + "ddl_" + lastName;
                    System.Web.UI.HtmlControls.HtmlSelect selectControl = Master.FindControl(controlId) as System.Web.UI.HtmlControls.HtmlSelect;
                    if(selectControl != null)
                    {
                        object value = property.GetValue(model, null);
                        if(value != null)
                            selectControl.Value = value.ToString();
                    }
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }

            Member mem = GetModel<Member>();
        }

        public T GetModel<T>() where T : new()
        {
            Type type = typeof(T);
            T t = new T();
            PropertyInfo[] propertyArray = type.GetProperties(); //获取所有的公有属性
            foreach(PropertyInfo property in propertyArray)
            {
                string lastName = property.Name;
                string controlId = NameHeader + "txt_" + lastName;
                ControlCollection c = Master.FindControl("ContentPlaceHolder1").Controls;
                foreach(var ctl in c)
                {
                    string typeCtrl = ctl.GetType().ToString();
                    if(typeCtrl.Contains("Input"))
                    {

                    }
                }
                //property.SetValue(type)
            }
            return t;
        }


        public string GetBankName(object bankCode)
        {
            string result = string.Empty;
            if(bankCode != null)
            {
                Sys_BankInfo bank = CacheService.BankList.FirstOrDefault(c => c.Code == bankCode.ToString());
                if(bank != null)
                    result = bank.Remark;
            }
            return result;
        }
        public string GetIndutyName(object code, bool isPayPlan)
        {
            string result = string.Empty;
            if(code != null)
            {
                CM_Induty bank = CacheService.IndutyList.FirstOrDefault(c => c.Code == code.ToString());
                if(bank != null)
                {
                    if(isPayPlan)
                        result = "线下消费";
                    else
                        result = bank.Name;
                }
                else
                {
                    result = "网络消费";
                }
            }
            return result;
        }


        protected void ExecuteScript(string script)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
        protected void ScriptAlert(string msg)
        {
            //msg = SystemHelper.Clear(msg);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script_alert", "alert('" + msg + "');", true);
        }


        protected void GetDictBindDDL(string parentCode, System.Web.UI.HtmlControls.HtmlSelect ddl)
        {
            var obj = CacheService.DictList.Where(c => c.ParentCode == parentCode);
            if(obj != null)
            {
                ddl.DataSource = obj;
                ddl.DataTextField = "Name";
                ddl.DataValueField = "Code";
                ddl.DataBind();
            }
        }
    }
}