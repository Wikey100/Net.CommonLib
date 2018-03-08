/**********************************************************
** 文件名： StationMapModel.cs
** 文件作用:车站设备节点配置参数实体
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

namespace DrawTools.Model
{
    public class StationMapModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string RecID { get; set; }

        /// <summary>
        /// 车站编号
        /// </summary>
        public string StationID { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备类型(01-AGM...)
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 设备子类型(主要区分AGM，双向、单向)
        /// </summary>
        public string DeviceSubType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string DeviceSeqInStation { get; set; }

        public string LobbyId { get; set; }

        public string GroupID { get; set; }

        public string DeviceSeqInGroup { get; set; }

        /// <summary>
        /// 位置-X
        /// </summary>
        public string XPos { get; set; }

        /// <summary>
        /// 位置-Y
        /// </summary>
        public string YPos { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAdd { get; set; }

        /// <summary>
        /// 设备宽度
        /// </summary>
        public string Device_W { get; set; }

        /// <summary>
        /// 设备高度
        /// </summary>
        public string Device_H { get; set; }

        /// <summary>
        /// 画板宽度
        /// </summary>
        public string Region_W { get; set; }

        /// <summary>
        /// 画板高度
        /// </summary>
        public string Region_H { get; set; }

        /// <summary>
        /// 设备角度  只针对agm
        /// </summary>
        public string Angle { get; set; }

        /// <summary>
        /// 文本大小
        /// </summary>
        public string TextFontSize { get; set; }

        /// <summary>
        /// 文本样式
        /// </summary>
        public string TextFonStyle { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        public string TextColor { get; set; }

        /// <summary>
        /// 双向agm摆放位置
        /// </summary>
        public string Entry { get; set; }

        /// <summary>
        /// 设备显示lable
        /// </summary>
        public string LableId { get; set; }

        /// <summary>
        /// 文本样式
        /// </summary>
        public string TextType { get; set; }

        /// <summary>
        /// 车站地图类型(区分设备与交换机)
        /// </summary>
        public string MapType { get; set; }

        public string UseFlag { get; set; }
    }
}