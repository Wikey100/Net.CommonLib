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
    /// 团体票发售
    /// </summary>
    public class TXBOMGroupTicketSale : BaseMessage
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
        /// 目的车站
        /// </summary>
        public string DestinationStationId { get; set; }

        /// <summary>
        /// 运营日
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 票卡主类型
        /// </summary>

        public string TicketMainType { get; set; }

        // <summary>
        /// 票卡子类型
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string UnitPrice { get; set; }

        // <summary>
        /// 总数量
        /// </summary>
        public string TotalAmount { get; set; }

        /// <summary>
        /// 交易总金额
        /// </summary>
        public string TxTotalAmt { get; set; }

        /// <summary>
        /// 手工台账号
        /// </summary>
        public string ManualId { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public TXBOMGroupTicketSale()
        {
            msgType = MsgType.TxnBomGroupTicketSale;
        }

        public TXBOMGroupTicketSale(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnBomGroupTicketSale;
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

            DestinationStationId = GetNextString(4);

            ReportDate = GetNextString(8);

            TicketMainType = GetNextString(2);

            TicketSubType = GetNextString(2);

            UnitPrice = GetNextString(16);

            TotalAmount = GetNextString(4);

            TxTotalAmt = GetNextString(16);

            ManualId = GetNextString(10);

            Spare = GetNextString(4);
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

            encodeBuf.AddRange(AddString(DestinationStationId, 4));

            encodeBuf.AddRange(AddString(ReportDate, 8));

            encodeBuf.AddRange(AddString(TicketMainType, 2));

            encodeBuf.AddRange(AddString(TicketSubType, 2));

            encodeBuf.AddRange(AddString(UnitPrice.PadLeft(16, '0'), 16));

            encodeBuf.AddRange(AddString(TotalAmount.PadLeft(4, '0'), 4));

            encodeBuf.AddRange(AddString(TxTotalAmt.PadLeft(16, '0'), 16));

            encodeBuf.AddRange(AddString(ManualId, 10));

            encodeBuf.AddRange(AddString(Spare.PadLeft(4, '0'), 4));
        }
    }
}