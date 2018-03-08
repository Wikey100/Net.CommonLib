/**********************************************************
** 文件名： Serializer.cs
** 文件作用: 配置文件序列化类
**
**------------------------------------------------------------------------------
**修改历史记录：
**修改时间   修改人    修改内容概要
**2013-04-22    xwj       新增
**********************************************************/

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Net.CommonLib.Util
{
    /// <summary>
    /// 配置文件序列化类
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <returns>反序列化对象</returns>
        public static object DeSerialise<T>(string fileName)
        {
            using (FileStream stream = new FileStream(GetConfigFilePath(fileName), FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 解析资源文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static object DeSerialiseRes<T>(string resName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="item">The ?.</param>
        public static void Serialize<T>(string fileName, object item)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fs, item);
            }
        }

        /// <summary>
        /// Gets the config file path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetConfigFilePath(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "XmlConfig", fileName);
        }

        /// <summary>
        /// 根据文件名获取文件内容
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetText(string fileName)
        {
            using (FileStream stream = new FileStream(GetConfigFilePath(fileName), FileMode.Open, FileAccess.Read))
            {
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string str = sr.ReadToEnd();
                return str;
            }
        }
    }
}