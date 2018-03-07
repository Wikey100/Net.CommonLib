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
    /// <summary>
    /// /撤销车票配发报文
    /// </summary>
    public class TicketDispatchRepealMessage : BaseMessage
    {
        /// <summary>
        /// 单据编号char(13)
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public string LineId { get; set; }

        public string StationId { get; set; }

        public string Result { get; set; }

        public TicketDispatchRepealMessage()
        {
            msgType = MsgType.TicketDispatchRepeal;
        }

        public TicketDispatchRepealMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TicketDispatchRepeal;
        }

        public override void Decode()
        {
            base.Decode();
            NoteId = GetNextString(13);
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            Result = GetNextString(1);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(NoteId, 13));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(Result, 1));
        }
    }
}