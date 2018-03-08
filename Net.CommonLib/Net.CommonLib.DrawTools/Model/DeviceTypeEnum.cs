/**********************************************************
** 文件名： DeviceTypeEnum.cs
** 文件作用:设备类型枚举
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

namespace DrawTools.Model
{
    public enum DeviceTypeEnum
    {
        ES = 00,
        TVM = 01,
        BOM = 02,
        CUS = 03,
        TCM = 04,
        PCA = 05,
        AGM = 06,
        SC = 11,
        ARRAY = 14,
        AGM_Sub_AGMChannel = 07,//或08
        AGM_Sub_AGMChannelDual = 09,
        AGM_Sub_AGMWallDual = 72,  //自定义子类型
        AGM_Sub_AGMWallDummy = 73,  //自定义子类型
        AGM_Sub_AGMWallSingle = 71,  //自定义子类型
    }
}