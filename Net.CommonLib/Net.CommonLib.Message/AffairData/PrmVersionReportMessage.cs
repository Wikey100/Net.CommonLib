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
    public class ParameterData
    {
        /// <summary>
        /// 参数类型（2）
        /// </summary>
        public string PrmType { get; set; }

        /// <summary>
        /// 当前版本(30)
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 将来版本(30)
        /// </summary>
        public string FutureVersion { get; set; }
    }

    public class PrmVersionReportMessage : BaseMessage
    {
        /// <summary>
        /// 设备编号char(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 采集时间CHAR(14)
        /// </summary>
        public string ReportTime { get; set; }

        public List<ParameterData> ParameterDataList = new List<ParameterData>();

        public PrmVersionReportMessage()
        {
            msgType = MsgType.PrmVersionReport;
        }

        public PrmVersionReportMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.PrmVersionReport;
        }

        public override void Decode()
        {
            base.Decode();
            DeviceId = GetNextString(8);
            ReportTime = GetNextString(14);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                ParameterData ms = new ParameterData();
                ms.PrmType = GetNextString(2);
                ms.CurrentVersion = GetNextString(30);
                ms.FutureVersion = GetNextString(30);
                ParameterDataList.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = ParameterDataList.Count;
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportTime, 14));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(3, '0'), 3));
            foreach (ParameterData ms in ParameterDataList)
            {
                encodeBuf.AddRange(AddString(ms.PrmType, 2));
                encodeBuf.AddRange(AddString(ms.CurrentVersion.PadLeft(30, ' '), 30));
                encodeBuf.AddRange(AddString(ms.FutureVersion.PadLeft(30, ' '), 30));
            }
        }
    }
}