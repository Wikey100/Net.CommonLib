/**********************************************************
** 文件名： AGMChannelDual.cs
** 文件作用:双向闸机
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using DrawTools.Model;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class AGMChannelDual : DrawRectangle
    {
        private int _objectID;
        private int _flag;
        private HVDirection _direction;
        private EDirection _entry;
        private bool _showProperty;
        private string arrayId;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;

        public string ArrayId
        {
            get { return arrayId; }
            set { arrayId = value; }
        }

        public AGMChannelDual()
            : this(0, 0, 1, 1)
        {
        }

        public AGMChannelDual(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _logicIDTail = "";
            _objectID = _objIdInc++;
            _tagIDBase = 0;
            _flag = _objIdInc++;
            _direction = HVDirection.Horizontal;
            _entry = EDirection.Left;
            _showProperty = false;
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            AGMChannelDual drawAGMChannelDual = new AGMChannelDual();
            drawAGMChannelDual.Rectangle = this.Rectangle;
            drawAGMChannelDual._tagIDBase = this._tagIDBase;
            drawAGMChannelDual._logicIDTail = this._logicIDTail;
            drawAGMChannelDual.DeviceIP = this.DeviceIP;
            drawAGMChannelDual.Direction = this.Direction;

            FillDrawObjectFields(drawAGMChannelDual);
            return drawAGMChannelDual;
        }

        public override DrawObject Clone(int n)
        {
            AGMChannelDual drawAGMChannelDual = new AGMChannelDual();
            drawAGMChannelDual.Rectangle = this.Rectangle;
            drawAGMChannelDual._tagIDBase = this._tagIDBase;
            drawAGMChannelDual.DeviceIP = this.DeviceIP;
            drawAGMChannelDual._logicIDTail = LogicIDAdd(_logicIDTail, n);
            drawAGMChannelDual.Direction = this.Direction;
            FillDrawObjectFields(drawAGMChannelDual);
            return drawAGMChannelDual;
        }

        private Point p11 = new Point(0, 0);
        private Point p12 = new Point(0, 0);
        private Point p13 = new Point(0, 0);
        private Point p14 = new Point(0, 0);
        private Point p15 = new Point(0, 0);
        private Point p16 = new Point(0, 0);
        private Point p17 = new Point(0, 0);

        private Point p21 = new Point(0, 0);
        private Point p22 = new Point(0, 0);
        private Point p23 = new Point(0, 0);
        private Point p24 = new Point(0, 0);
        private Point p25 = new Point(0, 0);
        private Point p26 = new Point(0, 0);
        private Point p27 = new Point(0, 0);

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);

            Brush brushout = null;
            if (_tagIDBase == 0)
                brushout = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
            else
                brushout = new SolidBrush(Color.FromArgb(255, 0, 255, 0));

            Brush brushin = new SolidBrush(Color.White);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            g.FillRectangle(brushout, frect);    //填充区域

            if (Angle != null)
            {
                _direction = (Angle == "Horizontal" ? HVDirection.Horizontal : HVDirection.Vertical);
            }

            //宽度大于高度，箭头横向
            if (Rectangle.Width > Rectangle.Height)
            {
                _direction = HVDirection.Horizontal;
            }
            //宽度小于高度，箭头竖向
            if (Rectangle.Width < Rectangle.Height)
            {
                _direction = HVDirection.Vertical;
            }

            switch (_direction)
            {
                case HVDirection.Horizontal:
                    {
                        p11.X = Rectangle.X + Rectangle.Width / 2; p11.Y = Rectangle.Y + Rectangle.Height / 2;
                        p12.X = Rectangle.X + Rectangle.Width / 4; p12.Y = Rectangle.Y + Rectangle.Height / 6;
                        p13.X = Rectangle.X + Rectangle.Width / 4; p13.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p14.X = Rectangle.X; p14.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p15.X = Rectangle.X; p15.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p16.X = Rectangle.X + Rectangle.Width / 4; p16.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p17.X = Rectangle.X + Rectangle.Width / 4; p17.Y = Rectangle.Y + 5 * Rectangle.Height / 6;

                        p21.X = Rectangle.X + Rectangle.Width / 2; p21.Y = Rectangle.Y + Rectangle.Height / 2;
                        p22.X = Rectangle.X + 3 * Rectangle.Width / 4; p22.Y = Rectangle.Y + Rectangle.Height / 6;
                        p23.X = Rectangle.X + 3 * Rectangle.Width / 4; p23.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p24.X = Rectangle.X + Rectangle.Width; p24.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p25.X = Rectangle.X + Rectangle.Width; p25.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p26.X = Rectangle.X + 3 * Rectangle.Width / 4; p26.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p27.X = Rectangle.X + 3 * Rectangle.Width / 4; p27.Y = Rectangle.Y + 5 * Rectangle.Height / 6;
                    }
                    break;

                case HVDirection.Vertical:
                    {
                        p11.X = Rectangle.X + Rectangle.Width / 2; p11.Y = Rectangle.Y + Rectangle.Height / 2;
                        p12.X = Rectangle.X + Rectangle.Width / 6; p12.Y = Rectangle.Y + Rectangle.Height / 4;
                        p13.X = Rectangle.X + 2 * Rectangle.Width / 6; p13.Y = Rectangle.Y + Rectangle.Height / 4;
                        p14.X = Rectangle.X + 2 * Rectangle.Width / 6; p14.Y = Rectangle.Y;
                        p15.X = Rectangle.X + 4 * Rectangle.Width / 6; p15.Y = Rectangle.Y;
                        p16.X = Rectangle.X + 4 * Rectangle.Width / 6; p16.Y = Rectangle.Y + Rectangle.Height / 4;
                        p17.X = Rectangle.X + 5 * Rectangle.Width / 6; p17.Y = Rectangle.Y + Rectangle.Height / 4;

                        p21.X = Rectangle.X + Rectangle.Width / 2; p21.Y = Rectangle.Y + Rectangle.Height / 2;
                        p22.X = Rectangle.X + 1 * Rectangle.Width / 6; p22.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                        p23.X = Rectangle.X + 2 * Rectangle.Width / 6; p23.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                        p24.X = Rectangle.X + 2 * Rectangle.Width / 6; p24.Y = Rectangle.Y + Rectangle.Height;
                        p25.X = Rectangle.X + 4 * Rectangle.Width / 6; p25.Y = Rectangle.Y + Rectangle.Height;
                        p26.X = Rectangle.X + 4 * Rectangle.Width / 6; p26.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                        p27.X = Rectangle.X + 5 * Rectangle.Width / 6; p27.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                    }
                    break;
            }

            //画箭头
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddLine(p11, p12);
            path.AddLine(p12, p13);
            path.AddLine(p13, p14);
            path.AddLine(p14, p15);
            path.AddLine(p15, p16);
            path.AddLine(p16, p17);
            path.AddLine(p17, p11);

            g.FillPath(brushin, path);
            g.DrawPath(pen, path);

            path = new GraphicsPath();

            path.StartFigure();
            path.AddLine(p21, p22);
            path.AddLine(p22, p23);
            path.AddLine(p23, p24);
            path.AddLine(p24, p25);
            path.AddLine(p25, p26);
            path.AddLine(p26, p27);
            path.AddLine(p27, p21);

            g.FillPath(brushin, path);
            g.DrawPath(pen, path);

            g.DrawRectangle(pen, Rectangle);       //画外框

            Brush brushin0 = new SolidBrush(Color.Red);
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            if (_showProperty)
            {
                g.DrawString(
                     _logicIDTail,
                     new Font("宋体", 9, FontStyle.Regular),
                     brushin0,
                     Rectangle, style);
            }
            pen.Dispose();
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            string entryValue;
            if (_entry == EDirection.Left)
                entryValue = "Left";
            else if (_entry == EDirection.Right)
                entryValue = "Right";
            else if (_entry == EDirection.Up)
            {
                entryValue = "Up";
            }
            else
                entryValue = "Down";

            DrawArea drawArea = (DrawArea)sender;

            EditorDialog dlg = new EditorDialog(_logicIDTail, this, entryValue, drawArea.GraphicsList.AGMVerify, DeviceTypeEnum.AGM, DeviceTypeEnum.AGM_Sub_AGMChannelDual, drawArea.GraphicsList.IPVerify);
            // dlg.ArrayId = arrayId;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (this._tagIDBase != 0)
                {
                    drawArea.GraphicsList.AGMVerify.Remove(_logicIDTail);
                }
                _logicIDTail = dlg.IDvalues;
                arrayId = dlg.ArrayId;
                this.x_axis = dlg.X_axis;
                this.y_axis = dlg.Y_axis;
                this.width = dlg.RWidth;
                this.height = dlg.RHeight;

                RecID = dlg.RecID;
                StationID = dlg.StationID;
                DeviceID = dlg.DeviceID;
                DeviceName = dlg.DeviceName;
                IpAdd = dlg.IpAdd;
                DeviceType = dlg.DeviceType;
                DeviceSubType = dlg.DeviceSubType;
                GroupID = dlg.GroupID;
                this.Device_H = dlg.Device_H;
                this.Device_W = dlg.Device_W;

                if (dlg.Entryvalues == "Left")
                    this.Entry = EDirection.Left;
                else if (dlg.Entryvalues == "Right")
                    this.Entry = EDirection.Right;
                else if (dlg.Entryvalues == "Up")
                    this.Entry = EDirection.Up;
                else
                    this.Entry = EDirection.Down;

                if (_tagIDBase == 0)
                    _flag = _objIdInc++;
                if (drawArea.AddText(this))
                {
                    _tagIDBase = _flag;

                    this.SetRectangle(x_axis, y_axis, width, height);
                    drawArea.SetDirty();
                    drawArea.Refresh();
                }
            }
        }

        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        public Rectangle RectangleLs
        {
            get { return Rectangle; }
            set { Rectangle = value; }
        }

        public HVDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public EDirection Entry
        {
            get { return _entry; }
            set { _entry = value; }
        }

        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public bool ShowProperty
        {
            get { return _showProperty; }
            set { _showProperty = value; }
        }

        public override void ShowItemProperty(bool IsShow)
        {
            this._showProperty = IsShow;
        }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public override void AntiClockWiseDirection()
        {
            base.AntiClockWiseDirection();
            if (this._direction == HVDirection.Horizontal)
                _direction = HVDirection.Vertical;
            else if (_direction == HVDirection.Vertical)
                _direction = HVDirection.Horizontal;
        }

        public override void ClockWiseDirection()
        {
            base.ClockWiseDirection();
            if (this._direction == HVDirection.Horizontal)
                _direction = HVDirection.Vertical;
            else if (_direction == HVDirection.Vertical)
                _direction = HVDirection.Horizontal;
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            base.MoveHandleTo(point, handleNumber);

            if (this.Rectangle.Width > this.Rectangle.Height)
                Direction = HVDirection.Horizontal;
            else
                Direction = HVDirection.Vertical;
        }
    }
}