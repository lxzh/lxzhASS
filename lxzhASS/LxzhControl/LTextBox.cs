using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lxzh {
    public partial class LTextBox : TextBox {
        private string oriText;
        [Browsable(false)]
        public string OriText {
            get { return oriText; }
            set { oriText = value; }
        }
        [Browsable(false)]
        public bool IsValueChanged {
            get {
                return oriText != this.Text;
            }
        }
        public LTextBox() {
            InitializeComponent();
        }
    }
}
