/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System.Collections.Generic;
using System.Text;

namespace Net.CommonLib.Message
{
    public class LccScopeFile : BaseMessage
    {
        public string FileVersion { get; set; }

        public string TransactionCount { get; set; }

        public List<string> TransactionList = new List<string>();

        public string FileName { get; set; }

        public string FileCreateTime { get; set; }

        public string[] TxnString { get; set; }

        public LccScopeFile()
        {
            msgType = MsgType.TxnDeviceTxnDataFile;
        }

        public LccScopeFile(string[] str)
        {
            msgType = MsgType.TxnDeviceTxnDataFile;
            var fileHeadStr = str[0];
            string[] heads = fileHeadStr.Split('\t');
            FileVersion = heads[0];
            TransactionCount = heads[1];
            FileName = heads[2];
            FileCreateTime = heads[3];
            for (int i = 1; i < str.Length; i++)
            {
                TransactionList.Add(str[i]);
            }
        }

        public string GernerateFile(string fileType, string Dir)
        {
            switch (fileType)
            {
                case "A0":
                case "A1":
                    string head = FileVersion + "\t" + TransactionCount.PadLeft(10, '0') + "\t" + FileName + "\t" + FileCreateTime;
                    string filePath = System.IO.Path.Combine(Dir, FileName);
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false, Encoding.ASCII))
                    {
                        sw.WriteLine(head);
                        foreach (var txn in TransactionList)
                        {
                            sw.WriteLine(txn);
                        }
                        sw.WriteLine("0".PadLeft(256, '0'));
                        sw.Flush();
                    }
                    return filePath;

                default:

                    break;
            }
            return null;
        }
    }
}