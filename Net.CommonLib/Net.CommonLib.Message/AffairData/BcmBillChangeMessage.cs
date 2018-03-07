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
    public class BcmBillChangeMessage : BaseMessage
    {
        /// <summary>
        /// 交易类型 （8）
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 车站代码CHAR(1)
        /// </summary>
        public string TxnStatus { get; set; }

        /// <summary>
        /// 交易序列号组(2)
        /// </summary>
        public string GroupSerialNo { get; set; }

        /// <summary>
        /// 交易序列号(8)
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 发生时间(14)
        /// </summary>
        public string BillChangeTime { get; set; }

        /// <summary>
        /// 输入币种1(2)
        /// </summary>
        public string InPutCashKind1 { get; set; }

        /// <summary>
        /// 输入币种1数量(2)
        /// </summary>
        public string InPutCashKind1Qty { get; set; }

        /// <summary>
        /// 输入币种1(2)
        /// </summary>
        public string InPutCashKind2 { get; set; }

        /// <summary>
        /// 输入币种1数量(2)
        /// </summary>
        public string InPutCashKind2Qty { get; set; }

        /// <summary>
        /// 输入币种1(2)
        /// </summary>
        public string OutPutCashKind1 { get; set; }

        /// <summary>
        /// 输出币种1数量(2)
        /// </summary>
        public string OutPutCashKind1Qty { get; set; }

        /// <summary>
        /// 输出币种1(2)
        /// </summary>
        public string OutPutCashKind2 { get; set; }

        /// <summary>
        /// 输出币种1数量(2)
        /// </summary>
        public string OutPutCashKind2Qty { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        public string Spare { get; set; }

        public BcmBillChangeMessage()
        {
            // msgType = MsgType;
        }

        public BcmBillChangeMessage(byte[] bytes)
            : base(bytes)
        {
            // msgType = "4210";
        }

        public override void Decode()
        {
            base.Decode();

            DeviceId = GetNextString(8);
            TxnStatus = GetNextString(1);
            GroupSerialNo = GetNextString(2);
            SerialNo = GetNextString(8);
            BillChangeTime = GetNextString(14);
            InPutCashKind1 = GetNextString(2);
            InPutCashKind1Qty = GetNextString(2);
            InPutCashKind2 = GetNextString(2);
            InPutCashKind2Qty = GetNextString(2);
            OutPutCashKind1 = GetNextString(2);
            OutPutCashKind1Qty = GetNextString(2);
            OutPutCashKind2 = GetNextString(2);
            OutPutCashKind2Qty = GetNextString(2);
            Spare = GetNextString(21);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(TxnStatus, 1));
            encodeBuf.AddRange(AddString(GroupSerialNo, 2));
            encodeBuf.AddRange(AddString(SerialNo, 8));
            encodeBuf.AddRange(AddString(BillChangeTime, 14));
            encodeBuf.AddRange(AddString(InPutCashKind1, 2));
            encodeBuf.AddRange(AddString(InPutCashKind1Qty, 2));
            encodeBuf.AddRange(AddString(InPutCashKind2, 2));
            encodeBuf.AddRange(AddString(InPutCashKind2Qty, 2));
            encodeBuf.AddRange(AddString(OutPutCashKind1, 2));
            encodeBuf.AddRange(AddString(OutPutCashKind1Qty, 2));
            encodeBuf.AddRange(AddString(OutPutCashKind2, 2));
            encodeBuf.AddRange(AddString(OutPutCashKind2Qty, 2));
            encodeBuf.AddRange(AddString(Spare, 21));
        }
    }
}