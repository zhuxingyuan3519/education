
using DBUtility;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// RoleEdit 的摘要说明
    /// </summary>
    public class RoleEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string cid = context.Request.QueryString["cid"];
            string cname = context.Request.QueryString["cname"];
            string pid = context.Request.QueryString["pid"];
            string url = context.Request.QueryString["murl"];
            string type = context.Request.QueryString["type"];//1-修改；2-删除；3-新增；4-修改图片;5-删除一级分类,6-删除二级分类
            string result = "0";
            if (!string.IsNullOrEmpty(cid))
            {
                Sys_Role obj = null;
                if (type == "1" || type == "4")
                {
                    obj = CommonBase.GetModel<Sys_Role>(cid);
                    string oldCode = obj.Code;

                    if (type == "1") //修改
                    {
                        obj.Id = cname;
                        obj.Name = url;
                    }
                    Hashtable hs = new Hashtable();
                    if (CommonBase.Update<Sys_Role>(obj))
                    {
                        result = "1";
                    }
                }
                else if (type == "3")//新增
                {
                    obj = new Sys_Role();
                    obj.Name = cname;
                    obj.Code = cname;
                    obj.Name = url;
                    if (pid == "0")//如果是一级分类，还需要查看一级分类的Code是否有重复，不能重复
                    {
                        if (CommonBase.GetModel<Sys_Role>(obj.Id) != null)
                        {
                            result = "-1";
                        }
                        else
                        {
                            if (CommonBase.Insert<Sys_Role>(obj))
                                result = obj.Code;//返回主建
                        }
                    }
                    else
                    {
                        if (CommonBase.Insert<Sys_Role>(obj))
                            result = obj.Code;//返回主建
                    }
                }
                else if (type == "5")
                {
                    //删除一级分类
                    obj = CommonBase.GetModel<Sys_Role>(cid);
                    result = CommonBase.Delete<Sys_Role>(obj).ToString();
                }

            }
            context.Response.Write(result);
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