/*******************************************************************
 * * 文件名： CustomWindow.cs
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Net.CommonLib.Controls
{
    public class CustomWindow : Window
    {
        private Button closeBtn;
        private Grid topArea;

        #region dps

        public Visibility CloseButtonVisible
        {
            get { return (Visibility)GetValue(CloseButtonVisibleProperty); }
            set { SetValue(CloseButtonVisibleProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonVisibleProperty =
            DependencyProperty.Register("CloseButtonVisible", typeof(Visibility), typeof(CustomWindow),
            new UIPropertyMetadata(Visibility.Visible));

        #endregion dps

        public CustomWindow()
        {
            this.Closed += new EventHandler(ChildWindow_Closed);
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {
            if (closeBtn != null)
            {
                closeBtn.Click -= new RoutedEventHandler(closeBtn_Click);
                closeBtn = null;
            }

            if (topArea != null)
            {
                topArea.MouseLeftButtonDown -= new MouseButtonEventHandler(topArea_MouseLeftButtonDown);
                topArea = null;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            closeBtn = this.Template.FindName("CloseBtn", this) as Button;
            topArea = this.Template.FindName("TopArea", this) as Grid;

            if (closeBtn != null)
            {
                closeBtn.Visibility = this.CloseButtonVisible;
                closeBtn.Click += new RoutedEventHandler(closeBtn_Click);
            }

            if (topArea != null)
            {
                topArea.MouseLeftButtonDown += new MouseButtonEventHandler(topArea_MouseLeftButtonDown);
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void topArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}