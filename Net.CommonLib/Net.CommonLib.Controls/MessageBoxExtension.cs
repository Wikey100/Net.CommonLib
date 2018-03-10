/*******************************************************************
 * * 文件名： MessageBoxExtension.cs
 * * 文件作用：MessageBox提示框扩展
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Windows;

namespace Net.CommonLib.Controls
{
    public static class MessageBoxExtension
    {
        private static string TITLE = "XX系统";

        /// <summary>
        /// 显示简单对话框
        /// </summary>
        /// <param name="messageText">显示信息</param>
        /// <param name="caption">对话框标题</param>
        /// <param name="button">显示按钮</param>
        /// <param name="ico">显示图标</param>
        /// <returns></returns>
        public static MessageBoxResult ShowSimple(string messageText, string caption, MessageBoxButton button, MessageBoxImage ico)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(messageText, caption, button, ico);
        }

        /// <summary>
        /// 显示简单对话框
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageBoxResult ShowSimpleInfo(string message)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(message, TITLE, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 显示简单对话框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageBoxResult ShowSimpleInfo(Window owner, string message)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(owner, message, TITLE, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 显示简单错误对话框
        /// </summary>
        /// <param name="mesage"></param>
        /// <returns></returns>
        public static MessageBoxResult ShowSimpleError(string mesage)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(mesage, TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// 显示简单错误对话框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="mesage"></param>
        /// <returns></returns>
        public static MessageBoxResult ShowSimpleError(Window owner, string mesage)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(owner, mesage, TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// 显示详细错误对话框
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static MessageBoxResult ShowDetailError(Exception exception)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(exception.ToString(), TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}