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
    public class ControlCommandExtendMessage : BaseMessage
    {
        /// <summary>
        /// 控制命令发送时间CHAR(14)
        /// </summary>
        public string CmdSendTime { get; set; }

        public string MsgCode { get; set; }

        /// <summary>
        /// 命令代码CHAR(4)
        /// </summary>
        public string CmdCode { get; set; }

        /// <summary>
        /// 软件版本类型 (2)
        /// </summary>
        public string CmdParam1 { get; set; }

        /// <summary>
        /// 预留,填0
        /// </summary>

        public string CmdParam2 { get; set; }

        /// <summary>
        /// 预留，填0
        /// </summary>
        public string CmdParam3 { get; set; }

        /// <summary>
        /// 目标设备
        /// </summary>
        public string DeviceId { get; set; }

        public ControlCommandExtendMessage()
        {
            msgType = MsgType.CmdContrlCmdExtend;
        }

        public ControlCommandExtendMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.CmdContrlCmdExtend;
        }

        public override void Decode()
        {
            base.Decode();
            CmdSendTime = GetNextString(14);
            MsgCode = GetNextString(4);
            CmdCode = GetNextString(4);
            CmdParam1 = GetNextString(2);
            CmdParam2 = GetNextString(8);
            CmdParam3 = GetNextString(16);
            DeviceId = GetNextString(8);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(CmdSendTime, 14));
            encodeBuf.AddRange(AddString(MsgCode, 4));
            encodeBuf.AddRange(AddString(CmdCode, 4));
            encodeBuf.AddRange(AddString(CmdParam1.PadLeft(2, '0'), 2));
            encodeBuf.AddRange(AddString(CmdParam2.PadLeft(8, '0'), 8));
            encodeBuf.AddRange(AddString(CmdParam3.PadLeft(16, '0'), 16));
            encodeBuf.AddRange(AddString(DeviceId.PadLeft(8, '0'), 8));
        }
    }
}