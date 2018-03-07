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
    public class FileUploadRequestMessage : BaseMessage
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
        public string FileType { get; set; }

        /// <summary>
        /// 预留,填0
        /// </summary>

        public string RequestDate { get; set; }

        /// <summary>
        /// 预留，填0
        /// </summary>
        public string CmdData { get; set; }

        public FileUploadRequestMessage()
        {
            msgType = MsgType.CmdFileUploadRequest;
        }

        public FileUploadRequestMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.CmdFileUploadRequest;
        }

        public override void Decode()
        {
            base.Decode();
            CmdSendTime = GetNextString(14);
            CmdCode = GetNextString(4);
            FileType = GetNextString(2);
            RequestDate = GetNextString(8);
            CmdData = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(CmdSendTime, 14));
            encodeBuf.AddRange(AddString(CmdCode, 4));
            encodeBuf.AddRange(AddString(FileType, 2));
            encodeBuf.AddRange(AddString(RequestDate, 8));
            encodeBuf.AddRange(AddString(CmdData.PadLeft(16, '0'), 16));
        }
    }
}