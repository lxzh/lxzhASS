using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace lxzh
{
    public class IniFile
    {
        public static string path;             //INI文件名  

        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        //声明读写INI文件的API函数  
        public static void initPath(string INIPath)  
        {  
            path = INIPath;  
        }

        public static void initConfigFile()
        {
            if (!File.Exists(IniFile.path))
            {
                string configStr=global::lxzh.Properties.Resources.config;
                try
                {
                    FileStream fs = new FileStream(IniFile.path, FileMode.CreateNew);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(configStr);
                    sw.Close();
                    fs.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        #endregion

        #region 读Ini文件

        public static string ReadIniData(string Section, string Key, string defaultText)
        {
            if (File.Exists(path))
            {
                StringBuilder strBuild = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, defaultText, strBuild, 1024, path);
                return strBuild.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region 写Ini文件

        public static bool WriteIniData(string Section, string Key, string Value)
        {
            if (File.Exists(path))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, path);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
