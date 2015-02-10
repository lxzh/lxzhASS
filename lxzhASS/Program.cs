using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace lxzh
{
    static class Program
    {
        public static MainForm mainForm;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            IniFile.initPath(Application.StartupPath+"\\"+global::lxzh.Properties.Resources.ConfigPath);
            Util.DEFAULT_SAVE_PIC_PATH = Application.StartupPath + "\\picture";
            Boolean createdNew; //返回是否赋予了使用线程的互斥体初始所属权
            System.Threading.Mutex instance = new System.Threading.Mutex(true, "lxzhASS", out createdNew); //同步基元变量
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (createdNew) {
                mainForm = new MainForm();
                Application.Run(mainForm);
                instance.ReleaseMutex();
            } else {
                Application.Run(new CaptureForm());
            }
        }
    }
}
