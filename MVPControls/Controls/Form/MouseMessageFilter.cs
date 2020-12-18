using MVPControls.Interop;
using System.Windows.Forms;

namespace MVPControls
{
    /// <summary>
    /// 鼠标移动消息处理
    /// </summary>
    public class MouseMessageFilter : IMessageFilter
    {
        public static event MouseEventHandler MouseMove;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == Win32.WM_MOUSEMOVE && MouseMove != null)
            {
                int x = Control.MousePosition.X, y = Control.MousePosition.Y;
                MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
            }

            return false;
        }
    }
}
