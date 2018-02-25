using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace lxzh
{
    class RegistryUtil
    {
        

        public static void setStart(bool state)
        {
            //
            //state=1,开机启动
            //state=0,禁止开机启动
            //
            //获取程序执行路径...
            string starupPath = Path.GetFullPath(Application.ExecutablePath);
            //class Micosoft.Win32.RegistryKey. 表示Window注册表中项级节点,此类是注册表装.
            RegistryKey local = Registry.LocalMachine;
            RegistryKey key;
            object obj;
            try
            {
                key = local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                obj = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", Util.KEY_NAME, null);
                if (state&& obj == null)
                {
                    //SetValue:存储值的名称
                    key.SetValue(Util.KEY_NAME, starupPath); 
                }
                else if (!state&& obj != null)
                {
                    //SetValue:存储值的名称
                    key.DeleteValue(Util.KEY_NAME);
                }
                local.Close();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }
        }
    }
}
