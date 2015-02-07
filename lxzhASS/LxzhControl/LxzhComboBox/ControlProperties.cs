using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace lxzh{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ControlProperties {
        public ControlProperties() { }

        #region PRIVATE PROPERTIES

        private bool _AutoSize = false;
        private bool _AutoEllipsis = false;
        private FlatStyle _FlatStyle = FlatStyle.Standard;
        private Color _ForeColor = SystemColors.ControlText;
        private RightToLeft _RightToLeft = RightToLeft.No;
        private ContentAlignment _TextAlign = ContentAlignment.MiddleLeft;

        #endregion

        #region PUBLIC PROPERTIES


        [DefaultValue(false)]
        public bool AutoEllipsis {
            get { return _AutoEllipsis; }
            set { _AutoEllipsis = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool AutoSize {
            get { return _AutoSize; }
            set { _AutoSize = true; OnPropertyChanged(); }
        }
        [DefaultValue(FlatStyle.Standard)]
        public FlatStyle FlatStyle {
            get { return _FlatStyle; }
            set { _FlatStyle = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ForeColor {
            get { return _ForeColor; }
            set { _ForeColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(RightToLeft.No)]
        public RightToLeft RightToLeft {
            get { return _RightToLeft; }
            set { _RightToLeft = value; OnPropertyChanged(); }
        }
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign {
            get { return _TextAlign; }
            set { _TextAlign = value; OnPropertyChanged(); }
        }

        #endregion

        #region EVENTS AND EVENT CALLERS

        /// <summary>
        /// Called when any property changes.
        /// </summary>
        public event EventHandler PropertyChanged;

        protected void OnPropertyChanged() {
            EventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion
    }
}
