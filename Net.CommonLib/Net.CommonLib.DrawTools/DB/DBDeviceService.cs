/**********************************************************
** 文件名： DBDeviceService.cs
** 文件作用:接收外部接口实现处理类
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-03-08    xwj       增加
**
**********************************************************/

namespace DrawTools.DB
{
    public class DBDeviceService
    {
        public static IDeviceInterface dbDevice { get; set; }

        public DBDeviceService(IDeviceInterface device)
        {
            dbDevice = device;
        }
    }
}