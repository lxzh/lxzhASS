using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace lxzh
{
    public class FormUtil 
    {
        public class WindowsInfo
        {
            private IntPtr handle;
            /// <summary>
            /// 句柄
            /// </summary>
            public IntPtr Handle { get { return handle; } set { handle = value; } }
            private string title;
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get { return title; } set { title = value; } }
            private bool isMinimzed;
            /// <summary>
            /// 是否最小
            /// </summary>
            public bool IsMinimzed { get { return isMinimzed; } set { isMinimzed = value; } }
            private bool isMaximized;
            /// <summary>
            /// 是否最大     
            /// </summary>
            public bool IsMaximized { get { return isMaximized; } set { isMaximized = value; } }
            public WindowsInfo()
            {
                handle = IntPtr.Zero;
                title = "";
                isMinimzed = false;
                isMaximized = false;
            }
            public WindowsInfo(IntPtr p_Handle, string p_Title, bool p_IsMinimized, bool p_IsMaximized)
            {
                this.handle = p_Handle;
                this.title = p_Title;
                this.isMinimzed = p_IsMinimized;
                this.isMaximized = p_IsMaximized;
            }
        }
        private static IList<WindowsInfo> windowsList = new List<WindowsInfo>();
        private static IntPtr statusBar;
        public delegate bool EnumWindowsProc(IntPtr p_Handle, int p_Param);
        private static bool NetEnumWindows(IntPtr p_Handle, int p_Param)
        {
            if (!Win32.IsWindowVisible(p_Handle))
                return true;
            StringBuilder titleString = new StringBuilder(256);
            Win32.GetWindowText(p_Handle, titleString, 256);
            if (string.IsNullOrEmpty(titleString.ToString()))
            {
                return true;
            }
            if (titleString.Length != 0 || (titleString.Length == 0) || p_Handle != statusBar)
            {
                windowsList.Add(new WindowsInfo(p_Handle, titleString.ToString(), Win32.IsIconic(p_Handle), Win32.IsZoomed(p_Handle)));
            }
            return true;
        }
        public static IList<WindowsInfo> Load()
        {
            statusBar = Win32.FindWindow("Shell_TrayWnd", "");
            EnumWindowsProc eunmWindows = new EnumWindowsProc(NetEnumWindows);
            Win32.EnumWindows(eunmWindows, 0);
            return windowsList;
        }

        public static Rectangle getForeWinRect()
        {
            RECT re = new RECT();
            IntPtr awin = Win32.GetForegroundWindow();
            Win32.GetWindowRect(awin, ref re);
            Rectangle rect = new Rectangle(re.Left, re.Top, re.Right - re.Left, re.Bottom - re.Top);
            return rect;
        }
    }
}
