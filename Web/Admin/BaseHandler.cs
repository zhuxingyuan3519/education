using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

using System.Text;
using Model;
using System.Data;

namespace Web.Admin
{
    public class BaseHandler : BasePage, IHttpHandler, IRequiresSessionState
    {
        protected int pageIndex = 0;
        protected int pageSize = 0;
        protected int count;
        protected Model.Member SessionModel
        {
            get
            {
                return Session["AdminMember"] as Model.Member;
            }
        }
        protected Model.Member TModel
        {
            get
            {
                return SessionModel;
            }
        }
        public new virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (!string.IsNullOrEmpty(context.Request["pageIndex"]))
            {
                pageIndex = int.Parse(context.Request["pageIndex"]);
            }
            if (!string.IsNullOrEmpty(context.Request["pageSize"]))
            {
                pageSize = int.Parse(context.Request["pageSize"]);
            }
        }
        public string Traditionalized(StringBuilder sb)
        {
            return sb.ToString();
        }

        public void ExportExcel(HttpContext context, string fileName, DataTable dtSourse, Dictionary<string, string> exportColumns)
        {

            ////遍历字典，去除掉datatabele中不需要的列
            foreach (KeyValuePair<string, string> stu in exportColumns)
            {
                if (dtSourse.Columns.Contains(stu.Key))
                {
                    dtSourse.Columns[stu.Key].Namespace = "export";
                    dtSourse.Columns[stu.Key].ColumnName = stu.Value;
                }
            }
            List<DataColumn> listCol = new List<DataColumn>();
            foreach (DataColumn col in dtSourse.Columns)
            {
                if (col.Namespace != "export")
                {
                    //需要删除的列
                    listCol.Add(col);
                }
            }
            foreach (DataColumn delCol in listCol)
            {
                dtSourse.Columns.Remove(delCol.ColumnName);
            }
            //重新进行列排序
            int index = 0;
            foreach (KeyValuePair<string, string> stu in exportColumns)
            {
                dtSourse.Columns[stu.Value].SetOrdinal(index);
                index++;
            }

            //把转换好的数据集合导出Excel
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName) + ".xls");

            // 设置编码和附件格式 
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Charset = "GB2312 ";

            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            stringWriter.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            System.Web.UI.HtmlTextWriter htmlTextWriter = new System.Web.UI.HtmlTextWriter(stringWriter);

            System.Web.UI.WebControls.DataGrid exportGrid = new System.Web.UI.WebControls.DataGrid();
            exportGrid.DataSource = dtSourse.DefaultView;
            exportGrid.AllowPaging = false;
            exportGrid.DataBind();
            exportGrid.RenderControl(htmlTextWriter);
            context.Response.Output.Write(stringWriter.ToString());

            context.Response.Flush();
            context.Response.End();
        }
    }
}