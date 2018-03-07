/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using Spire.Xls;
using System.Collections.Generic;
using System.Data;

namespace Net.CommonLib.ExcelReport
{
    public class TplLine
    {
        public TplLine()
        {
            StartColumnIndex = 1;
            InsertOption = InsertOption.AfterChange;
            CellList = new List<TplCell>();
            InsertedRowList = new List<int>(10000);
        }

        /// <summary>
        ///     包含模版单元格列表(添加动态列时更新)
        /// </summary>
        public List<TplCell> CellList { get; set; }

        /// <summary>
        ///     模版行起始列索引
        /// </summary>
        public int StartColumnIndex { get; set; }

        /// <summary>
        ///     是否包含横向分组
        /// </summary>
        public bool ContainsHGroup { get; set; }

        /// <summary>
        ///     添加行索引列表
        /// </summary>
        public List<int> InsertedRowList { get; set; }

        /// <summary>
        ///     行新增方式(默认AfterChange)
        /// </summary>
        public InsertOption InsertOption { get; set; }

        /// <summary>
        ///     Sheet模版
        /// </summary>
        public ReportSheetTemplate SheetTemplate { get; set; }

        /// <summary>
        ///     包含单元格数(动态列添加时更新)
        /// </summary>
        public int TplCellCount { get; set; }

        /// <summary>
        ///     模版行区域(动态列添加时更新区域)
        /// </summary>
        public CellRange TplRange { get; set; }

        /// <summary>
        ///     复制填充行(非分组更新单元格值)
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="currentRowIndex"></param>
        /// <param name="table"></param>
        /// <param name="valueRowIndex"></param>
        /// <returns></returns>
        public bool FillLine(GroupDataHolder holder, int currentRowIndex, DataTable table, int valueRowIndex)
        {
            var startCellIndex = IsNeedNewLine(holder, currentRowIndex, table, valueRowIndex);
            if (startCellIndex < 0)
            {
                return false;
            }

            RangeHelper.InsertCopyRange(TplRange.Worksheet, TplRange,
                StartColumnIndex, currentRowIndex, CellList.Count, 1,
                InsertRangeDirection.Down);

            InsertedRowList.Add(currentRowIndex);

            UpdateLine(holder, currentRowIndex, startCellIndex, table, valueRowIndex, MergeOption.Left, false);
            UpdateLine(holder, currentRowIndex, startCellIndex, table, valueRowIndex, MergeOption.Up, true);
            return true;
        }

        /// <summary>
        ///     是否新增行,返回新增行起始单元格索引(小于0不新增行)
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="currentRowIndex"></param>
        /// <param name="table"></param>
        /// <param name="valueRowIndex"></param>
        /// <returns></returns>
        public int IsNeedNewLine(GroupDataHolder holder, int currentRowIndex, DataTable table, int valueRowIndex)
        {
            if (InsertOption != InsertOption.Never)
            {
                //数据源第一行且行添加方式为First时新增行
                if ((valueRowIndex == 0) && ((InsertOption & InsertOption.OnFirst) != InsertOption.Never))
                {
                    return 0;
                }
                //数据源最后一行且行添加方式为Last时新增行
                if ((valueRowIndex == (table.Rows.Count - 1)) &&
                    ((InsertOption & InsertOption.OnLast) != InsertOption.Never))
                {
                    return 0;
                }
                if ((((InsertOption & InsertOption.AfterChange) == InsertOption.Never) &&
                     ((InsertOption & InsertOption.BeforeChange) == InsertOption.Never)) &&
                    ((InsertOption & InsertOption.Always) == InsertOption.Never))
                {
                    return -1;
                }
                for (var i = 0; i < CellList.Count; i++)
                {
                    var cell = CellList[i];
                    if ((cell.GroupAlign != GroupAlign.Horizontal) &&
                        cell.IsNeedNewCell(holder, InsertOption, cell.LastColIndex, currentRowIndex, table,
                            valueRowIndex))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        ///     更新行(合并处理,单元格无计算公式时填充)
        /// </summary>
        /// <param name="holder">分组数据持有</param>
        /// <param name="currentRowIndex">当前行</param>
        /// <param name="startCellIndex">起始单元格列索引</param>
        /// <param name="table">数据源</param>
        /// <param name="valueRowIndex">数据源行</param>
        /// <param name="mergeOption">合并方式</param>
        /// <param name="updateMergeOnly">只合并(不写数据)</param>
        private void UpdateLine(GroupDataHolder holder, int currentRowIndex, int startCellIndex, DataTable table,
            int valueRowIndex, MergeOption mergeOption, bool updateMergeOnly)
        {
            var lastColIndex = CellList[0].LastColIndex;
            for (var i = 0; i < CellList.Count; i++)
            {
                var cell = CellList[i];
                var isMerged = false;

                //分组合并处理
                if ((cell.MergeOption == mergeOption) && (
                    ((i < startCellIndex) || //startCellIndex:第一个需要分组的索引
                     ((i >= startCellIndex) && ((InsertOption & InsertOption.BeforeChange) != InsertOption.Never)))
                    || (cell.GroupAlign == GroupAlign.None)))
                {
                    var currentCellRange = RangeHelper.GetCell(TplRange.Worksheet, lastColIndex, currentRowIndex);
                    isMerged = cell.DoMerge(currentCellRange);
                }

                //无计算公式时,填充值
                if ((!isMerged && !updateMergeOnly) && (cell.Formula == null))
                {
                    var cellRange = RangeHelper.GetCell(TplRange.Worksheet, lastColIndex, currentRowIndex);
                    cell.WriteCell(SheetTemplate, holder, cellRange, table, valueRowIndex);
                }

                lastColIndex++;
            }
        }

        /// <summary>
        ///     单元格有公式时更新行数据.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="currentRowIndex"></param>
        /// <param name="table"></param>
        /// <param name="valueRowIndex"></param>
        public void UpdateRowData(GroupDataHolder holder, int currentRowIndex, DataTable table, int valueRowIndex)
        {
            var lastColIndex = CellList[0].LastColIndex;
            foreach (var cell in CellList)
            {
                if (cell.Formula != null)
                {
                    var currentCellRange = RangeHelper.GetCell(TplRange.Worksheet, lastColIndex, currentRowIndex);
                    cell.WriteCell(SheetTemplate, holder, currentCellRange, table, valueRowIndex);
                }
                lastColIndex++;
            }
        }
    }
}