using System.ComponentModel;
using System.Windows.Forms;

namespace lxzh {
    public partial class LComboBox : ComboBox {
        private int oriIndex;
        [Browsable(false)]
        public int OriIndex {
            get { return oriIndex; }
            set { oriIndex = value; }
        }
        [Browsable(false)]
        public bool IsValueChanged {
            get {
                return oriIndex != this.SelectedIndex;
            }
        }
        public LComboBox() {
            InitializeComponent();
        }
    }
}
