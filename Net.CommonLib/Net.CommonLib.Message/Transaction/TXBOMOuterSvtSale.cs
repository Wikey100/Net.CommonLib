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
    /// BOM 上SVT发售(外卡）
    /// </summary>
    public class TXBOMOuterSvtSale : BaseMessage
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
        /// 进入车站
        /// </summary>

        public string EntryStationId { get; set; }

        /// <summary>
        /// 票价区域代码
        /// </summary>
        public string TicketPriceCode { get; set; }

        /// <summary>
        /// 降级模式
        /// </summary>
        public string DegradeMode { get; set; }

        /// <summary>
        /// 交易总金额
        /// </summary>
        public string TxnTotalAmt { get; set; }

        /// <summary>
        /// 支付代码
        /// </summary>
        public string PayCode { get; set; }

        /// <summary>
        /// 票卡押金
        /// </summary>
        public string TicketDepositAmt { get; set; }

        /// <summary>
        /// 交易手续费
        /// </summary>
        public string TxnProcessingFee { get; set; }

        /// <summary>
        /// 交易原因代码
        /// </summary>
        public string TxnReasonCode { get; set; }

        /// <summary>
        /// 交易原因代码
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 班次序号
        /// </summary>
        public string ShiftId { get; set; }

        /// <summary>
        /// 交易钱包标示
        /// </summary>
        public string PurseFlag { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        //一卡通交易数据格式 TAB分割

        /// <summary>
        /// 终端编号
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// 终端标志
        /// </summary>
        public string TerminalFlag { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TxnTime { get; set; }

        /// <summary>
        /// 交易终端序列号
        /// </summary>
        public string TerminalSerialNumber { get; set; }

        /// <summary>
        /// 票卡逻辑卡号
        /// </summary>
        public string TicketLogicalId { get; set; }

        /// <summary>
        /// 票卡物理卡号
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
        /// 上次交易终端编号
        /// </summary>
        public string LastTerminalId { get; set; }

        /// <summary>
        /// 上次交易时间
        /// </summary>
        public string LastTxnTime { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public string TxnAmt { get; set; }

        /// <summary>
        /// 交易余额
        /// </summary>
        public string RemainAmt { get; set; }

        /// <summary>
        /// 交易类型 由支付方式代码（高位）和交易分类代码（低位）组成
        /// </summary>
        public string TxnAndPayType { get; set; }

        /// <summary>
        /// 本次终端入口编号
        /// </summary>
        public string ThisEntryTerminalId { get; set; }

        /// <summary>
        /// 本次入口日期
        /// </summary>
        public string ThisEntryDateTime { get; set; }

        /// <summary>
        /// 票卡联机计数
        /// </summary>
        public string TicketOnlineCounter { get; set; }

        /// <summary>
        /// 票卡联机计数
        /// </summary>
        public string TicketOfflineCounter { get; set; }

        /// <summary>
        /// 交易验证码
        /// </summary>
        public string TACCode { get; set; }

        /// <summary>
        /// 测试标志
        /// </summary>
        public string TestFlag { get; set; }

        /// <summary>
        /// ykt预留
        /// </summary>
        public string YKTSpare { get; set; }

        public TXBOMOuterSvtSale()
        {
            msgType = MsgType.TxnBomOuterSvtSale;
        }

        public TXBOMOuterSvtSale(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TxnBomOuterSvtSale;
        }

        public override void Decode()
        {
            base.Decode();
            DeviceId = GetNextString(8);
            TxnGroupNumber = GetNextString(2);
            SerialNumber = GetNextString(8);
            StationId = GetNextString(4);
            TxnType = GetNextString(2);
            EntryStationId = GetNextString(4);
            TicketPriceCode = GetNextString(2);
            DegradeMode = GetNextString(4);
            TxnTotalAmt = GetNextString(8);
            PayCode = GetNextString(1);
            TicketDepositAmt = GetNextString(8);
            TxnProcessingFee = GetNextString(8);
            TxnReasonCode = GetNextString(3);
            OperatorId = GetNextString(6);
            ShiftId = GetNextString(10);
            PurseFlag = GetNextString(1);
            Spare = GetNextString(1);
            //一卡通数据
            GetNextString(1);
            TerminalId = GetNextString(16);
            GetNextString(1);
            TerminalFlag = GetNextString(1);
            GetNextString(1);
            TxnTime = GetNextString(14);
            GetNextString(1);
            TerminalSerialNumber = GetNextString(8);
            GetNextString(1);
            TicketLogicalId = GetNextString(16);
            GetNextString(1);
            TicketPhysicalId = GetNextString(8);
            GetNextString(1);
            TicketMainType = GetNextString(2);
            GetNextString(1);
            TicketSubType = GetNextString(2);
            GetNextString(1);
            LastTerminalId = GetNextString(16);
            GetNextString(1);
            LastTxnTime = GetNextString(14);
            GetNextString(1);
            TxnAmt = GetNextString(8);
            GetNextString(1);
            RemainAmt = GetNextString(8);
            GetNextString(1);
            TxnAndPayType = GetNextString(2);
            GetNextString(1);
            ThisEntryTerminalId = GetNextString(16);
            GetNextString(1);
            ThisEntryDateTime = GetNextString(14);
            GetNextString(1);
            TicketOnlineCounter = GetNextString(6);
            GetNextString(1);
            TicketOfflineCounter = GetNextString(6);
            GetNextString(1);
            TACCode = GetNextString(8);
            GetNextString(1);
            TestFlag = GetNextString(1);
            GetNextString(1);
            YKTSpare = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(TxnGroupNumber.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(SerialNumber.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(EntryStationId, 4));
            encodeBuf.AddRange(AddString(TicketPriceCode, 2));
            encodeBuf.AddRange(AddString(DegradeMode, 4));
            encodeBuf.AddRange(AddString(TxnTotalAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(PayCode, 1));
            encodeBuf.AddRange(AddString(TicketDepositAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(TxnProcessingFee.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(TxnReasonCode, 3));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(ShiftId.PadLeft(10, '0'), 10));
            encodeBuf.AddRange(AddString(PurseFlag, 1));
            encodeBuf.AddRange(AddString(Spare, 1));
            //一卡通数据 tab 分割
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TerminalId, 16));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TerminalFlag, 1));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TxnTime, 14));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TerminalSerialNumber.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketLogicalId, 16));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketPhysicalId, 8));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketMainType, 2));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketSubType, 2));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(LastTerminalId, 16));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(LastTxnTime, 14));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TxnAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(RemainAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TxnAndPayType, 2));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(ThisEntryTerminalId, 16));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(ThisEntryDateTime, 14));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketOnlineCounter.PadLeft(6, '0'), 6));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TicketOfflineCounter.PadLeft(6, '0'), 6));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TACCode, 8));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(TestFlag, 1));
            encodeBuf.AddRange(AddString("\t", 1));
            encodeBuf.AddRange(AddString(YKTSpare.PadLeft(16, '0'), 16));
        }
    }
}