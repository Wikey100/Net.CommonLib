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
    public class TXBOMAdmin : BaseMessage
    {
        /// <summary>
        /// 交易类型 （2）
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 车站代码CHAR(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 操作员ID(6)
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作员ID(10)
        /// </summary>
        public string ShiftId { get; set; }

        /// <summary>
        /// 序列号(4)
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 行政交易类型(2)
        /// </summary>
        public string AdminType { get; set; }

        /// <summary>
        /// 现金收取标志(2)
        /// </summary>
        public string IncomeFlag { get; set; }

        /// <summary>
        /// 车站Id(4)
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 纠纷金额(4)
        /// </summary>
        public string DessesionAmt { get; set; }

        /// <summary>
        /// 相关设备(8)
        /// </summary>
        public string RelateDeviceId { get; set; }

        /// <summary>
        /// 相关票卡(16)
        /// </summary>
        public string RelateTicketType { get; set; }

        /// <summary>
        /// 发售出站票金额(16)
        /// </summary>
        public string SaleOutBoundTicketAmt { get; set; }

        /// <summary>
        /// 发售出站票金额(16)
        /// </summary>
        public string RelateOutBoundTicketSerialNo { get; set; }

        /// <summary>
        /// 发售出站票类型(8)
        /// </summary>
        public string OutBoundTicketType { get; set; }

        /// <summary>
        /// 交易发生时间(14)
        /// </summary>
        public string TxnTime { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public TXBOMAdmin()
        {
            msgType = MsgType.TxnBomAdmin;
        }

        public TXBOMAdmin(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnBomAdmin;
        }

        public override void Decode()
        {
            base.Decode();
            TxnType = GetNextString(2);
            DeviceId = GetNextString(8);
            OperatorId = GetNextString(6);
            ShiftId = GetNextString(10);
            SerialNo = GetNextString(4);
            AdminType = GetNextString(2);
            IncomeFlag = GetNextString(2);
            StationId = GetNextString(4);
            DessesionAmt = GetNextString(8);
            RelateDeviceId = GetNextString(8);
            RelateTicketType = GetNextString(16);
            SaleOutBoundTicketAmt = GetNextString(8);
            RelateOutBoundTicketSerialNo = GetNextString(10);
            OutBoundTicketType = GetNextString(1);
            TxnTime = GetNextString(14);
            Spare = GetNextString(17);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(ShiftId.PadLeft(10, '0'), 10));
            encodeBuf.AddRange(AddString(SerialNo.PadLeft(4, '0'), 4));
            encodeBuf.AddRange(AddString(AdminType, 2));
            encodeBuf.AddRange(AddString(IncomeFlag.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DessesionAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(RelateDeviceId, 8));
            encodeBuf.AddRange(AddString(RelateTicketType.PadLeft(16, '0'), 16));
            encodeBuf.AddRange(AddString(SaleOutBoundTicketAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(RelateOutBoundTicketSerialNo.PadLeft(8, '0'), 10));
            encodeBuf.AddRange(AddString(OutBoundTicketType, 1));
            encodeBuf.AddRange(AddString(TxnTime, 14));
            encodeBuf.AddRange(AddString(Spare.PadLeft(17, '0'), 17));
        }
    }
}