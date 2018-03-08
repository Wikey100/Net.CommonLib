/**********************************************************
** 文件名： AGMWallDummy.cs
** 文件作用:无检票闸机门
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
    public class AGMWallDummy : DrawRectangle
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

        public AGMWallDummy()
            : this(0, 0, 1, 1)
        {
        }

        public AGMWallDummy(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _logicIDTail = "";
            _objectID = _objIdInc++;
            _direction = HVDirection.Horizontal;

            Initialize();
        }

        public override DrawObject Clone()
        {
            AGMWallDummy drawAGMWallDummy = new AGMWallDummy();
            _objIdInc--;
            drawAGMWallDummy.Rectangle = this.Rectangle;
            drawAGMWallDummy._tagIDBase = this._tagIDBase;//绑定的label
            drawAGMWallDummy._logicIDTail = _logicIDTail;
            drawAGMWallDummy.Direction = this.Direction;
            FillDrawObjectFields(drawAGMWallDummy);
            return drawAGMWallDummy;
        }

        public override DrawObject Clone(int n)
        {
            AGMWallDummy drawAGMWallDummy = new AGMWallDummy();
            drawAGMWallDummy.Rectangle = this.Rectangle;
            drawAGMWallDummy._tagIDBase = this._tagIDBase;//绑定的label
            drawAGMWallDummy._logicIDTail = LogicIDAdd(_logicIDTail, n); //解决设备ID相同问题
            FillDrawObjectFields(drawAGMWallDummy);
            return drawAGMWallDummy;
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
    }
}