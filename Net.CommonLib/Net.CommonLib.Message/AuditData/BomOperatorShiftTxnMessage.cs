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
    public class TicketStatData
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
        /// 车票总数量(5)
        /// </summary>
        public string TicketCount { get; set; }

        /// <summary>
        /// 废票数量（5）
        /// </summary>
        public string BadTicketCount { get; set; }

        /// <summary>
        /// 退票数量(5)
        /// </summary>
        public string RefundTicketCount { get; set; }

        /// <summary>
        /// 黑名单票数量(5)
        /// </summary>
        public string BlackTicketCount { get; set; }
    }

    public class BomTxnData
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
        /// 交易类型(2)
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 支付类型（1）
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 交易数量总计(5)
        /// </summary>
        public string TxnTotalCount { get; set; }

        /// <summary>
        /// 交易现金总额(8)
        /// </summary>
        public string CashTotalAmt { get; set; }

        /// <summary>
        /// 卡交易总额(8)
        /// </summary>
        public string CardTotalAmt { get; set; }
    }

    public class BomAdminData
    {
        /// <summary>
        /// 票卡主类型 （2）
        /// </summary>
        public string AdminCode { get; set; }

        /// <summary>
        /// 票卡子类型 （2）
        /// </summary>
        public string InOutFlag { get; set; }

        /// <summary>
        /// 交易类型(2)
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 交易数量小计(5)
        /// </summary>
        public string TxnTotalCount { get; set; }

        /// <summary>
        /// 退款金额(8)
        /// </summary>
        public string RefundAmt { get; set; }

        /// <summary>
        /// 现金交易总额(8)
        /// </summary>
        public string CashTotalAmt { get; set; }

        /// <summary>
        /// 卡交易总额(8)
        /// </summary>
        public string CardTotalAmt { get; set; }

        /// <summary>
        /// 优惠兑现总额(8)
        /// </summary>
        public string FavorTotalAmt { get; set; }
    }

    public class LossHandleData
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
        /// 交易类型(2)
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 交易数量小计(1)
        /// </summary>
        public string LossFlag { get; set; }

        /// <summary>
        /// 交易数量小计(5)
        /// </summary>
        public string TxnCount { get; set; }

        /// <summary>
        /// 现金收益总额(8)
        /// </summary>
        public string CashTotalAmt { get; set; }
    }

    public class RefundHandleData
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
        /// 交易类型(2)
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 交易数量小计(1)
        /// </summary>
        public string RefundFlag { get; set; }

        /// <summary>
        /// 交易数量小计(5)
        /// </summary>
        public string TxnCount { get; set; }

        /// <summary>
        /// 现金收益总额(8)
        /// </summary>
        public string CashTotalAmt { get; set; }
    }

    public class GroupTicketData
    {
        /// <summary>
        /// 票卡主类型 （4）
        /// </summary>
        public string RedeemTimes { get; set; }

        /// <summary>
        /// 票卡子类型 （10）
        /// </summary>
        public string RedeemCount { get; set; }

        /// <summary>
        /// 交易类型(4)
        /// </summary>
        public string GroupTicketHandleTimes { get; set; }

        /// <summary>
        /// 交易数量小计(6)
        /// </summary>
        public string GroupTicketUseCount { get; set; }

        /// <summary>
        /// 现金收益总额(10)
        /// </summary>
        public string CashTotalAmt { get; set; }
    }

    public class BomOperatorShiftTxnMessage : BaseMessage
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
        /// 操作员ID CHAR(6)
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 班次Id CHAR(10)
        /// </summary>
        public string ShiftSeqId { get; set; }

        /// <summary>
        /// 审计产生时间(14)
        /// </summary>
        public string AuditDataCreateTime { get; set; }

        /// <summary>
        /// 班次开始时间(14)
        /// </summary>
        public string ShiftStartTime { get; set; }

        /// <summary>
        /// 班次结束时间(14)
        /// </summary>
        public string ShiftEndTime { get; set; }

        public List<TicketStatData> TicketStatDataList = new List<TicketStatData>();

        public List<BomTxnData> BomTxnDataList = new List<BomTxnData>();

        public List<BomAdminData> BomAdminDataList = new List<BomAdminData>();

        public List<LossHandleData> LossHandleDataList = new List<LossHandleData>();

        public List<RefundHandleData> RefundHandleDataList = new List<RefundHandleData>();

        public List<GroupTicketData> GroupTicketDataList = new List<GroupTicketData>();

        /// <summary>
        /// 班次结束时间(14)
        /// </summary>
        public string TxnHandleType { get; set; }

        /// <summary>
        /// 班次结束时间(14)
        /// </summary>
        public string AdminHandleType { get; set; }

        public BomOperatorShiftTxnMessage()
        {
            msgType = MsgType.AuditBomOperatorShiftData;
        }

        public BomOperatorShiftTxnMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AuditBomOperatorShiftData;
        }

        public override void Decode()
        {
            base.Decode();
            AuditType = GetNextString(1);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            ReportDate = GetNextString(8);
            OperatorId = GetNextString(6);
            ShiftSeqId = GetNextString(10);
            AuditDataCreateTime = GetNextString(14);
            ShiftStartTime = GetNextString(14);
            ShiftEndTime = GetNextString(14);

            int ticketStatCount = Convert.ToInt32(GetNextString(2).Trim());
            int txnDataCount = Convert.ToInt32(GetNextString(2).Trim());
            int adminCount = Convert.ToInt32(GetNextString(2).Trim());
            int lossCount = Convert.ToInt32(GetNextString(2).Trim());
            int refundCount = Convert.ToInt32(GetNextString(2).Trim());
            TxnHandleType = GetNextString(2).Trim();
            AdminHandleType = GetNextString(2).Trim();

            for (int i = 0; i < ticketStatCount; i++)
            {
                TicketStatData data = new TicketStatData();
                data.TicketMainType = GetNextString(2);
                data.TicketSubType = GetNextString(2);
                data.TicketCount = GetNextString(5).Trim();
                data.BadTicketCount = GetNextString(5).Trim();
                data.RefundTicketCount = GetNextString(5).Trim();
                data.BlackTicketCount = GetNextString(5).Trim();
                TicketStatDataList.Add(data);
            }

            for (int i = 0; i < txnDataCount; i++)
            {
                BomTxnData data = new BomTxnData();
                data.TicketMainType = GetNextString(2);
                data.TicketSubType = GetNextString(2);
                data.TxnType = GetNextString(2);
                data.PaymentType = GetNextString(1);
                data.TxnTotalCount = GetNextString(5).Trim(); ;
                data.CashTotalAmt = GetNextString(8).Trim(); ;
                data.CardTotalAmt = GetNextString(8).Trim(); ;
                BomTxnDataList.Add(data);
            }

            for (int i = 0; i < adminCount; i++)
            {
                BomAdminData data = new BomAdminData();
                data.AdminCode = GetNextString(2);
                data.InOutFlag = GetNextString(2);
                data.TxnType = GetNextString(2);
                data.TxnTotalCount = GetNextString(5).Trim(); ;
                data.RefundAmt = GetNextString(8).Trim(); ;
                data.CashTotalAmt = GetNextString(8).Trim(); ;
                data.CardTotalAmt = GetNextString(8).Trim(); ;
                data.FavorTotalAmt = GetNextString(8).Trim(); ;
                BomAdminDataList.Add(data);
            }

            for (int i = 0; i < lossCount; i++)
            {
                LossHandleData data = new LossHandleData();
                data.TicketMainType = GetNextString(2);
                data.TicketSubType = GetNextString(2);
                data.TxnType = GetNextString(2);
                data.LossFlag = GetNextString(1);
                data.TxnCount = GetNextString(5).Trim();
                data.CashTotalAmt = GetNextString(8).Trim();
                LossHandleDataList.Add(data);
            }

            for (int i = 0; i < refundCount; i++)
            {
                RefundHandleData data = new RefundHandleData();
                data.TicketMainType = GetNextString(2);
                data.TicketSubType = GetNextString(2);
                data.TxnType = GetNextString(2);
                data.RefundFlag = GetNextString(1);
                data.TxnCount = GetNextString(5).Trim();
                data.CashTotalAmt = GetNextString(8).Trim();
                RefundHandleDataList.Add(data);
            }

            GroupTicketData gdata = new GroupTicketData();
            gdata.RedeemTimes = GetNextString(4).Trim();
            gdata.RedeemCount = GetNextString(10).Trim();
            gdata.GroupTicketHandleTimes = GetNextString(4).Trim();
            gdata.GroupTicketUseCount = GetNextString(6).Trim();
            gdata.CashTotalAmt = GetNextString(10).Trim();
            GroupTicketDataList.Add(gdata);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(AuditType, 1));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(ShiftSeqId.PadLeft(10, '0'), 10));
            encodeBuf.AddRange(AddString(AuditDataCreateTime, 14));
            encodeBuf.AddRange(AddString(ShiftStartTime, 14));
            encodeBuf.AddRange(AddString(ShiftEndTime, 14));

            encodeBuf.AddRange(AddString(TicketStatDataList.Count.ToString().PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(BomTxnDataList.Count.ToString().PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(BomAdminDataList.Count.ToString().PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(LossHandleDataList.Count.ToString().PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(RefundHandleDataList.Count.ToString().PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(TxnHandleType.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(AdminHandleType.PadLeft(2, '0'), 2));

            foreach (TicketStatData item in TicketStatDataList)
            {
                encodeBuf.AddRange(AddString(item.TicketMainType, 2));
                encodeBuf.AddRange(AddString(item.TicketSubType, 2));
                encodeBuf.AddRange(AddString(item.TicketCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.BadTicketCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.RefundTicketCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.BlackTicketCount.PadLeft(5, '0'), 5));
            }

            foreach (BomTxnData item in BomTxnDataList)
            {
                encodeBuf.AddRange(AddString(item.TicketMainType, 2));
                encodeBuf.AddRange(AddString(item.TicketSubType, 2));
                encodeBuf.AddRange(AddString(item.TxnType, 2));
                encodeBuf.AddRange(AddString(item.PaymentType, 1));
                encodeBuf.AddRange(AddString(item.TxnTotalCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.CashTotalAmt.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(item.CardTotalAmt.PadLeft(8, '0'), 8));
            }

            foreach (BomAdminData item in BomAdminDataList)
            {
                encodeBuf.AddRange(AddString(item.AdminCode, 2));
                encodeBuf.AddRange(AddString(item.InOutFlag, 2));
                encodeBuf.AddRange(AddString(item.TxnType, 2));
                encodeBuf.AddRange(AddString(item.TxnTotalCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.RefundAmt.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(item.CashTotalAmt.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(item.CardTotalAmt.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(item.FavorTotalAmt.PadLeft(8, '0'), 8));
            }

            foreach (LossHandleData item in LossHandleDataList)
            {
                encodeBuf.AddRange(AddString(item.TicketMainType, 2));
                encodeBuf.AddRange(AddString(item.TicketSubType, 2));
                encodeBuf.AddRange(AddString(item.TxnType, 2));
                encodeBuf.AddRange(AddString(item.LossFlag, 1));
                encodeBuf.AddRange(AddString(item.TxnCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.CashTotalAmt.PadLeft(8, '0'), 8));
            }

            foreach (RefundHandleData item in RefundHandleDataList)
            {
                encodeBuf.AddRange(AddString(item.TicketMainType, 2));
                encodeBuf.AddRange(AddString(item.TicketSubType, 2));
                encodeBuf.AddRange(AddString(item.TxnType, 2));
                encodeBuf.AddRange(AddString(item.RefundFlag, 1));
                encodeBuf.AddRange(AddString(item.TxnCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.CashTotalAmt.PadLeft(8, '0'), 8));
            }

            foreach (GroupTicketData item in GroupTicketDataList)
            {
                encodeBuf.AddRange(AddString(item.RedeemTimes.PadLeft(4, '0'), 4));
                encodeBuf.AddRange(AddString(item.RedeemCount.PadLeft(10, '0'), 10));
                encodeBuf.AddRange(AddString(item.GroupTicketHandleTimes.PadLeft(4, '0'), 4));
                encodeBuf.AddRange(AddString(item.GroupTicketUseCount.PadLeft(6, '0'), 6));
                encodeBuf.AddRange(AddString(item.CashTotalAmt.PadLeft(10, '0'), 10));
            }
        }
    }
}