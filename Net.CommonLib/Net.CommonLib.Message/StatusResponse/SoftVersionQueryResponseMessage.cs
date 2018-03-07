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
    public class SoftVersionQueryResponseMessage : BaseMessage
    {
        /// <summary>
        /// 时间 CHAR(14)
        /// </summary>
        public string CollectTime { get; set; }

        /// <summary>
        /// 车站代码CHAR(4)
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 设备编码(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 状态描述 CHAR(4)，填0000
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// 当前软件版本，前补0 CHAR(2）
        /// </summary>
        public string SoftType { get; set; }

        /// <summary>
        /// 当前版本(12),前补0
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 将来版本(12),前补0
        /// </summary>
        public string FutureVersion { get; set; }

        /// <summary>
        /// 预留(8),前补0
        /// </summary>
        public string Spare { get; set; }

        public SoftVersionQueryResponseMessage()
        {
            msgType = MsgType.ResponeSoftVersion;
        }

        public SoftVersionQueryResponseMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.ResponeSoftVersion;
        }

        public override void Decode()
        {
            base.Decode();
            CollectTime = GetNextString(14);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            StatusDescription = GetNextString(4);
            SoftType = GetNextString(2);
            CurrentVersion = GetNextString(12);
            FutureVersion = GetNextString(12);
            Spare = GetNextString(8);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(CollectTime, 14));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(StatusDescription, 4));
            encodeBuf.AddRange(AddString(SoftType, 2));
            encodeBuf.AddRange(AddString(CurrentVersion.PadLeft(12, '0'), 12));
            encodeBuf.AddRange(AddString(FutureVersion.PadLeft(12, '0'), 12));
            encodeBuf.AddRange(AddString(Spare, 8));
        }
    }
}