/*******************************************************************
 * * 文件名： NumberTextBoxControl.cs
 * * 文件作用：数字文本框，仅限数字输入
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;

namespace Net.CommonLib.Controls
{
    public class NumberTextBoxControl : TextBox
    {
        public int Digits
        {
            get { return (int)GetValue(DigitsProperty); }
            set { SetValue(DigitsProperty, value); }
        }

        public static readonly DependencyProperty DigitsProperty = DependencyProperty.Register("Digits", typeof(int), typeof(NumberTextBoxControl), new PropertyMetadata(2));

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(NumberTextBoxControl), new PropertyMetadata(decimal.Zero));

        public NumberTextBoxControl()
                : base()
        {
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.TextChanged += new TextChangedEventHandler(NumericBox_TextChanged);
        }

        private string backupString = "";

        private void NumericBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string temp = tb.Text.Trim();
            if (!isDecimal(temp))
            {
                tb.Text = backupString;
                tb.Select(backupString.Length, 0);
                return;
            }
            decimal tempvalue = 0;
            Decimal.TryParse(temp, out tempvalue);

            backupString = temp;
            Value = tempvalue;
        }

        private bool isDecimal(string source)
        {
            foreach (char item in source)
            {
                if ((item < '0' || item > '9'))
                {
                    if (Digits == 0)
                        return false;
                    if (Digits != 0 && item != '.')
                        return false;
                }
            }
            return true;
        }
    }
}