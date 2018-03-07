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
    public class PasswordChangeMessage : BaseMessage
    {
        /// <summary>
        /// 消息代号8200
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 操作员编号
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 采集时间CHAR(14)
        /// </summary>
        public string ChangeTime { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public PasswordChangeMessage()
        {
            msgType = MsgType.PasswordChange;
        }

        public PasswordChangeMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.PasswordChange;
        }

        public override void Decode()
        {
            base.Decode();
            MessageCode = GetNextString(4);
            StationId = GetNextString(4);
            OperatorId = GetNextString(6);
            ChangeTime = GetNextString(14);
            OldPassword = GetNextString(32);
            NewPassword = GetNextString(32);
        }

        public override void Encode()
        {
            encodeBuf.Clear();

            encodeBuf.AddRange(AddString(MessageCode, 4));
            encodeBuf.AddRange(AddString(StationId, 4));
            encodeBuf.AddRange(AddString(OperatorId, 6));
            encodeBuf.AddRange(AddString(ChangeTime, 14));
            encodeBuf.AddRange(AddString(OldPassword, 32));
            encodeBuf.AddRange(AddString(NewPassword, 32));
        }
    }
}