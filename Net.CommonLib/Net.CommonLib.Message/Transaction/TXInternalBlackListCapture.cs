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
    /// 内卡黑名单捕获
    /// </summary>
    public class TXInternalBlackListCapture : BaseMessage
    {
        /// <summary>
        /// 发生交易设备代码
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 交易序列组号(2)
        /// </summary>
        public string TxnGroupNumber { get; set; }

        /// <summary>
        /// 交易序列号（8）
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 交易类型)
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 票卡主类型
        /// </summary>

        public string TicketMainType { get; set; }

        // <summary>
        /// 票卡子类型
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 票价区域代码
        /// </summary>
        public string TicketPriceCode { get; set; }

        /// <summary>
        /// sam卡卡号
        /// </summary>
        public string SamCardNumber { get; set; }

        /// <summary>
        /// 票卡逻辑卡号
        /// </summary>
        public string TicketLogicalId { get; set; }

        /// <summary>
        /// 票卡写操作计数
        /// </summary>
        public string TicketWriteCouter { get; set; }

        /// <summary>
        /// 本次票卡操作金额
        /// </summary>
        public string ThisTicketOperateAmt { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public string TicketRemainAmt { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TxnTime { get; set; }

        /// <summary>
        /// 交易验证码
        /// </summary>
        public string TACCode { get; set; }

        public TXInternalBlackListCapture()
        {
            msgType = MsgType.TxnInnerBlackListCapture;
        }

        public TXInternalBlackListCapture(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnInnerBlackListCapture;
        }

        public override void Decode()
        {
            base.Decode();

            DeviceId = GetNextString(8);
            TxnGroupNumber = GetNextString(2);
            SerialNumber = GetNextString(8);
            StationId = GetNextString(4);
            TxnType = GetNextString(2);
            TicketMainType = GetNextString(2);
            TicketSubType = GetNextString(2);
            TicketPriceCode = GetNextString(2);
            SamCardNumber = GetNextString(8);
            TicketLogicalId = GetNextString(16);
            TicketWriteCouter = GetNextString(6);
            ThisTicketOperateAmt = GetNextString(8);
            TicketRemainAmt = GetNextString(8);
            TxnTime = GetNextString(14);
            TACCode = GetNextString(8);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(TxnGroupNumber.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(SerialNumber.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(TicketMainType, 2));
            encodeBuf.AddRange(AddString(TicketSubType, 2));
            encodeBuf.AddRange(AddString(TicketPriceCode, 2));
            encodeBuf.AddRange(AddString(SamCardNumber, 8));
            encodeBuf.AddRange(AddString(TicketLogicalId, 16));
            encodeBuf.AddRange(AddString(TicketWriteCouter.PadLeft(6, '0'), 6));
            encodeBuf.AddRange(AddString(ThisTicketOperateAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(TicketRemainAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(TxnTime, 14));
            encodeBuf.AddRange(AddString(TACCode, 8));
        }
    }
}