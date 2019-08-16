using System;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class WaitWindow : Form
    {
        private Timer _showDelayTimer = new Timer();

        public string ProgressText
        {
            get { return this.lblProgress.Text; }
            set
            {
                if (this.lblProgress.InvokeRequired)
                {
                    this.lblProgress.Invoke(new MethodInvoker(delegate { this.lblProgress.Text = value; }));
                }
                else
                {
                    this.lblProgress.Text = value;
                }
            }
        }

        public double ProgressValue
        {
            get { return this.pgStatus.Value; }
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                if (this.pgStatus.InvokeRequired)
                {
                    this.pgStatus.Invoke(new MethodInvoker(delegate { this.pgStatus.Value = (int)value; }));
                }
                else
                {
                    this.pgStatus.Value = (int)value;
                }
            }
        }

        public WaitWindow()
        {
            InitializeComponent();

            //this.Hide();
            //this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
            
            //this._showDelayTimer = new Timer();
            //this._showDelayTimer.Interval = this._showDelay * 1000;
            //this._showDelayTimer.Tick += _showDelayTimer_Tick;
            //this._showDelayTimer.Start();
        }

        void _showDelayTimer_Tick(object sender, EventArgs e)
        {
        
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void WaitWindow_Load(object sender, EventArgs e)
        {
        }

    }
}
