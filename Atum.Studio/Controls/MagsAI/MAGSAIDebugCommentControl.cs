using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Engines.MagsAI;

namespace Atum.Studio.Controls.MagsAI
{
    public partial class MAGSAIDebugCommentControl : UserControl
    {
        private MagsAIPreview _previewControl;

        public MAGSAIDebugComment DataSource
        {
            get
            {
                var imageConverter = new ImageConverter();

                byte[] screenshotAsByteArray = null;
                if (this.picScreenshot.BackgroundImage != null)
                {
                    screenshotAsByteArray = (byte[])imageConverter.ConvertTo(this.picScreenshot.BackgroundImage, typeof(byte[]));
                }
                return new MAGSAIDebugComment() { Comment = this.txtComment.Text, ScreenshotAsByteArray = screenshotAsByteArray };
            }
        }

        public MAGSAIDebugCommentControl()
        {
            InitializeComponent();
        }

        public MAGSAIDebugCommentControl(MagsAIPreview previewControl)
        {
            InitializeComponent();

            this._previewControl = previewControl;
        }

        private void picScreenshot_Click(object sender, EventArgs e)
        {
            if (this._previewControl != null)
            {
                this.picScreenshot.BackgroundImage = this._previewControl.GLControl.CreateScreenshot();
            }
        }
    }
}
