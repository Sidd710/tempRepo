using Atum.Studio.Core.Events;
using System.Windows.Forms;

namespace Atum.Studio.Controls.MagsAI
{
    public partial class MagsAIPreview : UserControl
    {
        private OpenGL.SceneGLControl _glControl;
        internal OpenGL.SceneGLControl GLControl
        {
            get
            {
                return this._glControl;
            }
            set
            {
                if (!DesignMode)
                {
                    this._glControl = value;
                    this.Controls.Add(this._glControl);
                    if (this._glControl != null)
                    {
                        this._glControl.Render();
                    }
                }
            }
        }

        internal void RemoveGLControl()
        {
            this.Controls.Clear();
        }

        internal MagsAIProgressEventArgs Progress
        {
            set
            {
                if (this.progressBar1.InvokeRequired)
                {
                    this.progressBar1.Invoke(new MethodInvoker(delegate
                    {
                        this.Progress = value;
                    }));
                }
                else{
                    if (value.Percentage == 100)
                    {
                        this.progressBar1.Value = value.Percentage;
                        if (this.GLControl != null)
                        {
                            this.GLControl.Render();
                        }
                        
                    }
                    else
                    {
                        this.progressBar1.Value = value.Percentage;
                    }
                }
            }
        }

        public MagsAIPreview()
        {
            InitializeComponent();
        }

        private void MagsAIPreview_Load(object sender, System.EventArgs e)
        {
        }
    }
}
