/**********************************************************
** 文件名： AGMChannelEntryHelp.cs
** 文件作用:确认双向AGM相对付费区位置
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-05-05    xwj       增加
**
**********************************************************/

namespace DrawTools.DocToolkit
{
    public class AGMChannelEntryHelp
    {
        /// <summary>
        /// 转换双向AGM相对付费区位置(根据与付费区X、Y坐标相对位置，确定双向AGM在付费区UP或DOWN或LEFT或RIGHT位置)
        /// </summary>
        /// <param name="paidX">付费区X坐标</param>
        /// <param name="paidY">付费区Y坐标</param>
        /// <param name="paidHeight">付费区高度</param>
        /// <param name="paidWdith">付费区宽度</param>
        /// <param name="deviceCenterPointX">双向AGM设备中间点X坐标</param>
        /// <param name="deviceCenterPointY">双向AGM设备中间点Y坐标</param>
        /// <returns></returns>
        public static string ConvertAgmChannelDualDirection(int paidX, int paidY, int paidHeight, int paidWdith, int deviceCenterPointX, int deviceCenterPointY)
        {
            string entry = string.Empty;
            int paidMax_X = paidX + paidWdith;
            int paidMax_Y = paidY + paidHeight;
            if (deviceCenterPointX < paidX)
            {
                entry = "LEFT";
            }
            else if (deviceCenterPointX > paidX && deviceCenterPointX > paidMax_X)
            {
                entry = "RIGHT";
            }
            else if (deviceCenterPointX > paidX && deviceCenterPointX < paidMax_X && deviceCenterPointY < paidY)
            {
                entry = "UP";
            }
            else if (deviceCenterPointX > paidX && deviceCenterPointX < paidMax_X && deviceCenterPointY > paidMax_Y)
            {
                entry = "DOWN";
            }
            else
            {
                entry = "RIGHT";
            }
            return entry;
        }
    }
}