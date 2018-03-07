/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.ExcelReport
{
    public class KeyValuePair
    {
        public string KeyName { get; set; }
        public object KeyValue { get; set; }

        public override bool Equals(object obj)
        {
            if (this != obj)
            {
                var pair = obj as KeyValuePair;
                if (pair == null)
                {
                    return false;
                }
                if (!Equals(KeyName, pair.KeyName))
                {
                    return false;
                }
                if (!Equals(KeyValue, pair.KeyValue))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return (((KeyName != null) ? KeyName.GetHashCode() : 0) +
                    (29 * ((KeyValue != null) ? KeyValue.GetHashCode() : 0)));
        }
    }
}