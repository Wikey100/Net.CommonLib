/**********************************************************
** 文件名： ToolPort.cs
** 文件作用:Port  Tool
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.Device;
using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    public class ToolPort : ToolRectangle
    {
        public ToolPort()
        {
            Cursor = new Cursor(GetType(), "Ellipse.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new SwitchPort(e.X, e.Y, 45, 45));
        }
    }
}