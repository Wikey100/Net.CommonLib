/**********************************************************
** 文件名： Command.cs
** 文件作用:操作命令基类
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

namespace DrawTools.Command
{
    /// <summary>
    /// Base class for commands used for Undo - Redo
    /// </summary>
    public abstract class Command
    {
        protected GraphicsList graphicsList;

        public Command(GraphicsList graphicsList)
        {
            this.graphicsList = graphicsList;
        }

        public abstract void Execute();

        public abstract void Undo();

        public abstract void Redo();
    }
}