/**********************************************************
** 文件名： Text.cs
** 文件作用:Text设备
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
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class Text : DrawRectangle
    {
        private int _objectID;
        private int flgID;
        private string _textType;
        private string _text;
        private Font _font;
        private Color _fontColor;

        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        public int FlgID
        {
            get { return flgID; }
            set { flgID = value; }
        }

        public Rectangle RectangleLs
        {
            get { return Rectangle; }
            set { Rectangle = value; }
        }

        public string TextType
        {
            get { return _textType; }
            set { _textType = value; }
        }

        public string Texttest
        {
            get { return _text; }
            set { _text = value; }
        }

        public Font TextFont
        {
            get { return _font; }
            set { _font = value; }
        }

        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        public Text() : this(0, 0, 1, 1)
        {
        }

        public Text(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            _objectID = _objIdInc++;
            _text = "Text";
            _font = new Font("宋体", 9, FontStyle.Regular);
            _fontColor = Color.Black;
            _textType = "Text";
            setTextDisplay(x, y);
            Initialize();
        }

        public void setTextDisplay(int x, int y)
        {
            int d = _font.Height;
            float f = _font.Size;
            int a = System.Text.Encoding.Default.GetByteCount(_text);
            int w = (int)(f * a) / 1 * 1;
            Rectangle = new Rectangle(x, y, w, d);
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            Text drawText = new Text();
            drawText.Rectangle = this.Rectangle;
            drawText.TextFont = this.TextFont;
            drawText.ObjectID = this.ObjectID;
            drawText.Texttest = this.Texttest;
            drawText.FontColor = this.FontColor;
            drawText.TextType = this.TextType;
            FillDrawObjectFields(drawText);
            return drawText;
        }

        public override void Draw(Graphics g)
        {
            ContentAlignment alignmentValue = ContentAlignment.BottomLeft;
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Near;
            switch (alignmentValue)
            {
                case ContentAlignment.MiddleLeft:
                    style.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleRight:
                    style.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.MiddleCenter:
                    style.Alignment = StringAlignment.Center;
                    break;
            }
            g.DrawString(
                _text,
                _font,
                new SolidBrush(_fontColor),
                Rectangle, style);
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            AddTextForm dlg = new AddTextForm(_text);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _text = dlg.Textvalues;
                setTextDisplay(this.RectangleLs.X, this.RectangleLs.Y);
                ((DrawArea)sender).SetDirty();
            }
        }

        public override void SetTextColor()
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = this._fontColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _fontColor = dlg.Color;
            }
        }

        public override void SetTextSize()
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = this._font;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _font = dlg.Font;
                setTextDisplay(this.RectangleLs.X, this.RectangleLs.Y);
            }
        }

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public void SetFontColor(string color)
        {
            int first = color.IndexOf(',');
            int second = color.LastIndexOf(',');
            int r = int.Parse(color.Substring(0, first).Trim());
            int g = int.Parse(color.Substring(first + 1, second - (first + 1)).Trim());
            int b = int.Parse(color.Substring(second + 1).Trim());
            this._fontColor = Color.FromArgb(r, g, b);
        }
    }
}