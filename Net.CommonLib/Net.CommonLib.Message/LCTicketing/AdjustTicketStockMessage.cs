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
    public class AdjustTicketDetail
    {
        public string TikcetType { get; set; }

        public string ValidAdjustQty { get; set; }

        public string InValidAdjustQty { get; set; }
    }

    public class AdjustTicketStockMessage : BaseMessage
    {
        /// <summary>
        /// 单据编号char(13)
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// 线路
        /// </summary>
        public string LineId { get; set; }

        public string StationId { get; set; }

        public string Remark { get; set; }

        public List<AdjustTicketDetail> Details = new List<AdjustTicketDetail>();

        public AdjustTicketStockMessage()
        {
            msgType = MsgType.AdjustTicketStock;
        }

        public AdjustTicketStockMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AdjustTicketStock;
        }

        public override void Decode()
        {
            base.Decode();
            NoteId = GetNextString(13);
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            Remark = GetNextUnicodeString(256);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                AdjustTicketDetail ms = new AdjustTicketDetail();
                ms.TikcetType = GetNextString(4);
                ms.ValidAdjustQty = GetNextString(8);
                ms.InValidAdjustQty = GetNextString(8);
                Details.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = Details.Count;

            encodeBuf.AddRange(AddString(NoteId, 13));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddUnicodeString(Remark));
            // encodeBuf.AddRange(AddUnicodeString(Remark, 256));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(3, '0'), 3));
            foreach (AdjustTicketDetail ms in Details)
            {
                encodeBuf.AddRange(AddString(ms.TikcetType, 4));
                encodeBuf.AddRange(AddString(ms.ValidAdjustQty.PadLeft(8, ' '), 8));
                encodeBuf.AddRange(AddString(ms.InValidAdjustQty.PadLeft(8, ' '), 8));
            }
        }
    }
}