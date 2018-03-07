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
    public class DeviceInfo
    {
        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string UpperId { get; set; }

        public string DeviceType { get; set; }

        public string IpAddress { get; set; }

        public string DeviceGroup { get; set; }

        public string XyPosition { get; set; }

        public string ChangeType { get; set; }
    }

    public class DeviceRegisterMessage : BaseMessage
    {
        public string MessageCode { get; set; }

        public string CompanyId { get; set; }

        public string OcurrTime { get; set; }

        public List<DeviceInfo> DeviceInfos = new List<DeviceInfo>();

        public DeviceRegisterMessage()
        {
            msgType = MsgType.DeviceRegisterMessage;
        }

        public DeviceRegisterMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.DeviceRegisterMessage;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            CompanyId = GetNextString(2);
            OcurrTime = GetNextString(14);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                DeviceInfo info = new DeviceInfo();
                info.DeviceId = GetNextString(8);
                info.DeviceName = GetNextString(40);
                info.UpperId = GetNextString(4);
                info.DeviceType = GetNextString(2);
                info.IpAddress = GetNextString(15);
                info.DeviceGroup = GetNextString(2);
                info.XyPosition = GetNextString(20);
                info.ChangeType = GetNextString(1);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(CompanyId, 2));
            encodeBuf.AddRange(AddString(OcurrTime, 14));
            encodeBuf.AddRange(AddString(DeviceInfos.Count.ToString().PadLeft(3, '0'), 3));
            foreach (DeviceInfo item in DeviceInfos)
            {
                encodeBuf.AddRange(AddString(item.DeviceId, 8));
                encodeBuf.AddRange(AddString(item.DeviceName.PadLeft(40, ' '), 40));
                encodeBuf.AddRange(AddString(item.UpperId.PadLeft(4, ' '), 4));
                encodeBuf.AddRange(AddString(item.DeviceType, 2));
                encodeBuf.AddRange(AddString(item.IpAddress.PadLeft(15, ' '), 15));
                encodeBuf.AddRange(AddString(item.DeviceGroup, 2));
                encodeBuf.AddRange(AddString(item.XyPosition.PadLeft(20, ' '), 20));
                encodeBuf.AddRange(AddString(item.ChangeType, 1));
            }
        }
    }
}