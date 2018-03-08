/**********************************************************
** 文件名： CommandFullZoom.cs
** 文件作用:全屏命令
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System;

namespace DrawTools.Command
{
    public class CommandFullZoom : Command
    {
        private float rate;

        public CommandFullZoom(GraphicsList graphicsList)
            : base(graphicsList)
        {
        }

        public void SetRate(float rate)
        {
            this.rate = rate;
        }

        public override void Execute()
        {
            int x_aris = 0, y_aris = 0, width = 0, height = 0, n = 0;
            graphicsList.SelectAll();
            n = graphicsList.SelectionCount;
            DrawObject obj = null;
            for (int i = n - 1; i >= 0; i--)
            {
                obj = (DrawObject)graphicsList.ASelection[i];
                x_aris = (int)(obj.GetRectangle().X * rate);
                y_aris = (int)(obj.GetRectangle().Y * rate);
                width = (int)(obj.GetRectangle().Width * rate);
                height = (int)(obj.GetRectangle().Height * rate);
                obj.setRectangleX(x_aris);
                obj.setRectangleY(y_aris);
                obj.setRectangleWidth(width);
                obj.setRectangleHeight(height);
            }
        }

        public override void Redo()
        {
            throw new NotImplementedException();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}