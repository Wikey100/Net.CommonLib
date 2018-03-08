/**********************************************************
** 文件名： AGMChannel.cs
** 文件作用:进站或出站单向闸机
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
    public class AGMChannel : DrawRectangle
    {
        private int _objectID;
        private int _flag;
        private EDirection _direction;
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

        public AGMChannel()
            : this(0, 0, 1, 1)
        {
        }

        public AGMChannel(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _logicIDTail = "";
            _objectID = _objIdInc++;
            _tagIDBase = 0;
            _flag = _objIdInc++;
            _direction = EDirection.Left;
            _showProperty = false;

            Initialize();
        }

        public override DrawObject Clone()
        {
            AGMChannel drawAGMChannel = new AGMChannel();
            drawAGMChannel.Rectangle = this.Rectangle;
            drawAGMChannel._tagIDBase = this._tagIDBase;
            drawAGMChannel._logicIDTail = this._logicIDTail;
            drawAGMChannel.DeviceIP = this.DeviceIP;
            drawAGMChannel.Direction = this.Direction;
            FillDrawObjectFields(drawAGMChannel);
            return drawAGMChannel;
        }

        public override DrawObject Clone(int n)
        {
            AGMChannel drawAGMChannel = new AGMChannel();
            drawAGMChannel.Rectangle = this.Rectangle;
            drawAGMChannel._tagIDBase = this._tagIDBase;
            drawAGMChannel.DeviceIP = this.DeviceIP;
            drawAGMChannel._logicIDTail = LogicIDAdd(_logicIDTail, n);
            drawAGMChannel.Direction = this.Direction;
            FillDrawObjectFields(drawAGMChannel);
            return drawAGMChannel;
        }

        private Point p1 = new Point(0, 0);
        private Point p2 = new Point(0, 0);
        private Point p3 = new Point(0, 0);
        private Point p4 = new Point(0, 0);
        private Point p5 = new Point(0, 0);
        private Point p6 = new Point(0, 0);
        private Point p7 = new Point(0, 0);

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
            g.DrawRectangle(pen, Rectangle);       //画外框
            if (Entry != null)
            {
                _direction = (Entry == "Left" ? EDirection.Left : Entry == "Right" ? EDirection.Right : Entry == "Up" ? EDirection.Up : EDirection.Down);
            }

            switch (_direction)
            {
                case EDirection.Right:
                    {
                        p1.X = Rectangle.X + Rectangle.Width; p1.Y = Rectangle.Y + Rectangle.Height / 2;
                        p2.X = Rectangle.X + Rectangle.Width / 2; p2.Y = Rectangle.Y + Rectangle.Height / 6;
                        p3.X = Rectangle.X + Rectangle.Width / 2; p3.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p4.X = Rectangle.X; p4.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p5.X = Rectangle.X; p5.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p6.X = Rectangle.X + Rectangle.Width / 2; p6.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p7.X = Rectangle.X + Rectangle.Width / 2; p7.Y = Rectangle.Y + 5 * Rectangle.Height / 6;
                    }
                    break;

                case EDirection.Left:
                    {
                        p1.X = Rectangle.X; p1.Y = Rectangle.Y + Rectangle.Height / 2;
                        p2.X = Rectangle.X + Rectangle.Width / 2; p2.Y = Rectangle.Y + Rectangle.Height / 6;
                        p3.X = Rectangle.X + Rectangle.Width / 2; p3.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p4.X = Rectangle.X + Rectangle.Width; p4.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p5.X = Rectangle.X + Rectangle.Width; p5.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p6.X = Rectangle.X + Rectangle.Width / 2; p6.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p7.X = Rectangle.X + Rectangle.Width / 2; p7.Y = Rectangle.Y + 5 * Rectangle.Height / 6;
                    }
                    break;

                case EDirection.Up:
                    {
                        p1.X = Rectangle.X + Rectangle.Width / 2; p1.Y = Rectangle.Y;
                        p2.X = Rectangle.X + Rectangle.Width / 6; p2.Y = Rectangle.Y + Rectangle.Height / 2;
                        p3.X = Rectangle.X + 2 * Rectangle.Width / 6; p3.Y = Rectangle.Y + Rectangle.Height / 2;
                        p4.X = Rectangle.X + 2 * Rectangle.Width / 6; p4.Y = Rectangle.Y + Rectangle.Height;
                        p5.X = Rectangle.X + 4 * Rectangle.Width / 6; p5.Y = Rectangle.Y + Rectangle.Height;
                        p6.X = Rectangle.X + 4 * Rectangle.Width / 6; p6.Y = Rectangle.Y + Rectangle.Height / 2;
                        p7.X = Rectangle.X + 5 * Rectangle.Width / 6; p7.Y = Rectangle.Y + Rectangle.Height / 2;
                    }
                    break;

                case EDirection.Down:
                    {
                        p1.X = Rectangle.X + Rectangle.Width / 2; p1.Y = Rectangle.Y + Rectangle.Height;
                        p2.X = Rectangle.X + Rectangle.Width / 6; p2.Y = Rectangle.Y + Rectangle.Height / 2;
                        p3.X = Rectangle.X + 2 * Rectangle.Width / 6; p3.Y = Rectangle.Y + Rectangle.Height / 2;
                        p4.X = Rectangle.X + 2 * Rectangle.Width / 6; p4.Y = Rectangle.Y;
                        p5.X = Rectangle.X + 4 * Rectangle.Width / 6; p5.Y = Rectangle.Y;
                        p6.X = Rectangle.X + 4 * Rectangle.Width / 6; p6.Y = Rectangle.Y + Rectangle.Height / 2;
                        p7.X = Rectangle.X + 5 * Rectangle.Width / 6; p7.Y = Rectangle.Y + Rectangle.Height / 2;
                    }
                    break;
            }

            //画箭头
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddLine(p1, p2);
            path.AddLine(p2, p3);
            path.AddLine(p3, p4);
            path.AddLine(p4, p5);
            path.AddLine(p5, p6);
            path.AddLine(p6, p7);
            path.AddLine(p7, p1);
            g.FillPath(brushin, path);
            g.DrawPath(pen, path);
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
            DrawArea drawArea = (DrawArea)sender;
            EditorDialog dlg = new EditorDialog(_logicIDTail, this, "", drawArea.GraphicsList.AGMVerify, DeviceTypeEnum.AGM, DeviceTypeEnum.AGM_Sub_AGMChannel, drawArea.GraphicsList.IPVerify);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (this._tagIDBase != 0)
                {
                    drawArea.GraphicsList.AGMVerify.Remove(_logicIDTail);
                }
                arrayId = dlg.ArrayId;
                _logicIDTail = dlg.IDvalues;
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
                if (_tagIDBase == 0)
                    _flag = _objIdInc++;

                if (drawArea.AddText(this))
                {
                    _tagIDBase = _flag;
                    this.SetRectangle(this.x_axis, this.y_axis, this.width, this.height);
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

        public EDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
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

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public override void ShowItemProperty(bool IsShow)
        {
            this._showProperty = IsShow;
        }

        public override void AntiClockWiseDirection()
        {
            base.AntiClockWiseDirection();
            if (this._direction == EDirection.Left)
                _direction = EDirection.Down;
            else if (_direction == EDirection.Down)
                _direction = EDirection.Right;
            else if (_direction == EDirection.Right)
                _direction = EDirection.Up;
            else if (_direction == EDirection.Up)
                _direction = EDirection.Left;
        }

        public override void ClockWiseDirection()
        {
            base.ClockWiseDirection();
            if (this._direction == EDirection.Left)
                _direction = EDirection.Up;
            else if (_direction == EDirection.Up)
                _direction = EDirection.Right;
            else if (_direction == EDirection.Right)
                _direction = EDirection.Down;
            else if (_direction == EDirection.Down)
                _direction = EDirection.Left;
        }
    }
}