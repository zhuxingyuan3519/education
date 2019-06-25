using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

namespace Web.Admin.Member
{
    public partial class StructNet: System.Web.UI.Page
    {
        protected string iframeShow = "0", mamberData = string.Empty, lineData = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Request.QueryString["Action"]))
            {
                if(Request.QueryString["Action"].ToUpper() == "MODIFY")
                {
                    Response.Write(btnModify_Click());
                    Response.End();
                }
                if(Request.QueryString["Action"].ToUpper() == "ADD")
                {
                    Response.Write(btnAdd_Click());
                    Response.End();
                }
            }
        }
        protected string btnModify_Click()
        {
            string memberCode = Request.Form["userCode"];
            //获取到用户信息
            Model.Member member = DBUtility.CommonBase.GetModel<Model.Member>(memberCode);
            string title = string.Empty;
            if(member != null)
            {
                title += "<table>";
                title += "<tr>";
                title += "<td>账号唯一ID：</td><td>" + member.ID + "</td>";
                title += "</tr>";
                title += "<tr>";
                title += "<td>账号：</td><td>" + member.MID + "</td>";
                title += "</tr>";
                title += "<tr>";
                title += "<td>姓名：</td><td>" + member.MName + "</td>";
                title += "</tr>";
                title += "<tr>";
                title += "<td>分红状态：</td><td>" + ((member.IsFH == "1" || string.IsNullOrEmpty(member.IsFH)) ? "正常分红" : "停止分红") + "</td>";
                title += "</tr>";
                title += "<tr>";
                title += "<td>注册时间：</td><td>" + member.MCreateDate + "</td>";
                title += "</tr>";

                Model.Member memberMTJ = DBUtility.CommonBase.GetModel<Model.Member>(member.MTJ);
                if(memberMTJ != null)
                {
                    title += "<tr>";
                    title += "<td>推荐人：</td><td>" + memberMTJ.MID + "</td>";
                    title += "</tr>";
                    title += "<tr>";
                    title += "<td>推荐人姓名：</td><td>" + memberMTJ.MName + "</td>";
                    title += "</tr>";
                }
                title += "<tr>";
                title += "<td>当前级别：</td><td>" + member.Role.Name + "</td>";
                title += "</tr>";
                string swql = "SELECT t1.SignDate,t1.Fee,t2.Name,t2.Code FROM dbo.EN_SignUp t1 LEFT JOIN dbo.EN_Course t2 ON t1.CourseCode=t2.Code where  t1.MCode='" + member.ID + "'";
                DataTable dt = DBUtility.CommonBase.GetTable(swql);
                if(dt != null && dt.Rows.Count > 0)
                {
                    string remark = string.Empty;
                    foreach(DataRow dr in dt.Rows)
                    {
                        remark += MethodHelper.ConvertHelper.ToDateTime(dr["SignDate"].ToString(), DateTime.Now).ToString("yyyy-MM-dd HH:mm") + "缴费" + dr["Fee"].ToString() + "元报名" + dr["Name"].ToString();
                        remark += "<br/>";
                    }
                    title += "<tr>";
                    title += "<td>缴费课程：</td><td>" + remark + "</td>";
                    title += "</tr>";
                }
                title += "</table>";
            }
            return title;
        }
        protected string setDrowVal()
        {
            StringBuilder sbLine = new StringBuilder();
            StringBuilder sbData = new StringBuilder();
            //List<M_Rank> listRank0 = DBUtility.CommonBase.GetList<M_Rank>("PMCode='0'");
            string sql = "SELECT DISTINCT m.MCode,t2.MID,t2.MName FROM  (SELECT DISTINCT MCode FROM  M_Rank  UNION ALL SELECT DISTINCT PMCode FROM  M_Rank ) m LEFT JOIN dbo.Member t2 ON m.MCode=t2.ID WHERE m.MCode<>'0'";
            DataTable dtMember = DBUtility.CommonBase.GetTable(sql);
            foreach(DataRow dr in dtMember.Rows)
            {
                sbData.Append("{id:'" + dr["MCode"].ToString() + "'");
                sbData.Append(",label:'" + dr["MName"].ToString() + "'}");
                sbData.Append(",");
            }
            mamberData = sbData.ToString();
            if(sbData.Length > 0)
            {
                mamberData = mamberData.TrimEnd(',');
            }
            List<M_Rank> list = DBUtility.CommonBase.GetList<M_Rank>("").OrderBy(c => c.RankTime).ToList();
            List<M_Rank> listRank0 = list.Where(c => c.PMCode == "0").ToList();
            foreach(M_Rank rank in listRank0)
            {
                List<SelectTreeNode> listSelect = GetOrganizesTree(list, rank.MCode);
                foreach(SelectTreeNode match in listSelect)
                {
                    sbLine.Append("{from:'" + match.parentId + "'");
                    sbLine.Append(",to:'" + match.id + "'}");
                    sbLine.Append(",");
                }
            }
            lineData = sbLine.ToString();
            if(lineData.Length > 0)
            {
                lineData = lineData.TrimEnd(',');
            }
            return mamberData + "*" + lineData;
        }

        protected string btnAdd_Click()
        {
            return setDrowVal();
        }

        public class SelectTreeNode
        {
            public string id { get; set; }
            public string name { get; set; }
            public string parentId { get; set; }
            public string mbd { get; set; }
        }


        public static void GetOrganizeSelectTreeNodes(List<M_Rank> list, string id, ref List<SelectTreeNode> treeNodes)
        {
            if(list == null)
                return;
            List<M_Rank> sublist;
            if(!string.IsNullOrEmpty(id))
            {
                sublist = list.Where(t => t.PMCode == id).OrderBy(t => t.MBD).ToList();
                ////名下点数不够3个的,取到第一个
                //if(sublist.Count < 3)
                //{
                //    return;
                //}
            }
            else
            {
                sublist = list.Where(t => string.IsNullOrEmpty(t.PMCode)).ToList();
            }
            if(!sublist.Any())
                return;
            foreach(var item in sublist)
            {
                treeNodes.Add(new SelectTreeNode() { id = item.MCode, name = item.Code, parentId = item.PMCode });
                GetOrganizeSelectTreeNodes(list, item.MCode, ref treeNodes);
            }
        }

        public static List<SelectTreeNode> GetOrganizesTree(List<M_Rank> list, string id = null, string name = null)
        {
            List<SelectTreeNode> treeNodes = new List<SelectTreeNode>();
            GetOrganizeSelectTreeNodes(list, id, ref treeNodes);
            return treeNodes;
        }

    }
}