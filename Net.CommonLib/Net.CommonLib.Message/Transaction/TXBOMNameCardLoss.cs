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
    /// 记名卡挂失/解挂
    /// </summary>
    public class TXBOMNameCardLoss : BaseMessage
    {
        /// <summary>
        /// 交易类型)
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 发生交易设备代码
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 操作员ID(2)
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 班次号（8）
        /// </summary>
        public string ShiftId { get; set; }

        /// <summary>
        /// 交易序列号（8）
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 挂失标志
        /// </summary>
        public string LossFlag { get; set; }

        /// <summary>
        /// 票卡印刻号
        /// </summary>
        public string TicketCardId { get; set; }

        /// <summary>
        /// 票卡主类型
        /// </summary>

        public string TicketMainType { get; set; }

        // <summary>
        /// 票卡子类型
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 交易总金额
        /// </summary>
        public string TxTotalAmt { get; set; }

        /// <summary>
        /// 手工台账号
        /// </summary>
        public string ManualId { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TxnTime { get; set; }

        /// <summary>
        /// 身份证件类别
        /// </summary>
        public string CredentialsType { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string CredentialsNumber { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public string RefundAmt { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public TXBOMNameCardLoss()
        {
            msgType = MsgType.TxnBomNameCardLoss;
        }

        public TXBOMNameCardLoss(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnBomNameCardLoss;
        }

        public override void Decode()
        {
            base.Decode();

            TxnType = GetNextString(2);

            DeviceId = GetNextString(8);

            OperatorId = GetNextString(6);

            ShiftId = GetNextString(10);

            SerialNumber = GetNextString(4);

            StationId = GetNextString(4);

            LossFlag = GetNextString(1);

            TicketCardId = GetNextString(16);
            TicketMainType = GetNextString(2);

            TicketSubType = GetNextString(2);

            TxTotalAmt = GetNextString(8);

            ManualId = GetNextString(10);

            TxnTime = GetNextString(14);

            CredentialsType = GetNextString(1);

            CredentialsNumber = GetNextString(20);
            RefundAmt = GetNextString(10);
            Spare = GetNextString(1);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            base.Decode();

            encodeBuf.AddRange(AddString(TxnType, 2));

            encodeBuf.AddRange(AddString(DeviceId, 8));

            encodeBuf.AddRange(AddString(OperatorId, 6));

            encodeBuf.AddRange(AddString(ShiftId.PadLeft(10, '0'), 10));

            encodeBuf.AddRange(AddString(SerialNumber.PadLeft(4, '0'), 4));

            encodeBuf.AddRange(AddString(StationId, 4));

            encodeBuf.AddRange(AddString(LossFlag, 1));

            encodeBuf.AddRange(AddString(TicketCardId, 16));
            encodeBuf.AddRange(AddString(TicketMainType, 2));

            encodeBuf.AddRange(AddString(TicketSubType, 2));

            encodeBuf.AddRange(AddString(TxTotalAmt.PadLeft(8, '0'), 8));

            encodeBuf.AddRange(AddString(ManualId, 10));

            encodeBuf.AddRange(AddString(TxnTime, 14));

            encodeBuf.AddRange(AddString(CredentialsType, 1));

            encodeBuf.AddRange(AddString(CredentialsNumber, 20));

            encodeBuf.AddRange(AddString(RefundAmt.PadLeft(10, '0'), 10));

            encodeBuf.AddRange(AddString(Spare.PadLeft(1, '0'), 1));
        }
    }
}