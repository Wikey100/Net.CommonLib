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
    public class SystemDegradeModeMessage : BaseMessage
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
        /// 降级模式代码 (2)
        /// </summary>
        public string DegradeMode { get; set; }

        /// <summary>
        /// 降级模式标志2位（见控制命令详细内容降级模式中xx）+00+车站ID4位  (8)
        /// </summary>
        public string DegradeModeFlag { get; set; }

        /// <summary>
        /// 降级模式开始时间 （16）
        /// </summary>
        public string DegradeModeStartTime { get; set; }

        public SystemDegradeModeMessage()
        {
            msgType = MsgType.CmdSystemDegradeMode;
        }

        public SystemDegradeModeMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.CmdSystemDegradeMode;
        }

        public override void Decode()
        {
            base.Decode();
            CmdSendTime = GetNextString(14);
            CmdCode = GetNextString(4);
            DegradeMode = GetNextString(2);
            DegradeModeFlag = GetNextString(8);
            DegradeModeStartTime = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(CmdSendTime, 14));
            encodeBuf.AddRange(AddString(CmdCode, 4));
            encodeBuf.AddRange(AddString(DegradeMode, 2));
            encodeBuf.AddRange(AddString(DegradeModeFlag, 8));
            encodeBuf.AddRange(AddString(DegradeModeStartTime.PadLeft(16, '0'), 16));
        }
    }
}