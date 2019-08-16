using Atum.Studio.Controls.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    internal class StatusbarManager
    {
        private static bool _disabled { get; set; }

        private static StatusStrip _statusbar;

        private static ToolStripProgressBar CurrentProcessProgressbar { get; set; }
        private static ToolStripStatusLabel CurrentProcessProgressLabel { get; set; }
        private static ToolStripStatusLabel CurrentActionMessage { get; set; }

        private static SceneGLControl SceneControl;
        internal static bool ProcessingSceneProject { get; set; }

        internal static void Initialize(StatusStrip statusbar, SceneGLControl sceneControl)
        {
            if (!_disabled)
            {
                Core.Models.STLModel3D.OpenFileProcessing += STLModel3D_OpenFileProcessing;
                _statusbar = statusbar;
                SceneControl = sceneControl;

                foreach (ToolStripItem control in statusbar.Items)
                {
                    if (control.Name == "pgStatusOfCurrentProgress")
                    {
                        CurrentProcessProgressbar = (ToolStripProgressBar)control;
                    }
                    else if (control.Name == "lblStatusOfCurrentProgress")
                    {
                        CurrentProcessProgressLabel = (ToolStripStatusLabel)control;
                    }
                    else if (control.Name == "lblActionMessage")
                    {
                        CurrentActionMessage = (ToolStripStatusLabel)control;
                    }
                }

                //always hide progress status
                HideProgressStatus();
            }
        }

        private static void STLModel3D_OpenFileProcessing(object sender, Events.OpenFileEventArgs e)
        {
            if (!_disabled)
            {
                UpdateProgressStatus(e.Message, e.Percentage);
            }
        }

        internal static void HideProgressStatus()
        {
            if (!_disabled)
            {
                if (CurrentProcessProgressbar != null)
                {
                    if (_statusbar.InvokeRequired)
                    {
                        _statusbar.Invoke(new MethodInvoker(delegate
                        {
                            CurrentProcessProgressbar.Visible = false;
                            CurrentProcessProgressLabel.Visible = false;
                        }));
                    }
                    else
                    {
                        CurrentProcessProgressbar.Visible = false;
                        CurrentProcessProgressLabel.Visible = false;
                    }
                }
            }
        }

        internal static void UpdateProgressStatus(string text, int percentage)
        {
            if (!_disabled)
            {
                if (percentage > 0)
                {
                    if (_statusbar.InvokeRequired)
                    {
                        _statusbar.Invoke(new MethodInvoker(delegate
                        {
                            if (percentage != 100)
                            {
                                CurrentProcessProgressbar.Visible = true;
                                CurrentProcessProgressLabel.Visible = true;

                                CurrentProcessProgressbar.Value = percentage;
                                CurrentProcessProgressLabel.Text = text;

                                //ProcessingSceneProject = true;
                            }
                            else
                            {
                                HideProgressStatus();

                                //ProcessingSceneProject = false;

                              //  SceneControl.StartSelectionTimer();
                            }
                        }));
                    }
                    else
                    {
                        if (percentage != 100)
                        {
                            CurrentProcessProgressbar.Visible = true;
                            CurrentProcessProgressLabel.Visible = true;

                            CurrentProcessProgressbar.Value = percentage;
                            CurrentProcessProgressLabel.Text = text;

                            //ProcessingSceneProject = true;
                        }
                        else
                        {
                            HideProgressStatus();

                            //ProcessingSceneProject = false;

                            //SceneControl.StartSelectionTimer();


                        }
                    }
                }
            }
        }

        internal static void UpdateActionMessage(string text)
        {
            if (!_disabled)
            {
                if (CurrentActionMessage != null)
                {
                    if (_statusbar.InvokeRequired)
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            CurrentActionMessage.Visible = false;
                        }
                        else
                        {
                            CurrentActionMessage.Text = text;
                            CurrentActionMessage.Visible = true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            CurrentActionMessage.Visible = false;
                        }
                        else
                        {
                            CurrentActionMessage.Text = text;
                            CurrentActionMessage.Visible = true;
                        }
                    }
                }
            }
        }

        internal static void Enable()
        {
            _disabled = false;
        }

        internal static void Disable()
        {
            _disabled = true;
        }
    }
}
