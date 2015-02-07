using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace lxzh
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
            //Thread.Sleep(1500);
            this.Hide();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            IniFile.initConfigFile();
            
            //bool result=HotKey.RegisterHotKey(Handle, Util.HOTKEY_ID, HotKey.KeyModifiers.WindowsKey, Keys.S);
            //注册热键，默认为Win+S，Id号为100。  
            bool result1 = HotKey.RegisteHotkeyFromIni(Handle, Util.HOTKEY_WHOLE);
            bool result2 = HotKey.RegisteHotkeyFromIni(Handle, Util.HOTKEY_ACTIVE);
            bool result3 = HotKey.RegisteHotkeyFromIni(Handle, Util.HOTKEY_FREE);
            bool result4 = HotKey.RegisteHotkeyFromIni(Handle, Util.HOTKEY_LAST);
            if (!(result1 && result2 && result3 && result4)) {
                new Toast(1, "注册快捷键失败，可能存在快捷键冲突，请重新定义快捷键!").Show();
                new SettingForm().ShowDialog();
            }
            ////注册热键Ctrl+B，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。  
            //HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.Ctrl, Keys.B);
            ////注册热键Ctrl+Alt+D，Id号为102。HotKey.KeyModifiers.Alt也可以直接使用数字1来表示。  
            //HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.Alt | HotKey.KeyModifiers.Ctrl, Keys.D);
            ////注册热键F5，Id号为103。  
            //HotKey.RegisterHotKey(Handle, 103, HotKey.KeyModifiers.None, Keys.F5);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键   
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    int keyid = m.WParam.ToInt32();
                    if (keyid == Util.HOTKEY_WHOLE.KeyId) {
                        SaveScreen(keyid);
                    } else if (keyid == Util.HOTKEY_ACTIVE.KeyId) {
                        SaveScreen(keyid);
                    } else if (keyid == Util.HOTKEY_FREE.KeyId) {
                        StartCapture(false);
                    } else if (keyid == Util.HOTKEY_LAST.KeyId) {
                        SaveScreen(keyid);
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void SaveScreen(int hotkeyId)
        {
            new ScreenForm(hotkeyId).Show();
        }
        //启动截图
        private void StartCapture(bool bFromClip) {
            CaptureForm m_frmCapture;
            //if (m_frmCapture == null || m_frmCapture.IsDisposed)
                m_frmCapture = new CaptureForm();
            m_frmCapture.IsCaptureCursor = false;
            m_frmCapture.IsFromClipBoard = bFromClip;
            m_frmCapture.Show();
        }
        private void cmsSetting_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem item = e.ClickedItem as ToolStripMenuItem;
            switch (item.Name)
            {
                case "tsmiSetting":
                    new SettingForm().Show();
                    break;
                case "tsmiExit":
                    Application.Exit();
                    break;
            }
        }
    }
}
