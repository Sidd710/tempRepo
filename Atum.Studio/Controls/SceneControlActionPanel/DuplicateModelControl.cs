using System;
using System.Drawing;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Engines.PackingEngine;
using System.IO;

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class DuplicateModelControl : UserControl
    {
        internal event EventHandler DuplicatesMinus_Click;
        internal event EventHandler DuplicatesPlus_Click;
        internal event KeyPressEventHandler DuplicatesCount_KeyUp;

        internal int _totalAmount = 0;

        internal int MaxClones;
        internal int TotalAmount
        {
            get
            {
                return this._totalAmount;
            }
            set
            {
                if (this._totalAmount != value)
                {
                    this._totalAmount = value;

                    this.txtDuplicatesCount.TextChanged -= txtDuplicatesCount_TextChanged;
                    this.txtDuplicatesCount.Text = this._totalAmount.ToString();
                    this.txtDuplicatesCount.TextChanged += txtDuplicatesCount_TextChanged;
                }
            }
        }

        internal Color BackgroundColor;
        internal ModelFootprint ModelFootprint;

        public DuplicateModelControl(ModelFootprint footprint)
        {
            InitializeComponent();

            this.btnDuplicatesMinus.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.minus_black, new Size(24,24));
            this.btnDuplicatesPlus.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.plus_black, new Size(24, 24));

            ModelFootprint = footprint;
            MaxClones = footprint.CloneCount;
            string modelName = Path.GetFileNameWithoutExtension(footprint.Model.FileName);

            var font = FontManager.Montserrat14Regular;
            this.txtDuplicatesCount.Font = font;
            this.lblModelName.Font = font;

            var textSize = TextRenderer.MeasureText("1", font);
            this.txtDuplicatesCount.Top += (this.txtDuplicatesCount.Height - textSize.Height) / 2;

            lblModelName.Text = modelName;

            if (footprint.Model.LinkedClones.Count > 0)
            {
                this.txtDuplicatesCount.Text = (footprint.Model.LinkedClones.Count).ToString();
            }
            else
            {
                this.txtDuplicatesCount.Text = (footprint.Model.LinkedClones.Count + 1).ToString();
            }

            TotalAmount = int.Parse(this.txtDuplicatesCount.Text);
            this.BackColor = BackgroundColor;
            this.plDuplicateModelControl.BackColor = BackgroundColor;
        }

        private void btnDuplicatesMinus_Click(object sender, EventArgs e)
        {
            _totalAmount = int.Parse(this.txtDuplicatesCount.Text);
            if (_totalAmount > 1)
            {
                DuplicatesMinus_Click?.Invoke(this, e);
            }
        }

        private void btnDuplicatesPlus_Click(object sender, EventArgs e)
        {
            DuplicatesPlus_Click?.Invoke(this, e);
        }

        private void txtDuplicatesCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit((char)e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Delete || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtDuplicatesCount_TextChanged(object sender, EventArgs e)
        {
            this.txtDuplicatesCount.Text = this.txtDuplicatesCount.Text.Replace("\r\n", string.Empty);

            if (this.txtDuplicatesCount.Text == "0")
            {
                this.txtDuplicatesCount.Text = "1";
            }
            if (this.txtDuplicatesCount.Text != string.Empty)
            {
                this.TotalAmount = int.Parse(this.txtDuplicatesCount.Text);
                DuplicatesCount_KeyUp?.Invoke(this, null);
            }
        }

       
        private void txtDuplicatesCount_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtDuplicatesCount);
        }
    }
}
