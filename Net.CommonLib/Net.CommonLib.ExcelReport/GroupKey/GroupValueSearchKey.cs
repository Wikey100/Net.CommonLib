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
    public class GroupValueSearchKey
    {
        public string Formula { get; set; }
        public SearchKey SearchKey { get; set; }
        public ReusedKey ReusedKey { get; set; }
        public string ValueColName { get; set; }

        public GroupValueSearchKey Copy()
        {
            var copyKey = new GroupValueSearchKey();
            if (SearchKey != null)
            {
                copyKey.SearchKey = SearchKey.Copy();
            }
            copyKey.Formula = Formula;
            copyKey.ValueColName = ValueColName;
            copyKey.ReusedKey = ReusedKey;
            return copyKey;
        }

        public override bool Equals(object obj)
        {
            if (this != obj)
            {
                var compareKey = obj as GroupValueSearchKey;
                if (compareKey == null)
                {
                    return false;
                }
                if (!Equals(SearchKey, compareKey.SearchKey))
                {
                    return false;
                }
                if (!Equals(ValueColName, compareKey.ValueColName))
                {
                    return false;
                }
                if (!Equals(Formula, compareKey.Formula))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            var num = (SearchKey != null) ? SearchKey.GetHashCode() : 0;
            num = 29 * num + ((ValueColName != null) ? ValueColName.GetHashCode() : 0);
            return 29 * num + ((Formula != null) ? Formula.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return string.Concat("GroupValueSearchKey [", Formula.PadLeft(3), ", ", ValueColName.PadLeft(16), "]", SearchKey);
        }
    }
}