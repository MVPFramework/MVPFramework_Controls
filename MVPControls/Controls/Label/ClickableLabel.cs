using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MVPControls
{
    /// <summary>
    /// 可以点击的Label
    /// </summary>
    public class ClickableLabel:Label
    {
        /// <summary>
        /// 文本显示的正常颜色
        /// </summary>
        [Browsable(false)]
        public Color NormalColor { get; set; }

        /// <summary>
        /// 鼠标覆盖在文本上的颜色
        /// </summary>
        [Category("字体交互颜色")]
        public Color OverrideColor { get; set; }

        /// <summary>
        /// 鼠标按下去的时候文本的颜色
        /// </summary>
        [Category("字体交互颜色")]
        public Color DownColor { get; set; }


        private FontType _fontType = FontType.System;
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
                if (DesignMode)
                {
                    if (_fontType == FontType.System)
                    {
                        Refresh();
                    }
                    else if (_fontType == FontType.CustomFont && !string.IsNullOrEmpty(_customFontName))
                    {
                        var font = FontManager.GetFont(_customFontName);
                        if (font != null)
                        {
                            Font = new Font(font, Font.Size);
                            Refresh();
                        }
                        else
                        {
                            MessageBox.Show(_customFontName + " 字体不存在,请检查!");
                        }
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
                        Font = new Font(FontManager.GetFont(_customFontName), Font.Size);
                        Refresh();
                    }
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (UseFontType == FontType.CustomFont && !string.IsNullOrEmpty(_customFontName))
            {
                Font = new Font(FontManager.GetFont(_customFontName), Font.Size);
            }
            BackColor = Color.Transparent;
            NormalColor = ForeColor;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
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
                    MessageBox.Show(_customFontName + " 字体不存在,请检查!");
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ForeColor = OverrideColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ForeColor = NormalColor;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if(ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                ForeColor = OverrideColor;
            }
            else
            {
                ForeColor = NormalColor;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ForeColor = DownColor;
        }
    }
}
