using System.ComponentModel;
using System.Windows.Forms;

namespace lxzh {
    public partial class LCheckBox : CheckBox {
        private bool oriState;
        [Browsable(false)]
        public bool OriState {
            get { return oriState; }
            set { oriState = value; }
        }
        [Browsable(false)]
        public bool IsStateChanged {
            get {
                return oriState != this.Checked;
            }
        }
        public LCheckBox() {
            InitializeComponent();
        }
    }
}
