
using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Templete
{
    public partial class TempleteEdit : BasePage
    {
        protected override void SetPowerZone()
        {
            BindDDLFromDict("TempType", ddlType);
            BindDDLFromDict("IndutyType", ddlInduty);
        }
        protected override void SetValue(string id)
        {
            trTempUpload.Visible = false;
            T_Template model = CommonBase.GetModel<T_Template>(id);
            hidId.Value = id;
            txtName.Value = model.Name;
            txtPrice.Value = model.Price.ToString();
            ddlType.Value = model.Type;
            ddlInduty.Value = model.Induty;
            hidAtt.Value = model.AttURL;
            hidAttShow.Value = model.Field4;
            txtIndex.Value = model.Field1;
            hidCoverImg.Value = model.CoverImg;
            divImgContainer.InnerHtml = "<div><img src='../images/button-cross.png'  class='imgdel' onclick='deletePic(\"" + model.CoverImg + "\",this)'/><input type='hidden' name='uploadImg' value='" + model.CoverImg + "'/><img class='appendImg' src='" + model.CoverImg + "'/></div>";

            List<T_TempContainer> list = CommonBase.GetList<T_TempContainer>("IsDeleted=0 and TId='" + id + "'");
            rep_ConyainerList.DataSource = list;
            rep_ConyainerList.DataBind();
            List<PageToSalt> PSlist = new List<PageToSalt>();
            foreach (T_TempContainer tc in list)
            {
                if (PSlist.FirstOrDefault(c => c.page == tc.ContainerPage) == null)
                {
                    PageToSalt sa = new PageToSalt();
                    sa.page = tc.ContainerPage;
                    sa.salt = tc.Field1;
                    PSlist.Add(sa);
                }
            }
            seleSearch.DataSource = PSlist;
            seleSearch.DataTextField = "page";
            seleSearch.DataValueField = "salt";
            seleSearch.DataBind();
            seleSearch.Items.Insert(0, new ListItem("--请选择页面--", ""));
        }

        protected T_Template GetModel(T_Template model)
        {
            model.Name = Request.Form["txtName"];
            model.Price = decimal.Parse(Request.Form["txtPrice"]);
            model.Type = Request.Form["ddlType"];
            model.Induty = Request.Form["ddlInduty"];

            model.CoverImg = Request.Form["uploadImg"];
            model.Remark = Request.Form["txtRemark"];
            model.Field1 = Request.Form["txtIndex"];
            return model;
        }

        /// <summary>
        /// 查看模板配置信息
        /// </summary>
        /// <returns></returns>
        protected override string btnOther_Click()
        {
            if (!string.IsNullOrEmpty(Request["hidId"]))
            {
                T_Template model = CommonBase.GetModel<T_Template>(Request["hidId"]);
                if (model != null)
                {
                    return "1≌1≌" + GetTempContainer(Server.MapPath("/" + model.AttURL));
                }
            }
            else
            {
                return "0≌操作失败";
            }
            return "0≌操作失败";
        }

        /// <summary>
        /// 保存模板基本信息
        /// </summary>
        /// <returns></returns>
        protected override string btnAdd_Click()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["hidId"]))
                {
                    T_Template model = CommonBase.GetModel<T_Template>(Request["hidId"]);
                    if (model != null)
                    {
                        model = GetModel(model);
                        model.LastUpdateBy = TModel.MID;
                        model.LastUpdateTime = DateTime.Now;
                        if (CommonBase.Update<T_Template>(model))
                        {
                            return "1≌" + model.AttURL;
                            //return "1*1*" + GetTempContainer(Server.MapPath("/" + model.AttURL));
                        }
                    }
                }
                else
                {
                    T_Template model = new T_Template();
                    model.Id = GetGuid;
                    model = GetModel(model);
                    model.AttURL = Request.Form["hidAtt"];
                    model.Field4 = Request.Form["hidAttShow"];
                    model.CreatedBy = TModel.MID;
                    model.CreatedTime = DateTime.Now;
                    model.IsDeleted = false;
                    model.Status = 1;
                    if (uploadFile1(model))
                    {
                        if (uploadFile2(model))
                        {
                            //上传成功之后，再修改路径地址
                            model.AttURL = "temChange/" + model.Type + "/" + model.AttURL.Substring(model.AttURL.LastIndexOf('/') + 1, GetGuid.Length);
                            model.Field4 = "temShow/" + model.Type + "/" + model.Field4.Substring(model.Field4.LastIndexOf('/') + 1, GetGuid.Length);
                            if (CommonBase.Insert<T_Template>(model))
                            {
                                SetDefaultVal(model.AttURL);
                                //生成模板内容表
                                //return "1*1*" + GetTempContainer(Server.MapPath("/" + model.AttURL));
                                return "1≌" + model.Id;
                            }
                        }
                    }
                    return "0≌操作失败";
                }
            }
            catch (Exception e)
            {
                return "0≌修改失败：" + e.Message;
            }
            return "0≌操作失败";
        }

        protected string GetTempContainer(string attUrl)
        {
            List<T_TempContainer> list = new List<T_TempContainer>();
            string testUrl = attUrl;
            bool isExist = DirFileHelper.IsExistDirectory(testUrl);
            string pageList = string.Empty;
            List<PageToSalt> psList = new List<PageToSalt>();
            if (isExist)
            {
                string[] fileInfo = DirFileHelper.GetFileNames(testUrl);

                foreach (string tempHtmlUrl in fileInfo)
                {
                    //获取文件名
                    string salt = "t" + GetGuid.Substring(0, 5);
                    string fileName = tempHtmlUrl.Substring(tempHtmlUrl.LastIndexOf('\\') + 1);
                    pageList += salt + "|";

                    PageToSalt sa = new PageToSalt();
                    sa.page = fileName;
                    sa.salt = salt;
                    psList.Add(sa);

                    System.Text.Encoding code = FileHelper.GetType(tempHtmlUrl);  // System.Text.Encoding.UTF8;// FileHelper.GetFileEncodeType(tempHtmlUrl);
                    System.IO.StreamReader sr = null;
                    string str = null;
                    try
                    {
                        sr = new System.IO.StreamReader(tempHtmlUrl, code);
                        str = sr.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        sr.Close();
                    }
                    string[] arrayTest = str.Split('∽');
                    for (int i = 1; i <= arrayTest.Length - 1; i += 2)
                    {
                        if (!string.IsNullOrEmpty(arrayTest[i]))
                        {
                            T_TempContainer pa = new T_TempContainer();
                            pa.ContainerCode = "∽" + arrayTest[i] + "∽";
                            pa.ContainerPage = fileName;
                            pa.Field1 = salt;
                            list.Add(pa);
                        }
                    }
                }
            }
            return JsonHelper.ListToJson<T_TempContainer>(list, "TempContainer") + "≌" + JsonHelper.ListToJson<PageToSalt>(psList, "PageContainer");
        }


        #region 上传、解压，移动相关文件
        /// <summary>
        /// 修改过的模板文件上传解压
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected bool uploadFile1(T_Template model)
        {
            bool result = true;
            string afterMoveFile = MoveFile(model.AttURL, model.Type, "temChange");
            //上传的都是压缩文件，需要再解压
            string fileType = afterMoveFile.Substring(afterMoveFile.LastIndexOf('.') + 1).ToLower();
            string fileName = afterMoveFile.Substring(afterMoveFile.LastIndexOf('\\') + 1, GetGuid.Length);
            if (fileType == "zip")
                result = UnZipFile(afterMoveFile, model.Type, fileName, "temChange");
            else if (fileType == "rar")
                result = UnRarFile(afterMoveFile, model.Type, fileName, "temChange");
            return result;
        }
        /// <summary>
        /// 模板原始文件上传解压
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected bool uploadFile2(T_Template model)
        {
            bool result = true;
            string afterMoveFile = MoveFile(model.Field4, model.Type, "temShow");
            //上传的都是压缩文件，需要再解压
            string fileType = afterMoveFile.Substring(afterMoveFile.LastIndexOf('.') + 1).ToLower();
            string fileName = afterMoveFile.Substring(afterMoveFile.LastIndexOf('\\') + 1, GetGuid.Length);
            if (fileType == "zip")
                result = UnZipFile(afterMoveFile, model.Type, fileName, "temShow");
            else if (fileType == "rar")
                result = UnRarFile(afterMoveFile, model.Type, fileName, "temShow");
            return result;
        }
        private bool UnZipFile(string zipFilePath, string folder, string fileName, string temType)
        {
            return ZipHelper.UnZipFile(zipFilePath, Request.PhysicalApplicationPath + temType + "\\" + folder + "\\" + fileName);
        }
        private bool UnRarFile(string rarFilePath, string folder, string fileName, string temType)
        {
            bool result = true;
            string foldeRARr = Request.PhysicalApplicationPath + temType + "\\" + folder + "\\" + fileName;
            if (!Directory.Exists(foldeRARr))
                Directory.CreateDirectory(foldeRARr);
            string res = RarHelper.WinUnZip(rarFilePath, foldeRARr);
            if (res.Split('|')[0] == "1")
            {
                result = true;
            }
            else
            {
                //result = "压缩文件解压失败：失败原因：" + res.Split('|')[1];
                result = false;
            }
            return result;
        }
        protected string MoveFile(string attrUrl, string typeFile, string temType)
        {
            string OrignFile, NewFile;
            OrignFile = Server.MapPath(attrUrl);
            NewFile = Server.MapPath("\\" + temType + "\\" + typeFile + "\\" + attrUrl.Substring(attrUrl.LastIndexOf("/")));
            try
            {
                if (File.Exists(OrignFile))
                    File.Move(OrignFile, NewFile);
            }
            catch { }
            return NewFile;
        }
        #endregion
        /// <summary>
        /// 保存模板配置信息
        /// </summary>
        /// <returns></returns>
        protected override string btnModify_Click()
        {
            string tempId = Request.Form["hidId"];
            List<CommonObject> listSql = new List<CommonObject>();
            IList<T_TempContainer> list = GetDetailModelList();
            foreach (T_TempContainer objNew in list)
            {
                T_TempContainer sq = objNew;
                if (objNew.Id == 0)
                {
                    sq.ContainerPage = objNew.ContainerPage;
                    sq.ContainerName = objNew.ContainerName;
                    sq.ContainerCode = objNew.ContainerCode;

                    sq.CreatedBy = TModel.MID;
                    sq.CreatedTime = DateTime.Now;
                    sq.IsDeleted = false;
                    sq.Status = 1;
                    sq.TId = tempId;
                    CommonBase.Insert<T_TempContainer>(sq, listSql);
                }
                else
                {
                    sq = CommonBase.GetModel<T_TempContainer>(objNew.Id);
                    sq.ContainerCode = objNew.ContainerCode;
                    sq.ContainerName = objNew.ContainerName;
                    sq.ContainerPage = objNew.ContainerPage;
                    sq.LastUpdateBy = TModel.MID;
                    sq.LastUpdateTime = DateTime.Now;
                    CommonBase.Update<T_TempContainer>(sq, listSql);
                }
            }
            //删除需要删除的
            string deleteId = Request.Form["hidDelIds"];
            if (!string.IsNullOrEmpty(deleteId))
            {
                deleteId = deleteId.Substring(0, deleteId.Length);
                string delSql = "delete from T_TempContainer where Id in (" + deleteId + ")";
                listSql.Add(new CommonObject(delSql, null));
            }
            //更新或新增
            if (CommonBase.RunListCommit(listSql))
                return "1≌";
            else
                return "0≌";
        }

        #region 批量提取模板中替换字符
        public string[] EditTableKeysForTrain { get { return new string[] { "hidContainerCode", "hidSearchFileld", "ContainerName", "hidContainerPage", "hidContainerId" }; } }
        protected IList<T_TempContainer> GetDetailModelList()
        {
            string[] arrRequest = Request.Form.AllKeys;
            List<string> newDetailListTrain = new List<string>();
            List<T_TempContainer> listSafe = new List<T_TempContainer>();
            foreach (string str in arrRequest)
            {
                var key = str.Split('_')[0];
                if (EditTableNeedSaveKeys(key, EditTableKeysForTrain))
                {
                    newDetailListTrain.Add(str);
                }

            }
            List<string> strFlagsTrain = GetSetedGuid(newDetailListTrain);

            listSafe = AddToTotalModel(listSafe, AddListModel(strFlagsTrain, EditTableKeysForTrain));

            return listSafe;
        }
        protected List<T_TempContainer> AddToTotalModel(List<T_TempContainer> toAdd, List<T_TempContainer> origin)
        {
            foreach (T_TempContainer obj in origin)
            {
                T_TempContainer newObj = obj;
                toAdd.Add(newObj);
            }
            return toAdd;
        }
        protected List<T_TempContainer> AddListModel(List<string> strFlags, string[] EditTableKeys)
        {
            List<T_TempContainer> list = new List<T_TempContainer>();
            object id = null; string storehidContainerCode = "", storeContainerName = "", storehidContainerPage = "", storehidSearchFileld = "";
            foreach (string str in strFlags)
            {
                foreach (string sin in EditTableKeys)
                {
                    switch (sin)
                    {
                        case "hidContainerId":
                            id = Request.Form[sin + "_" + str];
                            break;
                        case "hidContainerCode":
                            storehidContainerCode = Request.Form[sin + "_" + str];
                            break;
                        case "ContainerName":
                            storeContainerName = Request.Form[sin + "_" + str];
                            break;
                        case "hidContainerPage":
                            storehidContainerPage = Request.Form[sin + "_" + str];
                            break;
                        case "hidSearchFileld":
                            storehidSearchFileld = Request.Form[sin + "_" + str];
                            break;
                    }
                }
                list.Add(NewEntity(id, storehidContainerCode, storeContainerName, storehidContainerPage, storehidSearchFileld));
            }
            return list;
        }
        private T_TempContainer NewEntity(object id, string argContainerCode, string argContainerName, string argContainerPage, string storehidSearchFileld)
        {
            T_TempContainer obj = null;
            if (!string.IsNullOrEmpty(argContainerCode))
            {
                obj = new T_TempContainer { Id = ToNullInt(id), ContainerCode = argContainerCode, ContainerName = argContainerName, ContainerPage = argContainerPage, Field1 = storehidSearchFileld };
            }
            return obj;
        }
        #endregion


        #region 自动读取模板中文本病替换成需要替换的文本
        /// <summary>
        /// 自动替换信息
        /// </summary>
        /// <param name="path">模板文件夹的路径</param>
        protected void SetDefaultVal(string path)
        {
            string tempRealUrl = Server.MapPath("\\"+path); ;
            string[] tempHtmlUrl = DirFileHelper.GetFileNames(tempRealUrl, "*.html", true);
            //update by zhxy at 2016年5月10日22:24:18 begin
            foreach (string files in tempHtmlUrl)
            {
                string fileName = files.Substring(files.LastIndexOf('\\') + 1);
                //System.Text.Encoding code = System.Text.Encoding.GetEncoding("utf-8");
                System.Text.Encoding code = FileHelper.GetFileEncodeType(files);
                StreamReader sr = null;
                StreamWriter sw = null;
                string tempContent = null, orginContent = string.Empty;
                try
                {
                    //读取模板内容
                    sr = new System.IO.StreamReader(files, code);
                    tempContent = sr.ReadToEnd();
                    orginContent = tempContent;
                    sr.Close();
                    List<T_TempContainer> modelList = new List<T_TempContainer>();
                    tempContent = NoHTML(tempContent);
                    //取出所有的标签中的文本
                    List<string> listString = BetweenArr(tempContent, ">", "<");
                    int i = 0;
                    foreach (string str in listString)
                    {
                        i++;
                        string replaceStr = "ForReplace" + i.ToString();
                        if (str.Length <= 2)
                            replaceStr = i.ToString();
                        orginContent = orginContent.Replace(">" + str + "<", ">∽" + replaceStr + "∽<");
                    }
                    code = System.Text.Encoding.GetEncoding("utf-8");
                    sw = new System.IO.StreamWriter(files, false, code);
                    sw.Write(orginContent);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    sr.Close();
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Htmlstring.Replace("\r\n", "");  //去除换行
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            //去除script标签
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //去除style标签
            Htmlstring = Regex.Replace(Htmlstring, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);
            //string pararn = @"<ul class='nav' id='nav'>]*?>.*?</ul>";
            //Htmlstring = Regex.Replace(Htmlstring, pararn, "", RegexOptions.IgnoreCase);
            ////删除HTML
            //Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            //Htmlstring.Replace("<", "");
            //Htmlstring.Replace(">", "");
            //Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        /// <summary>
        /// 取文本中间到List集合
        /// </summary>
        /// <param name="str">文本字符串</param>
        /// <param name="leftstr">左边文本</param>
        /// <param name="rightstr">右边文本</param>
        /// <returns>List集合</returns>
        public List<string> BetweenArr(string str, string leftstr, string rightstr)
        {
            List<string> list = new List<string>();
            int leftIndex = str.IndexOf(leftstr);//左文本起始位置
            int leftlength = leftstr.Length;//左文本长度
            int rightIndex = 0;
            string temp = "";
            while (leftIndex != -1)
            {
                rightIndex = str.IndexOf(rightstr, leftIndex + leftlength);
                if (rightIndex == -1)
                {
                    break;
                }
                temp = str.Substring(leftIndex + leftlength, rightIndex - leftIndex - leftlength);
                temp = temp.Replace("\b", "").Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("\r\n", "");
                if (!string.IsNullOrEmpty(temp.Trim()))
                    list.Add(temp);
                leftIndex = str.IndexOf(leftstr, rightIndex + 1);
            }
            return list;
        }

        #endregion

    }
}