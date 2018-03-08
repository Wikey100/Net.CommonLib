/**********************************************************
** 文件名： EditorDialog.cs
** 文件作用:设备属性编辑窗体
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.Device;
using DrawTools.DocToolkit;
using DrawTools.Log;
using DrawTools.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DrawTools
{
    public partial class EditorDialog : Form
    {
        private string ID;
        private string name;
        private string entry;
        private int x_axis;
        private List<string> _verifyList;
        private List<string> _ipVerifyList;
        private int y_axis;
        private int width;
        private int height;
        public string ArrayId { get; set; }
        //public string ArrayId
        //{
        //    get
        //    {
        //        return "";// this.tbArrayBox.Text.Trim();
        //    }

        //    set
        //    {
        //        this.tbArrayBox.Text = value;
        //    }
        //}
        public int X_axis
        {
            get { return x_axis; }
            set { value = x_axis; }
        }

        public int Y_axis
        {
            get { return y_axis; }
            set { value = y_axis; }
        }

        public int RWidth
        {
            get { return width; }
            set { value = width; }
        }

        public int RHeight
        {
            get { return height; }
            set { value = height; }
        }

        public string RecID { get; set; }
        public string StationID { get; set; }

        public string DeviceID { get; set; }

        public string DeviceName { get; set; }

        public string DeviceType { get; set; }

        public string DeviceSubType { get; set; }

        public string DeviceSeqInStation { get; set; }

        public string LobbyId { get; set; }

        public string GroupID { get; set; }

        public string DeviceSeqInGroup { get; set; }

        public string XPos { get; set; }

        public string YPos { get; set; }

        public string Angle { get; set; }

        public string IpAdd { get; set; }
        public string Device_W { get; set; }
        public string Device_H { get; set; }

        public string Region_W { get; set; }
        public string Region_H { get; set; }

        public EditorDialog(String ID, DrawObject obj, string entry, List<string> verifyList, DeviceTypeEnum DeviceTypeStr, DeviceTypeEnum subDeviceType, List<string> ipVerifyList)
        {
            InitializeComponent();
            _verifyList = verifyList;
            _ipVerifyList = ipVerifyList;
            this.ID = ID;
            this.name = obj.GetType().Name;
            //bool canArrayed = false;

            if (ID.Trim() != "")
            {
                textBox1.Text = ID.Substring(ID.Length - 2, 2);
                textBox1.Enabled = false;
            }
            else
            {
                textBox1.Text = "00";
                textBox1.Enabled = true;
            }
            this.x_axis = obj.GetRectangle().X;
            this.y_axis = obj.GetRectangle().Y;
            this.width = obj.GetRectangle().Width;
            this.height = obj.GetRectangle().Height;

            this.txt_Xaris.Text = this.x_axis.ToString();
            this.txt_Yaxis.Text = this.y_axis.ToString();

            DrawRectangle drawObj = null;
            if (name == "BOM")
            {
                drawObj = obj as BOM;
                this.IpAdd = drawObj.IpAdd;
                this.DeviceName = drawObj.DeviceName;
            }
            else if (name == "TVM")
            {
                drawObj = obj as TVM;
                this.IpAdd = drawObj.IpAdd;
                this.DeviceName = drawObj.DeviceName;
            }
            else if (name == "TCM")
            {
                drawObj = obj as TCM;
                this.IpAdd = drawObj.IpAdd;
                this.DeviceName = drawObj.DeviceName;
            }
            else if (name == "AGMChannelDual")
            {
                drawObj = obj as AGMChannelDual;
                //comboBox1.Visible = true;
                //label4.Visible = true;
                this.entry = entry;
                // comboBox1.Text = entry;
                this.IpAdd = drawObj.IpAdd;
                this.DeviceName = drawObj.DeviceName;
            }
            else if (name == "AGMChannel")
            {
                drawObj = obj as AGMChannel;
                this.IpAdd = drawObj.IpAdd;
                this.DeviceName = drawObj.DeviceName;
            }
            else if (name == "SC")
            {
                drawObj = obj as SC;
                this.IpAdd = drawObj.IpAdd;
                this.DeviceName = drawObj.DeviceName;
            }
            this.txtDeviceName.Text = drawObj.DeviceName;
            this.ipInputTextbox1.IP = drawObj.IpAdd;
            this.txtHeight.Text = drawObj.GetRectangle().Height.ToString();
            this.txtWidth.Text = drawObj.GetRectangle().Width.ToString();
            this.DeviceType = DeviceTypeStr.ToString();
            // if (textBox1.Text.Trim() == ID) button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ID = textBox1.Text.Trim();
            // entry = comboBox1.Text.Trim();
            this.x_axis = int.Parse(txt_Xaris.Text.Trim());
            this.y_axis = int.Parse(txt_Yaxis.Text.Trim());
            this.width = int.Parse(txtWidth.Text.Trim());
            this.height = int.Parse(txtHeight.Text.Trim());

            //添加保存的属性
            DeviceID = this.textBox1.Text.Trim();
            DeviceName = this.txtDeviceName.Text.Trim();
            IpAdd = this.ipInputTextbox1.IP.Trim();
            this.Device_W = this.txtWidth.Text.Trim();
            this.Device_H = this.txtHeight.Text.Trim();
            LogHelper.DeviceDeviceLogInfo(string.Format("更新设备配置信息_设备编号:{0},设备名称:{1},IP地址:{2}", DeviceID, DeviceName, IpAdd));
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            bool deviceflag = true;
            if (ActiveControl != (Control)button1) return;
            try
            {
                switch (name)
                {
                    case "BOM":
                        break;

                    case "TCM":
                        break;

                    case "TVM":
                        break;

                    case "AGMChannel":
                        break;

                    case "AGMChannelDual":
                        break;

                    default:
                        deviceflag = false;
                        break;
                }

                if (deviceflag && _verifyList.Contains(textBox1.Text.Trim()))
                {
                    e.Cancel = true;
                    MessageBox.Show("设备ID重复，同一车站同一设备类型不能有重复的设备ID,请重新设置！！");
                }
                else
                {
                    int i = Int32.Parse(textBox1.Text.Trim(), System.Globalization.NumberStyles.HexNumber);
                    if ((name == "BOM") || (name == "TCM") || (name == "TVM") || (name == "Array") || (name == "AGMChannel") || (name == "AGMChannelDual"))
                    {
                        if (textBox1.Text.Trim().Length != 2 || (textBox1.Text.Trim().Substring(0, 1).Equals("-")))
                        {
                            if (ID.Trim() != "")
                            {
                                textBox1.Text = ID;
                            }
                            else
                            {
                                textBox1.Text = "00";
                            }

                            e.Cancel = true;
                            MessageBox.Show(this, "ID error! 只能输入两位十六进制位数", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch
            {
                e.Cancel = true;
                if (ID.Trim() != "")
                {
                    textBox1.Text = ID;
                }
                else
                {
                    textBox1.Text = "00";
                }
                MessageBox.Show(this, "ID error!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string IDvalues
        {
            get { return ID; }
            set { ID = value; }
        }

        public string Entryvalues
        {
            get { return entry; }
            set { entry = value; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.txtDeviceName.Text = DeviceType + this.textBox1.Text;
            //if (ID != textBox1.Text.Trim())
            //{
            //    button1.Enabled = true;
            //    _verifyList.Remove(ID);
            //}
            //else
            //{
            //    button1.Enabled = false;
            //}
        }

        private void txtDeviceName_TextChanged(object sender, EventArgs e)
        {
            //if (DeviceName != txtDeviceName.Text.Trim())
            //{
            //    button1.Enabled = true;
            //}
            //else
            //{
            //    button1.Enabled = false;
            //}
        }

        private void txt_Xaris_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_Yaxis_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}