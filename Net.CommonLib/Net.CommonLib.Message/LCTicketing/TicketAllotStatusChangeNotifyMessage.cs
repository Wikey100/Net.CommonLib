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
    public class TicketAllotStatusChangeNotifyMessage : BaseMessage
    {
        /// <summary>
        /// 单据编号char(13)
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public string LineId { get; set; }

        public string Result { get; set; }

        public TicketAllotStatusChangeNotifyMessage()
        {
            msgType = MsgType.TicketAllotStatusChangeNotify;
        }

        public TicketAllotStatusChangeNotifyMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TicketAllotStatusChangeNotify;
        }

        public override void Decode()
        {
            base.Decode();
            NoteId = GetNextString(13);
            LineId = GetNextString(2);
            Result = GetNextString(1);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(NoteId, 13));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(Result, 1));
        }
    }
}