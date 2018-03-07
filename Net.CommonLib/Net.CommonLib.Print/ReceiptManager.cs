/*******************************************************************
 * * 文件名： ReceiptManager.cs
 * * 文件作用：小票管理类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2016-04-01    xwj       新增
 * *******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Net.CommonLib.Print
{
    public class ReceiptManager
    {
        private List<Receipt> ReceiptList = new List<Receipt>();
        private Dictionary<string, Receipt> receiptDict = new Dictionary<string, Receipt>();

        public Dictionary<string, Receipt> ReceiptDict
        {
            get { return receiptDict; }
            set { receiptDict = value; }
        }

        public static ReceiptManager Instance = new ReceiptManager();

        private NotePrinter printer;

        public NotePrinter Printer
        {
            get { return printer; }
            set { printer = value; }
        }

        private ReceiptManager()
        {
        }

        public void SetPrint(string receiptName, bool isPrint)
        {
            this.receiptDict[receiptName].Print = isPrint ? "1" : "0";
        }

        public void Print(string receiptName, string[] args)
        {
            printer.Print(GetReceipt(receiptName, args));
            Slid();
        }

        public void Print(string receipt)
        {
            printer.Print(receipt);
            Slid();
        }

        /// <summary>
        /// 滑动打印
        /// </summary>
        public void Slid()
        {
            printer.SlidePaper(0xb0);
        }

        public void Add(Receipt r)
        {
            ReceiptList.Add(r);
        }

        public string GetReceipt(string receiptName, string[] paras)
        {
            return receiptDict[receiptName].GetReceipt(paras);
        }

        public string GetReceipt(string receiptName)
        {
            return receiptDict[receiptName].GetReceipt();
        }

        public void Serialize(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Receipt>));
                serializer.Serialize(fs, ReceiptList);
            }
        }

        public void DeSerial(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                ReceiptList = new List<Receipt>();
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Receipt>));
                    ReceiptList = (List<Receipt>)serializer.Deserialize(stream);
                }
                catch { }
            }
            receiptDict.Clear();
            foreach (Receipt r in ReceiptList)
            {
                receiptDict.Add(r.ReceiptName, r);
            }
        }
    }

    /// <summary>
    /// 小票实体类
    /// </summary>
    [Serializable]
    public class Receipt
    {
        /// <summary>
        /// 小票名称
        /// </summary>
        [XmlAttribute("Name")]
        public string ReceiptName;

        /// <summary>
        /// 小票内容
        /// </summary>
        [XmlAttribute("Content")]
        public string ReceiptContent;

        /// <summary>
        ///  是否打印
        /// </summary>
        [XmlAttribute("Print")]
        public string Print;

        public string GetReceipt(string[] args)
        {
            return string.Format(ReceiptContent, args);
        }

        public string GetReceipt()
        {
            return ReceiptContent;
        }
    }
}