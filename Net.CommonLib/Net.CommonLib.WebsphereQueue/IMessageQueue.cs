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
    public interface IMessageQueue
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        void Send(QueueMessage message);

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="uri"></param>
        void Connect(string uri);

        /// <summary>
        /// 创建生产者
        /// </summary>
        void CreateProducer();

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <returns></returns>
        void CreateSubscriber(string name, IMessageListener listener);

        void CreateDurableSubscriber(string topicName, IMessageListener listener);

        void GetDeliveryMode();

        /// <summary>
        ///
        /// </summary>
        /// <param name="clientId"></param>
        void SetClientId(string clientId);

        QueueMessage CreateMessage(string msg);
    }
}