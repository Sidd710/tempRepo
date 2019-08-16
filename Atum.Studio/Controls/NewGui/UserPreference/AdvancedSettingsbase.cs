using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Atum.Studio.Controls.NewGUI.UserPreference
{
    public partial class AdvancedSettingsbase : UserControl
    {
        public event EventHandler onSelected;
        private string _SettingText;
        private bool _selected;

        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                this.UpdateControl();
            }
        }
        public string SettingText
        {
            get
            {
                return this._SettingText;
            }
            set
            {
                this._SettingText = value;
                this.UpdateControl();
            }
        }

        public AdvancedSettingsbase()
        {
            InitializeComponent();

            this.txtSettingText.onSelected += TxtSettingText_onSelected;
        }
        private void TxtSettingText_onSelected(object sender, EventArgs e)
        {
            this.onSelected?.Invoke(this, null);
        }

        public void UpdateControl()
        {
            try
            {
                this.txtSettingText.TextValue = this.SettingText;

                var colorPicture = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
                var f = Graphics.FromImage(colorPicture);
                f.Clear(this.Selected ? Color.FromArgb(255, 24, 0) : Color.White);
                f.SmoothingMode = SmoothingMode.AntiAlias;
                //    f.FillEllipse(new SolidBrush(Color.White), new RectangleF(1, 1, 14, 14));
                //f.FillEllipse(new SolidBrush(this._material.ModelColor), new RectangleF(1, 1, 14, 14));


                this.BackColor = this.Selected ? Color.FromArgb(255, 24, 0) : Color.White;
                this.txtSettingText.BackColor = this.BackColor;
                this.txtSettingText.ForeColor = this.Selected ? Color.White : Color.Black;
                this.Select();
            }
            catch (Exception exc)
            {

            }
        }
    }
}
