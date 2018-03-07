/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Message
{
    public class BomShiftDataMessage : BaseMessage
    {
        /// <summary>
        /// 设备编号char(8)
        /// </summary>
        public string DeviceId { get; set; }

        ///// <summary>
        ///// 设备IP char(15)
        ///// </summary>
        //public string DeviceIp { get; set; }

        /// <summary>
        /// 操作时间 CHAR(14)
        /// </summary>
        public string OperateTime { get; set; }

        /// <summary>
        /// 操作员ID CHAR(6)
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 班次CHAR(10)
        /// </summary>
        public string ShiftId { get; set; }

        /// <summary>
        /// 班次事件CHAR(1)
        /// </summary>
        public string ShiftEvent { get; set; }

        public BomShiftDataMessage()
        {
            msgType = MsgType.AuditBomShiftData;
        }

        public BomShiftDataMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AuditBomShiftData;
        }

        public override void Decode()
        {
            base.Decode();
            DeviceId = GetNextString(8);
            OperateTime = GetNextString(14);
            OperatorId = GetNextString(6);
            ShiftId = GetNextString(10);
            ShiftEvent = GetNextString(1);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(OperateTime, 14));
            encodeBuf.AddRange(AddString(OperatorId.PadLeft(6, ' '), 6));
            encodeBuf.AddRange(AddString(ShiftId.PadLeft(10, '0'), 10));
            encodeBuf.AddRange(AddString(ShiftEvent, 1));
        }
    }
}