using System;
using System.Drawing;
using System.Windows.Forms;

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

        private void ScreenForm_Load(object sender, EventArgs e)
        {
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            if (hotkeyId == Util.HOTKEY_ACTIVE.KeyId) {
                rect = FormUtil.getForeWinRect();
            } else if (hotkeyId == Util.HOTKEY_LAST.KeyId) {
                rect = Util.GetSavedRect(rect);
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

            string filename = Util.GetSavePicPath();
            bmp.Save(filename);
            Util.SaveRectInfo(rect);
            Clipboard.SetImage(bmp);

            //Toast toast = new Toast(2, string.Format("已截图保存至：\n{0}", filename));
            //toast.StartPos = ContentAlignment.BottomRight;
            //toast.Show();
        }
    }
}
