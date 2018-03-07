using IBM.WMQ;
using Net.CommonLib.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net.CommonLib.WebsphereQueue
{
    public class WebSphereMQConnector : IMessageQueue
    {
        private MQQueueManager queueManager;
        private string host;
        private string port;
        private string channel;
        private string qmName;
        private string userId;
        private string password;
        private string topciName;
        private ILog log;

        public const int FILE_SPLIT_LEN = 1024 * 1024 * 3;

        #region property

        public string Host
        {
            get
            {
                return host;
            }

            set
            {
                host = value;
            }
        }

        public string Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        public string Channel
        {
            get
            {
                return channel;
            }

            set
            {
                channel = value;
            }
        }

        public string QmName
        {
            get
            {
                return qmName;
            }

            set
            {
                qmName = value;
            }
        }

        public string UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public object Tag { get; set; }
        public string DurableSubscriptionName { get; set; }
        public EventHandler ReconnectEvent;

        public ILog Log
        {
            get
            {
                return log;
            }

            set
            {
                log = value;
            }
        }

        #endregion property

        private Hashtable properties;
        private string uri;

        public void Connect(string uri)
        {
            this.uri = uri;
            properties = new Hashtable();
            //properties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES);
            properties.Add(MQC.HOST_NAME_PROPERTY, host);
            properties.Add(MQC.PORT_PROPERTY, port);
            properties.Add(MQC.CHANNEL_PROPERTY, channel);
            properties.Add(MQC.USER_ID_PROPERTY, userId);
            properties.Add(MQC.PASSWORD_PROPERTY, password);
            properties.Add(MQC.CCSID_PROPERTY, 1381);
            // set this property may by cause error 2195
            // properties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED);
            queueManager = new MQQueueManager(uri, properties);
        }

        public void CreateSubscriber(string name, IMessageListener listener)
        {
            MQQueue queue = queueManager.AccessQueue(name, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);

            Task.Factory.StartNew(() =>
            {
                while (!listener.IsStop)
                {
                    try
                    {
                        var message = new MQMessage();
                        queue.Get(message);
                        var qm = ConvertBack(message);
                        listener.OnMessage(qm);
                        message.ClearMessage();
                    }
                    catch (MQException mqe)
                    {
                        if (mqe.ReasonCode == 2033)
                        {
                            LogMsg(string.Format("获取MQ【BUFF】消息异常,异常代码【2033】,队列:{0},MQ地址:{1},异常信息:{2}", name, host, mqe.ToString()));
                            Thread.Sleep(500);
                        }
                        else
                        {
                            LogMsg(string.Format("获取MQ【BUFF】消息异常,异常代码【2033】,队列:{0},MQ地址:{1},异常信息:{2}", name, host, mqe.ToString()));
                            Thread.Sleep(5000);
                            Reconnect(ref queueManager, ref queue, name);
                        }
                    }
                    catch (Exception e)
                    {
                        LogMsg(string.Format("获取MQ【BUFF】消息异常,异常代码【2033】,队列:{0},MQ地址:{1},异常信息:{2}", name, host, e.Message.ToString()));
                        Thread.Sleep(5000);
                        Reconnect(ref queueManager, ref queue, name);
                    }
                }
                queue.Close();
            });
        }

        public void CreateFileSubscriber(string name, string path, IMessageListener listener)
        {
            MQQueue queue = queueManager.AccessQueue(name, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);

            MQGetMessageOptions gmo = new MQGetMessageOptions();
            gmo.Options = MQC.MQGMO_FAIL_IF_QUIESCING;
            gmo.Options = gmo.Options + MQC.MQGMO_SYNCPOINT;
            gmo.Options = gmo.Options + MQC.MQGMO_WAIT;
            gmo.WaitInterval = 3000;
            gmo.Options = gmo.Options + MQC.MQGMO_ALL_MSGS_AVAILABLE;
            gmo.Options = gmo.Options + MQC.MQGMO_LOGICAL_ORDER;
            gmo.MatchOptions = MQC.MQMO_MATCH_GROUP_ID;
            gmo.Version = MQC.MQMD_VERSION_2;
            Task.Factory.StartNew(() =>
            {
                while (!listener.IsStop)
                {
                    try
                    {
                        var message = new MQMessage();
                        message.MQMD.Version = MQC.MQMD_VERSION_2;
                        List<byte> total = new List<byte>();

                        while (true)
                        {
                            queue.Get(message, gmo);

                            int msgLength = message.MessageLength;
                            byte[] buff = new byte[msgLength];
                            message.ReadFully(ref buff);
                            total.AddRange(buff);
                            if (gmo.GroupStatus == MQC.MQGS_LAST_MSG_IN_GROUP)
                            {
                                message.ClearMessage();
                                queueManager.Commit();
                                break;
                            }
                        }

                        byte[] findStr = System.Text.Encoding.Default.GetBytes("|");
                        byte[] totalBytes = total.ToArray();
                        var ret = Search(totalBytes, 0, findStr);
                        if (ret != -1)
                        {
                            byte[] fileNameHead = new byte[ret];
                            byte[] content = new byte[totalBytes.Length - ret - 1];
                            Array.Copy(totalBytes, 0, fileNameHead, 0, ret);

                            Array.Copy(totalBytes, ret + 1, content, 0, totalBytes.Length - ret - 1);

                            var fileName = System.Text.Encoding.Default.GetString(fileNameHead);
                            var generateFile = System.IO.Path.Combine(path, fileName);

                            using (FileStream fSteam = new FileStream(generateFile, FileMode.Create))
                            {
                                fSteam.Write(content, 0, content.Length);
                                fSteam.Flush();
                                fSteam.Close();
                                if (listener != null)
                                {
                                    QueueMessage qm = new QueueMessage();
                                    qm.Type = MessageType.File;
                                    qm.Data = fileName;
                                    listener.OnMessage(qm);
                                }
                            }
                        }
                    }
                    catch (MQException mqe)
                    {
                        if (mqe.ReasonCode == 2033)
                        {
                            LogMsg(string.Format("获取MQ【FILE】消息异常,异常代码【2033】,队列:{0},MQ地址:{1},异常信息:{2}", name, host, mqe.ToString()));
                            Thread.Sleep(500);
                        }
                        else
                        {
                            LogMsg(string.Format("获取MQ【FILE】消息异常,队列:{0},MQ地址:{1},异常信息:{2}", name, host, mqe.ToString()));
                            Thread.Sleep(5000);
                            Reconnect(ref queueManager, ref queue, name);
                        }
                    }
                    catch (Exception e)
                    {
                        LogMsg(string.Format("获取MQ【FILE】消息异常,队列:{0},MQ地址:{1},异常信息:{2}", name, host, e.Message.ToString()));
                        Thread.Sleep(5000);
                        Reconnect(ref queueManager, ref queue, name);
                    }
                }
                LogMsg(string.Format("队列{0},监控文件线程退出..."));
                queue.Close();
            });
        }

        public void CreateDurableSubscriber(string topicName, IMessageListener listener)
        {
            this.topciName = topicName;
            var topic = queueManager.AccessTopic(topicName, null,
                MQC.MQSO_CREATE | MQC.MQSO_FAIL_IF_QUIESCING | MQC.MQSO_RESUME,
                null, DurableSubscriptionName);

            var message = new MQMessage();
            Task.Factory.StartNew(() =>
            {
                while (!listener.IsStop)
                {
                    try
                    {
                        message = new MQMessage();
                        topic.Get(message, new MQGetMessageOptions()
                        {
                            WaitInterval = -1,
                            Options = MQC.MQGMO_WAIT | MQC.MQGMO_NO_SYNCPOINT
                        });
                        var qm = ConvertBack(message);
                        listener.OnMessage(qm);
                        message.ClearMessage();
                    }
                    catch (MQException mqe)
                    {
                        if (mqe.ReasonCode == 2033)
                        {
                            Thread.Sleep(500);
                        }
                        else
                        {
                            LogMsg("获取MQ（Topic）消息异常：" + host + mqe.ToString());
                            Thread.Sleep(5000);
                            Reconnect(ref queueManager, ref topic, topicName);
                        }
                    }
                    catch (Exception e)
                    {
                        LogMsg("获取MQ（Topic）消息异常：" + host + e.ToString());
                        Thread.Sleep(5000);
                        Reconnect(ref queueManager, ref topic, topicName);
                    }
                }
                topic.Close();
            });
        }

        private void Reconnect(ref MQQueueManager queueManager, ref MQTopic topic, string topicName)
        {
            try
            {
                LogMsg(string.Format("开始重连MQ：{0},{1}", host, topicName));
                try
                {
                    if (queueManager != null)
                    {
                        queueManager.Disconnect();
                    }
                    if (topic != null)
                    {
                        topic.Close();
                    }
                }
                catch (Exception e)
                {
                    LogMsg("释放连接失败" + e);
                }
                queueManager = new MQQueueManager(uri, properties);
                topic = queueManager.AccessTopic(topicName, null,
                    MQC.MQSO_CREATE | MQC.MQSO_FAIL_IF_QUIESCING | MQC.MQSO_RESUME,
                    null, DurableSubscriptionName);
                LogMsg(string.Format("重连MQ成功：{0},{1}", host, topicName));
                if (ReconnectEvent != null)
                {
                    ReconnectEvent(this.Tag, null);
                }
            }
            catch (Exception ex)
            {
                LogMsg(string.Format("重连MQ失败：host:{0},exp:{1}", host, ex));
            }
        }

        private void Reconnect(ref MQQueueManager queueManager, ref MQQueue queue, string queueName)
        {
            try
            {
                LogMsg(string.Format("开始重连MQ：{0},queueName:{1}", host, queueName));
                try
                {
                    if (queueManager != null)
                    {
                        queueManager.Disconnect();
                    }
                    if (queue != null)
                    {
                        queue.Close();
                    }
                }
                catch (Exception e)
                {
                    LogMsg("释放连接失败" + e);
                }
                queueManager = new MQQueueManager(uri, properties);
                queue = queueManager.AccessQueue(queueName, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);
                LogMsg(string.Format("重连MQ成功：{0},{1}", host, queueName));
                if (ReconnectEvent != null)
                {
                    ReconnectEvent(this.Tag, null);
                }
            }
            catch (Exception ex)
            {
                LogMsg(string.Format("重连MQ失败：host:{0},exp:{1}", host, ex));
            }
        }

        public QueueMessage CreateMessage(string msg)
        {
            throw new NotImplementedException();
        }

        public void CreateProducer()
        {
            throw new NotImplementedException();
        }

        public void GetDeliveryMode()
        {
            throw new NotImplementedException();
        }

        public void Send(QueueMessage message)
        {
            var msg = Convert(message);
            var buff = message.Data as byte[];
            if (message.DestinationType == MDestinationType.Topic)
            {
                var topic = queueManager.AccessTopic(message.DestinationName, null,
                    MQC.MQTOPIC_OPEN_AS_PUBLICATION, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);

                topic.Put(msg);
                LogMsg(string.Format("【SEND】\t【TOPIC】\t【{0}】\t【{1}】", message.DestinationName, GetString(buff, 0, buff.Length)));
                msg = null;
                queueManager.Disconnect();
            }
            else if (message.DestinationType == MDestinationType.Queue)
            {
                var queue = queueManager.AccessQueue(message.DestinationName,
                    MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);

                msg.MQMD.CorrelId = System.Text.Encoding.Default.GetBytes(string.Format("{0}|21", message.MessageId));
                msg.MQMD.Ccsid = 1381;
                queue.Put(msg);

                LogMsg(string.Format("【SEND】\t【BUFF】\t【{0}】\t【{1}】", message.DestinationName, GetString(buff, 0, buff.Length)));
                //释放连接
                queue.Close();
                queueManager.Disconnect();
            }
        }

        public void SendFile(string filePath, string DesQueueName)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buff = new byte[FILE_SPLIT_LEN];
                var fileName = System.IO.Path.GetFileName(filePath);
                var queue = queueManager.AccessQueue(DesQueueName,
                      MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);

                MQPutMessageOptions op = new MQPutMessageOptions();
                op.Options = op.Options + MQC.MQPMO_LOGICAL_ORDER;
                op.Options = op.Options + MQC.MQPMRF_GROUP_ID;

                int count = -1;
                MQMessage msg = new MQMessage();
                msg.MessageFlags = MQC.MQMF_MSG_IN_GROUP;
                msg.MQMD.CorrelId = System.Text.Encoding.Default.GetBytes("000|21");

                bool isContainFileName = true;
                while (true)
                {
                    if (isContainFileName)
                    {
                        byte[] headBytes = System.Text.Encoding.Default.GetBytes(fileName + "|");
                        List<byte> totalBytes = new List<byte>();
                        totalBytes.AddRange(headBytes);
                        byte[] b = new byte[FILE_SPLIT_LEN - headBytes.Length];
                        var tempCount = fs.Read(b, 0, FILE_SPLIT_LEN - headBytes.Length);
                        count = tempCount + headBytes.Length;
                        totalBytes.AddRange(b);
                        msg.Write(totalBytes.ToArray(), 0, count);
                        isContainFileName = false;
                    }
                    else
                    {
                        count = fs.Read(buff, 0, FILE_SPLIT_LEN);
                        msg.Write(buff, 0, count);
                    }
                    if (count == 0)
                    {
                        break;
                    }
                    if (count < FILE_SPLIT_LEN)
                    {
                        msg.MessageFlags = MQC.MQMF_LAST_MSG_IN_GROUP;
                    }
                    queue.Put(msg, op);
                    msg.ClearMessage();
                }
                queue.Close();
                queueManager.Disconnect();
                LogMsg(string.Format("【SEND】\t【FILE】\t【{0}】\t【{1}】", DesQueueName, fileName));
            }
        }

        public void SetClientId(string clientId)
        {
            DurableSubscriptionName = clientId;
        }

        private MQMessage Convert(QueueMessage message)
        {
            MQMessage msg = new MQMessage();
            if (message.Type == MessageType.Text)
            {
                msg.WriteString(message.Data.ToString());
            }
            else if (message.Type == MessageType.Bytes)
            {
                msg.Write((byte[])message.Data);
            }
            //else if (message.Type == MessageType.File)
            //{
            //    msg.MessageFlags = MQC.MQMF_MSG_IN_GROUP;

            //    byte[] source = (byte[])message.Data;
            //    int Total = source.Length;
            //    if (Total>FILE_SPLIT_LEN)
            //    {
            //        var len = Total / FILE_SPLIT_LEN;
            //        int j = 0;
            //        for (int i = 0; i < len; i++)
            //        {
            //            byte[] buff = new byte[FILE_SPLIT_LEN];
            //            Array.Copy(source, j, buff, 0, FILE_SPLIT_LEN);
            //            j = j + FILE_SPLIT_LEN;
            //            msg.Write(buff);
            //        }
            //        if (j < Total)
            //        {
            //            var remain = Total - j;
            //            byte[] lastBuff = new byte[remain];

            //            Array.Copy(source, j, lastBuff, 0, remain);
            //            msg.Write(lastBuff);
            //        }

            //        msg.MessageFlags = MQC.MQMF_LAST_MSG_IN_GROUP;
            //    }
            //    else
            //    {
            //        msg.Write((byte[])message.Data);
            //        msg.MessageFlags = MQC.MQMF_LAST_MSG_IN_GROUP;
            //    }

            //}
            return msg;
        }

        private QueueMessage ConvertBack(MQMessage msg)
        {
            return new QueueMessage()
            {
                Data = msg.ReadBytes(msg.DataLength),
            };
        }

        private void LogMsg(string msg)
        {
            if (Log != null)
            {
                Log.Info(msg);
            }
        }

        private string GetString(byte[] byteArray, int offset, int len)
        {
            StringBuilder str = new StringBuilder();
            for (int i = offset; i < offset + len; i++)
            {
                str.Append(byteArray[i].ToString("X2"));
            }
            return str.ToString();
        }

        private int Search(byte[] source, int startIndex, byte[] searchKey)
        {
            if (source == null || source.Length <= startIndex)
                return -1;
            if (searchKey == null || source.Length <= searchKey.Length)
                return -1;
            int i = -1;
            int j = -1;

            for (i = startIndex; i < source.Length; i++)
            {
                if (source.Length < searchKey.Length + i)
                    break;
                for (j = 0; j < searchKey.Length; j++)
                {
                    if (source[i + j] != searchKey[j])
                        break;
                }
                if (j == searchKey.Length)
                    return i;
            }
            return -1;
        }

        public void DisConnect()
        {
            if (queueManager != null)
            {
                queueManager.Disconnect();
            }
        }
    }
}