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
    public class LineClockExceptionReport : BaseMessage
    {
        public string MessageCode { get; set; }

        public string LineId { get; set; }

        public string CurrentTime { get; set; }

        public string DiffType { get; set; }

        public string DiffTime { get; set; }

        public LineClockExceptionReport()
        {
            msgType = MsgType.AccClockExceptionReport;
        }

        public LineClockExceptionReport(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AccClockExceptionReport;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            LineId = GetNextString(2);
            CurrentTime = GetNextString(14);
            DiffType = GetNextString(1);
            DiffTime = GetNextString(5);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(CurrentTime, 14));
            encodeBuf.AddRange(AddString(DiffType, 1));
            encodeBuf.AddRange(AddString(DiffTime.PadLeft(5, '0'), 5));
        }
    }
}