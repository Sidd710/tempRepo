using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Atum.Studio.Controls;
using System.Diagnostics;

namespace Atum.Studio.Core.Managers
{
    public class WaitWindowManager: IDisposable
    {
        private Form _parentForm;
        private BackgroundWorker _bwWaitWindow;
        public WaitWindow WaitWindow;
        private Timer tmrWindowProgress;

        public void Start(Form parentForm, BackgroundWorker bw)
        {
            if (parentForm != null)
            {
                _parentForm = parentForm;
                _parentForm.Move += new EventHandler(_parentForm_Move);
                _parentForm.Resize += new EventHandler(_parentForm_Resize);

                _bwWaitWindow = bw;
                _bwWaitWindow.WorkerReportsProgress = true;
                _bwWaitWindow.ProgressChanged += new ProgressChangedEventHandler(_bwWaitWindow_ProgressChanged);

                this.WaitWindow = new WaitWindow();
                this.CenterPopup();

                tmrWindowProgress = new Timer();
                tmrWindowProgress.Interval = 1000;
                tmrWindowProgress.Tick += tmrWindowProgress_Tick;
            }
        }

        void tmrWindowProgress_Tick(object sender, EventArgs e)
        {
            if (this.WaitWindow.ProgressValue == 100 && this.WaitWindow.Visible)
            {
                this.WaitWindow.Hide();
            }
            else if (this.WaitWindow.ProgressValue == 100 && !this.WaitWindow.Visible)
            {
                this.tmrWindowProgress.Stop();

            }

        }

        void _parentForm_Resize(object sender, EventArgs e)
        {
            this.CenterPopup();
        }

        void _parentForm_Move(object sender, EventArgs e)
        {
            this.CenterPopup();
        }

        private void CenterPopup()
        {
            this.WaitWindow.Location = new Point(_parentForm.Location.X + ((_parentForm.Width / 2) - (this.WaitWindow.Width / 2)), _parentForm.Location.Y + ((_parentForm.Height / 2) - (this.WaitWindow.Height / 2)));
        }

        void _bwWaitWindow_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var userState = e.UserState as WaitWindowUserState;
            if ((this.WaitWindow.ProgressText != userState.ProgressText || this.WaitWindow.ProgressValue != userState.ProgressValue))
            {
                UpdateProgress(userState.ProgressValue, userState.ProgressText);

                if (DAL.OS.OSProvider.IsWindows)
                {
                    if (userState.ProgressValue < 100)
                    {
                        Core.Taskbar.TaskbarAPI.SetValue(this._parentForm.Handle, userState.ProgressValue, 100);
                        if (!this.WaitWindow.Visible) this.WaitWindow.ShowDialog(this._parentForm);
                    }
                    else
                    {
                        Core.Taskbar.TaskbarAPI.SetState(this._parentForm.Handle, Taskbar.TaskbarAPI.TaskbarStates.NoProgress);
                    }
                }
            }

        }


        internal void UpdateProgress(double progressValue, string progressText)
        {
            try
            {
                if (progressValue == 100)
                {
                    
                    this.WaitWindow.ProgressValue = progressValue;
                    if (this._parentForm.InvokeRequired)
                    {
                        this._parentForm.Invoke(new MethodInvoker(delegate {
                            if (this._parentForm.WindowState == FormWindowState.Minimized)
                            {
                                this._parentForm.WindowState = FormWindowState.Normal;
                            }

                            this._parentForm.BringToFront();
                            
                        }));
                    }
                    else
                    {
                        if (this._parentForm.WindowState == FormWindowState.Minimized)
                        {
                            this._parentForm.WindowState = FormWindowState.Normal;
                        }

                        this._parentForm.BringToFront();
                    }

                    if (this.WaitWindow.InvokeRequired)
                    {
                        this.WaitWindow.Invoke(new MethodInvoker(delegate { this.WaitWindow.Hide(); }));
                    }
                    else
                    {
                        this.WaitWindow.Hide();
                    }
                }
                else
                {
                    if (!this.WaitWindow.Visible)
                    {
                        try
                        {
                            if (_parentForm != null)
                            {
                                _parentForm.Invoke((MethodInvoker)delegate () { this.WaitWindow.Show(_parentForm); });
                            }
                            else
                            {
                                this.WaitWindow.Show();
                            }
                        }
                        catch
                        {
                        }
                    }

                    this.WaitWindow.ProgressText = progressText;
                    this.WaitWindow.ProgressValue = progressValue;
                }
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        static void _bwWaitWindow_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (this.WaitWindow != null) this.WaitWindow.Dispose();
        }
    }
}
