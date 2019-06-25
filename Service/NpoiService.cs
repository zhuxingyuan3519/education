using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;
using NPOI.HSSF.Util;
using System.Text.RegularExpressions;

namespace Service
{
    public class NpoiService: IDisposable
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        public NpoiService(string fileName)
        {
            this.fileName = fileName;
            disposed = false;
        }

        /// <summary>
        /// 导出词库
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int ExportWordStorgeExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if(fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if(fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if(workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if(isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    //ICell icell1top;
                    for(j = 0; j < data.Columns.Count; ++j)
                    {
                        if(data.Columns[j].ColumnName == "Remark")
                            continue;
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                        //icell1top = row.CreateCell(j);
                        //icell1top.CellStyle = Getcellstyle(workbook, stylexls.头);
                        //icell1top.SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }
                ////设置列宽
                //if(isExport)
                //{
                //    sheet.SetColumnWidth(1, 70 * 256);
                //    sheet.SetColumnWidth(2, 20 * 256);
                //    sheet.SetColumnWidth(3, 40 * 256);
                //}
                //else
                //{
                //    //for(i = 0; i < data.Columns.Count; ++i)
                //    //{
                //    //    sheet.AutoSizeColumn(i);
                //    //}
                //}

                //HSSFRichTextString richtext = new HSSFRichTextString("Microsoft OfficeTM");
                //IFont fontBlue = workbook.CreateFont();
                ////fontBlue.FontHeightInPoints = 12;
                //fontBlue.Color = HSSFColor.Blue.Index;
                //fontBlue.IsBold = true;
                //richtext.ApplyFont(0, 6, fontBlue);


                for(i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    //ICell icell;
                    for(j = 0; j < data.Columns.Count; ++j)
                    {
                        if(data.Columns[j].ColumnName == "Remark")
                            continue;
                        if(data.Columns[j].ColumnName == "单词")
                        {
                            string remark = string.Empty;
                            if(data.Columns.Contains("Remark"))
                                remark = data.Rows[i]["Remark"].ToString();
                            if(string.IsNullOrEmpty(remark))
                            {
                                row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                            }
                            else
                            {
                                string content = data.Rows[i][j].ToString();
                                //if(content.Length > 3)
                                //{
                                ICell icell = row.CreateCell(j);
                                //设置富文本，部分文字加粗
                                XSSFRichTextString richtext = new XSSFRichTextString(data.Rows[i][j].ToString());
                                //XSSFRichTextString
                                IFont font3 = workbook.CreateFont();
                                //font3.TypeOffset = (short)FontSuperScript.SUPER;
                                //font3.IsItalic = true;//是否倾斜
                                font3.Color = HSSFColor.Blue.Index;
                                font3.IsBold = true;//是否加粗
                                //文本中从索引应用此格式
                                Dictionary<int, int> listDict = GetIndexList(remark, data.Rows[i][j].ToString());
                                foreach(KeyValuePair<int, int> key in listDict)
                                {
                                    richtext.ApplyFont(key.Key, key.Value, font3);
                                }
                                icell.SetCellValue(richtext);
                                //}
                                //else
                                //    row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                            }
                        }
                        else
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                //表格自适应宽度
                for(i = 0; i < data.Columns.Count; ++i)
                {
                    sheet.AutoSizeColumn(i);
                }
                workbook.Write(fs); //写入到excel
                Dispose();
                return count;
            }
            catch(Exception ex)
            {
                Dispose();
                throw new Exception(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// 导出转换后的造句
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sheetName"></param>
        /// <param name="isColumnWritten"></param>
        /// <returns></returns>
        public int ExportExchangeWordExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if(fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if(fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if(workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if(isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    //ICell icell1top;
                    for(j = 0; j < data.Columns.Count; ++j)
                    {
                        if(data.Columns.Contains("Remark") && data.Columns[j].ColumnName == "Remark")
                            continue;
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                        //icell1top = row.CreateCell(j);
                        //icell1top.CellStyle = Getcellstyle(workbook, stylexls.头);
                        //icell1top.SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }
                ////设置列宽
                //if(isExport)
                //{
                //    sheet.SetColumnWidth(1, 70 * 256);
                //    sheet.SetColumnWidth(2, 20 * 256);
                //    sheet.SetColumnWidth(3, 40 * 256);
                //}
                //else
                //{
                //    //for(i = 0; i < data.Columns.Count; ++i)
                //    //{
                //    //    sheet.AutoSizeColumn(i);
                //    //}
                //}

                //HSSFRichTextString richtext = new HSSFRichTextString("Microsoft OfficeTM");
                //IFont fontBlue = workbook.CreateFont();
                ////fontBlue.FontHeightInPoints = 12;
                //fontBlue.Color = HSSFColor.Blue.Index;
                //fontBlue.IsBold = true;
                //richtext.ApplyFont(0, 6, fontBlue);


                for(i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    //ICell icell;
                    for(j = 0; j < data.Columns.Count; ++j)
                    {
                        if(data.Columns.Contains("Remark") && data.Columns[j].ColumnName == "Remark")
                            continue;
                        if(data.Columns[j].ColumnName == "单词")
                        {
                            string remark = string.Empty;
                            if(data.Columns.Contains("Remark"))
                                remark = data.Rows[i]["Remark"].ToString();
                            if(string.IsNullOrEmpty(remark))
                            {
                                row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                            }
                            else
                            {
                                string content = data.Rows[i][j].ToString();
                                //if(content.Length > 3)
                                //{
                                ICell icell = row.CreateCell(j);
                                //设置富文本，部分文字加粗
                                XSSFRichTextString richtext = new XSSFRichTextString(data.Rows[i][j].ToString());
                                //XSSFRichTextString
                                IFont font3 = workbook.CreateFont();
                                //font3.TypeOffset = (short)FontSuperScript.SUPER;
                                //font3.IsItalic = true;//是否倾斜
                                font3.Color = HSSFColor.Blue.Index;
                                font3.IsBold = true;//是否加粗
                                //文本中从索引应用此格式
                                Dictionary<int, int> listDict = GetIndexList(remark, data.Rows[i][j].ToString());
                                foreach(KeyValuePair<int, int> key in listDict)
                                {
                                    richtext.ApplyFont(key.Key, key.Value, font3);
                                }
                                icell.SetCellValue(richtext);
                                //}
                                //else
                                //    row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                            }
                        }
                        else
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                //表格自适应宽度
                for(i = 0; i < data.Columns.Count; ++i)
                {
                    sheet.AutoSizeColumn(i);
                }
                workbook.Write(fs); //写入到excel
                Dispose();
                return count;
            }
            catch(Exception ex)
            {
                Dispose();
                throw new Exception(ex.ToString());
                return -1;
            }
        }



        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if(fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if(fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if(sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if(sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if(sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if(isFirstRowColumn)
                    {
                        for(int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if(cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if(cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for(int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if(row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for(int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if(j >= 0 && row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
                //Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    if(fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }
        /// <summary>
        /// 获取字符串中出现的位置
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="association"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetIndexList(string remark, string association)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            string[] splitArray = remark.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            bool isMatch = false;
            foreach(string str in splitArray)
            {
                string checkString = str;
                int firstIndexof = association.IndexOf(checkString);
                if(firstIndexof >= 0)
                {
                    isMatch = true;
                    int endIndexof = checkString.Length;
                    if(!dict.ContainsKey(firstIndexof))
                        dict.Add(firstIndexof, firstIndexof + endIndexof);
                }
            }
            if(!isMatch)
            {
                foreach(string str in splitArray)
                {
                    string checkString = str.Substring(0, str.Length - 1);
                    int firstIndexof = association.IndexOf(checkString);
                    if(firstIndexof >= 0)
                    {
                        isMatch = true;
                        int endIndexof = checkString.Length;
                        if(!dict.ContainsKey(firstIndexof))
                            dict.Add(firstIndexof, firstIndexof + endIndexof);
                    }
                }
            }

            //if (firstIndexof < 0)
            //{
            //    //查询从第一个字
            //    checkString = str.Substring(0, 1);
            //    firstIndexof = association.IndexOf(checkString);
            //    if (firstIndexof > 0)
            //    {
            //        if (!dict.ContainsKey(firstIndexof))
            //            dict.Add(firstIndexof, firstIndexof + 1);
            //    }
            //}
            //else
            //{
            //    int endIndexof = checkString.Length;
            //    if (!dict.ContainsKey(firstIndexof))
            //        dict.Add(firstIndexof, firstIndexof + endIndexof);
            //}


            return dict;
        }

        public enum stylexls
        {
            头,
            url,
            时间,
            数字,
            钱,
            百分比,
            中文大写,
            科学计数法,
            默认,
            English
        }

        static ICellStyle Getcellstyle(IWorkbook wb, stylexls str)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();

            //定义几种字体
            //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的
            IFont font12 = wb.CreateFont();
            font12.FontHeightInPoints = 10;
            font12.FontName = "微软雅黑";


            IFont font = wb.CreateFont();
            font.FontName = "微软雅黑";
            //font.Underline = 1;下划线


            IFont fontcolorblue = wb.CreateFont();
            fontcolorblue.Color = HSSFColor.OliveGreen.Blue.Index;
            fontcolorblue.IsItalic = true;//下划线
            fontcolorblue.FontName = "微软雅黑";

            IFont timesNewRoman = wb.CreateFont();
            timesNewRoman.Color = HSSFColor.Black.Index;
            timesNewRoman.FontName = "Times New Roman";


            //边框
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Dotted;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Hair;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Hair;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Dotted;
            //边框颜色
            cellStyle.BottomBorderColor = HSSFColor.OliveGreen.Blue.Index;
            cellStyle.TopBorderColor = HSSFColor.OliveGreen.Blue.Index;

            //背景图形，我没有用到过。感觉很丑
            //cellStyle.FillBackgroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;
            //cellStyle.FillForegroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;
            cellStyle.FillForegroundColor = HSSFColor.White.Index;
            // cellStyle.FillPattern = FillPatternType.NO_FILL;
            cellStyle.FillBackgroundColor = HSSFColor.Maroon.Index;

            //水平对齐
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

            //垂直对齐
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            //自动换行
            cellStyle.WrapText = true;

            //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对
            cellStyle.Indention = 0;

            //上面基本都是设共公的设置
            //下面列出了常用的字段类型
            switch(str)
            {
                case stylexls.头:
                    // cellStyle.FillPattern = FillPatternType.LEAST_DOTS;
                    cellStyle.SetFont(font12);
                    break;
                case stylexls.时间:
                    IDataFormat datastyle = wb.CreateDataFormat();

                    cellStyle.DataFormat = datastyle.GetFormat("yyyy/mm/dd");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.数字:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.English:
                    cellStyle.SetFont(timesNewRoman);
                    break;
                case stylexls.钱:
                    IDataFormat format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.url:
                    fontcolorblue.Underline = FontUnderlineType.Single;
                    cellStyle.SetFont(fontcolorblue);
                    break;
                case stylexls.百分比:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.中文大写:
                    IDataFormat format1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.科学计数法:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.默认:
                    cellStyle.SetFont(font);
                    break;
            }
            return cellStyle;
        }

        public static string GetNewString(string name)
        {
            //按照；拆分
            //先用空格拆分
            string[] emptyArray = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Empty;
            foreach(string noEmptyName in emptyArray)
            {
                //再用;拆分
                string[] array = noEmptyName.Split(new char[] { '；', '，' }, StringSplitOptions.RemoveEmptyEntries);
                string pattern = "[\u4e00-\u9fa5]+";
                foreach(string str in array)
                {
                    //替换掉英文字符
                    //中文
                    //英文
                    //pattern = "[a-zA-Z]";
                    //Regex r = new Regex(pattern);
                    Match matc = Regex.Match(str, pattern);
                    //return r.ToString();
                    //return r.Replace(name, "");
                    if(matc.Success)
                    {
                        result += matc.Value + "|";
                    }
                }
            }
            return result;
        }

        public static string SetNewWordAssociation(string chinese, string Association)
        {
            string result = Association;
            if(!string.IsNullOrEmpty(Association))
            {
                Dictionary<int, int> listDict = NpoiService.GetIndexList(chinese, Association);
                int i = 0;
                foreach(KeyValuePair<int, int> key in listDict)
                {
                    string oldMatch = Association.Substring(key.Key, ((Association.Length - key.Key) - (Association.Length - key.Value)));
                    if(!string.IsNullOrEmpty(oldMatch))
                        result = result.Replace(oldMatch, "<span class='sp_checked'>" + oldMatch + "</span>");
                }
            }
            return result;
        }

    }
}