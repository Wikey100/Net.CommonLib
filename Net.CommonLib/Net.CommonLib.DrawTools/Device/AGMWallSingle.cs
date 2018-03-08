/**********************************************************
** 文件名： AGMWallSingle.cs
** 文件作用:单向检票闸机门
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawTools.Device
{
    public class AGMWallSingle : DrawRectangle
    {
        private int _objectID;
        private int _flag;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;
        private EDirection _direction;

        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
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

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public AGMWallSingle()
            : this(0, 0, 1, 1)
        {
        }

        public AGMWallSingle(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _logicIDTail = "";
            _objectID = _objIdInc++;
            _direction = EDirection.Left;
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            AGMWallSingle drawAGMWallSingle = new AGMWallSingle();
            _objIdInc--;
            drawAGMWallSingle.Rectangle = this.Rectangle;

            drawAGMWallSingle._tagIDBase = this._tagIDBase;//绑定的label
            drawAGMWallSingle._logicIDTail = _logicIDTail;
            drawAGMWallSingle.Direction = this.Direction;

            FillDrawObjectFields(drawAGMWallSingle);
            return drawAGMWallSingle;
        }

        public override DrawObject Clone(int n)
        {
            AGMWallSingle drawAGMWallSingle = new AGMWallSingle();
            drawAGMWallSingle.Rectangle = this.Rectangle;
            drawAGMWallSingle._tagIDBase = this._tagIDBase;//绑定的label
            drawAGMWallSingle._logicIDTail = LogicIDAdd(_logicIDTail, n); //解决设备ID相同问题

            FillDrawObjectFields(drawAGMWallSingle);
            return drawAGMWallSingle;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
            Brush brushout = new SolidBrush(Color.FromArgb(255, 150, 150, 150));
            Brush brushin = new SolidBrush(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            g.DrawRectangle(pen, Rectangle);       //画外框
            g.FillRectangle(brushout, frect);    //填充区域
            int clientWidth = Rectangle.Width;
            int clientHeight = Rectangle.Height;
            int widther = (clientWidth > clientHeight) ? clientWidth : clientHeight;
            int shorter = (clientWidth > clientHeight) ? clientHeight : clientWidth;
            int blockWidth = (clientWidth > clientHeight) ? widther / 5 : shorter / 3;
            int blockHeight = (clientWidth > clientHeight) ? shorter / 3 : widther / 5;
            int blockX = (clientWidth > clientHeight) ? 7 * widther / 10 : shorter / 3;
            int blockY = (clientWidth > clientHeight) ? shorter / 3 : 7 * widther / 10;

            if (Entry != null)
            {
                _direction = (Entry == "Left" ? EDirection.Left : Entry == "Right" ? EDirection.Right : Entry == "Up" ? EDirection.Up : EDirection.Down);
            }

            switch (_direction)
            {
                case EDirection.Left:
                    {
                        blockWidth = Rectangle.Width / 5;
                        blockHeight = Rectangle.Height / 3;

                        blockX = Rectangle.X + Rectangle.Width / 10; blockY = Rectangle.Y + Rectangle.Height / 3;
                    }
                    break;

                case EDirection.Right:
                    {
                        blockWidth = Rectangle.Width / 5;
                        blockHeight = Rectangle.Height / 3;

                        blockX = Rectangle.X + 7 * Rectangle.Width / 10; blockY = Rectangle.Y + Rectangle.Height / 3;
                    }
                    break;

                case EDirection.Down:
                    {
                        blockWidth = Rectangle.Width / 3;
                        blockHeight = Rectangle.Height / 5;

                        blockX = Rectangle.X + Rectangle.Width / 3; blockY = Rectangle.Y + 7 * Rectangle.Height / 10;
                    }
                    break;

                case EDirection.Up:
                    {
                        blockWidth = Rectangle.Width / 3;
                        blockHeight = Rectangle.Height / 5;

                        blockX = Rectangle.X + Rectangle.Width / 3; blockY = Rectangle.Y + Rectangle.Height / 10;
                    }
                    break;
            }
            Rectangle rect = new Rectangle(blockX, blockY, blockWidth, blockHeight);
            g.DrawRectangle(pen, rect);       //画内框
            g.FillRectangle(brushin, rect);
            pen.Dispose();
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

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            base.MoveHandleTo(point, handleNumber);

            if (this.Rectangle.Width > this.Rectangle.Height)
                Direction = EDirection.Left;
            else
                Direction = EDirection.Up;
        }
    }
}