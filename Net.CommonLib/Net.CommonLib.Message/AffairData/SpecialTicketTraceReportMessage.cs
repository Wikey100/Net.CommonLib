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
    public class SpecialTicketTraceReportMessage : BaseMessage
    {
        /// <summary>
        /// 消息代号（4）
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 票卡使用时间
        /// </summary>
        public string TicketUseTime { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string TicketNo { get; set; }

        public SpecialTicketTraceReportMessage()
        {
            msgType = MsgType.SpecialTicketTraceReport;
        }

        public SpecialTicketTraceReportMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.SpecialTicketTraceReport;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            TicketUseTime = GetNextString(14);
            DeviceId = GetNextString(8);
            TicketNo = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(TicketUseTime, 14));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(TicketNo, 16));
        }
    }
}