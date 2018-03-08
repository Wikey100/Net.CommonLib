/**********************************************************
** 文件名： SwitchEditDialog.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System;
using System.Windows.Forms;

namespace DrawTools
{
    public partial class SwitchEditDialog : Form
    {
        private string monitorID;
        private string portID;
        private string deviceID;

        public SwitchEditDialog(string switchID, DrawObject obj)
        {
            InitializeComponent();
            this.monitorID = switchID;

            label1.Text = "交换机编号";
            textBox1.Text = this.monitorID;
            label2.Visible = textBox2.Visible =
                label3.Visible = textBox3.Visible = false;
        }

        public SwitchEditDialog(string monitorID, int portID, string deviceID, DrawObject obj)
        {
            InitializeComponent();
            this.monitorID = monitorID;
            this.portID = portID.ToString();
            this.deviceID = deviceID;

            label1.Text = "监控端口";
            textBox1.Text = this.monitorID;
            label2.Visible = textBox2.Visible =
                label3.Visible = textBox3.Visible = true;
            textBox2.Text = this.deviceID;
            textBox3.Text = this.portID;
        }

        public string MonitorID
        {
            get { return this.monitorID; }
            set { this.monitorID = value; }
        }

        public string DeviceID
        {
            get { return this.deviceID; }
            set { this.deviceID = value; }
        }

        public int PortID
        {
            get { return int.Parse(this.portID); }
            set { this.portID = value.ToString(); }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.monitorID = textBox1.Text.Trim();
            this.deviceID = textBox2.Text.Trim();
            this.portID = textBox3.Text.Trim();
        }
    }
}