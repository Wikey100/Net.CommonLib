/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Message.Transaction
{
    /// <summary>
    /// 外卡黑名单捕获
    /// </summary>
    public class TXOuterBlackListCapture : BaseMessage
    {
        /// <summary>
        /// 发生交易设备代码
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 交易类型)
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public string TicketRemainAmt { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        /// <summary>
        /// 终端编号
        /// </summary>

        public string TerminalId { get; set; }

        // <summary>
        /// 终端标志
        /// </summary>
        public string TerminalFlag { get; set; }

        /// <summary>
        /// 锁卡时间
        /// </summary>
        public string LockTime { get; set; }

        /// <summary>
        /// 票卡逻辑卡号
        /// </summary>
        public string TicketLogicalId { get; set; }

        /// <summary>
        /// sam卡卡号
        /// </summary>
        public string TicketPhysicalId { get; set; }

        /// <summary>
        /// 票卡主类型
        /// </summary>
        public string TicketMainType { get; set; }

        /// <summary>
        /// 票卡子类型
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 外卡预留
        /// </summary>
        public string Spare2 { get; set; }

        public TXOuterBlackListCapture()
        {
            msgType = MsgType.TxnOuterBlackListCapture;
        }

        public TXOuterBlackListCapture(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnOuterBlackListCapture;
        }

        public override void Decode()
        {
            base.Decode();

            DeviceId = GetNextString(8);
            StationId = GetNextString(4);
            TxnType = GetNextString(2);
            TicketRemainAmt = GetNextString(8);
            Spare = GetNextString(8);
            GetNextString(1);
            TerminalId = GetNextString(16);
            GetNextString(1);
            TerminalFlag = GetNextString(1);
            GetNextString(1);
            LockTime = GetNextString(14);
            GetNextString(1);
            TicketLogicalId = GetNextString(16);
            GetNextString(1);
            TicketPhysicalId = GetNextString(8);
            GetNextString(1);
            TicketMainType = GetNextString(2);
            GetNextString(1);
            TicketSubType = GetNextString(2);
            GetNextString(1);
            Spare2 = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(TicketRemainAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(Spare.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TerminalId, 16));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TerminalFlag, 1));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(LockTime, 14));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketLogicalId, 16));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketPhysicalId, 8));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketMainType, 2));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketSubType, 2));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(Spare2.PadLeft(16, '0'), 16));
        }
    }
}