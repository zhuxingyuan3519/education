using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace Web.m.app
{
    public partial class code_start_train: BasePage
    {

        protected bool CheckPrivage(string codeType)
        {
            bool result = true;
            //查看是否有权限
            string chaegeList = TModel.Learns;
            List<TD_ChargeList> listChargeList = CacheService.ChargeList;

            TD_ChargeList charge = listChargeList.FirstOrDefault(c => c.ChargeList.Contains(codeType));
            if(charge != null)
            {
                if(!string.IsNullOrEmpty(chaegeList) && chaegeList.Contains(codeType))
                { }
                else
                {
                    //您无权限
                    ErrMsg = "对不起，您无此训练权限";
                    result = false;
                }
            }
            return result;
        }


        protected string ErrMsg = string.Empty;
        protected override void SetPowerZone()
        {
            string codeType = Request.QueryString["codeType"];

            hid_codeType.Value = codeType;
            string showCount = Request.QueryString["trainCount"];
            int pageSize = MethodHelper.ConvertHelper.ToInt32(showCount, 1);
            int showTime = MethodHelper.ConvertHelper.ToInt32(Request.QueryString["showTime"], 0);
            hid_showTime.Value = showTime.ToString();
            //查看是不是复习考题的，如果是复习考题，就只有一个参数reviewCode
            string reviewCode = Request.QueryString["reviewCode"];
            hid_reviewcode.Value = reviewCode;
            if(!string.IsNullOrEmpty(reviewCode))
            {
                //获取到训练的主表
                T_TrainHeader header = CommonBase.GetModel<T_TrainHeader>(reviewCode);
                if(header == null)
                {
                    ErrMsg = "初始化训练项目失败";
                    return;
                }
                codeType = header.CodeType;
                if(!CheckPrivage(codeType))
                    return;
                hid_codeType.Value = codeType;
                pageSize = header.TrainCount;
                //showTime = header.ShowTime;
                //hid_showTime.Value = showTime.ToString();
                hid_trainCode.Value = header.Code;
                //更新复习时间
                header.Remark = DateTime.Now.ToString();
                if(showTime > 0)//如果是闪现，当前时间+4秒
                {
                    header.Remark = DateTime.Now.AddSeconds(4).ToString();
                }
                List<CommonObject> listComm = new List<CommonObject>();
                CommonBase.Update<T_TrainHeader>(header, new string[] { "Remark" }, listComm);
                CommonBase.RunListCommit(listComm);
                //查询到明细表
                if(header.CodeType == "3")
                {
                    #region 扑克牌复习
                    string orderBy = "Sort asc";
                    int currentIndex = 1;
                    string strWhere = "IsDeleted=0 and TrainCode='" + header.Code + "'";
                    int count;
                    DataTable dt = CommonBase.GetPageDataTable(currentIndex, 10000, strWhere, "CodeName", orderBy, "T_TrainDetail", out count);
                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("CodeWord");
                    dtResult.Columns.Add("CodeImage");
                    dtResult.Columns.Add("PuKeNum");
                    dtResult.Columns.Add("Remark");
                    dtResult.Columns.Add("RowNumber");
                    //获取到所有扑克牌密码
                    List<T_CodingMemory> listPuKe = CommonBase.GetList<T_CodingMemory>("IsDeleted=0 and CodeType='3'");
                    int RowNumber = 1;
                    foreach(DataRow row in dt.Rows)
                    {
                        string codeDetail = row["CodeName"].ToString();
                        string pkType = codeDetail.Substring(0, 1);
                        string pkNum = codeDetail.Substring(1, codeDetail.Length - 1);
                        T_CodingMemory puke = listPuKe.FirstOrDefault(c => c.Remark == pkType && c.PuKeNum == pkNum);
                        if(puke != null)
                        {
                            DataRow newNow = dtResult.NewRow();
                            newNow["CodeWord"] = puke.PKCode;
                            newNow["CodeImage"] = puke.CodeImage;
                            newNow["PuKeNum"] = puke.PuKeNum;
                            newNow["Remark"] = puke.Remark;
                            newNow["RowNumber"] = RowNumber;

                            dtResult.Rows.Add(newNow);
                        }
                        RowNumber++;
                    }
                    //看是不是闪现
                    if(showTime > 0)
                    {
                        rep_list.Visible = false;
                        hid_codevalue.Value = MethodHelper.JsonHelper.DataTableToJson(dtResult, "CodeNames");
                        //ExecuteScript("var data=" + );
                    }
                    else
                    {
                        rep_list.DataSource = dtResult;
                        rep_list.DataBind();
                    }
                    #endregion
                }
                else
                {
                    #region 其他类型复习
                    string orderBy = "Sort asc";
                    int currentIndex = 1;
                    string strWhere = "IsDeleted=0 and TrainCode='" + header.Code + "'";
                    int count;
                    DataTable dt = CommonBase.GetPageDataTable(currentIndex, 10000, strWhere, "CodeName CodeWord", orderBy, "T_TrainDetail", out count);
                    //看是不是闪现
                    if(showTime > 0)
                    {
                        rep_list.Visible = false;
                        hid_codevalue.Value = MethodHelper.JsonHelper.DataTableToJson(dt, "CodeNames");
                        //ExecuteScript("var data=" + );
                    }
                    else
                    {
                        rep_list.DataSource = dt;
                        rep_list.DataBind();
                    }
                    #endregion
                }
            }
            else
            {
                if(!CheckPrivage(codeType))
                    return;
                //正常训练
                //codeType=1:初级混合词，2-数字训练，3-扑克牌训练，4-字母训练，5-高级混合词
                if(codeType == "1"||codeType=="5")
                {
                    #region 1:混合词
                    string orderBy = "NEWID()";
                    int currentIndex = 1;
                    string strWhere = "IsDeleted=0";
                    if(codeType == "1")
                        strWhere += " and Status=1";
                    else if(codeType=="5")
                        strWhere += " and Status=2";
                    int count;
                    DataTable dt = CommonBase.GetPageDataTable(currentIndex, pageSize, strWhere, "CodeWord", orderBy, "T_CodingWord", out count);
                    //对查出来的这些保存到训练表中
                    //保存主表
                    List<CommonObject> listComm = new List<CommonObject>();
                    //查看是不是有未结束的
                    #region 保存到主表
                    string getSql = "select Code from T_TrainHeader where (EndTime ='' or AnswerBeginTime ='' OR AnswerEndTime ='')  and UserCode='" + TModel.ID + "'";
                    object objCode = CommonBase.GetSingle(getSql);
                    bool isUpdate = false;
                    T_TrainHeader header = new T_TrainHeader();
                    if(objCode != null)
                    {
                        isUpdate = true;
                        header = CommonBase.GetModel<T_TrainHeader>(objCode);
                        //删除之前没有结束的明细
                        string deleteDetail = "delete from T_TrainDetail where TrainCode='" + header.Code + "'";
                        listComm.Add(new CommonObject(deleteDetail, null));
                    }
                    else
                    {
                        header.Code = GetGuid;
                    }
                    if(showTime > 0)
                        header.BeginTime = DateTime.Now.AddSeconds(4);//增加4秒
                    else
                        header.BeginTime = DateTime.Now;
                    header.EndTime = DBNull.Value.ToString();
                    header.ReviewTime = 0;
                    header.AnswerBeginTime = DBNull.Value.ToString();
                    header.AnswerEndTime = DBNull.Value.ToString();
                    header.CorrectCount = 0;
                    header.ErrorCount = 0;


                    header.CodeType = codeType;
                    header.CreatedTime = DateTime.Now;
                    header.IsDeleted = false;
                    header.ShowTime = showTime;
                    header.Sort = 1;
                    header.Status = 1;
                    header.TrainCount = pageSize;
                    header.UserCode = TModel.ID;
                    if(isUpdate)
                        CommonBase.Update<T_TrainHeader>(header, listComm);
                    else
                        CommonBase.Insert<T_TrainHeader>(header, listComm);
                    #region 再插入明细表
                    //再插入明细表
                    int i = 1;
                    foreach(DataRow row in dt.Rows)
                    {
                        T_TrainDetail detail = new T_TrainDetail();
                        detail.Code = GetGuid;
                        detail.CodeName = row["CodeWord"].ToString();
                        detail.CreatedTime = DateTime.Now;
                        detail.IsDeleted = false;
                        detail.Sort = i;
                        detail.Status = 1;
                        detail.TrainCode = header.Code;
                        i++;
                        CommonBase.Insert<T_TrainDetail>(detail, listComm);
                    }
                    #endregion
                    #endregion
                    hid_trainCode.Value = header.Code;
                    if(CommonBase.RunListCommit(listComm))
                    {
                        //看是不是闪现
                        if(showTime > 0)
                        {
                            rep_list.Visible = false;
                            hid_codevalue.Value = MethodHelper.JsonHelper.DataTableToJson(dt, "CodeNames");
                            //ExecuteScript("var data=" + );
                        }
                        else
                        {
                            rep_list.DataSource = dt;
                            rep_list.DataBind();
                        }
                    }
                    else
                    {
                        //ScriptAlert("初始化训练失败，请联系管理员");
                        ErrMsg = "初始化训练失败，请联系管理员";
                    }
                    #endregion
                }
                else if(codeType == "2" || codeType == "4")
                {
                    #region 数字训练或字母训练，0-9数字取出随机的两位
                    string[] numArray = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                    string[] wordArray = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                    //保存主表
                    List<CommonObject> listComm = new List<CommonObject>();
                    #region 保存到主表
                    string getSql = "select Code from T_TrainHeader where (EndTime ='' or AnswerBeginTime ='' OR AnswerEndTime ='')  and UserCode='" + TModel.ID + "'";
                    object objCode = CommonBase.GetSingle(getSql);
                    bool isUpdate = false;
                    T_TrainHeader header = new T_TrainHeader();
                    if(objCode != null)
                    {
                        isUpdate = true;
                        header = CommonBase.GetModel<T_TrainHeader>(objCode);
                        //删除之前没有结束的明细
                        string deleteDetail = "delete from T_TrainDetail where TrainCode='" + header.Code + "'";
                        listComm.Add(new CommonObject(deleteDetail, null));
                    }
                    else
                    {
                        header.Code = GetGuid;
                    }
                    if(showTime > 0)
                        header.BeginTime = DateTime.Now.AddSeconds(4);//增加4秒
                    else
                        header.BeginTime = DateTime.Now;
                    header.EndTime = DBNull.Value.ToString();
                    header.ReviewTime = 0;
                    header.AnswerBeginTime = DBNull.Value.ToString();
                    header.AnswerEndTime = DBNull.Value.ToString();
                    header.CorrectCount = 0;
                    header.ErrorCount = 0;
                    header.CodeType = codeType;
                    header.CreatedTime = DateTime.Now;
                    header.IsDeleted = false;
                    header.ShowTime = showTime;
                    header.Sort = 1;
                    header.Status = 1;
                    header.TrainCount = pageSize;
                    header.UserCode = TModel.ID;
                    if(isUpdate)
                        CommonBase.Update<T_TrainHeader>(header, listComm);
                    else
                        CommonBase.Insert<T_TrainHeader>(header, listComm);
                    #region 再插入明细表
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CodeWord");
                    //再插入明细表
                    for(int i = 1; i <= pageSize; i++)
                    {
                        DataRow row = dt.NewRow();
                        T_TrainDetail detail = new T_TrainDetail();
                        detail.Code = GetGuid;
                        if(codeType == "4")
                            detail.CodeName = GetRandomCodeString(wordArray, 2).ToUpper();
                        else
                            detail.CodeName = GetRandomCodeString(numArray, 2).ToUpper();
                        row["CodeWord"] = detail.CodeName;
                        dt.Rows.Add(row);
                        detail.CreatedTime = DateTime.Now;
                        detail.IsDeleted = false;
                        detail.Sort = i;
                        detail.Status = 1;
                        detail.TrainCode = header.Code;
                        CommonBase.Insert<T_TrainDetail>(detail, listComm);
                    }
                    #endregion
                    #endregion

                    hid_trainCode.Value = header.Code;
                    if(CommonBase.RunListCommit(listComm))
                    {
                        //看是不是闪现
                        if(showTime > 0)
                        {
                            rep_list.Visible = false;
                            hid_codevalue.Value = MethodHelper.JsonHelper.DataTableToJson(dt, "CodeNames");
                            //ExecuteScript("var data=" + );
                        }
                        else
                        {
                            rep_list.DataSource = dt;
                            rep_list.DataBind();
                        }
                    }
                    else
                    {
                        //ScriptAlert("初始化训练失败，请联系管理员");
                        ErrMsg = "初始化训练失败，请联系管理员";
                    }
                    #endregion
                }
                else if(codeType == "3")
                {
                    #region 扑克牌训练
                    string orderBy = "NEWID()";
                    int currentIndex = 1;
                    string strWhere = "CodeType='3' and IsDeleted=0";
                    int count;
                    DataTable dt = CommonBase.GetPageDataTable(currentIndex, pageSize, strWhere, "PKCode  CodeWord,CodeImage,PuKeNum,Remark", orderBy, "T_CodingMemory", out count);
                    //对查出来的这些保存到训练表中
                    //保存主表
                    List<CommonObject> listComm = new List<CommonObject>();
                    //查看是不是有未结束的
                    #region 保存到主表
                    string getSql = "select Code from T_TrainHeader where (EndTime ='' or AnswerBeginTime ='' OR AnswerEndTime ='')  and UserCode='" + TModel.ID + "'";
                    object objCode = CommonBase.GetSingle(getSql);
                    bool isUpdate = false;
                    T_TrainHeader header = new T_TrainHeader();
                    if(objCode != null)
                    {
                        isUpdate = true;
                        header = CommonBase.GetModel<T_TrainHeader>(objCode);
                        //删除之前没有结束的明细
                        string deleteDetail = "delete from T_TrainDetail where TrainCode='" + header.Code + "'";
                        listComm.Add(new CommonObject(deleteDetail, null));
                    }
                    else
                    {
                        header.Code = GetGuid;
                    }
                    if(showTime > 0)
                        header.BeginTime = DateTime.Now.AddSeconds(4);//增加4秒
                    else
                        header.BeginTime = DateTime.Now;
                    header.EndTime = DBNull.Value.ToString();
                    header.ReviewTime = 0;
                    header.AnswerBeginTime = DBNull.Value.ToString();
                    header.AnswerEndTime = DBNull.Value.ToString();
                    header.CorrectCount = 0;
                    header.ErrorCount = 0;
                    header.CodeType = codeType;
                    header.CreatedTime = DateTime.Now;
                    header.IsDeleted = false;
                    header.ShowTime = showTime;
                    header.Sort = 1;
                    header.Status = 1;
                    header.TrainCount = pageSize;
                    header.UserCode = TModel.ID;
                    if(isUpdate)
                        CommonBase.Update<T_TrainHeader>(header, listComm);
                    else
                        CommonBase.Insert<T_TrainHeader>(header, listComm);
                    #region 再插入明细表
                    //再插入明细表
                    int i = 1;
                    foreach(DataRow row in dt.Rows)
                    {
                        T_TrainDetail detail = new T_TrainDetail();
                        detail.Code = GetGuid;
                        detail.CodeName = row["Remark"].ToString() + row["PuKeNum"].ToString();
                        detail.CreatedTime = DateTime.Now;
                        detail.IsDeleted = false;
                        detail.Sort = i;
                        detail.Status = 1;
                        detail.TrainCode = header.Code;
                        i++;
                        CommonBase.Insert<T_TrainDetail>(detail, listComm);
                    }
                    #endregion
                    #endregion
                    hid_trainCode.Value = header.Code;
                    if(CommonBase.RunListCommit(listComm))
                    {
                        //看是不是闪现
                        if(showTime > 0)
                        {
                            rep_list.Visible = false;
                            hid_codevalue.Value = MethodHelper.JsonHelper.DataTableToJson(dt, "CodeNames");
                            //ExecuteScript("var data=" + );
                        }
                        else
                        {
                            rep_list.DataSource = dt;
                            rep_list.DataBind();
                        }
                    }
                    else
                    {
                        //ScriptAlert("初始化训练失败，请联系管理员");
                        ErrMsg = "初始化训练失败，请联系管理员";
                    }

                    #endregion
                }
            }
        }
        protected string GetRandomCodeString(string[] array, int n)
        {
            string s = "";
            for(int i = 0; i < n; i++)
            {
                if(s.Length < n)
                {
                    //Random ran = new Random();
                    int arrayIndex = GetRandom().Next(0, array.Length - 1);
                    s += array[arrayIndex];
                }
                else
                {
                    break;
                }
            }
            return s;
        }

        public Random GetRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
            //Thread.Sleep(10);
            //long tick = DateTime.Now.Ticks;//一个以0.1纳秒为单位的时间戳，18位
            //int seed = int.Parse(tick.ToString().Substring(9)); //  int类型的最大值:  2147483647
            ////或者使用unchecked((int)tick)也可
            //return new Random(seed);
        }


    }
}