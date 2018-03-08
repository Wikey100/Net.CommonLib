/**********************************************************
** 文件名： SwitchPort.cs
** 文件作用:SwitchPort设备
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class SwitchPort : DrawRectangle
    {
        private string _monitorID;
        private string _deviceID;
        private int _portID;
        private bool _showProperty;
        private int _tagID;

        public SwitchPort()
            : this(0, 0, 1, 1)
        {
        }

        public SwitchPort(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _portID = _objIdInc++;
            _monitorID = "";
            _deviceID = _portID.ToString("D4");
            _tagID = 0;
            Initialize();
        }

        public override DrawObject Clone()
        {
            SwitchPort drawSPort = new SwitchPort();
            _objIdInc--;
            drawSPort.Rectangle = this.Rectangle;
            drawSPort.TagID = TagID;

            FillDrawObjectFields(drawSPort);
            return drawSPort;
        }

        public override DrawObject Clone(int n)
        {
            SwitchPort drawSPort = new SwitchPort();
            drawSPort.Rectangle = this.Rectangle;
            drawSPort.TagID = TagID;

            FillDrawObjectFields(drawSPort);
            return drawSPort;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
            Brush brushin = null;
            brushin = new SolidBrush(Color.FromArgb(255, 0, 255, 0));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            g.FillRectangle(brushin, frect);    //填充区域
            g.DrawRectangle(pen, Rectangle);       //画外框
            Rectangle zrect = new Rectangle(frect.X + (frect.Width - 10) / 2,
                frect.Y + (frect.Height - 10) / 2, frect.Width, frect.Height);
            Brush brushin0 = new SolidBrush(Color.Black);
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Near;
            if (true)
            {
                g.DrawString(
                     PortID.ToString(),
                     new Font("宋体", 10, FontStyle.Regular),
                     brushin0,
                     zrect, style);
            }
            pen.Dispose();
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            DrawArea drawArea = (DrawArea)sender;
            SwitchEditDialog dlg = new SwitchEditDialog(_monitorID, PortID, DeviceID, this);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                MonitorID = dlg.MonitorID;
                PortID = dlg.PortID;
                DeviceID = dlg.DeviceID;
                int i = 0;
                if (TagID == 0)
                    i = _objIdInc++;
                if (drawArea.AddText(this))
                {
                    TagID = i;
                    drawArea.SetDirty();
                }
            }
        }

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

        public string MonitorID
        {
            get { return _monitorID; }
            set { _monitorID = value; }
        }

        public string DeviceID
        {
            get { return _deviceID; }
            set { _deviceID = value; }
        }

        public int PortID
        {
            get { return _portID; }
            set { _portID = value; }
        }

        public int TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public bool ShowProperty
        {
            get { return _showProperty; }
            set { _showProperty = value; }
        }

        public Rectangle RectangleLs
        {
            get { return Rectangle; }
            set { Rectangle = value; }
        }

        public override void ShowItemProperty(bool IsShow)
        {
            this._showProperty = IsShow;
        }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }
    }
}