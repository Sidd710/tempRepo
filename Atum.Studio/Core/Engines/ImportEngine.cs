using System.Collections.Generic;
using System.IO;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using System.Runtime.Serialization.Formatters.Binary;
using Atum.Studio.Core.Shapes;
using System.Threading;
using Atum.Studio.Core.Managers;
using System.ComponentModel;
using System.Diagnostics;
using Atum.Studio.Controls.OpenGL;
using System.Linq;
using Atum.Studio.Core.Structs;
using System;

namespace Atum.Studio.Core.Engines
{
    internal class ImportEngine
    {
        internal static void ImportWorkspaceFileAsync(Form mainForm, string argumentFileName = null)
        {
            ProgressBarManager.UpdateMainPercentage(5);
            ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);

            if (argumentFileName == null)
            {
                using (var popup = new OpenFileDialog())
                {
                    popup.Filter = "(*.apf)|*.apf";
                    popup.Multiselect = false;
                    if (popup.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(popup.FileName))
                    {
                        if (File.Exists(popup.FileName))
                        {
                            var fileName = popup.FileName;
                            var backgroundWorker = new BackgroundWorker();
                            List<object> arguments = new List<object>() {  fileName };
                            UserProfileManager.UserProfile.AddRecentOpenedFile(new Controls.NewGui.SplashControl.UnlicensedControl.RecentFiles.RecentOpenedFile() { FileName = new FileInfo(popup.FileName).Name, FullPath = popup.FileName, AccessedDateTime = DateTime.Now });
                            UserProfileManager.Save();

                            backgroundWorker.DoWork += new DoWorkEventHandler(ImportWorkspaceFile);
                            backgroundWorker.RunWorkerAsync(arguments);
                        }
                    }
                }
            }
            else
            {
                var backgroundWorker = new BackgroundWorker();
                List<object> arguments = new List<object>() { argumentFileName };

                backgroundWorker.DoWork += new DoWorkEventHandler(ImportWorkspaceFile);
                backgroundWorker.RunWorkerCompleted += ImportWorkspaceFile_RunWorkerCompleted;
                backgroundWorker.RunWorkerAsync(arguments);
            }
        }

        private static void ImportWorkspaceFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            frmStudioMain.SceneControl.EnableRendering();
            frmStudioMain.SceneControl.Render();
        }

        internal static STLModel3D ImportModelUndoFileAsync(string argumentFileName, int modelIndex)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(WorkspaceFile));

            using (var streamReader = new StreamReader(argumentFileName))
            {
                var workspaceFile = (WorkspaceFile)serializer.Deserialize(streamReader);

                foreach (var object3d in workspaceFile.Objects)
                {
                    return new STLModel3D(STLModel3D.TypeObject.Model, ObjectView.BindingSupported, object3d, modelIndex, true);
                }
            }

            return null;
        }

        private static void ImportWorkspaceFile(object sender, DoWorkEventArgs args)
        {
            var fileName = ((List<object>)args.Argument)[0] as string;
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(WorkspaceFile));
            WorkspaceFile workspaceFile = null;

            frmStudioMain.SceneControl.DisableRendering();

            using (var streamReader = new StreamReader(fileName))
            {
                workspaceFile = (WorkspaceFile)serializer.Deserialize(streamReader);

                foreach (var object3d in workspaceFile.Objects)
                {
                    var stlModel = new STLModel3D(STLModel3D.TypeObject.Model, ObjectView.BindingSupported, object3d, ObjectView.NextObjectIndex, false);

                    stlModel.Triangles.UpdateSelectedOrientationTriangles(stlModel);
                    stlModel.UpdateBoundries();
                }

                if (!string.IsNullOrEmpty(PrintJobManager.PrintjobName)) { PrintJobManager.PrintjobName = workspaceFile.PrintJobName; }
            }
        }


        public static void AddManualSupportCone(float totalHeight, float topHeight, float topRadius, float middleRadius, float bottomHeight, float bottomRadius,
            int slicesCount, Vector3Class object3dPosition, Vector3Class position, Color color, STLModel3D stlModel, Vector3Class moveTranslation,
            float rotationAngleX, float rotationAngleZ, bool groundSupport, bool useSupportPenetration = false, float supportBottomWidthCorrection = 0f)
        {
            var supportCone = new SupportCone(totalHeight, topHeight, topRadius, middleRadius, bottomHeight, bottomRadius, slicesCount, object3dPosition, color, rotationAngleX, rotationAngleZ, stlModel, 
                groundSupport: groundSupport, useSupportPenetration: useSupportPenetration, bottomWidthCorrection: supportBottomWidthCorrection);
            supportCone.Selected = false;
            supportCone.CreationBy = SupportCone.typeCreationBy.Manual;
            supportCone.MoveTranslation = moveTranslation;
            supportCone.UpdateBoundries();

            if (groundSupport && !stlModel.SupportBasement)
            {
                supportCone.MoveTranslation = new Vector3Class(supportCone.MoveTranslation.X, supportCone.MoveTranslation.Y, 0);
            }

            lock (stlModel.SupportStructureLock)
            {
                stlModel.SupportStructure.Add(supportCone);
            }
        }

        public static void AddGridSupportCone(float totalHeight, float topHeight, float topRadius, float middleRadius, float bottomHeight, float bottomRadius,
            int slicesCount, Vector3 object3dPosition, Vector3 position, Color color, STLModel3D stlModel, Vector3 moveTranslation,
            float rotationAngleX, float rotationAngleZ, bool groundSupport, TriangleSurfaceInfo surface, bool useSupportPenetration = false, float supportBottomWidthCorrection = 0f)
        {
            var supportCone = new SupportCone(totalHeight, topHeight, topRadius, middleRadius, bottomHeight, bottomRadius, slicesCount, new Vector3Class( object3dPosition), color, rotationAngleX, rotationAngleZ, stlModel, 
                groundSupport: groundSupport, 
                useSupportPenetration: useSupportPenetration,
                bottomWidthCorrection: supportBottomWidthCorrection
                );
            supportCone.CreationBy = SupportCone.typeCreationBy.Manual;
            supportCone.Selected = false;
            supportCone.MoveTranslation = new Vector3Class( moveTranslation);
            supportCone.UpdateBoundries();
            lock (surface.SupportStructure)
            {
                surface.SupportStructure.Add(supportCone);
            }
        }

        private static bool WaitForAll(ManualResetEvent[] events)
        {
            bool result = false;
            try
            {
                if (events != null)
                {
                    for (int i = 0; i < events.Length; i++)
                    {
                        events[i].WaitOne();
                    }
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
