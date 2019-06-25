using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Service
{
    public class MessageService
    {
        /// <summary>
        /// 得到某会员的新消息
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetNewMessage(string userCode)
        {
            DataTable dt = GetNoReadMessage(userCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                string str = string.Empty;
                str = MethodHelper.JsonHelper.DataTableToJson(dt, "noticeInfo");
                return str;
            }
            else
                return string.Empty;
        }

        public static DataTable GetNoReadMessage(string userCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  t1.Code, t2.MID,1 as MType FROM DB_Message t1 left JOIN dbo.Member t2 on t1.SendCode=t2.ID ");
            strSql.Append(" where t1.IsDeleted=0 and t1.Status=1 and t1.ReceiveCode=@ReceiveCode  ");

            strSql.Append("  union all  select  DISTINCT  t1.RMcode AS Code, t2.MID,2 as MType FROM DB_ResponseMessage t1 left JOIN dbo.Member t2 on t1.SendCode=t2.ID ");
            strSql.Append(" where t1.IsDeleted=0 and t1.Status=1 and t1.ReceiveCode=@ReceiveCode  ");

            SqlParameter[] parameters = {
					new SqlParameter("@ReceiveCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = userCode;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
                return null;
        }


        /// <summary>
        /// 回复消息
        /// </summary>
        /// <param name="TModel">当前登陆人</param>
        /// <param name="mcode">消息主表的Code</param>
        /// <param name="msg">回复的消息</param>
        /// <param name="list">返回结果</param>
        public static void ResponseMessage(Model.Member TModel, string mcode, string msg, List<CommonObject> list)
        {
            DB_Message_Model msgModel = CommonBase.GetModel<DB_Message_Model>(mcode);
            if (msgModel != null)
            {
                msgModel.Status = 2;
                msgModel.LastUpdateTime = DateTime.Now;
                msgModel.LastUpdateBy = TModel.MID;
                CommonBase.Update<DB_Message_Model>(msgModel, list);
                ////查询回复列表收件人是自己的
                DB_ResponseMessage_Model hasRespnseMsg = CommonBase.GetList<DB_ResponseMessage_Model>("IsDeleted=0 and ReceiveCode='" + TModel.ID + "' and RMcode='" + mcode + "' Order by CreatedTime desc").FirstOrDefault();
                if (hasRespnseMsg != null)
                {
                    //如果最近一条的回复消息发送人是自己，
                    if (TModel.ID.ToString() == hasRespnseMsg.SendCode)
                    {
                        DB_ResponseMessage_Model reMsg = new DB_ResponseMessage_Model();
                        reMsg.Code = MethodHelper.CommonHelper.GetGuid;
                        reMsg.CreatedBy = TModel.MID;
                        reMsg.CreatedTime = DateTime.Now;
                        reMsg.IsDeleted = false;
                        reMsg.Message = msg;
                        reMsg.MType = "1";

                        reMsg.SendCode = TModel.ID.ToString();
                        reMsg.SendName = TModel.MID;
                        reMsg.Field1 = TModel.MName;
                        reMsg.ReceiveCode = msgModel.ReceiveCode;
                        reMsg.ReceiveName = msgModel.ReceiveName;
                        reMsg.Field2 = msgModel.Field2;
                        reMsg.RMcode = msgModel.Code;
                        reMsg.Status = 1;
                        CommonBase.Insert<DB_ResponseMessage_Model>(reMsg, list);
                    }
                    else
                    {
                        hasRespnseMsg.Status = 2;
                        hasRespnseMsg.LastUpdateTime = DateTime.Now;
                        hasRespnseMsg.LastUpdateBy = TModel.MID;
                        CommonBase.Update<DB_ResponseMessage_Model>(hasRespnseMsg, list);
                        //再插入一条回复数据
                        DB_ResponseMessage_Model reMsg = new DB_ResponseMessage_Model();
                        reMsg.Code = MethodHelper.CommonHelper.GetGuid;
                        reMsg.CreatedBy = TModel.MID;
                        reMsg.CreatedTime = DateTime.Now;
                        reMsg.IsDeleted = false;
                        reMsg.Message = msg;
                        reMsg.MType = "1";
                        //管理员的页面
                        reMsg.ReceiveCode = hasRespnseMsg.SendCode;
                        reMsg.ReceiveName = hasRespnseMsg.SendName;
                        reMsg.Field2 = hasRespnseMsg.Field1;
                        reMsg.RMcode = msgModel.Code;
                        reMsg.SendCode = TModel.ID.ToString();
                        reMsg.SendName = TModel.MID;
                        reMsg.Field1 = TModel.MName;
                        reMsg.Status = 1;
                        CommonBase.Insert<DB_ResponseMessage_Model>(reMsg, list);
                    }
                }
                else
                {
                    if (TModel.ID.ToString() == msgModel.SendCode)
                    {
                        DB_ResponseMessage_Model reMsg = new DB_ResponseMessage_Model();
                        reMsg.Code = MethodHelper.CommonHelper.GetGuid;
                        reMsg.CreatedBy = TModel.MID;
                        reMsg.CreatedTime = DateTime.Now;
                        reMsg.IsDeleted = false;
                        reMsg.Message = msg;
                        reMsg.MType = "1";

                        reMsg.SendCode = TModel.ID.ToString();
                        reMsg.SendName = TModel.MID;
                        reMsg.Field1 = TModel.MName;
                        reMsg.ReceiveCode = msgModel.ReceiveCode;
                        reMsg.ReceiveName = msgModel.ReceiveName;
                        reMsg.Field2 = msgModel.Field2;
                        reMsg.RMcode = msgModel.Code;
                        reMsg.Status = 1;
                        CommonBase.Insert<DB_ResponseMessage_Model>(reMsg, list);
                    }
                    else
                    {
                        DB_ResponseMessage_Model reMsg = new DB_ResponseMessage_Model();
                        reMsg.Code = MethodHelper.CommonHelper.GetGuid;
                        reMsg.CreatedBy = TModel.MID;
                        reMsg.CreatedTime = DateTime.Now;
                        reMsg.IsDeleted = false;
                        reMsg.Message = msg;
                        reMsg.MType = "1";

                        reMsg.SendCode = TModel.ID.ToString();
                        reMsg.SendName = TModel.MID;
                        reMsg.Field1 = TModel.MName;
                        reMsg.ReceiveCode = msgModel.SendCode;
                        reMsg.ReceiveName = msgModel.SendName;
                        reMsg.Field2 = msgModel.Field1;
                        reMsg.RMcode = msgModel.Code;
                        reMsg.Status = 1;
                        CommonBase.Insert<DB_ResponseMessage_Model>(reMsg, list);
                    }
                }
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="TModel">发送人</param>
        /// <param name="receiveMemberList">收信人列表</param>
        /// <param name="msg">发送消息内容</param>
        /// <param name="list">返回结果</param>
        public static void SendNewMessage(Model.Member TModel, List<Model.Member> receiveMemberList, string msg, List<CommonObject> list)
        {
            foreach (Model.Member mcode in receiveMemberList)
            {
                send_msg(TModel, mcode, msg, list, string.Empty);
                //DB_Message_Model model = new DB_Message_Model();
                //model.Code = MethodHelper.CommonHelper.GetGuid;
                //model.CreatedBy = TModel.MID;
                //model.CreatedTime = DateTime.Now;
                //model.IsDeleted = false;
                //model.MType = "1";
                //model.ReceiveCode = mcode.ID.ToString();
                //model.SendCode = TModel.ID.ToString();
                //model.Status = 1;
                //model.Message = msg;
                //model.SendName = TModel.MID;
                //model.Field1 = TModel.MName;
                //model.ReceiveName = mcode.MID;
                //model.Field2 = mcode.MName;
                //CommonBase.Insert<DB_Message_Model>(model, list);
            }
        }
        public static void SendNewMessage(Model.Member TModel, List<Model.Member> receiveMemberList, string msg, List<CommonObject> list, string remark)
        {
            foreach (Model.Member mcode in receiveMemberList)
            {
                send_msg(TModel, mcode, msg, list, remark);
            }
        }

        private static void send_msg(Model.Member TModel, Model.Member mcode, string msg, List<CommonObject> list, string msgRemark)
        {
            DB_Message_Model model = new DB_Message_Model();
            model.Code = MethodHelper.CommonHelper.GetGuid;
            model.CreatedBy = TModel.MID;
            model.CreatedTime = DateTime.Now;
            model.IsDeleted = false;
            model.MType = "1";
            model.ReceiveCode = mcode.ID.ToString();
            model.SendCode = TModel.ID.ToString();
            model.Status = 1;
            model.Message = msg;
            model.SendName = TModel.MID;
            model.Field1 = TModel.MName;
            model.ReceiveName = mcode.MID;
            model.Field2 = mcode.MName;
            model.Remark = msgRemark;
            CommonBase.Insert<DB_Message_Model>(model, list);
        }

        public static void DealHasReadMessage(Model.Member TModel, string mtype, string mcode, List<CommonObject> list)
        {
            if (mtype == "1")  //回复的第一条消息
            {
                DB_Message_Model msgModel = CommonBase.GetModel<DB_Message_Model>(mcode);
                if (msgModel != null)
                {
                    msgModel.Status = 2;
                    msgModel.LastUpdateTime = DateTime.Now;
                    msgModel.LastUpdateBy = TModel.MID;
                    CommonBase.Update<DB_Message_Model>(msgModel, list);
                }
            }
            else  //回复的第二条以上的消息
            {
                List<DB_ResponseMessage_Model> listReMsg = CommonBase.GetList<DB_ResponseMessage_Model>("IsDeleted=0 and Status=1 and  ReceiveCode='" + TModel.ID + "' and RMcode='" + mcode + "' Order by CreatedTime desc");
                foreach (DB_ResponseMessage_Model model in listReMsg)
                {
                    model.Status = 2;
                    model.LastUpdateBy = TModel.MID;
                    model.LastUpdateTime = DateTime.Now;
                    CommonBase.Update<DB_ResponseMessage_Model>(model, list);
                }
            }
        }
    }
}
