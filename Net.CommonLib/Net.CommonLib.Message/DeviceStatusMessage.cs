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
    public class ModuleStatus
    {
        /// <summary>
        /// 状态类别，0代表整机状态，1代表部件状态CHAR(8)
        /// </summary>
        public string StatusType { get; set; }

        /// <summary>
        /// 状态部件编号CHAR(5)
        /// </summary>
        public string ModueleId { get; set; }

        /// <summary>
        /// 值，CHAR(1)
        /// </summary>
        public string ModuleValue { get; set; }
    }

    public class DeviceStatusMessage : BaseMessage
    {
        /// <summary>
        /// 设备编号char(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 设备厂商编号char(2)
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// 采集时间CHAR(14)
        /// </summary>
        public string CollectTime { get; set; }

        public List<ModuleStatus> ModuleStatusList = new List<ModuleStatus>();

        public DeviceStatusMessage()
        {
            msgType = MsgType.DeviceStatus;
        }

        public DeviceStatusMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.DeviceStatus;
        }

        public override void Decode()
        {
            base.Decode();
            DeviceId = GetNextString(8);
            BrandId = GetNextString(2);
            CollectTime = GetNextString(14);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                ModuleStatus ms = new ModuleStatus();
                ms.StatusType = GetNextString(1);
                ms.ModueleId = GetNextString(5);
                ms.ModuleValue = GetNextString(4);
                ModuleStatusList.Add(ms);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = ModuleStatusList.Count;

            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(BrandId, 2));
            encodeBuf.AddRange(AddString(CollectTime, 14));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(3, '0'), 3));
            foreach (ModuleStatus ms in ModuleStatusList)
            {
                encodeBuf.AddRange(AddString(ms.StatusType, 1));
                encodeBuf.AddRange(AddString(ms.ModueleId, 5));
                encodeBuf.AddRange(AddString(ms.ModuleValue, 4));
            }
        }
    }
}