/**********************************************************
** 文件名： ToolArray.cs
** 文件作用:Array Tool
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    public class ToolArray : ToolRectangle
    {
        public ToolArray()
        {
            Cursor = new Cursor(GetType(), "Ellipse.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawTools.Device.Array(e.X, e.Y, 50, 50));
        }
    }
}