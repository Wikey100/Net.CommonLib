/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System.Data;

namespace Net.CommonLib.ExcelReport
{
    public class ReusedKey : KeyValuePair
    {
        private int lastUsedColIndex = -1;
        private int lastUsedRowIndex = -1;
        private DataTable lastUsedTable;
        private object lastValue;

        public object GetReusedValue(DataTable table, int rowIndex)
        {
            if (lastUsedTable != table)
            {
                //重置
                lastUsedTable = table;
                lastUsedColIndex = table.Columns.IndexOf(KeyName);
                lastUsedRowIndex = -1;
                lastValue = null;
            }
            else if (lastUsedRowIndex == rowIndex)
            {
                return lastValue;
            }
            lastUsedRowIndex = rowIndex;
            lastValue = lastUsedColIndex < 0 ? null : table.Rows[rowIndex][lastUsedColIndex];
            return lastValue;
        }
    }
}