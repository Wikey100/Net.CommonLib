/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using Spire.Xls;
using System;
using System.Collections;
using System.Data;
using System.Globalization;

namespace Net.CommonLib.ExcelReport
{
    public class RangeHelper
    {
        public static int GetColValueByIndexCount;
        public static int GetColValueCount;
        public static int GetRangeCallTimes;
        public static int InsertCopyRangeCallTimes;
        public static int UpdateCellValueCallTimes;

        public static object FormatValue(ReportSheetTemplate tpl, object value, string format)
        {
            if (!string.IsNullOrEmpty(format) && (value != null))
            {
                switch (format)
                {
                    case TemplateFlags.FormatDate:
                        return ValueToDate(value);

                    case TemplateFlags.FormatMonth:
                        return ValueToMonth(value);

                    case TemplateFlags.FormatTime:
                        return ValueToTime(value);

                    case TemplateFlags.FormatDateTime:
                        return ValueToDateTime(value);

                    case TemplateFlags.FormatTimeSpan:
                        return ValueToTimeSpan(value, tpl);

                    case TemplateFlags.FormatFen:
                        return ValueToFen(value);
                }
            }
            return value;
        }

        public static CellRange GetCell(Worksheet sheet, int startColumn, int startRow)
        {
            return GetRange(sheet, startColumn, startRow, 1, 1);
        }

        /// <summary>
        /// 从DataTable中获取值(指定行列,列名不存在时返回null)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static object GetTableValue(DataTable table, int row, string colName)
        {
            GetColValueCount++;
            var index = table.Columns.IndexOf(colName);
            if (!string.IsNullOrEmpty(colName) && (index >= 0))
            {
                return table.Rows[row][index];
            }
            return null;
        }

        public static object GetColValueByIndex(DataTable table, int rowIndex, int colIndex)
        {
            GetColValueByIndexCount++;
            return table.Rows[rowIndex][colIndex];
        }

        public static CellRange GetEntireCol(Worksheet sheet, int colIndex)
        {
            return sheet[1, colIndex].EntireColumn;
        }

        public static CellRange GetEntireRow(Worksheet sheet, int rowIndex)
        {
            return sheet[rowIndex, 1].EntireRow;
        }

        public static CellRange GetLine(Worksheet sheet, int startColumn, int startRow)
        {
            return sheet[startRow, startColumn].EntireRow;
        }

        public static CellRange GetRange(Worksheet sheet, int startColumn, int startRow, int columns, int rows)
        {
            GetRangeCallTimes++;
            return sheet[startRow, startColumn, (startRow + rows) - 1, (startColumn + columns) - 1];
        }

        public static IEnumerator GetRangeCells(CellRange range)
        {
            var array = (Array)range.GetType().BaseType.GetProperty("Cells").GetValue(range, new object[0]);
            return array.GetEnumerator();
        }

        /// <summary>
        /// 插入复制单元格区域
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="orign">原始区域</param>
        /// <param name="targetColumn">目标起始列</param>
        /// <param name="targetRow">目标起始行</param>
        /// <param name="columns">复制列数</param>
        /// <param name="rows">复制行数</param>
        /// <param name="direction">复制方向</param>
        /// <param name="lastColumnCount">末尾剩余列数(向右复制时先移动,再新增)</param>
        /// <returns></returns>
        public static CellRange InsertCopyRange(Worksheet sheet, CellRange orign,
            int targetColumn, int targetRow, int columns, int rows,
            InsertRangeDirection direction, int lastColumnCount = 1)
        {
            InsertCopyRangeCallTimes++;

            var destRange = GetRange(sheet, targetColumn, targetRow, columns, rows);

            if (direction == InsertRangeDirection.Right)
            {
                //向右复制时先把剩余列向右移动,并解除目标区域合并
                var orignColumnRange = GetRange(sheet, targetColumn, targetRow, lastColumnCount, rows);
                var destColumnRange = GetRange(sheet, targetColumn + columns, targetRow, lastColumnCount, rows);

                orignColumnRange.Move(destColumnRange, true, false);

                destRange = GetRange(sheet, targetColumn, targetRow, columns, rows);
                destRange.UnMerge();
            }

            orign.Copy(destRange, false, true);

            #region 复制行高,列宽

            var orignStartColumn = orign.Column;
            var orignStartRow = orign.Row;
            switch (direction)
            {
                case InsertRangeDirection.Right: //复制列宽
                    for (var i = 0; i < columns; i++)
                    {
                        var orignEntireCol = GetEntireCol(sheet, i + orignStartColumn);
                        var targetEntireCol = GetEntireCol(sheet, (i + targetColumn) + 1);
                        try
                        {
                            targetEntireCol.ColumnWidth = orignEntireCol.ColumnWidth;
                        }
                        catch (Exception exception2)
                        {
                            Console.WriteLine("Set ColumnWidth Error: " + exception2);
                        }
                    }
                    break;

                case InsertRangeDirection.Down: //复制行高
                    for (var j = 0; j < rows; j++)
                    {
                        var orignEntireRow = GetEntireRow(sheet, j + orignStartRow);
                        var targetEntireRow = GetEntireRow(sheet, j + targetRow);
                        try
                        {
                            targetEntireRow.RowHeight = orignEntireRow.RowHeight;
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("Set RowHeight Error: " + exception);
                        }
                    }
                    break;
            }

            #endregion 复制行高,列宽

            return destRange;
        }

        public static bool MergeRanges(CellRange currentCellRange, MergeOption mOption)
        {
            var sheet = currentCellRange.Worksheet;
            CellRange range;
            currentCellRange.Value2 = null;
            currentCellRange.Text = "";
            switch (mOption)
            {
                case MergeOption.Up:
                    {
                        range = GetRange(sheet, currentCellRange.Column, currentCellRange.Row - 1, 1, 1);
                        if (!range.HasMerged)
                        {
                            GetRange(sheet, currentCellRange.Column, currentCellRange.Row - 1, 1, 2).Merge();
                            break;
                        }
                        var mergeArea = range.MergeArea;
                        var columns = (currentCellRange.Column - mergeArea.Column) + 1;
                        var rows = (currentCellRange.Row - mergeArea.Row) + 1;
                        if (columns < mergeArea.ColumnCount)
                        {
                            columns = mergeArea.ColumnCount;
                        }
                        if (rows < mergeArea.RowCount)
                        {
                            rows = mergeArea.RowCount;
                        }
                        GetRange(sheet, mergeArea.Column, mergeArea.Row, columns, rows).Merge();
                        break;
                    }
                case MergeOption.Left:
                    range = GetRange(sheet, currentCellRange.Column - 1, currentCellRange.Row, 1, 1);
                    if (!range.HasMerged)
                    {
                        GetRange(sheet, currentCellRange.Column - 1, currentCellRange.Row, 2, 1).Merge();
                    }
                    else
                    {
                        var range3 = range.MergeArea;
                        var columnCount = (currentCellRange.Column - range3.Column) + 1;
                        var rowCount = (currentCellRange.Row - range3.Row) + 1;
                        if (columnCount < range3.ColumnCount)
                        {
                            columnCount = range3.ColumnCount;
                        }
                        if (rowCount < range3.RowCount)
                        {
                            rowCount = range3.RowCount;
                        }
                        GetRange(sheet, range3.Column, range3.Row, columnCount, rowCount).Merge();
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        public static void UpdateCellValue(ReportSheetTemplate tpl, CellRange cell, object value, string format)
        {
            UpdateCellValueCallTimes++;
            value = FormatValue(tpl, value, format);
            try
            {
                switch (format)
                {
                    case TemplateFlags.FormatDate:
                    case TemplateFlags.FormatMonth:
                    case TemplateFlags.FormatTime:
                    case TemplateFlags.FormatDateTime:
                    case TemplateFlags.FormatTimeSpan:
                        break;

                    case TemplateFlags.FormatFen:
                    case TemplateFlags.FormatNumber:
                        double outValue;
                        cell.NumberValue = double.TryParse(value == null ? string.Empty : value.ToString(), out outValue)
                            ? outValue
                            : 0.0;
                        return;
                }
            }
            catch
            {
                // ignored
            }
            cell.Text = Convert.ToString(value);
        }

        private static object ValueToDate(object value)
        {
            var s = value.ToString();
            if (s.Length < 8)
            {
                return value;
            }
            if (s.Length > 8)
            {
                s = s.Substring(0, 8);
            }
            try
            {
                return DateTime.ParseExact(s, "yyyyMMdd", null).ToString("yyyy年MM月dd日");
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Concat("parse Date Error [", s, "]: ", exception));
                return value;
            }
        }

        private static object ValueToDateTime(object value)
        {
            var s = value.ToString();
            if (s.Length < 14)
            {
                return value;
            }
            if (s.Length > 14)
            {
                s = s.Substring(0, 14);
            }
            try
            {
                return DateTime.ParseExact(s, "yyyyMMddHHmmss", null).ToString("yyyy年MM月dd日 HH:mm:ss");
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Concat("parse Datetime Error [", s, "]: ", exception));
                return value;
            }
        }

        private static object ValueToFen(object value)
        {
            try
            {
                return (Convert.ToDouble(value) / 100.0);
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Concat("parse double Error [", value, "]: ", exception));
                return value;
            }
        }

        private static object ValueToMonth(object value)
        {
            var s = value.ToString();
            if (s.Length < 6)
            {
                return value;
            }
            if (s.Length > 6)
            {
                s = s.Substring(0, 6);
            }
            try
            {
                return DateTime.ParseExact(s, "yyyyMM", null).ToString("yyyy年MM月");
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Concat("parse Month Error [", s, "]: ", exception));
                return value;
            }
        }

        private static object ValueToTime(object value)
        {
            var s = value.ToString();
            if (s.Length < 6)
            {
                return value;
            }
            if (s.Length > 6)
            {
                s = s.Substring(s.Length - 6);
            }
            try
            {
                return DateTime.ParseExact(s, "HHmmss", null).ToString("HH:mm:ss");
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Concat("parse Time Error [", s, "]: ", exception));
                return value;
            }
        }

        private static object ValueToTimeSpan(object value, ReportSheetTemplate tpl)
        {
            int num;
            object obj2;
            DateTime time;
            if (((value == null) || !tpl.ParamMap.TryGetValue("time_span", out obj2)) ||
                (!int.TryParse(obj2.ToString(), out num)
                 || !DateTime.TryParseExact(value.ToString(), "yyyyMMddHHmmss", null, DateTimeStyles.None, out time)))
            {
                return ValueToTime(value);
            }
            var time2 = time.Add(new TimeSpan(0, 0, num, 0));
            return (time.ToString("HH:mm") + "-" + time2.ToString("HH:mm"));
        }
    }
}