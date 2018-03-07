/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;

namespace Net.CommonLib.Ftp
{
    public class DownloadProgressEventArgs : EventArgs
    {
        private long bytesRecevied;

        public long BytesRecevied
        {
            get { return bytesRecevied; }
            set { bytesRecevied = value; }
        }

        private long totalBytesToRecevice;

        public long TotalBytesToRecevice
        {
            get { return totalBytesToRecevice; }
            set { totalBytesToRecevice = value; }
        }
    }
}