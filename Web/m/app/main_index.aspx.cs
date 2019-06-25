using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class main_index : BasePage
    {
        protected int BannerListCount = 0;
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var bannerList = Service.CacheService.POSBankList.Where(c => c.Code == "2");
                BannerListCount = bannerList.Count();
                repBannerList.DataSource = bannerList;
                repBannerList.DataBind();

                if (Session["Member"] != null)
                {
                    TModel = Session["Member"] as Member;

                    //if (IsVip == 0)
                    //{
                    //    //不是VIP，再看不是今天注册的，只有一天的体验期，按照00:00-24:00为限，当天注册的会员，到夜里24点就关闭
                    //    DateTime mcreatedate = TModel.MCreateDate;
                    //    if (mcreatedate.Date < DateTime.Now.Date) 
                    //    {
                    //        divZuiXinKouZi.Visible = false;
                    //    }
                    //}
                    List<DB_EveryDayRemind> List = CommonBase.GetList<Model.DB_EveryDayRemind>("IsDeleted=0 and DATEDIFF(dd,LogDate,GETDATE())=0 and RType<>'6'   and MID=" + TModel.ID);

                    List<DB_EveryDayRemind> ListKouzi = CommonBase.GetList<Model.DB_EveryDayRemind>("IsDeleted=0 and IsRead=0 and RType='6' and MID=" + TModel.ID);
                    List.AddRange(ListKouzi); //添加到集合中
                    //看看是不是规划表修改了
                    string strWhere = "IsDeleted=0 and DATEDIFF(dd,PlanEndDate,GETDATE())<=0 and Remark='True' AND  Company=" + TModel.ID;
                    if (CommonBase.GetList<CM_PlanHeader>(strWhere).Count > 0)
                    {
                        DB_EveryDayRemind remind = new DB_EveryDayRemind();
                        remind.Remark = "系统已自动重新规划您未操作的还款/消费计划，为减小您的还款压力，请每日登陆系统执行。";
                        List.Add(remind);
                    }
                    if (List.Count <= 0)
                    {
                        remindListBox.Visible = false;
                    }
                    else
                    {
                        repRemindList.DataSource = List;
                        repRemindList.DataBind();
                    }
                }
                else
                {
                    remindListBox.Visible = false;
                    //divZuiXinKouZi.Visible = false;
                }
            }
        }
    }
}
