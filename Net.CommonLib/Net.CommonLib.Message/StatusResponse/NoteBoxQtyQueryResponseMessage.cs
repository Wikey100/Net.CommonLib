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
    public class NoteBoxQtyQueryResponseMessage : BaseMessage
    {
        /// <summary>
        /// 采集时间CHAR(14)
        /// </summary>
        public string CollectTime { get; set; }

        /// <summary>
        /// 车站代码CHAR(4)
        /// </summary>
        public string StationId { get; set; }

        public string DeviceId { get; set; }

        public string ModuleId { get; set; }

        public List<BoxDataRecord> BoxDataRecordList = new List<BoxDataRecord>();

        public NoteBoxQtyQueryResponseMessage()
        {
            msgType = MsgType.ResponeNoteQtyQuery;
            // BoxDataRecordList = new List<BoxDataRecord>();
        }

        public NoteBoxQtyQueryResponseMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.ResponeNoteQtyQuery;
            //  BoxDataRecordList = new List<BoxDataRecord>();
        }

        public override void Decode()
        {
            base.Decode();
            CollectTime = GetNextString(14);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            ModuleId = GetNextString(5);
            int count = Convert.ToInt32(GetNextString(3));
            for (int i = 0; i < count; i++)
            {
                BoxDataRecord data = new BoxDataRecord();
                data.MainType = GetNextString(2);
                data.SubType = GetNextString(2);
                data.Count = GetNextString(8);
                BoxDataRecordList.Add(data);
            }
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            int count = BoxDataRecordList.Count;
            encodeBuf.AddRange(AddString(CollectTime, 14));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ModuleId, 5));
            encodeBuf.AddRange(AddString(count.ToString().PadLeft(3, '0'), 3));
            foreach (BoxDataRecord ms in BoxDataRecordList)
            {
                encodeBuf.AddRange(AddString(ms.MainType, 2));
                encodeBuf.AddRange(AddString(ms.SubType, 2));
                encodeBuf.AddRange(AddString(ms.Count.ToString().PadLeft(8, '0'), 8));
            }
        }
    }
}