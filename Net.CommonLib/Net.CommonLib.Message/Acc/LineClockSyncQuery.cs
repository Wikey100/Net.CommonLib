/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Message.Acc
{
    public class LineClockSyncQuery : BaseMessage
    {
        public string MessageCode { get; set; }

        public LineClockSyncQuery()
        {
            msgType = MsgType.AccLineClockSyncQuery;
        }

        public LineClockSyncQuery(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AccLineClockSyncQuery;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(MessageCode, 4));
        }
    }
}