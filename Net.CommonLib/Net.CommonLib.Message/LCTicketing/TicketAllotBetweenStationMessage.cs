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
    /// 车票调拨单
    /// </summary>
    public class TicketAllotBetweenStationMessage : BaseMessage
    {
        /// <summary>
        /// 单据编号char(13)
        /// </summary>
        public string NoteId { get; set; }

        public string RefApplyId { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string OutStationId { get; set; }

        public string InStationId { get; set; }

        public string OperatorId { get; set; }

        public string OperateTime { get; set; }

        public string Remark { get; set; }

        public List<TicketAllotDetail> ApplyDetailList = new List<TicketAllotDetail>();

        public TicketAllotBetweenStationMessage()
        {
            msgType = MsgType.TicketAllotBetweenStation;
        }

        public TicketAllotBetweenStationMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TicketAllotBetweenStation;
        }

        public override void Decode()
        {
            base.Decode();
            NoteId = GetNextString(13);
            RefApplyId = GetNextString(13);
            LineId = GetNextString(2);
            OutStationId = GetNextString(4);
            InStationId = GetNextString(4);
            OperatorId = GetNextString(6);
            OperateTime = GetNextString(14);
            Remark = GetNextUnicodeString(256);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                TicketAllotDetail ms = new TicketAllotDetail();
                ms.SeqID = GetNextString(4);
                ms.CardType = GetNextString(2);
                ms.CardTypeEx = GetNextString(2);
                ms.CardStatus = GetNextString(2);
                ms.CardStyle = GetNextString(2);
                ms.TradeMoney = GetNextString(8);
                ms.ExpirationDate = GetNextString(14);
                ms.TicketNum = GetNextString(8);
                ms.Remarks = GetNextUnicodeString(256);
                ApplyDetailList.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = ApplyDetailList.Count;

            encodeBuf.AddRange(AddString(NoteId, 13));
            encodeBuf.AddRange(AddString(RefApplyId.PadLeft(13, ' '), 13));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(OutStationId, 4));
            encodeBuf.AddRange(AddString(InStationId, 4));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(OperateTime, 14));
            encodeBuf.AddRange(AddUnicodeString(Remark.PadLeft(256, ' '), 256));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(3, '0'), 3));
            foreach (TicketAllotDetail ms in ApplyDetailList)
            {
                encodeBuf.AddRange(AddString(ms.SeqID, 4));
                encodeBuf.AddRange(AddString(ms.CardType, 2));
                encodeBuf.AddRange(AddString(ms.CardTypeEx, 2));
                encodeBuf.AddRange(AddString(ms.CardStatus.PadLeft(2, '0'), 2));
                encodeBuf.AddRange(AddString(ms.CardStyle.PadLeft(2, '0'), 2));
                encodeBuf.AddRange(AddString(ms.TradeMoney.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(ms.ExpirationDate, 14));
                encodeBuf.AddRange(AddString(ms.TicketNum.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddUnicodeString(ms.Remarks.PadLeft(256, ' '), 256));
            }
        }
    }
}