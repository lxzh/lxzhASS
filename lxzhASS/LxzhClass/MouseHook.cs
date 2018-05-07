using System;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace lxzh
{
    public class MouseHook
    {
        #region Win32

        private const int WH_MOUSE_LL = 14;//全局鼠标Hook 7是局部的 13全局键盘 2局部键盘
        private const uint WM_LBUTTONDOWN = 0x201;
        private const uint WM_LBUTTONUP = 0x202;
        private const uint WM_RBUTTONDOWN = 0x204;
        private const uint WM_RBUTTONUP = 0x205;

        public struct POINT
        {
            public int X;
            public int Y;
        }
        //鼠标结构信息
        public struct MSLLHOOTSTRUCT
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        #endregion

        public delegate void MHookEventHandler(object sender, MHookEventArgs e);
        public event MHookEventHandler MHookEvent;

        private IntPtr hHook;
        public IntPtr HHook {
            get { return hHook; }
        }

        GCHandle gc;
        //Hook回调函数
        private int MouseHookProcedure(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && MHookEvent != null) {
                MSLLHOOTSTRUCT stMSLL = (MSLLHOOTSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOTSTRUCT));
                ButtonStatus btnStatus = ButtonStatus.None;
                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                    btnStatus = ButtonStatus.LeftDown;
                else if (wParam == (IntPtr)WM_LBUTTONUP)
                    btnStatus = ButtonStatus.LeftUp;
                else if (wParam == (IntPtr)WM_RBUTTONDOWN)
                    btnStatus = ButtonStatus.RightDown;
                else if (wParam == (IntPtr)WM_RBUTTONUP)
                    btnStatus = ButtonStatus.RightUp;
                MHookEvent(this, new MHookEventArgs(btnStatus, stMSLL.pt.X, stMSLL.pt.Y));
            }
            return Win32.CallNextHookEx(hHook, nCode, wParam, lParam);
        }
        //设置Hook
        public bool SetHook() {
            if (hHook != IntPtr.Zero)
                return false;
            Win32.HookProc mouseCallBack = new Win32.HookProc(MouseHookProcedure);
            hHook = Win32.SetWindowsHookEx(WH_MOUSE_LL, mouseCallBack,
                Win32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
            if (hHook != IntPtr.Zero) {
                gc = GCHandle.Alloc(mouseCallBack);
                return true;
            }
            return false;
        }
        //卸载Hook
        public bool UnLoadHook() {
            if (hHook == IntPtr.Zero)
                return false;
            if (Win32.UnhookWindowsHookEx(hHook)) {
                hHook = IntPtr.Zero;
                gc.Free();
                return true;
            }
            return false;
        }
    }
    //鼠标状态枚举值
    public enum ButtonStatus { LeftDown, LeftUp, RightDown, RightUp, None }
    //事件参数
    public class MHookEventArgs : EventArgs
    {
        private ButtonStatus mButton;
        public ButtonStatus MButton {
            get { return mButton; }
        }

        private int x;
        public int X {
            get { return x; }
        }

        private int y;
        public int Y {
            get { return y; }
        }

        public MHookEventArgs(ButtonStatus btn, int cx, int cy) {
            this.mButton = btn;
            this.x = cx;
            this.y = cy;
        }
    }
}
