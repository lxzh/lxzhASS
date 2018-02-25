using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace lxzh
{
    public class HotKey
    {
        public static bool RegisteSetHotkey(IntPtr hWnd,KeyModel keyModel)
        {
            int[] hotKeyValue = keyModel.getHotkey();
            KeyModifiers keyModifier = KeyModifiers.None;
            for(int i=0;i<4;i++)
            {
                if(hotKeyValue[i] == 1)
                {
                    KeyModifiers tmp = (KeyModifiers)(Math.Pow(2, i));
                    if (keyModifier == KeyModifiers.None)
                        keyModifier = tmp;
                    else
                        keyModifier = keyModifier|tmp;
                }
            }
            Keys keys = (Keys)(Keys.A + hotKeyValue[4]);
            Win32.UnregisterHotKey(hWnd, keyModel.KeyId);
            bool result = Win32.RegisterHotKey(hWnd, keyModel.KeyId, keyModifier, keys);
            if (result)
            {
                keyModel.RegistedHotkey = keyModel.Hotkey;
            }
            return result;
        }

        public static bool RegisteHotkeyFromIni(IntPtr hWnd, KeyModel keyModel) {
            int[] hotKeyValue = getHotkeyFromIni(keyModel);
            KeyModifiers keyModifier = KeyModifiers.None;
            for (int i = 0; i < 4; i++) {
                if (hotKeyValue[i] == 1) {
                    KeyModifiers tmp = (KeyModifiers)(Math.Pow(2, i));
                    if (keyModifier == KeyModifiers.None)
                        keyModifier = tmp;
                    else
                        keyModifier = keyModifier | tmp;
                }
            }
            Keys keys = (Keys)(Keys.A + hotKeyValue[4]);
            Win32.UnregisterHotKey(hWnd, keyModel.KeyId);
            bool result=Win32.RegisterHotKey(hWnd, keyModel.KeyId, keyModifier, keys);
            if (result) {
                keyModel.RegistedHotkey = keyModel.Hotkey;
            }
            return result;
        }

        public static int[] getHotkeyFromIni(KeyModel keyModel) {
            int[] hotKeyValue = new int[5];
            string hotkey = IniFile.ReadIniData(Util.CONFIG_SECTION, keyModel.KeyName, keyModel.DefaultHotkey).ToLower().Trim().Replace(" ", "");
            if (hotkey == "")
                hotkey = keyModel.DefaultHotkey.ToLower();
            int assistKeyCount = -1;
            while (assistKeyCount < 1) {
                if (assistKeyCount == -1) {
                    assistKeyCount = 0;
                } else {
                    hotkey = keyModel.DefaultHotkey;
                }
                assistKeyCount += hotKeyValue[0] = hotkey.Contains("alt") ? 1 : 0;
                assistKeyCount += hotKeyValue[1] = hotkey.Contains("ctrl") ? 1 : 0;
                assistKeyCount += hotKeyValue[2] = hotkey.Contains("shift") ? 1 : 0;
                assistKeyCount += hotKeyValue[3] = hotkey.Contains("win") ? 1 : 0;
                if (assistKeyCount == 0) {
                    MessageBox.Show("配置文件不存在或者被手动修改，恢复默认快捷键", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    keyModel.IsValueChanged = true;
                    keyModel.PreHotkey = keyModel.DefaultHotkey;
                }
            }
            hotkey = hotkey.Replace("ctrl", "").Replace("shift", "").Replace("alt", "").Replace("win", "").Replace("+", "");
            if (!(hotkey.Length == 1 && hotkey[0] >= 'a' && hotkey[0] <= 'z')) {
                hotkey = keyModel.NoText.ToLower();
            }
            hotKeyValue[4] = (int)(hotkey[0] - 'a');
            keyModel.setHotkey(hotKeyValue);
            return hotKeyValue;
        }
    }  
}
