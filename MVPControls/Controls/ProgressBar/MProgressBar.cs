using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MVPControls
{
    /// <summary>
    /// 滑动条. 可以通过设置ForeColor、BackColor来改变滑动条的背景色和前景色
    /// </summary>
    public class MProgressBar : ProgressBar
    {
        public MProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = e.ClipRectangle;
            rect.Width = (int)(rect.Width * ((double)Value / Maximum));
            if (ProgressBarRenderer.IsSupported)
            {
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            }
            
            rect.Height = rect.Height;
            e.Graphics.FillRectangle(new SolidBrush(ForeColor), 0, 0, rect.Width, rect.Height);
            e.Graphics.FillRectangle(new SolidBrush(BackColor), rect.Width, 0, e.ClipRectangle.Width - rect.Width, rect.Height);
        }
    }
}
