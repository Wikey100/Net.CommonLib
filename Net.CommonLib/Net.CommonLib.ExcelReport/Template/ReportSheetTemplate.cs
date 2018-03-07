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

namespace Net.CommonLib.ExcelReport
{
    public class ReportSheetTemplate
    {
        public ReportSheetTemplate()
        {
            BlockList = new List<TplBlock>();
            ParamMap = new Dictionary<string, object>();
        }

        /// <summary>
        ///     已写行数
        /// </summary>
        private int AlreadyWriteRows { get; set; }

        /// <summary>
        ///     自适应列宽
        /// </summary>
        public bool AutoFit { get; set; }

        public List<TplBlock> BlockList { get; set; }
        public DataSet DataSource { get; set; }
        public Dictionary<string, object> ParamMap { get; set; }
        public Worksheet Sheet { get; set; }
        public Dictionary<string, string> EmptyFieldsDict { get; set; }

        /// <summary>
        ///     删除模版数据
        /// </summary>
        public void Clear()
        {
            Sheet.DeleteColumn(1);
            Sheet.DeleteColumn(1);
            Sheet.DeleteRow(1, TemplateFlags.IndexTemplateEndRow);

            if (AutoFit)
            {
                var lastrow = Sheet.LastRow;
                var lastCol = Sheet.LastColumn;
                Sheet.Range[1, 1, lastrow, lastCol].AutoFitColumns();
            }
        }

        public void FillTemplate()
        {
            foreach (var block in BlockList)
            {
                if (block.CopyOnly)
                {
                    #region Copy 传入参数部分

                    var rangeCells = RangeHelper.GetRangeCells(
                        RangeHelper.InsertCopyRange(Sheet, block.TplRange,
                            block.StartParseColumnIndex, TemplateFlags.IndexTemplateEndRow + AlreadyWriteRows,
                            block.TplColumCount, block.TplRowCount,
                            InsertRangeDirection.Down));

                    while (rangeCells.MoveNext())
                    {
                        var currentCell = (CellRange)rangeCells.Current;
                        if (!currentCell.HasMerged)
                        {
                            var cellValue = currentCell.Value2 as string;
                            if (((cellValue != null) && cellValue.StartsWith("#")) &&
                                ((cellValue.Length > 1) && (ParamMap != null)))
                            {
                                var strArray = cellValue.Substring(1)
                                    .Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                object paramValue;
                                ParamMap.TryGetValue(strArray[0], out paramValue);
                                var format = "";
                                if (strArray.Length > 1)
                                {
                                    format = strArray[1].ToLower();
                                }
                                RangeHelper.UpdateCellValue(this, currentCell, paramValue, format);
                            }

                            //模版单元格为合并单元格时进行合并
                            var templateCell = RangeHelper.GetRange(Sheet, currentCell.Column,
                                ((currentCell.Row - TemplateFlags.IndexTemplateEndRow) - AlreadyWriteRows) +
                                block.StartParseRowIndex, 1, 1);
                            if (templateCell.HasMerged)
                            {
                                var startColumn = templateCell.MergeArea.Column;
                                var startRow = ((templateCell.MergeArea.Row + TemplateFlags.IndexTemplateEndRow) +
                                                AlreadyWriteRows) -
                                               block.StartParseRowIndex;

                                var mergeColumnCount = templateCell.MergeArea.ColumnCount;
                                var mergeRowCount = templateCell.MergeArea.RowCount;

                                var needMergeArea =
                                    RangeHelper.GetRange(Sheet, startColumn, startRow, mergeColumnCount, mergeRowCount);

                                if (!needMergeArea.HasMerged)
                                {
                                    needMergeArea.Merge();
                                }
                            }
                        }
                    }
                    AlreadyWriteRows += block.TplRowCount;

                    #endregion Copy 传入参数部分
                }
                else
                {
                    block.StartParseRowIndex = TemplateFlags.IndexTemplateEndRow + AlreadyWriteRows;

                    if (block.TplColumnTableIndex >= 0 && DataSource.Tables.Count > block.TplColumnTableIndex)
                    {
                        //动态填充模版列
                        block.CreateDynamicColumn(DataSource.Tables[block.TplColumnTableIndex]);
                    }
                    if (block.DataTableIndex < 0 || DataSource.Tables.Count <= block.DataTableIndex)
                    {
                        throw new ArgumentException(string.Format(
                            "DataTable [{0}] of Block [{1}] not found in DataSet!", block.DataTableIndex, block.Name));
                    }

                    if ((DataSource.Tables[block.DataTableIndex].Rows.Count <= 0))
                    {
                        var emptyTable = new DataTable();

                        #region 数据源无数据时填充EmptyTable

                        var stringType = Type.GetType("System.String");
                        if (stringType != null)
                        {
                            if (EmptyFieldsDict != null && EmptyFieldsDict.Count > 0)
                            {
                                foreach (var key in EmptyFieldsDict.Keys)
                                {
                                    emptyTable.Columns.Add(new DataColumn(key, stringType));
                                }

                                var row = emptyTable.NewRow();
                                foreach (DataColumn column in emptyTable.Columns)
                                {
                                    var field = EmptyFieldsDict[column.ColumnName];
                                    if (field.StartsWith("#") && ParamMap != null)
                                    {
                                        object paramValue;
                                        if (ParamMap.TryGetValue(field.Substring(1), out paramValue))
                                        {
                                            row[column.ColumnName] = paramValue.ToString();
                                        }
                                        else
                                        {
                                            row[column.ColumnName] = field;
                                        }
                                    }
                                    else
                                    {
                                        row[column.ColumnName] = field;
                                    }
                                }
                                emptyTable.Rows.Add(row);
                            }
                            else
                            {
                                emptyTable.Columns.Add(new DataColumn("column1", stringType));
                                emptyTable.Columns.Add(new DataColumn("column2", stringType));
                                emptyTable.Rows.Add("0", "0");
                            }
                        }

                        #endregion 数据源无数据时填充EmptyTable

                        AlreadyWriteRows += block.FillBlock(emptyTable);
                    }
                    else
                    {
                        AlreadyWriteRows += block.FillBlock(DataSource.Tables[block.DataTableIndex]);
                    }
                }
            }

            JoinTable();
        }

        private void JoinTable()
        {
            if (BlockList.Count >= 2)
            {
                TplBlock block = BlockList[1]; //要合并的第一个Block
                int startRowIndex = block.StartParseRowIndex;
                int startColumn = block.StartParseColumnIndex + block.ColumnsCount;
                int num3 = 0;

                for (int i = 2; i < BlockList.Count; i++) //以第一个block为基准,合并其他指定joinat的block
                {
                    TplBlock block2 = BlockList[i];
                    if ((block2.Joinat >= 0) && (block2.RowsCount > 0))
                    {
                        //获取joinat之后的区域
                        CellRange range = RangeHelper.GetRange(Sheet,
                            block2.StartParseColumnIndex + block2.Joinat + 1,
                            block2.StartParseRowIndex - num3,
                            block2.ColumnsCount,
                            block2.RowsCount);

                        CellRange destRange = RangeHelper.GetRange(Sheet, startColumn + 1, block.StartParseRowIndex,
                            block2.ColumnsCount, block2.RowsCount);

                        range.Copy(destRange);//block2区域复制到block1右边

                        Sheet.DeleteRow(range.Row, range.RowCount);

                        num3 += block2.RowsCount;

                        if ((block2.DynamicColumn != null) && (block2.DynamicColumn.StartCellIndex == block2.Joinat))
                        {
                            foreach (var line in block2.TplLineList)
                            {
                                if (line.ContainsHGroup) //block2中包含横向分组的行hg
                                {
                                    bool flag = false;
                                    foreach (var rowIndex in line.InsertedRowList)
                                    {
                                        int startRow = (rowIndex - block2.StartParseRowIndex) + startRowIndex;
                                        //动态添加移动的起始行
                                        CellRange range2 = RangeHelper.GetRange(Sheet, startColumn, startRow, 1, 1);
                                        //相同行区域的block1最后单元格?
                                        if (range2.MergeArea != null)
                                        {
                                            range2 = RangeHelper.GetRange(Sheet, range2.MergeArea.Column,
                                                //?range2合并区域的起始单元格
                                                range2.MergeArea.Row, 1, 1);
                                        }

                                        CellRange range3 = RangeHelper.GetRange(Sheet, startColumn + 1, startRow, 1, 1);
                                        //block2的第一个单元格
                                        if (range3.MergeArea != null)
                                        {
                                            range3 = RangeHelper.GetRange(Sheet, range3.MergeArea.Column, //第一个单元格的起始区域
                                                range3.MergeArea.Row, 1, 1);
                                        }

                                        if (range2.Text.Equals(range3.Text))
                                        {
                                            RangeHelper.GetRange(Sheet, range2.Column, range2.Row,
                                                (range3.Column + range3.ColumnCount) - range2.Column,
                                                Math.Min(range3.RowCount, range2.RowCount)).Merge(true);
                                            flag = true;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        startColumn += block2.ColumnsCount - block2.Joinat; //合并后起始列更新
                    }
                }
            }
        }
    }
}