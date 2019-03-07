/*******************************************************************
 * * 文件名： MQConsumer.cs
 * * 文件作用： MQConsumer 消息消费者
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2019-03-06    xwj       新增
 * *******************************************************************/

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Net.CommonLib.ActiveMqQueue
{
    public class MQConsumer
    {
        #region 字段/属性

        /// <summary>
        ///连接地址
        /// </summary>
        private string mqURL = string.Empty;

        /// <summary>
        /// MQ用户名
        /// </summary>
        private string mqUserName = string.Empty;

        /// <summary>
        /// MQ密码
        /// </summary>
        private string mqUserPWD = string.Empty;

        /// <summary>
        /// 生产者约定消息主题
        /// </summary>
        private string activeQueueTopicName = "ProduceTopicName";

        #endregion 字段/属性

        #region 构造函数

        public MQConsumer(string mqUrl, string mqUserName, string mqUserPWD, string activeQueueTopicName)
        {
            this.mqURL = mqUrl;
            this.mqUserName = mqUserName;
            this.mqUserPWD = mqUserPWD;
            this.activeQueueTopicName = activeQueueTopicName;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 初始化消息队列会话
        /// </summary>
        private void InitMQ()
        {
            try
            {
                Log.Instance.Info("初始化MQ会话..");
                //创建连接工厂
                IConnectionFactory factory = new ConnectionFactory(mqURL, mqUserName);
                //通过工厂创建连接
                IConnection connection = factory.CreateConnection(mqUserName, mqUserPWD);
                connection.ClientId = System.Net.Dns.GetHostName();
                connection.Start();
                ISession session = connection.CreateSession();
                //通过会话创建一个消费者
                IMessageConsumer consumer = session.CreateConsumer(new ActiveMQTopic(activeQueueTopicName));
                consumer.Listener += new MessageListener(Consumer_Listener);
            }
            catch (Exception ex)
            {
                Log.Instance.Error("初始化MQ会话出现错误", ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 消息事件处理
        /// </summary>
        /// <param name="message"></param>
        private void Consumer_Listener(IMessage message)
        {
            try
            {
                ITextMessage msg = (ITextMessage)message;
                //指定委托进行消息处理
                Task.Factory.StartNew(() =>
                {
                    MQModel model = ConvertObjectModelByJson(msg.Text);
                    Log.Instance.Info(string.Format("处理ActiveMQ消息[Guid:{0},Method:{1},ContenJson:{2}]", model.Guid, model.Method, model.ContenJson));
                });
            }
            catch (Exception ex)
            {
                Log.Instance.Error("注册监听事件出现错误", ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 反序列化把json字节流转换成实体类
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private MQModel ConvertObjectModelByJson(string msg)
        {
            try
            {
                Log.Instance.Info(string.Format("接收消息字节流:{0}", msg));
                DataContractJsonSerializer seralizer = new DataContractJsonSerializer(typeof(MQModel));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(msg));
                object obj = seralizer.ReadObject(stream);
                MQModel model = (MQModel)obj;
                return model;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("反序列化解析消息出现错误", ex);
                throw new Exception(ex.Message);
            }
        }

        #endregion 方法
    }
}