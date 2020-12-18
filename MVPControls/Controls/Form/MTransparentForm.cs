using MVPControls.Interop;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MVPControls
{
    /// <summary>
    /// 思路:
    /// 采用双层窗口：底层背景窗口层与顶层控件层，
    /// 用控件层Show()背景层,同时处理窗体的窗口移动事件，
    /// 让另外一个窗体同步移动或者做其它事情。
    /// </summary>
    public class MTransparentForm : Form
    {
        private System.Drawing.Point _preMousePos;// 鼠标在控件上前一帧的位置
        private bool haveHandle = false; // 已创建窗口句柄?
        private MForm Main;// 控件层

        public MTransparentForm(MForm main)
        {
            if (main == null)
            {
                throw new ApplicationException("必须传递一个MForm实例进来!");
            }
                
            this.Main = main;
            FormBorderStyle = FormBorderStyle.None;// 无边框模式
            Initialize();
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

        public void SetBits()
        {
            if (BackgroundImage != null)
            {
                Bitmap bitmap = new Bitmap(BackgroundImage,this.Width, this.Height);
                SetBitmap(bitmap, 255);
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

        private void Initialize()
        {
            if (Main == null)
                return;
            this.SuspendLayout();
            // 置顶窗口
            TopMost = Main.TopMost;
            Main.BringToFront();
            // 是否在任务栏显示
            ShowInTaskbar = false;
            // 设置大小
            Width = Main.Width;
            Height = Main.Height;
            // 设置背景
            //Bitmap bitmaps = new Bitmap(Main.BackgroundImage, Size);
            BackgroundImage = Main.BackgroundImage;
            //
            Main.Owner = this;

            // 设置图层显示的位置
            this.Location = Main.Location;

            // 绘图层状态改变
            Main.LocationChanged += new EventHandler(Main_LocationChanged);
            Main.SizeChanged += new EventHandler(Main_SizeChanged);
            Main.VisibleChanged += new EventHandler(Main_VisibleChanged);
            this.LocationChanged += new EventHandler(Main_LocationChanged);
            // 
            Main.MouseDown += new MouseEventHandler(MForm_MouseDown);
            Main.MouseMove += new MouseEventHandler(MForm_MouseMove);
            Main.MouseUp += new MouseEventHandler(MForm_MouseUp);

            this.MouseDown += new MouseEventHandler(MForm_MouseDown);
            this.MouseMove += new MouseEventHandler(MForm_MouseMove);
            this.MouseUp += new MouseEventHandler(MForm_MouseUp);

            this.Load += new System.EventHandler(this.MForm_Load);
            this.ResumeLayout();
        }

        private System.Drawing.Point mouseOffset; // 记录鼠标指针的坐标
        private bool isMouseDown = false; // 记录鼠标按键是否按下
        // 鼠标抬起
        private void MForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }

        // 鼠标抬起
        private void MForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                System.Drawing.Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                var form = sender as Form;
                if (form == this)
                {
                    Location = mousePos;
                }
                else
                {
                    Main.Location = mousePos;
                }
            }
        }

        // 鼠标按下
        private void MForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOffset = new System.Drawing.Point(-e.X, -e.Y);
            isMouseDown = true;
        }

        // 主窗体显示或隐藏
        private void Main_VisibleChanged(object sender, EventArgs e)
        {
            this.Visible = Main.Visible;
        }

        // 主窗体大小改变时
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            Width = Main.Width;
            Height = Main.Height;
            SetBits();
        }

        // 移动主窗体
        private void Main_LocationChanged(object sender, EventArgs e)
        {
            Form form = sender as Form;
            if (form == this)
            {
                Main.Location = Location;
                
            }
            else
            {
                Location = Main.Location;
            }
        }

        private void MForm_Load(object sender, EventArgs e)
        {
            SetBits();
        }

    }
}
