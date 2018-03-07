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
    public class SamInfoData
    {
        public string SamCardId { get; set; }

        public string SamCardType { get; set; }

        public string SamCardNo { get; set; }
    }

    public class SamCardInfoReportMessage : BaseMessage
    {
        /// <summary>
        /// 消息代号（4）
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 线路Id
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 线路Id
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 线路Id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 控制命令发送时间CHAR(14)
        /// </summary>
        public string OccurTime { get; set; }

        public List<SamInfoData> SamInfoDataList = new List<SamInfoData>();

        public SamCardInfoReportMessage()
        {
            msgType = MsgType.SamInfoReport;
        }

        public SamCardInfoReportMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.SamInfoReport;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            LineId = GetNextString(2);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            OccurTime = GetNextString(14);

            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                SamInfoData ms = new SamInfoData();
                ms.SamCardId = GetNextString(8);
                ms.SamCardType = GetNextString(2);
                ms.SamCardNo = GetNextString(2);
                SamInfoDataList.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = SamInfoDataList.Count;
            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(OccurTime, 14));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(3, '0'), 3));
            foreach (SamInfoData ms in SamInfoDataList)
            {
                encodeBuf.AddRange(AddString(ms.SamCardId.PadLeft(8, '0'), 8));
                encodeBuf.AddRange(AddString(ms.SamCardType.PadLeft(2, '0'), 2));
                encodeBuf.AddRange(AddString(ms.SamCardNo.PadLeft(2, '0'), 2));
            }
        }
    }
}