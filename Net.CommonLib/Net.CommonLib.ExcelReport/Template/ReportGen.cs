using Spire.Xls;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System.IO;

namespace Net.CommonLib.ExcelReport
{
    public class ReportGen
    {
        private readonly Dictionary<string, object> paramMap = new Dictionary<string, object>();

        public void AddParam(string name, object value)
        {
            paramMap[name] = value;
        }

        public void LoadStreamReport(string exportFilePath, Stream stream, DataSet dataSet, bool isExcel)
        {
            var book = new Workbook();
            book.LoadFromStream(stream);

            stream.Close();

            //Log.Instance.Reset();

            var bookTemplate = new ReportBookTemplate { DataSource = dataSet };
            bookTemplate.LoadTemplate(book, paramMap);
            bookTemplate.FillTemplate();

            book.SaveToFile(exportFilePath, isExcel ? FileFormat.Version2010 : FileFormat.PDF);
            book.Dispose();
            //GC.Collect();

            Process.Start(exportFilePath);
        }
    }
}