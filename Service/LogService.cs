using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class LogService
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="member"></param>
        /// <param name="LogType">1-登录系统；2-创建会员；3-删除会员；4-创建管理员；5-删除管理员，6-开通或禁用系统，7-会员自主注册，8-支付相关；10-端口消耗记录</param>
        /// <param name="LogMessage"></param>
        public static void Log(Model.Member member, string LogType, string LogMessage, string loginAddress = "")
        {
            try
            {
                DB_Log log = new DB_Log();
                if(LogType == "1")
                {
                    string province = string.Empty, city = string.Empty, zone = string.Empty, pointer = string.Empty;
                    if(!string.IsNullOrEmpty(loginAddress))
                    {
                        string[] array = loginAddress.Split('|');
                        if(array.Length == 4)
                        {
                            province = array[0];
                            city = array[1];
                            zone = array[2];
                            pointer = array[3];
                        }
                        else if(array.Length == 3)
                        {
                            province = array[0];
                            city = array[1];
                            zone = array[2];
                        }
                        else if(array.Length == 2)
                        {
                            province = array[0];
                            city = array[1];
                        }
                        else if(array.Length == 1)
                        {
                            province = array[0];
                        }
                    }
                    log = CommonBase.GetList<DB_Log>("IsDeleted=0 and LType='1' and  MID=" + member.ID + " and  DATEDIFF(dd,LogDate,GETDATE())=0").FirstOrDefault();
                    if(log != null)
                    {
                        log.Field1 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        log.Status = log.Status + 1;
                        log.Field2 = province;
                        log.Field3 = city;
                        log.Field4 = zone;
                        log.Field5 = pointer;
                        CommonBase.Update<DB_Log>(log);
                    }
                    else
                    {
                        log = new DB_Log();
                        log.Code = CommonHelper.GetGuid;
                        log.Field2 = province;
                        log.Field3 = city;
                        log.Field4 = zone;
                        log.Field5 = pointer;
                        log.CreatedBy = member.MID;
                        log.CreatedTime = DateTime.Now;
                        log.IsDeleted = false;
                        log.LogDate = DateTime.Now;
                        log.LType = LogType;
                        log.MCode = member.MID;
                        log.MID = member.ID;
                        log.OperatorRole = member.RoleCode;
                        log.Remark = LogMessage;
                        log.Status = 0;
                        CommonBase.Insert<DB_Log>(log);
                    }
                }
                else
                {
                    log.Code = CommonHelper.GetGuid;
                    log.CreatedBy = member.MID;
                    log.CreatedTime = DateTime.Now;
                    log.IsDeleted = false;
                    log.LogDate = DateTime.Now;
                    log.LType = LogType;
                    log.MCode = member.MID;
                    log.MID = member.ID;
                    log.OperatorRole = member.RoleCode;
                    log.Remark = LogMessage;
                    log.Status = 0;
                    CommonBase.Insert<DB_Log>(log);
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="LogType">1-登录系统；2-创建会员；3-删除会员；4-创建管理员；5-删除管理员，6-开通或禁用系统，7-会员自主注册，8-支付相关；10-端口消耗记录</param>
        /// <param name="LogMessage"></param>
        /// <param name="listComm"></param>
        public static void Log(Model.Member member, string LogType, string LogMessage, List<CommonObject> listComm)
        {
            try
            {
                DB_Log log = new DB_Log();
                if(LogType == "1")
                {
                    log = CommonBase.GetList<DB_Log>("IsDeleted=0 and MID=" + member.ID + " and  DATEDIFF(dd,LogDate,GETDATE())=0").FirstOrDefault();
                    if(log != null)
                    {
                        log.Field1 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        log.Status = log.Status + 1;
                        CommonBase.Update<DB_Log>(log, listComm);
                    }
                    else
                    {
                        log = new DB_Log();
                        log.Code = CommonHelper.GetGuid;
                        log.CreatedBy = member.MID;
                        log.CreatedTime = DateTime.Now;
                        log.IsDeleted = false;
                        log.LogDate = DateTime.Now;
                        log.LType = LogType;
                        log.MCode = member.MID;
                        log.MID = member.ID;
                        log.OperatorRole = member.RoleCode;
                        log.Remark = LogMessage;
                        log.Status = 0;
                        CommonBase.Insert<DB_Log>(log);
                    }
                }
                else
                {
                    log.Code = CommonHelper.GetGuid;
                    log.CreatedBy = member.MID;
                    log.CreatedTime = DateTime.Now;
                    log.IsDeleted = false;
                    log.LogDate = DateTime.Now;
                    log.LType = LogType;
                    log.MCode = member.MID;
                    log.MID = member.ID;
                    log.OperatorRole = member.RoleCode;
                    log.Remark = LogMessage;
                    log.Status = 0;
                    CommonBase.Insert<DB_Log>(log, listComm);
                }
            }
            catch
            {

            }
        }
        public static void Log(string ID, string mid, string LogType, string LogMessage, List<CommonObject> listComm)
        {
            try
            {
                DB_Log log = new DB_Log();
                log = new DB_Log();
                log.Code = CommonHelper.GetGuid;
                log.CreatedBy = mid;
                log.CreatedTime = DateTime.Now;
                log.IsDeleted = false;
                log.LogDate = DateTime.Now;
                log.LType = LogType;
                log.MCode = mid;
                log.MID = ID;
                log.OperatorRole = string.Empty;
                log.Remark = LogMessage;
                log.Status = 0;
                CommonBase.Insert<DB_Log>(log, listComm);
            }
            catch
            {

            }
        }
    }
}
