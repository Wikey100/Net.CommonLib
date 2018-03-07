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
    public class CoinBoxAddMessage : BaseMessage
    {
        /// <summary>
        /// 交易类型 （2）
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 车站代码CHAR(4)
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 车站代码CHAR(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 运营日(8)
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 测试标志(1)
        /// </summary>
        public string TestFlag { get; set; }

        /// <summary>
        /// 测试标志(1)
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 操作员ID(6)
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 钱箱ID（10）
        /// </summary>
        public string CashBoxId { get; set; }

        /// <summary>
        /// 硬币1数量（5）
        /// </summary>
        public string Coin1Qty { get; set; }

        /// <summary>
        /// 硬币2数量(5)
        /// </summary>
        public string Coin2Qty { get; set; }

        /// <summary>
        /// 硬币3数量(5)
        /// </summary>
        public string Coin3Qty { get; set; }

        /// <summary>
        /// 硬币补充时间(14)
        /// </summary>
        public string CoinAddTime { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public CoinBoxAddMessage()
        {
            msgType = MsgType.TvmCoinBoxAdd;
        }

        public CoinBoxAddMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TvmCoinBoxAdd;
        }

        public override void Decode()
        {
            base.Decode();
            TxnType = GetNextString(2);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            ReportDate = GetNextString(8);
            TestFlag = GetNextString(1);
            SerialNo = GetNextString(3);
            OperatorId = GetNextString(6);
            CashBoxId = GetNextString(10);
            Coin1Qty = GetNextString(5);
            Coin2Qty = GetNextString(5);
            Coin3Qty = GetNextString(5);
            CoinAddTime = GetNextString(14);
            Spare = GetNextString(7);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(TestFlag, 1));
            encodeBuf.AddRange(AddString(SerialNo.PadLeft(3, '0'), 3));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(CashBoxId.PadLeft(10, '0'), 10));
            encodeBuf.AddRange(AddString(Coin1Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Coin2Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Coin3Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(CoinAddTime, 14));
            encodeBuf.AddRange(AddString(Spare.PadLeft(7, '0'), 7));
        }
    }
}