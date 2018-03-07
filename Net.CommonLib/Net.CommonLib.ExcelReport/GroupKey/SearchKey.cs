/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System.Collections.Generic;
using System.Data;

namespace Net.CommonLib.ExcelReport
{
    public class SearchKey : KeyValuePair
    {
        public bool IsFixedValue { get; set; }
        public ReusedKey ReusedKey { get; set; }
        public SearchKey NextKey { get; set; }

        public SearchKey Copy()
        {
            return Copy(true);
        }

        private SearchKey Copy(bool copyValue)
        {
            var key = new SearchKey
            {
                KeyName = KeyName,
                ReusedKey = ReusedKey
            };
            if (copyValue)
            {
                key.KeyValue = KeyValue;
                key.IsFixedValue = IsFixedValue;
            }
            if (NextKey != null)
            {
                key.NextKey = NextKey.Copy(copyValue);
            }
            return key;
        }

        public void FillKey(DataTable table, int rowIndex)
        {
            if (!IsFixedValue)
            {
                if (string.IsNullOrEmpty(KeyName))
                {
                    KeyValue = null;
                }
                if (ReusedKey == null)
                {
                    ReusedKey = FindReusedKey(KeyName);
                }
                KeyValue = ReusedKey.GetReusedValue(table, rowIndex);
            }
            if (NextKey != null)
            {
                NextKey.FillKey(table, rowIndex);
            }
        }

        public override bool Equals(object obj)
        {
            if (this != obj)
            {
                var key = obj as SearchKey;
                if (key == null)
                {
                    return false;
                }
                if (!base.Equals(obj))
                {
                    return false;
                }
                if (!Equals(NextKey, key.NextKey))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() + (29 * ((NextKey != null) ? NextKey.GetHashCode() : 0)));
        }

        public override string ToString()
        {
            return string.Concat("SearchKey: [", KeyName, ", ", KeyValue, ", ", IsFixedValue ? "T" : "F", "]next->", NextKey);
        }

        private static readonly List<ReusedKey> ReusedKeyPool = new List<ReusedKey>();

        public static string GetReusedKeyInfo()
        {
            return "ReusedKeyPool Size: " + ReusedKeyPool.Count;
        }

        public static void ClearKeyPool()
        {
            ReusedKeyPool.Clear();
        }

        public static ReusedKey FindReusedKey(string keyName)
        {
            foreach (var reusedKey in ReusedKeyPool)
            {
                if (Equals(reusedKey.KeyName, keyName))
                {
                    return reusedKey;
                }
            }
            var item = new ReusedKey
            {
                KeyName = keyName
            };
            ReusedKeyPool.Add(item);
            return item;
        }
    }
}