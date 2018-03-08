/**********************************************************
** 文件名： ToolObject.cs
** 文件作用:ToolObject
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.Command;
using DrawTools.Controls;
using DrawTools.DocToolkit;
using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    /// <summary>
    /// Base class for all tools which create new graphic object
    /// </summary>
    public abstract class ToolObject : Tool
    {
        private Cursor cursor;

        /// <summary>
        /// Tool cursor.
        /// </summary>
        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }

        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            try
            {
                drawArea.GraphicsList[0].Normalize();
                drawArea.AddCommandToHistory(new CommandAdd(drawArea.GraphicsList));
                drawArea.ActiveTool = DrawToolType.Pointer;

                drawArea.Capture = false;
                drawArea.Refresh();
            }
            catch { }
        }

        /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);
            string oName = o.GetType().Name;
            if (oName == "Switch" || oName == "SwitchPort")
            {
                drawArea.mapType = "NetDataSet";
            }
            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }
    }
}