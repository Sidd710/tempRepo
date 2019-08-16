using Atum.Studio.Controls.NewGui;
using Atum.Studio.Controls.OpenGL;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    public class ProgressBarManager
    {
        private static SceneControlProgressbar _progressbarMain = new SceneControlProgressbar();
        private static SceneControlProgressbar _progressbarMaterials = new SceneControlProgressbar();

        #region mainform progressbar
        public static void InitialiseMain()
        {

            STLModel3D.OpenFileProcessing += STLModel3D_OpenFileProcessing;
            STLModel3D.OpenFileProcesssed += STLModel3D_OpenFileProcesssed;
            Engines.MagsAI.MagsAIEngine.ModelProgressChanged += MagsAIEngine_ModelProgressChanged;
            Engines.MagsAI.MagsAIEngine.CalcModelAngledSurfaceSupportSliceProcessed += MagsAIEngine_CalcModelAngledSurfaceSupportSliceProcessed;
        }

        private static void MagsAIEngine_CalcModelAngledSurfaceSupportSliceProcessed(object sender, MagsAIProgressEventArgs e)
        {
            UpdateMainPercentage(e.Percentage);
        }

        private static void MagsAIEngine_ModelProgressChanged(object sender, MagsAIProgressEventArgs e)
        {
            UpdateMainPercentage(e.Percentage);
        }

        private static void STLModel3D_OpenFileProcesssed(object sender, OpenFileEventArgs e)
        {
            UpdateMainPercentage(100);
        }

        private static void STLModel3D_OpenFileProcessing(object sender, OpenFileEventArgs e)
        {
            UpdateMainPercentage(e.Percentage);
        }

        private static void CreateProgressBarMain()
        {
            var modelToolbar = frmStudioMain.SceneControl.Controls.OfType<SceneControlPrintJobPropertiesToolbar>().FirstOrDefault();
            if (modelToolbar != null && modelToolbar.Visible)
            {
                _progressbarMain = new SceneControlProgressbar();
                _progressbarMain.Top = modelToolbar.Top - _progressbarMain.Height;
                _progressbarMain.Left = modelToolbar.Left;
                _progressbarMain.Width = modelToolbar.Width;
                _progressbarMain.Visible = true;
            }

            if (frmStudioMain.SceneControl.Controls.OfType<SceneControlProgressbar>().Count() == 0)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate
                    {
                        frmStudioMain.SceneControl.Controls.Add(_progressbarMain);
                    }));
                }
                else
                {
                    frmStudioMain.SceneControl.Controls.Add(_progressbarMain);
                }
            }

        }

        internal static void UpdateMainPercentage(float value)
        {
            if ((_progressbarMain == null || (frmStudioMain.SceneControl != null) && frmStudioMain.SceneControl.Controls.OfType<SceneControlProgressbar>().Count() == 0) && frmStudioMain.SceneControl != null && frmStudioMain.SceneControl.Controls.OfType<SceneControlPrintJobPropertiesToolbar>().Count() > 0)
            {
                CreateProgressBarMain();
            }

            if (frmStudioMain.SceneControl != null)
            {
                var modelToolbar = frmStudioMain.SceneControl.Controls.OfType<SceneControlPrintJobPropertiesToolbar>().FirstOrDefault();
                if (modelToolbar != null && _progressbarMain != null)
                {
                    _progressbarMain.Top = modelToolbar.Top - _progressbarMain.Height;
                    _progressbarMain.Left = modelToolbar.Left;
                    _progressbarMain.Width = modelToolbar.Width;
                    _progressbarMain.ProgressValue = value;
                }
            }
        }

        #endregion

        #region mainform progressbar

        internal static void CreateOnlineMaterialProgressbar(Panel panel)
        {
            _progressbarMaterials = new SceneControlProgressbar();
            _progressbarMaterials.Dock = DockStyle.Bottom;
            _progressbarMaterials.Visible = true;

                if (panel.InvokeRequired)
                {
                    panel.Invoke(new MethodInvoker(delegate
                    {
                        panel.Controls.Add(_progressbarMaterials);
                    }));
                }
                else
                {
                panel.Controls.Add(_progressbarMaterials);
                }
            

        }

        internal static void UpdateOnlineMaterialPercentage(float value)
        {
            _progressbarMaterials.ProgressValue = value;
        }
        #endregion
    }
}
