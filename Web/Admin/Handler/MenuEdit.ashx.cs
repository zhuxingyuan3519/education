using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Model;
using System.Collections;
using DBUtility;
using MethodHelper;

namespace Web.Admin.Handler
{
    /// <summary>
    /// MenuEdit 的摘要说明
    /// </summary>
    public class MenuEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string cid = context.Request.QueryString["cid"];
            string cname = context.Request.QueryString["cname"];
            string pid = context.Request.QueryString["pid"];
            string privageId = context.Request.QueryString["privageid"];
            string url = context.Request.QueryString["murl"];
            string icon = context.Request.QueryString["micon"];
            string index = context.Request.QueryString["mindex"];
            string mtype = context.Request.QueryString["mtype"];
            string type = context.Request.QueryString["type"];//1-修改；2-删除；3-新增；4-修改图片;5-删除一级分类,6-删除二级分类
            string result = "0";
            if (!string.IsNullOrEmpty(cid))
            {
                Sys_Privage obj = null;
                if (type == "1" || type == "4")
                {
                    obj = CommonBase.GetModel<Sys_Privage>(cid);
                    string oldCode = obj.Id;
                    if (type == "1") //修改
                    {
                        obj.Name = cname;
                        obj.URL = url;
                        int mindex = 0;
                        bool b = int.TryParse(index, out mindex);
                        obj.MenuIndex = mindex;
                        obj.Icon = icon;
                    }
                    //else if (type == "2")//删除
                    //    obj.IsDeleted = true;
                    Hashtable hs = new Hashtable();
                    if (CommonBase.Update<Sys_Privage>(obj))
                    {
                        if (pid == "0")
                        {
                            string sql = "update Sys_Privage set ParentCode='" + obj.Id + "' where ParentCode='" + oldCode + "'";
                            CommonBase.RunSql(sql);
                            result = "1";
                        }
                        result = "1";
                    }
                }
                else if (type == "3")//新增
                {
                    obj = new Sys_Privage();
                    obj.Id = privageId;
                    obj.ParentCode = pid;
                    obj.Name = cname;
                    obj.URL = url;
                    obj.PrivageType =int.Parse(mtype);
                    int mindex = 0;
                    bool b = int.TryParse(index, out mindex);
                    obj.MenuIndex = mindex;
                    obj.Icon = icon;
                    //obj.Id = MethodHelper.CommonHelper.GetGuid;
                    if (pid == "0")//如果是一级分类，还需要查看一级分类的Code是否有重复，不能重复
                    {
                        if (CommonBase.GetModel<Sys_Privage>(cid) != null)
                        {
                            result = "-1";
                        }
                        //obj.Id = Sys_Privage_Bll.GetNewCode();
                        else
                        {
                            result = CommonBase.Insert<Sys_Privage>(obj).ToString();
                            result = obj.Id;//返回主建
                        }
                    }
                    else
                    {
                        result = CommonBase.Insert<Sys_Privage>(obj).ToString();
                        result = obj.Id;//返回主建
                    }
                }
                else if (type == "5")
                {
                    //删除一级分类
                    obj = CommonBase.GetModel<Sys_Privage>(cid);
                    result = CommonBase.Delete<Sys_Privage>(obj).ToString();
                }
                else if (type == "6")
                {
                    //删除二级分类
                    obj = CommonBase.GetModel<Sys_Privage>(cid);
                    result = CommonBase.Delete<Sys_Privage>(obj).ToString();
                }

                if (type == "5" && result != "0")
                {
                    //删除子类
                    string sql = "DELETE FROM Sys_Privage where ParentCode='" + cid + "'";
                    CommonBase.RunSql(sql);
                }
            }
            //刷新缓存
            CacheHelper.RemoveAllCache("Sys_Privage");
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