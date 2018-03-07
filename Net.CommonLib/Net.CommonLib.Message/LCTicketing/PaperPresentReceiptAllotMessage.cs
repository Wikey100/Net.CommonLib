/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Message.LCTicketing
{
    public class PaperPresentReceiptAllotMessage : BaseMessage
    {
        /// <summary>
        /// 单据编号char(13)
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationId { get; set; }

        public string OperateTime { get; set; }

        public string TicketType { get; set; }

        public string BeginNumber { get; set; }

        public string EndNumber { get; set; }

        public string Unit { get; set; }

        public PaperPresentReceiptAllotMessage()
        {
            msgType = MsgType.PaperPresentReceiptAllot;
        }

        public PaperPresentReceiptAllotMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.PaperPresentReceiptAllot;
        }

        public override void Decode()
        {
            base.Decode();
            NoteId = GetNextString(13);
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            OperateTime = GetNextString(14);
            TicketType = GetNextString(4);
            BeginNumber = GetNextString(8);
            EndNumber = GetNextString(8);
            Unit = GetNextString(8);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(NoteId, 13));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(OperateTime, 14));
            encodeBuf.AddRange(AddString(TicketType, 4));
            encodeBuf.AddRange(AddString(BeginNumber.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(EndNumber.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(Unit.PadLeft(8, '0'), 8));
        }
    }
}