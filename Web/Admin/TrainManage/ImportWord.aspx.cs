﻿
using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.TrainManage
{
    public partial class ImportWord : BasePage
    {
        protected Model.EN_Video tempModel = null;
        protected override void SetPowerZone()
        {
            GetDictBindDDL("BookVersion", ddl_Version);
            ddl_Version.Items.Insert(0, new ListItem("--版本--", ""));

            GetDictBindDDL("Grade", ddl_Grade);
            ddl_Grade.Items.Insert(0, new ListItem("--年级--", ""));

            GetDictBindDDL("Leavel", ddl_Leavel);
            ddl_Leavel.Items.Insert(0, new ListItem("--学期--", ""));
        }
        protected override void SetValue(string code)
        {

        }

        protected override string btnAdd_Click()
        {
            try
            {
                //更新范围
                string option = Request.Form["ddl_option"];
                string version = Request.Form["ddl_Version"];
                string grade = Request.Form["ddl_Grade"];
                string leavel = Request.Form["ddl_Leavel"];
                string isCommon = Request.Form["ddl_isCommon"];
                DataTable dtResult = new DataTable();
                if (!string.IsNullOrEmpty(Request["hid_files"]))
                {
                    string files = HttpContext.Current.Server.MapPath("/Attachment/" + Request["hid_files"]);
                    using (NpoiService excelHelper = new NpoiService(files))
                    {
                        if (option == "4")
                        {
                            #region 导入文档再导出新的造句文档
                            DataTable dt = excelHelper.ExcelToDataTable(null, true);
                            //dt.Columns[0].ColumnName = "单词";
                            ////移除其他列
                            //if(dt.Columns.Count >= 2)
                            //{
                            //    for(int p = dt.Columns.Count - 1; p >= 1; p--)
                            //    {
                            //        dt.Columns.RemoveAt(p);
                            //    }
                            //}
                            //dt.Columns.Add("序号");
                            //dt.Columns.Add("音标");
                            //dt.Columns.Add("中文");
                            dt.Columns.Add("Remark");
                            dtResult = dt.Clone();

                            //查询到词库
                            List<T_Words> listWords = CommonBase.GetList<T_Words>("IsDeleted=0");
                            int cindex = 1;
                            foreach (DataRow row in dt.Rows)
                            {
                                string words = row["单词"].ToString();
                                if (words.IndexOf("'") > 0)
                                {
                                    words = words.Replace("'", "@");
                                }
                                if (string.IsNullOrEmpty(words.Trim()) && string.IsNullOrEmpty(row["序号"].ToString()) && string.IsNullOrEmpty(row["音标"].ToString()) && string.IsNullOrEmpty(row["中文"].ToString()))
                                {
                                    continue;
                                }

                                //查找单元
                                if ((!string.IsNullOrEmpty(words) && words.ToLower().StartsWith("unit")) || (!string.IsNullOrEmpty(words) && words.ToLower().StartsWith("module")))
                                {
                                    DataRow addRow = dtResult.NewRow();
                                    addRow["序号"] = "";
                                    addRow["单词"] = words;
                                    addRow["音标"] = "";
                                    addRow["中文"] = "";
                                    dtResult.Rows.Add(addRow);
                                }
                                else
                                {
                                    words = words.Replace("*", "");
                                    row["序号"] = cindex;
                                    cindex++;
                                    //if(string.IsNullOrEmpty(words))
                                    //    continue;
                                    T_Words wordModel = listWords.FirstOrDefault(c => c.English.ToLower().TrimEnd().TrimStart() == words.ToLower().TrimEnd().TrimStart());
                                    if (wordModel == null)
                                    {
                                        //row["音标"] = string.Empty;
                                        //row["中文"] = string.Empty;
                                        dtResult.ImportRow(row);
                                    }
                                    else
                                    {
                                        row["音标"] = wordModel.Phonetic;
                                        row["中文"] = wordModel.Chinese;
                                        dtResult.ImportRow(row);
                                        //添加一行熟词
                                        if (!string.IsNullOrEmpty(wordModel.HotWord))
                                        {
                                            DataRow addRow = dtResult.NewRow();
                                            addRow["序号"] = "";
                                            addRow["单词"] = wordModel.HotWord;
                                            addRow["音标"] = string.Empty;
                                            addRow["中文"] = string.Empty;
                                            dtResult.Rows.Add(addRow);
                                        }

                                        //添加一行模块拆分
                                        dtResult = AddDataRow(dtResult, "模块拆分", wordModel.Module1, wordModel.Chinese, false);
                                        dtResult = AddDataRow(dtResult, "情景联想", wordModel.Association1, wordModel.Chinese, true);

                                        //添加一行模块拆分
                                        dtResult = AddDataRow(dtResult, "模块拆分", wordModel.Module2, wordModel.Chinese, false);
                                        dtResult = AddDataRow(dtResult, "情景联想", wordModel.Association2, wordModel.Chinese, true);

                                        //添加一行模块拆分
                                        dtResult = AddDataRow(dtResult, "模块拆分", wordModel.Module3, wordModel.Chinese, false);
                                        dtResult = AddDataRow(dtResult, "情景联想", wordModel.Association3, wordModel.Chinese, true);

                                        //添加一行模块拆分
                                        dtResult = AddDataRow(dtResult, "模块拆分", wordModel.Module4, wordModel.Chinese, false);
                                        dtResult = AddDataRow(dtResult, "情景联想", wordModel.Association4, wordModel.Chinese, true);

                                    }
                                    DataRow addEmptyRow = dtResult.NewRow();
                                    addEmptyRow["序号"] = string.Empty;
                                    addEmptyRow["单词"] = string.Empty;
                                    addEmptyRow["音标"] = string.Empty;
                                    addEmptyRow["中文"] = string.Empty;
                                    dtResult.Rows.Add(addEmptyRow);
                                }
                            }
                            //重新设置Table列顺序
                            dtResult.Columns["序号"].SetOrdinal(0);
                            dtResult.Columns["单词"].SetOrdinal(1);
                            dtResult.Columns["音标"].SetOrdinal(2);
                            dtResult.Columns["中文"].SetOrdinal(3);
                            #endregion
                        }
                        else if (option == "5")
                        {
                            #region 导入已匹配文件，生成新的单词列表
                            DataTable dt = excelHelper.ExcelToDataTable(null, true);
                            dtResult = dt.Clone();
                            int sexIndex;
                            string seIndex = string.Empty, newIndex = string.Empty;
                            foreach (DataRow row in dt.Rows)
                            {
                                //看是否以数字开头
                                seIndex = row["序号"].ToString();
                                newIndex = seIndex.Trim().TrimEnd('、');
                                if (int.TryParse(newIndex, out sexIndex))
                                {
                                    dtResult.ImportRow(row);
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region 正常的导入
                            DataTable dt = excelHelper.ExcelToDataTable(null, true);
                            dt.Columns[0].ColumnName = "序号";
                            dt.Columns[1].ColumnName = "单词";
                            dt.Columns[2].ColumnName = "音标";
                            dt.Columns[3].ColumnName = "中文";
                            DataTable result = dt.Clone();
                            string index = "", word = "", yinbiao = "", chinese = "", unit = "", subIndex = "";
                            T_Words lastWord = null;
                            int outIndex;
                            int module = 0, association = 0;
                            List<T_Words> listWord = new List<T_Words>();
                            List<T_Words> listAllWards = CommonBase.GetList<T_Words>();

                            foreach (DataRow row in dt.Rows)
                            {
                                index = row["序号"].ToString();
                                word = row["单词"].ToString();

                                if (word.IndexOf("'") > 0)
                                {
                                    word = word.Replace("'", "@");
                                }
                                yinbiao = row["音标"].ToString();
                                chinese = row["中文"].ToString();
                                //所有列都为空，则跳过这一行
                                if (!string.IsNullOrEmpty(word))
                                {
                                    //查找单元
                                    if ((string.IsNullOrEmpty(index) && !string.IsNullOrEmpty(word) && word.ToLower().StartsWith("unit") && string.IsNullOrEmpty(yinbiao) && string.IsNullOrEmpty(chinese)) || (string.IsNullOrEmpty(index) && !string.IsNullOrEmpty(word) && word.ToLower().StartsWith("module") && string.IsNullOrEmpty(yinbiao) && string.IsNullOrEmpty(chinese)))
                                    {
                                        unit = word;
                                        //提取出单元中的数字
                                        unit = System.Text.RegularExpressions.Regex.Replace(unit, @"[^0-9]+", "");
                                    }
                                    subIndex = index.TrimEnd('、');
                                    //序号行是否能转换成数字，能转换成数字就是
                                    if (int.TryParse(subIndex, out outIndex))
                                    {
                                        module = 0;
                                        association = 0;
                                        index = outIndex.ToString();
                                        word = word.Replace("△", "").Replace("(be) ", "");
                                        T_Words wordExistInList = listWord.Where(c => c.English.TrimEnd().TrimStart() == word.TrimEnd().TrimStart()).OrderByDescending(c => c.CreatedTime).FirstOrDefault();
                                        listWord.Remove(wordExistInList);
                                        //如果已在集合中有了，下面的就要覆盖前面的
                                        //if(wordModel != null)
                                        //    continue;
                                        //查看该单词是不是在词库中存在
                                        string queryString = word;
                                        //if(word.IndexOf("'") > 0)
                                        //    queryString = queryString.Replace("'", "''");
                                        T_Words wordModel = listAllWards.Where(c => c.English.TrimEnd().TrimStart() == queryString.TrimEnd().TrimStart()).OrderByDescending(c => c.CreatedTime).FirstOrDefault();//  CommonBase.GetList<T_Words>("IsDeleted=0 and RTRIM(English)='" + queryString.TrimEnd().TrimStart() + "'").FirstOrDefault();
                                        //if(wordModel == null)
                                        //{
                                        //    wordModel = wordExistInList;
                                        //}
                                        if (wordModel == null)
                                        {
                                            //再查询已经存在的列表中是否有，主要查询这次的文档中是否存在重复的
                                            wordModel = new T_Words();
                                            wordModel.Version = isCommon;
                                            wordModel.Code = GetGuid;
                                            wordModel.CreatedTime = DateTime.Now;

                                            //wordModel.Version = version;
                                            wordModel.Grade = grade;
                                            wordModel.Leavel = leavel;
                                            wordModel.WIndex = index;

                                            wordModel.Unit = unit;
                                            wordModel.English = word.TrimEnd().TrimStart();
                                            wordModel.Phonetic = yinbiao;
                                            wordModel.Chinese = chinese;
                                            wordModel.Sort = outIndex;
                                            wordModel.HotWord = "";
                                            wordModel.Module1 = "";
                                            wordModel.Association1 = "";

                                            wordModel.Module2 = "";
                                            wordModel.Association2 = "";

                                            wordModel.Module3 = "";
                                            wordModel.Association3 = "";

                                            wordModel.Module4 = "";
                                            wordModel.Association4 = "";

                                            //lastWord = wordModel;
                                            //listWord.Add(wordModel);

                                            wordModel.IsDeleted = false;
                                            wordModel.IsUpdate = false;
                                        }
                                        else
                                        {
                                            if (option == "1" || option == "3")
                                            {
                                                if (wordModel.Version == "0")
                                                {
                                                    wordModel.Version = isCommon;
                                                }
                                            }
                                            if (option == "2")//只更新了教材
                                            {
                                                if (string.IsNullOrEmpty(wordModel.Chinese))
                                                    wordModel.Chinese = chinese;
                                                if (string.IsNullOrEmpty(wordModel.Phonetic))
                                                    wordModel.Chinese = yinbiao;
                                            }

                                            wordModel.Chinese = chinese;
                                            wordModel.WIndex = index;
                                            wordModel.Unit = unit;
                                            wordModel.IsUpdate = true;
                                            wordModel.HotWord = "";
                                            wordModel.Association1 = "";
                                            wordModel.Association3 = "";
                                            wordModel.Association4 = "";
                                            wordModel.Association2 = "";
                                            wordModel.Module1 = "";
                                            wordModel.Module2 = "";
                                            wordModel.Module3 = "";
                                            wordModel.Module4 = "";
                                        }
                                        lastWord = wordModel;
                                        listWord.Add(wordModel);
                                    }
                                    else
                                    {
                                        if (lastWord != null && string.IsNullOrEmpty(yinbiao) && !word.ToLower().StartsWith("unit"))
                                        {
                                            if (index == "" && word != "")
                                                lastWord.HotWord = word;
                                            if (index.StartsWith("模块拆分"))
                                            {
                                                module++;
                                                switch (module)
                                                {
                                                    case 1: lastWord.Module1 = word; break;
                                                    case 2: lastWord.Module2 = word; break;
                                                    case 3: lastWord.Module3 = word; break;
                                                    case 4: lastWord.Module4 = word; break;
                                                }
                                            }

                                            if (index.StartsWith("情景联想"))
                                            {
                                                association++;
                                                switch (association)
                                                {
                                                    case 1: lastWord.Association1 = word; break;
                                                    case 2: lastWord.Association2 = word; break;
                                                    case 3: lastWord.Association3 = word; break;
                                                    case 4: lastWord.Association4 = word; break;
                                                }
                                            }

                                            //从集合中移除原有的，把现在这个加入进去
                                            T_Words toRemoveModel = listWord.FirstOrDefault(c => c.Code == lastWord.Code);
                                            if (toRemoveModel != null)
                                                listWord.Remove(toRemoveModel);
                                            listWord.Add(lastWord);

                                        }
                                    }
                                }
                            }
                            List<CommonObject> listCommon = new List<CommonObject>();
                            List<T_Words> lastResult = listWord;// new List<T_Words>();
                            //先删除
                            //string deleteSql = "DELETE FROM dbo.T_Words WHERE Version='" + Request.Form["ddl_Version"] + "' AND Grade='" + Request.Form["ddl_Grade"] + "' AND Leavel='" + Request.Form["ddl_Leavel"] + "'";
                            //listCommon.Add(new CommonObject(deleteSql, null));
                            List<T_VersionVsWords> listVersion = new List<T_VersionVsWords>();
                            listVersion = CommonBase.GetList<T_VersionVsWords>("Version='" + version + "' and Grade='" + grade + "' and Leavel='" + leavel + "'");
                            foreach (T_Words model in lastResult)
                            {
                                if (model.IsUpdate)
                                {
                                    if (option == "1" || option == "3")
                                    {
                                        CommonBase.Update<T_Words>(model, listCommon);
                                    }
                                    if (option == "2" || option == "3")
                                    {
                                        if (option == "2")
                                            CommonBase.Update<T_Words>(model, new string[] { "Chinese", "Phonetic" }, listCommon);

                                        //更新教材版本对应表
                                        T_VersionVsWords versionModel = listVersion.FirstOrDefault(c => c.WordCode == model.Code);
                                        if (versionModel == null)
                                        {
                                            versionModel = new T_VersionVsWords();
                                            versionModel.Code = GetGuid;
                                            versionModel.CreatedTime = DateTime.Now;
                                            versionModel.Sort = 1;
                                            versionModel.Status = 1;
                                            versionModel.Grade = grade;
                                            versionModel.Leavel = leavel;
                                            versionModel.Version = version;
                                            versionModel.WordCode = model.Code;
                                            versionModel.IsDeleted = false;
                                            versionModel.Unit = model.Unit;
                                            versionModel.WIndex = model.WIndex;
                                            CommonBase.Insert<T_VersionVsWords>(versionModel, listCommon);
                                        }
                                        else
                                        {
                                            versionModel.Unit = model.Unit;
                                            versionModel.WIndex = model.WIndex;
                                            CommonBase.Update<T_VersionVsWords>(versionModel, listCommon);
                                        }
                                    }
                                }
                                else
                                {

                                    if (option == "1" || option == "3" || option == "2")
                                    {
                                        CommonBase.Insert<T_Words>(model, listCommon);
                                    }
                                    if (option == "2" || option == "3")
                                    {
                                        //新增之后，把新增的单词与教材版本对应起来
                                        T_VersionVsWords versionModel = new T_VersionVsWords();
                                        versionModel.Code = GetGuid;
                                        versionModel.CreatedTime = DateTime.Now;
                                        versionModel.Grade = grade;
                                        versionModel.IsDeleted = false;
                                        versionModel.Leavel = leavel;
                                        versionModel.Sort = 1;
                                        versionModel.Status = 1;
                                        versionModel.Unit = model.Unit;
                                        versionModel.Version = version;
                                        versionModel.WIndex = model.WIndex;
                                        versionModel.WordCode = model.Code;
                                        CommonBase.Insert<T_VersionVsWords>(versionModel, listCommon);
                                    }
                                }
                            }
                            if (CommonBase.RunListCommit(listCommon))
                                return CommonHelper.Response(true, "操作成功！");
                            else
                            {
                                return CommonHelper.Response(false, "操作失败，请重试！");
                            }
                            #endregion
                        }
                    }


                    if (option == "4" || option == "5")
                    {
                        string fileName = MethodHelper.CommonHelper.CreateNo();
                        string exportfiles = HttpContext.Current.Server.MapPath("/Attachment/download/" + fileName + ".xlsx");
                        using (NpoiService excelHelper = new NpoiService(exportfiles))
                        {
                            //再把这个DataTable导出
                            int resultCount = excelHelper.ExportExchangeWordExcel(dtResult, "Sheet1", true);
                            if (resultCount == -1)
                            {
                                return MethodHelper.CommonHelper.Response(false, "导出失败，请重试");
                            }
                            else
                            {
                                return MethodHelper.CommonHelper.Response(true, "/Attachment/download/" + fileName + ".xlsx");
                            }
                        }
                    }

                    return CommonHelper.Response(false, "操作失败，请重试！");

                    //Model.EN_Video model = CommonBase.GetModel<Model.EN_Video>(Request.Form["hidId"]);
                    //if (model != null)
                    //{
                    //    model = GetModel(model);
                    //    CommonBase.Update<Model.EN_Video>(model, listComm);
                    //    if (CommonBase.RunListCommit(listComm))
                    //        return CommonHelper.Response(true, "操作成功！");
                    //    else
                    //        return CommonHelper.Response(true, "操作失败，请重试！");
                    //}
                }
                return CommonHelper.Response(false, "操作失败，请重试！");
            }
            catch (Exception e)
            {
                return CommonHelper.Response(false, "操作失败，请联系管理员");
            }
            return CommonHelper.Response(false, "操作失败，请联系管理员");
        }

        protected DataTable AddDataRow(DataTable dtResult, string xuhao, string word, string chinese, bool isAddRemark)
        {
            if (!string.IsNullOrEmpty(word))
            {
                DataRow addRow = dtResult.NewRow();
                addRow["序号"] = xuhao;
                addRow["单词"] = word;
                addRow["音标"] = string.Empty;
                addRow["中文"] = string.Empty;
                if (isAddRemark)
                    addRow["Remark"] = NpoiService.GetNewString(chinese);
                dtResult.Rows.Add(addRow);
            }
            return dtResult;
        }


    }
}