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
    public class AgmAuditData
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
        /// 进站数量统计(5)
        /// </summary>
        public string EntryCount { get; set; }

        /// <summary>
        /// 出站数据统计（5）
        /// </summary>
        public string ExitCount { get; set; }

        /// <summary>
        /// 出站总金额(8)
        /// </summary>
        public string ExitTotalAmt { get; set; }
    }

    public class AgmAuditDataMessage : BaseMessage
    {
        /// <summary>
        /// 审计类型(1)
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
        public string AgmTxnTime { get; set; }

        /// <summary>
        /// Agm类型(1)
        /// </summary>

        public string AgmType { get; set; }

        public List<AgmAuditData> AgmAuditDataList = new List<AgmAuditData>();

        public AgmAuditDataMessage()
        {
            msgType = MsgType.AuditAgmTxnAuditData;
        }

        public AgmAuditDataMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AuditAgmTxnAuditData;
        }

        public override void Decode()
        {
            base.Decode();
            AuditType = GetNextString(1);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            ReportDate = GetNextString(8);
            AgmTxnTime = GetNextString(14);
            AgmType = GetNextString(1);
            int count = Convert.ToInt32(GetNextString(2));
            for (int i = 0; i < count; i++)
            {
                AgmAuditData data = new AgmAuditData();
                data.TicketMainType = GetNextString(2);
                data.TicketSubType = GetNextString(2);
                data.EntryCount = GetNextString(5);
                data.ExitCount = GetNextString(5);
                data.ExitTotalAmt = GetNextString(8);
                AgmAuditDataList.Add(data);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(AuditType, 1));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(AgmTxnTime, 14));
            encodeBuf.AddRange(AddString(AgmType, 1));
            encodeBuf.AddRange(AddString(AgmAuditDataList.Count.ToString().PadLeft(2, '0'), 2));
            foreach (AgmAuditData item in AgmAuditDataList)
            {
                encodeBuf.AddRange(AddString(item.TicketMainType, 2));
                encodeBuf.AddRange(AddString(item.TicketSubType, 2));
                encodeBuf.AddRange(AddString(item.EntryCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.ExitCount.PadLeft(5, '0'), 5));
                encodeBuf.AddRange(AddString(item.ExitTotalAmt.PadLeft(8, '0'), 8));
            }
        }
    }
}