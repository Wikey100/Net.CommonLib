using System;
using System.Windows.Forms;

namespace IpInputExt.Ctrls
{
    public class NumberTextBoxExt : TextBox
    {
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        private int index;

        //定义委托
        public delegate void PressBackspaceHandle(int index);

        public event PressBackspaceHandle OnPressBackspace;

        public NumberTextBoxExt()
            : base()
        {
            this.ShortcutsEnabled = false;
            this.MaxLength = 3;
            this.BorderStyle = BorderStyle.None;
            this.TextAlign = HorizontalAlignment.Center;
            this.Size = new System.Drawing.Size(30, 14);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            int ipNum = GetIpNum();
            if (ipNum >= 0)
            {
                if (ipNum > 255)
                {
                    this.Text = "255";
                    this.SelectionStart = 3;
                }
            }
            base.OnTextChanged(e);
        }

        private int GetIpNum()
        {
            int ip;
            if (this.Text.Length > 0)
            {
                if (int.TryParse(this.Text, out ip))
                {
                    return ip;
                }
            }
            return -1;
        }

        /// <summary>
        /// 键盘输入控制
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public override bool PreProcessMessage(ref Message m)
        {
            //Console.WriteLine(m.WParam.ToInt32().ToString());
            if (m.WParam.ToInt32() >= (int)'0' && m.WParam.ToInt32() <= (int)'9')
            {//数字键0-9
                return base.PreProcessMessage(ref m);
            }
            else if (m.WParam.ToInt32() >= 96 && m.WParam.ToInt32() <= 105)
            {//数字键盘0-9
                return base.PreProcessMessage(ref m);
            }
            else if (m.WParam.ToInt32() >= 37 && m.WParam.ToInt32() <= 40)
            {//方向控制键：上下左右
                return base.PreProcessMessage(ref m);
            }
            else if (m.WParam.ToInt32() == 8)
            {//删除键
                if (this.Text.Length == 0)
                {
                    if (OnPressBackspace != null)
                    {
                        OnPressBackspace(this.index);
                    }
                }
                return base.PreProcessMessage(ref m);
            }
            else if (m.WParam.ToInt32() == 9)
            {
                return base.ProcessKeyMessage(ref m);
            }
            else if (m.WParam.ToInt32() == 110 || m.WParam.ToInt32() == 190)
            {//如果输入“.”则切换到下一个
                m.WParam = new IntPtr(9);
                return base.ProcessKeyMessage(ref m);
            }
            else if (m.WParam.ToInt32() == 46)
            {//Del键
                return base.PreProcessMessage(ref m);
            }
            else
            {
                return true;
            }
        }
    }
}