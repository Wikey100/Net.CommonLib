/**********************************************************
** 文件名： DeviceInterface.cs
** 文件作用:车站设备绘制工具对外接口
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-03-07    xwj       增加
**
**********************************************************/

using DrawTools.Model;
using System.Collections.Generic;

namespace DrawTools.DB
{
    public interface IDeviceInterface
    {
        /// <summary>
        /// 获取线路所有车站列表信息
        /// </summary>
        /// <returns></returns>
        List<FilterItem> GetStationList(string lineId);

        List<LineModel> GetAllLines();

        /// <summary>
        /// 获取车站设备列表
        /// </summary>
        /// <param name="stationId">车站编码</param>
        /// <param name="mapType">车站地图类型(车站设备-device,网络设备-NetDataSet)</param>
        /// <returns></returns>
        List<StationMapModel> GetStationDeviceList(string stationId, string mapType);

        /// <summary>
        /// 保存车站设备地图数据
        /// </summary>
        /// <param name="model">设备Model</param>
        /// <returns></returns>
        bool SaveStationDevice(List<StationMapModel> model);
    }

    public class LineModel
    {
        public string LineName { get; set; }

        public string LineCode { get; set; }
    }
}