using DBUtility;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Handler
{
    /// <summary>
    /// UpdateMemberAddress 的摘要说明
    /// </summary>
    public class UpdateMemberAddress : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (string.IsNullOrEmpty(context.Request["type"]))
            {
                List<Model.Member> listMember = CommonBase.GetList<Model.Member>("");
                List<memberdata> list = new List<memberdata>();
                foreach (Model.Member mem in listMember)
                {
                    if (!string.IsNullOrEmpty(mem.Province))
                    {
                        Model.Sys_StandardArea address = CacheService.SatandardAddressList.Where(c => c.AdCode ==mem.Province).FirstOrDefault();
                        if (address != null)
                        {
                            string adname = address.Name;
                            list.Add(new memberdata(mem.ID, adname, "Province"));
                        }
                    }

                    if (!string.IsNullOrEmpty(mem.City))
                    {
                        Model.Sys_StandardArea address = CacheService.SatandardAddressList.Where(c => c.AdCode == mem.City).FirstOrDefault();
                        if (address != null)
                        {
                            string adname = address.Name;
                            list.Add(new memberdata(mem.ID, adname, "City"));
                        }
                    }

                    if (!string.IsNullOrEmpty(mem.Zone))
                    {
                        Model.Sys_StandardArea address = CacheService.SatandardAddressList.Where(c => c.AdCode == mem.Zone).FirstOrDefault();
                        if (address != null)
                        {
                            string adname = address.Name;
                            list.Add(new memberdata(mem.ID, adname, "Zone"));
                        }
                    }

                    if (!string.IsNullOrEmpty(mem.AreaId.ToString()))
                    {
                        Model.Sys_StandardArea address = CacheService.SatandardAddressList.Where(c => c.AdCode == mem.AreaId.ToString()).FirstOrDefault();
                        if (address != null)
                        {
                            string adname = address.Name;
                            list.Add(new memberdata(mem.ID, adname, "AreaId"));
                        }
                    }
                }
                context.Response.Write(MethodHelper.JsonHelper.ObjectVarToJson(list));
            }
            else
            {
                //保存
                string mid = context.Request["mid"];
                string field = context.Request["field"];
                string adcode = context.Request["adcode"];
                string sql = "update Member set " + field + "='" + adcode + "' where ID=" + mid;
                CommonBase.RunSql(sql);
            }
        }

        class memberdata
        {
            public memberdata(string _mid, string _addname, string _field)
            {
                this.addname = _addname;
                this.mid = _mid;
                this.field = _field;
            }
            public string mid { get; set; }
            public string addname { get; set; }
            public string field { get; set; }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}