/*******************************************************************
 * * 文件名： ExpanderGridSplitterControl.cs
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
    public class ExpanderGridSplitterControl : GridSplitter
    {
        public ExpanderGridSplitterControl()
        {
            this.Unloaded += new RoutedEventHandler(ExpanderGridSplitter_Unloaded);
        }

        #region 依懒属性

        public DefinitionBase GridDefinition
        {
            get { return (DefinitionBase)GetValue(GridDefinitionProperty); }
            set { SetValue(GridDefinitionProperty, value); }
        }

        public static readonly DependencyProperty GridDefinitionProperty =
            DependencyProperty.Register("GridDefinition", typeof(DefinitionBase), typeof(ExpanderGridSplitterControl),
            new UIPropertyMetadata(null));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ExpanderGridSplitterControl),
            new UIPropertyMetadata(true, new PropertyChangedCallback(IsExpandedChanged)));

        public ExpandDirectEnum ExpandToDirection
        {
            get { return (ExpandDirectEnum)GetValue(ExpandToDirectionProperty); }
            set { SetValue(ExpandToDirectionProperty, value); }
        }

        public static readonly DependencyProperty ExpandToDirectionProperty =
            DependencyProperty.Register("ExpandToDirection", typeof(ExpandDirectEnum), typeof(ExpanderGridSplitterControl),
            new UIPropertyMetadata(ExpandDirectEnum.ExpandToRight));

        #endregion 依懒属性

        #region 字段/属性

        private Image expandedBtn;
        private GridLength lastGridLength;
        private double minGridLengthValue = 0;
        private double maxGridLengthValue = 0;

        #endregion 字段/属性

        #region 方法

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.expandedBtn = this.Template.FindName("Img", this) as Image;
            if (this.expandedBtn != null)
            {
                lastGridLength = GetGridLength();
                minGridLengthValue = GetMinGridLengthValue();
                maxGridLengthValue = GetMaxGridLengthValue();
                this.expandedBtn.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(expandedBtn_PreviewMouseLeftButtonDown);
                this.DragCompleted += new System.Windows.Controls.Primitives.DragCompletedEventHandler(ExpanderGridSplitter_DragCompleted);
            }
        }

        private static void IsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExpanderGridSplitterControl gs = d as ExpanderGridSplitterControl;
            gs.Expender((bool)e.NewValue);
        }

        private void Expender(bool newValue)
        {
            if (this.GridDefinition == null)
            {
                throw new ArgumentNullException("GridDefinition");
            }
            if (this.GridDefinition is RowDefinition)
            {
                RowDefinition rowDefinition = this.GridDefinition as RowDefinition;

                if (newValue)
                {
                    rowDefinition.Height = lastGridLength;
                    rowDefinition.MinHeight = minGridLengthValue;
                    rowDefinition.MaxHeight = maxGridLengthValue;
                }
                else
                {
                    rowDefinition.Height = new GridLength(0);
                    rowDefinition.MinHeight = 0;
                }
            }
            else if (this.GridDefinition is ColumnDefinition)
            {
                ColumnDefinition columnDefinition = this.GridDefinition as ColumnDefinition;

                if (newValue)
                {
                    columnDefinition.Width = lastGridLength;
                    columnDefinition.MinWidth = minGridLengthValue;
                    columnDefinition.MaxWidth = maxGridLengthValue;
                }
                else
                {
                    columnDefinition.Width = new GridLength(0);
                    columnDefinition.MinWidth = 0;
                }
            }
        }

        private void expandedBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsExpanded = !this.IsExpanded;
            e.Handled = true;
        }

        private void ExpanderGridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            lastGridLength = GetGridLength();
        }

        private double GetMinGridLengthValue()
        {
            double temp = 0;
            if (this.GridDefinition is RowDefinition)
            {
                RowDefinition rowDefinition = this.GridDefinition as RowDefinition;
                temp = rowDefinition.MinHeight;
            }
            else if (this.GridDefinition is ColumnDefinition)
            {
                ColumnDefinition columnDefinition = this.GridDefinition as ColumnDefinition;
                temp = columnDefinition.MinWidth;
            }
            return temp;
        }

        private double GetMaxGridLengthValue()
        {
            double temp = 0;
            if (this.GridDefinition is RowDefinition)
            {
                RowDefinition rowDefinition = this.GridDefinition as RowDefinition;
                temp = rowDefinition.MaxHeight;
            }
            else if (this.GridDefinition is ColumnDefinition)
            {
                ColumnDefinition columnDefinition = this.GridDefinition as ColumnDefinition;
                temp = columnDefinition.MaxWidth;
            }
            return temp;
        }

        private GridLength GetGridLength()
        {
            GridLength temp = new GridLength();
            if (this.GridDefinition is RowDefinition)
            {
                RowDefinition rowDefinition = this.GridDefinition as RowDefinition;
                temp = rowDefinition.Height;
            }
            else if (this.GridDefinition is ColumnDefinition)
            {
                ColumnDefinition columnDefinition = this.GridDefinition as ColumnDefinition;
                temp = columnDefinition.Width;
            }
            return temp;
        }

        private void ExpanderGridSplitter_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.expandedBtn != null)
            {
                this.expandedBtn = null;
            }
        }

        #endregion 方法
    }

    public enum ExpandDirectEnum
    {
        /// <summary>
        /// 往左边展开
        /// </summary>
        ExpandToLeft,

        /// <summary>
        /// 往右边展开
        /// </summary>
        ExpandToRight,

        /// <summary>
        /// 往上展开
        /// </summary>
        ExpandToUp,

        /// <summary>
        /// 往下展开
        /// </summary>
        ExpandToDown
    }
}