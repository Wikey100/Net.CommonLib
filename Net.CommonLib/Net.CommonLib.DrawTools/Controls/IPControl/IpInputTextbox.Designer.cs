namespace IpInputExt.Ctrls
{
    partial class IpInputTextbox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lbldot1 = new System.Windows.Forms.Label();
            this.lbldot2 = new System.Windows.Forms.Label();
            this.lbldot3 = new System.Windows.Forms.Label();
            this.Ip4 = new IpInputExt.Ctrls.NumberTextBoxExt();
            this.Ip3 = new IpInputExt.Ctrls.NumberTextBoxExt();
            this.Ip2 = new IpInputExt.Ctrls.NumberTextBoxExt();
            this.Ip1 = new IpInputExt.Ctrls.NumberTextBoxExt();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlMain.Controls.Add(this.Ip4);
            this.pnlMain.Controls.Add(this.Ip3);
            this.pnlMain.Controls.Add(this.Ip2);
            this.pnlMain.Controls.Add(this.Ip1);
            this.pnlMain.Controls.Add(this.lbldot3);
            this.pnlMain.Controls.Add(this.lbldot2);
            this.pnlMain.Controls.Add(this.lbldot1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(160, 21);
            this.pnlMain.TabIndex = 0;
            // 
            // lbldot1
            // 
            this.lbldot1.AutoSize = true;
            this.lbldot1.Location = new System.Drawing.Point(32, 4);
            this.lbldot1.Name = "lbldot1";
            this.lbldot1.Size = new System.Drawing.Size(11, 12);
            this.lbldot1.TabIndex = 1;
            this.lbldot1.Text = ".";
            // 
            // lbldot2
            // 
            this.lbldot2.AutoSize = true;
            this.lbldot2.Location = new System.Drawing.Point(74, 4);
            this.lbldot2.Name = "lbldot2";
            this.lbldot2.Size = new System.Drawing.Size(11, 12);
            this.lbldot2.TabIndex = 5;
            this.lbldot2.Text = ".";
            // 
            // lbldot3
            // 
            this.lbldot3.AutoSize = true;
            this.lbldot3.Location = new System.Drawing.Point(114, 4);
            this.lbldot3.Name = "lbldot3";
            this.lbldot3.Size = new System.Drawing.Size(11, 12);
            this.lbldot3.TabIndex = 6;
            this.lbldot3.Text = ".";
            // 
            // Ip4
            // 
            this.Ip4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Ip4.Location = new System.Drawing.Point(123, 1);
            this.Ip4.MaxLength = 3;
            this.Ip4.Name = "Ip4";
            this.Ip4.ShortcutsEnabled = false;
            this.Ip4.Size = new System.Drawing.Size(30, 14);
            this.Ip4.TabIndex = 4;
            this.Ip4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Ip3
            // 
            this.Ip3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Ip3.Location = new System.Drawing.Point(82, 1);
            this.Ip3.MaxLength = 3;
            this.Ip3.Name = "Ip3";
            this.Ip3.ShortcutsEnabled = false;
            this.Ip3.Size = new System.Drawing.Size(30, 14);
            this.Ip3.TabIndex = 3;
            this.Ip3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ip3.TextChanged += new System.EventHandler(this.Ip3_TextChanged);
            this.Ip3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Ip3_KeyPress);
            // 
            // Ip2
            // 
            this.Ip2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Ip2.Location = new System.Drawing.Point(41, 1);
            this.Ip2.MaxLength = 3;
            this.Ip2.Name = "Ip2";
            this.Ip2.ShortcutsEnabled = false;
            this.Ip2.Size = new System.Drawing.Size(30, 14);
            this.Ip2.TabIndex = 2;
            this.Ip2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ip2.TextChanged += new System.EventHandler(this.Ip2_TextChanged);
            this.Ip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Ip2_KeyPress);
            // 
            // Ip1
            // 
            this.Ip1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Ip1.Location = new System.Drawing.Point(0, 1);
            this.Ip1.MaxLength = 3;
            this.Ip1.Name = "Ip1";
            this.Ip1.ShortcutsEnabled = false;
            this.Ip1.Size = new System.Drawing.Size(30, 14);
            this.Ip1.TabIndex = 0;
            this.Ip1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ip1.TextChanged += new System.EventHandler(this.Ip1_TextChanged);
            this.Ip1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Ip1_KeyPress);
            // 
            // IpInputTextbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Name = "IpInputTextbox";
            this.Size = new System.Drawing.Size(160, 21);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private NumberTextBoxExt Ip1;
        private System.Windows.Forms.Label lbldot1;
        private System.Windows.Forms.Label lbldot3;
        private System.Windows.Forms.Label lbldot2;
        private NumberTextBoxExt Ip4;
        private NumberTextBoxExt Ip3;
        private NumberTextBoxExt Ip2;
    }
}
