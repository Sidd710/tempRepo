using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public class FloatInputField18: RichTextBox
    {
        public FloatInputField18()
        {
            this.SelectAll();
            this.SelectionAlignment = HorizontalAlignment.Right;
            this.DeselectAll();

            this.Multiline = false;
            this.Height = 32;
            
            this.BorderStyle = BorderStyle.None;

            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat18Regular;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {

            }
            else if ((e.KeyChar == '-') && !this.Text.Contains("-") && this.SelectionStart == 0)
            {

            }
            else if (e.KeyChar == ',' || e.KeyChar == '.')
            {
                if ((!this.Text.Contains(".") && !this.Text.Contains(".")))
                {
                    e.KeyChar = '.';
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }

            if (!e.Handled)
            {
                if (this.SelectedText.Length > 0)
                {
                    this.Text = string.Empty;
                }
                var currentSelectionIndex = this.SelectionStart;
                this.SelectAll();
                this.SelectionAlignment = HorizontalAlignment.Right;
                this.DeselectAll();
                this.SelectionStart = currentSelectionIndex;
            }

            //base.OnKeyPress(e);
        }
    }
}
