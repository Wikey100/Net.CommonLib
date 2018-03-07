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
    public class NoteBoxAddMessage : BaseMessage
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
        /// 纸币1数量（5）
        /// </summary>
        public string Note1Qty { get; set; }

        /// <summary>
        /// 纸币1数量（5）
        /// </summary>
        public string Note2Qty { get; set; }

        /// <summary>
        /// 纸币1数量（5）
        /// </summary>
        public string Note3Qty { get; set; }

        /// <summary>
        /// 纸币1数量（5）
        /// </summary>
        public string Note4Qty { get; set; }

        /// <summary>
        /// 纸币1数量（5）
        /// </summary>
        public string Note5Qty { get; set; }

        /// <summary>
        /// 纸币1数量（5）
        /// </summary>
        public string Note6Qty { get; set; }

        /// <summary>
        /// 纸币1数量（5）
        /// </summary>
        public string Note7Qty { get; set; }

        /// <summary>
        /// 纸币补充时间(14)
        /// </summary>
        public string NoteAddTime { get; set; }

        public NoteBoxAddMessage()
        {
            msgType = MsgType.TvmNoteBoxAdd;
        }

        public NoteBoxAddMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TvmNoteBoxAdd;
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
            Note1Qty = GetNextString(5);
            Note2Qty = GetNextString(5);
            Note3Qty = GetNextString(5);
            Note4Qty = GetNextString(5);
            Note5Qty = GetNextString(5);
            Note6Qty = GetNextString(5);
            Note7Qty = GetNextString(5);
            NoteAddTime = GetNextString(14);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(TestFlag, 1));
            encodeBuf.AddRange(AddString(SerialNo, 3));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(CashBoxId, 10));
            encodeBuf.AddRange(AddString(Note1Qty, 5));
            encodeBuf.AddRange(AddString(Note2Qty, 5));
            encodeBuf.AddRange(AddString(Note3Qty, 5));
            encodeBuf.AddRange(AddString(Note4Qty, 5));
            encodeBuf.AddRange(AddString(Note5Qty, 5));
            encodeBuf.AddRange(AddString(Note6Qty, 5));
            encodeBuf.AddRange(AddString(Note7Qty, 5));
            encodeBuf.AddRange(AddString(NoteAddTime, 14));
        }
    }
}