using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public partial class SceneControlProgressbar : UserControl  
    {
        public float ProgressValue
        {
            set
            {
                if (value != 100)
                {
                    this.plProgressValue.BackColor = BrandingManager.Menu_Item_HighlightColor;

                    var progressStepSize = this.plMaxValue.Width / 100f;
                    if (value <= 99f)
                    {
                        progressStepSize = value * progressStepSize;
                    }
                    else
                    {
                        progressStepSize = 99f * progressStepSize;
                    }

                    if (this.plProgressValue.InvokeRequired)
                    {
                        this.plProgressValue.Invoke(new MethodInvoker(delegate
                        {
                            if (this.plProgressValue.Width != (int)progressStepSize)
                            {
                                this.plProgressValue.Width = (int)progressStepSize;
                            }
                        }));
                    }
                    else
                    {
                        if (this.plProgressValue.Width != (int)progressStepSize)
                        {
                            this.plProgressValue.Width = (int)progressStepSize;
                        }
                    }
                }
                else
                {
                    if (Parent != null)
                    {
                        if (Parent.InvokeRequired)
                        {
                            try
                            {
                                Parent.Invoke(new MethodInvoker(delegate
                                {
                                    Parent.Controls.Remove(this);
                                }));
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            Parent.Controls.Remove(this);
                        }
                    }
                    
                }
            }
        }

        public SceneControlProgressbar()
        {
            InitializeComponent();
        }
    }
}
