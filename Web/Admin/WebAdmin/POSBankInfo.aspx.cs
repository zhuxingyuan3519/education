using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using Service;
using DBUtility;
using MethodHelper;

namespace Web.Admin.WebAdmin
{
    public partial class POSBankInfo : BasePage
    {
        protected override void SetPowerZone()
        {
            string code = Request.QueryString["code"];
            if(!string.IsNullOrEmpty(code))
                BindRep(code);
        }
        protected void BindRep(string code)
        {
            hidCode.Value = code;
            rep_List.DataSource = CacheService.POSBankList.Where(c=>c.Code==code);
            rep_List.DataBind();
        }

        protected override string btnAdd_Click()
        {
            IList<CM_POSBank> list = GetDetailModelList();
            List<CommonObject> comm = new List<CommonObject>();
            foreach (CM_POSBank objNew in list)
            {
                CM_POSBank sq = objNew;
                if (objNew.Id == 0)
                {
                    //新增的
                    sq.Code = Request.Form["hidCode"];
                    sq.CreatedBy = TModel.MID;
                    sq.CreatedTime = DateTime.Now;
                    sq.IsDeleted = false;
                    sq.Status = true;
                    CommonBase.Insert<CM_POSBank>(sq, comm);
                }
                else
                {
                    //修改的
                    sq = CommonBase.GetModel<CM_POSBank>(objNew.Id);
                    sq.LinkUrl = objNew.LinkUrl;
                    sq.Name = objNew.Name;
                    sq.PicUrl = objNew.PicUrl;
                    sq.Remark = objNew.Remark;
                    sq.UpdatedBy = TModel.MID;
                    sq.UpdatedTime = DateTime.Now;
                    CommonBase.Update<CM_POSBank>(sq, comm);
                }
            }
            //删除 
            string deleteIsd = Request.Form["hidDelIds"];
            if (!string.IsNullOrEmpty(deleteIsd))
            {
                deleteIsd = deleteIsd.Substring(0, deleteIsd.LastIndexOf(','));
                string sql = "delete from CM_POSBank where Id in (" + deleteIsd + ")";
                comm.Add(new CommonObject(sql, null));
            }
            if(CommonBase.RunListCommit(comm))
            {
                //更新魂村
                CacheHelper.RemoveAllCache("CM_POSBank");
                return "操作成功";
            }
            else
                return "操作失败";

        }



        public string[] EditTableKeysForTrain { get { return new string[] { "Code", "Name", "Pic", "hidId","link","remark" }; } }

        protected IList<CM_POSBank> GetDetailModelList()
        {
            string[] arrRequest = Request.Form.AllKeys;
            IList<string> newDetailListTrain = new List<string>();
            IList<CM_POSBank> listSafe = new List<CM_POSBank>();
            foreach (string str in arrRequest)
            {
                var key = str.Split('_')[0];
                if (EditTableNeedSaveKeys(key, EditTableKeysForTrain))
                {
                    newDetailListTrain.Add(str);
                }

            }
            IList<string> strFlagsTrain = GetSetedGuid(newDetailListTrain);

            listSafe = AddToTotalModel(listSafe, AddListModel(strFlagsTrain, EditTableKeysForTrain));

            return listSafe;
        }
        protected IList<CM_POSBank> AddToTotalModel(IList<CM_POSBank> toAdd, IList<CM_POSBank> origin)
        {
            foreach (CM_POSBank obj in origin)
            {
                CM_POSBank newObj = obj;
                toAdd.Add(newObj);
            }
            return toAdd;
        }


        protected IList<CM_POSBank> AddListModel(IList<string> strFlags, string[] EditTableKeys)
        {
            IList<CM_POSBank> list = new List<CM_POSBank>();
            object id = null; string code = "", name = "", picUrl = "",link="",remark="";
            foreach (string str in strFlags)
            {
                foreach (string sin in EditTableKeys)
                {
                    switch (sin)
                    {
                        case "hidId": id = Request.Form[sin + "_" + str]; break;
                        case "Code":
                            code = Request.Form[sin + "_" + str];
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                code = Request.Form[sin + "_" + str];
                            break;
                        case "Name":
                            name = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                name = Request.Form[sin + "_" + str];
                            break;
                        case "Pic":
                            picUrl = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                picUrl = Request.Form[sin + "_" + str];
                            break;
                        case "link":
                            link = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                link = Request.Form[sin + "_" + str];
                            break;
                        case "remark":
                            remark = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                remark = Request.Form[sin + "_" + str];
                            break;
                    }
                }
                list.Add(NewEntity(id, code, name, picUrl, link,remark));
            }
            return list;
        }
        private CM_POSBank NewEntity(object id, string code, string name, string picUrl, string link, string remark)
        {
            CM_POSBank obj = null;
            if (!string.IsNullOrEmpty(name))
            {
                obj = new CM_POSBank {  Id= ToNullInt(id), Name=name,  Code=code, PicUrl=picUrl , LinkUrl=link,Remark=remark};
            }
            return obj;
        }
    }
}