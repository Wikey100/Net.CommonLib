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
    public class QueueMessage
    {
        public string DestinationName { get; set; }
        public DeliveryMode DeliveryMode { get; set; }
        public int Priority { get; set; }

        public MessageType Type { get; set; }

        public MDestinationType DestinationType { get; set; }

        public object Data { get; set; }

        public string MessageId { get; set; }

        public void Ack()
        { }
    }

    public enum MDestinationType
    {
        Queue = 0,
        Topic = 1,
        TemporaryQueue = 2,
        TemporaryTopic = 3,
    }

    public enum MessageType
    {
        Text,
        Bytes,
        File,
    }

    public enum DeliveryMode
    {
        Persisent,
        NonPersisent,
    }
}