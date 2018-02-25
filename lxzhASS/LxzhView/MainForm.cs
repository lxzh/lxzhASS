using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace lxzh {
    public partial class MainForm : Form {
        private string[] extensions;
        private string[] keys;

        private bool isSettingChanged = false;

        public bool IsSettingChanged {
            get { return isSettingChanged; }
            set {
                if (!isSettingChanged && value)
                    isSettingChanged = value;
            }
        }

        private bool isSaved = false;
        public static bool isAlive = false;

        private LCheckBox[][] chkHotkeys;
        private LComboBox[] cbbHotkeys;
        private KeyModel[] hotKeys;
        private const int hotKeyCount = 7;
		
        private Point mouseOff = new Point();
        private bool leftFlag = false;

        public MainForm() {
            InitializeComponent();
            initControlArray();
            initParams();
        }

        /// <summary>
        /// 初始化控件数组
        /// </summary>
        private void initControlArray() {
            chkHotkeys = new LCheckBox[hotKeyCount][];
            cbbHotkeys = new LComboBox[hotKeyCount];
            hotKeys = new KeyModel[] { Util.HOTKEY_WHOLE, Util.HOTKEY_ACTIVE, Util.HOTKEY_FREE, Util.HOTKEY_LAST, Util.HOTKEY_CLIP, Util.HOTKEY_TXTPIN, Util.HOTKEY_EXIT };

            chkHotkeys[0] = new LCheckBox[4];
            chkHotkeys[0][0] = chkCtrl1;
            chkHotkeys[0][1] = chkShift1;
            chkHotkeys[0][2] = chkAlt1;
            chkHotkeys[0][3] = chkWin1;
            cbbHotkeys[0] = cbbKeys1;

            chkHotkeys[1] = new LCheckBox[4];
            chkHotkeys[1][0] = chkCtrl2;
            chkHotkeys[1][1] = chkShift2;
            chkHotkeys[1][2] = chkAlt2;
            chkHotkeys[1][3] = chkWin2;
            cbbHotkeys[1] = cbbKeys2;

            chkHotkeys[2] = new LCheckBox[4];
            chkHotkeys[2][0] = chkCtrl3;
            chkHotkeys[2][1] = chkShift3;
            chkHotkeys[2][2] = chkAlt3;
            chkHotkeys[2][3] = chkWin3;
            cbbHotkeys[2] = cbbKeys3;

            chkHotkeys[3] = new LCheckBox[4];
            chkHotkeys[3][0] = chkCtrl4;
            chkHotkeys[3][1] = chkShift4;
            chkHotkeys[3][2] = chkAlt4;
            chkHotkeys[3][3] = chkWin4;
            cbbHotkeys[3] = cbbKeys4;

            chkHotkeys[4] = new LCheckBox[4];
            chkHotkeys[4][0] = chkCtrl5;
            chkHotkeys[4][1] = chkShift5;
            chkHotkeys[4][2] = chkAlt5;
            chkHotkeys[4][3] = chkWin5;
            cbbHotkeys[4] = cbbKeys5;

            chkHotkeys[5] = new LCheckBox[4];
            chkHotkeys[5][0] = chkCtrl6;
            chkHotkeys[5][1] = chkShift6;
            chkHotkeys[5][2] = chkAlt6;
            chkHotkeys[5][3] = chkWin6;
            cbbHotkeys[5] = cbbKeys6;

            chkHotkeys[6] = new LCheckBox[4];
            chkHotkeys[6][0] = chkCtrl7;
            chkHotkeys[6][1] = chkShift7;
            chkHotkeys[6][2] = chkAlt7;
            chkHotkeys[6][3] = chkWin7;
            cbbHotkeys[6] = cbbKeys7;
        }

        private void initParams() {
            extensions = new string[] { "png", "jpg", "bmp" };
            keys = new string[Util.KEY_NUM];
            char startChar = 'A';
            for (int i = 0; i < Util.KEY_NUM; i++) {
                keys[i] = "" + (char)(startChar + i);
            }
            cbbExtension.Items.AddRange(extensions);
            cbbKeys1.Items.AddRange(keys);
            cbbKeys2.Items.AddRange(keys);
            cbbKeys3.Items.AddRange(keys);
            cbbKeys4.Items.AddRange(keys);
            cbbKeys5.Items.AddRange(keys);
            cbbKeys6.Items.AddRange(keys);
            cbbKeys7.Items.AddRange(keys);
            btnScan.Click += (s, e) => { ScanFolder(); };
            btnSave.Click += (s, e) => { btnSaveClick(); };
            btnCancle.Click += (s, e) => { btnCancelClick(); };
        }

        private void MainForm_Load(object sender_, EventArgs e_) {
            //增加透明和可移动
            #region 增加透明
            this.Opacity = 0.8;
            #endregion
            #region 增加移动
            this.MouseDown += (sender, e) => {
                if (e.Button == MouseButtons.Left) {
                    mouseOff = new Point(-e.X, -e.Y);
                    leftFlag = true;
                }
            };
            this.MouseMove += (sender, e) => {
                if (leftFlag) {
                    Point mouseSet = Control.MousePosition;
                    mouseSet.Offset(mouseOff.X, mouseOff.Y);  //calculate the last point
                    ((ContainerControl)sender).Location = mouseSet;
                }
            };
            this.MouseUp += (sender, e) => {
                if (leftFlag) {
                    leftFlag = false;
                }
            };
            #endregion
            this.Hide();
            IniFile.initConfigFile();
            if (!registHotKey()) {
                new Toast(2, "注册快捷键失败，可能存在快捷键冲突，\n请重新定义快捷键!").Show();
                this.WindowState = FormWindowState.Normal;
                this.Show();
                loadSetting();
            } else {
                this.BeginInvoke(new MethodInvoker(() => loadSetting()));
                //new Thread(new ThreadStart(loadSetting)).Start();
                this.Location = new Point(-500, -500);  //将窗体移除屏幕外(否则一闪而过)
                this.BeginInvoke(new MethodInvoker(() => this.Visible = false));    //因为直接this.visible = false没用
            }
        }

        private void loadSetting() {
            Control.CheckForIllegalCrossThreadCalls = false;

            #region 读取配置文件设置项
            string autorun = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.START_ON_POWER_ON, "true").ToLower();
            string path = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.SAVE_PIC_PATH, Util.DEFAULT_SAVE_PIC_PATH);
            string fileextension = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.SAVE_FILE_EXTENSION, Util.DEFAULT_FILE_EXTENSION).ToLower();
            #endregion

            #region 设置开机启动项
            if (autorun.Equals("true")) {
                chkSPO.Checked = true;
            } else if (autorun.Equals("false")) {
                chkSPO.Checked = false;
            } else {
                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.START_ON_POWER_ON, "true");
                chkSPO.Checked = true;
            }
            chkSPO.OriState = chkSPO.Checked;
            #endregion

            #region 设置截图文件保存目录
            if (Directory.Exists(path)) {
                txtSavePath.Text = path;
            } else {
                path = Application.StartupPath + "\\picture";
                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.SAVE_PIC_PATH, path);
                txtSavePath.Text = path;
            }
            txtSavePath.OriText = txtSavePath.Text;
            #endregion

            #region 设置截图文件扩展名
            if (keys.Contains(fileextension)) {
                cbbExtension.SelectedItem = fileextension;
            } else {
                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.SAVE_FILE_EXTENSION, extensions[0]);
                cbbExtension.SelectedIndex = 0;
            }
            cbbExtension.OriIndex = cbbExtension.SelectedIndex;
            #endregion

            #region 加载快捷键配置
            Util.HOTKEY_WHOLE.IsValueChanged = false;
            int[] hotKeyValue = HotKey.getHotkeyFromIni(Util.HOTKEY_WHOLE);
            chkAlt1.OriState = chkAlt1.Checked = hotKeyValue[0] == 1;
            chkCtrl1.OriState = chkCtrl1.Checked = hotKeyValue[1] == 1;
            chkShift1.OriState = chkShift1.Checked = hotKeyValue[2] == 1;
            chkWin1.OriState = chkWin1.Checked = hotKeyValue[3] == 1;
            cbbKeys1.OriIndex = cbbKeys1.SelectedIndex = hotKeyValue[4];

            Util.HOTKEY_ACTIVE.IsValueChanged = false;
            hotKeyValue = HotKey.getHotkeyFromIni(Util.HOTKEY_ACTIVE);
            chkAlt2.OriState = chkAlt2.Checked = hotKeyValue[0] == 1;
            chkCtrl2.OriState = chkCtrl2.Checked = hotKeyValue[1] == 1;
            chkShift2.OriState = chkShift2.Checked = hotKeyValue[2] == 1;
            chkWin2.OriState = chkWin2.Checked = hotKeyValue[3] == 1;
            cbbKeys2.OriIndex = cbbKeys2.SelectedIndex = hotKeyValue[4];

            Util.HOTKEY_FREE.IsValueChanged = false;
            hotKeyValue = HotKey.getHotkeyFromIni(Util.HOTKEY_FREE);
            chkAlt3.OriState = chkAlt3.Checked = hotKeyValue[0] == 1;
            chkCtrl3.OriState = chkCtrl3.Checked = hotKeyValue[1] == 1;
            chkShift3.OriState = chkShift3.Checked = hotKeyValue[2] == 1;
            chkWin3.OriState = chkWin3.Checked = hotKeyValue[3] == 1;
            cbbKeys3.OriIndex = cbbKeys3.SelectedIndex = hotKeyValue[4];

            Util.HOTKEY_LAST.IsValueChanged = false;
            hotKeyValue = HotKey.getHotkeyFromIni(Util.HOTKEY_LAST);
            chkAlt4.OriState = chkAlt4.Checked = hotKeyValue[0] == 1;
            chkCtrl4.OriState = chkCtrl4.Checked = hotKeyValue[1] == 1;
            chkShift4.OriState = chkShift4.Checked = hotKeyValue[2] == 1;
            chkWin4.OriState = chkWin4.Checked = hotKeyValue[3] == 1;
            cbbKeys4.OriIndex = cbbKeys4.SelectedIndex = hotKeyValue[4];

            Util.HOTKEY_CLIP.IsValueChanged = false;
            hotKeyValue = HotKey.getHotkeyFromIni(Util.HOTKEY_CLIP);
            chkAlt5.OriState = chkAlt5.Checked = hotKeyValue[0] == 1;
            chkCtrl5.OriState = chkCtrl5.Checked = hotKeyValue[1] == 1;
            chkShift5.OriState = chkShift5.Checked = hotKeyValue[2] == 1;
            chkWin5.OriState = chkWin5.Checked = hotKeyValue[3] == 1;
            cbbKeys5.OriIndex = cbbKeys5.SelectedIndex = hotKeyValue[4];

            Util.HOTKEY_TXTPIN.IsValueChanged = false;
            hotKeyValue = HotKey.getHotkeyFromIni(Util.HOTKEY_TXTPIN);
            chkAlt6.OriState = chkAlt6.Checked = hotKeyValue[0] == 1;
            chkCtrl6.OriState = chkCtrl6.Checked = hotKeyValue[1] == 1;
            chkShift6.OriState = chkShift6.Checked = hotKeyValue[2] == 1;
            chkWin6.OriState = chkWin6.Checked = hotKeyValue[3] == 1;
            cbbKeys6.OriIndex = cbbKeys6.SelectedIndex = hotKeyValue[4];
			
			Util.HOTKEY_EXIT.IsValueChanged = false;
            hotKeyValue = HotKey.getHotkeyFromIni(Util.HOTKEY_EXIT);
            chkAlt7.OriState = chkAlt7.Checked = hotKeyValue[0] == 1;
            chkCtrl7.OriState = chkCtrl7.Checked = hotKeyValue[1] == 1;
            chkShift7.OriState = chkShift7.Checked = hotKeyValue[2] == 1;
            chkWin7.OriState = chkWin7.Checked = hotKeyValue[3] == 1;
            cbbKeys7.OriIndex = cbbKeys7.SelectedIndex = hotKeyValue[4];
            #endregion

        }

        private bool registHotKey() {
            bool result = true;
            for (int i = 0; i < hotKeyCount; i++) {
                result = result && HotKey.RegisteHotkeyFromIni(Handle,hotKeys[i]);
            }
            return result;
        }

        private void ScanFolder() {
            fbdSelectDir.SelectedPath = txtSavePath.Text;
            if (fbdSelectDir.ShowDialog() == DialogResult.OK) {
                if (txtSavePath.Text != fbdSelectDir.SelectedPath) {
                    txtSavePath.Text = fbdSelectDir.SelectedPath;
                }
            }
        }

        private void cmsSetting_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            ToolStripMenuItem item = e.ClickedItem as ToolStripMenuItem;
            switch (item.Name) {
                case "tsmiSetting":
                    showWindow();
                    break;
                case "tsmiExit":
                    Application.Exit();
                    break;
            }
        }

        //显示窗体
        private void showWindow() {
            this.Location = new Point(
                (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
            this.Visible = true;
            this.Activate();
        }

        private void btnSaveClick() {
            if (checkChanged() && !isSaved) {
                if (!SaveSetting()) {
                    return;
                }
            }
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void btnCancelClick() {
            if (checkChanged() && !isSaved) {
                if (MessageBox.Show(this, "设置项已修改，是否保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    if (!SaveSetting()) {
                        return;
                    }
                }
            }
            this.Hide();
            this.ShowInTaskbar = false;
        }

        protected override void WndProc(ref Message m) {
            //按快捷键   
            switch (m.Msg) {
                case Win32.WM_HOTKEY:
                    int keyid = m.WParam.ToInt32();
                    if (keyid == Util.HOTKEY_WHOLE.KeyId) {
                        SaveScreen(keyid);
                    } else if (keyid == Util.HOTKEY_ACTIVE.KeyId) {
                        SaveScreen(keyid);
                    } else if (keyid == Util.HOTKEY_FREE.KeyId) {
                        StartCapture(false);
                    } else if (keyid == Util.HOTKEY_LAST.KeyId) {
                        SaveScreen(keyid);
                    } else if (keyid == Util.HOTKEY_CLIP.KeyId) {
                        StartCapture(true);
                    } else if (keyid == Util.HOTKEY_TXTPIN.KeyId) {
                        StartPinCode(Clipboard.ContainsText());
                    } else if (keyid == Util.HOTKEY_EXIT.KeyId) {
                        Close();
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        protected override void DefWndProc(ref Message m) {
            if (m.Msg == 0x104) {
                StartCapture(false);
                return; 
            } else { } 
            base.DefWndProc(ref m); 
        }

        private void SaveScreen(int hotkeyId) {
            ScreenForm sf = new ScreenForm(hotkeyId);
            sf.Show();
        }
        //启动截图
        public void StartCapture(bool fromClip) {
            CaptureForm capture = new CaptureForm();
            try {
                if (!CaptureForm.isAlive) {
                    capture.IsCaptureCursor = false;
                    capture.IsFromClipBoard = fromClip;
                    capture.Show();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        public void StartPinCode(bool fromClip) {
            PinForm pin = new PinForm();
            if (fromClip) {
                pin.CodeTxt = Clipboard.GetText();
            }
            pin.Show();
        }

        /// <summary>
        /// 保存设置项
        /// </summary>
        /// <returns></returns>
        private bool SaveSetting() {
            string path = txtSavePath.Text;
            bool result = true;
            try {
                DirectoryInfo di = new DirectoryInfo(path);
            } catch (Exception e) {
                result = false;
                Console.WriteLine(e.ToString());
            }
            if (!checkPathChar(path))
                result = false;
            if (!result) {
                new Toast(1, "截图文件保存路径不是有效的文件路径，请重新输入！").Show();
                return false;
            } else {
                if (!Directory.Exists(path)) {
                    new Toast(1, "截图文件保存路径不存在，请重新输入！").Show();
                    ScanFolder();
                }
                #region 保存配置
                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.START_ON_POWER_ON, chkSPO.Checked.ToString());
                RegistryUtil.setStart(chkSPO.Checked);

                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.SAVE_FILE_EXTENSION, cbbExtension.SelectedItem.ToString());

                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.SAVE_PIC_PATH, txtSavePath.Text);

                for (int i = 0; i < hotKeyCount; i++) {
                    if (!hotKeys[i].setHotkey(new bool[] { chkHotkeys[i][0].Checked, chkHotkeys[i][1].Checked, chkHotkeys[i][2].Checked, chkHotkeys[i][3].Checked }, cbbHotkeys[i].SelectedIndex)) {
                        cbbHotkeys[i].Select();
                        new Toast(1, "请至少选择一个辅助键(Ctrl/Alt/Shift/Win)！").Show();
                        return false;
                    }
                }

                #endregion
                if (!checkHotkeyConflit()) {
                    return false;
                }
                #region 注册新设置的快捷键
                for (int i = 0; i < hotKeyCount; i++) {
                    if (hotKeys[i].RegistedHotkey != hotKeys[i].Hotkey) {
                        IniFile.WriteIniData(Util.CONFIG_SECTION, hotKeys[i].KeyName, hotKeys[i].Hotkey);
                        result &= HotKey.RegisteSetHotkey(Program.mainForm.Handle, hotKeys[i]);
                    }
                }
             
                if (!result) {
                    new Toast(2, "保存失败，请检查是否有快捷键和其他应用程序存在冲突").Show();
                }
                #endregion
                isSaved = result;
                return isSaved;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool checkPathChar(string path) {
            if (path == null)
                return false;
            string speCh = @"\\";
            if (path.Contains(speCh))
                return false;
            //windows文件路径不能包含的特殊字符
            char[] keychs = { (char)34, (char)47, (char)60, (char)62, (char)63, (char)124 };
            for (int i = 0; i < keychs.Length; i++) {
                if (path.Contains(keychs[i]))
                    return false;
            }
            if (path.Contains(":") && path.IndexOf(":") != 1)
                return false;
            return true;
        }
        /// <summary>
        /// 设置是否变更
        /// </summary>
        /// <returns></returns>
        private bool checkChanged() {
            bool result = txtSavePath.IsValueChanged || cbbExtension.IsValueChanged || chkSPO.IsStateChanged;
            if (!result) {
                for (int i = 0; i < hotKeyCount; i++) {
                    for (int j = 0; j < 4; j++) {
                        result |= chkHotkeys[i][j].IsStateChanged;
                    }
                    result |= cbbHotkeys[i].IsValueChanged;
                }
            }
            return result;
        }
        /// <summary>
        /// 检测快捷键是否冲突
        /// </summary>
        /// <returns></returns>
        private bool checkHotkeyConflit() {
            for (int i = 0; i < 4; i++) {
                for (int j = i + 1; j < hotKeyCount; j++) {
                    if (hotKeys[j].Hotkey == hotKeys[i].Hotkey) {
                        new Toast(2, "快捷键存在冲突,请重新设置").Show();
                        object cbbKeys = Util.GetControlByName(this, "cbbKeys" + (j + 1));
                        if (cbbKeys != null) {
                            ((ComboBox)cbbKeys).Select();
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void lbl1_Click(object sender, EventArgs e)
        {
            new Toast(3, "截取当前屏幕，并将结果放置在剪贴板中").ShowDialog();
        }

        private void lbl2_Click(object sender, EventArgs e)
        {
            new Toast(3, "截取当前屏幕中的活动程序，并将结果放置在剪贴板中").ShowDialog();
        }

        private void lbl3_Click(object sender, EventArgs e)
        {
            new Toast(2, "普通意义上的截图").ShowDialog();
        }

        private void lbl4_Click(object sender, EventArgs e)
        {
            new Toast(3, "在上一次截图的位置继续截取图片并放置到剪贴板中").ShowDialog();
        }

        private void lxzhTipLabel1_Click(object sender, EventArgs e)
        {
            new Toast(3, "将剪贴板中的图像放置在屏幕顶层予以截图").ShowDialog();
        }
    }
}
