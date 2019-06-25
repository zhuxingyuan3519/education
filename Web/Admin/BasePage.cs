using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.UI.HtmlControls;
using Model;
using System.Threading;
using Service;
using DBUtility;
using System.Text.RegularExpressions;

namespace Web.Admin
{
    public class BasePage: System.Web.UI.Page
    {
        public Model.Member TModel
        {
            get
            {
                if(Session["AdminMember"] == null)
                {
                    if(!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        int indenity = 0;
                        int.TryParse(User.Identity.Name, out indenity);
                        if(indenity > 0)
                        {
                            Model.Member model = CommonBase.GetModel<Model.Member>(User.Identity.Name);
                            if(model != null)
                            {
                                Session["AdminMember"] = model;
                            }
                        }
                    }
                }
                return Session["AdminMember"] as Model.Member;
            }
            set { }
        }

        public static string NameHeader = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            //////z作为测试
            //if (Session["AdminMember"] == null)
            //{
            //    Model.Member model = new Model.Member();
            //    model = CommonBase.GetModel<Model.Member>(692828);
            //    Session["AdminMember"] = model;
            //}

            TModel = Session["AdminMember"] as Model.Member;
            if(TModel == null)
            {
                Response.Write("<script>window.location='/Admin/Login'</script>");
                Response.End();
            }
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

        protected string GetDictNameByCode(string code, string parentCode)
        {
            CM_Dict obj = CacheService.DictList.Where(c => c.ParentCode == parentCode && c.Code == code).FirstOrDefault();
            if(obj != null)
            {
                return obj.Name;
            }
            else
                return "";
        }

        protected void BindDDLFromDict(string parentCode, System.Web.UI.HtmlControls.HtmlSelect ddl)
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
        protected List<string> GetSetedGuid(List<string> newDetailListTrain)
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
        public string GetAddressName(string objId)
        {
            string result = string.Empty;
            if(!string.IsNullOrEmpty(objId))
            {

                Sys_StandardArea address = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == objId);
                if(address != null)
                {
                    result = address.Name;
                }
            }
            return result;
        }
        //public string GetIndutyName(string objId)
        //{
        //    string result = string.Empty;
        //    if (!string.IsNullOrEmpty(objId))
        //    {
        //        int id = -1;
        //        bool b = int.TryParse(objId, out id);
        //        if (b)
        //        {
        //            Sys_Industry_Model model = Sys_Industry_Bll.Model.FirstOrDefault(c => c.JobId == id);
        //            if (model != null)
        //            {
        //                string[] argIds = model.JobIndex.Split('|');
        //                foreach (string str in argIds)
        //                {
        //                    if (!string.IsNullOrEmpty(str))
        //                    {
        //                        Sys_Industry_Model firstModel = Sys_Industry_Bll.Model.FirstOrDefault(c => c.JobId.ToString() == str);
        //                        if (firstModel != null)
        //                            result += firstModel.JobName + "-";
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    result = result.Substring(0, result.Length - 1);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        public class PageToSalt
        {
            public string page { set; get; }
            public string salt { set; get; }
        }
        public string GetBankName(string bankCode)
        {
            string result = string.Empty;
            Sys_BankInfo bank = CacheService.BankList.FirstOrDefault(c => c.Code == bankCode);
            if(bank != null)
                result = bank.Remark;
            return result;
        }

        //Add by zhuxy,其他页面可能公众的方法
        protected IList<string> GetSetedGuid(IList<string> newDetailListTrain)
        {
            IList<string> strFlagsTrain = new List<string>();
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


        /// <summary>
        /// 校验权限
        /// </summary>
        /// <param name="powerCode"></param>
        /// <returns></returns>
        public bool IsHasPower(string powerCode)
        {
            bool result = false;
            if(TModel.Role.IsAdmin)
            {
                int count = CommonBase.GetSingle<Sys_RolePower>("RoleCode='" + TModel.Role.Code + "' and PrivageId='" + powerCode + "'");
                if(count > 0)
                    result = true;
            }
            else
            {
                int count = CommonBase.GetSingle<Sys_RolePower>("MID='" + TModel.ID + "' and PrivageId='" + powerCode + "'");
                if(count > 0)
                    result = true;
            }
            return result;
        }
        /// <summary>
        /// 代理商的代理地区
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public string GetAgentAddress(long areaId)
        {
            //查询到代理地区
            string addressAgent = string.Empty;
            Sys_StandardArea address = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == areaId.ToString());
            if(address == null)
                return addressAgent;
            if(address.Level == "40")//区县
            {
                //找到市级
                Sys_StandardArea city = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == address.CityCode);
                if(city != null)
                {
                    Sys_StandardArea province = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == city.ProCode.Trim());
                    if(province != null)
                    {
                        addressAgent = province.Name + "—" + city.Name + "—" + address.Name;
                    }
                }
            }
            else if(address.Level == "30")//市级
            {
                //找到市级
                Sys_StandardArea province = CacheService.SatandardAddressList.FirstOrDefault(c => c.AdCode == address.ProCode.Trim());
                {
                    if(province != null)
                    {
                        addressAgent = province.Name + "—" + address.Name;
                    }
                }
            }
            else if(address.Level == "20")//省级
            {
                //找到市级
                addressAgent = address.Name;
            }
            return addressAgent;
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