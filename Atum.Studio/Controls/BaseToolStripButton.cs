using System.ComponentModel;
using System.Windows.Forms;
using static Atum.Studio.Core.Enums;

namespace Atum.Studio.Controls
{
    public class BaseToolStripButton : ToolStripButton
    {
        [Browsable(true)]
        public SoftwareLevelType MinVisibleLevel { get; set; }

        public bool InvokeChecked
        {
            set
            {
                var toolstripActions = this.Owner;
                if (toolstripActions.InvokeRequired)
                {
                    toolstripActions.Invoke(new MethodInvoker(delegate
                    {
                        this.Checked = value;
                    }));
                }
                else
                {
                    this.Checked = value;
                }
            }
        }

        public bool InvokeEnabled
        {
            set
            {
                var toolstripActions = this.Owner;
                if (toolstripActions != null)
                {
                    if (toolstripActions.InvokeRequired)
                    {
                        toolstripActions.Invoke(new MethodInvoker(delegate
                        {
                            this.Enabled = value;
                        }));
                    }
                    else
                    {
                        this.Enabled = value;
                    }
                }
            }
        }


    }
}
