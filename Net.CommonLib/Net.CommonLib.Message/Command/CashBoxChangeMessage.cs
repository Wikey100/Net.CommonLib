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
    public class CashBoxChangeMessage : BaseMessage
    {
        /// <summary>
        /// 控制命令发送时间CHAR(14)
        /// </summary>
        public string CmdSendTime { get; set; }

        /// <summary>
        /// 命令代码CHAR(4)
        /// </summary>
        public string CmdCode { get; set; }

        /// <summary>
        /// 钱箱或票箱模块代码 (2)
        /// </summary>
        public string BoxModuleCode { get; set; }

        /// <summary>
        /// 操作员ID(8),前补00
        /// </summary>

        public string OperatorId { get; set; }

        /// <summary>
        /// 有效截止时间(16),前补0
        /// </summary>
        public string ValidEndTime { get; set; }

        public CashBoxChangeMessage()
        {
            msgType = MsgType.CmdCashBoxChange;
        }

        public CashBoxChangeMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.CmdCashBoxChange;
        }

        public override void Decode()
        {
            base.Decode();
            CmdSendTime = GetNextString(14);
            CmdCode = GetNextString(4);
            BoxModuleCode = GetNextString(2);
            OperatorId = GetNextString(8);
            ValidEndTime = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(CmdSendTime, 14));
            encodeBuf.AddRange(AddString(CmdCode, 4));
            encodeBuf.AddRange(AddString(BoxModuleCode, 2));
            encodeBuf.AddRange(AddString(OperatorId.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(ValidEndTime.PadLeft(16, '0'), 16));
        }
    }
}