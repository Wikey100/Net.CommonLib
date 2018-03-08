/*******************************************************************
 * * 文件名： ImagePageButtonControl.cs
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Net.CommonLib.Controls
{
    public class ImagePageButtonControl : Button
    {
        private Image innerImage;

        public ImagePageButtonControl()
        {
            this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(ImageButton_IsEnabledChanged);
        }

        private void ImageButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (innerImage == null)
            {
                return;
            }

            if (this.IsEnabled && ImageSource != null)
            {
                innerImage.Source = ImageSource;
            }
            else if (!this.IsEnabled && GrayImageSource != null)
            {
                innerImage.Source = GrayImageSource;
            }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImagePageButtonControl), new UIPropertyMetadata(null));

        public ImageSource GrayImageSource
        {
            get { return (ImageSource)GetValue(GrayImageSourceProperty); }
            set { SetValue(GrayImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GrayImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GrayImageSourceProperty =
            DependencyProperty.Register("GrayImageSource", typeof(ImageSource), typeof(ImagePageButtonControl), new UIPropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            innerImage = this.Template.FindName("PART_INNER_IMAGE", this) as Image;

            if (this.IsEnabled && ImageSource != null)
            {
                innerImage.Source = ImageSource;
            }
            else if (!this.IsEnabled && GrayImageSource != null)
            {
                innerImage.Source = GrayImageSource;
            }
        }
    }
}