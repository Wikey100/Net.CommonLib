/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Net.CommonLib.ExcelReport
{
    public class CellForumla
    {
        public string Formula { get; set; }

        public List<GroupValueSearchKey> KeyList { get; set; }

        public CellForumla()
        {
            Formula = string.Empty;
            KeyList = new List<GroupValueSearchKey>();
        }

        public CellForumla Copy()
        {
            var forumla = new CellForumla
            {
                Formula = Formula
            };

            foreach (var key in KeyList)
            {
                forumla.KeyList.Add(key.Copy());
            }

            return forumla;
        }

        public object GetValue(GroupDataHolder holder, DataTable table, int rowIndex)
        {
            object lastValue = 0;
            foreach (var groupKey in KeyList)
            {
                if (groupKey.SearchKey != null)
                {
                    groupKey.SearchKey.FillKey(table, rowIndex);
                }
                var groupValue = holder.GetValue(groupKey);
                lastValue = Calculate(Formula, lastValue, groupValue);
            }
            return lastValue;
        }

        public static object Calculate(string formula, object lastValue, object value)
        {
            switch (formula)
            {
                case TemplateFlags.ForumlaNone:
                    return value;

                case TemplateFlags.ForumlaSum:
                    lastValue = ToNumber(lastValue) + ToNumber(value);
                    return lastValue;

                case TemplateFlags.ForumlaCount:
                    lastValue = ToNumber(lastValue) + 1.0;
                    return lastValue;
            }
            throw new ArgumentOutOfRangeException("formula", formula,
                "Formula [" + formula + "] is not supported.");
        }

        private static double ToNumber(object value)
        {
            if (value == null)
            {
                return 0.0;
            }
            double result;
            double.TryParse(value.ToString(), out result);
            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(string.Concat("CellForumla [", Formula, "] C:", KeyList.Count, ""));
            foreach (var key in KeyList)
            {
                builder.Append("  ");
                builder.Append(key);
            }
            return builder.ToString();
        }
    }
}