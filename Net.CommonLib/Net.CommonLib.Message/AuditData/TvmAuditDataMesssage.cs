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

namespace Net.CommonLib.Message
{
    public class TvmSaleData
    {
        /// <summary>
        /// 票卡主类型 （2）
        /// </summary>
        public string TicketMainType { get; set; }

        /// <summary>
        /// 票卡子类型 （2）
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 储值票(5)
        /// </summary>
        public string SvtCount { get; set; }

        /// <summary>
        /// 单程票统计（5）
        /// </summary>
        public string SjtCount { get; set; }

        /// <summary>
        /// 购票总金额(8)
        /// </summary>
        public string SaleTotalAmt { get; set; }
    }

    public class TvmChargeData
    {
        /// <summary>
        /// 票卡主类型 （2）
        /// </summary>
        public string TicketMainType { get; set; }

        /// <summary>
        /// 票卡子类型 （2）
        /// </summary>
        public string TicketSubType { get; set; }

        /// <summary>
        /// 充值总数量(5)
        /// </summary>
        public string ChargeCount { get; set; }

        /// <summary>
        /// 充值总金额(8)
        /// </summary>
        public string ChargeTotalAmt { get; set; }
    }

    public class TvmAuditDataMessage : BaseMessage
    {
        /// <summary>
        /// 审计类型(1) 2
        /// </summary>
        public string AuditType { get; set; }

        /// <summary>
        /// 发生交易车站(4)
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 设备交易类型(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 运营日 CHAR(8)
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 发生时间(14)
        /// </summary>
        public string TvmTxnTime { get; set; }

        /// <summary>
        /// 本日累计补充硬币次数（2）
        /// </summary>
        public string TodayAddCoinTimes { get; set; }

        /// <summary>
        /// 本日累计补充硬币1数量(5五毛)（5）
        /// </summary>
        public string TodayAddCoin1Count { get; set; }

        /// <summary>
        /// 本日累计补充硬币2数量(一元)（5）
        /// </summary>
        public string TodayAddCoin2Count { get; set; }

        /// <summary>
        /// 本日累计补充硬币3数量(预留)（5）
        /// </summary>
        public string TodayAddCoin3Count { get; set; }

        /// <summary>
        /// 本日累计接收硬币1数量(5五毛)（5）
        /// </summary>
        public string TodayAcceptCoin1Count { get; set; }

        /// <summary>
        /// 本日累计接收硬币2数量(一元)（5）
        /// </summary>
        public string TodayAcceptCoin2Count { get; set; }

        /// <summary>
        /// 本日累计接收硬币3数量(预留)（5）
        /// </summary>
        public string TodayAcceptCoin3Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币1数量(一元)（5）
        /// </summary>
        public string TodayAcceptNote1Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币2数量(2元)（5）
        /// </summary>
        public string TodayAcceptNote2Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币3数量(5元)（5）
        /// </summary>
        public string TodayAcceptNote3Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币4数量(10元)（5）
        /// </summary>
        public string TodayAcceptNote4Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币5数量(20元)（5）
        /// </summary>
        public string TodayAcceptNote5Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币6数量(50元)（5）
        /// </summary>
        public string TodayAcceptNote6Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币7数量(100元)（5）
        /// </summary>
        public string TodayAcceptNote7Count { get; set; }

        /// <summary>
        /// 本日累计接收纸币8数量(预留)（5）
        /// </summary>
        public string TodayAcceptNote8Count { get; set; }

        /// <summary>
        /// 硬币钱箱更换数量 (2)
        /// </summary>
        public string TodayChangeCoinBoxTimes { get; set; }

        /// <summary>
        /// 纸币钱箱更换次数（2）
        /// </summary>
        public string TodayChangeNoteBoxTimes { get; set; }

        /// <summary>
        /// 单日废票数量 (5)
        /// </summary>
        public string TodayBadTicketCount { get; set; }

        /// <summary>
        /// 现金购买单程票数量 （5）
        /// </summary>
        public string CashSjtCount { get; set; }

        /// <summary>
        /// 现金购买单程票金额 （8）
        /// </summary>
        public string CashSjtAmt { get; set; }

        public List<TvmSaleData> TvmSaleDataList = new List<TvmSaleData>();

        public List<TvmChargeData> TvmChargeDataList = new List<TvmChargeData>();

        public TvmAuditDataMessage()
        {
            msgType = MsgType.AuditTvmTxnAuditData;
        }

        public TvmAuditDataMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AuditTvmTxnAuditData;
        }

        public override void Decode()
        {
            base.Decode();
            AuditType = GetNextString(1);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            ReportDate = GetNextString(8);
            TvmTxnTime = GetNextString(14);
            TodayAddCoinTimes = GetNextString(2);
            TodayAddCoin1Count = GetNextString(5);
            TodayAddCoin2Count = GetNextString(5);
            TodayAddCoin3Count = GetNextString(5);
            TodayAcceptCoin1Count = GetNextString(5);
            TodayAcceptCoin2Count = GetNextString(5);
            TodayAcceptCoin3Count = GetNextString(5);
            TodayAcceptNote1Count = GetNextString(5);
            TodayAcceptNote2Count = GetNextString(5);
            TodayAcceptNote3Count = GetNextString(5);
            TodayAcceptNote4Count = GetNextString(5);
            TodayAcceptNote5Count = GetNextString(5);
            TodayAcceptNote6Count = GetNextString(5);
            TodayAcceptNote7Count = GetNextString(5);
            TodayAcceptNote8Count = GetNextString(5);
            TodayChangeCoinBoxTimes = GetNextString(2);
            TodayChangeNoteBoxTimes = GetNextString(2);
            TodayBadTicketCount = GetNextString(5);
            CashSjtCount = GetNextString(5);
            CashSjtAmt = GetNextString(8);

            int count1 = Convert.ToInt32(GetNextString(2));
            int count2 = Convert.ToInt32(GetNextString(2));
            for (int i = 0; i < count1; i++)
            {
                TvmSaleData data = new TvmSaleData();
                data.TicketMainType = GetNextString(2);
                data.TicketSubType = GetNextString(2);
                data.SvtCount = GetNextString(5);
                data.SjtCount = GetNextString(5);
                data.SaleTotalAmt = GetNextString(8);
                TvmSaleDataList.Add(data);
            }

            for (int i = 0; i < count2; i++)
            {
                TvmChargeData data = new TvmChargeData();
                data.TicketMainType = GetNextString(2);
                data.TicketSubType = GetNextString(2);
                data.ChargeCount = GetNextString(5);
                data.ChargeTotalAmt = GetNextString(8);
                TvmChargeDataList.Add(data);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(AuditType, 1));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(TvmTxnTime, 14));
            encodeBuf.AddRange(AddString(TodayAddCoinTimes.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(TodayAddCoin1Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAddCoin2Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAddCoin3Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptCoin1Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptCoin2Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptCoin3Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote1Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote2Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote3Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote4Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote5Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote6Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote7Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayAcceptNote8Count.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(TodayChangeCoinBoxTimes.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(TodayChangeNoteBoxTimes.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(TodayBadTicketCount.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(CashSjtCount.PadLeft(5, '0'), 5));
            encodeBuf.AddRange(AddString(CashSjtAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(TvmSaleDataList.Count.ToString().PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(TvmChargeDataList.Count.ToString().PadLeft(2, '0'), 2));

            foreach (TvmSaleData item in TvmSaleDataList)
            {
                encodeBuf.AddRange(AddString(item.TicketMainType, 2));
                encodeBuf.AddRange(AddString(item.TicketSubType, 2));
                encodeBuf.AddRange(AddString(item.SvtCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.SjtCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.SaleTotalAmt.PadLeft(8, '0'), 8));
            }

            foreach (TvmChargeData item in TvmChargeDataList)
            {
                encodeBuf.AddRange(AddString(item.TicketMainType, 2));
                encodeBuf.AddRange(AddString(item.TicketSubType, 2));
                encodeBuf.AddRange(AddString(item.ChargeCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.ChargeTotalAmt.PadLeft(8, '0'), 8));
            }
        }
    }
}