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
    /// <summary>
    /// 模式变更通知
    /// </summary>
    public class RunningModeChangeNotifyMessage : BaseMessage
    {
        /// <summary>
        /// 消息代码 （4） 9805
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 原始模式CHAR(2)
        /// </summary>
        public string OriginalMode { get; set; }

        /// <summary>
        /// 新模式CHAR(2)
        /// </summary>
        public string NewMode { get; set; }

        /// <summary>
        /// 车站编号(4)
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 模式开始时间（14）
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 备注（50）
        /// </summary>
        public string Remark { get; set; }

        public RunningModeChangeNotifyMessage()
        {
            msgType = MsgType.AccModeChangeNotify;
        }

        public RunningModeChangeNotifyMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.AccModeChangeNotify;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            OriginalMode = GetNextString(2);
            NewMode = GetNextString(2);
            StationId = GetNextString(4);
            StartTime = GetNextString(14);
            Remark = GetNextString(50);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(OriginalMode, 2));
            encodeBuf.AddRange(AddString(NewMode, 2));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(StartTime, 14));
            encodeBuf.AddRange(AddString(Remark.PadLeft(50, '0'), 50));
        }
    }
}