/*******************************************************************
 * * 文件名： MQProducer.cs
 * * 文件作用： MQProducer 消息生产者
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2019-03-06    xwj       新增
 * *******************************************************************/

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.IO;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Net;


namespace Net.CommonLib.ActiveMqQueue
{
    public class MQProducer
    {
        #region 字段/属性
        /// <summary>
        /// 消息用户名
        /// </summary>
        private string userName;

        /// <summary>
        /// 消息用户密码
        /// </summary>
        private string userPWD;

        /// <summary>
        /// 消息连接地址
        /// </summary>
        private string mqURL;

        /// <summary>
        /// 队列名
        /// </summary>
        private string queueName = "Produce_Topic";

        /// <summary>
        /// 连接工厂
        /// </summary>
        private ConnectionFactory connectFactory;

        /// <summary>
        /// 会话
        /// </summary>
        private ISession session;

        /// <summary>
        /// 消息生产者
        /// </summary>
        private IMessageProducer messageProducer;

        /// <summary>
        /// 与MQ连接
        /// </summary>
        private IConnection connection = null;

        private IDestination destination = null;

        #endregion

        #region 构造函数

        public MQProducer(string url,string userName,string userPWD)
        {
            this.mqURL = url;
            this.userName = userName;
            this.userPWD = userPWD;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 初始化生产者消息队列
        /// </summary>
        private void InitProducerMQ()
        {
            try
            {
                Log.Instance.Info("初始化生产者消息队列...");
                connectFactory = new ConnectionFactory(mqURL, Dns.GetHostName());
                connection = connectFactory.CreateConnection(userName,userPWD);
                connection.Start();
                session = connection.CreateSession();
                destination = session.GetDestination("ProduceTopicName");
                messageProducer = session.CreateProducer(destination);
                messageProducer.DeliveryMode = MsgDeliveryMode.Persistent; //MQ服务器停止工作后，消息不再保留
            }
            catch (Exception ex)
            {
                Log.Instance.Error("初始化生产者消息队列出现错误",ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 向消息对列发送消息
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="method"></param>
        /// <param name="contenjson"></param>
        protected void SendProduceMsg(string guid,string method,string contenjson)
        {
            try
            {
                Log.Instance.Info(string.Format("向消息队列发送消息[GUID:{0},Method:{1},ContenJson:{2}]",guid,method,contenjson));
                var model = new MQModel()
                {
                    Guid = guid,
                    Method = method,
                    ContenJson = contenjson
                };
                var msg = session.CreateObjectMessage(model);
                messageProducer.Send(msg);
            }
            catch (Exception ex)
            {
                Log.Instance.Error("向消息对列发送消息出现错误", ex);
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}