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
    public class TplBlock
    {
        public TplBlock()
        {
            GroupKeyList = new List<GroupValueSearchKey>(10000);
            CountedMap = new Dictionary<GroupValueSearchKey, bool>(10000);
            Holder = new GroupDataHolder();

            TplLineList = new List<TplLine>();

            Joinat = -1;
            LastUsedLineValueIndex = -1;

            TplColumCount = 1;
            TplRowCount = 1;

            DataTableIndex = TplColumnTableIndex = -1;
        }

        /// <summary>
        ///     模版分组值查找Key列表
        /// </summary>
        public List<GroupValueSearchKey> GroupKeyList { get; set; }

        /// <summary>
        ///     分组值查找Key计算字典(添加数据行时判断Key值是否已计算)
        /// </summary>
        public Dictionary<GroupValueSearchKey, bool> CountedMap { get; set; }

        /// <summary>
        ///     分组Key数据值持有(更新,获取)
        /// </summary>
        public GroupDataHolder Holder { get; set; }

        /// <summary>
        ///     包含模版行列表
        /// </summary>
        public List<TplLine> TplLineList { get; set; }

        /// <summary>
        ///     模版块解析起始列索引
        /// </summary>
        public int StartParseColumnIndex { get; set; }

        /// <summary>
        ///     模版块解析起始行索引
        /// </summary>
        public int StartParseRowIndex { get; set; }

        /// <summary>
        ///     模版块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     只复制(用于填充固定参数(#开始))
        /// </summary>
        public bool CopyOnly { get; set; }

        /// <summary>
        ///     块中合并起始列索引(与Sheet中第一个绑定数据Block右边合并)
        /// </summary>
        public int Joinat { get; set; }

        /// <summary>
        ///     模板块包含列数
        /// </summary>
        public int TplColumCount { get; set; }

        /// <summary>
        ///     模板块包含行数
        /// </summary>
        public int TplRowCount { get; set; }

        /// <summary>
        ///     模版块区域
        /// </summary>
        public CellRange TplRange { get; set; }

        /// <summary>
        ///     模板块数据源索引[DataSet中]
        /// </summary>
        public int DataTableIndex { get; set; }

        /// <summary>
        ///     模版块动态列绑定数据源索引[DataSet中]
        /// </summary>
        public int TplColumnTableIndex { get; set; }

        /// <summary>
        ///     填充最近一行数据时是否更新所有行
        /// </summary>
        public bool UpdateAllRow { get; set; }

        /// <summary>
        ///     是否已创建动态列(使用动态列数据源表绑定,而不是在遍历数据源表时动态添加)
        /// </summary>
        public bool IsDynamicCloumnsCreated { get; set; }

        /// <summary>
        ///     动态列(解析模版时包含hg对应横向分组列)
        /// </summary>
        public TplCloumn DynamicColumn { get; set; }

        /// <summary>
        ///     块填充行数
        /// </summary>
        public int RowsCount { get; set; }

        /// <summary>
        ///     块填充列数
        /// </summary>
        public int ColumnsCount { get; set; }

        /// <summary>
        ///     最近使用模版行(填充数据源时)
        /// </summary>
        public TplLine LastUsedLine { get; set; }

        /// <summary>
        ///     最近使用数据源中行索引(填充数据源时)
        /// </summary>
        public int LastUsedLineValueIndex { get; set; }

        public int CreateDynamicColumn(DataTable table)
        {
            if (DynamicColumn == null)
            {
                return 0;
            }
            ColumnsCount -= DynamicColumn.GroupColumnCount;
            if ((table == null) || (table.Rows.Count <= 0))
            {
                return 0;
            }
            for (var i = 0; i < table.Rows.Count; i++)
            {
                DynamicColumn.CheckEachColumn(this, Holder, 0, table, i);
            }
            IsDynamicCloumnsCreated = true;
            RangeHelper.GetRange(TplRange.Worksheet, DynamicColumn.StartColIndex + DynamicColumn.GroupColumnCount,
                TplRange.Row,
                ((TplColumCount - DynamicColumn.StartCellIndex) - DynamicColumn.GroupColumnCount) - 1, TplRowCount)
                .Copy(
                    RangeHelper.GetRange(TplRange.Worksheet, DynamicColumn.StartColIndex, TplRange.Row,
                        ((TplColumCount - DynamicColumn.StartCellIndex) - DynamicColumn.GroupColumnCount) - 1,
                        TplRowCount), true, true);
            TplColumCount -= DynamicColumn.GroupColumnCount;
            foreach (var line in TplLineList)
            {
                line.CellList.RemoveRange(DynamicColumn.StartCellIndex, DynamicColumn.GroupColumnCount);
                line.TplCellCount -= DynamicColumn.GroupColumnCount;
                line.TplRange = RangeHelper.GetRange(TplRange.Worksheet, 3, line.TplRange.Row, TplColumCount, 1);
            }
            return 1;
        }

        public int FillBlock(DataTable table)
        {
            var rowIndex = StartParseRowIndex;
            for (var i = 0; i < table.Rows.Count; i++)
            {
                CountedMap.Clear();
                if ((DynamicColumn != null) && !IsDynamicCloumnsCreated)
                {
                    DynamicColumn.CheckColumn(this, Holder, rowIndex, table, i);
                }
                foreach (var groupKey in GroupKeyList)
                {
                    if (groupKey.ReusedKey == null)
                    {
                        groupKey.ReusedKey = SearchKey.FindReusedKey(groupKey.ValueColName);
                    }
                    var copyKey = groupKey.Copy();
                    if (copyKey.SearchKey != null)
                    {
                        copyKey.SearchKey.FillKey(table, i);
                    }
                    Holder.AddValue(CountedMap, copyKey, table, i);
                }
                for (var k = 0; k < TplLineList.Count; k++)
                {
                    var updateLine = TplLineList[k];
                    var isNewLine = updateLine.FillLine(Holder, rowIndex, table, i);
                    if (isNewLine)
                    {
                        FillLastLine(k, LastUsedLine, rowIndex - 1, table, LastUsedLineValueIndex); //?填充上一行值
                        LastUsedLine = updateLine;
                        LastUsedLineValueIndex = i;
                        if ((i + 1) >= table.Rows.Count)
                        {
                            FillLastLine(k, updateLine, rowIndex, table, LastUsedLineValueIndex);
                        }

                        rowIndex += 1;
                        RowsCount += 1;
                    }
                }
            }
            MergeHGroupCells();
            if (!table.ExtendedProperties.ContainsKey("TableType") ||
                (table.ExtendedProperties["TableType"].ToString() != "CustumEmpty"))
            {
                MergeVGroupCells();
            }
            return RowsCount;
        }

        private void FillLastLine(int currentLineIndex, TplLine updateLine, int rowIndex, DataTable table,
            int valueIndex)
        {
            if (updateLine != null)
            {
                updateLine.UpdateRowData(Holder, rowIndex, table, valueIndex);
                if (UpdateAllRow)
                {
                    for (var i = 0; i <= currentLineIndex; i++)
                    {
                        var line = TplLineList[i];
                        if (line.ContainsHGroup && (line.InsertedRowList.Count > 0))
                        {
                            line.UpdateRowData(Holder, line.InsertedRowList[line.InsertedRowList.Count - 1], table,
                                valueIndex);
                        }
                    }
                }
            }
        }

        private TplCloumn FindDynamicColumn()
        {
            var column = new TplCloumn
            {
                GroupColumnCount = 0
            };

            //遍历模版行,以包含包含横向分组的第一行为基准,查找动态列数量
            foreach (var line in TplLineList)
            {
                for (var k = 0; k < line.CellList.Count; k++)
                {
                    var cell = line.CellList[k];
                    if (cell.GroupAlign == GroupAlign.Horizontal)
                    {
                        if (column.GroupColumnCount <= 0)
                        {
                            column.StartCellIndex = k;
                            column.StartColIndex = cell.LastColIndex;
                        }
                        column.GroupColumnCount++;
                    }
                }
                if (column.GroupColumnCount > 0)
                {
                    break;
                }
            }

            if (column.GroupColumnCount <= 0)
            {
                return null;
            }

            column.TplLastColumnIndex = (TplColumCount - column.StartCellIndex) - column.GroupColumnCount;
            if (column.TplLastColumnIndex <= 0)
            {
                column.TplLastColumnIndex = 1;
            }
            column.TplRange = RangeHelper.GetRange(TplRange.Worksheet, column.StartColIndex, StartParseRowIndex,
                column.GroupColumnCount,
                TplLineList.Count);

            //添加横向分组单元格(包含公式)
            foreach (var line in TplLineList)
            {
                foreach (var cell in line.CellList)
                {
                    if (cell.GroupAlign == GroupAlign.Horizontal)
                    {
                        var item = cell.Copy();
                        column.CellList.Add(item);
                        if (!column.GroupColList.Contains(cell.TplGroupColumnName))
                        {
                            column.GroupColList.Add(cell.TplGroupColumnName);
                        }
                    }
                }
            }
            return column;
        }

        public void InitDynamicColumn(ReportSheetTemplate template)
        {
            DynamicColumn = FindDynamicColumn();
            if (DynamicColumn != null)
            {
                DynamicColumn.Tpl = template;
            }
        }

        private void MergeHGroupCells()
        {
            for (var i = 0; i < TplLineList.Count; i++)
            {
                var line = TplLineList[i];
                if (line.ContainsHGroup)
                {
                    foreach (var startRow in line.InsertedRowList)
                    {
                        object objA = null;
                        var startColIndex = DynamicColumn.StartColIndex;
                        for (var k = 0; k < DynamicColumn.InsertCount; k++)
                        {
                            var currentCellRange = RangeHelper.GetCell(TplRange.Worksheet, startColIndex, startRow);
                            var objB = currentCellRange.Value2;
                            if ((objA != null) && Equals(objA, objB))
                            {
                                if ((i == (TplLineList.Count - 1)) || TplLineList[i + 1].ContainsHGroup)
                                {
                                    RangeHelper.MergeRanges(currentCellRange, MergeOption.Left);
                                }
                                else if ((k % DynamicColumn.GroupColumnCount) > 0)
                                {
                                    RangeHelper.MergeRanges(currentCellRange, MergeOption.Left);
                                }
                            }
                            objA = objB;
                            startColIndex++;
                        }
                        var num6 = DynamicColumn.StartCellIndex + DynamicColumn.InsertCount;
                        for (var m = num6; m < line.CellList.Count; m++)
                        {
                            var cell = line.CellList[m];
                            if (cell.MergeOption == MergeOption.Left)
                            {
                                RangeHelper.MergeRanges(
                                    RangeHelper.GetCell(TplRange.Worksheet, startColIndex, startRow), MergeOption.Left);
                            }
                            startColIndex++;
                        }
                    }
                }
            }
        }

        private void MergeVGroupCells()
        {
            foreach (var line in TplLineList)
            {
                var colIndex = line.StartColumnIndex;
                foreach (var cell in line.CellList)
                {
                    if ((cell.MergeOption != MergeOption.Up) ||
                        ((cell.GroupAlign != GroupAlign.None) && (cell.GroupAlign != GroupAlign.Always)))
                    {
                        colIndex++;
                    }
                    else
                    {
                        foreach (var startRow in line.InsertedRowList)
                        {
                            var currentCellRange = RangeHelper.GetCell(TplRange.Worksheet, colIndex, startRow);
                            if (!currentCellRange.HasMerged)
                            {
                                RangeHelper.MergeRanges(currentCellRange, MergeOption.Up);
                            }
                        }
                        colIndex++;
                    }
                }
            }
        }
    }
}