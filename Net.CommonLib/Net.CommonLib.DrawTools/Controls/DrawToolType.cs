/**********************************************************
** 文件名： DrawToolType.cs
** 文件作用:设备工具枚举
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-05-31    xwj       增加
**
**********************************************************/

namespace DrawTools.Controls
{
    public enum DrawToolType
    {
        Pointer,
        Rectangle,
        Ellipse,
        Line,
        Polygon,
        Bom,
        AGMChannel,
        AGMChannelDual,
        AGMWallDual,
        AGMWallDummy,
        AGMWallSingle,
        Array,
        PaidArea,
        SC,
        TCM,
        TVM,
        Text,
        Switch,//lrh
        Port,//lrh
        NumberOfDrawTools
    }
}