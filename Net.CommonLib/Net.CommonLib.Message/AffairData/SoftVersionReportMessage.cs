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
    public class SoftData
    {
        /// <summary>
        /// 软件类型
        /// </summary>
        public string SoftType { get; set; }

        /// <summary>
        /// 当前版本(30)
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 将来版本(30)
        /// </summary>
        public string FutureVersion { get; set; }
    }

    public class SoftVersionReportMessage : BaseMessage
    {
        /// <summary>
        /// 设备编号char(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 采集时间CHAR(14)
        /// </summary>
        public string ReportTime { get; set; }

        public List<SoftData> SoftDataList = new List<SoftData>();

        public SoftVersionReportMessage()
        {
            msgType = MsgType.SoftVersionReport;
        }

        public SoftVersionReportMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.SoftVersionReport;
        }

        public override void Decode()
        {
            base.Decode();
            DeviceId = GetNextString(8);
            ReportTime = GetNextString(14);
            int count = Convert.ToInt32(GetNextString(2));
            for (int i = 0; i < count; i++)
            {
                SoftData ms = new SoftData();
                ms.SoftType = GetNextString(2);
                ms.CurrentVersion = GetNextString(30);
                ms.FutureVersion = GetNextString(30);
                SoftDataList.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = SoftDataList.Count;
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportTime, 14));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(2, '0'), 2));
            foreach (SoftData ms in SoftDataList)
            {
                encodeBuf.AddRange(AddString(ms.SoftType, 2));
                encodeBuf.AddRange(AddString(ms.CurrentVersion.PadLeft(30, ' '), 30));
                encodeBuf.AddRange(AddString(ms.FutureVersion.PadLeft(30, ' '), 30));
            }
        }
    }
}