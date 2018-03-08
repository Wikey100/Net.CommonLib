/**********************************************************
** 文件名： CommandAdd.cs
** 文件作用:添加命令
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
    /// <summary>
    /// Add new object command
    /// </summary>
    public class CommandAdd : Command
    {
        private DrawObject drawObject;

        public CommandAdd(GraphicsList graphiList)
            : base(graphiList)
        {
            // Keep copy of added object
            this.drawObject = (DrawObject)graphicsList[0].Clone();
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override void Undo()
        {
            graphicsList.DeleteLastAddedObject();
        }

        public override void Redo()
        {
            graphicsList.UnselectAll();
            graphicsList.Add(drawObject);
        }
    }
}