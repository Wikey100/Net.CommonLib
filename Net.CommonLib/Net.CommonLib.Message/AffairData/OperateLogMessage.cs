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
    public class OperateLogMessage : BaseMessage
    {
        /// <summary>
        /// 控制命令发送时间CHAR(14)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 命令代码CHAR(1)
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// 钱箱或票箱模块代码 (1)
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 操作员ID(6),前补00
        /// </summary>

        public string OperatorId { get; set; }

        /// <summary>
        /// 有效截止时间(14),前补0
        /// </summary>
        public string LogCreateTime { get; set; }

        public string LogContent { get; set; }

        public OperateLogMessage()
        {
            msgType = MsgType.OperateLogData;
        }

        public OperateLogMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.OperateLogData;
        }

        public override void Decode()
        {
            base.Decode();
            DeviceId = GetNextString(8);
            LogLevel = GetNextString(1);
            LogType = GetNextString(1);
            OperatorId = GetNextString(6);
            LogContent = GetNextString(255);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(DeviceId, 8));
            encodeBuf.AddRange(AddString(LogLevel, 1));
            encodeBuf.AddRange(AddString(LogType, 1));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(LogContent, 255));
        }
    }
}