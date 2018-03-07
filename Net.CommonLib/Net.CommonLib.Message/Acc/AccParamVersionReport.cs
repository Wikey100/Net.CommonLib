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

namespace Net.CommonLib.Message.Acc
{
    public class AccParameterData
    {
        /// <summary>
        /// 参数类型（2）
        /// </summary>
        public string PrmType { get; set; }

        /// <summary>
        /// 当前版本(30)
        /// </summary>
        public string CurrentVersion { get; set; }
    }

    public class AccParamVersionReport : BaseMessage
    {
        public string MessageCode { get; set; }

        public string LineId { get; set; }

        public List<AccParameterData> ParameterDataList = new List<AccParameterData>();

        public AccParamVersionReport()
        {
            msgType = MsgType.AccParamVersionReport;
        }

        public AccParamVersionReport(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AccParamVersionReport;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            LineId = GetNextString(2);
            int count = Convert.ToInt32(GetNextString(2));
            for (int i = 0; i < count; i++)
            {
                AccParameterData ms = new AccParameterData();
                ms.PrmType = GetNextString(2);
                ms.CurrentVersion = GetNextString(30).Trim();
                ParameterDataList.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(ParameterDataList.Count.ToString().PadLeft(2, '0'), 2));
            foreach (AccParameterData item in ParameterDataList)
            {
                encodeBuf.AddRange(AddString(item.PrmType, 2));
                encodeBuf.AddRange(AddString(item.CurrentVersion.PadLeft(30, ' '), 30));
            }
        }
    }
}