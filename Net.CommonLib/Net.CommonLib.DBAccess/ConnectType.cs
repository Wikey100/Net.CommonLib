/*******************************************************************
 * * 文件名： ConnectType.cs
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// 数据库连接类型
    /// </summary>
    public enum ConnectType
    {
        /// <summary>
        /// sysbase数据库连接
        /// </summary>
        Sybase,

        /// <summary>
        /// Sqlite数据库连接
        /// </summary>
        SQLite,

        /// <summary>
        /// SqlServer数据库连接
        /// </summary>
        SQLServer,

        /// <summary>
        /// Oracle数据库连接
        /// </summary>
        Oracle,

        /// <summary>
        /// Access数据库连接
        /// </summary>
        MsAccess,

        /// <summary>
        /// MySql数据库连接
        /// </summary>
        MySql,

        /// <summary>
        ///
        /// </summary>
        OleDb,
    }
}