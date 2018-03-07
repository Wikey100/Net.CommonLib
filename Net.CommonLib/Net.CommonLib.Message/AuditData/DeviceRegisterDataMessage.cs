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
    public class RegisterKeyValue
    {
        /// <summary>
        /// 存储器Id CHAR(6)
        /// </summary>
        public string RegisterId { get; set; }

        /// <summary>
        /// 存储器值 CHAR(10）
        /// </summary>
        public string RegisterValue { get; set; }
    }

    public class DeviceRegisterDataMessage : BaseMessage
    {
        /// <summary>
        /// 消息代号char(4)
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 设备Id char(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 运营日 CHAR(8)
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 采集时间 CHAR(14)
        /// </summary>
        public string CollectTime { get; set; }

        /// <summary>
        /// 班次CHAR(10)
        /// </summary>
        public List<RegisterKeyValue> RegisterList = new List<RegisterKeyValue>();

        public DeviceRegisterDataMessage()
        {
            msgType = MsgType.AuditRegisterData;
        }

        public DeviceRegisterDataMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AuditRegisterData;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            DeviceId = GetNextString(8);
            ReportDate = GetNextString(8);
            CollectTime = GetNextString(14);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                RegisterKeyValue registerData = new RegisterKeyValue();
                registerData.RegisterId = GetNextString(6);
                registerData.RegisterValue = GetNextString(10);
                RegisterList.Add(registerData);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(CollectTime, 14));
            encodeBuf.AddRange(AddString(RegisterList.Count.ToString().PadLeft(3, '0'), 3));
            foreach (RegisterKeyValue rk in RegisterList)
            {
                encodeBuf.AddRange(AddString(rk.RegisterId.PadLeft(6, '0'), 6));
                encodeBuf.AddRange(AddString(rk.RegisterValue.PadLeft(10, '0'), 10));
            }
        }
    }
}