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
    public class PrmVersionQueryRequsetMessage : BaseMessage
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
        /// 参数类型 (2)
        /// </summary>
        public string PrmType { get; set; }

        /// <summary>
        /// 预留,填0
        /// </summary>

        public string VerSeqno { get; set; }

        /// <summary>
        /// 预留，填0
        /// </summary>
        public string CmdData { get; set; }

        public PrmVersionQueryRequsetMessage()
        {
            msgType = MsgType.CmdPrmVersionRequest;
        }

        public PrmVersionQueryRequsetMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.CmdPrmVersionRequest;
        }

        public override void Decode()
        {
            base.Decode();
            CmdSendTime = GetNextString(14);
            CmdCode = GetNextString(4);
            PrmType = GetNextString(2);
            VerSeqno = GetNextString(8);
            CmdData = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(CmdSendTime, 14));
            encodeBuf.AddRange(AddString(CmdCode, 4));
            encodeBuf.AddRange(AddString(PrmType, 2));
            encodeBuf.AddRange(AddString(VerSeqno.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(CmdData.PadLeft(16, '0'), 16));
        }
    }
}