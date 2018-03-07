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
    public class ReplenishmentData
    {
        /// <summary>
        /// 关联单号编号CHAR(20)
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// 差错操作员ID CHAR(6)
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 差错日期char(8)
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 补款金额(10)
        /// </summary>
        public string Amt { get; set; }

        /// <summary>
        /// 补款原因（4）
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 补款类型（2）
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 补款说明(50)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注说明(50)
        /// </summary>
        public string Remark { get; set; }
    }

    public class ReplenishmentNotifyMessage : BaseMessage
    {
        /// <summary>
        /// 车站编号(4)
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 要求补款日期 char(8)
        /// </summary>
        public string RequireDate { get; set; }

        /// <summary>
        /// 统计截止日期 CHAR(14)
        /// </summary>
        public string StatisticDate { get; set; }

        /// <summary>
        /// 短款总额 CHAR(10)
        /// </summary>
        public string TotalAmt { get; set; }

        /// <summary>
        /// 班次CHAR(10)
        /// </summary>
        public List<ReplenishmentData> ReplenishmentDataList = new List<ReplenishmentData>();

        public ReplenishmentNotifyMessage()
        {
            msgType = MsgType.AuditReplenishmentNotify;
        }

        public ReplenishmentNotifyMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AuditReplenishmentNotify;
        }

        public override void Decode()
        {
            base.Decode();
            StationId = GetNextString(4);
            RequireDate = GetNextString(8);
            StatisticDate = GetNextString(14);
            TotalAmt = GetNextString(10);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                ReplenishmentData data = new ReplenishmentData();
                data.ReceiptId = GetNextString(20);
                data.OperatorId = GetNextString(6);
                data.ReportDate = GetNextString(8);
                data.Amt = GetNextString(10);
                data.Reason = GetNextString(4);
                data.Type = GetNextString(2);
                data.Description = GetNextUnicodeString(50);
                data.Remark = GetNextUnicodeString(50);
                ReplenishmentDataList.Add(data);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(RequireDate, 8));
            encodeBuf.AddRange(AddString(StatisticDate, 14));
            encodeBuf.AddRange(AddString(TotalAmt.ToString().PadLeft(10, '0'), 10));
            encodeBuf.AddRange(AddString(ReplenishmentDataList.Count.ToString().PadLeft(3, '0'), 3));
            foreach (ReplenishmentData item in ReplenishmentDataList)
            {
                encodeBuf.AddRange(AddString(item.ReceiptId, 20));
                encodeBuf.AddRange(AddString(item.OperatorId, 6));
                encodeBuf.AddRange(AddString(item.ReportDate, 8));
                encodeBuf.AddRange(AddString(item.Amt.ToString().PadLeft(10, '0'), 10));
                encodeBuf.AddRange(AddString(item.Reason, 4));
                encodeBuf.AddRange(AddString(item.Type, 2));
                encodeBuf.AddRange(AddUnicodeString(item.Description.PadRight(50, ' '), 50));
                encodeBuf.AddRange(AddUnicodeString(item.Remark.PadRight(50, ' '), 50));
            }
        }
    }
}