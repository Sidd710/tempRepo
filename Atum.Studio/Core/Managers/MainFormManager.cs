using Atum.Studio.Controls.NewGui.ExportControl;
using Atum.Studio.Controls.NewGui.MainMenu;
using Atum.Studio.Controls.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    public class MainFormManager
    {
        public static List<String> DroppedSTLFiles { get; set; }
        public static List<string> DroppedProjectFiles { get; set; }

        public static event EventHandler MainMenuOpened;

        internal static event EventHandler HotKeyEscPressed;
        internal static event EventHandler HotKeyDPressed;
        internal static event EventHandler HotKeyFPressed;
        internal static event EventHandler HotKeyGPressed;
        internal static event EventHandler HotKeyMPressed;
        internal static event EventHandler HotKeyPPressed;
        internal static event EventHandler HotKeyOPressed;
        internal static event EventHandler HotKeyRPressed;
        internal static event EventHandler HotKeySPressed;
        internal static event EventHandler HotKeyQPressed;
        internal static event EventHandler HotKeyXPressed;
        internal static event EventHandler HotKeyZPressed;
        internal static event EventHandler HotKeyCtlCPressed;
        internal static event EventHandler HotKeyCtlPPressed;
        internal static event EventHandler HotKeyCtlZPressed;
        internal static event EventHandler HotKeyDelPressed;
        internal static event EventHandler HotKeyDecreaseSelectedBoxSizePressed;
        internal static event EventHandler HotKeyIncreaseSelectedBoxSizePressed;

        public static Action DragDropSTLFile;
        public static Action DragDropAPFFile;

        public static bool ProcessingDragDrop { get; set; }

        private static frmStudioMain _mainForm;
        private static MainMenuStrip _mainFormMenu;

        internal static bool IsInExportMode
        {
            get
            {
                return frmStudioMain.SceneControl.Controls.OfType<ExportUserControl>().Count() > 0;
            }
        }

        public static void Start(frmStudioMain mainForm)
        {
            DroppedSTLFiles = new List<string>();
            DroppedProjectFiles = new List<string>();

            //add handlers
            _mainForm = mainForm;
            _mainForm.AllowDrop = true;
            _mainForm.DragEnter += new DragEventHandler(MainForm_DragEnter);
            _mainForm.DragDrop += new DragEventHandler(MainForm_DragDrop);

            _mainFormMenu = new MainMenuStrip();
            _mainFormMenu.OnSelectMenuItem += MainFormDropdownMenu_OnSelectMenuItem;
            _mainFormMenu.Top = 0;
            _mainFormMenu.Left = 16;
        }

        public static void UpdateSceneControlEvents(SceneGLControl sceneControl)
        {
            if (sceneControl != null)
            {
                sceneControl.ContextMenu_DeleteModel += SceneControl_ContextMenu_DeleteModel;
                sceneControl.ContextMenu_Undo += SceneControl_ContextMenu_Undo;
                sceneControl.ContextMenu_DeleteSupportCone += SceneControl_ContextMenu_DeleteSupportCone;
            }
        }

        private static void SceneControl_ContextMenu_DeleteSupportCone(object sender, EventArgs e)
        {
            HotKeyDelPressed?.Invoke(null, null);
        }

        private static void SceneControl_ContextMenu_Undo(object sender, EventArgs e)
        {
            HotKeyCtlZPressed?.Invoke(null, null);
        }

        private static void SceneControl_ContextMenu_DeleteModel(object sender, EventArgs e)
        {
            HotKeyDelPressed?.Invoke(null, null);
        }

        public static void ShowMenu()
        {
            if (!frmStudioMain.SceneControl.Controls.Contains(_mainFormMenu))
            {
                frmStudioMain.SceneControl.Controls.Add(_mainFormMenu);
            }
            _mainFormMenu.BringToFront();
        }

        public static void CloseMenu()
        {
            if (frmStudioMain.SceneControl.Controls.Contains(_mainFormMenu))
            {
                frmStudioMain.SceneControl.Controls.Remove(_mainFormMenu);
                SceneControlToolbarManager.Redraw(frmStudioMain.SceneControl);
            }
        }

        private static void MainFormDropdownMenu_OnSelectMenuItem(object sender, EventArgs e)
        {
            MainMenuOpened?.Invoke(sender, null);
        }

        public static void ProcesArguments(string[] arguments)
        {
            RegistryManager.GetRegistryProfileSettings();

            if (arguments != null && arguments.Length > 0)
            {
                foreach (var arg in arguments)
                {
                    if (!string.IsNullOrEmpty(arg))
                    {
                        if (arg.ToLower() == "verbose=true" || RegistryManager.RegistryProfile.VerboseMode)
                        {
                            DAL.ApplicationSettings.Settings.VerboseMode = true;
                            DAL.Managers.LoggingManager.Start();
                        }

                        if (arg.ToLower().EndsWith(".apf"))
                        {
                            DroppedProjectFiles.Add(arg);
                            UserProfileManager.UserProfile.AddRecentOpenedFile(new Controls.NewGui.SplashControl.UnlicensedControl.RecentFiles.RecentOpenedFile() { FileName = new FileInfo(arg).Name, FullPath = arg, AccessedDateTime = DateTime.Now });
                            UserProfileManager.Save();
                        }
                        else if (arg.ToLower().EndsWith(".stl") || arg.ToLower().EndsWith(".3mf"))
                        {
                            DroppedSTLFiles.Add(arg);
                            UserProfileManager.UserProfile.AddRecentOpenedFile(new Controls.NewGui.SplashControl.UnlicensedControl.RecentFiles.RecentOpenedFile() { FileName = new FileInfo(arg).Name, FullPath = arg, AccessedDateTime = DateTime.Now });
                            UserProfileManager.Save();
                        }
                    }
                }
            }
        }

        public static bool ProcessKeyPress(ref Message msg, Keys keyData)
        {
            frmStudioMain.SceneControl.ModelContextFormMenu.Close(true);
            frmStudioMain.SceneControl.SupportConeContextFormMenu.Close(true);

            var keyPressHandled = false;

            var shortcutKeys = new List<Keys>()
            {
                Keys.D,
                Keys.F,
                Keys.G,
                Keys.O,
                Keys.M,
                Keys.P,
                Keys.Q,
                Keys.R,
                Keys.S,
                Keys.X,
                Keys.Z,
                Keys.Delete,
                (Keys.Control | Keys.C),
                (Keys.Control | Keys.P),
                (Keys.Control | Keys.Z),
            };

            if (shortcutKeys.Contains(keyData))
            {
                //skip all toolbar controls when they are focussed
                if (SceneControlToolbarManager.PrintJobProperties.IsPrintJobNameFocused)
                {
                    return keyPressHandled;
                }
                else
                {
                    switch (keyData)
                    {

                        case Keys.D:
                            keyPressHandled = true;
                            HotKeyDPressed?.Invoke(null, null);
                            break;
                        case Keys.F:
                            keyPressHandled = true;
                            HotKeyFPressed?.Invoke(null, null);
                            break;
                        case Keys.G:
                            keyPressHandled = true;
                            HotKeyGPressed?.Invoke(null, null);
                            break;
                        case Keys.M:
                            keyPressHandled = true;
                            HotKeyMPressed?.Invoke(null, null);
                            break;
                        case Keys.O:
                            keyPressHandled = true;
                            HotKeyOPressed?.Invoke(null, null);
                            break;
                        case Keys.P:
                            keyPressHandled = true;
                            HotKeyPPressed?.Invoke(null, null);
                            break;
                        case Keys.Q:
                            keyPressHandled = true;
                            HotKeyQPressed?.Invoke(null, null);
                            break;
                        case Keys.R:
                            keyPressHandled = true;
                            HotKeyRPressed?.Invoke(null, null);
                            break;
                        case Keys.S:
                            keyPressHandled = true;
                            HotKeySPressed?.Invoke(null, null);
                            break;
                        case Keys.X:
                            keyPressHandled = true;
                            HotKeyXPressed?.Invoke(null, null);
                            break;
                        case Keys.Z:
                            keyPressHandled = true;
                            HotKeyZPressed?.Invoke(null, null);
                            break;
                        case Keys.Delete:
                            if (!IsInExportMode && !SceneActionControlManager.IsSupportPropertiesVisible)
                            {
                                keyPressHandled = true;
                                HotKeyDelPressed?.Invoke(null, null);
                            }
                            break;
                        case (Keys.Control | Keys.C):
                            keyPressHandled = true;
                            HotKeyCtlCPressed?.Invoke(null, null);
                            break;
                        case (Keys.Control | Keys.P):
                            keyPressHandled = true;
                            HotKeyCtlPPressed?.Invoke(null, null);
                            break;
                        case (Keys.Control | Keys.Z):
                            keyPressHandled = true;
                            HotKeyCtlZPressed?.Invoke(null, null);
                            break;

                    }
                }
            }

            return keyPressHandled;
        }

        static void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        static void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var validSTLFile = false;
            var validAPFFile = false;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                DAL.Managers.LoggingManager.WriteToLog("Main Form Manager", "Draw/drop completed", file);

                if (file.ToLower().EndsWith("stl"))
                {
                    validSTLFile = true;
                    DroppedSTLFiles.Add(file);
                }
                else if (file.ToLower().EndsWith("apf"))
                {
                    validAPFFile = true;
                    DroppedProjectFiles.Add(file);
                }
            }

            if (validSTLFile)
            {
                if (!ProcessingDragDrop)
                {
                    DragDropSTLFile?.Invoke();
                }
            }

            if (validAPFFile)
            {
                if (!ProcessingDragDrop)
                {
                    DragDropAPFFile?.Invoke();
                }
            }
        }


    }
}
