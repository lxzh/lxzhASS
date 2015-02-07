using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

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
        #region HOTKEY_WHOLE
        /// <summary>
        /// 设置-全屏截图字段
        /// </summary>
        //public static string HOTKEY_WHOLE = "hotkey_whole";
        /// <summary>
        /// 全屏截图快捷键ID
        /// </summary>
        public const int HOTKEY_WHOLE_ID = 100;
        /// <summary>
        /// 默认全屏截图快捷键
        /// </summary>
        public static string DEFAULT_HOTKEY_WHOLE = "Win+S";
        #endregion
        public static KeyModel HOTKEY_WHOLE = new KeyModel("hotkey_whole","全屏截图", "Win+S", "S", 100);
        #region HOTKEY_ACTIVE
        /// <summary>
        /// 设置-活动窗体截图字段
        /// </summary>
        //public static string HOTKEY_ACTIVE = "hotkey_active";
        /// <summary>
        /// 活动窗体截图快捷键ID
        /// </summary>
        public const int HOTKEY_ACTIVE_ID = 101;
        /// <summary>
        /// 默认活动窗体截图快捷键
        /// </summary>
        public static string DEFAULT_HOTKEY_ACTIVE = "Alt+S";
        #endregion 
        public static KeyModel HOTKEY_ACTIVE = new KeyModel("hotkey_active", "", "Alt+S", "S", 101);
        #region HOTKEY_FREE
        /// <summary>
        /// 设置-自由截图字段
        /// </summary>
        //public static string HOTKEY_FREE = "hotkey_free";
        /// <summary>
        /// 自由截图快捷键ID
        /// </summary>
        public const int HOTKEY_FREE_ID = 102;
        /// <summary>
        /// 默认自由截图快捷键
        /// </summary>
        public static string DEFAULT_HOTKEY_FREE = "Ctrl+Alt+A";
        #endregion 
        public static KeyModel HOTKEY_FREE = new KeyModel("hotkey_free", "", "Ctrl+Alt+A", "A", 102);
        #region HOTKEY_LAST
        /// <summary>
        /// 设置-重复上次位置截图字段
        /// </summary>
        //public static string HOTKEY_LAST = "hotkey_last";
        /// <summary>
        /// 重复上次位置截图快捷键ID
        /// </summary>
        public const int HOTKEY_LAST_ID = 103;
        /// <summary>
        /// 默认重复上次位置截图快捷键
        /// </summary>
        public static string DEFAULT_HOTKEY_LAST = "Ctrl+Alt+L";
        #endregion 
        public static KeyModel HOTKEY_LAST = new KeyModel("hotkey_last", "", "Ctrl+Alt+L", "L", 103);
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
