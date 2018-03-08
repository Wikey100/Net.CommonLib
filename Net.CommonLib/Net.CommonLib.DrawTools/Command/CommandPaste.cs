/**********************************************************
** 文件名： CommandPaste.cs
** 文件作用:黏贴命令
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.Device;
using DrawTools.DocToolkit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrawTools.Command
{
    public class CommandPaste : Command
    {
        private List<DrawObject> copyList;
        private int pointX;
        private int PointY;

        public CommandPaste(GraphicsList graphiList)
            : base(graphiList)
        {
        }

        public void SetMousePosition(int xPosition, int yPosition)
        {
            this.pointX = xPosition;
            this.PointY = yPosition;
        }

        public override void Execute()
        {
            copyList = graphicsList.CopyList;
            int n = copyList.Count;
            int relative_X = copyList[0].GetRectangle().X;
            int relative_Y = copyList[0].GetRectangle().Y;
            for (int i = 0; i < n; i++)
            {
                int obX_aris = copyList[i].GetRectangle().X - relative_X + pointX;
                int obY_aris = copyList[i].GetRectangle().Y - relative_Y + PointY;
                copyList[i].setRectangleX(obX_aris);
                copyList[i].setRectangleY(obY_aris);
                AddNewObject(copyList[i]);
            }
        }

        protected void AddNewObject(DrawObject o)
        {
            graphicsList.UnselectAll();
            o.Selected = true;
            int listCount = -1;
            int maxValue = 0;
            int left = 0, top = 0, tagid = -1, intflag = -1;
            string id = "";
            string name = o.GetType().Name;
            switch (name)
            {
                case "BOM":
                    listCount = graphicsList.BomVerify.Count;
                    if (listCount > 0)
                    {
                        var max = Int32.Parse(graphicsList.BomVerify.Max());
                        maxValue = Int32.Parse(listCount > 0 ? graphicsList.BomVerify.Max() : "0", System.Globalization.NumberStyles.HexNumber); //十六进制转十进制
                        if (maxValue == 255)  //限制只能添加两位十六进制位数
                        {
                            graphicsList.BomVerify.Remove("ff");
                            graphicsList.BomVerify.Remove("FF");
                            maxValue = Int32.Parse(graphicsList.BomVerify.Max(), System.Globalization.NumberStyles.HexNumber); //取第二大值
                        }
                        BOM bom = (BOM)o.Clone(maxValue + 1);
                        left = bom.RectangleLs.X;
                        top = bom.RectangleLs.Y;
                        id = bom.LogicIDTail;
                        tagid = bom.TagIDBase;
                        intflag = bom.Flag;

                        if (tagid != 0)
                        {
                            //同步标志位
                            bom.TagIDBase = intflag;
                            Text bomprop = new Text();
                            bomprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            bomprop.Texttest = "";// id;
                            bomprop.setTextDisplay(left, top - 25);
                            bomprop.ObjectID = intflag;
                            bomprop.TextType = "BOM_Logical_ID";
                            this.graphicsList.Add(bom);
                            this.graphicsList.Add(bomprop);
                        }
                        else
                        {
                            this.graphicsList.Add((BOM)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((BOM)o.Clone());
                    }
                    break;

                case "AGMChannel":
                    listCount = graphicsList.AGMVerify.Count;
                    if (listCount > 0)
                    {
                        AGMChannel agmChannel = (AGMChannel)o.Clone(maxValue + 1);
                        left = agmChannel.RectangleLs.X;
                        top = agmChannel.RectangleLs.Y;
                        id = agmChannel.LogicIDTail;
                        intflag = agmChannel.Flag;
                        tagid = agmChannel.TagIDBase;
                        if (tagid != 0)
                        {
                            //同步标志位
                            agmChannel.TagIDBase = intflag;
                            Text agmChannelprop = new Text();
                            agmChannelprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            agmChannelprop.setTextDisplay(left, top - 25);
                            agmChannelprop.ObjectID = intflag;
                            agmChannelprop.TextType = "AGM_Logical_ID";
                            this.graphicsList.Add(agmChannel);
                            this.graphicsList.Add(agmChannelprop);
                        }
                        else
                        {
                            this.graphicsList.Add((AGMChannel)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((AGMChannel)o.Clone());
                    }
                    break;

                case "AGMChannelDual":
                    listCount = graphicsList.AGMVerify.Count;
                    if (listCount > 0)
                    {
                        AGMChannelDual agmChannelDual = (AGMChannelDual)o.Clone(maxValue + 1);
                        left = agmChannelDual.RectangleLs.X;
                        top = agmChannelDual.RectangleLs.Y;
                        id = agmChannelDual.LogicIDTail;
                        intflag = agmChannelDual.Flag;
                        tagid = agmChannelDual.TagIDBase;

                        if (tagid != 0)
                        {
                            //同步标志位
                            agmChannelDual.TagIDBase = intflag;
                            Text agmChannelDualprop = new Text();
                            agmChannelDualprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            agmChannelDualprop.setTextDisplay(left, top - 25);
                            agmChannelDualprop.ObjectID = intflag;
                            agmChannelDualprop.TextType = "AGMDual_Logical_ID";
                            this.graphicsList.Add(agmChannelDual);
                            this.graphicsList.Add(agmChannelDualprop);
                        }
                        else
                        {
                            this.graphicsList.Add((AGMChannelDual)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((AGMChannelDual)o.Clone());
                    }
                    break;

                case "AGMWallSingle":
                    listCount = graphicsList.AGMWallSingleVerify.Count;

                    if (listCount > 0)
                    {
                        AGMWallSingle agmWallSingle = (AGMWallSingle)o.Clone(maxValue + 1);
                        left = agmWallSingle.RectangleLs.X;
                        top = agmWallSingle.RectangleLs.Y;
                        id = agmWallSingle.LogicIDTail;
                        intflag = agmWallSingle.Flag;
                        tagid = agmWallSingle.TagIDBase;

                        if (tagid != 0)
                        {
                            //同步标志位
                            agmWallSingle.TagIDBase = intflag;
                            Text agmChannelDualprop = new Text();
                            agmChannelDualprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            agmChannelDualprop.setTextDisplay(left, top - 25);
                            agmChannelDualprop.ObjectID = intflag;
                            agmChannelDualprop.TextType = "AGMWallSingle_Logical_ID";
                            this.graphicsList.Add(agmWallSingle);
                            this.graphicsList.Add(agmChannelDualprop);
                        }
                        else
                        {
                            this.graphicsList.Add((AGMWallSingle)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((AGMWallSingle)o.Clone());
                    }
                    break;

                case "AGMWallDual":
                    listCount = graphicsList.AGMWallDualVerify.Count;

                    if (listCount > 0)
                    {
                        AGMWallDual agmWallDual = (AGMWallDual)o.Clone(maxValue + 1);
                        left = agmWallDual.RectangleLs.X;
                        top = agmWallDual.RectangleLs.Y;
                        id = agmWallDual.LogicIDTail;
                        intflag = agmWallDual.Flag;
                        tagid = agmWallDual.TagIDBase;
                        if (tagid != 0)
                        {
                            //同步标志位
                            agmWallDual.TagIDBase = intflag;
                            Text agmChannelDualprop = new Text();
                            agmChannelDualprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            agmChannelDualprop.setTextDisplay(left, top - 25);
                            agmChannelDualprop.ObjectID = intflag;
                            agmChannelDualprop.TextType = "AGMWallDual_Logical_ID";
                            this.graphicsList.Add(agmWallDual);
                            this.graphicsList.Add(agmChannelDualprop);
                            if (HasInclude(graphicsList.AGMVerify, id))
                            {
                                MessageBox.Show("设备ID重复，同一车站同一设备类型不能有重复的设备ID！");
                            }
                        }
                        else
                        {
                            this.graphicsList.Add((AGMWallDual)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((AGMWallDual)o.Clone());
                    }
                    break;

                case "AGMWallDummy":
                    listCount = graphicsList.AGMWallDummyVerify.Count;

                    if (listCount > 0)
                    {
                        AGMWallDummy agmWallDummy = (AGMWallDummy)o.Clone(maxValue + 1);
                        left = agmWallDummy.RectangleLs.X;
                        top = agmWallDummy.RectangleLs.Y;
                        id = agmWallDummy.LogicIDTail;
                        intflag = agmWallDummy.Flag;
                        tagid = agmWallDummy.TagIDBase;

                        if (tagid != 0)
                        {
                            //同步标志位
                            agmWallDummy.TagIDBase = intflag;
                            Text agmChannelDualprop = new Text();
                            agmChannelDualprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            agmChannelDualprop.setTextDisplay(left, top - 25);
                            agmChannelDualprop.ObjectID = intflag;
                            agmChannelDualprop.TextType = "AGMWallDummy_Logical_ID";
                            this.graphicsList.Add(agmWallDummy);
                            this.graphicsList.Add(agmChannelDualprop);
                        }
                        else
                        {
                            this.graphicsList.Add((AGMWallDummy)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((AGMWallDummy)o.Clone());
                    }
                    break;

                case "TCM":
                    listCount = graphicsList.TCMVerify.Count;
                    if (listCount > 0)
                    {
                        TCM tcm = (TCM)o.Clone(maxValue + 1);
                        left = tcm.RectangleLs.X;
                        top = tcm.RectangleLs.Y;
                        id = tcm.LogicIDTail;
                        intflag = tcm.Flag;
                        tagid = tcm.TagIDBase;

                        if (tagid != 0)
                        {
                            //同步标志位
                            tcm.TagIDBase = intflag;
                            Text tcmprop = new Text();
                            tcmprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            tcmprop.setTextDisplay(left, top - 25);
                            tcmprop.ObjectID = intflag;
                            tcmprop.TextType = "TCM_Logical_ID";
                            this.graphicsList.Add(tcm);
                            this.graphicsList.Add(tcmprop);
                        }
                        else
                        {
                            this.graphicsList.Add((TCM)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((TCM)o.Clone());
                    }
                    break;

                case "TVM":
                    listCount = graphicsList.TVMVerify.Count;
                    if (listCount > 0)
                    {
                        TVM tvm = (TVM)o.Clone(maxValue + 1);
                        left = tvm.RectangleLs.X;
                        top = tvm.RectangleLs.Y;
                        id = tvm.LogicIDTail;
                        intflag = tvm.Flag;
                        tagid = tvm.TagIDBase;

                        if (tagid != 0)
                        {
                            //同步标志位
                            tvm.TagIDBase = intflag;
                            Text tvmprop = new Text();
                            tvmprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            tvmprop.setTextDisplay(left, top - 25);
                            tvmprop.ObjectID = intflag;
                            tvmprop.TextType = "TVM_Logical_ID";
                            this.graphicsList.Add(tvm);
                            this.graphicsList.Add(tvmprop);
                        }
                        else
                        {
                            this.graphicsList.Add((TVM)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((TVM)o.Clone());
                    }
                    break;

                case "SC":
                    listCount = graphicsList.SCVerify.Count;

                    if (listCount > 0)
                    {
                        SC sc = (SC)o.Clone(maxValue + 1);
                        left = sc.RectangleLs.X;
                        top = sc.RectangleLs.Y;
                        id = sc.LogicIDTail;
                        intflag = sc.Flag;
                        tagid = sc.TagIDBase;

                        if (tagid != 0)
                        {
                            //同步标志位
                            sc.TagIDBase = intflag;
                            Text scprop = new Text();
                            scprop.TextFont = new Font("宋体", 9, FontStyle.Regular);
                            scprop.setTextDisplay(left, top - 25);
                            scprop.ObjectID = intflag;
                            scprop.TextType = "SC_Logical_ID";
                            this.graphicsList.Add(sc);
                            this.graphicsList.Add(scprop);
                        }
                        else
                        {
                            this.graphicsList.Add((SC)o.Clone());
                        }
                    }
                    else
                    {
                        this.graphicsList.Add((SC)o.Clone());
                    }
                    break;

                default:
                    this.graphicsList.Add(o.Clone());
                    break;
            }
        }

        private bool HasInclude(List<string> _verify, string sKey)
        {
            if (sKey != "")
            {
                if (_verify.Contains(sKey))
                {
                    return true;
                }
                else
                {
                    _verify.Add(sKey);
                    return false;
                }
            }
            return false;
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