using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MVPControls.Interop
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

        public const int MF_REMOVE = 0x1000;
        public const int SC_RESTORE = 0xF120; //还原
        public const int SC_MOVE = 0xF010; //移动
        public const int SC_SIZE = 0xF000; //大小
        public const int SC_MINIMIZE = 0xF020; //最小化
        public const int SC_MAXIMIZE = 0xF030; //最大化
        public const int SC_CLOSE = 0xF060; //关闭 

        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_COMMAND = 0x0111;

        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;

        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_PAINT = 0xF;
        public const int WM_CREATE = 0x1;
        public const int WM_NCCREATE = 0x81;
        public const int WM_NCPAINT = 0x85;
        public const int WM_PRINT = 0x317;
        public const int WM_DESTROY = 0x2;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_SHARED_MENU = 0x1E2;
        public const int HC_ACTION = 0;
        public const int WH_CALLWNDPROC = 4;
        public const int GWL_WNDPROC = -4;

        public const int WS_SYSMENU = 0x80000;
        public const int WS_SIZEBOX = 0x40000;

        public const int WS_MAXIMIZEBOX = 0x10000;

        public const int WS_MINIMIZEBOX = 0x20000;

        public const byte AC_SRC_OVER = 0;
        public const Int32 ULW_ALPHA = 2;
        public const byte AC_SRC_ALPHA = 1;

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr ExtCreateRegion(IntPtr lpXform, uint nCount, IntPtr rgnData);

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

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

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

    [StructLayout(LayoutKind.Sequential)]
    public struct Size
    {
        public Int32 cx;
        public Int32 cy;

        public Size(Int32 x, Int32 y)
        {
            cx = x;
            cy = y;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public Int32 x;
        public Int32 y;

        public Point(Int32 x, Int32 y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
