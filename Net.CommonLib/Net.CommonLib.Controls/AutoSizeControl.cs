/*******************************************************************
 * * 文件名： AutoSizeControl.cs
 * * 文件作用：自动放大缩小区域控件
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System.Windows;
using System.Windows.Controls;

namespace Net.CommonLib.Controls
{
    public enum SizeModeEnum
    {
        /// <summary>
        /// 1:1大小
        /// </summary>
        LifeSize,

        /// <summary>
        /// 自适应大小
        /// </summary>
        AutoSize
    }

    public class AutoSizeControl : ContentControl
    {
        #region dps

        public SizeModeEnum SizeMode
        {
            get { return (SizeModeEnum)GetValue(SizeModeProperty); }
            set { SetValue(SizeModeProperty, value); }
        }

        public static readonly DependencyProperty SizeModeProperty =
            DependencyProperty.Register("SizeMode", typeof(SizeModeEnum), typeof(AutoSizeControl),
            new UIPropertyMetadata(SizeModeEnum.AutoSize));

        #endregion dps

        #region 字段/属性

        private Button lifeSizeBtn;
        private Button autoSizeBtn;

        #endregion 字段/属性

        #region 方法

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            lifeSizeBtn = this.Template.FindName("PART_LIFESIZE_BTN", this) as Button;
            autoSizeBtn = this.Template.FindName("PART_AUTOSIZE_BTN", this) as Button;

            if (lifeSizeBtn != null)
            {
                lifeSizeBtn.Click += delegate { LifeSize(); };
            }

            if (autoSizeBtn != null)
            {
                autoSizeBtn.Click += delegate { AutoSize(); };
            }
        }

        private void LifeSize()
        {
            SizeMode = SizeModeEnum.LifeSize;
        }

        private void AutoSize()
        {
            SizeMode = SizeModeEnum.AutoSize;
        }

        #endregion 方法
    }
}