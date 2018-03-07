/*******************************************************************
 * * 文件名： AutoSizeControl.cs
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Net.CommonLib.UI
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

            #endregion

            #region 字段/属性

            private Button lifeSizeBtn;
            private Button autoSizeBtn;

            #endregion

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

            #endregion
        }
}
