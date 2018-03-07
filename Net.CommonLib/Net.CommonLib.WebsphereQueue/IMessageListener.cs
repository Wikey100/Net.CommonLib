/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.WebsphereQueue
{
    public abstract class IMessageListener
    {
        public bool IsStop { get; set; }

        public abstract void OnMessage(QueueMessage message);
    }
}