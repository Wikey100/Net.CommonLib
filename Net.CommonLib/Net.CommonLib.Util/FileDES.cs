/*******************************************************************
 * * 文件名： FileDES.cs
 * * 文件作用： 文件加密解密
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2018-03-10    xwj       新增
 * *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace Net.CommonLib.Util
{
    public class FileDES
    {
        /// <summary>
        /// 按照key，解密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        private static byte[] DecryptCipherText(byte[] key, byte[] cipherText)
        {
            TripleDES ta = TripleDES.Create();
            ta.KeySize = 0x80;
            ta.Key = key;
            ta.GenerateIV();
            ta.Mode = CipherMode.ECB;
            ta.Padding = PaddingMode.PKCS7;
            MemoryStream ms = new MemoryStream(cipherText);
            CryptoStream cs = new CryptoStream(ms, ta.CreateDecryptor(), CryptoStreamMode.Read);
            byte[] plainbytesT = new byte[cipherText.Length];
            int i = 0;
            try
            {
                int b;
                while ((b = cs.ReadByte()) != -1)
                {
                    plainbytesT[i] = (byte)b;
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            byte[] plainbytes = new byte[i];
            Array.Copy(plainbytesT, 0, plainbytes, 0, i);
            try
            {
                cs.Close();
                ms.Close();
            }
            catch { }
            return plainbytes;
        }

        /// <summary>
        /// 解密内容
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="nLenOfFile"></param>
        /// <returns></returns>
        public static string DecryptContent(byte[] buf, int nLenOfFile)
        {
            byte[] key = GetKey(4, 0x10, buf);
            byte[] cipherText = GetCipherText(20, nLenOfFile - 20, buf);
            cipherText = DecryptCipherText(key, cipherText);
            return Encoding.Default.GetString(cipherText);
        }

        /// <summary>
        /// 解密内容
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool DecryptContent(byte[] buf, ref byte[] content)
        {
            bool success = true;
            try
            {
                byte[] key = GetKey(4, 0x10, buf);
                byte[] cipherText = GetCipherText(20, buf.Length - 20, buf);
                content = DecryptCipherText(key, cipherText);
            }
            catch
            {
                //.log.Error( e.Message, e );
                success = false;
            }
            return success;
        }

        /// <summary>
        /// 解密一个fromFile文件，写到toFile上。
        /// </summary>
        /// <param name="fromFile"></param>
        /// <param name="toFile"></param>
        /// <returns></returns>
        public static bool DecryptFileTo(string fromFile, string toFile)
        {
            string content = DecryptFile(fromFile);
            FileStream fs = new FileStream(toFile, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
            return true;
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="prmFile"></param>
        /// <returns></returns>
        public static string DecryptFile(string prmFile)
        {
            byte[] buf = new byte[0x1388000];
            int nLenOfFile = GetCipherbytes(prmFile, ref buf);
            if (nLenOfFile <= 0)
            {
                Console.WriteLine("Encrypted file is wrong!");
                return null;
            }
            return DecryptContent(buf, nLenOfFile);
        }

        /// <summary>
        /// 加密一个文件，返回fileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetEncryptedFileStream(string fileName)
        {
            throw new Exception("不支持加密文件的导出");

            //throw new NotImplementedException();//lrh
            //string fileTmpDir = ConfigurationManager.AppSettings["File_TEMP"] + "crypted\\";
            //if (!Directory.Exists( fileTmpDir ))
            //{
            //    Directory.CreateDirectory( fileTmpDir );
            //};

            //FileStream fs = new FileStream( EncryptFile( fileName, fileTmpDir + new FileInfo( fileName ).Name + ".crypted" ), FileMode.Open, FileAccess.Read, FileShare.ReadWrite );

            //return fs;
        }

        /// <summary>
        /// 加密文件到某文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="toFileName"></param>
        /// <returns></returns>
        public static string EncryptFile(string fileName, string toFileName)
        {
            string plainText = GetPlainText(fileName);
            if (plainText == null)
            {
                Console.WriteLine(" file {0} is empty!", fileName);
                return null;
            }
            else
            {
                byte[] cipherbytes = EncryptPlainText(plainText);
                string cipherFile = toFileName;
                WriteCipherbytes(cipherbytes, cipherFile);
                return cipherFile;
            }
        }

        /// <summary>
        /// 加密字节
        /// </summary>
        /// <param name="plainbytes"></param>
        /// <returns></returns>
        public static byte[] EncryptPlainBytes(byte[] plainbytes)
        {
            TripleDES ta = TripleDES.Create();
            ta.KeySize = 128;
            ta.GenerateKey();
            ta.GenerateIV();
            ta.Mode = CipherMode.ECB;
            ta.Padding = PaddingMode.PKCS7;
            byte[] head = new byte[4];
            new Random().NextBytes(head);
            byte[] key = ta.Key;
            byte[] cipherbytes = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, ta.CreateEncryptor(), CryptoStreamMode.Write);
                //  byte[] plainbytes = Encoding.Default.GetBytes(plainText);
                cs.Write(plainbytes, 0, plainbytes.Length);
                cs.Close();
                byte[] cipherText = ms.ToArray();
                ms.Close();
                int len = cipherText.Length + 20;
                cipherbytes = new byte[len];
                head.CopyTo(cipherbytes, 0);
                key.CopyTo(cipherbytes, 4);
                cipherText.CopyTo(cipherbytes, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return cipherbytes;
        }

        /// <summary>
        /// 加密字符串返回加密后的字节
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static byte[] EncryptPlainText(string plainText)
        {
            TripleDES ta = TripleDES.Create();
            ta.KeySize = 128;
            ta.GenerateKey();
            ta.GenerateIV();
            ta.Mode = CipherMode.ECB;
            ta.Padding = PaddingMode.PKCS7;
            byte[] head = new byte[4];
            new Random().NextBytes(head);
            byte[] key = ta.Key;
            byte[] cipherbytes = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, ta.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] plainbytes = Encoding.Default.GetBytes(plainText);
                cs.Write(plainbytes, 0, plainbytes.Length);
                cs.Close();
                byte[] cipherText = ms.ToArray();
                ms.Close();
                int len = cipherText.Length + 20;
                cipherbytes = new byte[len];
                head.CopyTo(cipherbytes, 0);
                key.CopyTo(cipherbytes, 4);
                cipherText.CopyTo(cipherbytes, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return cipherbytes;
        }

        /// <summary>
        /// 取字节数
        /// </summary>
        /// <param name="prmFile"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        private static int GetCipherbytes(string prmFile, ref byte[] buf)
        {
            int len = 0;
            try
            {
                FileStream fs = new FileStream(prmFile, FileMode.Open);
                len = fs.Read(buf, 0, buf.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return len;
        }

        /// <summary>
        /// 取密文
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="len"></param>
        /// <param name="cipherbytes"></param>
        /// <returns></returns>
        private static byte[] GetCipherText(int pos, int len, byte[] cipherbytes)
        {
            byte[] ct = new byte[len];
            Array.Copy(cipherbytes, pos, ct, 0, len);
            return ct;
        }

        /// <summary>
        /// 取key
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="len"></param>
        /// <param name="cipherbytes"></param>
        /// <returns></returns>
        private static byte[] GetKey(int pos, int len, byte[] cipherbytes)
        {
            byte[] k = new byte[len];
            Array.Copy(cipherbytes, pos, k, 0, len);
            return k;
        }

        /// <summary>
        /// 取明文
        /// </summary>
        /// <param name="prmFile"></param>
        /// <returns></returns>
        private static string GetPlainText(string prmFile)
        {
            string plainText = null;
            try
            {
                StreamReader sr = new StreamReader(prmFile, Encoding.Default);
                plainText = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return plainText;
        }

        /// <summary>
        /// 写入加密key
        /// </summary>
        /// <param name="cipherbytes"></param>
        /// <param name="cipherFile"></param>
        private static void WriteCipherbytes(byte[] cipherbytes, string cipherFile)
        {
            try
            {
                FileStream fs = new FileStream(cipherFile, FileMode.OpenOrCreate);
                fs.Write(cipherbytes, 0, cipherbytes.Length);
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
