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

namespace Net.CommonLib.ExcelReport
{
    public class TplLoader
    {
        public static Dictionary<string, string> ParseKeyValuePair(string text)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var str in text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var strArray2 = str.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                var str2 = "";
                if (strArray2.Length > 1)
                {
                    str2 = strArray2[1];
                }
                dictionary.Add(strArray2[0].Trim().ToLower(), str2);
            }
            return dictionary;
        }

        private static MergeOption ParseMergeOption(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                switch (text.ToLower())
                {
                    case TemplateFlags.MergeUp:
                        return MergeOption.Up;

                    case TemplateFlags.MergeLeft:
                        return MergeOption.Left;
                }
            }
            return MergeOption.Never;
        }

        public static ReportSheetTemplate ParseSheetTemplate(Worksheet sheet)
        {
            var template = new ReportSheetTemplate
            {
                Sheet = sheet
            };
            var sheetIndex = sheet.Index + 1;
            for (var i = 1; i < TemplateFlags.IndexTemplateEndRow; i++)
            {
                var str = (string)RangeHelper.GetRange(sheet, 1, i, 1, 1).Value2;
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.Equals(TemplateFlags.EmptyFields, StringComparison.CurrentCultureIgnoreCase))
                    {
                        var emptyFieldStr = RangeHelper.GetRange(sheet, 2, i, 1, 1).Value2 as string;
                        if (!string.IsNullOrEmpty(emptyFieldStr))
                        {
                            template.EmptyFieldsDict = ParseKeyValuePair(emptyFieldStr);
                        }
                    }
                    else
                    {
                        var dict = ParseKeyValuePair(str);

                        template.AutoFit = dict.ContainsKey(TemplateFlags.BlockAutoFit) &&
                                           dict[TemplateFlags.BlockAutoFit] == "true";

                        var namePrefix = string.Concat("S", sheetIndex, ".");

                        //Todo:JoinAt 动态列合并
                        int joinat;

                        var block = new TplBlock
                        {
                            StartParseColumnIndex = 2,
                            StartParseRowIndex = i,
                            ColumnsCount = int.Parse(dict[TemplateFlags.BlockColumnCount]),
                            TplColumCount = int.Parse(dict[TemplateFlags.BlockColumnCount]),
                            TplRowCount = int.Parse(dict[TemplateFlags.BlockRowCount]),
                            Name = dict.ContainsKey(TemplateFlags.BlockName)
                                ? dict[TemplateFlags.BlockName]
                                : string.Concat(namePrefix + "block", template.BlockList.Count + 1),
                            DataTableIndex = dict.ContainsKey(TemplateFlags.BlockTable)
                                ? int.Parse(dict[TemplateFlags.BlockTable])
                                : -1,
                            TplColumnTableIndex = dict.ContainsKey(TemplateFlags.BlockColumnTalbe)
                                ? int.Parse(dict[TemplateFlags.BlockColumnTalbe])
                                : -1,
                            CopyOnly = dict.ContainsKey(TemplateFlags.BlockCopyOnly) &&
                                       dict[TemplateFlags.BlockCopyOnly] == "true",
                            UpdateAllRow = dict.ContainsKey(TemplateFlags.BlockUpdateAllRow) &&
                                           dict[TemplateFlags.BlockUpdateAllRow] == "true",
                            Joinat = dict.ContainsKey(TemplateFlags.BlockJoinAt) &&
                                     int.TryParse(dict[TemplateFlags.BlockJoinAt], out joinat)
                                ? joinat
                                : -1
                        };

                        block.TplRange = RangeHelper.GetRange(sheet, block.StartParseColumnIndex, block.StartParseRowIndex,
                            block.ColumnsCount, block.TplRowCount);

                        if (block.CopyOnly)
                        {
                            template.BlockList.Add(block);
                        }
                        else
                        {
                            for (var j = 0; j < block.TplRowCount; j++)
                            {
                                var startColumn = TemplateFlags.IndexLineContendStartColumn;
                                var line = ParseLine(sheet, block, startColumn, j + block.StartParseRowIndex);

                                line.SheetTemplate = template;
                                line.StartColumnIndex = startColumn;
                                line.InsertOption = GetLineInsertOption(
                                    RangeHelper.GetCell(sheet, TemplateFlags.IndexLineInsertOptionColumn,
                                        j + block.StartParseRowIndex).Value2 as string);
                                line.TplCellCount = block.ColumnsCount;
                                block.TplLineList.Add(line);
                            }

                            block.InitDynamicColumn(template);

                            template.BlockList.Add(block);
                        }
                    }
                }
            }
            return template;
        }

        private static TplLine ParseLine(Worksheet sheet, TplBlock block, int startCol, int startRow)
        {
            var colCount = block.ColumnsCount;
            var line = new TplLine
            {
                TplRange = RangeHelper.GetRange(sheet, startCol, startRow, colCount, 1)
            };
            for (var i = 0; i < colCount; i++)
            {
                var range = RangeHelper.GetCell(sheet, startCol + i, startRow);
                var cell = new TplCell
                {
                    TplRange = range,
                    LastColIndex = i + startCol
                };
                var str = range.Value2 as string;
                if (!string.IsNullOrEmpty(str))
                {
                    ParseCell(block, line, cell, str.Trim());
                }
                line.CellList.Add(cell);
            }
            return line;
        }

        private static void ParseCell(TplBlock block, TplLine line, TplCell cell, string text)
        {
            text = text.Trim();
            if (text.StartsWith("R1C1:"))
            {
                cell.UseR1C1Formula = true;
                cell.TplTextContent = text.Substring(5);
            }
            else if (text[0] != TemplateFlags.CellStart)
            {
                cell.TplTextContent = text;
            }
            else
            {
                var index = text.IndexOf(TemplateFlags.CellEnd);
                if (index > 0)
                {
                    if ((index + 1) != text.Length)
                    {
                        cell.TplTextContent = text.Substring(index + 1);
                    }
                    text = text.Substring(1, index - 1);
                }
                cell.TplValueColName = text;
                var pair = ParseKeyValuePair(text);
                cell.TplFormat = GetPairValue(pair, TemplateFlags.CellFormat);
                if (!string.IsNullOrEmpty(cell.TplFormat))
                {
                    cell.TplFormat = cell.TplFormat.ToLower();
                }
                cell.MergeOption = ParseMergeOption(GetPairValue(pair, TemplateFlags.CellMergeOption));

                #region CellGroupAlign

                if (GetPairValue(pair, TemplateFlags.CellVGroup) != null)
                {
                    cell.GroupAlign = GroupAlign.Vertical;
                    cell.TplGroupColumnName = GetPairValue(pair, TemplateFlags.CellVGroup).Trim().ToUpper();

                    Console.WriteLine(
                        string.Format("Contains Vg GroupColName:{0}", cell.TplGroupColumnName));
                }
                else if (GetPairValue(pair, TemplateFlags.CellHGroup) != null)
                {
                    cell.GroupAlign = GroupAlign.Horizontal;
                    cell.TplGroupColumnName = GetPairValue(pair, TemplateFlags.CellHGroup).ToUpper();
                    line.ContainsHGroup = true;
                    var insertOption = GetLineInsertOption(GetPairValue(pair, TemplateFlags.CellHGroupOption));
                    if ((insertOption != InsertOption.AfterChange) &&
                        (insertOption != InsertOption.BeforeChange))
                    {
                        insertOption = InsertOption.AfterChange;
                    }
                    cell.HgOption = insertOption;

                    Console.WriteLine(
                        string.Format("Contains Hg GroupColName:{0},LineInsertOption:{1}", cell.TplGroupColumnName,
                            insertOption));
                }

                #endregion CellGroupAlign

                var pairValue = GetPairValue(pair, TemplateFlags.CellValue);
                if (string.IsNullOrEmpty(pairValue))
                {
                    if (!string.IsNullOrEmpty(cell.TplGroupColumnName))
                    {
                        //无值(v)标记时以分组列字段绑定
                        cell.TplValueColName = cell.TplGroupColumnName;
                    }
                }
                else
                {
                    cell.TplDefaultContent = GetPairValue(pair, TemplateFlags.CellDefaultContent);

                    if (ParseCellFormula(cell, ref pairValue))
                    {
                        var groupKey = ParseCellFormulaGroupKey(block, line, cell.Formula.Formula, pairValue);

                        Console.WriteLine(groupKey.ToString());

                        cell.Formula.KeyList.Add(groupKey);
                        block.GroupKeyList.Add(groupKey.Copy());
                    }
                }
            }
        }

        /// <summary>
        /// 解析单元格公式
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="pairValue"></param>
        /// <returns>是否包含公式</returns>
        private static bool ParseCellFormula(TplCell cell, ref string pairValue)
        {
            var formulaStartIndex = pairValue.IndexOf(TemplateFlags.CellValueFormulaStart);
            if (formulaStartIndex < 0)
            {
                cell.TplValueColName = pairValue.Trim().ToUpper();
                return false;
            }
            cell.Formula = new CellForumla
            {
                //分隔符(前作为公式名
                Formula = (formulaStartIndex == 0) ? "" : pairValue.Substring(0, formulaStartIndex).ToLower()
            };

            //截取()内值
            var formulaEndIndex = pairValue.IndexOf(TemplateFlags.CellValueFormulaEnd);
            if (formulaEndIndex < 0)
            {
                Console.WriteLine("Warning:: formula not closed. [" + pairValue + "]");
                pairValue = pairValue.Substring(formulaStartIndex + 1);
            }
            else
            {
                pairValue = pairValue.Substring(formulaStartIndex + 1, (formulaEndIndex - formulaStartIndex) - 1);
            }
            return true;
        }

        private static GroupValueSearchKey ParseCellFormulaGroupKey(TplBlock block, TplLine line, string formulaName,
            string pairValue)
        {
            var groupKey = new GroupValueSearchKey
            {
                Formula = formulaName
            };

            var valueKeyArray = pairValue.Trim()
                .Split(new[] { TemplateFlags.CellValueKeySpliter }, StringSplitOptions.RemoveEmptyEntries);

            SearchKey nextKey = null;

            for (var i = 0; i < valueKeyArray.Length; i++)
            {
                //GroupKey筛选固定值时
                var valueFixedArray = valueKeyArray[i].Split(new[] { TemplateFlags.CellValueFixedSpliter },
                    StringSplitOptions.RemoveEmptyEntries);

                if (i == 0)
                {
                    //第一项Key作为绑定关联列名
                    groupKey.ValueColName = valueFixedArray[0].Trim().ToUpper();
                }
                else
                {
                    var searchKey = new SearchKey
                    {
                        KeyName = valueFixedArray[0].Trim().ToUpper()
                    };
                    if (valueFixedArray.Length > 1)
                    {
                        //包含=号赋固定值
                        searchKey.IsFixedValue = true;
                        searchKey.KeyValue = valueFixedArray[1];
                    }
                    if (searchKey.KeyName == TemplateFlags.CellGroupKeySearchPattern)
                    {
                        //从之前添加的分组cells中查找
                        searchKey = GetGroupedSearchKey(block, line);
                        if (searchKey == null)
                        {
                            continue;
                        }
                    }
                    if (nextKey == null)
                    {
                        groupKey.SearchKey = searchKey;
                        nextKey = searchKey;
                    }
                    else
                    {
                        nextKey.NextKey = searchKey;
                        nextKey = searchKey;
                    }

                    //nextKey置尾,后续值追加到searchKey尾部
                    while (nextKey.NextKey != null)
                    {
                        nextKey = nextKey.NextKey;
                    }
                }
            }

            return groupKey;
        }

        private static SearchKey GetGroupedSearchKey(TplBlock block, TplLine line)
        {
            SearchKey searchKey = null;
            SearchKey nextKey = null;

            //水平分组查找同行
            foreach (var cell in line.CellList)
            {
                if (cell.GroupAlign == GroupAlign.Vertical)
                {
                    var cellVGroupKey = new SearchKey { KeyName = cell.TplGroupColumnName };

                    if (searchKey == null)
                    {
                        searchKey = cellVGroupKey;
                    }

                    if (nextKey == null)
                    {
                        nextKey = cellVGroupKey;
                    }
                    else
                    {
                        //递进赋值NextKey
                        nextKey.NextKey = cellVGroupKey;
                        nextKey = cellVGroupKey;
                    }
                }
            }
            //垂直分组遍历Block行查找同列
            foreach (var vLine in block.TplLineList)
            {
                if (vLine.CellList.Count > line.CellList.Count)
                {
                    var cell = vLine.CellList[line.CellList.Count];
                    if (cell.GroupAlign == GroupAlign.Horizontal)
                    {
                        var cellHGroupKey = new SearchKey { KeyName = cell.TplGroupColumnName };
                        if (searchKey == null)
                        {
                            searchKey = cellHGroupKey;
                        }
                        if (nextKey == null)
                        {
                            nextKey = cellHGroupKey;
                        }
                        else
                        {
                            nextKey.NextKey = cellHGroupKey;
                            nextKey = cellHGroupKey;
                        }
                    }
                }
            }
            return searchKey;
        }

        /// <summary>
        /// 解析行添加方式,默认OnFirst
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private static InsertOption GetLineInsertOption(string option)
        {
            if (!string.IsNullOrEmpty(option))
            {
                option = option.ToLower();
                switch (option)
                {
                    case TemplateFlags.InsertOnLast:
                        return InsertOption.OnLast;

                    case TemplateFlags.InsertOnBefore:
                        return InsertOption.BeforeChange;

                    case TemplateFlags.InsertOnAfter:
                        return InsertOption.AfterChange;

                    case TemplateFlags.InsertNever:
                        return InsertOption.Never;

                    case TemplateFlags.InsertAll:
                        return InsertOption.Always;
                }
            }
            return InsertOption.OnFirst;
        }

        private static string GetPairValue(Dictionary<string, string> pair, string key)
        {
            string str;
            if (pair.TryGetValue(key, out str))
            {
                return str;
            }
            return null;
        }
    }
}