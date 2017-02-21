using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;

namespace lxzh
{
    public class Util
    {
        /// <summary>
        /// 截图文件配置分区
        /// </summary>
        public static string CONFIG_SECTION = "config";
        /// <summary>
        /// 窗体信息分区
        /// </summary>
        public static string FORM_SECTION = "form";

        /// <summary>
        /// 设置-开机自启字段
        /// </summary>
        public static string START_ON_POWER_ON = "autorun";
        
        public static KeyModel HOTKEY_WHOLE = new KeyModel("hotkey_whole", "全屏截图", "Ctrl+G", "G", 100);

        public static KeyModel HOTKEY_ACTIVE = new KeyModel("hotkey_active", "活动窗体截图", "Ctrl+D", "D", 101);

        public static KeyModel HOTKEY_FREE = new KeyModel("hotkey_free", "自由截图", "Shift+D", "D", 102);

        public static KeyModel HOTKEY_LAST = new KeyModel("hotkey_last", "重复上次选框截图", "Ctrl+Alt+L", "L", 103);

        public static KeyModel HOTKEY_CLIP = new KeyModel("hotkey_clip", "从剪切板截图", "Ctrl+Shift+E", "E", 104);

        public static KeyModel HOTKEY_EXIT = new KeyModel("hotkey_exit", "退出程序", "Ctrl+Alt+H", "H", 105);

        //public static KeyModel HOTKEY_WHOLE = new KeyModel("hotkey_whole", "全屏截图", "Ctrl+G", "G", 100);

        //public static KeyModel HOTKEY_ACTIVE = new KeyModel("hotkey_active", "活动窗体截图", "Alt+S", "S", 101);

        //public static KeyModel HOTKEY_FREE = new KeyModel("hotkey_free", "自由截图", "Ctrl+Alt+A", "A", 102);

        //public static KeyModel HOTKEY_LAST = new KeyModel("hotkey_last", "重复上次选框截图", "Ctrl+Alt+L", "L", 103);

        //public static KeyModel HOTKEY_CLIP = new KeyModel("hotkey_clip", "从剪切板截图", "Ctrl+Shift+C", "C", 104);

        //public static KeyModel HOTKEY_EXIT = new KeyModel("hotkey_exit", "退出程序", "Ctrl+Shift+E", "E", 105);
        /// <summary>
        /// 字母按键个数
        /// </summary>
        public static int KEY_NUM = 26;

        /// <summary>
        /// 设置-截图文件保存路径字段
        /// </summary>
        public static string SAVE_PIC_PATH = "path";
        /// <summary>
        /// 默认截图文件保存路径
        /// </summary>
        public static string DEFAULT_SAVE_PIC_PATH = "";

        /// <summary>
        /// 设置-文件扩展名字段
        /// </summary>
        public static string SAVE_FILE_EXTENSION = "file_extension";
        /// <summary>
        /// 默认截图文件扩展名
        /// </summary>
        public static string DEFAULT_FILE_EXTENSION = "png";
        /// <summary>
        /// 注册表注册名
        /// </summary>
        public static string KEY_NAME = "lxzhASS";

        public static Pen BoardPen = new Pen(new SolidBrush(System.Drawing.SystemColors.Highlight), 1.4F);

        //绘制圆角
        public static void SetWindowRegion(Control control, int radiu) {
            Rectangle rect = new Rectangle(0, 0, control.Width - 2, control.Height - 2);//this.Left-10,this.Top-10,this.Width-10,this.Height-10);                 
            GraphicsPath FormPath = GetRoundedRectPath(rect, 2);
            control.Region = new Region(FormPath);
        }

        public static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius) {
            int diameter = radius * 2;
            int arcX1 = rect.X;
            int arcX2 = rect.Right - diameter;
            int arcY1 = rect.Y;
            int arcY2 = rect.Bottom - diameter;
            GraphicsPath roundedRect = new GraphicsPath();
            //   左上角
            roundedRect.AddArc(arcX1, arcY1, diameter, diameter, 180, 90);
            //roundedRect.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            //   右上角
            roundedRect.AddArc(arcX2, arcY1, diameter, diameter, 270, 90);
            //roundedRect.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius - 1);
            //   右下角
            roundedRect.AddArc(arcX2, arcY2, diameter, diameter, 0, 90);
            //roundedRect.AddLine(rect.Right - radius - 1, rect.Bottom, rect.X + radius, rect.Bottom);
            //   左下角
            roundedRect.AddArc(arcX1, arcY2, diameter, diameter, 90, 90);
            //roundedRect.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            //roundedRect.CloseFigure();
            roundedRect.CloseAllFigures();
            return roundedRect;
        }
        public static GraphicsPath GetRoundedRectPath(RectangleF rect, float radius) {
            float diameter = radius * 2;
            float arcX1 = rect.X;
            float arcX2 = rect.Right - diameter;
            float arcY1 = rect.Y;
            float arcY2 = rect.Bottom - diameter;
            GraphicsPath roundedRect = new GraphicsPath();
            //   左上角
            roundedRect.AddArc(arcX1, arcY1, diameter, diameter, 180, 90);
            //roundedRect.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            //   右上角
            roundedRect.AddArc(arcX2, arcY1, diameter, diameter, 270, 90);
            //roundedRect.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius - 1);
            //   右下角
            roundedRect.AddArc(arcX2, arcY2, diameter, diameter, 0, 90);
            //roundedRect.AddLine(rect.Right - radius - 1, rect.Bottom, rect.X + radius, rect.Bottom);
            //   左下角
            roundedRect.AddArc(arcX1, arcY2, diameter, diameter, 90, 90);
            //roundedRect.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            //roundedRect.CloseFigure();
            roundedRect.CloseAllFigures();
            return roundedRect;
        }

        public static bool isZeroRect(Rectangle r) {
            return (r.X == 0 && r.Y == 0 && r.Height == 0 && r.Height == 0);
        }

        /// <summary>
        /// 根据控件名获取控件实例
        /// </summary>
        /// <param name="name">控件名</param>
        /// <returns>控件实例 需强制转换为对应控件类型</returns>
        public static object GetControlByName(Form frm,string name) {
            object o = frm.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance).GetValue(frm);
            return o;
        }
        /// <summary>
        /// 保存时获取当前时间字符串作为默认文件名
        /// </summary>
        /// <returns>当前时间字符串文件名</returns>
        public static string GetSavePicPath() {
            string path = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.SAVE_PIC_PATH, Util.DEFAULT_SAVE_PIC_PATH);
            string extention = IniFile.ReadIniData(Util.CONFIG_SECTION, Util.SAVE_FILE_EXTENSION, Util.DEFAULT_FILE_EXTENSION);
            if (string.IsNullOrEmpty(path))
                path = Util.DEFAULT_SAVE_PIC_PATH;
            if (string.IsNullOrEmpty(extention))
                extention = Util.DEFAULT_FILE_EXTENSION;
            if (!Directory.Exists(path))
                CreateDeepFolder(path);
            return string.Format("{0}\\{1}.{2}", path, DateTime.Now.ToString("yyyyMMdd HHmmss"), extention);
        }

        private static bool CreateDeepFolder(string path) {
            bool result;
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Parent.Exists) {
                result = CreateDeepFolder(di.Parent.FullName);
            }
            di.Create();
            result = true;
            return result;
        }
    }

    //截图类型
    public enum PrintType
    {
        WholeScreen, ActiveWin, FreeScreen
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;                            //最左坐标
        public int Top;                             //最上坐标
        public int Right;                           //最右坐标
        public int Bottom;                          //最下坐标
    }

    //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）  
    [Flags()]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Ctrl = 2,
        Shift = 4,
        WindowsKey = 8
    }
}
