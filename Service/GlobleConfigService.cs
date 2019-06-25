using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class GlobleConfigService
    {
        public static Sys_GlobleConfig GetGlobleConfig
        {
            get
            {
                return CacheService.GlobleConfig;
            }
        }

        public static SH_RedBagConfig GetRedBagGlobleConfig
        {
            get
            {
                return CacheService.RedBagGlobleConfig;
            }
        }

        public static Sys_WebConfig GetWebConfig(string code)
        {
            Sys_WebConfig webConfigModel = Service.CacheService.WebConfigList.Where(c => c.Code == code).FirstOrDefault();
            if (webConfigModel != null)
            {
                return webConfigModel;
            }
            else
            {
                return new Sys_WebConfig();
            }
        }
    }
}
