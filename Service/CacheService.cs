using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CacheService
    {
        /// <summary>
        /// 缓存地址信息
        /// </summary>
        //public static List<Sys_Address> AddressList
        //{
        //    get
        //    {
        //        //List<Sys_StandardArea> listStandArdAdd = CacheService.SatandardAddressList;

        //        ////List<Sys_Address> listOldAddress=listStandArdAdd.

        //        //var list = CacheHelper.Get<List<Sys_StandardArea>>("Sys_StandardArea");
        //        //if (CacheHelper.Get<List<Sys_StandardArea>>("Sys_StandardArea") == null)
        //        //{
        //        //    list = CommonBase.GetList<Sys_StandardArea>("IsDeleted=0");
        //        //    CacheHelper.Insert("Sys_StandardArea", list);
        //        //}
        //        return null;
        //    }
        //}
        public static IList<Sys_Address> AddressList
        {
            get
            {
                var list = CacheHelper.Get<List<Sys_Address>>("Sys_Address_List");
                if(CacheHelper.Get<List<Sys_Address>>("Sys_Address_List") == null)
                {
                    list = CommonBase.GetList<Sys_Address>("");
                    CacheHelper.Insert("Sys_Address_List", list);
                }
                return list;
            }
        }
        public static List<SH_PrizePool> PrizePoolList
        {
            get
            {
                var list = CacheHelper.Get<List<SH_PrizePool>>("SH_PrizePool");
                if(CacheHelper.Get<List<SH_PrizePool>>("SH_PrizePool") == null)
                {
                    list = CommonBase.GetList<SH_PrizePool>("IsDeleted=0");
                    CacheHelper.Insert("SH_PrizePool", list);
                }
                return list;
            }
        }
        public static IList<CM_Dict> DictList
        {
            get
            {
                var list = CacheHelper.Get<List<CM_Dict>>("CM_Dict");
                if(CacheHelper.Get<List<CM_Dict>>("CM_Dict") == null)
                {
                    list = CommonBase.GetList<CM_Dict>("IsDeleted=0");
                    CacheHelper.Insert("CM_Dict", list);
                }
                return list;
            }
        }
        public static IList<TD_SHMoney> SHMoneyList
        {
            get
            {
                var list = CacheHelper.Get<List<TD_SHMoney>>("TD_SHMoney");
                if(list == null)
                {
                    list = CommonBase.GetList<TD_SHMoney>("");
                    CacheHelper.Insert("TD_SHMoney", list);
                }
                return list;
            }
        }
        public static IList<CM_Induty> IndutyList
        {
            get
            {
                var list = CacheHelper.Get<List<CM_Induty>>("CM_Induty");
                if(CacheHelper.Get<List<CM_Induty>>("CM_Induty") == null)
                {
                    list = CommonBase.GetList<CM_Induty>("IsDeleted=0");
                    CacheHelper.Insert("CM_Induty", list);
                }
                return list;
            }
        }
        public static IList<Sys_BankInfo> BankList
        {
            get
            {
                var list = CacheHelper.Get<List<Sys_BankInfo>>("Sys_BankInfo");
                if(CacheHelper.Get<List<Sys_BankInfo>>("Sys_BankInfo") == null)
                {
                    list = CommonBase.GetList<Sys_BankInfo>("IsDeleted=0");
                    CacheHelper.Insert("Sys_BankInfo", list);
                }
                return list;
            }
        }
        public static IList<Sys_CreditMarket> CreditMarketList
        {
            get
            {
                var list = CacheHelper.Get<List<Sys_CreditMarket>>("Sys_CreditMarket");
                if(CacheHelper.Get<List<Sys_CreditMarket>>("Sys_CreditMarket") == null)
                {
                    list = CommonBase.GetList<Sys_CreditMarket>("IsDeleted=0");
                    CacheHelper.Insert("Sys_CreditMarket", list);
                }
                return list;
            }
        }
        public static IList<CM_POSBank> POSBankList
        {
            get
            {
                var list = CacheHelper.Get<List<CM_POSBank>>("CM_POSBank");
                if(CacheHelper.Get<List<CM_POSBank>>("CM_POSBank") == null)
                {
                    list = CommonBase.GetList<CM_POSBank>("IsDeleted=0");
                    CacheHelper.Insert("CM_POSBank", list);
                }
                return list;
            }
        }

        /// <summary>
        /// 缓存行业信息
        /// </summary>
        //public static IList<Sys_Industry> DictList
        //{
        //    get
        //    {
        //        var list = CacheHelper.Get<List<Sys_Industry>>("Sys_Industry_List");
        //        if (CacheHelper.Get<List<Sys_Industry>>("Sys_Industry_List") == null)
        //        {
        //            list = CommonBase.GetList<Sys_Industry>("IsDeleted=0");
        //            CacheHelper.Insert("Sys_Industry_List", list);
        //        }
        //        return list;
        //    }
        //}

        public static IList<Sys_Role> RoleList
        {
            get
            {
                var list = CacheHelper.Get<List<Sys_Role>>("Sys_Role");
                if(list == null)
                {
                    list = CommonBase.GetList<Sys_Role>("");
                    CacheHelper.Insert("Sys_Role", list);
                }
                return list;
            }
        }
        public static IList<Sys_Privage> PrivageList
        {
            get
            {
                var list = CacheHelper.Get<List<Sys_Privage>>("Sys_Privage");
                if(list == null)
                {
                    list = CommonBase.GetList<Sys_Privage>("");
                    CacheHelper.Insert("Sys_Privage", list);
                }
                return list;
            }
        }

        public static List<CM_Induty> Induty
        {
            get
            {
                var list = CacheHelper.Get<List<CM_Induty>>("CM_Induty");
                if(CacheHelper.Get<List<CM_Induty>>("CM_Induty") == null)
                {
                    list = CommonBase.GetList<CM_Induty>("IsDeleted=0");
                    CacheHelper.Insert("CM_Induty", list);
                }
                return list;
            }
        }

        public static List<Sys_WebConfig> WebConfigList
        {
            get
            {
                var list = CacheHelper.Get<List<Sys_WebConfig>>("Sys_WebConfig");
                if(CacheHelper.Get<List<Sys_WebConfig>>("Sys_WebConfig") == null)
                {
                    list = CommonBase.GetList<Sys_WebConfig>("Status=1");
                    CacheHelper.Insert("Sys_WebConfig", list);
                }
                return list;
            }
        }

        public static List<Sys_StandardArea> SatandardAddressList
        {
            get
            {
                var list = CacheHelper.Get<List<Sys_StandardArea>>("Sys_StandardArea");
                if(CacheHelper.Get<List<Sys_StandardArea>>("Sys_StandardArea") == null)
                {
                    list = CommonBase.GetList<Sys_StandardArea>("Status=1");
                    CacheHelper.Insert("Sys_StandardArea", list);
                }
                return list;
            }
        }

        public static List<TD_ChargeList> ChargeList
        {
            get
            {
                var list = CacheHelper.Get<List<TD_ChargeList>>("TD_ChargeList");
                if(CacheHelper.Get<List<TD_ChargeList>>("TD_ChargeList") == null)
                {
                    list = CommonBase.GetList<TD_ChargeList>("");
                    CacheHelper.Insert("TD_ChargeList", list);
                }
                return list;
            }
        }

        public static Sys_GlobleConfig GlobleConfig
        {
            get
            {
                var list = CacheHelper.Get<Sys_GlobleConfig>("Sys_GlobleConfig");
                if(CacheHelper.Get<Sys_GlobleConfig>("Sys_GlobleConfig") == null)
                {
                    list = CommonBase.GetList<Sys_GlobleConfig>("").FirstOrDefault();
                    CacheHelper.Insert("Sys_GlobleConfig", list);
                }
                return list;
            }
        }
        public static SH_RedBagConfig RedBagGlobleConfig
        {
            get
            {
                var list = CacheHelper.Get<SH_RedBagConfig>("SH_RedBagConfig");
                if(CacheHelper.Get<SH_RedBagConfig>("SH_RedBagConfig") == null)
                {
                    list = CommonBase.GetList<SH_RedBagConfig>("").FirstOrDefault();
                    CacheHelper.Insert("SH_RedBagConfig", list);
                }
                return list;
            }
        }

        public static Configuration ConfigurationModel
        {
            get
            {
                var list = CacheHelper.Get<Configuration>("Configuration");
                if(CacheHelper.Get<Configuration>("Configuration") == null)
                {
                    list = CommonBase.GetList<Configuration>("").FirstOrDefault();
                    CacheHelper.Insert("Configuration", list);
                }
                return list;
            }
        }
    }
}
