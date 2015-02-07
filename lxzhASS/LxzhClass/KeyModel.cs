using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lxzh {
    public class KeyModel {
        private string keyName;
        /// <summary>
        /// 快捷键名
        /// </summary>
        public string KeyName {
            get { return keyName; }
            set { keyName = value; }
        }
        private string description;
        /// <summary>
        /// 快捷键描述
        /// </summary>
        public string Description {
            get { return description; }
            set { description = value; }
        }

        private string hotkey;
        /// <summary>
        /// 快捷键字符串
        /// </summary>
        public string Hotkey {
            get { return hotkey; }
            set { hotkey = value; }
        }
        private string preHotkey;
        /// <summary>
        /// 修改之前的快捷键字符串
        /// </summary>
        public string PreHotkey {
            get { return preHotkey; }
            set { preHotkey = value; }
        }
        private string registedHotkey;
        /// <summary>
        /// 已经注册的快捷键字符串
        /// </summary>
        public string RegistedHotkey {
            get { return registedHotkey; }
            set { registedHotkey = value; }
        }

        private string defaultHotkey;
        /// <summary>
        /// 默认快捷键
        /// </summary>
        public string DefaultHotkey {
            get { return defaultHotkey; }
            set { defaultHotkey = value; }
        }
        private string noText;
        /// <summary>
        /// 快捷键字符
        /// </summary>
        public string NoText {
            get { return noText; }
            set { noText = value; }
        }
        private int keyId;
        /// <summary>
        /// 快捷键Id
        /// </summary>
        public int KeyId {
            get { return keyId; }
        }
        private int[] hotKeyValue;
        
        private bool isValueChanged;
        /// <summary>
        /// 是否修改了快捷键
        /// </summary>
        public bool IsValueChanged {
            get { return isValueChanged; }
            set { isValueChanged = value; }
        }

        public KeyModel(string keyName,string description, string defaultHotkey, string noText, int keyId) {
            if (string.IsNullOrEmpty(defaultHotkey)||defaultHotkey.Length < 5) {
                throw new Exception("default hotkey error!");
            }
            this.keyName = keyName; this.description = description; this.defaultHotkey = defaultHotkey;
            this.noText = defaultHotkey[defaultHotkey.Length-1]+""; this.keyId = keyId;
        }

        public int[] getHotkey() {
            if (hotKeyValue == null || hotKeyValue.Length != 5) {
                hotKeyValue = new int[5];
                string hotkey = IniFile.ReadIniData(Util.CONFIG_SECTION, keyName, noText).ToLower().Trim().Replace(" ", "");
                if (hotkey == "")
                    hotkey = Util.DEFAULT_HOTKEY_WHOLE.ToLower();
                hotKeyValue[0] = hotkey.Contains("alt") ? 1 : 0;
                hotKeyValue[1] = hotkey.Contains("ctrl") ? 1 : 0;
                hotKeyValue[2] = hotkey.Contains("shift") ? 1 : 0;
                hotKeyValue[3] = hotkey.Contains("win") ? 1 : 0;

                hotkey = hotkey.Replace("ctrl", "").Replace("shift", "").Replace("alt", "").Replace("win", "").Replace("+", "");
                if (!(hotkey.Length == 1 && hotkey[0] >= 'a' && hotkey[0] <= 'z')) {
                    hotkey = noText.ToLower();
                }
                hotKeyValue[4] = (int)(hotkey[0] - 'a');
            }
            return hotKeyValue;
        }
        /// <summary>
        /// 设置快捷键字符串
        /// </summary>
        /// <param name="isChecked"></param>
        /// <param name="keyindex"></param>
        /// <returns>是否包含辅助键</returns>
        public bool setHotkey(bool[] isChecked,int keyindex) {
            hotkey = "";
            if (isChecked[1])
                hotkey += "Ctrl+";
            if (isChecked[0])
                hotkey += "Alt+";
            if (isChecked[2])
                hotkey += "Shift+";
            if (isChecked[3])
                hotkey += "Win+";
            hotkey += (char)('A' + keyindex);
            if (!isValueChanged && preHotkey != hotkey) {
                isValueChanged = true;
            }
            return hotkey.Length>1;
        }
        public string setHotkey(int[] hotKeyValue) {
            hotkey = "";
            if (hotKeyValue[1] == 1)
                hotkey += "Ctrl+";
            if (hotKeyValue[0] == 1)
                hotkey += "Alt+";
            if (hotKeyValue[2] == 1)
                hotkey += "Shift+";
            if (hotKeyValue[3] == 1)
                hotkey += "Win+";
            hotkey += (char)('A' + hotKeyValue[4]);
            preHotkey = hotkey;
            return hotkey;
        }
    }
}
