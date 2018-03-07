/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Collections.Generic;

namespace Net.CommonLib.Message.LCTicketing
{
    public class InventoryTicketDetail
    {
        public string TikcetType { get; set; }

        public string ValidSystemQty { get; set; }

        public string ValidRealQty { get; set; }

        public string InValidSystemQty { get; set; }

        public string InValidRealQty { get; set; }
    }

    public class InventoryPaperDetail
    {
        public string TikcetType { get; set; }

        public string UnitPrice { get; set; }

        public string RealQty { get; set; }

        public string BeginNumber { get; set; }

        public string EndNumber { get; set; }
    }

    public class StockInventoryMessage : BaseMessage
    {
        /// <summary>
        /// 单据编号char(13)
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// 台账编号
        /// </summary>
        public string StandingBookId { get; set; }

        /// <summary>
        /// 线路
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 车站
        /// </summary>

        public string StationId { get; set; }

        public string ReportDate { get; set; }

        public string InventoryTime { get; set; }

        public string SystemSpareCash { get; set; }

        /// <summary>
        /// 实际备用金
        /// </summary>
        public string RealSpareCash { get; set; }

        /// <summary>
        /// 值班站长
        /// </summary>

        public string ConfirmOperatorId { get; set; }

        public string OperatorId { get; set; }

        public string Remark { get; set; }

        public List<InventoryTicketDetail> TicketDetails = new List<InventoryTicketDetail>();

        public List<InventoryPaperDetail> PaperDetails = new List<InventoryPaperDetail>();

        public StockInventoryMessage()
        {
            msgType = MsgType.StockInventory;
        }

        public StockInventoryMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.StockInventory;
        }

        public override void Decode()
        {
            base.Decode();

            NoteId = GetNextString(13);
            StandingBookId = GetNextUnicodeString();
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            ReportDate = GetNextString(14);
            InventoryTime = GetNextString(14);
            SystemSpareCash = GetNextString(10);
            RealSpareCash = GetNextString(10);
            ConfirmOperatorId = GetNextString(6);
            OperatorId = GetNextString(6);
            Remark = GetNextUnicodeString();
            int count1 = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count1; i++)
            {
                InventoryTicketDetail ms = new InventoryTicketDetail();
                ms.TikcetType = GetNextString(4);
                ms.ValidSystemQty = GetNextString(10);
                ms.ValidRealQty = GetNextString(10);
                ms.InValidSystemQty = GetNextString(10);
                ms.InValidRealQty = GetNextString(10);
                TicketDetails.Add(ms);
            }
            int count2 = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count2; i++)
            {
                InventoryPaperDetail ms = new InventoryPaperDetail();
                ms.TikcetType = GetNextString(4);
                ms.UnitPrice = GetNextString(8);
                ms.RealQty = GetNextString(10);
                ms.BeginNumber = GetNextString(10);
                ms.EndNumber = GetNextString(10);
                PaperDetails.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count1 = TicketDetails.Count;
            int count2 = PaperDetails.Count;

            encodeBuf.AddRange(AddString(NoteId, 13));
            encodeBuf.AddRange(AddUnicodeString(StandingBookId));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(ReportDate, 14));
            encodeBuf.AddRange(AddString(InventoryTime, 14));
            encodeBuf.AddRange(AddString(SystemSpareCash, 10));
            encodeBuf.AddRange(AddString(RealSpareCash, 10));
            encodeBuf.AddRange(AddString(ConfirmOperatorId, 6));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddUnicodeString(Remark));
            encodeBuf.AddRange(AddString(count1.ToString().PadLeft(3, '0'), 3));
            foreach (InventoryTicketDetail ms in TicketDetails)
            {
                encodeBuf.AddRange(AddString(ms.TikcetType, 4));
                encodeBuf.AddRange(AddString(ms.ValidSystemQty.PadLeft(10, '0'), 10));
                encodeBuf.AddRange(AddString(ms.ValidRealQty.PadLeft(10, '0'), 10));
                encodeBuf.AddRange(AddString(ms.InValidSystemQty.PadLeft(10, '0'), 10));
                encodeBuf.AddRange(AddString(ms.InValidRealQty.PadLeft(10, '0'), 10));
            }
            encodeBuf.AddRange(AddString(count2.ToString().PadLeft(3, '0'), 3));

            foreach (InventoryPaperDetail ms in PaperDetails)
            {
                encodeBuf.AddRange(AddString(ms.TikcetType, 4));
                encodeBuf.AddRange(AddString(ms.UnitPrice.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(ms.RealQty.PadLeft(10, '0'), 10));
                encodeBuf.AddRange(AddString(ms.BeginNumber.PadLeft(10, '0'), 10));
                encodeBuf.AddRange(AddString(ms.EndNumber.PadLeft(10, '0'), 10));
            }
        }
    }
}