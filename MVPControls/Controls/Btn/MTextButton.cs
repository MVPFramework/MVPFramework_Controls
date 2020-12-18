using MVPControls.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using MVPControls.Interop;

namespace MVPControls
{
    /// <summary>
    /// 按钮状态
    /// </summary>
    public enum ButtonStatus
    {
        Normal,
        Override,
        Down,
        Disable
    }

    /// <summary>
    /// MVPFramework TextButton控件
    /// </summary>
    [ToolboxItem(true)]
    public class MTextButton : Button
    {
        private FontType _fontType = FontType.System;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (UseFontType == FontType.CustomFont && !string.IsNullOrEmpty(_customFontName))
            {
                var font = FontManager.GetFont(_customFontName);
                if (font != null)
                {
                    Font = new Font(font, Font.Size);
                }
            }
        }

        /// <summary>
        /// 是否使用自定义字体
        /// </summary>
        [Category("自定义字体")]
        [DefaultValue(FontType.System)]
        public FontType UseFontType
        {
            get
            {
                return _fontType;
            }
            set
            {
                if (_fontType != value)
                {
                    _fontType = value;
                }
                if (_fontType == FontType.CustomFont && !string.IsNullOrEmpty(_customFontName))
                {
                    var font = FontManager.GetFont(_customFontName);
                    if (font != null)
                    {
                        Font = new Font(font, Font.Size);
                        Refresh();
                    }
                    else
                    {
                        _fontType = FontType.System;
                        MessageBox.Show(_customFontName + " 字体不存在,请检查!");
                    }
                }

            }
        }

        /// <summary>
        /// 自定字体名, 使用此属性, 需要将FontType设置为CustomFont
        /// 通常, 会将自定义字体放在Resources.resx文件中
        /// </summary>
        private string _customFontName = string.Empty;
        [Category("自定义字体")]
        public string CustomFontName
        {
            get { return _customFontName; }
            set
            {
                if (_customFontName != value && !string.IsNullOrEmpty(value))
                {
                    _customFontName = value;
                    if (_fontType == FontType.CustomFont)
                    {
                        var font = FontManager.GetFont(_customFontName);
                        if (font != null)
                        {
                            Font = new Font(font, Font.Size);
                            Refresh();
                        }
                        else
                        {
                            _fontType = FontType.System;
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// 控件是否可以接受用户交互(是否可以接受交互事件)
        /// </summary>
        public bool Enable
        {
            get { return Enabled; }
            set
            {
                Status = value ? ButtonStatus.Normal : ButtonStatus.Disable;
                Enabled = value;
                Invalidate();
            }
        }
        #region 按钮状态图片相关

        /// <summary>
        /// 按钮 禁用状态显示的图片
        /// </summary>
        private Image _disableImage;

        /// <summary>
        /// 按钮 按下状态显示的图片
        /// </summary>
        private Image _downImage;

        /// <summary>
        /// 按钮 正常状态显示的图片
        /// </summary>
        private Image _normalImage;

        /// <summary>
        /// 按钮 悬浮状态显示的图片
        /// </summary>
        private Image _overrideImage;

        [Category("自定义图片")]
        public Image DisableImage
        {
            get
            {
                return _disableImage;
            }
            set
            {
                _disableImage = value;
                Invalidate();
            }
        }

        [Category("自定义图片")]
        public Image DownImage
        {
            get
            {
                return _downImage;
            }
            set
            {
                _downImage = value;
                Invalidate();
            }
        }

        [Category("自定义图片")]
        public Image NormalImage
        {
            get
            {
                return _normalImage;
            }
            set
            {
                _normalImage = value;
                Size = _normalImage.Size;
                ClientSize = _normalImage.Size;
                Invalidate();
            }
        }

        [Category("自定义图片")]
        public Image OverrideImage
        {
            get
            {
                return _overrideImage;
            }
            set
            {
                _overrideImage = value;
                // TODO 
                // 设置一个最合适的大小
                Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 按钮状态
        /// </summary>
        public ButtonStatus Status { get; set; }

        /// <summary>
        /// 按钮显示的文字
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                switch (_fontType)
                {
                    case FontType.CustomFont:
                        var font = FontManager.GetFont(_customFontName);
                        if (font != null)
                        {
                            Font = new Font(font, Font.Size);
                        }
                        else
                        {
                            _fontType = FontType.System;
                        }
                        break;
                }
                Invalidate();
            }
        }

        //private Image tempShowButtonImg = null;

        /// <summary>
        /// 按钮的绘制函数
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(Parent.BackColor);

            Image tempShowButtonImg = null;
            switch (Status)
            {
                case ButtonStatus.Normal:
                    tempShowButtonImg = _normalImage;
                    break;
                case ButtonStatus.Down:
                    tempShowButtonImg = _downImage;
                    break;
                case ButtonStatus.Override:
                    tempShowButtonImg = _overrideImage;
                    break;
                case ButtonStatus.Disable:
                    tempShowButtonImg = _disableImage;
                    break;
                default:
                    tempShowButtonImg = _normalImage;
                    break;
            }
            if (tempShowButtonImg != null)
            {
                g.DrawImage(tempShowButtonImg, ClientRectangle);
            }

            var fontSize = g.MeasureString(Text, Font);
            var textRect = new Rectangle(ClientRectangle.Location, new System.Drawing.Size(ClientRectangle.Width, ClientRectangle.Height + (int)(fontSize.Height / 2 * .9f)));

            // 绘制文本
            g.DrawString(
                Text,
                Font,
                new SolidBrush(ForeColor),
                textRect,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        /// <summary>
        /// 处理Windows消息
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode || IsDisposed) return;
            if (Status == ButtonStatus.Disable)
            { // 禁用状态 直接return
                return;
            }

            if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {// 如果鼠标没有悬浮在控件上
                Status = ButtonStatus.Normal;
                return;
            }

            if (m.Msg == Win32.WM_MOUSEMOVE && ClientRectangle.Contains(PointToClient(Cursor.Position)) && Status!= ButtonStatus.Down)
            {
                // 鼠标在控件上悬浮
                Status = ButtonStatus.Override;
                Invalidate();
            }
            else if (m.Msg == Win32.WM_LBUTTONDOWN && ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                // 鼠标左键在控件上点击
                Status = ButtonStatus.Down;
                Invalidate();
            }
            else if (m.Msg == Win32.WM_LBUTTONUP && ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                // 鼠标左键在控件上抬起
                Status = ButtonStatus.Override;
                Invalidate();
            }
            else if (m.Msg == Win32.WM_LBUTTONUP && !ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                // 鼠标左键不在控件的范围抬起
                Status = ButtonStatus.Normal;
                Invalidate();
            }
        }
    }
}
