using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MVPControls
{
    public class Win32
    {
        /// <summary>
        /// The wm mousemove
        /// </summary>
        public const int WM_MOUSEMOVE = 0x0200;
        /// <summary>
        /// The wm lbuttondown
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;
        /// <summary>
        /// The wm lbuttonup
        /// </summary>
        public const int WM_LBUTTONUP = 0x0202;
        /// <summary>
        /// The wm rbuttondown
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x0204;
        /// <summary>
        /// The wm lbuttondblclk
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>
        /// The wm mouseleave
        /// </summary>
        public const int WM_MOUSELEAVE = 0x02A3;
        /// <summary>
        /// The wm nclbuttondown
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// 指定窗口发送消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// 为当前的应用程序释放鼠标捕获
        /// </summary>
        /// <returns>TRUE（非零）表示成功，零表示失败</returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// 在屏幕的任意地方显示一个弹出式菜单
        /// </summary>
        /// <param name="hmenu">弹出式菜单的句柄</param>
        /// <param name="fuFlags">位置标志</param>
        /// <param name="x">弹出式菜单在屏幕坐标系统中的X位置</param>
        /// <param name="y">弹出式菜单在屏幕坐标系统中的Y位置</param>
        /// <param name="hwnd">用于接收弹出式菜单命令的窗口的句柄。应该使用窗体的窗口句柄</param>
        /// <param name="lptpm">用屏幕坐标定义的一个矩形，如用户在这个矩形的范围内单击，则弹出式菜单不会关闭。如单击弹出式菜单之外的任何一个地方，则会关闭菜单。可以设为NULL</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        /// <summary>
        /// 取得指定窗口的系统菜单的句柄。在vb环境，“系统菜单”的正式名称为“控制菜单”，即单击窗口左上角的控制框时出现的菜单
        /// </summary>
        /// <param name="hWnd">窗口的句柄</param>
        /// <param name="bRevert">如设为TRUE，表示接收原始的系统菜单</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// 获取指定的显示器的窗口句柄
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        /// <summary>
        /// 返回一个显示器的信息
        /// </summary>
        /// <param name="hmonitor">显示器的句柄</param>
        /// <param name="info">显示器信息</param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX info);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    public class MONITORINFOEX
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
        public RECT rcMonitor = new RECT();
        public RECT rcWork = new RECT();
        public int dwFlags = 0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] szDevice = new char[32];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Width()
        {
            return right - left;
        }

        public int Height()
        {
            return bottom - top;
        }
    }
}
