/**********************************************************
** 文件名： CommandDeleteAll.cs
** 文件作用:Delete All command
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System.Collections.Generic;

namespace DrawTools.Command
{
    public class CommandDeleteAll : Command
    {
        private List<DrawObject> cloneList;

        public CommandDeleteAll(GraphicsList graphiList)
            : base(graphiList)
        {
            cloneList = new List<DrawObject>();

            // Make clone of the whole list.
            // Add objects in reverse order because GraphicsList.Add
            // insert every object to the beginning.
            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                cloneList.Add(graphicsList[i].Clone());
            }
        }

        public override void Execute()
        {
            graphicsList.BomVerify.Clear();
            graphicsList.AGMVerify.Clear();
            //graphicsList.AGMDualVerify.Clear();
            graphicsList.TCMVerify.Clear();
            graphicsList.TVMVerify.Clear();
            graphicsList.SCVerify.Clear();
            DrawObject._objIdInc = 1;
            graphicsList.Clear();
            //LogHelper.DeviceDeviceLogInfo(string.Format("删除车站:{0}内所有设备配置信息",));
        }

        public override void Undo()
        {
            // Add all objects from clone list to list -
            // opposite to DeleteAll
            foreach (DrawObject o in cloneList)
            {
                graphicsList.Add(o);
            }
        }

        public override void Redo()
        {
            // Clear list - make DeleteAll again
            graphicsList.BomVerify.Clear();
            graphicsList.AGMVerify.Clear();
            //graphicsList.AGMDualVerify.Clear();
            graphicsList.TCMVerify.Clear();
            graphicsList.TVMVerify.Clear();
            DrawObject._objIdInc = 1;
            graphicsList.Clear();
        }
    }
}