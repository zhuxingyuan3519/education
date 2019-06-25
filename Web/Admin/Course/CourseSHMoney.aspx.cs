using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Course
{
    public partial class CourseSHMoney : BasePage
    {
        protected override void SetPowerZone()
        {
            BindRep();
        }
        protected void BindRep()
        {
            string types = Request.QueryString["chk"];
            string code = Request.QueryString["ccode"];
            hidCcode.Value = code;
            hisSHType.Value = types;
            List<TD_SHMoney_Dict> dictList = CommonBase.GetList<TD_SHMoney_Dict>("IsDeleted=0");
            List<TD_SHMoney_Dict> dictListCpoy = new List<TD_SHMoney_Dict>();
            if (!string.IsNullOrEmpty(types))
            {
                string[] spl = types.Split(';');
                foreach (string sp in spl)
                {
                    if (!string.IsNullOrEmpty(sp))
                    {
                        TD_SHMoney_Dict dict = dictList.FirstOrDefault(c => c.Code == sp);
                        if (dict != null)
                        {
                            dict.Remark = code;
                            dictListCpoy.Add(dict);

                        }
                    }
                }
            }

            repSHMoneyTypeList.DataSource = dictListCpoy;
            repSHMoneyTypeList.DataBind();

        }

        //绑定子控件
        protected void rptypelist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //判断里层repeater处于外层repeater的哪个位置（ AlternatingItemTemplate，FooterTemplate，
            //HeaderTemplate，，ItemTemplate，SeparatorTemplate）
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("rep_ShoneyList") as Repeater;//找到里层的repeater对象
                TD_SHMoney_Dict rowv = (TD_SHMoney_Dict)e.Item.DataItem;//找到分类Repeater关联的数据项 
                //先查各自定义的，没有配置的话再从模板读取
                List<TD_SHMoney> list = CommonBase.GetList<TD_SHMoney>("Code='" + rowv.Code + "' and Field4='" + rowv.Remark + "'");
                if (list.Count == 0)
                    list = CommonBase.GetList<TD_SHMoney>("Code='" + rowv.Code + "' and Field4=''");
                rep.DataSource = list;
                rep.DataBind();
            }
        }

        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            string[] allkeys = Request.Form.AllKeys;
            listComm.Add(new CommonObject("delete from TD_SHMoney where Field4='" + Request.Form["hidCcode"] + "'", null));
            string types = Request.Form["hisSHType"];
            if (!string.IsNullOrEmpty(types))
            {
                string[] spl = types.Split(';');
                foreach (string sp in spl)
                {
                    if (!string.IsNullOrEmpty(sp))
                    {
                      
                        GetSaveData(allkeys, listComm, sp);
                    }
                }
            }
            if (CommonBase.RunListCommit(listComm))
            {
                CacheHelper.RemoveAllCache("TD_SHMoney");
                return "操作成功";
            }
            else
                return "操作失败";
        }

        protected void GetSaveData(string[] allkeys, List<CommonObject> listComm, string startWith)
        {
            List<string> list = new List<string>();
            foreach (string str in allkeys)
            {
                if (str.StartsWith(startWith + "_"))
                {
                    string id = str.Split('_')[1];
                    if (!list.Contains(startWith + "_" + id))
                        list.Add(startWith + "_" + id);
                }
            }
            foreach (string str in list)
            {
                string remarkVal = Request.Form[str + "_Remark"];
                string floatVal = Request.Form[str + "_TJFloat"];
                string field3Val = Request.Form[str + "_Field3"];
                string tjIndexVal = Request.Form[str + "_TJIndex"];
                string tjRoleCodeVal = Request.Form[str + "_RoleCode"];
                TD_SHMoney shmoney = new TD_SHMoney();
                shmoney.Code = startWith;
                shmoney.Company = 1;
                shmoney.FHFloat = 0;
                if (!string.IsNullOrEmpty(field3Val))
                    shmoney.Field3 = "1";
                else
                    shmoney.Field3 = "2";
                shmoney.Field4 = Request.Form["hidCcode"];
                shmoney.IsDeleted = false;
                shmoney.Remark = remarkVal;
                shmoney.TJFloat = decimal.Parse(floatVal);
                shmoney.TJIndex = int.Parse(tjIndexVal);
                shmoney.RoleCode = tjRoleCodeVal;
                shmoney.Status = 1;
                CommonBase.Insert<TD_SHMoney>(shmoney, listComm);
            }


        }
    }
}