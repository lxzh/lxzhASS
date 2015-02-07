using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace lxzh
{
    public partial class ScreenForm : Form
    {
        private int hotkeyId;

        public int HotkeyId {
            get { return hotkeyId; }
            set { hotkeyId = value; }
        }

        public ScreenForm()
        {
            InitializeComponent();
        }
        public ScreenForm(int hotkeyId) {
            InitializeComponent();
            this.hotkeyId = hotkeyId;
        }

        private void FullScreenForm_Load(object sender, EventArgs e)
        {
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            if (hotkeyId == Util.HOTKEY_ACTIVE.KeyId) {
                rect = FormUtil.getForeWinRect();
            } else if (hotkeyId == Util.HOTKEY_LAST.KeyId) {
                rect = getFormInfo(rect);
            }
            if (rect.Left < 0) {
                rect.Width += rect.Left;
                rect.X = 0;
            }
            if (rect.Top < 0) {
                rect.Height += rect.Top;
                rect.Y = 0;
            }
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(rect.Width, rect.Height));

            string filename = getPath();
            bmp.Save(filename);
            saveFormInfo(rect);
            Clipboard.SetImage(bmp);

            //Toast toast = new Toast(2, string.Format("已截图保存至：\n{0}", filename));
            //toast.StartPos = ContentAlignment.BottomRight;
            //toast.Show();
            this.Close();
        }

        private string getPath() {
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

        private Rectangle getFormInfo(Rectangle rect) {
            int x = Int32.Parse(IniFile.ReadIniData(Util.FORM_SECTION, "x", rect.Left + ""));
            int y = Int32.Parse(IniFile.ReadIniData(Util.FORM_SECTION, "y", rect.Top + ""));
            int w = Int32.Parse(IniFile.ReadIniData(Util.FORM_SECTION, "width", rect.Width + ""));
            int h = Int32.Parse(IniFile.ReadIniData(Util.FORM_SECTION, "height", rect.Height + ""));
            return new Rectangle(x, y, w, h);
        }

        private void saveFormInfo(Rectangle rect) {
            IniFile.WriteIniData(Util.FORM_SECTION, "x", rect.Left + "");
            IniFile.WriteIniData(Util.FORM_SECTION, "y", rect.Top + "");
            IniFile.WriteIniData(Util.FORM_SECTION, "width", rect.Width + "");
            IniFile.WriteIniData(Util.FORM_SECTION, "height", rect.Height + "");
        }

        private bool CreateDeepFolder(string path)
        {
            bool result;
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Parent.Exists)
            {
                result = CreateDeepFolder(di.Parent.FullName);
            }
            di.Create();
            result = true;
            return result;
        }
    }
}
