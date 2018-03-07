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
    public class AccClockSyncCommand : BaseMessage
    {
        public string SendTime { get; set; }
        public string MessageCode { get; set; }

        public AccClockSyncCommand()
        {
            msgType = MsgType.AccClockSyncCommand;
        }

        public AccClockSyncCommand(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AccClockSyncCommand;
        }

        public override void Decode()
        {
            base.Decode();
            SendTime = GetNextString(14);
            MessageCode = GetNextString(4);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(SendTime, 14));
            encodeBuf.AddRange(AddString(MessageCode, 4));
        }
    }
}