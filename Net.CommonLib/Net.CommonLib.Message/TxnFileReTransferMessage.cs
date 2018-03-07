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
    public class TxnFileReTransferMessage : BaseMessage
    {
        /// <summary>
        /// 时间（2）
        /// </summary>
        public string OccurTime { get; set; }

        /// <summary>
        /// 命令代码(4)
        /// </summary>
        public string CommandCode { get; set; }

        /// <summary>
        /// 数据类型CHAR(8)
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 命令号
        /// </summary>
        public string CommandNo { get; set; }

        public TxnFileReTransferMessage()
        {
            //  msgType = "8001";
            msgType = MsgType.CmdFileUploadRequest;
        }

        public TxnFileReTransferMessage(byte[] bytes)
            : base(bytes)
        {
            msgType = MsgType.CmdFileUploadRequest;
        }

        public override void Decode()
        {
            base.Decode();
            OccurTime = GetNextString(14);
            CommandCode = GetNextString(4);
            DataType = GetNextString(2);
            SerialNo = GetNextString(8);
            CommandNo = GetNextString(16);
        }

        public override void Encode()
        {
            encodeBuf.Clear();
            encodeBuf.AddRange(AddString(OccurTime, 14));
            encodeBuf.AddRange(AddString(CommandCode, 4));
            encodeBuf.AddRange(AddString(DataType, 2));
            encodeBuf.AddRange(AddString(SerialNo, 8));
            encodeBuf.AddRange(AddString(CommandNo.PadLeft(16, '0'), 16));
        }
    }
}