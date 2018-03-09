/*******************************************************************
 * * 文件名： FileHelper.cs
 * * 文件作用： 文件处理帮助类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2018-03-10    xwj       新增
 * *******************************************************************/
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Net.CommonLib.Util
{
    public class FileHelper
    {
        public static void CreateZipFile(string filesPath, string zipFilePath)
        {

            if (!Directory.Exists(filesPath))
            {
                throw new DirectoryNotFoundException("指定目录不存在");
            }

            try
            {
                string[] filenames = Directory.GetFiles(filesPath);
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                {

                    s.SetLevel(9); // 压缩级别 0-9
                    byte[] buffer = new byte[4096]; //缓冲区大小
                    foreach (string file in filenames)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("压缩文件失败", ex.InnerException);
            }
        }


        public static void UnZipFile(string zipFilePath, string unZipDir)
        {
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("文件不存在！");
            }
            if (!Directory.Exists(unZipDir))
            {
                Directory.CreateDirectory(unZipDir);
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                string outPutDir = unZipDir;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(Path.Combine(outPutDir, theEntry.Name)))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        public static byte[] ReadFile(string filePath, bool enctypted)
        {
            byte[] buf = null;

            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    buf = new byte[fs.Length];
                    fs.Read(buf, 0, (int)fs.Length);
                    fs.Close();
                    if (enctypted)
                    {
                        //  buf = FileDES.EncryptPlainBytes(buf);
                    }
                    return buf;
                }
            }
            throw new FileNotFoundException();
        }

        public static string WriteFile(string fileName, byte[] buf, bool encrypted)
        {
            string dir = Path.Combine(@"D:\test\", "prmtes");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string filePath = Path.Combine(dir, fileName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            return filePath;
        }
    }
}
