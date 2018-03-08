/**********************************************************
** 文件名： ToolPolygon.cs
** 文件作用:Polygon Tool
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
    /// Polygon tool
    /// </summary>
    public class ToolPolygon : ToolObject
    {
        public ToolPolygon()
        {
            Cursor = new Cursor(GetType(), "Pencil.cur");
        }

        private int lastX;
        private int lastY;
        private DrawPolygon newPolygon;
        private const int minDistance = 15 * 15;

        /// <summary>
        /// Left nouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            // Create new polygon, add it to the list
            // and keep reference to it
            newPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
            AddNewObject(drawArea, newPolygon);
            lastX = e.X;
            lastY = e.Y;
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if (e.Button != MouseButtons.Left)
                return;

            if (newPolygon == null)
                return;                 // precaution

            Point point = new Point(e.X, e.Y);
            int distance = (e.X - lastX) * (e.X - lastX) + (e.Y - lastY) * (e.Y - lastY);

            if (distance < minDistance)
            {
                // Distance between last two points is less than minimum -
                // move last point
                newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
            }
            else
            {
                // Add new point
                newPolygon.AddPoint(point);
                lastX = e.X;
                lastY = e.Y;
            }

            drawArea.Refresh();
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            newPolygon = null;

            base.OnMouseUp(drawArea, e);
        }
    }
}