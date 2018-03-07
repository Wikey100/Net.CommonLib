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

namespace Net.CommonLib.Message.Ticketing
{
    public class TicketStockDetail
    {
        /// <summary>
        /// CHAR（4） 票卡主类型
        /// </summary>
        public string TicketType { get; set; }

        /// <summary>
        /// CHAR(1) 票卡状态
        /// </summary>
        public string TicketStatus { get; set; }

        /// <summary>
        /// 金额 CHAR(8) 分
        /// </summary>
        public string TradeMoney { get; set; }

        /// <summary>
        /// 有效期 CHAR（8）
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 票卡数量 CHAR(8)
        /// </summary>
        public string TicketAmount { get; set; }
    }

    public class TicketStockReportMessage : BaseMessage
    {
        /// <summary>
        /// 线路 CHAR（2）
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 车站(4)
        /// </summary>
        public string StationId { get; set; }

        public List<TicketStockDetail> TicketStockDetailList = new List<TicketStockDetail>();

        public TicketStockReportMessage()
        {
            msgType = MsgType.TicketingTicketStockReport;
        }

        public TicketStockReportMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TicketingTicketStockReport;
        }

        public override void Decode()
        {
            base.Decode();
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                TicketStockDetail data = new TicketStockDetail();
                data.TicketType = GetNextString(4);
                data.TicketStatus = GetNextString(1);
                data.TradeMoney = GetNextString(8);
                data.ExpireDate = GetNextString(8);
                data.TicketAmount = GetNextString(8);
                TicketStockDetailList.Add(data);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(TicketStockDetailList.Count.ToString().PadLeft(3, '0'), 3));
            foreach (TicketStockDetail item in TicketStockDetailList)
            {
                encodeBuf.AddRange(AddString(item.TicketType, 4));
                encodeBuf.AddRange(AddString(item.TicketStatus, 1));
                encodeBuf.AddRange(AddString(item.TradeMoney.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(item.ExpireDate, 8));
                encodeBuf.AddRange(AddString(item.TicketAmount.PadLeft(8, '0'), 8));
            }
        }
    }
}