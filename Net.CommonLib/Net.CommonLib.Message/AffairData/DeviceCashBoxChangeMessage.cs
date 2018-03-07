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
    public class DeviceCashBoxChangeMessage : BaseMessage
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
        /// 放入操作员ID(6)
        /// </summary>
        public string PutInOperatorId { get; set; }

        /// <summary>
        /// 取出操作员ID(6)
        /// </summary>
        public string GetOutOperatorId { get; set; }

        /// <summary>
        /// 放入时间
        /// </summary>
        public string PutInTime { get; set; }

        /// <summary>
        /// 取出时间
        /// </summary>
        public string GetOutTime { get; set; }

        /// <summary>
        /// 钱箱ID（16）
        /// </summary>
        public string CashBoxId { get; set; }

        /// <summary>
        /// 钱箱种类（1）
        /// </summary>
        public string CashBoxType { get; set; }

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
        /// 纸币1数量(5)
        /// </summary>
        public string Note1Qty { get; set; }

        /// <summary>
        /// 纸币2数量(5)
        /// </summary>
        public string Note2Qty { get; set; }

        /// <summary>
        /// 纸币3数量(5)
        /// </summary>
        public string Note3Qty { get; set; }

        /// <summary>
        /// 纸币4数量(5)
        /// </summary>
        public string Note4Qty { get; set; }

        /// <summary>
        /// 纸币5数量(5)
        /// </summary>
        public string Note5Qty { get; set; }

        /// <summary>
        /// 纸币6数量(5)
        /// </summary>
        public string Note6Qty { get; set; }

        /// <summary>
        /// 纸币7数量(5)
        /// </summary>
        public string Note7Qty { get; set; }

        /// <summary>
        /// 纸币8数量(5)
        /// </summary>
        public string Note8Qty { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public DeviceCashBoxChangeMessage()
        {
            msgType = MsgType.TvmCashBoxChange;
        }

        public DeviceCashBoxChangeMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TvmCashBoxChange;
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
            PutInOperatorId = GetNextString(6);
            GetOutOperatorId = GetNextString(6);
            PutInTime = GetNextString(14);
            GetOutTime = GetNextString(14);
            CashBoxId = GetNextString(10);
            CashBoxType = GetNextString(1);
            Coin1Qty = GetNextString(5);
            Coin2Qty = GetNextString(5);
            Coin3Qty = GetNextString(5);
            Note1Qty = GetNextString(5);
            Note2Qty = GetNextString(5);
            Note3Qty = GetNextString(5);
            Note4Qty = GetNextString(5);
            Note5Qty = GetNextString(5);
            Note6Qty = GetNextString(5);
            Note7Qty = GetNextString(5);
            Note8Qty = GetNextString(5);
            Spare = GetNextString(8);
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
            encodeBuf.AddRange(AddString(PutInOperatorId, 6));
            encodeBuf.AddRange(AddString(GetOutOperatorId, 6));
            encodeBuf.AddRange(AddString(PutInTime, 14));
            encodeBuf.AddRange(AddString(GetOutTime, 14));
            encodeBuf.AddRange(AddString(CashBoxId.PadLeft(10, '0'), 10));
            encodeBuf.AddRange(AddString(CashBoxType, 1));
            encodeBuf.AddRange(AddString(Coin1Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Coin2Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Coin3Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note1Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note2Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note3Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note4Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note5Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note6Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note7Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Note8Qty.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(Spare.PadLeft(8, '0'), 8));
        }
    }
}