/**********************************************************
** 文件名： Switch.cs
** 文件作用:Switch设备
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
    public class Switch : DrawRectangle
    {
        private string _switchID;
        private bool _showProperty;
        private HVDirection _direction;

        public Switch()
            : this(0, 0, 1, 1)
        {
        }

        public Switch(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            int id = _objIdInc++;
            _switchID = string.Format("SW{0}", id.ToString("D2"));
            _direction = HVDirection.Horizontal;
            Initialize();
        }

        public override DrawObject Clone()
        {
            Switch drawSwitch = new Switch();
            _objIdInc--;
            // _objIdInc--;
            drawSwitch.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawSwitch);
            return drawSwitch;
        }

        public override DrawObject Clone(int n)
        {
            Switch drawSwitch = new Switch();
            // _objIdInc++;
            drawSwitch.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawSwitch);
            return drawSwitch;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
            // g.DrawRectangle(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));
            Brush brushout = new SolidBrush(Color.FromArgb(255, 150, 150, 150));
            Brush brushin = new SolidBrush(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            g.DrawRectangle(pen, Rectangle);       //画外框
            g.FillRectangle(brushout, frect);    //填充区域
            Rectangle zrect = new Rectangle(frect.X, frect.Y, frect.Width, frect.Height);
            Brush brushin0 = new SolidBrush(Color.Black);
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Near;
            if (true)
            {
                g.DrawString(
                     string.Format("交换机:{0}", SwitchID),
                     new Font("宋体", 10, FontStyle.Regular),
                     brushin0,
                     zrect, style);
            }

            pen.Dispose();
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            DrawArea drawArea = (DrawArea)sender;
            SwitchEditDialog dlg = new SwitchEditDialog(_switchID, this);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SwitchID = dlg.MonitorID;

                if (drawArea.AddText(this))
                {
                    drawArea.SetDirty();
                }
            }
        }

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

        public string SwitchID
        {
            get { return _switchID; }
            set { _switchID = value; }
        }

        public HVDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
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