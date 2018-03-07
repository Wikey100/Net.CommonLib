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
    public class ReportBookTemplate
    {
        private readonly List<ReportSheetTemplate> sheetList = new List<ReportSheetTemplate>();

        public DataSet DataSource { get; set; }

        public void LoadTemplate(Workbook book, Dictionary<string, object> paramMap)
        {
            for (var i = 0; i < book.Worksheets.Count; i++)
            {
                var sheet = book.Worksheets[i];
                var template = TplLoader.ParseSheetTemplate(sheet);
                template.DataSource = DataSource;
                template.ParamMap = paramMap;
                sheetList.Add(template);
            }
        }

        public void FillTemplate()
        {
            var now = DateTime.Now;
            foreach (var template in sheetList)
            {
                template.FillTemplate();
            }

            foreach (var template in sheetList)
            {
                template.Clear();
            }

            SearchKey.ClearKeyPool();

            #region Log

            foreach (var template in sheetList)
            {
                foreach (var block in template.BlockList)
                {
                    Console.WriteLine(string.Concat("Block: ", block.Name, " gKeyList: ", block.GroupKeyList.Count,
                        " ValueList: ", block.Holder.GroupValueDict.Count));

                    foreach (var pair in block.Holder.GroupValueDict)
                    {
                        Console.WriteLine("VV--" + pair.Key + "--" + pair.Value);
                    }
                }
            }
            Console.WriteLine("GetColVlaue Call Times: " + RangeHelper.GetColValueCount);
            Console.WriteLine("GetColVlaueByIndex Call Times: " + RangeHelper.GetColValueByIndexCount);

            Console.WriteLine(SearchKey.GetReusedKeyInfo());

            Console.WriteLine("GetRange Call Times: " + RangeHelper.GetRangeCallTimes);
            Console.WriteLine("UpdateCellValue Call Times: " + RangeHelper.GetRangeCallTimes);
            Console.WriteLine("insertCopyRange Call Times: " + RangeHelper.InsertCopyRangeCallTimes);
            Console.WriteLine("Total time: " + DateTime.Now.Subtract(now).TotalMilliseconds + "ms.");

            #endregion Log
        }
    }
}