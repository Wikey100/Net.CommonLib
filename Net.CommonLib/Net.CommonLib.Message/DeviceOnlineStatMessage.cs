/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Message
{
    public class DeviceOnlineStatMessage : BaseMessage
    {
        /// <summary>
        /// 消息代号8200
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 采集时间CHAR(14)
        /// </summary>
        public string SendTime { get; set; }

        public string DeviceId { get; set; }

        public string OnlineStat { get; set; }

        public DeviceOnlineStatMessage()
        {
            msgType = MsgType.DeviceOnlineStat;
        }

        public DeviceOnlineStatMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.DeviceOnlineStat;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            SendTime = GetNextString(14);
            DeviceId = GetNextString(8);
            OnlineStat = GetNextString(1);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(SendTime, 14));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(OnlineStat, 1));
        }
    }
}