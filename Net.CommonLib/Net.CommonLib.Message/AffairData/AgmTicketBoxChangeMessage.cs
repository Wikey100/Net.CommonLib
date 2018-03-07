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
    public class AgmTicketBoxChangeMessage : BaseMessage
    {
        /// <summary>
        /// 交易类型 （2）
        /// </summary>
        public string TxnType { get; set; }

        /// <summary>
        /// 车站代码CHAR(4)
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 车站代码CHAR(8)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 运营日(8)
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 票箱取出时间（14）
        /// </summary>
        public string BoxGetOutTime { get; set; }

        /// <summary>
        /// 票箱ID（16）
        /// </summary>
        public string TicketBoxId { get; set; }

        /// <summary>
        /// 操作员ID(6)
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 单程票子类型(2)
        /// </summary>
        public string TicketType { get; set; }

        /// <summary>
        /// 测试标志(1)
        /// </summary>
        public string TestFlag { get; set; }

        /// <summary>
        /// 票卡数量（7）
        /// </summary>
        public string TicketQty { get; set; }

        /// <summary>
        /// 上次放入时间
        /// </summary>
        public string LastPutInTime { get; set; }

        /// <summary>
        /// 上次放入操作员ID
        /// </summary>
        public string LastPutInOperatorId { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public AgmTicketBoxChangeMessage()
        {
            msgType = MsgType.AgmTicketBoxChange;
        }

        public AgmTicketBoxChangeMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AgmTicketBoxChange;
        }

        public override void Decode()
        {
            base.Decode();
            TxnType = GetNextString(2);
            StationId = GetNextString(4);
            DeviceId = GetNextString(8);
            ReportDate = GetNextString(8);
            BoxGetOutTime = GetNextString(14);
            TicketBoxId = GetNextString(16);
            OperatorId = GetNextString(6);
            TicketType = GetNextString(2);
            TestFlag = GetNextString(1);
            TicketQty = GetNextString(7);
            LastPutInTime = GetNextString(14);
            LastPutInOperatorId = GetNextString(6);
            Spare = GetNextString(8);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(TxnType, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(ReportDate, 8));
            encodeBuf.AddRange(AddString(BoxGetOutTime, 14));
            encodeBuf.AddRange(AddString(TicketBoxId.PadLeft(16, '0'), 16));
            encodeBuf.AddRange(AddString(OperatorId.PadLeft(6, '0'), 6));
            encodeBuf.AddRange(AddString(TicketType, 2));
            encodeBuf.AddRange(AddString(TestFlag, 1));
            encodeBuf.AddRange(AddString(TicketQty.PadLeft(7, '0'), 7));
            encodeBuf.AddRange(AddString(LastPutInTime, 14));
            encodeBuf.AddRange(AddString(LastPutInOperatorId, 6));
            encodeBuf.AddRange(AddString(Spare.PadLeft(8, '0'), 8));
        }
    }
}