/**********************************************************
** 文件名： AGMWallDual.cs
** 文件作用:双向可检票闸机门
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
    public class AGMWallDual : DrawRectangle
    {
        private int _objectID;
        private int _flag;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;

        private HVDirection _direction;

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

        public HVDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public AGMWallDual()
            : this(0, 0, 1, 1)
        {
        }

        public AGMWallDual(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _objectID = _objIdInc++;
            _logicIDTail = "";
            _direction = HVDirection.Horizontal;

            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            AGMWallDual drawAGMWallDual = new AGMWallDual();
            _objIdInc--;
            drawAGMWallDual.Rectangle = this.Rectangle;

            drawAGMWallDual._tagIDBase = this._tagIDBase;//绑定的label
            drawAGMWallDual._logicIDTail = _logicIDTail;
            drawAGMWallDual.Direction = this.Direction;

            FillDrawObjectFields(drawAGMWallDual);
            return drawAGMWallDual;
        }

        public override DrawObject Clone(int n)
        {
            AGMWallDual drawAGMWallDual = new AGMWallDual();
            drawAGMWallDual.Rectangle = this.Rectangle;
            drawAGMWallDual._tagIDBase = this._tagIDBase;//绑定的label
            drawAGMWallDual._logicIDTail = LogicIDAdd(_logicIDTail, n); //解决设备ID相同问题

            FillDrawObjectFields(drawAGMWallDual);
            return drawAGMWallDual;
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
            int blockWidth = 0;
            int blockHeight = 0;
            int blockX_1 = 0;
            int blockY_1 = 0;
            int blockX_2 = 0;
            int blockY_2 = 0;
            if (Entry != null)
            {
                _direction = (Entry == "Horizontal" ? HVDirection.Horizontal : HVDirection.Vertical);
            }

            switch (Direction)
            {
                case HVDirection.Horizontal:
                    {
                        blockWidth = Rectangle.Width / 5;
                        blockHeight = Rectangle.Height / 3;

                        blockX_1 = Rectangle.X + Rectangle.Width / 10; blockY_1 = Rectangle.Y + Rectangle.Height / 3;
                        blockX_2 = Rectangle.X + 7 * Rectangle.Width / 10; blockY_2 = Rectangle.Y + Rectangle.Height / 3;
                    }
                    break;

                case HVDirection.Vertical:
                    {
                        blockWidth = Rectangle.Width / 3;
                        blockHeight = Rectangle.Height / 5;

                        blockX_1 = Rectangle.X + Rectangle.Width / 3; blockY_1 = Rectangle.Y + Rectangle.Height / 10;
                        blockX_2 = Rectangle.X + Rectangle.Width / 3; blockY_2 = Rectangle.Y + 7 * Rectangle.Height / 10;
                    }
                    break;
            }
            Rectangle rect1 = new Rectangle(blockX_1, blockY_1, blockWidth, blockHeight);
            Rectangle rect2 = new Rectangle(blockX_2, blockY_2, blockWidth, blockHeight);
            g.DrawRectangle(pen, rect1);       //画内框1
            g.FillRectangle(brushin, rect1);
            g.DrawRectangle(pen, rect2);       //画内框2
            g.FillRectangle(brushin, rect2);
            pen.Dispose();
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