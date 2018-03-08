/**********************************************************
** 文件名： UndoManager.cs
** 文件作用:动态加载云服务接口动态库
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using System.Collections.Generic;

namespace DrawTools.DocToolkit
{
    /// <summary>
    /// Class is responsible for executing Undo - Redo operations
    /// 实际上，这个就是命令的操作者，他构造函数的参数应该是Command,而不是graphicsList.因为他主要的就是命令的持有者和操作者。
    /// 事务的实现就是通过它来实现的。（谢兴国注）
    /// </summary>
    public class UndoManager
    {
        #region Class Members

        // GraphicsList graphicsList;   老的命令模式用的
        private List<DrawTools.Command.Command> historyList;

        private int nextUndo;

        #endregion Class Members

        #region Constructor

        public UndoManager(GraphicsList graphicsList)
        {
            //  this.graphicsList = graphicsList;
            ClearHistory();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Return true if Undo operation is available
        /// </summary>
        public bool CanUndo
        {
            get
            {
                // If the NextUndo pointer is -1, no commands to undo
                if (nextUndo < 0 ||
                    nextUndo > historyList.Count - 1)   // precaution
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Return true if Redo operation is available
        /// </summary>
        public bool CanRedo
        {
            get
            {
                // If the NextUndo pointer points to the last item, no commands to redo
                if (nextUndo == historyList.Count - 1)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion Properties

        #region Public Functions

        /// <summary>
        /// Clear History
        /// </summary>
        public void ClearHistory()
        {
            historyList = new List<DrawTools.Command.Command>();
            nextUndo = -1;
        }

        /// <summary>
        /// Add new command to history.
        /// Called by client after executing some action.
        /// </summary>
        /// <param name="command"></param>
        public void AddCommandToHistory(DrawTools.Command.Command command)
        {
            // Purge history list
            this.TrimHistoryList();

            // Add command and increment undo counter
            historyList.Add(command);

            nextUndo++;
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }
            // Get the Command object to be undone
            DrawTools.Command.Command command = historyList[nextUndo];

            // Execute the Command object's undo method
            command.Undo();

            // Move the pointer up one item
            nextUndo--;
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            // Get the Command object to redo
            int itemToRedo = nextUndo + 1;
            DrawTools.Command.Command command = historyList[itemToRedo];

            // Execute the Command object
            command.Redo();

            // Move the undo pointer down one item
            nextUndo++;
        }

        #endregion Public Functions

        #region Private Functions

        private void TrimHistoryList()
        {
            // We can redo any undone command until we execute a new
            // command. The new command takes us off in a new direction,
            // which means we can no longer redo previously undone actions.
            // So, we purge all undone commands from the history list.*/

            // Exit if no items in History list
            if (historyList.Count == 0)
            {
                return;
            }

            // Exit if NextUndo points to last item on the list
            if (nextUndo == historyList.Count - 1)
            {
                return;
            }

            // Purge all items below the NextUndo pointer
            for (int i = historyList.Count - 1; i > nextUndo; i--)
            {
                historyList.RemoveAt(i);
            }
        }

        #endregion Private Functions
    }
}