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
    public class StationTicketStockReportMessage : BaseMessage
    {
        public string LineId { get; set; }

        public string StationId { get; set; }

        public string TicketType { get; set; }

        public string TicketStatus { get; set; }

        public string TicketQty { get; set; }

        public string ChangeType { get; set; }

        public string TradeMoney { get; set; }

        public string ExpireDate { get; set; }

        public StationTicketStockReportMessage()
        {
            msgType = MsgType.StationTicketStockReport;
        }

        public StationTicketStockReportMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.StationTicketStockReport;
        }

        public override void Decode()
        {
            base.Decode();
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            TicketType = GetNextString(4);
            TicketStatus = GetNextString(1);
            TicketQty = GetNextString(8);
            ChangeType = GetNextString(1);
            TradeMoney = GetNextString(4);
            ExpireDate = GetNextString(8);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(TicketType, 4));
            encodeBuf.AddRange(AddString(TicketStatus, 1));
            encodeBuf.AddRange(AddString(TicketQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(ChangeType, 1));
            encodeBuf.AddRange(AddString(TradeMoney.PadLeft(4, '0'), 4));
            encodeBuf.AddRange(AddString(ExpireDate, 8));
        }
    }
}