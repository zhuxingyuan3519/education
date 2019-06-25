using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Finance
{
    public partial class ExportExcel : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strWhere = "'1'='1' and IsDeleted=0 ";

                if (!string.IsNullOrEmpty(Request["nPayBeginTime"]))
                {
                    DateTime time = DateTime.Now;
                    DateTime.TryParse(HttpUtility.UrlDecode(Request["nPayBeginTime"]) + " 00:00:00", out time);
                    strWhere += " and FHDate>='" + time + "'";
                }
                if (!string.IsNullOrEmpty(Request["nPayEndTime"]))
                {
                    DateTime time = DateTime.Now;
                    DateTime.TryParse(HttpUtility.UrlDecode(Request["nPayEndTime"]) + " 23:59:59", out time);
                    strWhere += " and FHDate<='" + time + "'";
                }
                if (!string.IsNullOrEmpty(Request["type"])) //是否是查询奖金池分红
                {
                    strWhere += " and  charindex('P-',FHType)=1";
                    if (TModel.RoleCode != "Manage" && TModel.RoleCode != "Admin")
                        strWhere += " and MID=" + TModel.ID;
                }
                if (!string.IsNullOrEmpty(Request["nMID"])) //会员昵称
                {
                    strWhere += " and FHMCode like '%" + Request["nMID"] + "%'";
                }

                if (TModel.RoleCode == "Admin")
                {
                    strWhere += " and (MID in (select ID FROM Member where Company=" + TModel.ID + ") or MID='" + TModel.ID + "')";
                }
                if (!string.IsNullOrEmpty(TModel.Role.AreaLeave) && int.Parse(TModel.Role.AreaLeave) >= 20)
                {
                    strWhere += " and (MID in (select ID FROM Member where Agent=" + TModel.ID + ") or MID='" + TModel.ID + "')";
                }
                int count;
                DataTable dt = CommonBase.GetPageDataTable(1, int.MaxValue, strWhere, "*", "FHDate DESC", "TD_FHLog", out count);
                StreamExport(dt, null, "a11.xls", this.Page);
            }
        }

        /// <summary>
        /// DataTable通过流导出Excel
        /// </summary>
        /// <param name="ds">数据源DataSet或者DataTable</param>
        /// <param name="columns">DataTable中列对应的列名(可以是中文),若为null则取DataTable中的字段名</param>
        /// <param name="fileName">保存文件名(例如：a.xls)</param>
        /// <returns></returns>
        public bool StreamExport(DataTable dt, System.Collections.ArrayList columns, string fileName, System.Web.UI.Page pages)
        {
            if (dt.Rows.Count > 65535) //总行数大于Excel的行数
            {
                throw new Exception("预导出的数据总行数大于excel的行数");
            }
            if (string.IsNullOrEmpty(fileName)) return false;
            StringBuilder content = new StringBuilder();
            StringBuilder strtitle = new StringBuilder();
            content.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
            content.Append("<head><title></title><meta http-equiv='Content-Type' content=\"text/html; charset=gb2312\">");
            //注意：[if gte mso 9]到[endif]之间的代码，用于显示Excel的网格线，若不想显示Excel的网格线，可以去掉此代码
            content.Append("<!--[if gte mso 9]>");
            content.Append("<xml>");
            content.Append(" <x:ExcelWorkbook>");
            content.Append("  <x:ExcelWorksheets>");
            content.Append("   <x:ExcelWorksheet>");
            content.Append("    <x:Name>Sheet1</x:Name>");
            content.Append("    <x:WorksheetOptions>");
            content.Append("      <x:Print>");
            content.Append("       <x:ValidPrinterInfo />");
            content.Append("      </x:Print>");
            content.Append("    </x:WorksheetOptions>");
            content.Append("   </x:ExcelWorksheet>");
            content.Append("  </x:ExcelWorksheets>");
            content.Append("</x:ExcelWorkbook>");
            content.Append("</xml>");
            content.Append("<![endif]-->");
            content.Append("</head><body><table style='border-collapse:collapse;table-layout:fixed;'><tr>");
            if (columns != null)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i] != null && columns[i] != "")
                    {
                        content.Append("<td><b>" + columns[i] + "</b></td>");
                    }
                    else
                    {
                        content.Append("<td><b>" + dt.Columns[i].ColumnName + "</b></td>");
                    }
                }
            }
            else
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    content.Append("<td><b>" + dt.Columns[j].ColumnName + "</b></td>");
                }
            }
            content.Append("</tr>\n");
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                content.Append("<tr>");
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    object obj = dt.Rows[j][k];
                    Type type = obj.GetType();
                    if (type.Name == "Int32" || type.Name == "Single" || type.Name == "Double" || type.Name == "Decimal")
                    {
                        double d = obj == DBNull.Value ? 0.0d : Convert.ToDouble(obj);
                        if (type.Name == "Int32" || (d - Math.Truncate(d) == 0))
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0'>{0}</td>", obj);
                        else
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0.00'>{0}</td>", obj);
                    }
                    else
                        content.AppendFormat("<td style='vnd.ms-excel.numberformat:@'>{0}</td>", obj);
                }
                content.Append("</tr>\n");
            }
            content.Append("</table></body></html>");
            content.Replace("&nbsp;", "");
            pages.Response.Clear();
            pages.Response.Buffer = true;
            pages.Response.ContentType = "application/ms-excel";  //"application/ms-excel";
            pages.Response.Charset = "UTF-8";
            pages.Response.ContentEncoding = System.Text.Encoding.UTF7;
            fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            pages.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            pages.Response.Write(content.ToString());
            //pages.Response.End();  //注意，若使用此代码结束响应可能会出现“由于代码已经过优化或者本机框架位于调用堆栈之上,无法计算表达式的值。”的异常。
            HttpContext.Current.ApplicationInstance.CompleteRequest(); //用此行代码代替上一行代码，则不会出现上面所说的异常。
            return true;
        }
    }
}