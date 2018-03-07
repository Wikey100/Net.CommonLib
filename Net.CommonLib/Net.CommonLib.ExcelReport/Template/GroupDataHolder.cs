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
    public class GroupDataHolder
    {
        public Dictionary<GroupValueSearchKey, object> GroupValueDict { get; set; }

        public GroupDataHolder()
        {
            GroupValueDict = new Dictionary<GroupValueSearchKey, object>(100000);
        }

        public void AddValue(Dictionary<GroupValueSearchKey, bool> map, GroupValueSearchKey groupKey, DataTable table,
            int valueIndex)
        {
            if (!map.ContainsKey(groupKey))
            {
                object groupValue;
                if (groupKey.SearchKey != null)
                {
                    for (var searchKey = groupKey.SearchKey; searchKey != null; searchKey = searchKey.NextKey)
                    {
                        if (searchKey.ReusedKey == null)
                        {
                            searchKey.ReusedKey = SearchKey.FindReusedKey(searchKey.KeyName);
                        }
                        if (searchKey.IsFixedValue)
                        {
                            if (!Equals(searchKey.ReusedKey.GetReusedValue(table, valueIndex), searchKey.KeyValue))
                            {
                                return;
                            }
                        }
                    }
                }
                if (!GroupValueDict.TryGetValue(groupKey, out groupValue))
                {
                    groupValue = 0;
                    GroupValueDict.Add(groupKey, groupValue);
                }
                if (groupKey.ReusedKey == null)
                {
                    groupKey.ReusedKey = SearchKey.FindReusedKey(groupKey.ValueColName);
                }
                GroupValueDict[groupKey] = CalculateForumla(groupKey.Formula, groupValue,
                    groupKey.ReusedKey.GetReusedValue(table, valueIndex));
                map[groupKey] = true;
            }
        }

        public object CalculateForumla(string formulaName, object lastValue, object value)
        {
            return CellForumla.Calculate(formulaName.ToLower(), lastValue, value);
        }

        public object GetValue(GroupValueSearchKey groupKey, object defaultValue = null)
        {
            object groupValue;
            return GroupValueDict.TryGetValue(groupKey, out groupValue) ? groupValue : defaultValue;
        }
    }
}