/**********************************************************
** 文件名： PersistWindowState.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

#region Using directives

using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools.DocToolkit
{
    public class PersistWindowState
    {
        #region Members

        private Form ownerForm;
        private string registryPath;
        private int normalLeft;
        private int normalTop;
        private int normalWidth;
        private int normalHeight;

        private FormWindowState windowState = FormWindowState.Normal;

        private bool allowSaveMinimized = false;

        #endregion Members

        #region Constructor

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="sRegPath"></param>
        /// <param name="owner"></param>
        public PersistWindowState(string path, Form owner)
        {
            if (path == null ||
                path.Length == 0)
            {
                registryPath = "Software\\Unknown";
            }
            else
            {
                registryPath = path;
            }

            if (!registryPath.EndsWith("\\"))
                registryPath += "\\";

            registryPath += "MainForm";

            ownerForm = owner;

            // subscribe to parent form's events

            ownerForm.Closing += OnClosing;
            ownerForm.Resize += OnResize;
            ownerForm.Move += OnMove;
            ownerForm.Load += OnLoad;

            // get initial width and height in case form is never resized
            normalWidth = ownerForm.Width;
            normalHeight = ownerForm.Height;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// AllowSaveMinimized property (default value false)
        /// </summary>
        public bool AllowSaveMinimized
        {
            get
            {
                return allowSaveMinimized;
            }
            set
            {
                allowSaveMinimized = value;
            }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Parent form is resized.
        /// Keep current size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize(object sender, System.EventArgs e)
        {
            // save width and height
            if (ownerForm.WindowState == FormWindowState.Normal)
            {
                normalWidth = ownerForm.Width;
                normalHeight = ownerForm.Height;
            }
        }

        /// <summary>
        /// Parent form is moved.
        /// Keep current window position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMove(object sender, System.EventArgs e)
        {
            // save position
            if (ownerForm.WindowState == FormWindowState.Normal)
            {
                normalLeft = ownerForm.Left;
                normalTop = ownerForm.Top;
            }

            // save state
            windowState = ownerForm.WindowState;
        }

        /// <summary>
        /// Parent form is closing.
        /// Keep last state in Registry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // save position, size and state
            RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
            key.SetValue("Left", normalLeft);
            key.SetValue("Top", normalTop);
            key.SetValue("Width", normalWidth);
            key.SetValue("Height", normalHeight);

            // check if we are allowed to save the state as minimized (not normally)
            if (!allowSaveMinimized)
            {
                if (windowState == FormWindowState.Minimized)
                    windowState = FormWindowState.Normal;
            }

            key.SetValue("WindowState", (int)windowState);
        }

        /// <summary>
        /// Parent form is loaded.
        /// Read last state from Registry and set it to form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoad(object sender, System.EventArgs e)
        {
            // attempt to read state from registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);
            if (key != null)
            {
                int left = (int)key.GetValue("Left", ownerForm.Left);
                int top = (int)key.GetValue("Top", ownerForm.Top);
                int width = (int)key.GetValue("Width", ownerForm.Width);
                int height = (int)key.GetValue("Height", ownerForm.Height);
                FormWindowState windowState = (FormWindowState)key.GetValue("WindowState", (int)ownerForm.WindowState);

                ownerForm.Location = new Point(left, top);
                ownerForm.Size = new Size(width, height);
                ownerForm.WindowState = windowState;
            }
        }

        #endregion Event Handlers
    }
}