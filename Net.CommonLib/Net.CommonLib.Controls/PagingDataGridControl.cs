/*******************************************************************
 * * 文件名： PagingDataGridControl.cs
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Net.CommonLib.Controls
{
    /// <summary>
    /// 分页DataGrid
    /// </summary>
    [TemplatePart(Name = PagingDataGridControl.ElementFirstPageImageButton, Type = typeof(ImagePageButtonControl))]
    [TemplatePart(Name = PagingDataGridControl.ElementPerviousPageImageButton, Type = typeof(ImagePageButtonControl))]
    [TemplatePart(Name = PagingDataGridControl.ElementNextPageImageButton, Type = typeof(ImagePageButtonControl))]
    [TemplatePart(Name = PagingDataGridControl.ElementLastPageImageButton, Type = typeof(ImagePageButtonControl))]
    [TemplatePart(Name = PagingDataGridControl.ElementRefreshImageButton, Type = typeof(ImagePageButtonControl))]
    [TemplatePart(Name = PagingDataGridControl.ElementPageSizeListComBox, Type = typeof(ComboBox))]
    [TemplatePart(Name = PagingDataGridControl.ElementPageIndexTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PagingDataGridControl.ElementPageCountTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PagingDataGridControl.ElementStartTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PagingDataGridControl.ElementEndTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PagingDataGridControl.ElementCountTextBlock, Type = typeof(TextBlock))]
    public class PagingDataGridControl : DataGrid
    {
        static PagingDataGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PagingDataGridControl), new FrameworkPropertyMetadata(typeof(PagingDataGridControl)));
        }

        public PagingDataGridControl()
        {
            CanUserAddRows = false;
        }

        #region 字段和属性

        private const string ElementFirstPageImageButton = "PART_FirstPage";
        private const string ElementPerviousPageImageButton = "PART_PerviousPage";
        private const string ElementNextPageImageButton = "PART_NextPage";
        private const string ElementLastPageImageButton = "PART_LastPage";
        private const string ElementRefreshImageButton = "PART_Refresh";

        private const string ElementPageSizeListComBox = "PART_PageSizeList";
        private const string ElementPageIndexTextBox = "PART_PageIndex";

        private const string ElementPageCountTextBlock = "PART_PageCount";
        private const string ElementStartTextBlock = "PART_Start";
        private const string ElementEndTextBlock = "PART_End";
        private const string ElementCountTextBlock = "PART_Count";

        private ImagePageButtonControl btnFirst, btnPrevious, btnNext, btnLast, btnRefresh;
        private ComboBox cboPageSize;
        private TextBox txtPageIndex;
        private TextBlock tbPageCount, tbStart, tbEnd, tbCount;

        public delegate void PagingChangedEventHandler(object sender, PagingChangedEventArgs args);

        private PagingChangedEventArgs pagingChangedEventArgs;

        #endregion 字段和属性

        #region 依赖属性

        public bool IsShowRowHeaderNumber
        {
            get { return (bool)GetValue(IsShowRowHeaderNumberProperty); }
            set { SetValue(IsShowRowHeaderNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowRowHeaderNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowRowHeaderNumberProperty =
            DependencyProperty.Register("IsShowRowHeaderNumber", typeof(bool), typeof(PagingDataGridControl), new UIPropertyMetadata(true));

        public bool IsShowPaging
        {
            get { return (bool)GetValue(IsShowPagingProperty); }
            set { SetValue(IsShowPagingProperty, value); }
        }

        /// <summary>
        /// 是否显示分页
        /// </summary>
        public static readonly DependencyProperty IsShowPagingProperty =
            DependencyProperty.Register("IsShowPaging", typeof(bool), typeof(PagingDataGridControl), new UIPropertyMetadata(true));

        public bool AllowPaging
        {
            get { return (bool)GetValue(AllowPagingProperty); }
            set { SetValue(AllowPagingProperty, value); }
        }

        /// <summary>
        /// 是否允许分页
        /// </summary>
        public static readonly DependencyProperty AllowPagingProperty =
            DependencyProperty.Register("AllowPaging", typeof(bool), typeof(PagingDataGridControl), new UIPropertyMetadata(true));

        public string PageSizeList
        {
            get { return (string)GetValue(PageSizeListProperty); }
            set { SetValue(PageSizeListProperty, value); }
        }

        /// <summary>
        /// 显示每页记录数字符串列表
        /// 例:10,20,30
        /// </summary>
        public static readonly DependencyProperty PageSizeListProperty =
            DependencyProperty.Register("PageSizeList", typeof(string), typeof(PagingDataGridControl), new UIPropertyMetadata(null, (s, e) =>
            {
                PagingDataGridControl dp = s as PagingDataGridControl;
                if (dp.PageSizeItemsSource == null)
                {
                    dp.PageSizeItemsSource = new List<int>();
                }
                if (dp.PageSizeItemsSource != null)
                {
                    List<string> strs = e.NewValue.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    dp.PageSizeItemsSource.Clear();
                    strs.ForEach(c =>
                    {
                        dp.PageSizeItemsSource.Add(Convert.ToInt32(c));
                    });
                }
            }));

        protected IList<int> PageSizeItemsSource
        {
            get { return (IList<int>)GetValue(PageSizeItemsSourceProperty); }
            set { SetValue(PageSizeItemsSourceProperty, value); }
        }

        /// <summary>
        /// 显示每页记录数集合
        /// </summary>
        protected static readonly DependencyProperty PageSizeItemsSourceProperty =
            DependencyProperty.Register("PageSizeItemsSource", typeof(IList<int>), typeof(PagingDataGridControl), new UIPropertyMetadata(new List<int> { 5, 10, 20, 30, 50 }));

        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(PagingDataGridControl), new UIPropertyMetadata(0));

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 每页记录数，默认：10
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PagingDataGridControl), new UIPropertyMetadata(10));

        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        /// <summary>
        /// 当前页码，默认：1
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(PagingDataGridControl), new UIPropertyMetadata(1));

        protected int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        protected static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(PagingDataGridControl), new UIPropertyMetadata(10));

        protected int Start
        {
            get { return (int)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        /// <summary>
        /// 起始记录数
        /// </summary>
        protected static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(int), typeof(PagingDataGridControl), new UIPropertyMetadata(0));

        protected int End
        {
            get { return (int)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        /// <summary>
        /// 终止记录数
        /// </summary>
        protected static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(int), typeof(PagingDataGridControl), new UIPropertyMetadata(0));

        public string SortField
        {
            get { return (string)GetValue(SortFieldProperty); }
            set { SetValue(SortFieldProperty, value); }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public static readonly DependencyProperty SortFieldProperty =
            DependencyProperty.Register("SortField", typeof(string), typeof(PagingDataGridControl), new UIPropertyMetadata(null));

        public static readonly RoutedEvent PagingChangedEvent = EventManager.RegisterRoutedEvent("PagingChangedEvent", RoutingStrategy.Bubble, typeof(PagingChangedEventHandler), typeof(PagingDataGridControl));

        /// <summary>
        /// 分页事件
        /// </summary>
        public event PagingChangedEventHandler PagingChanged
        {
            add
            {
                AddHandler(PagingChangedEvent, value);
            }
            remove
            {
                RemoveHandler(PagingChangedEvent, value);
            }
        }

        #endregion 依赖属性

        #region 重写方法

        /// <summary>
        /// 应用样式
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            btnFirst = GetTemplateChild(ElementFirstPageImageButton) as ImagePageButtonControl;
            btnPrevious = GetTemplateChild(ElementPerviousPageImageButton) as ImagePageButtonControl;
            btnNext = GetTemplateChild(ElementNextPageImageButton) as ImagePageButtonControl;
            btnLast = GetTemplateChild(ElementLastPageImageButton) as ImagePageButtonControl;
            btnRefresh = GetTemplateChild(ElementRefreshImageButton) as ImagePageButtonControl;

            cboPageSize = GetTemplateChild(ElementPageSizeListComBox) as ComboBox;
            txtPageIndex = GetTemplateChild(ElementPageIndexTextBox) as TextBox;

            tbPageCount = GetTemplateChild(ElementPageCountTextBlock) as TextBlock;
            tbStart = GetTemplateChild(ElementStartTextBlock) as TextBlock;
            tbEnd = GetTemplateChild(ElementEndTextBlock) as TextBlock;
            tbCount = GetTemplateChild(ElementCountTextBlock) as TextBlock;

            btnFirst.Click += new RoutedEventHandler(btnFirst_Click);
            btnLast.Click += new RoutedEventHandler(btnLast_Click);
            btnNext.Click += new RoutedEventHandler(btnNext_Click);
            btnPrevious.Click += new RoutedEventHandler(btnPrevious_Click);
            btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);

            cboPageSize.SelectionChanged += new SelectionChangedEventHandler(cboPageSize_SelectionChanged);
            txtPageIndex.PreviewKeyDown += new KeyEventHandler(txtPageIndex_PreviewKeyDown);
            txtPageIndex.LostFocus += new RoutedEventHandler(txtPageIndex_LostFocus);

            this.Loaded += new RoutedEventHandler(PagingDataGrid_Loaded);
        }

        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);

            if (IsShowRowHeaderNumber)
            {
                e.Row.Header = e.Row.GetIndex() + 1;
            }
        }

        #endregion 重写方法

        #region 分页事件

        private void RaisePageChanged()
        {
            if (pagingChangedEventArgs == null)
            {
                pagingChangedEventArgs = new PagingChangedEventArgs(PagingChangedEvent, PageSize, PageIndex);
            }

            if (AllowPaging)
            {
                pagingChangedEventArgs.PageSize = this.PageSize;
                pagingChangedEventArgs.PageIndex = this.PageIndex;
            }
            else
            {
                pagingChangedEventArgs.PageSize = this.PageSize = int.MaxValue;
                pagingChangedEventArgs.PageIndex = 1;
            }

            RaiseEvent(pagingChangedEventArgs);

            //calc start、end
            if (ItemsSource != null)
            {
                int curCount = 0;
                IEnumerator enumrator = ItemsSource.GetEnumerator();
                while (enumrator.MoveNext())
                {
                    curCount++;
                }

                //不允许分页处理
                if (!AllowPaging)
                {
                    PageSizeItemsSource.Clear();
                    PageSizeItemsSource.Add(curCount);
                    PageSize = curCount;
                }

                Start = (PageIndex - 1) * PageSize + 1;
                End = Start + curCount - 1;

                if (Total % PageSize != 0)
                {
                    PageCount = Total / PageSize + 1;
                }
                else
                {
                    PageCount = Total / PageSize;
                }
            }
            else
            {
                Start = End = PageCount = Total = 0;
            }

            //调整图片的显示
            btnFirst.IsEnabled = btnPrevious.IsEnabled = (PageIndex != 1);
            btnNext.IsEnabled = btnLast.IsEnabled = (PageIndex != PageCount);
        }

        private void PagingDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            RaisePageChanged();
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            PageIndex = 1;
            RaisePageChanged();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex > 1)
            {
                --PageIndex;
            }
            RaisePageChanged();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Total % PageSize != 0)
            {
                PageCount = Total / PageSize + 1;
            }
            else
            {
                PageCount = Total / PageSize;
            }

            if (PageIndex < PageCount)
            {
                ++PageIndex;
            }
            RaisePageChanged();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (Total % PageSize != 0)
            {
                PageCount = Total / PageSize + 1;
            }
            else
            {
                PageCount = Total / PageSize;
            }
            PageIndex = PageCount;
            RaisePageChanged();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RaisePageChanged();
        }

        private void txtPageIndex_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPageIndex_LostFocus(sender, null);
            }
        }

        private void txtPageIndex_LostFocus(object sender, RoutedEventArgs e)
        {
            int pIndex = 0;
            try
            {
                pIndex = Convert.ToInt32(txtPageIndex.Text);
            }
            catch { pIndex = 1; }

            if (pIndex < 1) PageIndex = 1;
            else if (pIndex > PageCount) PageIndex = PageCount;
            else PageIndex = pIndex;

            RaisePageChanged();
        }

        private void cboPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                PageSize = (int)cboPageSize.SelectedItem;
                RaisePageChanged();
            }
        }

        #endregion 分页事件
    }

    public class PagingChangedEventArgs : RoutedEventArgs
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Sort { get; set; }

        public PagingChangedEventArgs(RoutedEvent eventToRaise, int pageSize, int pageIndex, string sort = null)
            : base(eventToRaise)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Sort = sort;
        }
    }
}