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
    /// <summary>
    /// 预赋值明细
    /// </summary>
    public class PreInitTicketDetail
    {
        public string TicketType { get; set; }

        public string TicketName { get; set; }

        public string UnitPrice { get; set; }

        public string ReceiveQty { get; set; }

        public string ReturnQty { get; set; }

        public string CreateTime { get; set; }

        public string ReportDate { get; set; }

        public string ExpirationDate { get; set; }
    }

    /// <summary>
    /// 预赋值车票结算信息
    /// </summary>
    public class PreInitTicketSettleMessage : BaseMessage
    {
        public string LineId { get; set; }

        public string StationId { get; set; }

        public string ReceiveAmount { get; set; }

        public string OperatorId { get; set; }

        public List<PreInitTicketDetail> PreInitDetailList = new List<PreInitTicketDetail>();

        public PreInitTicketSettleMessage()
        {
            msgType = MsgType.PreInitTicketSettle;
        }

        public PreInitTicketSettleMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.PreInitTicketSettle;
        }

        public override void Decode()
        {
            base.Decode();
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            ReceiveAmount = GetNextString(8);
            OperatorId = GetNextString(6);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                PreInitTicketDetail ms = new PreInitTicketDetail();
                ms.TicketType = GetNextString(4);
                ms.TicketName = GetNextUnicodeString(20);
                ms.UnitPrice = GetNextString(8);
                ms.ReceiveQty = GetNextString(8);
                ms.ReturnQty = GetNextString(8);
                ms.CreateTime = GetNextString(14);
                ms.ReportDate = GetNextString(14);
                ms.ExpirationDate = GetNextString(14);
                PreInitDetailList.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = PreInitDetailList.Count;
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(ReceiveAmount.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(3, '0'), 3));
            foreach (PreInitTicketDetail ms in PreInitDetailList)
            {
                encodeBuf.AddRange(AddString(ms.TicketType, 4));
                encodeBuf.AddRange(AddUnicodeString(ms.TicketName, 20));
                encodeBuf.AddRange(AddString(ms.UnitPrice.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(ms.ReceiveQty.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(ms.ReturnQty.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(ms.CreateTime, 14));
                encodeBuf.AddRange(AddString(ms.ReportDate, 14));
                encodeBuf.AddRange(AddString(ms.ExpirationDate, 14));
            }
        }
    }
}