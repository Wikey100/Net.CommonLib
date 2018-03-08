/**********************************************************
** 文件名： ToolRectangle.cs
** 文件作用:Rectangle Tool
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System.Drawing;
using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    public class ToolRectangle : ToolObject
    {
        public ToolRectangle()
        {
            Cursor = new Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawRectangle(e.X, e.Y, 1, 1));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    Point point = new Point(e.X, e.Y);
                    drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                    drawArea.Refresh();
                }
                catch { }
            }
        }
    }
}