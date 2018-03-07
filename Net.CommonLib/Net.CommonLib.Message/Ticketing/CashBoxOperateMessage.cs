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
    public class CashBoxOperateMessage : BaseMessage
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
        /// 设备编号 8
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 箱子编号 8
        /// </summary>
        public string CashBoxID { get; set; }

        /// <summary>
        /// 箱子类型 2
        /// </summary>
        public string CashBoxType { get; set; }

        /// <summary>
        /// RFID 8
        /// </summary>
        public string CashBoxRfid { get; set; }

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
        /// 清点状态 2
        /// </summary>
        public string ClearStatus { get; set; }

        /// <summary>
        /// 种类 2
        /// </summary>
        public string CashKind { get; set; }

        /// <summary>
        ///  一元硬币数量 8
        /// </summary>
        public string OneCoinQty { get; set; }

        /// <summary>
        /// 一元纸币数量 8
        /// </summary>
        public string OneYuanQty { get; set; }

        public string FiveYuanQty { get; set; }

        public string TenYuanQty { get; set; }

        public string TwentyYuanQty { get; set; }

        public string FiftyYuanQty { get; set; }

        public string HundredYuanQty { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public string CashQty { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public string CashTotalAmt { get; set; }

        /// <summary>
        /// ID1
        /// </summary>
        public string OperatorID { get; set; }

        /// <summary>
        /// ID2
        /// </summary>
        public string DoublePrivilegeOperatorID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string UpdateTime { get; set; }

        public CashBoxOperateMessage()
        {
            msgType = MsgType.TicketingCashBoxOperate;
        }

        public CashBoxOperateMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.TicketingCashBoxOperate;
        }

        public override void Decode()
        {
            base.Decode();
            LineId = GetNextString(2);
            OperatorType = GetNextString(2);
            StationID = GetNextString(4);
            DeviceId = GetNextString(8);
            CashBoxID = GetNextString(8);
            CashBoxType = GetNextString(2);
            CashBoxRfid = GetNextString(8);
            LocationStatus = GetNextString(2);
            InstallLocation = GetNextString(2);
            InstallStatus = GetNextString(2);
            ClearStatus = GetNextString(2);
            CashKind = GetNextString(2);
            OneCoinQty = GetNextString(8);
            OneYuanQty = GetNextString(8);
            FiveYuanQty = GetNextString(8);
            TenYuanQty = GetNextString(8);
            TwentyYuanQty = GetNextString(8);
            FiftyYuanQty = GetNextString(8);
            HundredYuanQty = GetNextString(8);
            CashQty = GetNextString(8);
            CashTotalAmt = GetNextString(8);
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
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(CashBoxID, 8));
            encodeBuf.AddRange(AddString(CashBoxType, 2));
            encodeBuf.AddRange(AddString(CashBoxRfid, 8));
            encodeBuf.AddRange(AddString(LocationStatus, 2));
            encodeBuf.AddRange(AddString(InstallLocation, 2));
            encodeBuf.AddRange(AddString(InstallStatus, 2));
            encodeBuf.AddRange(AddString(ClearStatus, 2));
            encodeBuf.AddRange(AddString(CashKind, 2));
            encodeBuf.AddRange(AddString(OneCoinQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(OneYuanQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(FiveYuanQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(TenYuanQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(TwentyYuanQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(FiftyYuanQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(HundredYuanQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(CashQty.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(CashTotalAmt.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(OperatorID, 6));
            encodeBuf.AddRange(AddString(DoublePrivilegeOperatorID, 6));
            encodeBuf.AddRange(AddString(UpdateTime, 14));
        }
    }
}