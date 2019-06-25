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
    public partial class credit_market : BasePage
    {
        protected override void SetPowerZone()
        {
            BindRep();
        }
        protected void BindRep()
        {
            rep_List.DataSource = CacheService.CreditMarketList;
            rep_List.DataBind();
        }

        protected override string btnAdd_Click()
        {
            IList<Sys_CreditMarket> list = GetDetailModelList();
            List<CommonObject> comm = new List<CommonObject>();
            foreach (Sys_CreditMarket objNew in list)
            {
                Sys_CreditMarket sq = objNew;
                if (objNew.Id == 0)
                {
                    //新增的
                    sq.CreatedBy = TModel.MID;
                    sq.CreatedTime = DateTime.Now;
                    sq.IsDeleted = false;
                    sq.Status = true;
                    CommonBase.Insert<Sys_CreditMarket>(sq, comm);
                }
                else
                {
                    //修改的
                    sq = CommonBase.GetModel<Sys_CreditMarket>(objNew.Id);
                    string oldCode = sq.Code, newCode = objNew.Code;
                    sq.Code = objNew.Code;
                    //sq.LinkUrl = objNew.LinkUrl;
                    sq.Name = objNew.Name;
                    sq.PicUrl = objNew.PicUrl;
                    sq.Remark = objNew.Remark;
                    sq.LinkUrl = objNew.LinkUrl;
                    CommonBase.Update<Sys_CreditMarket>(sq, comm);
                }
            }
            //删除要删除的
            string deleteIds = Request.Form["hidDelIds"];
            if (!string.IsNullOrEmpty(deleteIds))
            {
                deleteIds = deleteIds.Substring(0, deleteIds.LastIndexOf(','));
                string delteSql = "delete from Sys_CreditMarket where Id in (" + deleteIds + ")";
                comm.Add(new CommonObject(delteSql, null));
            }
            if (CommonBase.RunListCommit(comm))
            {
                //更新魂村
                CacheHelper.RemoveAllCache("Sys_CreditMarket");
                return "操作成功";
            }
            else
                return "操作失败";

        }



        public string[] EditTableKeysForTrain { get { return new string[] { "Code", "Name", "Pic", "hidId", "link", "remark" }; } }

        protected IList<Sys_CreditMarket> GetDetailModelList()
        {
            string[] arrRequest = Request.Form.AllKeys;
            IList<string> newDetailListTrain = new List<string>();
            IList<Sys_CreditMarket> listSafe = new List<Sys_CreditMarket>();
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
        protected IList<Sys_CreditMarket> AddToTotalModel(IList<Sys_CreditMarket> toAdd, IList<Sys_CreditMarket> origin)
        {
            foreach (Sys_CreditMarket obj in origin)
            {
                Sys_CreditMarket newObj = obj;
                toAdd.Add(newObj);
            }
            return toAdd;
        }


        protected IList<Sys_CreditMarket> AddListModel(IList<string> strFlags, string[] EditTableKeys)
        {
            IList<Sys_CreditMarket> list = new List<Sys_CreditMarket>();
            object id = null; string code = "", name = "", picUrl = "", link = "", remark = "";
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
                    }
                }
                list.Add(NewEntity(id, code, name, picUrl, link, remark));
            }
            return list;
        }
        private Sys_CreditMarket NewEntity(object id, string code, string name, string picUrl, string link, string remark)
        {
            Sys_CreditMarket obj = null;
            if (!string.IsNullOrEmpty(name))
            {
                obj = new Sys_CreditMarket { Id = ToNullInt(id), Name = name, Code = code, PicUrl = picUrl, LinkUrl = link, Remark = remark };
            }
            return obj;
        }
    }
}