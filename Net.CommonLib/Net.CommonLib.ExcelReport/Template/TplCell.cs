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
using System.Data;

namespace Net.CommonLib.ExcelReport
{
    public class TplCell
    {
        public TplCell()
        {
            TplDefaultContent = TplGroupColumnName = TplTextContent = TplValueColName = string.Empty;
        }

        /// <summary>
        ///     单元格分组对齐方式
        /// </summary>
        public GroupAlign GroupAlign { get; set; }

        /// <summary>
        ///     单元格公式
        /// </summary>
        public CellForumla Formula { get; set; }

        /// <summary>
        ///     单元格水平分组时添加方式
        /// </summary>
        public InsertOption HgOption { get; set; }

        /// <summary>
        /// 单元格列索引(Sheet中)
        /// </summary>
        public int LastColIndex { get; set; }

        /// <summary>
        /// 单元格最近分组值
        /// </summary>
        public object LastGroupedValue { get; set; }

        /// <summary>
        ///     单元格合并方式
        /// </summary>
        public MergeOption MergeOption { get; set; }

        /// <summary>
        /// 获取分组值时重用Key
        /// </summary>
        public ReusedKey ReusedKey { get; set; }

        /// <summary>
        ///     无绑定值时显示默认值
        /// </summary>
        public string TplDefaultContent { get; set; }

        /// <summary>
        ///     绑定字段显示格式
        /// </summary>
        public string TplFormat { get; set; }

        /// <summary>
        ///     分组列绑定字段 hg: ; vg:
        /// </summary>
        public string TplGroupColumnName { get; set; }

        /// <summary>
        ///     模版行单元格区域
        /// </summary>
        public CellRange TplRange { get; set; }

        /// <summary>
        ///     模版单元格原始文本(添加动态列时更新值)
        /// </summary>
        public string TplTextContent { get; set; }

        /// <summary>
        ///     单元格值绑定字段(无公式时)
        /// </summary>
        public string TplValueColName { get; set; }

        /// <summary>
        ///     使用R1C1引用公式
        /// </summary>
        public bool UseR1C1Formula { get; set; }

        public TplCell Copy()
        {
            var cell = new TplCell
            {
                GroupAlign = GroupAlign,
                LastColIndex = LastColIndex,
                TplGroupColumnName = TplGroupColumnName,
                TplValueColName = TplValueColName,
                TplFormat = TplFormat,
                TplTextContent = TplTextContent,
                TplDefaultContent = TplDefaultContent,
                UseR1C1Formula = UseR1C1Formula,
                HgOption = HgOption
            };
            if (Formula != null)
            {
                cell.Formula = Formula.Copy();
            }
            cell.LastGroupedValue = null;
            return cell;
        }

        public bool DoMerge(CellRange currentCellRange)
        {
            return currentCellRange != null && RangeHelper.MergeRanges(currentCellRange, MergeOption);
        }

        public bool IsNeedNewCell(GroupDataHolder holder, InsertOption insertOption, int currentColIndex,
            int currentRowIndex, DataTable table, int valueRowIndex)
        {
            if (GroupAlign == GroupAlign.Always)
            {
                return true;
            }
            if (GroupAlign == GroupAlign.None)
            {
                //无分组时,行添加方式为all时才新增单元格
                return ((insertOption & InsertOption.Always) != InsertOption.Never);
            }
            var currentGroupValue = GetGroupValue(holder, table, valueRowIndex);
            switch (GroupAlign)
            {
                case GroupAlign.Vertical:
                case GroupAlign.Horizontal:
                    if ((insertOption & InsertOption.BeforeChange) == InsertOption.Never)
                    {
                        //行添加方式不为before时,判断与上一分组值是否相等(不相等则新增)
                        if ((LastGroupedValue != null) && LastGroupedValue.Equals(currentGroupValue))
                        {
                            return false;
                        }
                        LastGroupedValue = currentGroupValue;
                        return true;
                    }
                    if ((valueRowIndex + 1) < table.Rows.Count)
                    {
                        //before时(非数据源最后一行),判断与下一行单元格值是否相等(不相等则新增)
                        var nextGroupValue = GetGroupValue(holder, table, valueRowIndex + 1);
                        return !Equals(currentGroupValue, nextGroupValue);
                    }
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 写Sheet表单元格
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="holder"></param>
        /// <param name="currentCellRange"></param>
        /// <param name="table"></param>
        /// <param name="valueRowIndex"></param>
        public void WriteCell(ReportSheetTemplate tpl, GroupDataHolder holder, CellRange currentCellRange,
            DataTable table, int valueRowIndex)
        {
            if (UseR1C1Formula)
            {
                currentCellRange.FormulaR1C1 = "=" + TplTextContent;
            }
            else
            {
                var cellValue = GetValue(holder, table, valueRowIndex);
                if ((cellValue == null || cellValue == string.Empty) ||
                    (cellValue == DBNull.Value))
                {
                    cellValue = TplDefaultContent;
                }
                RangeHelper.UpdateCellValue(tpl, currentCellRange, cellValue, TplFormat);
                if (GroupAlign == GroupAlign.Vertical)
                {
                    //纵向分组才更新最后分组值,水平分组在判断是否新增单元格时更新(before情况)
                    LastGroupedValue = GetGroupValue(holder, table, valueRowIndex);
                }
            }
        }

        /// <summary>
        /// 获取单元格显示值 优先级:公式->水平分组列添加值->数据源值
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="table"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private object GetValue(GroupDataHolder holder, DataTable table, int rowIndex)
        {
            if (Formula != null)
            {
                return Formula.GetValue(holder, table, rowIndex);
            }
            if (HgOption != InsertOption.Never)
            {
                return TplTextContent; //插入动态列时更新赋值
            }
            var tableValue = RangeHelper.GetTableValue(table, rowIndex, TplValueColName);
            return tableValue ?? TplTextContent;
        }

        /// <summary>
        /// 获取分组值
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="table"></param>
        /// <param name="valueIndex"></param>
        /// <returns></returns>
        public object GetGroupValue(GroupDataHolder holder, DataTable table, int valueIndex)
        {
            if (ReusedKey == null)
            {
                ReusedKey = SearchKey.FindReusedKey(TplGroupColumnName);
            }
            return ReusedKey.GetReusedValue(table, valueIndex);
        }
    }
}