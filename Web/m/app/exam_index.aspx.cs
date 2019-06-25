using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace Web.m.app
{
    /// <summary>
    /// 开始答题页面
    /// </summary>
    public partial class exam_index : BasePage
    {
        protected string PaperName = string.Empty, QuestionCount = string.Empty, EvaluationHeaderCode = string.Empty;
        protected override void SetPowerZone()
        {
            string paperCode = Request.QueryString["code"];
            EvaluationHeaderCode = paperCode;
            T_EvaluationHeader header = CommonBase.GetModel<T_EvaluationHeader>(paperCode);
            if (header != null)
            {
                ////再查询出试卷
                //T_EvaluationPaper paper = CommonBase.GetModel<T_EvaluationPaper>(header.PaperCode);
                //if(paper != null)
                //{
                PaperName = header.PaperCode;
                //}
                QuestionCount = header.QuestionCount.ToString();
            }
        }
        protected override string btnAdd_Click()
        {
            try
            {
                string pram = Request.Form["ans"];
                string evaluationCode = Request.Form["evaluationCode"];

                List<T_EvaluationDetail> list = JsonHelper.JsonToList<List<Model.T_EvaluationDetail>>(pram);
                List<CommonObject> listCommon = new List<CommonObject>();
                foreach (T_EvaluationDetail detail in list)
                {
                    if (detail.Answer.IndexOf("'") > 0)
                    {
                        detail.Answer = detail.Answer.Replace("'", "@");
                    }
                    CommonBase.Update<T_EvaluationDetail>(detail, new string[] { "Answer" }, listCommon);
                }
                CommonBase.RunListCommit(listCommon);
                //查询到这次测评
                T_EvaluationHeader header = CommonBase.GetModel<T_EvaluationHeader>(evaluationCode);
                if (header == null)
                {
                    return CommonHelper.Response(false, "操作失败：未查询到测评试卷！");
                }
                //更新测评试题的正确或错误
                List<T_EvaluationDetail> listDetail = CommonBase.GetList<T_EvaluationDetail>("HeaderCode='" + evaluationCode + "'");
                int CorrectCount = 0, ErrorCount = 0, AnswerCount = 0;
                foreach (T_EvaluationDetail detail in listDetail)
                {
                    if (detail.Answer != null && !string.IsNullOrEmpty(detail.Answer.Trim()))
                    {
                        AnswerCount++;
                        if (detail.Answer.Trim() == detail.WordEnglish.Trim())
                        {
                            CorrectCount++;
                            detail.Status = 1;
                        }
                        else
                        {
                            ErrorCount++;
                            detail.Status = 2;
                        }
                    }
                    else
                    {
                        detail.Status = 0;
                    }
                    CommonBase.Update<T_EvaluationDetail>(detail, new string[] { "Status" }, listCommon);
                }
                header.EvalEndTime = DateTime.Now;
                header.AnswerCount = AnswerCount;
                header.CorrectCount = CorrectCount;
                header.ErrorCount = ErrorCount;
                CommonBase.Update<T_EvaluationHeader>(header, new string[] { "EvalEndTime", "AnswerCount", "CorrectCount", "ErrorCount" }, listCommon);
                //测评次数字段+1
                TModel.SendRedBagCount = TModel.SendRedBagCount + 1;
                CommonBase.Update<Model.Member>(TModel, new string[] { "SendRedBagCount" }, listCommon);
                Service.LogService.Log(TModel, "1", "提交试卷", listCommon);
                if (CommonBase.RunListCommit(listCommon))
                {
                    return CommonHelper.Response(true, header.Code);
                }
                else
                {
                    return CommonHelper.Response(false, "操作失败，请重试");
                }
            }
            catch (Exception ex)
            {
                return CommonHelper.Response(false, ex.ToString());
            }
        }

        protected override string btnQuery_Click()
        {
            string pageIndex = Request["pageIndex"];
            string paSize = Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string evaluationCode = Request["evaluationCode"];
            string strWhere = "HeaderCode='" + evaluationCode + "'";

            string pram = Request["ans"];
            List<T_EvaluationDetail> list = JsonHelper.JsonToList<List<Model.T_EvaluationDetail>>(pram);
            List<CommonObject> listCommon = new List<CommonObject>();
            foreach (T_EvaluationDetail detail in list)
            {
                CommonBase.Update<T_EvaluationDetail>(detail, new string[] { "Answer" }, listCommon);
            }
            CommonBase.RunListCommit(listCommon);

            string tables = "T_EvaluationDetail";
            string fields = "*";
            string orderBy = "Sort ASC";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, fields, orderBy, tables);
            return JsonHelper.DataSetToJson(dt);
        }
    }
}