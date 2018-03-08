using DrawTools.Log;

/**********************************************************
** 文件名： SetDrawForm.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DrawTools
{
    public partial class SetDrawForm : Form
    {
        private string stationID;
        private int width;
        private int height;
        private string deviceIP;

        public SetDrawForm(string stationID, int width, int height, string deviceIP)
        {
            InitializeComponent();
            textBox1.Text = stationID;
            // textBox2.Text = width.ToString();
            // textBox3.Text = height.ToString();
            ipInputTextbox1.IP = deviceIP.ToString();
            this.deviceIP = deviceIP;
            this.stationID = stationID;
            this.width = width;
            this.height = height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StationID = textBox1.Text.Trim();
            //AWidth = int.Parse(textBox2.Text.Trim());
            //AHeight = int.Parse(textBox3.Text.Trim());
            DeviceIP = ipInputTextbox1.IP.ToString().Trim();
            LogHelper.DeviceDeviceLogInfo(string.Format("更新车站服务器SC配置信息_车站SC编号:{0},IP地址:{1}", StationID, DeviceIP));
        }

        public string DeviceIP
        {
            get { return deviceIP; }
            set { deviceIP = value; }
        }

        public string StationID
        {
            get { return stationID; }
            set { stationID = value; }
        }

        public int AWidth
        {
            get { return width; }
            set { width = value; }
        }

        public int AHeight
        {
            get { return height; }
            set { height = value; }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (int.Parse(textBox2.Text.Trim()) > 2000 || int.Parse(textBox2.Text.Trim()) < 0)
                //{
                //    MessageBox.Show(this, "无效的值,Width值不超过2000", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    e.Cancel = true;
                //}
            }
            catch
            {
                MessageBox.Show(this, "输入Width值无效", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (int.Parse(textBox3.Text.Trim()) > 1000 || int.Parse(textBox3.Text.Trim()) < 0)
                //{
                //    MessageBox.Show(this, "无效的值,Width值不超过1000", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    e.Cancel = true;
                //}
            }
            catch
            {
                MessageBox.Show(this, "输入Width值无效", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtBoxIP_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (_ipVerifyList.Contains(this.txtBoxIP.Text.Trim()))
                //{
                //    MessageBox.Show("设备IP重复，同一车站设备不能有重复的设备IP,请重新设置！！");
                //}

                //Regex reg = new Regex(@"(?n)^(([1-9]?[0-9]|1[0-9]{2}|2([0-4][0-9]|5[0-5]))\.){3}([1-9]?[0-9]|1[0-9]{2}|2([0-4][0-9]|5[0-5]))$");
                //if (!reg.IsMatch(this.txtBoxIP.Text.Trim()))
                //{
                //    e.Cancel = true;
                //    MessageBox.Show("设备IP格式不正确，请重新设置！！");
                //}
            }
            catch
            {
            }
        }
    }
}