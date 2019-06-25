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
    public partial class BankInfo : BasePage
    {
        protected override void SetPowerZone()
        {
            BindRep();
        }
        protected void BindRep()
        {
            //Sys_BankInfoBLL bll = new Sys_BankInfoBLL();
            rep_List.DataSource = CacheService.BankList;
            rep_List.DataBind();
        }

        protected override string btnAdd_Click()
        {
            IList<Sys_BankInfo> list = GetDetailModelList();
            List<CommonObject> comm = new List<CommonObject>();
            foreach (Sys_BankInfo objNew in list)
            {
                Sys_BankInfo sq = objNew;
                if (objNew.Id == 0)
                {
                    //新增的
                    sq.LinkUrl ="";
                    sq.CreatedBy = TModel.MID;
                    sq.CreatedTime = DateTime.Now;
                    sq.IsDeleted = false;
                    sq.Status = true;
                    CommonBase.Insert<Sys_BankInfo>(sq, comm);
                }
                else
                {
                    //修改的
                    sq = CommonBase.GetModel<Sys_BankInfo>(objNew.Id);
                    string oldCode=sq.Code,newCode=objNew.Code;
                    sq.Code = objNew.Code;
                    //sq.LinkUrl = objNew.LinkUrl;
                    sq.Name = objNew.Name;
                    sq.PicUrl = objNew.PicUrl;
                    sq.BankMail = objNew.BankMail;
                    sq.BankTel = objNew.BankTel;
                    sq.Remark = objNew.Remark;
                    sq.UpdatedBy = TModel.MID;
                    sq.UpdatedTime = DateTime.Now;
                    CommonBase.Update<Sys_BankInfo>(sq, comm);
                    if (oldCode != newCode)
                    {
                        //再修改系统中所有的银行信息
                        string update1 = "UPDATE CM_Template SET Bank='" + newCode + "' WHERE Bank='" + oldCode + "'";
                        string update2 = "UPDATE CM_Archives SET Bank='" + newCode + "' WHERE Bank='" + oldCode + "'";
                        string update3 = "UPDATE CM_PlanHeader SET Bank='" + newCode + "' WHERE Bank='" + oldCode + "'";
                        string update4 = "UPDATE CM_PlanDetail SET Bank='" + newCode + "' WHERE Bank='" + oldCode + "'";
                        comm.Add(new CommonObject(update1, null));
                        comm.Add(new CommonObject(update2, null));
                        comm.Add(new CommonObject(update3, null));
                        comm.Add(new CommonObject(update4, null));
                    }
                }
            }
            if(CommonBase.RunListCommit(comm))
            {
                //更新魂村
                CacheHelper.RemoveAllCache("Sys_BankInfo");
                return "操作成功";
            }
            else
                return "操作失败";

        }



        public string[] EditTableKeysForTrain { get { return new string[] { "Code", "Name", "Pic", "hidId", "link", "remark", "banktel", "bankmail" }; } }

        protected IList<Sys_BankInfo> GetDetailModelList()
        {
            string[] arrRequest = Request.Form.AllKeys;
            IList<string> newDetailListTrain = new List<string>();
            IList<Sys_BankInfo> listSafe = new List<Sys_BankInfo>();
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
        protected IList<Sys_BankInfo> AddToTotalModel(IList<Sys_BankInfo> toAdd, IList<Sys_BankInfo> origin)
        {
            foreach (Sys_BankInfo obj in origin)
            {
                Sys_BankInfo newObj = obj;
                toAdd.Add(newObj);
            }
            return toAdd;
        }


        protected IList<Sys_BankInfo> AddListModel(IList<string> strFlags, string[] EditTableKeys)
        {
            IList<Sys_BankInfo> list = new List<Sys_BankInfo>();
            object id = null; string code = "", name = "", picUrl = "", link = "", remark = "", banktel = "", bankmail = "";
            foreach (string str in strFlags)
            {
                foreach (string sin in EditTableKeys)
                {
                    switch (sin)
                    {
                        case "hidId": id = Request.Form[sin + "_" + str]; break;
                        case "Code":
                            code = string.Empty;
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
                        case "banktel":
                            banktel = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                banktel = Request.Form[sin + "_" + str];
                            break;
                        case "bankmail":
                            bankmail = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                bankmail = Request.Form[sin + "_" + str];
                            break;
                    }
                }
                list.Add(NewEntity(id, code, name, picUrl, link,remark,banktel,bankmail));
            }
            return list;
        }
        private Sys_BankInfo NewEntity(object id, string code, string name, string picUrl, string link, string remark, string banktel, string bankmail)
        {
            Sys_BankInfo obj = null;
            if (!string.IsNullOrEmpty(name))
            {
                obj = new Sys_BankInfo {  Id= ToNullInt(id), Name=name,  Code=code, PicUrl=picUrl , LinkUrl=link,Remark=remark,BankTel=banktel,BankMail=bankmail};
            }
            return obj;
        }
    }
}