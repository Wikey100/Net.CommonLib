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
    public class ReplenishmentConfirmMessage : BaseMessage
    {
        public string LineId { get; set; }

        public string StationId { get; set; }

        public string OperatorId { get; set; }

        public string ReportDate { get; set; }

        public string Status { get; set; }

        public ReplenishmentConfirmMessage()
        {
            msgType = MsgType.ReplennishmentConfirm;
        }

        public ReplenishmentConfirmMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.ReplennishmentConfirm;
        }

        public override void Decode()
        {
            base.Decode();
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            OperatorId = GetNextString(6);
            ReportDate = GetNextString(8);
            Status = GetNextString(2);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(Status.PadLeft(2, '0'), 2));
        }
    }
}