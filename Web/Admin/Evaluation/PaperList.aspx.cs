
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using MethodHelper;
using Model;
using Service;

namespace Web.Admin.Evaluation
{
    public partial class PaperList: BasePage
    {
        protected override void SetPowerZone()
        {
            GetDictBindDDL("BookVersion", ddl_Version);
            ddl_Version.Items.Insert(0, new ListItem("--版本--", ""));

            GetDictBindDDL("Grade", ddl_Grade);
            ddl_Grade.Items.Insert(0, new ListItem("--年级--", ""));

            GetDictBindDDL("Leavel", ddl_Leavel);
            ddl_Leavel.Items.Insert(0, new ListItem("--学期--", ""));

            GetDictBindDDL("Unit", ddl_unit);
            ddl_unit.Items.Insert(0, new ListItem("--章节--", ""));


            GetDictBindDDL("BookVersion", ddl_PaperVersion);
            ddl_PaperVersion.Items.Insert(0, new ListItem("--版本--", ""));

            GetDictBindDDL("Grade", ddl_PaperGrade);
            ddl_PaperGrade.Items.Insert(0, new ListItem("--年级--", ""));

            GetDictBindDDL("Leavel", ddl_PaperLeavel);
            ddl_PaperLeavel.Items.Insert(0, new ListItem("--学期--", ""));

            GetDictBindDDL("Unit", ddl_PaperUnit);
            ddl_PaperUnit.Items.Insert(0, new ListItem("--章节--", ""));
        }

        protected override string btnQuery_Click()
        {
            string pageIndex = Request["pageIndex"];
            string paSize = Request["pageSize"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);
            string strWhere = "t1.IsDeleted=0 ";

            string nRegistBeginTime = Request["nRegistBeginTime"];
            string ddl_Version = Request["ddl_Version"];
            string ddl_Grade = Request["ddl_Grade"];
            string ddl_Leavel = Request["ddl_Leavel"];
            string ddl_unit = Request["ddl_unit"];
            ////获取到unit name
            //ddl_unit = GetDictNameByCode(ddl_unit, "Unit");

            string name = Request["nName"];
            if(!string.IsNullOrEmpty(ddl_Version))
                strWhere += " and t1.Version='" + ddl_Version + "'";
            if(!string.IsNullOrEmpty(ddl_Grade))
                strWhere += " and t1.Grade='" + ddl_Grade + "'";
            if(!string.IsNullOrEmpty(ddl_Leavel))
                strWhere += " and t1.Leavel='" + ddl_Leavel + "'";
            if(!string.IsNullOrEmpty(ddl_unit))
                strWhere += " and t1.Unit='" + ddl_unit + "'";
            if(!string.IsNullOrEmpty(name))
                strWhere += " and t1.Name like '%" + name + "%'";

            string nRegistEndTime = Request["nRegistEndTime"];
            if(!string.IsNullOrEmpty(nRegistBeginTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistBeginTime) + " 00:00:00", out time);
                strWhere += " and t1.CreatedTime>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if(!string.IsNullOrEmpty(nRegistEndTime))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(nRegistEndTime) + " 23:59:59", out time);
                strWhere += " and t1.CreatedTime<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }


            string tables = "T_EvaluationPaper t1 ";
            string fields = "t1.*";
            string orderBy = "t1.CreatedTime DESC";
            int count;
            DataTable dt = CommonBase.GetPageDataTable(currentIndex, pageSize, strWhere, fields, orderBy, tables, out count);
            dt.Columns.Add("WordCount");
            dt.Columns.Add("EvaluationCount");
            foreach(DataRow row in dt.Rows)
            {
                //查询出来单词数量
                string wordCountWhere = "t2.English IS NOT NULL";
                if(!string.IsNullOrEmpty(row["Version"].ToString()))
                    wordCountWhere += " and t1.Version='" + row["Version"].ToString() + "'";
                if(!string.IsNullOrEmpty(row["Grade"].ToString()))
                    wordCountWhere += " and t1.Grade='" + row["Grade"].ToString() + "'";
                if(!string.IsNullOrEmpty(row["Leavel"].ToString()))
                    wordCountWhere += " and t1.Leavel='" + row["Leavel"].ToString() + "'";
                if(!string.IsNullOrEmpty(row["Unit"].ToString()))
                    wordCountWhere += " and t1.Unit='" + row["Unit"].ToString() + "'";
                string wordCountSQL = "SELECT ISNULL(COUNT(1),0) FROM dbo.T_VersionVsWords t1 LEFT JOIN dbo.T_Words t2 ON t1.WordCode=t2.Code where " + wordCountWhere;
                wordCountSQL += " UNION ALL ";
                wordCountSQL += " SELECT ISNULL(COUNT(DISTINCT UserCode),0) FROM dbo.T_EvaluationHeader WHERE PaperCode='" + row["Code"].ToString() + "' AND IsDeleted=0";
                DataTable dtCount = CommonBase.GetTable(wordCountSQL);
                if(dtCount != null && dtCount.Rows.Count > 1)
                {
                    row["WordCount"] = dtCount.Rows[0][0];
                    //查询出来参加测评的人数
                    row["EvaluationCount"] = dtCount.Rows[1][0];
                }
            }
            return JsonHelper.GetAdminDataTableToJson(dt, count);
        }

        protected override string btnAdd_Click()
        {
            string version = Request["version"];
            string grade = Request["grade"];
            string leavel = Request["leavel"];
            string unit = Request["unit"];
            //unit = 
            //查看是否已经存在
            string wordCountWhere = "1=1";
            if(!string.IsNullOrEmpty(version))
                wordCountWhere += " and t1.Version='" + version + "'";
            if(!string.IsNullOrEmpty(grade))
                wordCountWhere += " and t1.Grade='" + grade + "'";
            if(!string.IsNullOrEmpty(leavel))
                wordCountWhere += " and t1.Leavel='" + leavel + "'";
            if(!string.IsNullOrEmpty(unit))
                wordCountWhere += " and t1.Unit='" + unit + "'";
            string wordCountSQL = "SELECT ISNULL(COUNT(1),0) FROM dbo.T_EvaluationPaper t1 where " + wordCountWhere;
            if(MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(wordCountSQL), 0) > 0)
            {
                return CommonHelper.Response(false, "选择的范围内已存在测评试卷");
            }
            List<CommonObject> listCommon = new List<CommonObject>();
            T_EvaluationPaper paper = new T_EvaluationPaper();
            paper.Code = GetGuid;
            paper.CreatedBy = TModel.MID;
            paper.CreatedTime = DateTime.Now;
            paper.Grade = grade;
            paper.IsDeleted = false;
            paper.Leavel = leavel;
            paper.Name = GetDictNameByCode(version, "BookVersion") + GetDictNameByCode(grade, "Grade") + GetDictNameByCode(leavel, "Leavel") + GetDictNameByCode(unit, "Unit")+"单词测评";
            paper.Status = 1;
            paper.Sort = 1;
            paper.Unit = unit;
            paper.Version = version;
            CommonBase.Insert<T_EvaluationPaper>(paper, listCommon);
            LogService.Log(TModel, "1", "添加试卷" + paper.Name, listCommon);
            if(CommonBase.RunListCommit(listCommon))
                return CommonHelper.Response(true, "操作成功！");
            else
                return CommonHelper.Response(false, "操作失败，请重试！");
        }
    }
}