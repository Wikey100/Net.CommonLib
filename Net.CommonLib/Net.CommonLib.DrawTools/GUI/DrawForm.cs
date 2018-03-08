/**********************************************************
** 文件名： DrawForm.cs
** 文件作用:绘画面板窗体
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.Controls;
using DrawTools.DocToolkit;
using DrawTools.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security;
using System.Windows.Forms;

namespace DrawTools
{
    public partial class DrawForm : Form
    {
        #region Members

        public StationMapForm mainform;
        private DrawArea drawArea;
        public DocManager docManager;
        private DragDropManager dragDropManager;
        private MruManager mruManager;
        private int num;
        private string argumentFile = "";   // file name from command line
        private const string registryPath = "Software\\AlexF\\DrawTools";

        public DrawArea getDrawArea()
        {
            return drawArea;
        }

        #endregion Members

        #region Properties

        /// <summary>
        /// File name from the command line
        /// </summary>
        public string ArgumentFile
        {
            get
            {
                return argumentFile;
            }
            set
            {
                argumentFile = value;
            }
        }

        /// <summary>
        /// Get reference to Edit menu item.
        /// Used to show context menu in DrawArea class.
        /// </summary>
        /// <value></value>
        public ToolStripMenuItem ContextParent
        {
            get
            {
                return mainform.editToolStripMenuItem;
            }
        }

        #endregion Properties

        #region Constructor

        public DrawForm(StationMapForm mainform, int childnum, string stationId)
        {
            InitializeComponent();
            this.mainform = mainform;
            num = childnum;

            // Create draw area
            drawArea = new DrawArea(stationId);
            drawArea.Location = new System.Drawing.Point(0, 0);
            drawArea.Size = new System.Drawing.Size(10, 10);
            drawArea.Owner = this;
            drawArea.Dock = DockStyle.Fill;
            this.Controls.Add(drawArea);

            // Helper objects (DocManager and others)
            InitializeHelperObjects();

            drawArea.Initialize(this, docManager);
            //ResizeDrawArea();

            LoadSettingsFromRegistry();

            // Open file passed in the command line
            if (ArgumentFile.Length > 0)
                OpenDocument(ArgumentFile);

            // Subscribe to DropDownOpened event for each popup menu
            // (see details in MainForm_DropDownOpened)
            foreach (ToolStripItem item in mainform.menuStrip1.Items)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ((ToolStripMenuItem)item).DropDownOpened += DrawForm_DropDownOpened;
                }
            }
        }

        #endregion Constructor

        #region DocManager Event Handlers

        /// <summary>
        /// Load document from the stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void docManager_LoadEvent(object sender, SerializationEventArgs e)
        {
            // DocManager asks to load document from supplied stream
            try
            {
                drawArea.GraphicsList = (GraphicsList)e.Formatter.Deserialize(e.SerializationStream);
            }
            catch (ArgumentNullException ex)
            {
                HandleLoadException(ex, e);
            }
            catch (SerializationException ex)
            {
                HandleLoadException(ex, e);
            }
            catch (SecurityException ex)
            {
                HandleLoadException(ex, e);
            }
        }

        /// <summary>
        /// Save document to stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void docManager_SaveEvent(object sender, SerializationEventArgs e)
        {
            // DocManager asks to save document to supplied stream
            try
            {
                e.Formatter.Serialize(e.SerializationStream, drawArea.GraphicsList);
            }
            catch (ArgumentNullException ex)
            {
                HandleSaveException(ex, e);
            }
            catch (SerializationException ex)
            {
                HandleSaveException(ex, e);
            }
            catch (SecurityException ex)
            {
                HandleSaveException(ex, e);
            }
        }

        #endregion DocManager Event Handlers

        #region Event Handlers

        private void DrawForm_Load(object sender, EventArgs e)
        {
            //// Create draw area
            //drawArea = new DrawArea();
            //drawArea.Location = new System.Drawing.Point(0, 0);
            //drawArea.Size = new System.Drawing.Size(10, 10);
            //drawArea.Owner = this;
            //drawArea.Dock = DockStyle.Fill;
            //this.Controls.Add(drawArea);

            //// Helper objects (DocManager and others)
            //InitializeHelperObjects();

            //drawArea.Initialize(this, docManager);
            ////ResizeDrawArea();

            //LoadSettingsFromRegistry();

            //// Open file passed in the command line
            //if (ArgumentFile.Length > 0)
            //    OpenDocument(ArgumentFile);

            //// Subscribe to DropDownOpened event for each popup menu
            //// (see details in MainForm_DropDownOpened)
            //foreach (ToolStripItem item in mainform.menuStrip1.Items)
            //{
            //    if (item.GetType() == typeof(ToolStripMenuItem))
            //    {
            //        ((ToolStripMenuItem)item).DropDownOpened += DrawForm_DropDownOpened;
            //    }
            //}
        }

        /// <summary>
        /// Form is closing  MDI窗体已取消
        /// </summary>
        private void DrawForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!docManager.CloseDocument())
                {
                    e.Cancel = true;
                    return;
                }
            }
            //解决MDI关闭子窗体时发生“无法访问已释放对象”的.net框架问题
            this.Hide();
            this.Parent = null;
            SaveSettingsToRegistry();
        }

        /// <summary>
        /// Popup menu item (File, Edit ...) is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawForm_DropDownOpened(object sender, EventArgs e)
        {
            // Reset active tool to pointer.
            // This prevents bug in rare case when non-pointer tool is active, user opens
            // main main menu and after this clicks in the drawArea. MouseDown event is not
            // raised in this case (why ??), and MouseMove event works incorrectly.
            drawArea.ActiveTool = DrawToolType.Pointer;
        }

        #endregion Event Handlers

        #region Other Functions

        /// <summary>
        /// Set draw area to all form client space except toolbar
        /// </summary>
        private void ResizeDrawArea()
        {
            Rectangle rect = this.ClientRectangle;
            drawArea.Left = rect.Left;
            drawArea.Top = rect.Top + mainform.menuStrip1.Height + mainform.toolStrip1.Height;
            drawArea.Width = rect.Width;
            drawArea.Height = rect.Height - mainform.menuStrip1.Height - mainform.toolStrip1.Height;
        }

        /// <summary>
        /// Initialize helper objects from the DocToolkit Library.
        ///
        /// Called from Form1_Load. Initialized all objects except
        /// PersistWindowState wich must be initialized in the
        /// form constructor.
        /// </summary>
        private void InitializeHelperObjects()
        {
            // DocManager
            DocManagerData data = new DocManagerData();
            data.FormOwner = this;
            data.UpdateTitle = true;
            data.FileDialogFilter = "DrawTools files (*.xml)|*.xml|All Files (*.*)|*.*";
            data.NewDocName = "Untitled";
            data.RegistryPath = registryPath;
            data.num = num;
            docManager = new DocManager(data);
            docManager.RegisterFileType("xml", "xmlfile", "DrawTools File");
            // Subscribe to DocManager events.
            docManager.SaveEvent += docManager_SaveEvent;
            docManager.LoadEvent += docManager_LoadEvent;

            // Make "inline subscription" using anonymous methods.
            docManager.OpenEvent += delegate (object sender, OpenFileEventArgs e)
            {
                // Update MRU List
                if (e.Succeeded)
                    mruManager.Add(e.FileName);
                else
                    mruManager.Remove(e.FileName);
            };

            docManager.DocChangedEvent += delegate (object o, EventArgs e)
            {
                drawArea.Refresh();
                drawArea.ClearHistory();
            };

            docManager.ClearEvent += delegate (object o, EventArgs e)
            {
                if (drawArea.GraphicsList != null)
                {
                    drawArea.GraphicsList.Clear();
                    drawArea.ClearHistory();
                    drawArea.Refresh();
                }
            };
            docManager.NewDocument();
            // DragDropManager
            //dragDropManager = new DragDropManager(this);
            //dragDropManager.FileDroppedEvent += delegate(object sender, FileDroppedEventArgs e)
            //{
            //    //OpenDocument(e.FileArray.GetValue(0).ToString()); //TODO:取消打开文件操作
            //};

            // MruManager
            mruManager = new MruManager();

            mruManager.Initialize(
                this,       // owner form
                null,      // Recent Files menu item
                mainform.fileToolStripMenuItem,            // parent
                registryPath);                     // Registry path to keep MRU list

            //mruManager.MruOpenEvent += delegate(object sender, MruFileOpenEventArgs e)
            //{
            //    //   OpenDocument(e.FileName);
            //    //this.mainform.GetRecentFiles(e.FileName);  //取消打开最近文件
            //};
        }

        /// <summary>
        /// Handle exception from docManager_LoadEvent function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        private void HandleLoadException(Exception ex, SerializationEventArgs e)
        {
            MessageBox.Show(this,
                "Open File operation failed. File name: " + e.FileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);

            e.Error = true;
        }

        /// <summary>
        /// Handle exception from docManager_SaveEvent function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        private void HandleSaveException(Exception ex, SerializationEventArgs e)
        {
            MessageBox.Show(this,
                "Save File operation failed. File name: " + e.FileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);
            e.Error = true;
        }

        /// <summary>
        /// Open document.
        /// Used to open file passed in command line or dropped into the window
        /// </summary>
        /// <param name="file"></param>
        public void OpenDocument(string file)
        {
            //docManager.OpenDocument(file);
        }

        /// <summary>
        /// Load application settings from the Registry
        /// </summary>
        private void LoadSettingsFromRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
                //   DrawObject.LastUsedColor = Color.FromArgb((int)key.GetValue("Color",Color.Black.ToArgb()));
                DrawObject.LastUsedColor = Color.Black;
                //   DrawObject.LastUsedPenWidth = (int)key.GetValue("Width",1);
                DrawObject.LastUsedPenWidth = 1;
                //this.drawArea.AWidth = (int)key.GetValue("DrawAreaWidth", 920);
                //this.drawArea.AHeight = (int)key.GetValue("DrawAreaHeight", 670);
                this.drawArea.AWidth = (int)key.GetValue("DrawAreaWidth", 1300);
                this.drawArea.AHeight = (int)key.GetValue("DrawAreaHeight", 900);
            }
            catch (ArgumentNullException ex)
            {
                HandleRegistryException(ex);
            }
            catch (SecurityException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleRegistryException(ex);
            }
        }

        /// <summary>
        /// Save application settings to the Registry
        /// </summary>
        private void SaveSettingsToRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
                key.SetValue("Color", DrawObject.LastUsedColor.ToArgb());
                key.SetValue("Width", DrawObject.LastUsedPenWidth);
                key.SetValue("DrawAreaWidth", this.drawArea.AWidth);
                key.SetValue("DrawAreaHeight", this.drawArea.AHeight);
            }
            catch (SecurityException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleRegistryException(ex);
            }
        }

        private void HandleRegistryException(Exception ex)
        {
            Trace.WriteLine("Registry operation failed: " + ex.Message);
        }

        /// <summary>
        /// Set Pointer draw tool
        /// </summary>
        public void CommandPointer()
        {
            drawArea.ActiveTool = DrawToolType.Pointer;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        public void CommandRectangle()
        {
            drawArea.ActiveTool = DrawToolType.Rectangle;
        }

        /// <summary>
        /// Set Ellipse draw tool
        /// </summary>
        public void CommandEllipse()
        {
            drawArea.ActiveTool = DrawToolType.Ellipse;
        }

        /// <summary>
        /// Set bom draw tool
        /// </summary>
        public void CommandBOM()
        {
            drawArea.ActiveTool = DrawToolType.Bom;
        }

        public void CommandAGMChannel()
        {
            drawArea.ActiveTool = DrawToolType.AGMChannel;
        }

        public void CommandAGMChannelDual()
        {
            drawArea.ActiveTool = DrawToolType.AGMChannelDual;
        }

        public void CommandAGMWallDual()
        {
            drawArea.ActiveTool = DrawToolType.AGMWallDual;
        }

        public void CommandAGMWallDummy()
        {
            drawArea.ActiveTool = DrawToolType.AGMWallDummy;
        }

        public void CommandAGMWallSingle()
        {
            drawArea.ActiveTool = DrawToolType.AGMWallSingle;
        }

        public void CommandArray()
        {
            drawArea.ActiveTool = DrawToolType.Array;
        }

        public void CommandPaidArea()
        {
            drawArea.ActiveTool = DrawToolType.PaidArea;
        }

        public void CommandSC()
        {
            drawArea.ActiveTool = DrawToolType.SC;
        }

        public void CommandTCM()
        {
            drawArea.ActiveTool = DrawToolType.TCM;
        }

        public void CommandTVM()
        {
            drawArea.ActiveTool = DrawToolType.TVM;
        }

        public void CommandSwitch()
        {
            drawArea.ActiveTool = DrawToolType.Switch;
        }

        public void CommandPort()
        {
            drawArea.ActiveTool = DrawToolType.Port;
        }

        public void CommandText()
        {
            drawArea.ActiveTool = DrawToolType.Text;
        }

        public void CommandStationObject()
        {
            drawArea.setStationObject();
        }

        public void CommandTextFont()
        {
            drawArea.setTextsize();
        }

        public void CommandTextColor()
        {
            drawArea.setTextcolor();
        }

        public void CommandShowProperty(bool check)
        {
            drawArea.showPreproty(check);
        }

        public void Commandgridlines()
        {
            drawArea.gridlines();
        }

        public void CommandAlign(string Direction)
        {
            drawArea.setAlign(Direction);
        }

        public void CommandsetSize(string Size)
        {
            drawArea.setSize(Size);
        }

        public void CommandCopyObject()
        {
            drawArea.CopySelectionObject();
        }

        public void CommandPasteObject()
        {
            drawArea.PasteSelectionObject();
        }

        public void CommandSetRate(float rate)
        {
            drawArea.SetRate(rate);
        }

        /// <summary>
        /// Set Line draw tool
        /// </summary>
        public void CommandLine()
        {
            drawArea.ActiveTool = DrawToolType.Line;
        }

        /// <summary>
        /// Set Polygon draw tool
        /// </summary>
        public void CommandPolygon()
        {
            drawArea.ActiveTool = DrawToolType.Polygon;
        }

        /// <summary>
        /// Show About dialog
        /// </summary>
        public void CommandAbout()
        {
            //FrmAbout frm = new FrmAbout();
            //frm.ShowDialog(this);
        }

        /// <summary>
        /// Open new file
        /// </summary>
        public void CommandNew()
        {
            docManager.NewDocument();
        }

        /// <summary>
        /// Open file
        /// </summary>
        public void CommandOpen(string FileName)
        {
            // docManager.OpenDocument(FileName);
        }

        public void CommandOpen(List<StationMapModel> modelList, string mapType)
        {
            docManager.OpenDocument(modelList, mapType);
        }

        public int CommandCheckSCVerty()
        {
            return docManager.CheckSCDevieVerty();
        }

        /// <summary>
        /// Save file
        /// </summary>
        public void CommandSave()
        {
            docManager.SaveDocument(DocManager.SaveType.Save);
        }

        /// <summary>
        /// save data stationMap
        /// </summary>
        public void CommandSaveData(string mapType)
        {
            docManager.SaveDocumentData(mapType);
        }

        /// <summary>
        /// Save As
        /// </summary>
        public void CommandSaveAs()
        {
            docManager.SaveDocument(DocManager.SaveType.SaveAs);
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void CommandUndo()
        {
            drawArea.Undo();
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void CommandRedo()
        {
            drawArea.Redo();
        }

        public void CommandAntiClockWise()
        {
            drawArea.AntiClockWise();
        }

        public void CommandClockWise()
        {
            drawArea.ClockWise();
        }

        #endregion Other Functions
    }
}