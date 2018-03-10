/*******************************************************************
 * * 文件名：DownProgressEvent.cs
 * * 文件作用：定义下载处理委托
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Ftp
{
    /// <summary>
    /// 下载进程处理
    /// </summary>
    public delegate void DownLoadProgressEventHandler(object sender, DownloadProgressEventArgs args);
}