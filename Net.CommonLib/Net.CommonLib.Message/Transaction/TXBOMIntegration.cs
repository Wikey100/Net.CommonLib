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
    /// BOM积分兑换
    /// </summary>
    public class TXBOMIntegration : BaseMessage
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
        /// 交易时间
        /// </summary>
        public string TxnTime { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 票卡Id
        /// </summary>
        public string TicketId { get; set; }

        /// <summary>
        /// 票卡主类型
        /// </summary>

        public string TicketMainType { get; set; }

        // <summary>
        /// 票卡子类型
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 积分余额
        /// </summary>
        public string IntegrationRemain { get; set; }

        // <summary>
        /// 积分扣除
        /// </summary>
        public string IntegrationWithdraw { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public TXBOMIntegration()
        {
            msgType = MsgType.TxnBomIntegration;
        }

        public TXBOMIntegration(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnBomIntegration;
        }

        public override void Decode()
        {
            base.Decode();
            TxnType = GetNextString(2);
            DeviceId = GetNextString(8);

            OperatorId = GetNextString(6);
            ShiftId = GetNextString(10);
            SerialNumber = GetNextString(4);

            TxnTime = GetNextString(14);

            StationId = GetNextString(4);

            TicketId = GetNextString(16);

            TicketMainType = GetNextString(2);

            TicketSubType = GetNextString(2);

            IntegrationRemain = GetNextString(10);

            IntegrationWithdraw = GetNextString(10);

            Spare = GetNextString(12);
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

            encodeBuf.AddRange(AddString(TxnTime, 14));

            encodeBuf.AddRange(AddString(StationId, 4));

            encodeBuf.AddRange(AddString(TicketId, 16));

            encodeBuf.AddRange(AddString(TicketMainType, 2));

            encodeBuf.AddRange(AddString(TicketSubType, 2));

            encodeBuf.AddRange(AddString(IntegrationRemain.PadLeft(10, '0'), 10));

            encodeBuf.AddRange(AddString(IntegrationWithdraw.PadLeft(10, '0'), 10));

            encodeBuf.AddRange(AddString(Spare.PadLeft(12, '0'), 12));
        }
    }
}