/**********************************************************
** 文件名： ToolSwitch.cs
** 文件作用:Switch Tool
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
    public class ToolSwitch : ToolRectangle
    {
        public ToolSwitch()
        {
            Cursor = new Cursor(GetType(), "Ellipse.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new Switch(e.X, e.Y, 414, 80));
        }
    }
}