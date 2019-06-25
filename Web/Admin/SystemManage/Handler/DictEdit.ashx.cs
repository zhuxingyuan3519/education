using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using System.Collections;
using DBUtility;
using MethodHelper;

namespace Web.SystemManage.Handler
{
    /// <summary>
    /// DictEdit 的摘要说明
    /// </summary>
    public class DictEdit: IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string cid = context.Request.QueryString["cid"];
            string cname = context.Request.QueryString["cname"];
            string pid = context.Request.QueryString["pid"];
            string ccode = context.Request.QueryString["ccode"];
            string type = context.Request.QueryString["type"];//1-修改；2-删除；3-新增；4-修改图片;5-删除一级分类,6-删除二级分类
            string result = "0";
            if(!string.IsNullOrEmpty(cid))
            {
                Model.CM_Dict obj = null;
                if(type == "1" || type == "2" || type == "4")
                {
                    obj = CommonBase.GetModel<CM_Dict>(cid);
                    string oldCode = obj.Code;
                    obj.LastUpdateBy = "system";
                    obj.LastUpdateTime = DateTime.Now;
                    if(type == "1")
                    {
                        obj.Name = cname;
                        obj.Code = ccode;
                    }
                    else if(type == "2")
                        obj.IsDeleted = true;
                    Hashtable hs = new Hashtable();
                    if(CommonBase.Update<CM_Dict>(obj))
                    {
                        if(pid == "0")
                        {
                            string sql = "update CM_Dict set ParentCode='" + obj.Code + "' where ParentCode='" + oldCode + "'";
                            CommonBase.RunSql(sql);
                            result = "1";
                        }
                        result = "1";
                    }
                }
                else if(type == "3")
                {
                    obj = new Model.CM_Dict();
                    obj.ParentCode = pid;
                    obj.CreatedBy = "system";
                    obj.CreatedTime = DateTime.Now;
                    obj.Name = cname;
                    obj.Code = string.IsNullOrEmpty(ccode) ? Guid.NewGuid().ToString().Replace("-", "").Replace(" ", "").Substring(0, 6) : ccode;
                    if(pid == "0")//如果是一级分类，还需要查看一级分类的Code是否有重复，不能重复
                    {
                        if(CommonBase.GetList<CM_Dict>("IsDeleted=0 and ParentCode=0 and Code='" + obj.Code + "'").Count > 0)
                            result = "-1";
                        else
                            result = CommonBase.Insert<CM_Dict>(obj).ToString();
                    }
                    else
                        result = CommonBase.Insert<CM_Dict>(obj).ToString();

                }
                else if(type == "5")
                {
                    //删除一级分类
                    obj = CommonBase.GetList<CM_Dict>("Code='" + cid + "' and ParentCode='0'").FirstOrDefault();
                    obj.LastUpdateBy = "system";
                    obj.LastUpdateTime = DateTime.Now;
                    obj.IsDeleted = true;
                    result = CommonBase.Update<CM_Dict>(obj).ToString();
                }
                else if(type == "6")
                {
                    //删除二级分类
                    obj = CommonBase.GetList<CM_Dict>("Code='" + cid + "' and ParentCode='" + pid + "'").FirstOrDefault();
                    obj.LastUpdateBy = "system";
                    obj.LastUpdateTime = DateTime.Now;
                    obj.IsDeleted = true;
                    result = CommonBase.Update<CM_Dict>(obj).ToString();
                }

                if(type == "5" && result != "0")
                {
                    //删除子类
                    string sql = "update CM_Dict set IsDeleted=1 where ParentCode='" + cid + "'";
                    CommonBase.RunSql(sql);
                }
            }
            CacheHelper.RemoveAllCache("CM_Dict");
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