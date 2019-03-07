/*******************************************************************
 * * 文件名： ActiveMQModel.cs
 * * 文件作用： ActiveMQ 实体类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2019-03-06    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.ActiveMqQueue
{
    public class MQModel
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 接口参数
        /// </summary>
        public string ContenJson { get; set; }
    }
}