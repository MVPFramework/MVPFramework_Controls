using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MVPControls
{
    public partial class MForm : Form
    {
        //绘制层
        public MTransparentForm transparentForm = null;
        private Image transparentBackground = null;
        [Category("MVPControls"), Description("透明窗口背景")]
        public Image TransparentBackground
        {
            get { return transparentBackground; }
            set
            {
                if (value != null && transparentBackground != value)
                {
                    transparentBackground = value;
                    if (transparentForm != null)
                    {
                        transparentForm.BackgroundImage = transparentBackground;
                        transparentForm.SetBits();
                    }

                    // DesignMode 下特殊处理, 方便设计
                    if (DesignMode)
                    {
                        BackgroundImage = value;
                        Size = BackgroundImage.Size;
                        ClientSize = BackgroundImage.Size;
                        Refresh();
                    }
                }
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (!DesignMode && value == Color.Transparent)
                {
                    base.BackColor = Color.LimeGreen;
                }

                base.BackColor = value;
            }
        }

        public MForm()
        {
            InitializeComponent();
            SetStyles();
            Init();
        }

        private void SetStyles()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);
            base.UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }

        //[Browsable(false)]
        //public override Color BackColor { get; set; }
        [Browsable(false)]
        public override Image BackgroundImage { get; set; }

        private void Init()
        {
            base.BackgroundImageLayout = ImageLayout.None;
            this.BackgroundImage = null;
            this.TransparencyKey = Color.LimeGreen;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                this.BackColor = Color.LimeGreen;
                BackgroundImage = null;
                transparentForm = new MTransparentForm(this);
                transparentForm.BackgroundImage = transparentBackground;
                transparentForm.SetBits();
                transparentForm.Show();
            }
            else
            {
                this.BackColor = Color.Transparent;
            }
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Owner.Close();
            base.OnClosing(e);
        }
    }
}
