/**********************************************************
** 文件名： CommandDelete.cs
** 文件作用:Delete command
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.Device;
using DrawTools.DocToolkit;
using DrawTools.Log;
using System.Collections.Generic;

namespace DrawTools.Command
{
    public class CommandDelete : Command
    {
        private List<DrawObject> cloneList;

        public CommandDelete(GraphicsList graphiList)
            : base(graphiList)
        {
            cloneList = new List<DrawObject>();
            foreach (DrawObject o in graphicsList.Selection)
            {
                cloneList.Add(o.Clone());
            }
        }

        public override void Execute()
        {
            foreach (DrawObject o in cloneList)
            {
                if (o.GetType().Name.Equals("Text"))
                {
                    Text label = (Text)o;
                    switch (label.TextType)
                    {
                        case "BOM_Logical_ID":
                            graphicsList.BomVerify.Remove(label.Texttest);
                            graphicsList.BomVerify.Remove(label.Texttest.Substring(label.Texttest.Length - 2)); //同时删除文本相关设备列表中的数据
                            break;

                        case "AGM_Logical_ID":
                            graphicsList.AGMVerify.Remove(label.Texttest);
                            graphicsList.AGMVerify.Remove(label.Texttest.Substring(label.Texttest.Length - 2));
                            break;

                        case "AGMDual_Logical_ID":
                            graphicsList.AGMVerify.Remove(label.Texttest);
                            graphicsList.AGMVerify.Remove(label.Texttest.Substring(label.Texttest.Length - 2));
                            break;

                        case "AGMWallDual_ID":
                            graphicsList.AGMWallDualVerify.Remove(label.Texttest);
                            break;

                        case "AGMWallDummy_Logical_ID":
                            graphicsList.AGMWallDummyVerify.Remove(label.Texttest);
                            break;

                        case "AGMWallSingle_Logical_ID":
                            graphicsList.AGMWallSingleVerify.Remove(label.Texttest);
                            break;

                        case "TVM_Logical_ID":
                            graphicsList.TVMVerify.Remove(label.Texttest);
                            graphicsList.TVMVerify.Remove(label.Texttest.Substring(label.Texttest.Length - 2));
                            break;

                        case "TCM_Logical_ID":
                            graphicsList.TCMVerify.Remove(label.Texttest);
                            graphicsList.TCMVerify.Remove(label.Texttest.Substring(label.Texttest.Length - 2));
                            break;

                        case "SC_Logical_ID":
                            graphicsList.SCVerify.Remove(label.Texttest);
                            graphicsList.SCVerify.Remove(label.Texttest.Substring(label.Texttest.Length - 2));
                            break;

                        default:
                            break;
                    }
                    graphicsList.SelectDevice(label.ObjectID);
                }
                else
                {
                    switch (o.GetType().Name)
                    {
                        case "BOM":
                            graphicsList.BomVerify.Remove(o.LogicIDTail);
                            graphicsList.IPVerify.Remove(o.DeviceIP);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除BOM设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "AGMChannel":
                            graphicsList.AGMVerify.Remove(o.LogicIDTail);
                            graphicsList.IPVerify.Remove(o.DeviceIP);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除AGMChannel设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "AGMChannelDual":
                            graphicsList.AGMVerify.Remove(o.LogicIDTail);
                            graphicsList.IPVerify.Remove(o.DeviceIP);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除AGMChannelDual设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "AGMWallDual":
                            graphicsList.AGMWallDualVerify.Remove(o.LogicIDTail);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除AGMWallDual设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "AGMWallDummy":
                            graphicsList.AGMWallDummyVerify.Remove(o.LogicIDTail);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除AGMWallDummy设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "AGMWallSingle":
                            graphicsList.AGMWallSingleVerify.Remove(o.LogicIDTail);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除AGMWallSingle设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "TCM":
                            graphicsList.TCMVerify.Remove(o.LogicIDTail);
                            graphicsList.IPVerify.Remove(o.DeviceIP);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除TCM设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "TVM":
                            graphicsList.TVMVerify.Remove(o.LogicIDTail);
                            graphicsList.IPVerify.Remove(o.DeviceIP);
                            graphicsList.SelectAlist(o.TagIDBase);
                            LogHelper.DeviceDeviceLogInfo(string.Format("删除TVM设备配置信息_设备编号:{0},设备IP地址:{1}", o.LogicIDTail, o.DeviceIP));
                            break;

                        case "SC":
                            graphicsList.SCVerify.Remove(o.LogicIDTail);
                            graphicsList.IPVerify.Remove(o.DeviceIP);
                            graphicsList.SelectAlist(o.TagIDBase);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public override void Undo()
        {
            graphicsList.UnselectAll();
            foreach (DrawObject o in cloneList)
            {
                graphicsList.Add(o);
            }
        }

        public override void Redo()
        {
            int n = graphicsList.Count;
            for (int i = n - 1; i >= 0; i--)
            {
                bool toDelete = false;
                DrawObject objectToDelete = graphicsList[i];
                foreach (DrawObject o in cloneList)
                {
                    if (objectToDelete.ID == o.ID)
                    {
                        toDelete = true;
                        break;
                    }
                }
                if (toDelete)
                {
                    graphicsList.RemoveAt(i);
                }
            }
        }
    }
}