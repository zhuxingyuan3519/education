using DBUtility;
using MethodHelper;
using Model;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Web.Admin.Handler
{
    /// <summary>
    /// FHLogList 的摘要说明
    /// </summary>
    public class ImportWordList: BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "'1'='1' and t1.IsDeleted=0 ";
            string fields = "*";
            string tables = "";
            string sortBy = "t2.English asc";
            string export = context.Request["export"];

            if(!string.IsNullOrEmpty(context.Request["word"]) && context.Request["word"] == "1")
            {
                //词库查询
                fields = "t1.*";
                tables = "T_Words t1";
                sortBy = "t1.English asc";
                strWhere += " and t1.Version='1' ";

                if(!string.IsNullOrEmpty(context.Request["nTitle"]))
                    strWhere += " and t1.English like '" + context.Request["nTitle"] + "%'";
                if(!string.IsNullOrEmpty(context.Request["nName"]))
                    strWhere += " and t1.Chinese like '%" + context.Request["nName"] + "%'";
            }
            else
            {
                //教材词集
                fields = "t1.Code, t1.Version, t1.Grade, t1.Leavel, t1.Unit, t1.WIndex,t1.WordCode,t2.English,t2.HotWord, t2.Phonetic,t2.Chinese,t2.Module1,t2.Module2,t2.Module3,t2.Module4,t2.Association1,t2.Association2,t2.Association3,t2.Association4";
                tables = "T_VersionVsWords t1 LEFT JOIN dbo.T_Words t2 ON t1.WordCode=t2.Code";
                //strWhere += " and t2.English is not null ";
                if(context.Request["export"] == "1" && context.Request["nomatch"] == "1")
                {
                    strWhere += " AND t2.Module1='' AND t2.HotWord='' ";
                }
                else if(context.Request["export"] == "1" && context.Request["nomatch"] == "0")
                {
                    strWhere += " and t2.English is not null ";
                }
                if(!string.IsNullOrEmpty(context.Request["nTitle"]))
                    strWhere += " and t2.English like '" + context.Request["nTitle"] + "%'";
                if(!string.IsNullOrEmpty(context.Request["nName"]))
                    strWhere += " and t2.Chinese like '%" + context.Request["nName"] + "%'";
                if(!string.IsNullOrEmpty(context.Request["ddl_Version"]))
                    strWhere += " and t1.Version='" + context.Request["ddl_Version"] + "'";
                if(!string.IsNullOrEmpty(context.Request["ddl_Grade"])) //会员昵称
                    strWhere += " and t1.Grade= '" + context.Request["ddl_Grade"] + "'";
                if(!string.IsNullOrEmpty(context.Request["ddl_Leavel"]))
                    strWhere += " and t1.Leavel='" + context.Request["ddl_Leavel"] + "'";
                if(!string.IsNullOrEmpty(context.Request["ddl_Unit"]))
                    strWhere += " and t1.Unit='" + context.Request["ddl_Unit"] + "'";

                sortBy = " CONVERT(INT,ISNULL(t1.Unit,1)) ASC ,CONVERT(INT,ISNULL(t1.WIndex,'0')) ASC";
            }



            if(!string.IsNullOrEmpty(context.Request["ddl_Sort"]))
            {
                if(context.Request["ddl_Sort"] == "1")
                    sortBy = "t2.English asc";
                else if(context.Request["ddl_Sort"] == "2")
                    sortBy = " CONVERT(INT,ISNULL(t1.Unit,1)) ASC ,CONVERT(INT,ISNULL(t1.WIndex,'0')) ASC";
            }
            //    strWhere += " and Leavel='" + context.Request["ddl_Leavel"] + "'";
            //if(!string.IsNullOrEmpty(context.Request["ddl_Leavel"]))
            //    strWhere += " and Leavel='" + context.Request["ddl_Leavel"] + "'";

            int count;

            if(export == "1" && context.Request["word"] == "1") //导出词库
            {
                fields = "t1.*";
                tables = "T_Words t1";
                pageIndex = 1;
                pageSize = int.MaxValue;
                sortBy = "t1.English asc";
            }
            else if(export == "1" && string.IsNullOrEmpty(context.Request["word"])) //导出教材词集
            {
                pageIndex = 1;
                pageSize = int.MaxValue;

                sortBy = " CONVERT(INT,ISNULL(t1.Unit,1)) ASC ,CONVERT(INT,ISNULL(t1.WIndex,'0')) ASC";
                if(!string.IsNullOrEmpty(context.Request["ddl_Sort"]))
                {
                    if(context.Request["ddl_Sort"] == "1")
                        sortBy = "t2.English asc";
                    else if(context.Request["ddl_Sort"] == "2")
                        sortBy = " CONVERT(INT,ISNULL(t1.Unit,1)) ASC ,CONVERT(INT,ISNULL(t1.WIndex,'0')) ASC";
                }
            }
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, fields, sortBy, tables, out count);
            //是否导出excel
            if(export == "1")
            {
                string wordParm = context.Request["word"];
                //如果该参数为空，那么就是导出教材词集
                #region 导出Excel
                string exportsort = context.Request["exportsort"];
                string fileName = string.Empty;
                if(string.IsNullOrEmpty(wordParm))
                {
                    //教材单元词集
                    fileName = GetDictNameByCode(context.Request["ddl_Version"], "BookVersion") + GetDictNameByCode(context.Request["ddl_Grade"], "Grade") + GetDictNameByCode(context.Request["ddl_Leavel"], "Leavel");
                    if(exportsort == "1")
                        fileName += "单词顺序";
                    if(context.Request["nomatch"] == "1")
                        fileName += "未匹配";
                    fileName += "-" + MethodHelper.CommonHelper.CreateNo();
                }
                else
                {
                    //导出词库
                    fileName += "记无忧单词库-" + MethodHelper.CommonHelper.CreateNo();
                }
                string files = HttpContext.Current.Server.MapPath("/Attachment/download/" + fileName + ".xlsx");
                DataTable dtResult = new DataTable();
                using(NpoiService excelHelper = new NpoiService(files))
                {
                    if(exportsort == "1")
                    {
                        #region 只是导出单词顺序
                        //dtResult.Columns.Add("单词");
                        //dtResult.Columns.Add("顺序");
                        //dtResult.Columns.Add("中文");

                        dtResult.Columns.Add("序号");
                        dtResult.Columns.Add("单词");
                        dtResult.Columns.Add("音标");
                        dtResult.Columns.Add("中文");

                        //查看不同的单元,添加单元名称
                        DataTable dtUnit = dt.DefaultView.ToTable(true, "Unit");
                        List<int> listUnit = new List<int>();
                        //按照顺序排序
                        foreach(DataRow unitRow in dtUnit.Rows)
                        {
                            string result = unitRow["Unit"].ToString();// System.Text.RegularExpressions.Regex.Replace(unitRow["Unit"].ToString(), @"[^0-9]+", "");
                            if(!listUnit.Contains(MethodHelper.ConvertHelper.ToInt32(result, 0)))
                                listUnit.Add(MethodHelper.ConvertHelper.ToInt32(result, 0));
                        }

                        listUnit.Sort();
                        int index = 1;
                        foreach(int unitSort in listUnit)
                        {
                            //添加一行单元名称
                            DataRow addRow = dtResult.NewRow();
                            addRow["单词"] = "Unit " + unitSort;
                            addRow["序号"] = "";
                            addRow["中文"] = "";
                            dtResult.Rows.Add(addRow);
                            //再查找该单元中的所有单词，按照顺序进行排序
                            DataRow[] unitWordList = dt.Select("Unit='" + unitSort + "'");
                            foreach(DataRow row in unitWordList)
                            {
                                //新的一行
                                addRow = dtResult.NewRow();
                                addRow["单词"] = row["English"] = row["English"].ToString().TrimEnd().TrimStart().Replace("@", "'").Replace("\"", "'");
                                addRow["序号"] = row["WIndex"];// index.ToString() + "、";
                                addRow["中文"] = row["Chinese"];
                                dtResult.Rows.Add(addRow);
                                index++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        dtResult.Columns.Add("序号");
                        dtResult.Columns.Add("单词");
                        dtResult.Columns.Add("音标");
                        dtResult.Columns.Add("中文");
                        dtResult.Columns.Add("Remark");
                        if(string.IsNullOrEmpty(wordParm))
                        {
                            #region 导出教材单元词集
                            //查看不同的单元,添加单元名称
                            DataTable dtUnit = dt.DefaultView.ToTable(true, "Unit");
                            List<int> listUnit = new List<int>();
                            //按照顺序排序
                            foreach(DataRow unitRow in dtUnit.Rows)
                            {
                                string result = unitRow["Unit"].ToString();// System.Text.RegularExpressions.Regex.Replace(unitRow["Unit"].ToString(), @"[^0-9]+", "");
                                if(!listUnit.Contains(MethodHelper.ConvertHelper.ToInt32(result, 0)))
                                    listUnit.Add(MethodHelper.ConvertHelper.ToInt32(result, 0));
                            }

                            listUnit.Sort();
                            int index = 1;
                            string unitSortString = "";
                            foreach(int unitSort in listUnit)
                            {
                                //添加一行单元名称
                                if(unitSort > 0)
                                {
                                    unitSortString = unitSort.ToString();
                                }
                                DataRow addRow = dtResult.NewRow();
                                addRow["序号"] = "";
                                addRow["单词"] = "Unit " + unitSortString;
                                addRow["音标"] = "";
                                addRow["中文"] = "";
                                dtResult.Rows.Add(addRow);
                                //再查找该单元中的所有单词，按照顺序进行排序
                                DataRow[] unitWordList = dt.Select("Unit='" + unitSortString + "'");
                                foreach(DataRow row in unitWordList)
                                {
                                    //新的一行
                                    addRow = dtResult.NewRow();
                                    addRow["序号"] = index.ToString() + "、";//row["WIndex"];//
                                    addRow["单词"] = row["English"] = row["English"].ToString().TrimEnd().Replace("@", "'").Replace("\"", "'");
                                    addRow["音标"] = row["Phonetic"];
                                    addRow["中文"] = row["Chinese"];
                                    dtResult.Rows.Add(addRow);

                                    //再添加一行熟词分解
                                    if(!string.IsNullOrEmpty(row["HotWord"].ToString()))
                                    {
                                        addRow = dtResult.NewRow();
                                        addRow["序号"] = "";
                                        addRow["单词"] = row["HotWord"];
                                        addRow["音标"] = "";
                                        addRow["中文"] = "";
                                        dtResult.Rows.Add(addRow);
                                    }


                                    //添加一行模块拆分
                                    dtResult = AddDataRow(dtResult, "模块拆分", row["Module1"].ToString(), row["Chinese"].ToString(), false);
                                    dtResult = AddDataRow(dtResult, "情景联想", row["Association1"].ToString(), row["Chinese"].ToString(), true);

                                    dtResult = AddDataRow(dtResult, "模块拆分", row["Module2"].ToString(), row["Chinese"].ToString(), false);
                                    dtResult = AddDataRow(dtResult, "情景联想", row["Association2"].ToString(), row["Chinese"].ToString(), true);

                                    dtResult = AddDataRow(dtResult, "模块拆分", row["Module3"].ToString(), row["Chinese"].ToString(), false);
                                    dtResult = AddDataRow(dtResult, "情景联想", row["Association3"].ToString(), row["Chinese"].ToString(), true);

                                    dtResult = AddDataRow(dtResult, "模块拆分", row["Module4"].ToString(), row["Chinese"].ToString(), false);
                                    dtResult = AddDataRow(dtResult, "情景联想", row["Association4"].ToString(), row["Chinese"].ToString(), true);

                                    //dtResult = AddDataRow(dtResult, "", string.Empty, string.Empty, false);


                                    //加一行空白行
                                    addRow = dtResult.NewRow();
                                    addRow["序号"] = string.Empty;
                                    addRow["单词"] = string.Empty;
                                    addRow["音标"] = string.Empty;
                                    addRow["中文"] = string.Empty;
                                    dtResult.Rows.Add(addRow);
                                    index++;
                                }

                            }
                            #endregion
                        }
                        else
                        {
                            #region 导出词库
                            int index = 1;
                            foreach(DataRow row in dt.Rows)
                            {
                                //新的一行
                                DataRow addRow = dtResult.NewRow();
                                addRow["序号"] = index.ToString() + "、";
                                addRow["单词"] = row["English"] = row["English"].ToString().TrimEnd().Replace("@", "'").Replace("\"", "'");
                                addRow["音标"] = row["Phonetic"];
                                addRow["中文"] = row["Chinese"];
                                dtResult.Rows.Add(addRow);

                                //再添加一行熟词分解
                                if(!string.IsNullOrEmpty(row["HotWord"].ToString()))
                                {
                                    addRow = dtResult.NewRow();
                                    addRow["序号"] = "";
                                    addRow["单词"] = row["HotWord"];
                                    addRow["音标"] = "";
                                    addRow["中文"] = "";
                                    dtResult.Rows.Add(addRow);
                                }


                                //添加一行模块拆分
                                dtResult = AddDataRow(dtResult, "模块拆分", row["Module1"].ToString(), row["Chinese"].ToString(), false);
                                dtResult = AddDataRow(dtResult, "情景联想", row["Association1"].ToString(), row["Chinese"].ToString(), true);

                                dtResult = AddDataRow(dtResult, "模块拆分", row["Module2"].ToString(), row["Chinese"].ToString(), false);
                                dtResult = AddDataRow(dtResult, "情景联想", row["Association2"].ToString(), row["Chinese"].ToString(), true);

                                dtResult = AddDataRow(dtResult, "模块拆分", row["Module3"].ToString(), row["Chinese"].ToString(), false);
                                dtResult = AddDataRow(dtResult, "情景联想", row["Association3"].ToString(), row["Chinese"].ToString(), true);

                                dtResult = AddDataRow(dtResult, "模块拆分", row["Module4"].ToString(), row["Chinese"].ToString(), false);
                                dtResult = AddDataRow(dtResult, "情景联想", row["Association4"].ToString(), row["Chinese"].ToString(), true);

                                //dtResult = AddDataRow(dtResult, "", string.Empty, string.Empty, false);


                                //加一行空白行
                                addRow = dtResult.NewRow();
                                addRow["序号"] = string.Empty;
                                addRow["单词"] = string.Empty;
                                addRow["音标"] = string.Empty;
                                addRow["中文"] = string.Empty;
                                dtResult.Rows.Add(addRow);
                                index++;
                            }
                            #endregion
                        }
                    }
                    int resultCount = excelHelper.ExportWordStorgeExcel(dtResult, "Sheet1", true);
                    if(resultCount == -1)
                    {
                        context.Response.Write(MethodHelper.CommonHelper.Response(false, "导出失败，请重试"));
                    }
                    else
                    {
                        context.Response.Write(MethodHelper.CommonHelper.Response(true, "/Attachment/download/" + fileName + ".xlsx"));
                    }
                }
                #endregion
            }
            else
            {
                #region 普通的查询
                string version = string.Empty;
                string Module1 = string.Empty;
                string Module2 = string.Empty;
                string Module3 = string.Empty;
                string Module4 = string.Empty;
                foreach(DataRow row in dt.Rows)
                {
                    version = string.Empty;
                    Module1 = string.Empty;
                    Module2 = string.Empty;
                    Module3 = string.Empty;
                    if(dt.Columns.Contains("Version"))
                    {
                        row["Version"] = DictService.GetDictValue(row["Version"].ToString(), "BookVersion");
                    }
                    if(dt.Columns.Contains("Grade"))
                    {
                        row["Grade"] = DictService.GetDictValue(row["Grade"].ToString(), "Grade");
                    }
                    if(dt.Columns.Contains("Leavel"))
                    {
                        row["Leavel"] = DictService.GetDictValue(row["Leavel"].ToString(), "Leavel");
                    }
                    if(dt.Columns.Contains("Unit"))
                    {
                        row["Unit"] = DictService.GetDictValue(row["Unit"].ToString(), "Unit");
                    }
                    row["Chinese"] = row["Chinese"].ToString().TrimEnd();
                    row["English"] = row["English"].ToString().TrimEnd().Replace("@", "'").Replace("\"", "'");
                    //记忆方法：
                    if(!string.IsNullOrEmpty(row["Module1"].ToString()))
                        Module1 += "模块拆分：" + row["Module1"].ToString() + "<br/>";
                    if(!string.IsNullOrEmpty(row["Association1"].ToString()))
                        Module1 += "情景联想：" + NpoiService.SetNewWordAssociation(NpoiService.GetNewString(row["Chinese"].ToString()), row["Association1"].ToString());
                    row["Module1"] = Module1;


                    if(!string.IsNullOrEmpty(row["Module2"].ToString()))
                        Module2 += "模块拆分：" + row["Module2"].ToString() + "<br/>";
                    if(!string.IsNullOrEmpty(row["Association2"].ToString()))
                        Module2 += "情景联想：" + NpoiService.SetNewWordAssociation(NpoiService.GetNewString(row["Chinese"].ToString()), row["Association2"].ToString());
                    row["Module2"] = Module2;


                    if(!string.IsNullOrEmpty(row["Module3"].ToString()))
                        Module3 += "模块拆分：" + row["Module3"].ToString() + "<br/>";
                    if(!string.IsNullOrEmpty(row["Association3"].ToString()))
                        Module3 += "情景联想：" + NpoiService.SetNewWordAssociation(NpoiService.GetNewString(row["Chinese"].ToString()), row["Association3"].ToString());
                    row["Module3"] = Module3;

                    if(!string.IsNullOrEmpty(row["Module4"].ToString()))
                        Module4 += "模块拆分：" + row["Module4"].ToString() + "<br/>";
                    if(!string.IsNullOrEmpty(row["Association4"].ToString()))
                        Module4 += "情景联想：" + NpoiService.SetNewWordAssociation(NpoiService.GetNewString(row["Chinese"].ToString()), row["Association4"].ToString());
                    row["Module3"] = Module3;
                }
                context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

                #endregion
            }
        }


        protected DataTable AddDataRow(DataTable dtResult, string xuhao, string word, string chinese, bool isAddRemark)
        {
            if(!string.IsNullOrEmpty(word))
            {
                DataRow addRow = dtResult.NewRow();
                addRow["序号"] = xuhao;
                addRow["单词"] = word;
                addRow["音标"] = string.Empty;
                addRow["中文"] = string.Empty;
                if(isAddRemark)
                    addRow["Remark"] = NpoiService.GetNewString(chinese);
                dtResult.Rows.Add(addRow);
            }
            return dtResult;
        }



    }




}