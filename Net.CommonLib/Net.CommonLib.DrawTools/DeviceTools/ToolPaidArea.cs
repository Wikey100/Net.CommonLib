/**********************************************************
** 文件名： ToolPaidArea.cs
** 文件作用:PaidArea Tool
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
    public class ToolPaidArea : ToolRectangle
    {
        public ToolPaidArea()
        {
            Cursor = new Cursor(GetType(), "Line.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new PaidArea(e.X, e.Y, 150, 100));
        }
    }
}