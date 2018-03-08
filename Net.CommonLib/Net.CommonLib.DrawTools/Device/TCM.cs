/**********************************************************
** 文件名： TCM.cs
** 文件作用:TCM设备
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using DrawTools.Model;
using DrawTools.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class TCM : DrawRectangle
    {
        private int _objectID;
        private int _flag;

        private int x_axis;
        private int y_axis;
        private int width;
        private int height;
        private bool _showProperty;

        public TCM()
            : this(0, 0, 1, 1)
        {
        }

        public TCM(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _objectID = _objIdInc++;
            _logicIDTail = "";
            _tagIDBase = 0;
            _showProperty = false;
            _flag = _objIdInc;
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            TCM drawTCM = new TCM();
            drawTCM.Rectangle = this.Rectangle;
            drawTCM._tagIDBase = this._tagIDBase;
            drawTCM._logicIDTail = this._logicIDTail;
            drawTCM.DeviceIP = this.DeviceIP;

            FillDrawObjectFields(drawTCM);
            return drawTCM;
        }

        public override DrawObject Clone(int n)
        {
            TCM drawTCM = new TCM();
            drawTCM.Rectangle = this.Rectangle;
            drawTCM._tagIDBase = this._tagIDBase;
            drawTCM.DeviceIP = this.DeviceIP;
            drawTCM._logicIDTail = LogicIDAdd(_logicIDTail, n);

            FillDrawObjectFields(drawTCM);
            return drawTCM;
        }

        public override void Draw(Graphics g)
        {
            Rectangle rect_down = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Image img = new Bitmap(Resources.TCM);
            g.DrawImage(img, rect_down);
            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Brush brushin0 = new SolidBrush(Color.Red);
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            if (_showProperty)
            {
                g.DrawString(
                     _logicIDTail,
                     new Font("宋体", 9, FontStyle.Regular),
                     brushin0,
                     frect, style);
            }
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            DrawArea drawArea = (DrawArea)sender;
            EditorDialog dlg = new EditorDialog(_logicIDTail, this, "", drawArea.GraphicsList.TCMVerify, DeviceTypeEnum.TCM, DeviceTypeEnum.TCM, drawArea.GraphicsList.IPVerify);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (this._tagIDBase != 0)
                {
                    drawArea.GraphicsList.TCMVerify.Remove(_logicIDTail);
                }
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

                    this.SetRectangle(x_axis, y_axis, width, height);
                    drawArea.SetDirty();
                    drawArea.Refresh();
                }
            }
        }

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

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
    }
}