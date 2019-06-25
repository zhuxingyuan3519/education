using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Handler
{
    /// <summary>
    /// SynsAMapAddressData 的摘要说明
    /// </summary>
    public class SynsAMapAddressData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string citycode = context.Request["ccctycode"];
            string citytelcode = context.Request["citycode"];
            string adcode = context.Request["adcode"];
            string name = context.Request["name"];
            string center = context.Request["center"];
            string level = context.Request["level"];
            string proCode = context.Request["proCode"];

            Model.Sys_StandardArea area = new Model.Sys_StandardArea();
            area.AdCode = adcode;
            area.CityTelCode = citytelcode;
            area.Level = level;
            area.Name = name;
            area.Sort = 0;
            area.Status = true;
            area.Center = center;
            area.UpdateTime = DateTime.Now;
            if (area.Level == "country")
                area.LevelInt = 10;
            else if (area.Level == "province")
                area.LevelInt = 20;
            else if (area.Level == "city")
                area.LevelInt = 30;
            else if (area.Level == "district")
                area.LevelInt = 40;
            if (area.Level != "province" || area.Level != "country")
                area.ProCode = proCode;

            if (area.Level != "province" || area.Level != "country" || area.Level != "city")
                area.CityCode = citycode;

            if (area.Level != "biz_area")
            { //不保存街道信息
                if (CommonBase.Insert<Model.Sys_StandardArea>(area))
                    context.Response.Write("0");
                else
                    throw new Exception();
            }

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