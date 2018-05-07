using System;
using System.Runtime.InteropServices;

namespace lxzh
{
    public class KeyHook
    {
        #region Win32
        public struct KeyInfoStruct
        {
            public int vkCode;        //按键键码
            public int scanCode;
            public int flags;       //键盘是否按下的标志
            public int time;
            public int dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;      //钩子类型 全局钩子
        private const int WM_KEYUP = 0x101;     //按键抬起
        private const int WM_KEYDOWN = 0x100;       //按键按下

        #endregion

        public delegate void KeyHookHanlder(object sender, KeyHookEventArgs e);
        public event KeyHookHanlder KeyHookEvent;

        private IntPtr hHook;
        private GCHandle gc;

        protected virtual void OnKeyHookEvent(KeyHookEventArgs e) {
            if (KeyHookEvent != null) this.KeyHookEvent(this, e);
        }

        private int KeyHookProcedure(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && KeyHookEvent != null) {
                KeyInfoStruct inputInfo = (KeyInfoStruct)Marshal.PtrToStructure(lParam, typeof(KeyInfoStruct));
                if (wParam == (IntPtr)WM_KEYDOWN) {//如果按键按下
                    this.OnKeyHookEvent(new KeyHookEventArgs(inputInfo.vkCode));
                }
            }
            return Win32.CallNextHookEx(hHook, nCode, wParam, lParam);//继续传递消息
        }

        public IntPtr SetHook() {
            if (hHook == IntPtr.Zero) {
                Win32.HookProc keyCallBack = new Win32.HookProc(KeyHookProcedure);
                hHook = Win32.SetWindowsHookEx(
                    WH_KEYBOARD_LL, keyCallBack,
                    Win32.GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName),
                    0);
                gc = GCHandle.Alloc(keyCallBack);
            }
            return hHook;
        }

        public IntPtr UnLoadHook() {
            if (hHook != IntPtr.Zero) {
                if (Win32.UnhookWindowsHookEx(hHook)) 
                    hHook = IntPtr.Zero;
            }
            return hHook;
        }
    }

    public class KeyHookEventArgs : EventArgs {
        private int keyCode;
        public int KeyCode {
            get { return keyCode; }
        }

        public KeyHookEventArgs(int code) {
            this.keyCode = code;
        }
    }
}
