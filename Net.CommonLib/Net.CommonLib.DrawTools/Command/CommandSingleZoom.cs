/**********************************************************
** 文件名： CommandSingleZoom.cs
** 文件作用:单屏命令
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
    public class CommandSingleZoom : Command
    {
        private float rate;

        public CommandSingleZoom(GraphicsList graphicslist)
            : base(graphicslist)
        {
        }

        public void SetRate(float rate)
        {
            this.rate = rate;
        }

        public override void Execute()
        {
            int width = 0, height = 0, n = 0;
            n = graphicsList.SelectionCount;
            if (n > 0)
            {
                DrawObject obj = null;
                for (int i = n - 1; i >= 0; i--)
                {
                    obj = (DrawObject)graphicsList.ASelection[i];
                    width = (int)(obj.GetRectangle().Width * rate);
                    height = (int)(obj.GetRectangle().Height * rate);
                    obj.setRectangleWidth(width);
                    obj.setRectangleHeight(height);
                }
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