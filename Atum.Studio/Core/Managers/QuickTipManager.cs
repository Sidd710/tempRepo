using Atum.Studio.Controls.QuickTips;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Atum.Studio.Core.Managers
{
    public class QuickTipManager
    {
        private static Controls.OpenGL.SceneGLControl _sceneControl;

        public static void Initialize(Controls.OpenGL.SceneGLControl sceneControl)
        {
            _sceneControl = sceneControl;
        }

        public static void ShowTip(string caption, string text)
        {
            RemoveAllTips();

            var quickTipControl = new QuickTipActionInformation();
            _sceneControl.Controls.Add(quickTipControl);
            quickTipControl.Show(caption, text);
        }

        public static void ShowTip(QuickTipActionInformation quickTip)
        {
            RemoveAllTips();

            _sceneControl.Controls.Add(quickTip);
            quickTip.Show();
        }

        private static void RemoveAllTips()
        {
            var controlsToRemove = new List<QuickTipActionInformation>();

            foreach (Control control in _sceneControl.Controls)
            {
                if (control is QuickTipActionInformation)
                {
                    controlsToRemove.Add(control as QuickTipActionInformation);
                }
            }

            foreach(var control in controlsToRemove)
            {
                _sceneControl.Controls.Remove(control);
            }
        }

        public static void UpdateText(string text)
        {
            foreach(Control control in _sceneControl.Controls){
                if (control is QuickTipActionInformation){
                    var quickTip = control as QuickTipActionInformation;
                    quickTip.QuickTipText = text;
                };
            }
        }

        public static void HideTip()
        {
            if (_sceneControl != null)
            {
                if (_sceneControl.InvokeRequired)
                {
                    _sceneControl.Invoke(new MethodInvoker(delegate
                    {
                        _sceneControl.Controls.Clear();
                        _sceneControl.Update();
                    }));
                }
                else
                {
                    _sceneControl.Controls.Clear();
                    _sceneControl.Update();
                }
            }
        }
    }
}
