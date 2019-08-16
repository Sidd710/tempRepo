using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class frmWait : Form
    {
        private int _steps;
        private int _currentStep;
        private int _showDelay = 2;
        private Timer _showDelayTimer = new Timer();

        public frmWait()
        {
            InitializeComponent();
        }

        public frmWait(string initialText,Form parent)
        {
            InitializeComponent();

            this.ProgressText = initialText;
            this.ProgressValue = 0;
            this.TopMost = true;
            this.Left = parent.Left + (parent.Width / 2) - (this.Width / 2);
            this.Top = parent.Top + (parent.Height / 2) - (this.Height / 2);
            this.Visible = false;
            StartDelayedShow();

        }

        public frmWait(string initialText, int steps, Form parent)
        {
            InitializeComponent();

            this.ProgressText = initialText;
            this.ProgressValue = 0;
            this._steps = steps;
            this.TopMost = true;
            this.Left = parent.Left + (parent.Width / 2) - (this.Width / 2);
            this.Top = parent.Top + (parent.Height / 2) - (this.Height / 2);
            this.Visible = false;

            StartDelayedShow();

        }

        void StartDelayedShow()
        {
            _showDelayTimer.Interval = this._showDelay * 1000;
            _showDelayTimer.Tick += _showDelayTimer_Tick;
            _showDelayTimer.Start();
        }


        void _showDelayTimer_Tick(object sender, EventArgs e)
        {
            this._showDelayTimer.Stop();
            this.Show();
        }

        public void UpdateStep(string nextTextLine)
        {
            this._currentStep++;
            this.ProgressText = nextTextLine;
            this.ProgressValue = (this.pbProgress.Maximum / this._steps) * _currentStep;
        }

        public string ProgressText
        {
            set
            {
                if (this.MessageLabel.InvokeRequired)
                {
                    this.MessageLabel.Invoke(new MethodInvoker(delegate { this.MessageLabel.Text = value; }), null);
                }
                else
                {
                    this.MessageLabel.Text = value;
                }
            }
        }

        public int ProgressValue
        {
            set
            {
                if (value > 99 && value < 100)
                {
                    if (this.pbProgress.InvokeRequired)
                    {
                        this.pbProgress.Invoke(new MethodInvoker(delegate { this.pbProgress.Value = value; }), null);
                    }
                    else if (value > 100)
                    {
                        this.pbProgress.Value = 100;
                    }
                }
                else
                {
                    if (this.pbProgress.InvokeRequired)
                    {
                        this.pbProgress.Invoke(new MethodInvoker(delegate { this.pbProgress.Value = value; }), null);
                    }
                    else
                    {
                        this.pbProgress.Value = value;
                    }
                }
            }
        }

        public void CloseWindow()
        {
            this.ProgressValue = 0;

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { this.Close(); ; }), null);
            }
            else
            {
                this.Close();
            }
        }
    }
}
