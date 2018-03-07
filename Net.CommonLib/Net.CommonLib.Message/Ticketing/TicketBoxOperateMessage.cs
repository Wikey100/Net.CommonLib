/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Message.Ticketing
{
    public class TicketBoxOperateMessage : BaseMessage
    {
        /// <summary>
        /// 线路 CHAR（2）
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 操作类型 2
        /// </summary>
        public string OperatorType { get; set; }

        /// <summary>
        /// 车站编号 4
        /// </summary>
        public string StationID { get; set; }

        /// <summary>
        /// 票箱编号 8
        /// </summary>
        public string TicketBoxID { get; set; }

        /// <summary>
        /// 设备编号 8
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 箱子类型 2
        /// </summary>
        public string TicketBoxType { get; set; }

        /// <summary>
        /// RFID 8
        /// </summary>
        public string TicketBoxRfid { get; set; }

        /// <summary>
        /// 位置状态 2
        /// </summary>
        public string LocationStatus { get; set; }

        /// <summary>
        /// 安装位置 2
        /// </summary>
        public string InstallLocation { get; set; }

        /// <summary>
        /// 安装状态 2
        /// </summary>
        public string InstallStatus { get; set; }

        /// <summary>
        /// 清点状态
        /// </summary>
        public string ClearStatus { get; set; }

        /// <summary>
        /// 操作前数量 8
        /// </summary>
        public string BeforeOperateQty { get; set; }

        /// <summary>
        /// 操作数量 8
        /// </summary>
        public string TicketQty { get; set; }

        /// <summary>
        /// 操作员ID 6
        /// </summary>
        public string OperatorID { get; set; }

        /// <summary>
        /// 操作员ID 6
        /// </summary>
        public string DoublePrivilegeOperatorID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string UpdateTime { get; set; }

        public TicketBoxOperateMessage()
        {
            msgType = MsgType.TicketingTicketBoxOperate;
        }

        public TicketBoxOperateMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TicketingTicketBoxOperate;
        }

        public override void Decode()
        {
            base.Decode();
            LineId = GetNextString(2);
            OperatorType = GetNextString(2);
            StationID = GetNextString(4);
            TicketBoxID = GetNextString(8);
            DeviceId = GetNextString(8);
            TicketBoxType = GetNextString(2);
            TicketBoxRfid = GetNextString(8);
            LocationStatus = GetNextString(2);
            InstallLocation = GetNextString(2);
            InstallStatus = GetNextString(2);
            ClearStatus = GetNextString(2);
            BeforeOperateQty = GetNextString(8);
            TicketQty = GetNextString(8);
            OperatorID = GetNextString(6);
            DoublePrivilegeOperatorID = GetNextString(6);
            UpdateTime = GetNextString(14);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(LineId, 2));
            encodeBuf.AddRange(AddString(OperatorType, 2));
            encodeBuf.AddRange(AddString(StationID, 4));
            encodeBuf.AddRange(AddString(TicketBoxID, 8));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(TicketBoxType, 2));
            encodeBuf.AddRange(AddString(TicketBoxRfid, 8));
            encodeBuf.AddRange(AddString(LocationStatus, 2));
            encodeBuf.AddRange(AddString(InstallLocation, 2));
            encodeBuf.AddRange(AddString(InstallStatus, 2));
            encodeBuf.AddRange(AddString(ClearStatus, 2));
            encodeBuf.AddRange(AddString(BeforeOperateQty.PadLeft(8, ' '), 8));
            encodeBuf.AddRange(AddString(TicketQty.PadLeft(8, ' '), 8));
            encodeBuf.AddRange(AddString(OperatorID, 6));
            encodeBuf.AddRange(AddString(DoublePrivilegeOperatorID, 6));
            encodeBuf.AddRange(AddString(UpdateTime, 14));
        }
    }
}