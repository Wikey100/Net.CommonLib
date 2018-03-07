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
    /// <summary>
    ///     模版解析标志
    /// </summary>
    public class TemplateFlags
    {
        #region 解析索引固定值

        /// <summary>
        ///     模版结束行索引值
        /// </summary>
        public const int IndexTemplateEndRow = 150;

        /// <summary>
        ///     模版行开始解析列索引 3
        /// </summary>
        public const int IndexLineContendStartColumn = 3;

        /// <summary>
        ///     模版行添加方式解析列索引 2
        /// </summary>
        public const int IndexLineInsertOptionColumn = 2;

        #endregion 解析索引固定值

        #region 模版块block解析

        /// <summary>
        ///     块包含列数 cols
        /// </summary>
        public const string BlockColumnCount = "cols";

        /// <summary>
        ///     块包含行数 rows
        /// </summary>
        public const string BlockRowCount = "rows";

        /// <summary>
        ///     块名 name
        /// </summary>
        public const string BlockName = "name";

        /// <summary>
        ///     块只复制标志 copy
        /// </summary>
        public const string BlockCopyOnly = "copy";

        /// <summary>
        ///     块更新所有行标志 updateallrow
        /// </summary>
        public const string BlockUpdateAllRow = "updateallrow";

        /// <summary>
        ///     块自适应宽高标志 autofit
        /// </summary>
        public const string BlockAutoFit = "autofit";

        /// <summary>
        ///     块合并标志 joinat
        /// </summary>
        public const string BlockJoinAt = "joinat";

        /// <summary>
        ///     块数据源 table
        /// </summary>
        public const string BlockTable = "table";

        /// <summary>
        ///     块空数据源绑定字段(当无数据源时绑定)
        /// </summary>
        public const string EmptyFields = "empty";

        /// <summary>
        ///     块动态列数据源 coltable
        /// </summary>
        public const string BlockColumnTalbe = "coltable";

        #endregion 模版块block解析

        #region 模版单元格Cell解析

        /// <summary>
        ///     单元格解析起始符 {
        /// </summary>
        public const char CellStart = '{';

        /// <summary>
        ///     单元格解析结束符 }
        /// </summary>
        public const char CellEnd = '}';

        /// <summary>
        ///     单元格值格式标志 f
        /// </summary>
        public const string CellFormat = "f";

        /// <summary>
        ///     单元格合并方式 m
        /// </summary>
        public const string CellMergeOption = "m";

        /// <summary>
        ///     单元格垂直分组标志 vg
        /// </summary>
        public const string CellVGroup = "vg";

        /// <summary>
        ///     单元格横向分组标志 hg
        /// </summary>
        public const string CellHGroup = "hg";

        /// <summary>
        ///     单元格横向分组添加方式 hgo
        /// </summary>
        public const string CellHGroupOption = "hgo";

        /// <summary>
        ///     单元格内容标志 v
        /// </summary>
        public const string CellValue = "v";

        /// <summary>
        ///     单元格默认值标志 default
        /// </summary>
        public const string CellDefaultContent = "default";

        /// <summary>
        ///     单元格公式起始符 (
        /// </summary>
        public const char CellValueFormulaStart = '(';

        /// <summary>
        ///     单元格公式结束符 )
        /// </summary>
        public const char CellValueFormulaEnd = ')';

        /// <summary>
        ///     单元格Key分隔符 ,
        /// </summary>
        public const char CellValueKeySpliter = ',';

        /// <summary>
        ///     单元格Key包含固定值分隔符 =
        /// </summary>
        public const char CellValueFixedSpliter = '=';

        /// <summary>
        ///     单元格分组Key查找模式 % (查找行已添加的分组)
        /// </summary>
        public const string CellGroupKeySearchPattern = "%";

        /// <summary>
        ///     单元格向上合并标志 up
        /// </summary>
        public const string MergeUp = "up";

        /// <summary>
        ///     单元格向左合并标志 left
        /// </summary>
        public const string MergeLeft = "left";

        /// <summary>
        ///     单元格公式:无
        /// </summary>
        public const string ForumlaNone = "";

        /// <summary>
        ///     单元格公式:合计sum
        /// </summary>
        public const string ForumlaSum = "sum";

        /// <summary>
        ///     单元格公式:计数count
        /// </summary>
        public const string ForumlaCount = "count";

        /// <summary>
        ///     单元格格式:日期(年月日) date
        /// </summary>
        public const string FormatDate = "date";

        /// <summary>
        ///     单元格格式:日期时间(年月日时分秒) datetime
        /// </summary>
        public const string FormatDateTime = "datetime";

        /// <summary>
        ///     单元格格式:分(除100) fen
        /// </summary>
        public const string FormatFen = "fen";

        /// <summary>
        ///     单元格格式:月(年月) month
        /// </summary>
        public const string FormatMonth = "month";

        /// <summary>
        ///     单元格格式:数字 num
        /// </summary>
        public const string FormatNumber = "num";

        /// <summary>
        ///     单元格格式:时间(时分秒) time
        /// </summary>
        public const string FormatTime = "time";

        /// <summary>
        ///     单元格格式:时间间隔 span
        /// </summary>
        public const string FormatTimeSpan = "span";

        #endregion 模版单元格Cell解析

        #region 数据添加方式 InsertOption

        /// <summary>
        ///     第一行时添加(遍历数据源)
        /// </summary>
        public const string InsertOnFirst = "first";

        /// <summary>
        ///     最后一行时添加(遍历数据源)
        /// </summary>
        public const string InsertOnLast = "last";

        /// <summary>
        ///     分组值改变前添加
        /// </summary>
        public const string InsertOnBefore = "before";

        /// <summary>
        ///     值改变后添加
        /// </summary>
        public const string InsertOnAfter = "after";

        /// <summary>
        ///     不添加
        /// </summary>
        public const string InsertNever = "never";

        /// <summary>
        ///     时钟添加
        /// </summary>
        public const string InsertAll = "all";

        #endregion 数据添加方式 InsertOption
    }
}