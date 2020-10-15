using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MVPControls
{
    public class MForm:Form
    {
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        /// <summary>
        /// 指定窗体边框的样式
        /// </summary>
        public new FormBorderStyle FormBorderStyle
        {
            get
            {
                return base.FormBorderStyle;
            }
            set
            {
                base.FormBorderStyle = value;
            }
        }

        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
                Size = BackgroundImage.Size;
                ClientSize = BackgroundImage.Size;
                Refresh();
            }
        }

        private readonly Cursor[] _resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };
        private bool _leftMouseDown;
        private Point _preMousePos;// 鼠标在控件上前一帧的位置

        /// <summary>
        /// 窗口最大化
        /// </summary>
        private bool _maximized;
        public bool Maximezed
        {
            get => _maximized;set => _maximized = value;
        }

        public MForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer|ControlStyles.ResizeRedraw, true);
            _preMousePos = Point.Empty;
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
        }

        /// <summary>
        /// 绘制窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(Color.White);

            //Draw backgroundimage
            if (BackgroundImage != null)
            {
                g.DrawImage(BackgroundImage, ClientRectangle);
            }



            // Determine whether or not we even should be drawing the buttons.
            // TODO
        }

        /// <summary>
        /// 处理Win32消息
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode || IsDisposed) return;

            if (m.Msg == Win32.WM_MOUSEMOVE)
            {
                if (_leftMouseDown)
                {
                    var deltaPos = new Point(Cursor.Position.X - _preMousePos.X, Cursor.Position.Y - _preMousePos.Y);
                    Location = new Point(deltaPos.X + Location.X, deltaPos.Y + Location.Y);
                }
                _preMousePos = Cursor.Position;
            }
            else if (m.Msg == Win32.WM_LBUTTONDOWN)
            {
                _leftMouseDown = true;
            }else if (m.Msg == Win32.WM_LBUTTONUP)
            {
                _leftMouseDown = false;
            }
        }

        /// <summary>
        /// 鼠标移动回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDisposed) return;

            var clientCursorPos = PointToClient(e.Location); 
            OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0));
        }
    }

    /// <summary>
    /// 鼠标移动消息处理
    /// </summary>
    public class MouseMessageFilter : IMessageFilter
    {
        public static event MouseEventHandler MouseMove;

        public bool PreFilterMessage(ref Message m)
        {
            if(m.Msg == Win32.WM_MOUSEMOVE && MouseMove != null)
            {
                int x = Control.MousePosition.X, y = Control.MousePosition.Y;
                MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
            }

            return false;
        }
    }
}
