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

namespace lxzh
{
    public partial class SettingForm : Form
    {
        private string[] extensions;
        private string[] keys;
        
        private bool isSettingChanged = false;
        private bool isSaved = false;

        public bool IsSettingChanged
        {
            get { return isSettingChanged; }
            set 
            {
                if(!isSettingChanged&&value)
                    isSettingChanged = value; 
            }
        }

        private LCheckBox[][] chkHotkeys;
        private LComboBox[] cbbHotkeys;

        public SettingForm()
        {
            InitializeComponent();
            initControlArray();
            initParams();
        }

        private void initControlArray() {
            chkHotkeys=new LCheckBox[4][];
            cbbHotkeys=new LComboBox[4];

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
        }

        private void initParams()
        {
            extensions=new string[]{"png","jpg","bmp"};
            keys = new string[Util.KEY_NUM];
            char startChar = 'A';
            for (int i = 0; i < Util.KEY_NUM; i++)
            {
                keys[i] = "" + (char)(startChar + i);
            }
            cbbExtension.Items.AddRange(extensions);
            cbbKeys1.Items.AddRange(keys);
            cbbKeys2.Items.AddRange(keys);
            cbbKeys3.Items.AddRange(keys);
            cbbKeys4.Items.AddRange(keys);
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            #region 读取配置文件设置项
            string autorun = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.START_ON_POWER_ON, "true").ToLower();
            string path = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.SAVE_PIC_PATH, Util.DEFAULT_SAVE_PIC_PATH);
            string fileextension = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.SAVE_FILE_EXTENSION, Util.DEFAULT_FILE_EXTENSION).ToLower();
            #endregion 

            #region 设置开机启动项
            if (autorun.Equals("true"))
            {
                chkSPO.Checked = true;
            }
            else if (autorun.Equals("false"))
            {
                chkSPO.Checked = false;
            }
            else
            {
                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.START_ON_POWER_ON, "true");
                chkSPO.Checked = true;
            }
            chkSPO.OriState = chkSPO.Checked;
            #endregion 

            //设置截图文件保存目录
            if (Directory.Exists(path))
            {
                txtSavePath.Text = path;
            }
            else
            {
                path = Application.StartupPath + "\\picture";
                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.SAVE_PIC_PATH, path);
                txtSavePath.Text = path;
            }
            txtSavePath.OriText = txtSavePath.Text;
            #region 设置截图文件扩展名
            if (keys.Contains(fileextension))
            {
                cbbExtension.SelectedItem = fileextension;
            }
            else
            {
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
            #endregion 
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            ScanFolder();
        }

        private void ScanFolder()
        {
            fbdSelectDir.SelectedPath = txtSavePath.Text;
            if (fbdSelectDir.ShowDialog() == DialogResult.OK)
            {
                if (txtSavePath.Text != fbdSelectDir.SelectedPath)
                {
                    txtSavePath.Text = fbdSelectDir.SelectedPath;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkChanged() && !isSaved)
            {
                if (!SaveSetting())
                {
                    return;
                }
            }
            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            if (checkChanged() && !isSaved)
            {
                if (MessageBox.Show(this, "设置项已修改，是否保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!SaveSetting())
                    {
                        return;
                    }
                }
            }
            this.Close();
        }

        private bool SaveSetting()
        {
            string path=txtSavePath.Text;
            bool result = true;
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
            }
            catch (Exception e)
            {
                result = false;
                Console.WriteLine(e.ToString());
            }
            if (!checkPathChar(path))
                result = false;
            if (!result)
            {
                new Toast(1, "截图文件保存路径不是有效的文件路径，请重新输入！").Show();
                return false;
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    new Toast(1, "截图文件保存路径不存在，请重新输入！").Show();
                    ScanFolder();
                }

                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.START_ON_POWER_ON, chkSPO.Checked.ToString());
                RegistryUtil.setStart(chkSPO.Checked);

                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.SAVE_FILE_EXTENSION, cbbExtension.SelectedItem.ToString());

                IniFile.WriteIniData(Util.CONFIG_SECTION, Util.SAVE_PIC_PATH, txtSavePath.Text);

                if(!Util.HOTKEY_WHOLE.setHotkey(new bool[] { chkAlt1.Checked, chkCtrl1.Checked, chkShift1.Checked, chkWin1.Checked }, cbbKeys1.SelectedIndex)){
                    cbbKeys1.Select();
                    new Toast(1, "请至少选择一个辅助键(Ctrl/Alt/Shift/Win)！").Show();
                    return false;
                }
                if(!Util.HOTKEY_ACTIVE.setHotkey(new bool[] { chkAlt2.Checked, chkCtrl2.Checked, chkShift2.Checked, chkWin2.Checked }, cbbKeys2.SelectedIndex)){
                    cbbKeys2.Select();
                    new Toast(1, "请至少选择一个辅助键(Ctrl/Alt/Shift/Win)！").Show();
                    return false;
                }
                if(!Util.HOTKEY_FREE.setHotkey(new bool[] { chkAlt3.Checked, chkCtrl3.Checked, chkShift3.Checked, chkWin3.Checked }, cbbKeys3.SelectedIndex)){
                    cbbKeys3.Select();
                    new Toast(1, "请至少选择一个辅助键(Ctrl/Alt/Shift/Win)！").Show();
                    return false;
                }
                if (!Util.HOTKEY_LAST.setHotkey(new bool[] { chkAlt4.Checked, chkCtrl4.Checked, chkShift4.Checked, chkWin4.Checked }, cbbKeys4.SelectedIndex)) {
                    cbbKeys4.Select();
                    new Toast(1, "请至少选择一个辅助键(Ctrl/Alt/Shift/Win)！").Show();
                    return false;
                }

                if (!checkHotkeyConflit()) {
                    return false;
                }

                if (Util.HOTKEY_WHOLE.RegistedHotkey != Util.HOTKEY_WHOLE.Hotkey) {
                    IniFile.WriteIniData(Util.CONFIG_SECTION, Util.HOTKEY_WHOLE.KeyName, Util.HOTKEY_WHOLE.Hotkey);
                    result &= HotKey.RegisteSetHotkey(Program.mainForm.Handle, Util.HOTKEY_WHOLE);
                }
                if (Util.HOTKEY_ACTIVE.RegistedHotkey != Util.HOTKEY_ACTIVE.Hotkey) {
                    IniFile.WriteIniData(Util.CONFIG_SECTION, Util.HOTKEY_ACTIVE.KeyName, Util.HOTKEY_ACTIVE.Hotkey);
                    result &= HotKey.RegisteSetHotkey(Program.mainForm.Handle, Util.HOTKEY_ACTIVE);
                }
                if (Util.HOTKEY_FREE.RegistedHotkey != Util.HOTKEY_FREE.Hotkey) {
                    IniFile.WriteIniData(Util.CONFIG_SECTION, Util.HOTKEY_FREE.KeyName, Util.HOTKEY_FREE.Hotkey);
                    result &= HotKey.RegisteSetHotkey(Program.mainForm.Handle, Util.HOTKEY_FREE);
                }
                if (Util.HOTKEY_LAST.RegistedHotkey != Util.HOTKEY_LAST.Hotkey) {
                    IniFile.WriteIniData(Util.CONFIG_SECTION, Util.HOTKEY_LAST.KeyName, Util.HOTKEY_LAST.Hotkey);
                    result &= HotKey.RegisteSetHotkey(Program.mainForm.Handle, Util.HOTKEY_LAST);
                }
                
                if (!result) {
                    new Toast(2, "保存失败，请检查是否有快捷键和其他应用程序存在冲突").Show();
                }
                isSaved = result;
                return isSaved;
            }
        }

        private bool checkPathChar(string path)
        {
            if(path==null)
                return false;
            string speCh = @"\\";
            if (path.Contains(speCh))
                return false;
            char[] keychs = { (char)34, (char)47, (char)60, (char)62, (char)63, (char)124 };
            for (int i = 0; i < keychs.Length; i++)
            {
                if (path.Contains(keychs[i]))
                    return false;
            }
            if (path.Contains(":") && path.IndexOf(":") != 1)
                return false;
            return true;
        }

        private bool checkChanged() {
            bool result = txtSavePath.IsValueChanged || cbbExtension.IsValueChanged || chkSPO.IsStateChanged;
            if (!result) {
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 4; j++) {
                        result |= chkHotkeys[i][j].IsStateChanged;
                    }
                    result |= cbbHotkeys[i].IsValueChanged;
                }
            }
            return result;
        }

        private bool checkHotkeyConflit() {
            string[] hotkeys = new string[] { Util.HOTKEY_WHOLE.Hotkey, Util.HOTKEY_ACTIVE.Hotkey, Util.HOTKEY_FREE.Hotkey, Util.HOTKEY_LAST.Hotkey };
            for (int i = 0; i < 3; i++) {
                for (int j = i + 1; j < 4; j++) {
                    if (hotkeys[j] == hotkeys[i]) {
                        new Toast(2, "快捷键存在冲突,请重新设置").Show();
                        object cbbKeys = Util.GetControlByName(this,"cbbKeys"+(j+1));
                        if (cbbKeys != null) {
                            ((ComboBox)cbbKeys).Select();
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void chkItem_CheckedChanged(object sender, EventArgs e) {

        }

        private void txtSavePath_TextChanged(object sender, EventArgs e) {

        }
    }
}
