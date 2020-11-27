using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MVPControls
{
    /// <summary>
    /// 支持九宫切图的图片控件
    /// </summary>
    public class MNineImage:Control
    {
        // 九宫切线
        private int _xLine1 = 10, _xLine2 = 10;
        private int _yLine1 = 10, _yLine2 = 10;

        [Category("九宫图设置")]
        [Description("九宫从左往右，计数为1的线的X值")]
        public int XLine1
        {
            get { return _xLine1; }
            set { _xLine1 = value;Refresh(); }
        }

        [Category("九宫图设置")]
        [Description("九宫从左往右，计数为1的线的X值")]
        public int XLine2
        {
            get { return _xLine2; }
            set { _xLine2 = value; Refresh(); }
        }

        [Category("九宫图设置")]
        [Description("九宫从左往右，计数为1的线的X值")]
        public int yLine1
        {
            get { return _yLine1; }
            set { _yLine1 = value; Refresh(); }
        }

        [Category("九宫图设置")]
        [Description("九宫从左往右，计数为1的线的X值")]
        public int yLine2
        {
            get { return _yLine2; }
            set { _yLine2 = value; Refresh(); }
        }

        private Image _sourceImage;
        [Category("九宫图设置")]
        [Description("需要进行九宫操作的图片.")]
        public Image SourceImage
        {
            get
            {
                return _sourceImage;
            }
            set
            {
                _sourceImage = value;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;

            if (_sourceImage != null)
            {
                var delta_x1 = _xLine1;
                var delta_x2 = _xLine2 - _xLine1;
                var delta_x3 = _sourceImage.Size.Width - _xLine2;

                var delta_y1 = _yLine1;
                var delta_y2 = _yLine2 - _yLine1;
                var delta_y3 = _sourceImage.Size.Height - _yLine2;

                // image 相关
                var top_left_rect = new Rectangle(0, 0, delta_x1, delta_y1);
                var top_center_rect = new Rectangle(_xLine1, 0, delta_x2, delta_y1);
                var top_right_rect = new Rectangle(_xLine2, 0, delta_x3, delta_y1);

                var center_left_rect = new Rectangle(0, _yLine1, delta_x1, delta_y2);
                var center_center_rect = new Rectangle(_xLine1, _yLine1, delta_x2, delta_y2);
                var center_right_rect = new Rectangle(_xLine2, _yLine1, delta_x3, delta_y2);

                var bottom_left_rect = new Rectangle(0, _yLine2, delta_x1, delta_y3);
                var bottom_center_rect = new Rectangle(_xLine1, _yLine2, delta_x2, delta_y3);
                var bottom_right_rect = new Rectangle(_xLine2, _yLine2, delta_x3, delta_y3);

                // control 相关
                var paint_top_left_rect = top_left_rect;
                var paint_top_center_rect = new Rectangle(_xLine1, 0, ClientRectangle.Width - delta_x1 - delta_x3, delta_y1);
                var paint_top_right_rect = new Rectangle(ClientRectangle.Width - delta_x3, 0, delta_x3, delta_y1);

                var paint_center_left_rect = new Rectangle(0, _yLine1, delta_x1, ClientRectangle.Height - delta_y1 - delta_y3);
                var paint_center_center_rect = new Rectangle(_xLine1, _yLine1, ClientRectangle.Width - delta_x1 - delta_x3, ClientRectangle.Height - delta_y1 - delta_y3);
                var paint_center_right_rect = new Rectangle(ClientRectangle.Width - delta_x3, _yLine1, delta_x3, ClientRectangle.Height - delta_y1 - delta_y3);

                var paint_bottom_left_rect = new Rectangle(0, ClientRectangle.Height - delta_y3, delta_x1, delta_y3);
                var paint_bottom_center_rect = new Rectangle(_xLine1, ClientRectangle.Height - delta_y3, ClientRectangle.Width - delta_x1 - delta_x3, delta_y3);
                var paint_bottom_right_rect = new Rectangle(ClientRectangle.Width - delta_x3, ClientRectangle.Height - delta_y3, delta_x3, delta_y3);

                // 绘制Image
                g.DrawImage(_sourceImage, paint_top_left_rect, top_left_rect, GraphicsUnit.Pixel);
                g.DrawImage(_sourceImage, paint_top_center_rect, top_center_rect, GraphicsUnit.Pixel);
                g.DrawImage(_sourceImage, paint_top_right_rect, top_right_rect, GraphicsUnit.Pixel);

                g.DrawImage(_sourceImage, paint_center_left_rect, center_left_rect, GraphicsUnit.Pixel);
                g.DrawImage(_sourceImage, paint_center_center_rect, center_center_rect, GraphicsUnit.Pixel);
                g.DrawImage(_sourceImage, paint_center_right_rect, center_right_rect, GraphicsUnit.Pixel);

                g.DrawImage(_sourceImage, paint_bottom_left_rect, bottom_left_rect, GraphicsUnit.Pixel);
                g.DrawImage(_sourceImage, paint_bottom_center_rect, bottom_center_rect, GraphicsUnit.Pixel);
                g.DrawImage(_sourceImage, paint_bottom_right_rect, bottom_right_rect, GraphicsUnit.Pixel);
            }
            
        }
    }
}
