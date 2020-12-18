using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using MVPControls.Interop;

namespace MVPControls
{
    public class MTransparentForm : Form
    {
        [Browsable(false)]
        public MouseState MouseState { get; set; }

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

        private bool _isLayeredWindowForm = true;
        [Category("MVPControls"), Description("Is the LayeredWindow?")]
        public virtual bool IsLayeredWindowForm
        {
            get
            {
                return _isLayeredWindowForm;
            }
            set
            {
                this.IsLayeredWindowForm = value;
            }
        }

        private readonly Cursor[] _resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };
        private bool _leftMouseDown;
        private System.Drawing.Point _preMousePos;// 鼠标在控件上前一帧的位置
        private bool haveHandle = false; // 已创建窗口句柄?

        public MTransparentForm()
        {
            FormBorderStyle = FormBorderStyle.None;// 无边框模式
            _preMousePos = System.Drawing.Point.Empty;
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
            InitializeComponent();
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                // 虽然启用了SupportsTransparentBackColor， 但是这个只在DesignMode模式下有效
                // 具体原因待查
                // 所以, 用下面代码来规避运行时BackColor被设置为Color.Transparent的情况
                if (!DesignMode && value == Color.Transparent)
                    base.BackColor = Color.Black;
                else
                    base.BackColor = value;

            }
        }

        private void InitializeStyles()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            InitializeStyles();
            base.OnHandleCreated(e);
            haveHandle = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000;
                if (!base.DesignMode)
                {
                    cp.ExStyle |= 0x00080000;  //  WS_EX_LAYERED 扩展样式
                }
                return cp;
            }
        }

        public void SetBitmap(Bitmap bitmap, byte opacity)
        {
            if (!haveHandle) return;

            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("位图必须是32位包含alpha 通道");

            IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));   // 创建GDI位图句柄，效率较低
                oldBitmap = Win32.SelectObject(memDc, hBitmap);

                Interop.Size size = new Interop.Size(bitmap.Width, bitmap.Height);
                Interop.Point pointSource = new Interop.Point(0, 0);
                Interop.Point topPos = new Interop.Point(Left, Top);
                Interop.BLENDFUNCTION blend = new Interop.BLENDFUNCTION();
                blend.BlendOp = Win32.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = Win32.AC_SRC_ALPHA;

                Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBitmap);

                    Win32.DeleteObject(hBitmap);
                }
                Win32.DeleteDC(memDc);
            }
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
                    var deltaPos = new System.Drawing.Point(Cursor.Position.X - _preMousePos.X, Cursor.Position.Y - _preMousePos.Y);
                    Location = new System.Drawing.Point(deltaPos.X + Location.X, deltaPos.Y + Location.Y);
                }
                _preMousePos = Cursor.Position;
            }
            else if (m.Msg == Win32.WM_LBUTTONDOWN)
            {
                _leftMouseDown = true;
            }
            else if (m.Msg == Win32.WM_LBUTTONUP)
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Load += new System.EventHandler(this.MForm_Load);
            this.ResumeLayout();
        }

        private void MForm_Load(object sender, EventArgs e)
        {
            if (BackgroundImage != null)
            {
                SetBitmap(new Bitmap(BackgroundImage), 255);
            }
        }
    }
}
