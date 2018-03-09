/**********************************************************
** 文件名： Encrypt.cs
** 文件作用: 
**
**------------------------------------------------------------------------------
**修改历史记录：
**修改时间   修改人    修改内容概要
**2018-03-10    xwj       新增
**********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Net.CommonLib.Util
{
    public class Encrypt
    {
        // 声明对称算法变量
        private SymmetricAlgorithm mCSP;
        // 初始化向量
        private const string CIV = "Wo8l/+0RuByR12se6Net111A";
        // 密钥（常量）
        private const string CKEY = "unWzLS9D/8i=";

        /// <summary>
        /// 实例化
        /// </summary>
        public Encrypt()
        {
            //定义访问数据加密标准 (DES) 算法的加密服务提供程序 (CSP) 版本的包装对象,此类是SymmetricAlgorithm的派生类
            mCSP = new DESCryptoServiceProvider();
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="Value">需加密的字符串</param>
        /// <returns></returns>
        public string EncryptString(string Value)
        {
            // 定义基本的加密转换运算
            ICryptoTransform ct;
            // 定义内存流
            MemoryStream ms;
            // 定义将内存流链接到加密转换的流
            CryptoStream cs;
            byte[] byt;
            // CreateEncryptor创建(对称数据)加密对象
            // 用指定的密钥和初始化向量创建对称数据加密标准
            ct = mCSP.CreateEncryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));
            // 将Value字符转换为UTF-8编码的字节序列
            byt = Encoding.UTF8.GetBytes(Value);
            // 创建内存流
            ms = new MemoryStream();
            // 将内存流链接到加密转换的流
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            // 写入内存流
            cs.Write(byt, 0, byt.Length);
            // 将缓冲区中的数据写入内存流，并清除缓冲区
            cs.FlushFinalBlock();
            // 释放内存流
            cs.Close();
            // 将内存流转写入字节数组并转换为string字符
            return Convert.ToBase64String(ms.ToArray());
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">要解密的字符串</param>
        /// <returns>string</returns>
        public string DecryptString(string Value)
        {
            // 定义基本的加密转换运算
            ICryptoTransform ct;
            // 定义内存流
            MemoryStream ms;
            // 定义将数据流链接到加密转换的流
            CryptoStream cs;
            byte[] byt;
            // 用指定的密钥和初始化向量创建对称数据解密标准
            ct = mCSP.CreateDecryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));
            // 将Value(Base 64)字符转换成字节数组
            byt = Convert.FromBase64String(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            // 将字节数组中的所有字符解码为一个字符串
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
