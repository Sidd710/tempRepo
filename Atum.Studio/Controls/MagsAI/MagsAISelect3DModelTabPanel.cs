using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.ModelView;
using System.IO;

namespace Atum.Studio.Controls.MagsAI
{
    public partial class MagsAISelect3DModelTabPanel : UserControl
    {
        internal string FilePath { get; set; }
        internal event EventHandler ModelLoaded;

        public MagsAISelect3DModelTabPanel()
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.DragEnter += MagsAIWelcomeTabPanel_DragEnter;
            this.DragDrop += MagsAIWelcomeTabPanel_DragDrop;

            this.lblDropLabel.DragEnter += MagsAIWelcomeTabPanel_DragEnter;
            this.lblDropLabel.DragDrop += MagsAIWelcomeTabPanel_DragDrop;
        }

        private void STLModel3D_OpenFileProcesssed(object sender, Core.Events.OpenFileEventArgs e)
        {
            ModelLoaded?.Invoke(null, null);
        }

        private void STLModel3D_OpenFileProcessing(object sender, Core.Events.OpenFileEventArgs e)
        {
            if (this.progressBar1.InvokeRequired)
            {
                this.progressBar1.Invoke(new MethodInvoker(delegate { STLModel3D_OpenFileProcessing(sender, e); }));
            }
            else
            {
                this.progressBar1.Value = e.Percentage;
            }
        }

        private void MagsAIWelcomeTabPanel_DragDrop(object sender, DragEventArgs e)
        {
            var validSTLFile = false;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (file.ToLower().EndsWith("stl"))
                {
                    validSTLFile = true;
                }

                if (validSTLFile)
                {
                    ObjectView.Create(new string[] { file }, Color.Aqua, true, null);
                }
            }
        }

        private void MagsAIWelcomeTabPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void btnSelect3DModel_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "(*.stl)|*.stl";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.FilePath = openFileDialog.FileName;
                    this.txt3DModelFilePath.Text = this.FilePath;

                    ObjectView.Create(new string[] { this.FilePath }, Color.Aqua, true, null);
                }
            }
        }
    }
}
