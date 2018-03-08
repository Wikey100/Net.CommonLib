/**********************************************************
** 文件名： SOAPInterface.cs
** 文件作用:动态加载云服务接口动态库
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.Command;
using DrawTools.Controls;
using DrawTools.DB;
using DrawTools.DocToolkit;
using DrawTools.Log;
using DrawTools.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace DrawTools
{
    public partial class StationMapForm : Form
    {
        #region Members

        public DrawForm currentlyDrawForm;
        private const string registryPath = "Software\\AlexF\\DrawTools";
        private PersistWindowState persistState;
        private int childnum = 1;
        private MenuCommandService _menuCommandService;

        #endregion Members

        public StationMapForm()
        {
            InitializeComponent();
            persistState = new PersistWindowState(registryPath, this);
            DesignSurface surface = new DesignSurface();
            IServiceContainer container = surface.GetService(typeof(IServiceContainer)) as IServiceContainer;
            _menuCommandService = new MenuCommandService(surface);
            if (container != null)
            {
                container.AddService(typeof(IMenuCommandService), _menuCommandService);
            }
            DrawForm();
            InitStationDeviceType();
        }

        private void StationMapForm_Load(object sender, EventArgs e)
        {
            Application.Idle += delegate (object o, EventArgs a)
            {
                SetStateOfControls();
            };
        }

        private System.Collections.ObjectModel.ObservableCollection<FilterItem> stationFilterList = new System.Collections.ObjectModel.ObservableCollection<FilterItem>();

        private List<FilterItem> lineFilterList = new List<FilterItem>();

        /// <summary>
        /// 初始化车站信息、设备类型列表
        /// </summary>
        private void InitStationDeviceType()
        {
            try
            {
                var lines = DBDeviceService.dbDevice.GetAllLines();

                foreach (var line in lines)
                {
                    lineFilterList.Add(new FilterItem(line.LineCode, line.LineName));
                }
                this.comBoxLine.ComboBox.DataSource = lineFilterList;
                comBoxLine.ComboBox.ValueMember = "Code";
                comBoxLine.ComboBox.DisplayMember = "Description";
                LogHelper.DeviceDeviceLogInfo("初始线路数据成功");

                //初始化车站信息
                var list = DBDeviceService.dbDevice.GetStationList(lines[0].LineCode);
                for (int i = 0; i < list.Count; i++)
                {
                    LogHelper.DeviceDeviceLogInfo("车站" + list[i].Code);
                    if (list[i].Code != "0600") //0600总线路不加入
                    {
                        stationFilterList.Add(new FilterItem(list[i].Code, list[i].Description));
                    }
                }
                this.comBoxStation.ComboBox.DataSource = stationFilterList;
                comBoxStation.ComboBox.ValueMember = "Code";
                comBoxStation.ComboBox.DisplayMember = "Description";
                LogHelper.DeviceDeviceLogInfo("初始化车站数据成功");
            }
            catch (Exception ex)
            {
                LogHelper.DeviceConfigLogError(string.Format("初始化车站数据失败"), ex);
            }
        }

        /// <summary>
        /// 初始化新建绘画面板
        /// </summary>
        private void DrawForm()
        {
            if (stationFilterList.Count > 0)
            {
                DrawForm drawForm = new DrawForm(this, childnum++, stationFilterList[0].Code);
                drawForm.TopLevel = false;
                drawForm.Dock = System.Windows.Forms.DockStyle.Fill;
                drawForm.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Add(drawForm);
                currentlyDrawForm = drawForm;
                drawForm.Show();
            }
        }

        public void SetStateOfControls()
        {
            // Select active tool
            try
            {
                toolStripButtonPointer.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.Pointer);
                AGMWallSingletoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.AGMWallSingle);
                AGMWallDualtoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.AGMWallDual);
                AGMWallDummytoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.AGMWallDummy);
                AGMChannelsingletoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.AGMChannel);
                AGMChannelDualtoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.AGMChannelDual);
                TVMtoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.TVM);
                BOMtoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.Bom);
                TCMtoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.TCM);
                PorttoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.Port);
                SwitchtoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.Switch);
                SCtoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.SC);
                PaidAreatoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.PaidArea);
                ArraytoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.Array);
                TexttoolStripButton.Checked = (currentlyDrawForm.getDrawArea().ActiveTool == DrawToolType.Text);

                //set device and Net flag
                bool deviceMaptype = currentlyDrawForm.getDrawArea().IsDeviceMap;
                BOMtoolStripButton.Enabled = deviceMaptype;
                TCMtoolStripButton.Enabled = deviceMaptype;
                AGMChannelDualtoolStripButton.Enabled = deviceMaptype;
                AGMChannelsingletoolStripButton.Enabled = deviceMaptype;
                TVMtoolStripButton.Enabled = deviceMaptype;
                AGMWallDualtoolStripButton.Enabled = deviceMaptype;
                AGMWallDummytoolStripButton.Enabled = deviceMaptype;
                AGMWallSingletoolStripButton.Enabled = deviceMaptype;
                SCtoolStripButton.Enabled = deviceMaptype;
                PaidAreatoolStripButton.Enabled = deviceMaptype;
                ArraytoolStripButton.Enabled = deviceMaptype;
                TexttoolStripButton.Enabled = deviceMaptype;

                bool netMapType = currentlyDrawForm.getDrawArea().IsNetMap;
                PorttoolStripButton.Enabled = netMapType;
                SwitchtoolStripButton.Enabled = netMapType;
                //define bool param
                bool objects = (currentlyDrawForm.getDrawArea().GraphicsList.Count > 0);
                bool selectedObjects = (currentlyDrawForm.getDrawArea().GraphicsList.SelectionCount > 0);
                bool selectedObject = (currentlyDrawForm.getDrawArea().GraphicsList.SelectionCount == 1);
                bool selectedMore = (currentlyDrawForm.getDrawArea().GraphicsList.SelectionCount > 1);
                bool hasCopyCount = (currentlyDrawForm.getDrawArea().GraphicsList.CopyList.Count > 0);
                copytoolStripMenuItem.Enabled = (deviceMaptype && selectedObjects);
                pastetoolStripMenuItem.Enabled = (deviceMaptype && hasCopyCount);

                // Edit operations
                deleteToolStripMenuItem.Enabled = selectedObjects;
                deleteAllToolStripMenuItem.Enabled = objects;
                selectAllToolStripMenuItem.Enabled = objects;
                unselectAllToolStripMenuItem.Enabled = objects;
                moveToFrontToolStripMenuItem.Enabled = selectedObjects;
                moveToBackToolStripMenuItem.Enabled = selectedObjects;
                propertiesToolStripMenuItem.Enabled = selectedObjects;

                textFontToolStripMenuItem.Enabled = selectedObject;
                textColorToolStripMenuItem.Enabled = selectedObject;

                topToolStripMenuItem.Enabled = selectedMore;
                leftToolStripMenuItem.Enabled = selectedMore;
                rightToolStripMenuItem.Enabled = selectedMore;
                bottomToolStripMenuItem.Enabled = selectedMore;
                widthToolStripMenuItem.Enabled = selectedMore;
                heightToolStripMenuItem.Enabled = selectedMore;
                bothToolStripMenuItem.Enabled = selectedMore;
                toolStripButtonUndo.Enabled = selectedObject;
                toolStripButtonRedo.Enabled = selectedObject;

                clockWiseToolStripMenuItem.Enabled = selectedObject;
                antiClockWiseToolStripMenuItem1.Enabled = selectedObject;
            }
            catch { }
        }

        private void BOMtoolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandBOM();
                SetDeviceMaptype();
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制BOM设备发生异常:" + ex);
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //选择所有
            try
            {
                currentlyDrawForm.getDrawArea().GraphicsList.SelectAll();
                currentlyDrawForm.getDrawArea().Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("选择设备发生异常:" + ex);
            }
        }

        private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //取消选择所有
            try
            {
                currentlyDrawForm.getDrawArea().GraphicsList.UnselectAll();
                currentlyDrawForm.getDrawArea().Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("取消选择所有设备发生异常:" + ex);
            }
        }

        private void copytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            //复制设备对象
            try
            {
                currentlyDrawForm.CommandCopyObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show("复制设备发生异常:" + ex);
            }
        }

        private void pastetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            //黏贴
            try
            {
                currentlyDrawForm.CommandPasteObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show("黏贴设备发生异常:" + ex);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //删除对象
            try
            {
                CommandDelete command = new CommandDelete(currentlyDrawForm.getDrawArea().GraphicsList);
                command.Execute();
                if (currentlyDrawForm.getDrawArea().GraphicsList.DeleteSelection())
                {
                    currentlyDrawForm.getDrawArea().SetDirty();
                    currentlyDrawForm.getDrawArea().Refresh();
                    currentlyDrawForm.getDrawArea().AddCommandToHistory(command);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除设备发生异常:" + ex);
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //删除所有
            try
            {
                CommandDeleteAll command = new CommandDeleteAll(currentlyDrawForm.getDrawArea().GraphicsList);
                command.Execute();
                currentlyDrawForm.getDrawArea().SetDirty();
                currentlyDrawForm.getDrawArea().Refresh();
                currentlyDrawForm.getDrawArea().AddCommandToHistory(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除所有设备发生异常:" + ex);
            }
        }

        private void moveToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //移动到前面层
            try
            {
                if (currentlyDrawForm.getDrawArea().GraphicsList.MoveSelectionToFront())
                {
                    currentlyDrawForm.getDrawArea().SetDirty();
                    currentlyDrawForm.getDrawArea().Refresh();
                }
            }
            catch { }
        }

        private void moveToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //移动到后一层
            try
            {
                if (currentlyDrawForm.getDrawArea().GraphicsList.MoveSelectionToBack())
                {
                    currentlyDrawForm.getDrawArea().SetDirty();
                    currentlyDrawForm.getDrawArea().Refresh();
                }
            }
            catch { }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //显示属性
            try
            {
                currentlyDrawForm.getDrawArea().GetDrawArea_DoubleClick();
                currentlyDrawForm.getDrawArea().SetDirty();
                currentlyDrawForm.getDrawArea().Refresh();
            }
            catch { }
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //align_top
            try
            {
                currentlyDrawForm.CommandAlign("Top");
            }
            catch { }
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //align_left
            try
            {
                currentlyDrawForm.CommandAlign("Left");
            }
            catch { }
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //align_right
            try
            {
                currentlyDrawForm.CommandAlign("Right");
            }
            catch { }
        }

        private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //align_botton
            try
            {
                currentlyDrawForm.CommandAlign("Bottom");
            }
            catch { }
        }

        private void textFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show textFonrTool
            try
            {
                currentlyDrawForm.CommandTextFont();
            }
            catch { }
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show textColor
            try
            {
                currentlyDrawForm.CommandTextColor();
            }
            catch { }
        }

        private void showPrepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show property
            if (showPrepToolStripMenuItem.Checked == true)
            {
                showPrepToolStripMenuItem.Checked = false;
            }
            else
            {
                showPrepToolStripMenuItem.Checked = true;
            }
            currentlyDrawForm.CommandShowProperty(showPrepToolStripMenuItem.Checked);
        }

        private void toolStripButtonPointer_Click(object sender, EventArgs e)
        {
            //change pointer
            try
            {
                currentlyDrawForm.CommandPointer();
            }
            catch { }
        }

        private void AGMWallSingletoolStripButton_Click(object sender, EventArgs e)
        {
            //draw agmwallsingle
            try
            {
                currentlyDrawForm.CommandAGMWallSingle();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void AGMWallDualtoolStripButton_Click(object sender, EventArgs e)
        {
            //draw agmwalldual
            try
            {
                currentlyDrawForm.CommandAGMWallDual();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void AGMWallDummytoolStripButton_Click(object sender, EventArgs e)
        {
            //draw agmwalldummy
            try
            {
                currentlyDrawForm.CommandAGMWallDummy();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void AGMChannelsingletoolStripButton_Click(object sender, EventArgs e)
        {
            //draw agmchannelsingle
            try
            {
                currentlyDrawForm.CommandAGMChannel();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void AGMChannelDualtoolStripButton_Click(object sender, EventArgs e)
        {
            //draw agmchanneldual
            try
            {
                currentlyDrawForm.CommandAGMChannelDual();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void TVMtoolStripButton_Click(object sender, EventArgs e)
        {
            //draw tvm
            try
            {
                currentlyDrawForm.CommandTVM();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void TCMtoolStripButton_Click(object sender, EventArgs e)
        {
            //draw tcm
            try
            {
                currentlyDrawForm.CommandTCM();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void SCtoolStripButton_Click(object sender, EventArgs e)
        {
            //draw sc
            try
            {
                int scDeviceVerty = currentlyDrawForm.CommandCheckSCVerty();
                if (scDeviceVerty == 0)
                {
                    currentlyDrawForm.CommandSC();
                    SetDeviceMaptype();
                }
                else
                {
                    MessageBox.Show("同一车站只允许添加一个SC！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch { }
        }

        private void PaidAreatoolStripButton_Click(object sender, EventArgs e)
        {
            //draw paidarea
            try
            {
                currentlyDrawForm.CommandPaidArea();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void ArraytoolStripButton_Click(object sender, EventArgs e)
        {
            //draw array
            try
            {
                currentlyDrawForm.CommandArray();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void TexttoolStripButton_Click(object sender, EventArgs e)
        {
            //draw text
            try
            {
                currentlyDrawForm.CommandText();
                SetDeviceMaptype();
            }
            catch { }
        }

        private void PorttoolStripButton_Click(object sender, EventArgs e)
        {
            //draw port
            try
            {
                currentlyDrawForm.CommandPort();
                SetNetMaptype();
            }
            catch { }
        }

        private void SwitchtoolStripButton_Click(object sender, EventArgs e)
        {
            //draw 交换机
            try
            {
                currentlyDrawForm.CommandSwitch();
                SetNetMaptype();
            }
            catch { }
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            //撤消上一步
            try
            {
                currentlyDrawForm.CommandUndo();
                //currentlyDrawForm.CommandAntiClockWise();
            }
            catch { }
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            //回到前一步
            try
            {
                currentlyDrawForm.CommandRedo();
                //currentlyDrawForm.CommandClockWise();
            }
            catch { }
        }

        //private void toolStripButtonNew_Click_1(object sender, EventArgs e)
        //{
        //    this.panel1.Controls.Clear();
        //    DrawForm drawForm = new DrawForm(this, childnum++, sender.ToString());
        //    drawForm.TopLevel = false;
        //    drawForm.Dock = System.Windows.Forms.DockStyle.Fill;
        //    drawForm.FormBorderStyle = FormBorderStyle.None;
        //    this.panel1.Controls.Add(drawForm);
        //    currentlyDrawForm = drawForm;
        //    drawForm.Show();
        //    SetStateOfControls();
        //}

        private void OpenNewDrawForm(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            DrawForm drawForm = new DrawForm(this, childnum++, sender.ToString());
            drawForm.TopLevel = false;
            drawForm.Dock = System.Windows.Forms.DockStyle.Fill;
            drawForm.FormBorderStyle = FormBorderStyle.None;
            this.panel1.Controls.Add(drawForm);
            currentlyDrawForm = drawForm;
            drawForm.Show();
            SetStateOfControls();
        }

        private void SetDeviceMaptype()
        {
            if (currentlyDrawForm.getDrawArea().IsNetMap) currentlyDrawForm.getDrawArea().IsNetMap = false;
            SetStateOfControls(); //TODO:添加此动态控制，会导致键盘删除设备不成功,但不加，却无法控制可用状态
        }

        private void SetNetMaptype()
        {
            if (currentlyDrawForm.getDrawArea().IsDeviceMap) currentlyDrawForm.getDrawArea().IsDeviceMap = false;
            SetStateOfControls();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                //TODO:取消交换机设备
                string mapType = "device";// this.comBoxMapType.ComboBox.SelectedItem.ToString().Trim();
                currentlyDrawForm.CommandSaveData(mapType);
            }
            catch { }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            float rate;
            switch (toolStripComboBox1.SelectedItem.ToString().Trim())
            {
                case "200%":
                    rate = 2.0f;
                    break;

                case "150%":
                    rate = 1.5f;
                    break;

                case "120%":
                    rate = 1.2f;
                    break;

                case "100%":
                    rate = 1.0f;
                    break;

                case "50%":
                    rate = 0.5f;
                    break;

                case "10%":
                    rate = 0.1f;
                    break;

                default:
                    rate = 1.0f;
                    break;
            }
            try
            {
                currentlyDrawForm.CommandSetRate(rate);
            }
            catch { }
        }

        private void ttextFont2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show textFonrTool
            try
            {
                currentlyDrawForm.CommandTextFont();
            }
            catch { }
        }

        private void textColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show textColor
            try
            {
                currentlyDrawForm.CommandTextColor();
            }
            catch { }
        }

        private void topToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //align_top
            try
            {
                currentlyDrawForm.CommandAlign("Top");
            }
            catch { }
        }

        private void leftToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //align_left
            try
            {
                currentlyDrawForm.CommandAlign("Left");
            }
            catch { }
        }

        private void rightToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //align_right
            try
            {
                currentlyDrawForm.CommandAlign("Right");
            }
            catch { }
        }

        private void bottomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //align_botton
            try
            {
                currentlyDrawForm.CommandAlign("Bottom");
            }
            catch { }
        }

        private void widthToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandsetSize("Width");
            }
            catch { }
        }

        private void heightToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandsetSize("Height");
            }
            catch { }
        }

        private void bothToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandsetSize("Both");
            }
            catch { }
        }

        private void clockWiseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandClockWise();
            }
            catch { }
        }

        private void antiClockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandAntiClockWise();
            }
            catch { }
        }

        private void widthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandsetSize("Width");
            }
            catch { }
        }

        private void heightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandsetSize("Height");
            }
            catch { }
        }

        private void clockWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandClockWise();
            }
            catch { }
        }

        private void antiClockWiseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                currentlyDrawForm.CommandAntiClockWise();
            }
            catch { }
        }

        private void comBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  GetStationMapInfo();
            if (comBoxLine.ComboBox.Items.Count > 0)
            {
                string lineId = comBoxLine.ComboBox.SelectedValue.ToString().Trim();
                LineChange(lineId);
            }
        }

        private void comBoxStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStationMapInfo();
            //if (comBoxStation.ComboBox.Items.Count > 0)
            //{
            //    string lineId = comBoxStation.ComboBox.SelectedValue.ToString().Trim();
            //    LineChange(lineId);
            //}
        }

        private void comBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 初始化车站设备地图信息
        /// </summary>
        private void GetStationMapInfo()
        {
            try
            {
                if (comBoxStation.ComboBox.Items.Count > 0) // && comBoxMapType.ComboBox.Items.Count > 0
                {
                    string stationId = comBoxStation.ComboBox.SelectedValue.ToString().Trim();
                    string mapType = "device";// comBoxMapType.ComboBox.SelectedValue.ToString().Trim();
                    var modelList = DBDeviceService.dbDevice.GetStationDeviceList(stationId, mapType);
                    OpenNewDrawForm(stationId, null);
                    if (modelList.Count > 0)
                    {
                        currentlyDrawForm.CommandOpen(modelList, mapType);
                    }
                    LogHelper.DeviceDeviceLogInfo(string.Format("初始化车站:{0}设备配置地图数据成功!", stationId));
                }
            }
            catch (Exception ex)
            {
                LogHelper.DeviceConfigLogError("初始化车站设备地图信息出错", ex);
                MessageBox.Show("初始化车站设备地图信息出错", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LineChange(string lineId)
        {
            var list = DBDeviceService.dbDevice.GetStationList(lineId);
            List<FilterItem> stations = new List<FilterItem>();
            // stationFilterList.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                // stationFilterList.Add(new FilterItem(list[i].Code, list[i].Description));
                stations.Add(new FilterItem(list[i].Code, list[i].Description));
            }
            //this.comBoxStation.ComboBox.DataSource = null;
            //this.comBoxStation.Items.Clear();
            this.comBoxStation.ComboBox.DataSource = stations;
            comBoxStation.ComboBox.ValueMember = "Code";
            comBoxStation.ComboBox.DisplayMember = "Description";
            LogHelper.DeviceDeviceLogInfo("切换车站数据成功");
        }
    }
}