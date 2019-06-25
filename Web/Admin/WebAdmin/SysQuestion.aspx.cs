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
    public partial class SysQuestion : BasePage
    {
        protected override void SetPowerZone()
        {
            BindRep();
        }
        protected void BindRep()
        {
            //Sys_BankInfoBLL bll = new Sys_BankInfoBLL();
            rep_List.DataSource = CommonBase.GetList<Sys_SecurityQuestion>("IsDeleted=0");
            rep_List.DataBind();
        }

        protected override string btnAdd_Click()
        {
            IList<Sys_SecurityQuestion> list = GetDetailModelList();
            List<CommonObject> comm = new List<CommonObject>();
            foreach (Sys_SecurityQuestion objNew in list)
            {
                Sys_SecurityQuestion sq = objNew;
                if (objNew.ID == 0)
                {
                    //新增的
                    sq.CreatedBy = TModel.MID;
                    sq.CreatedTime = DateTime.Now;
                    sq.IsDeleted = false;
                    sq.Status = 1;
                    sq.Code = GetGuid;
                    CommonBase.Insert<Sys_SecurityQuestion>(sq, comm);
                }
                else
                {
                    //修改的
                    sq = CommonBase.GetModel<Sys_SecurityQuestion>(objNew.ID);
                    string oldCode = sq.Code, newCode = objNew.Code;
                    sq.Question = objNew.Question;
                    CommonBase.Update<Sys_SecurityQuestion>(sq, comm);
                }
            }
            string hidDelIds = Request.Form["hidDelIds"];
            if (!string.IsNullOrEmpty(hidDelIds))
            {
                foreach (string hidId in hidDelIds.Split(','))
                {
                    if (!string.IsNullOrEmpty(hidId))
                    {
                        string delteSql = "update Sys_SecurityQuestion set IsDeleted=1 where ID=" + hidId;
                        comm.Add(new CommonObject(delteSql, null));
                    }
                }
            }
            if (CommonBase.RunListCommit(comm))
            {
                return "操作成功";
            }
            else
                return "操作失败";

        }



        public string[] EditTableKeysForTrain { get { return new string[] { "Name", "hidId" }; } }

        protected IList<Sys_SecurityQuestion> GetDetailModelList()
        {
            string[] arrRequest = Request.Form.AllKeys;
            IList<string> newDetailListTrain = new List<string>();
            IList<Sys_SecurityQuestion> listSafe = new List<Sys_SecurityQuestion>();
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
        protected IList<Sys_SecurityQuestion> AddToTotalModel(IList<Sys_SecurityQuestion> toAdd, IList<Sys_SecurityQuestion> origin)
        {
            foreach (Sys_SecurityQuestion obj in origin)
            {
                Sys_SecurityQuestion newObj = obj;
                toAdd.Add(newObj);
            }
            return toAdd;
        }


        protected IList<Sys_SecurityQuestion> AddListModel(IList<string> strFlags, string[] EditTableKeys)
        {
            IList<Sys_SecurityQuestion> list = new List<Sys_SecurityQuestion>();
            object id = null; string name = "";
            foreach (string str in strFlags)
            {
                foreach (string sin in EditTableKeys)
                {
                    switch (sin)
                    {
                        case "hidId": id = Request.Form[sin + "_" + str]; break;
                        case "Name":
                            name = string.Empty;
                            if (Request.Form[sin + "_" + str] != null && !string.IsNullOrEmpty(Request.Form[sin + "_" + str]))
                                name = Request.Form[sin + "_" + str];
                            break;
                    }
                }
                list.Add(NewEntity(id, name));
            }
            return list;
        }
        private Sys_SecurityQuestion NewEntity(object id, string name)
        {
            Sys_SecurityQuestion obj = null;
            if (!string.IsNullOrEmpty(name))
            {
                obj = new Sys_SecurityQuestion { ID = ToNullInt(id), Question = name };
            }
            return obj;
        }
    }
}