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
    /// 特殊票卡申领
    /// </summary>
    public class TXBOMSpecialTicketRequest : BaseMessage
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
        /// 交易时间
        /// </summary>
        public string TxnTime { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 票卡逻辑Id
        /// </summary>
        public string TicketLogicalId { get; set; }

        /// </summary>

        public string TicketMainType { get; set; }

        // <summary>
        /// 票卡子类型
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 身份证件类别
        /// </summary>
        public string CredentialsType { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string CredentialsNumber { get; set; }

        /// <summary>
        /// j旧卡逻辑Id
        /// </summary>
        public string OriginalLogicalId { get; set; }

        // <summary>
        /// 旧卡状态
        /// </summary>
        public string OriginalCardStatus { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public TXBOMSpecialTicketRequest()
        {
            msgType = MsgType.TxnBomSpecialTicketRequest;
        }

        public TXBOMSpecialTicketRequest(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnBomSpecialTicketRequest;
        }

        public override void Decode()
        {
            base.Decode();
            TxnType = GetNextString(2);
            DeviceId = GetNextString(8);

            OperatorId = GetNextString(6);
            ShiftId = GetNextString(10);

            TxnTime = GetNextString(14);

            StationId = GetNextString(4);

            TicketLogicalId = GetNextString(16);

            TicketMainType = GetNextString(2);

            TicketSubType = GetNextString(2);

            CredentialsType = GetNextString(1);

            CredentialsNumber = GetNextString(20);

            OriginalLogicalId = GetNextString(16);

            OriginalCardStatus = GetNextString(2);

            Spare = GetNextString(17);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            base.Decode();

            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(DeviceId, 8));

            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(ShiftId.PadLeft(10, '0'), 10));

            encodeBuf.AddRange(AddString(TxnTime, 14));

            encodeBuf.AddRange(AddString(StationId, 4));

            encodeBuf.AddRange(AddString(TicketLogicalId, 16));

            encodeBuf.AddRange(AddString(TicketMainType, 2));

            encodeBuf.AddRange(AddString(TicketSubType, 2));

            encodeBuf.AddRange(AddString(CredentialsType, 1));

            encodeBuf.AddRange(AddString(CredentialsNumber, 20));

            encodeBuf.AddRange(AddString(OriginalLogicalId, 16));

            encodeBuf.AddRange(AddString(OriginalCardStatus, 2));

            encodeBuf.AddRange(AddString(Spare.PadLeft(17, '0'), 17));
        }
    }
}