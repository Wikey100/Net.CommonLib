using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Net.CommonLib.Controls
{
    [ToolboxBitmap(typeof(RadioButton))]
    public class RadioButtonEx : RadioButton
    {
        private Color _baseColor = Color.FromArgb(51, 161, 224);
        private ControlState _controlState;

        private static readonly ContentAlignment RightAlignment =
            ContentAlignment.TopRight |
            ContentAlignment.BottomRight |
            ContentAlignment.MiddleRight;

        private static readonly ContentAlignment LeftAligbment =
            ContentAlignment.TopLeft |
            ContentAlignment.BottomLeft |
            ContentAlignment.MiddleLeft;

        public RadioButtonEx()
            : base()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
        }

        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                base.Invalidate();
            }
        }

        protected virtual int DefaultCheckButtonWidth
        {
            get { return 12; }
        }

        internal ControlState ControlState
        {
            get { return _controlState; }
            set
            {
                if (_controlState != value)
                {
                    _controlState = value;
                    base.Invalidate();
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ControlState = ControlState.Hover;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ControlState = ControlState.Pressed;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (ClientRectangle.Contains(e.Location))
                {
                    ControlState = ControlState.Hover;
                }
                else
                {
                    ControlState = ControlState.Normal;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            Rectangle radioButtonrect;
            Rectangle textRect;

            CalculateRect(out radioButtonrect, out textRect);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color borderColor;
            Color innerBorderColor;
            Color checkColor;
            bool hover = false;

            if (Enabled)
            {
                switch (ControlState)
                {
                    case ControlState.Hover:
                        borderColor = _baseColor;
                        innerBorderColor = _baseColor;
                        checkColor = GetColor(_baseColor, 0, 35, 24, 9);
                        hover = true;
                        break;

                    case ControlState.Pressed:
                        borderColor = _baseColor;
                        innerBorderColor = GetColor(_baseColor, 0, -13, -8, -3);
                        checkColor = GetColor(_baseColor, 0, -35, -24, -9);
                        hover = true;
                        break;

                    default:
                        borderColor = _baseColor;
                        innerBorderColor = Color.Empty;
                        checkColor = _baseColor;
                        break;
                }
            }
            else
            {
                borderColor = SystemColors.ControlDark;
                innerBorderColor = SystemColors.ControlDark;
                checkColor = SystemColors.ControlDark;
            }

            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, radioButtonrect);
            }

            if (hover)
            {
                using (Pen pen = new Pen(innerBorderColor, 2F))
                {
                    g.DrawEllipse(pen, radioButtonrect);
                }
            }

            if (Checked)
            {
                radioButtonrect.Inflate(-2, -2);
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(radioButtonrect);
                    using (PathGradientBrush brush = new PathGradientBrush(path))
                    {
                        brush.CenterColor = checkColor;
                        brush.SurroundColors = new Color[] { Color.White };
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, 0.4f, 1f };
                        blend.Factors = new float[] { 0f, 0.4f, 1f };
                        brush.Blend = blend;
                        g.FillEllipse(brush, radioButtonrect);
                    }
                }
                radioButtonrect.Inflate(2, 2);
            }

            using (Pen pen = new Pen(borderColor))
            {
                g.DrawEllipse(pen, radioButtonrect);

                //System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Gray, 2);
                //e.Graphics.DrawRectangle(pen, 0, 0, this.Width, this.Height);
            }

            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));

            System.Drawing.Pen pen2 = new System.Drawing.Pen(System.Drawing.Color.Gray, 1);

            e.Graphics.DrawRectangle(pen2, textRect.Location.X, textRect.Location.Y, textRect.Width - 1, textRect.Height - 1);
        }

        private void CalculateRect(
            out Rectangle radioButtonRect, out Rectangle textRect)
        {
            radioButtonRect = new Rectangle(
                0, 0, DefaultCheckButtonWidth, DefaultCheckButtonWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAligbment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = RightToLeft == RightToLeft.Yes;

            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        radioButtonRect.Y = 2;
                        break;

                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        radioButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;

                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        radioButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                radioButtonRect.X = 1;

                textRect = new Rectangle(
                    radioButtonRect.Right + 2,
                    0,
                    Width - radioButtonRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        radioButtonRect.Y = 2;
                        break;

                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        radioButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;

                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        radioButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                radioButtonRect.X = Width - DefaultCheckButtonWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - DefaultCheckButtonWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        radioButtonRect.Y = 2;
                        textRect.Y = radioButtonRect.Bottom + 2;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;

                    case ContentAlignment.MiddleCenter:
                        radioButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;

                    case ContentAlignment.BottomCenter:
                        radioButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;
                }

                radioButtonRect.X = (Width - DefaultCheckButtonWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        private Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;

            if (a + a0 > 255) { a = 255; } else { a = Math.Max(a + a0, 0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(r + r0, 0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(g + g0, 0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(b + b0, 0); }

            return Color.FromArgb(a, r, g, b);
        }

        internal static TextFormatFlags GetTextFormatFlags(
            ContentAlignment alignment,
            bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;

                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;

                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;

                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;

                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;

                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;

                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;

                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;

                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }
    }

    /// <summary>
    /// 控件的状态。
    /// </summary>
    public enum ControlState
    {
        /// <summary>
        ///  正常。
        /// </summary>
        Normal,

        /// <summary>
        /// 鼠标进入。
        /// </summary>
        Hover,

        /// <summary>
        /// 鼠标按下。
        /// </summary>
        Pressed,

        /// <summary>
        /// 获得焦点。
        /// </summary>
        Focused,
    }
}