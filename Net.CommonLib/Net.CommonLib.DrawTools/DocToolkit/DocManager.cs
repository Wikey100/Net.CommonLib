/**********************************************************
** 文件名： DocManager.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

#region Using directives

using DrawTools.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools.DocToolkit
{
    #region Class DocManager

    public class DocManager
    {
        #region Events

        public event SaveEventHandler SaveEvent;

        public event LoadEventHandler LoadEvent;

        public event OpenFileEventHandler OpenEvent;

        public event EventHandler ClearEvent;

        public event EventHandler DocChangedEvent;

        #endregion Events

        #region Members

        private string fileName = "";
        private bool dirty = false;
        private Form frmOwner;
        private string newDocName;
        private string fileDlgFilter;
        private string registryPath;
        private bool updateTitle;
        private const string registryValue = "Path";
        private string fileDlgInitDir = "";         // file dialog initial directory
        private int childFormNumber;
        private string filetitlename;

        #endregion Members

        #region Enum

        /// <summary>
        /// Enumeration used for Save function
        /// </summary>
        public enum SaveType
        {
            Save,
            SaveAs
        }

        #endregion Enum

        #region Constructor

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="data"></param>
        public DocManager(DocManagerData data)
        {
            frmOwner = data.FormOwner;
            frmOwner.Closing += OnClosing;

            updateTitle = data.UpdateTitle;

            newDocName = data.NewDocName;

            fileDlgFilter = data.FileDialogFilter;

            registryPath = data.RegistryPath;

            childFormNumber = data.num;

            filetitlename = newDocName + childFormNumber;

            if (!registryPath.EndsWith("\\"))
                registryPath += "\\";

            registryPath += "FileDir";

            // attempt to read initial directory from registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);

            if (key != null)
            {
                string s = (string)key.GetValue(registryValue);

                if (!Empty(s))
                    fileDlgInitDir = s;
            }
        }

        #endregion Constructor

        #region Public functions and Properties

        /// <summary>
        /// Dirty property (true when document has unsaved changes).
        /// </summary>
        public bool Dirty
        {
            get
            {
                return dirty;
            }
            set
            {
                dirty = value;
                SetCaption();
            }
        }

        /// <summary>
        /// Open new document
        /// </summary>
        /// <returns></returns>
        public bool NewDocument()
        {
            SetFileName("");
            Dirty = false;
            return true;
        }

        /// <summary>
        /// Close document
        /// </summary>
        /// <returns></returns>
        public bool CloseDocument()
        {
            if (!this.dirty)
                return true;

            DialogResult res = MessageBox.Show(
                frmOwner,
                "Save changes " + filetitlename + " ?",
                Application.ProductName,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation);

            switch (res)
            {
                case DialogResult.Yes: return SaveDocument(SaveType.Save);
                case DialogResult.No: return true;
                case DialogResult.Cancel: return false;
                default: Debug.Assert(false); return false;
            }
        }

        /// <summary>
        /// Open document
        /// </summary>
        /// <param name="newFileName">
        /// Document file name. Empty - function shows Open File dialog.
        /// </param>
        /// <returns></returns>
        //public bool OpenDocument(string newFileName)
        //{
        //    fileDlgInitDir = new FileInfo(newFileName).DirectoryName;
        //    // Read the data
        //    try
        //    {
        //        Control drawdontrol = frmOwner.Controls[0];
        //        if (!((DrawControl)drawdontrol).OpenXML(newFileName)) return false;
        //    }

        //    catch (ArgumentNullException ex) { return HandleOpenException(ex, newFileName); }
        //    catch (ArgumentOutOfRangeException ex) { return HandleOpenException(ex, newFileName); }
        //    catch (ArgumentException ex) { return HandleOpenException(ex, newFileName); }
        //    catch (SecurityException ex) { return HandleOpenException(ex, newFileName); }
        //    catch (FileNotFoundException ex) { return HandleOpenException(ex, newFileName); }
        //    catch (DirectoryNotFoundException ex) { return HandleOpenException(ex, newFileName); }
        //    catch (PathTooLongException ex) { return HandleOpenException(ex, newFileName); }
        //    catch (IOException ex) { return HandleOpenException(ex, newFileName); }

        //    // Clear dirty bit, cache the file name and set the caption
        //    Dirty = false;
        //    SetFileName(newFileName);
        //    filetitlename = newFileName;

        //    if (OpenEvent != null)
        //    {
        //        // report success
        //        OpenEvent(this, new OpenFileEventArgs(newFileName, true));
        //    }
        //    // Success
        //    return true;
        //}

        public bool OpenDocument(List<StationMapModel> modelList, string mapType)
        {
            Control drawdontrol = frmOwner.Controls[0];
            if (!((DrawControl)drawdontrol).OpenData(modelList, mapType)) return false;
            return true;
        }

        /// <summary>
        /// Save file.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SaveDocument(SaveType type)
        {
            // Get the file name
            string newFileName = this.fileName;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = fileDlgFilter;

            if ((type == SaveType.SaveAs) ||
                Empty(newFileName))
            {
                if (!Empty(newFileName))
                {
                    saveFileDialog1.InitialDirectory = Path.GetDirectoryName(newFileName);
                    saveFileDialog1.FileName = Path.GetFileName(newFileName);
                }
                else
                {
                    saveFileDialog1.InitialDirectory = fileDlgInitDir;
                    saveFileDialog1.FileName = filetitlename + ".xml";
                }

                DialogResult res = saveFileDialog1.ShowDialog(frmOwner);

                if (res != DialogResult.OK)
                    return false;

                newFileName = saveFileDialog1.FileName;
                fileDlgInitDir = new FileInfo(newFileName).DirectoryName;
            }

            // Write the data
            try
            {
                Control drawdontrol = frmOwner.Controls[0];
                ((DrawControl)drawdontrol).SaveXML(newFileName);
            }
            catch (Exception ex) { return HandleSaveException(ex, newFileName); }
            Dirty = false;
            SetFileName(newFileName);
            filetitlename = newFileName;

            if (OpenEvent != null)
            {
                OpenEvent(this, new OpenFileEventArgs(newFileName, true));
            }
            return true;
        }

        /// <summary>
        /// 以数据库方式保存
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SaveDocumentData(string mapType)
        {
            // Write the data
            try
            {
                Control drawdontrol = frmOwner.Controls[0];
                ((DrawControl)drawdontrol).SaveData(mapType);
            }
            catch (Exception ex) { }
            return true;
        }

        public int CheckSCDevieVerty()
        {
            try
            {
                Control drawdontrol = frmOwner.Controls[0];
                return ((DrawControl)drawdontrol).CheckSCDeviceVertyList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Assosciate file type with this program in the Registry
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true - OK, false - failed</returns>
        public bool RegisterFileType(
            string fileExtension,
            string progId,
            string typeDisplayName)
        {
            try
            {
                string s = String.Format(CultureInfo.InvariantCulture, ".{0}", fileExtension);

                // Register custom extension with the shell
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(s))
                {
                    // Map custom  extension to a ProgID
                    key.SetValue(null, progId);
                }

                // create ProgID key with display name
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(progId))
                {
                    key.SetValue(null, typeDisplayName);
                }

                // register icon
                using (RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(progId + @"\DefaultIcon"))
                {
                    key.SetValue(null, Application.ExecutablePath + ",0");
                }

                // Register open command with the shell
                string cmdkey = progId + @"\shell\open\command";
                using (RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(cmdkey))
                {
                    // Map ProgID to an Open action for the shell
                    key.SetValue(null, Application.ExecutablePath + " \"%1\"");
                }

                // Register application for "Open With" dialog
                string appkey = "Applications\\" +
                    new FileInfo(Application.ExecutablePath).Name +
                    "\\shell";
                using (RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(appkey))
                {
                    key.SetValue("FriendlyCache", Application.ProductName);
                }
            }
            catch (ArgumentNullException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (SecurityException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return HandleRegistryException(ex);
            }

            return true;
        }

        #endregion Public functions and Properties

        #region Other Functions

        /// <summary>
        /// Hanfle exception from RegisterFileType function
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool HandleRegistryException(Exception ex)
        {
            Trace.WriteLine("Registry operation failed: " + ex.Message);
            return false;
        }

        /// <summary>
        /// Save initial directory to the Registry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
            key.SetValue(registryValue, fileDlgInitDir);
        }

        /// <summary>
        /// Set file name and change owner's caption
        /// </summary>
        /// <param name="fileName"></param>
        private void SetFileName(string fileName)
        {
            this.fileName = fileName;
            SetCaption();
        }

        /// <summary>
        /// Set owner form caption
        /// </summary>
        private void SetCaption()
        {
            if (!updateTitle)
                return;

            frmOwner.Text = string.Format(
                CultureInfo.InvariantCulture,
                "{0} - {1}{2}",
                Application.ProductName,
                Empty(this.fileName) ? filetitlename : this.fileName,//Path.GetFileName(this.fileName),
                this.dirty ? "*" : "");
        }

        /// <summary>
        /// Handle exception in OpenDocument function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool HandleOpenException(Exception ex, string fileName)
        {
            MessageBox.Show(frmOwner,
                "Open File operation failed. File name: " + fileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);

            if (OpenEvent != null)
            {
                // report failure
                OpenEvent(this, new OpenFileEventArgs(fileName, false));
            }

            return false;
        }

        /// <summary>
        /// Handle exception in SaveDocument function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool HandleSaveException(Exception ex, string fileName)
        {
            MessageBox.Show(frmOwner,
                "Save File operation failed. File name: " + fileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);

            return false;
        }

        /// <summary>
        /// Helper function - test if string is empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool Empty(string s)
        {
            return s == null || s.Length == 0;
        }

        #endregion Other Functions
    }

    #endregion Class DocManager

    #region Delegates

    public delegate void SaveEventHandler(object sender, SerializationEventArgs e);

    public delegate void LoadEventHandler(object sender, SerializationEventArgs e);

    public delegate void OpenFileEventHandler(object sender, OpenFileEventArgs e);

    #endregion Delegates

    #region Class SerializationEventArgs

    /// <summary>
    /// Serialization event arguments.
    /// Used in events raised from DocManager class.
    /// Class contains information required to load/save file.
    /// </summary>
    public class SerializationEventArgs : System.EventArgs
    {
        private IFormatter formatter;
        private Stream stream;
        private string fileName;
        private bool errorFlag;

        public SerializationEventArgs(IFormatter formatter, Stream stream,
            string fileName)
        {
            this.formatter = formatter;
            this.stream = stream;
            this.fileName = fileName;
            errorFlag = false;
        }

        public bool Error
        {
            get
            {
                return errorFlag;
            }
            set
            {
                errorFlag = value;
            }
        }

        public IFormatter Formatter
        {
            get
            {
                return formatter;
            }
        }

        public Stream SerializationStream
        {
            get
            {
                return stream;
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }

    #endregion Class SerializationEventArgs

    #region Class OpenFileEventArgs

    /// <summary>
    /// Open file event arguments.
    /// Used in events raised from DocManager class.
    /// Class contains name of file and result of Open operation.
    /// </summary>
    public class OpenFileEventArgs : System.EventArgs
    {
        private string fileName;
        private bool success;

        public OpenFileEventArgs(string fileName, bool success)
        {
            this.fileName = fileName;
            this.success = success;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public bool Succeeded
        {
            get
            {
                return success;
            }
        }
    }

    #endregion Class OpenFileEventArgs

    #region class DocManagerData

    /// <summary>
    /// Class used for DocManager class initialization
    /// </summary>
    public class DocManagerData
    {
        public DocManagerData()
        {
            frmOwner = null;
            updateTitle = true;
            newDocName = "Untitled";
            fileDlgFilter = "All Files (*.*)|*.*";
            registryPath = "Software\\Unknown";
            num = 1;
        }

        private Form frmOwner;
        private bool updateTitle;
        private string newDocName;
        private string fileDlgFilter;
        private string registryPath;
        public int num;

        public Form FormOwner
        {
            get
            {
                return frmOwner;
            }
            set
            {
                frmOwner = value;
            }
        }

        public bool UpdateTitle
        {
            get
            {
                return updateTitle;
            }
            set
            {
                updateTitle = value;
            }
        }

        public string NewDocName
        {
            get
            {
                return newDocName;
            }
            set
            {
                newDocName = value;
            }
        }

        public string FileDialogFilter
        {
            get
            {
                return fileDlgFilter;
            }
            set
            {
                fileDlgFilter = value;
            }
        }

        public string RegistryPath
        {
            get
            {
                return registryPath;
            }
            set
            {
                registryPath = value;
            }
        }
    };

    #endregion class DocManagerData
}