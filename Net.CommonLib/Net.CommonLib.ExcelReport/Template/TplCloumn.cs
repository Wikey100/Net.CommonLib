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
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Net.CommonLib.ExcelReport
{
    public class TplCloumn
    {
        public TplCloumn()
        {
            CellList = new List<TplCell>();
            GroupColList = new List<string>();
            GroupColumnCount = 1;
            LastValueMap = new List<object[]>();
            TplLastColumnIndex = 1;
        }

        public List<TplCell> CellList { get; set; }

        /// <summary>
        ///     动态分组列数量
        /// </summary>
        public int GroupColumnCount { get; set; }

        public List<string> GroupColList { get; set; }

        /// <summary>
        ///     已添加列数量
        /// </summary>
        public int InsertCount { get; set; }

        public List<object[]> LastValueMap { get; set; }

        /// <summary>
        ///     动态分组起始单元格索引(在Block中)
        /// </summary>
        public int StartCellIndex { get; set; }

        /// <summary>
        ///     起始列索引(在模版中)
        /// </summary>
        public int StartColIndex { get; set; }

        public ReportSheetTemplate Tpl { get; set; }

        /// <summary>
        ///     动态分组结束列索引
        /// </summary>
        public int TplLastColumnIndex { get; set; }

        public CellRange TplRange { get; set; }

        public int CheckColumn(TplBlock block, GroupDataHolder holder, int currentRowIndex, DataTable dataTable,
            int valueIndex)
        {
            if (IsNeedNewCol(dataTable, valueIndex) < 0)
            {
                return 0;
            }
            InsertColumn(block, holder, dataTable, valueIndex, true);
            return 1;
        }

        public void CheckEachColumn(TplBlock block, GroupDataHolder holder, int currentRowIndex, DataTable columnTalbe,
            int valueIndex)
        {
            for (var i = 0; i < GroupColumnCount; i++)
            {
                var flag = false;
                foreach (var line in block.TplLineList)
                {
                    var cell = line.CellList[StartCellIndex + i];
                    if (cell.GroupAlign == GroupAlign.Horizontal)
                    {
                        if (!flag)
                        {
                            if (cell.IsNeedNewCell(holder, cell.HgOption, 0, currentRowIndex, columnTalbe, valueIndex))
                            {
                                InsertOneColumn(block, i, holder, columnTalbe, valueIndex, false);
                                flag = true;
                            }
                        }
                        else if ((cell.HgOption & InsertOption.BeforeChange) == InsertOption.Never)
                        {
                            cell.LastGroupedValue = cell.GetGroupValue(holder, columnTalbe, valueIndex);
                        }
                    }
                }
            }
        }

        private static bool CompareArray(object[] object1, object[] object2)
        {
            if (object1.Length != object2.Length)
            {
                return false;
            }
            for (var i = 0; i < object1.Length; i++)
            {
                if (!Equals(object1[i], object2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public void InsertColumn(TplBlock block, GroupDataHolder holder, DataTable dataTable, int valueIndex,
            bool hasData)
        {
            if (InsertCount > 0)
            {
                if (hasData)
                {
                    var range = RangeHelper.GetRange(TplRange.Worksheet,
                        (StartColIndex + InsertCount) - GroupColumnCount, block.StartParseRowIndex,
                        GroupColumnCount, block.RowsCount);

                    //Console.WriteLine(string.Format(
                    //    "orign1:[StartColIndex-{4},InsertCount-{0},groupColumnCount-{1},StartRowIndex-{2},RowCount{3},tplLastColumnIndex{5}]",
                    //    InsertCount, groupColumnCount, block.StartRowIndex, block.RowCount, StartCellIndex, tplLastColumnIndex));

                    RangeHelper.InsertCopyRange(TplRange.Worksheet, range,
                        StartColIndex + InsertCount, block.StartParseRowIndex,
                        GroupColumnCount, block.RowsCount,
                        InsertRangeDirection.Right,
                        TplLastColumnIndex);
                }
                var orign = RangeHelper.GetRange(TplRange.Worksheet, (StartColIndex + InsertCount) - GroupColumnCount,
                    block.TplRange.Row, GroupColumnCount, block.TplRowCount);

                RangeHelper.InsertCopyRange(TplRange.Worksheet, orign,
                    StartColIndex + InsertCount, orign.Row,
                    GroupColumnCount, block.TplRowCount,
                    InsertRangeDirection.Right, TplLastColumnIndex);

                RefreshLineTplRanges(block, GroupColumnCount);
                block.TplColumCount += GroupColumnCount;
                block.ColumnsCount += GroupColumnCount;
            }
            for (var i = 0; i < block.TplLineList.Count; i++)
            {
                var line = block.TplLineList[i];
                for (var j = 0; j < GroupColumnCount; j++)
                {
                    var num3 = (StartCellIndex + ((InsertCount > 0) ? (InsertCount - GroupColumnCount) : 0)) + j;
                    var tplCell = line.CellList[num3];
                    if (InsertCount > 0)
                    {
                        tplCell = tplCell.Copy();
                        tplCell.LastColIndex += GroupColumnCount;
                        line.CellList.Insert(num3 + GroupColumnCount, tplCell);
                    }
                    if (tplCell.Formula != null)
                    {
                        foreach (var groupKey in tplCell.Formula.KeyList)
                        {
                            for (var searchKey = groupKey.SearchKey; searchKey != null; searchKey = searchKey.NextKey)
                            {
                                if (IsGroupedColumn(searchKey.KeyName))
                                {
                                    searchKey.KeyValue = RangeHelper.GetTableValue(dataTable, valueIndex,
                                        searchKey.KeyName);
                                    searchKey.IsFixedValue = true;
                                }
                            }
                            block.GroupKeyList.Add(groupKey.Copy());
                            if (groupKey.SearchKey != null)
                            {
                                groupKey.SearchKey.FillKey(dataTable, valueIndex);
                            }
                            block.Holder.AddValue(block.CountedMap, groupKey, dataTable, valueIndex);
                        }
                    }
                    else if (tplCell.HgOption != InsertOption.Never)
                    {
                        tplCell.TplTextContent = Convert.ToString(
                            RangeHelper.GetTableValue(dataTable, valueIndex, tplCell.TplValueColName));
                    }
                    tplCell.GroupAlign = GroupAlign.None;
                    Console.WriteLine(string.Concat("Inserted hg [", i.ToString().PadLeft(3), "][",
                        num3.ToString().PadLeft(3), "] = ", tplCell.Formula));
                    if (i < block.RowsCount)
                    {
                        var currentCellRange = RangeHelper.GetCell(TplRange.Worksheet, (StartColIndex + InsertCount) + j,
                            block.StartParseRowIndex + i);
                        tplCell.WriteCell(Tpl, holder, currentCellRange, dataTable, valueIndex);
                    }
                }
            }
            InsertCount += GroupColumnCount;
        }

        public void InsertOneColumn(TplBlock block, int colIndex, GroupDataHolder holder, DataTable columnTalbe,
            int valueIndex, bool hasData)
        {
            if (hasData)
            {
                var range = RangeHelper.GetRange(TplRange.Worksheet, StartColIndex + colIndex, block.StartParseRowIndex,
                    1,
                    block.RowsCount);
                RangeHelper.InsertCopyRange(TplRange.Worksheet, range,
                    (StartColIndex + GroupColumnCount) + InsertCount, block.StartParseRowIndex,
                    1, block.RowsCount,
                    InsertRangeDirection.Right, TplLastColumnIndex);
            }
            var orign = RangeHelper.GetRange(TplRange.Worksheet, StartColIndex + colIndex, block.TplRange.Row, 1,
                block.TplRowCount);

            RangeHelper.InsertCopyRange(TplRange.Worksheet, orign,
                (StartColIndex + GroupColumnCount) + InsertCount, orign.Row,
                1, block.TplRowCount,
                InsertRangeDirection.Right, TplLastColumnIndex);

            RefreshLineTplRanges(block, 1);
            block.TplColumCount++;
            block.ColumnsCount++;
            for (var i = 0; i < block.TplLineList.Count; i++)
            {
                var line = block.TplLineList[i];
                var num2 = StartCellIndex + colIndex;
                var tplCell = line.CellList[num2].Copy();
                tplCell.LastColIndex++;
                line.CellList.Insert((StartCellIndex + GroupColumnCount) + InsertCount, tplCell);
                if (tplCell.Formula != null)
                {
                    foreach (var groupKey in tplCell.Formula.KeyList)
                    {
                        if (groupKey.ReusedKey == null)
                        {
                            groupKey.ReusedKey = SearchKey.FindReusedKey(groupKey.ValueColName);
                        }
                        for (var key3 = groupKey.SearchKey; key3 != null; key3 = key3.NextKey)
                        {
                            if (IsGroupedColumn(key3.KeyName))
                            {
                                key3.KeyValue = RangeHelper.GetTableValue(columnTalbe, valueIndex, key3.KeyName);
                                key3.IsFixedValue = true;
                            }
                        }
                        block.GroupKeyList.Add(groupKey.Copy());
                        if (groupKey.SearchKey != null)
                        {
                            groupKey.SearchKey.FillKey(columnTalbe, valueIndex);
                        }
                        block.Holder.AddValue(block.CountedMap, groupKey, columnTalbe, valueIndex);
                    }
                }
                tplCell.GroupAlign = GroupAlign.None;
                Console.WriteLine(string.Concat("Inserted hg [", i.ToString().PadLeft(3), "][",
                    num2.ToString().PadLeft(3), "] = ", tplCell.Formula));
                if (i < block.RowsCount)
                {
                    var currentCellRange = RangeHelper.GetCell(TplRange.Worksheet,
                        (StartColIndex + GroupColumnCount) + InsertCount,
                        block.StartParseRowIndex + i);
                    tplCell.WriteCell(Tpl, holder, currentCellRange, columnTalbe, valueIndex);
                }
            }
            InsertCount++;
        }

        private bool IsGroupedColumn(string colName)
        {
            return
                CellList.Any(cell => (cell.GroupAlign == GroupAlign.Horizontal) && (cell.TplGroupColumnName == colName));
        }

        private int IsNeedNewCol(DataTable table, int valueIndex)
        {
            var objArray = new object[GroupColList.Count];
            for (var i = 0; i < GroupColList.Count; i++)
            {
                var colName = GroupColList[i];
                objArray[i] = RangeHelper.GetTableValue(table, valueIndex, colName);
            }
            if (LastValueMap.Any(objArray2 => CompareArray(objArray, objArray2)))
            {
                return -1;
            }
            LastValueMap.Add(objArray);
            return 0;
        }

        private void RefreshLineTplRanges(TplBlock block, int colCount)
        {
            foreach (var line in block.TplLineList)
            {
                line.TplRange = RangeHelper.GetRange(line.TplRange.Worksheet,
                    line.TplRange.Column, line.TplRange.Row,
                    line.TplCellCount + InsertCount, colCount);
                line.TplCellCount += colCount;
            }
        }
    }
}