using System;
using System.Text;

using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace lxzh {
    public class Win32 {
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_LBUTTONDBLCLK = 0x0203;

        public const int WM_MOUSELEAVE = 0x02A3;

        public const int WM_PAINT = 0x000F;
        public const int WM_ERASEBKGND = 0x0014;

        public const int WM_PRINT = 0x0317;

        //const int EN_HSCROLL       =   0x0601;
        //const int EN_VSCROLL       =   0x0602;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;

        public const int EM_GETSEL = 0x00B0;
        public const int EM_LINEINDEX = 0x00BB;
        public const int EM_LINEFROMCHAR = 0x00C9;

        public const int EM_POSFROMCHAR = 0x00D6;

        public const int RDW_INVALIDATE = 0x1;
        public const int RDW_INTERNALPAINT = 0x2;
        public const int RDW_NOERASE = 0x20;

        public const int WM_PRINTCLIENT = 0x0318;

        public const long PRF_CHECKVISIBLE = 0x00000001L;
        public const long PRF_NONCLIENT = 0x00000002L;
        public const long PRF_CLIENT = 0x00000004L;
        public const long PRF_ERASEBKGND = 0x00000008L;
        public const long PRF_CHILDREN = 0x00000010L;
        public const long PRF_OWNED = 0x00000020L;

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]//获取桌面的句柄
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]//在桌面找寻子窗体
        public static extern IntPtr ChildWindowFromPointEx(IntPtr pHwnd, LPPOINT pt, uint uFlgs);
        public const int CWP_SKIPDISABLED = 0x2;   //忽略不可用窗体
        public const int CWP_SKIPINVISIBL = 0x1;   //忽略隐藏的窗体
        public const int CWP_All = 0x0;            //一个都不忽略

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        // Put this declaration in your class   //IntPtr
        [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("USER32.DLL", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]//进行坐标转换 （再窗体内部进行查找）
        public static extern bool ScreenToClient(IntPtr hWnd, out LPPOINT lpPoint);

        public struct LPPOINT {
            public int X;
            public int Y;
        }



        [DllImport("user32.dll")]//获得句柄对象的位置
        public static extern bool GetWindowRect(IntPtr hWnd, out LPRECT lpRect);

        public struct LPRECT {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out PCURSORINFO pci);

        public struct PCURSORINFO {
            public int cbSize;
            public int flag;
            public IntPtr hCursor;
            public LPPOINT ptScreenPos;
        }

        [DllImport("user32.dll")]//注册全局热键
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        /// <summary> 
        /// 注册快捷键
        /// </summary>
        /// <param name="hWnd">要定义热键的窗口的句柄</param>
        /// <param name="id">定义热键ID（不能与其它ID重复）</param>
        /// <param name="fsModifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效</param>
        /// <param name="vk">定义热键的内容</param>
        /// <returns>执行失败，返回0,否则不为0。要得到扩展错误信息，调用GetLastError。</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        /// <summary>
        /// 取消注册快捷键 卸载全局热键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, ref LPRECT lpRect, bool bErase);

        [DllImport("user32.dll", EntryPoint = "GetDCEx", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool RedrawWindow(IntPtr hwnd, ref LPRECT rcUpdate, IntPtr hrgnUpdate, int flags);

        [DllImport("user32.dll")]
        public static extern int EnumWindows(FormUtil.EnumWindowsProc ewp, int lParam);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //获取当前窗口句柄:GetForegroundWindow()
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        //获取窗口大小及位置
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]   //设置钩子  第二个参数为回调函数指针
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hmod, int dwThreadid);
        [DllImport("user32.dll")]   //传递到下一个钩子
        public static extern int CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]   //卸载钩子
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("kernel32.dll")] //获取模块句柄  
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32.DLL", EntryPoint = "GetCaretBlinkTime")]
        public static extern uint GetCaretBlinkTime();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        public static bool CaptureWindow(System.Windows.Forms.Control control,
                        ref System.Drawing.Bitmap bitmap) {
            //This function captures the contents of a window or control
            Graphics g = Graphics.FromImage(bitmap);

            //PRF_CHILDREN // PRF_NONCLIENT
            int meint = (int)(PRF_CLIENT | PRF_ERASEBKGND); //| PRF_OWNED ); //  );
            IntPtr meptr = new System.IntPtr(meint);

            IntPtr hdc = g.GetHdc();
            Win32.SendMessage(control.Handle, Win32.WM_PRINT, hdc, meptr);

            g.ReleaseHdc(hdc);
            g.Dispose();
            return true;
        }
    }
}
