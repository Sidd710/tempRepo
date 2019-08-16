using System;
using System.Collections.Generic;
using System.Text;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Models;
using System.Windows.Forms;
using System.IO;
using Atum.Studio.Core.Shapes;
using System.Linq;
using System.Drawing;
using Atum.Studio.Core.Engines.MagsAI;
using Atum.Studio.Controls.OpenGL;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Core.Engines
{
    [Serializable]
    public class WorkspaceFile
    {
        public enum VersionType
        {
            Version1 = 0,
            Version2 = 1,
        }

        public string ApplicationVersion { get; set; }

        public List<WorkspaceSTLModel> Objects = new List<WorkspaceSTLModel>();
        public DAL.Materials.MaterialsCatalog CurrentMaterialCatalog { get; set; }
        public string PrintJobName = string.Empty;
        public VersionType Version { get; set; }

        public List<MAGSAIDebugComment> MAGSAIDebugComments = new List<MAGSAIDebugComment>();
    }

    internal class ExportEngine
    {
        internal static Action<int, int> ExportToAPF_Progress;
        internal static Action ExportToAPF_Completed;

        private static int CurrentProgress = 0;
        private static int MaxValue = 0;

        internal static void ExportCurrentWorkspace(bool autosaveMode = false, string autoSavePath = null, List<MAGSAIDebugComment> magsAIComments = null)
        {
            var filePath = autoSavePath;
            if (filePath == null)
            {
                filePath = PrintJobManager.ProjectSaveFilePath;
            }
            var parentDirectory = new FileInfo(filePath).Directory.FullName;
            if (!Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }


            //save
            var modelCount = 0;
            foreach (var model in ObjectView.Objects3D)
            {
                if (model != null && !(model is GroundPane))
                {
                    var stlModel = ((STLModel3D)model);
                    modelCount++;
                    modelCount += stlModel.TotalObjectSupportCones.Count;
                    if (!autosaveMode) stlModel.ExportingToAPFProgress += StlModel_ExportingToAPFProgress;
                }
            }

            MaxValue = modelCount;
            CurrentProgress = 0;

            if (!autosaveMode) ExportToAPF_Progress?.Invoke(0, modelCount);

            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(WorkspaceFile));
                using (var streamWriter = new StreamWriter(filePath, false))
                {
                    var exportFile = new WorkspaceFile();
                    exportFile.ApplicationVersion = BrandingManager.ApplicationVersion;
                    exportFile.Version = WorkspaceFile.VersionType.Version2;
                    foreach (var object3d in ObjectView.Objects3D)
                    {
                        if (object3d is STLModel3D && !(object3d is GroundPane))
                        {
                            var stlModel = object3d as STLModel3D;
                            exportFile.Objects.Add(stlModel.ExportToXMLFile());
                            if (!autosaveMode) stlModel.ExportingToAPFProgress -= StlModel_ExportingToAPFProgress;
                        }
                    }


                    //export material settings
                 //   exportFile.CurrentMaterialCatalog = SceneControlPrintJobPropertiesToolbar.SelectedMaterialCatalog;
                    exportFile.PrintJobName = PrintJobManager.PrintjobName;

                    if (magsAIComments != null)
                    {
                        var imageConverter = new ImageConverter();
                        foreach (var magsAIComment in magsAIComments)
                        {
                            exportFile.MAGSAIDebugComments.Add(magsAIComment);
                        }
                    }


                    serializer.Serialize(streamWriter, exportFile);


                    if (!autosaveMode)
                    {
                        ExportToAPF_Completed?.Invoke();
                        UserProfileManager.UserProfile.AddRecentOpenedFile(new Controls.NewGui.SplashControl.UnlicensedControl.RecentFiles.RecentOpenedFile() { FileName = new FileInfo(filePath).Name, FullPath = filePath, AccessedDateTime = DateTime.Now });
                        UserProfileManager.Save();
                    }
                }
            }
            catch (IOException exc)
            {
                //MessageBox.Show(exc.Message);
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.StackTrace);
            }
        }

        internal static void ExportSelectedModelProperties(string filePath, int modelIndex)
        {
            //save
            try
            {
                var parentDirectory = new FileInfo(filePath).Directory.FullName;
                if (!Directory.Exists(parentDirectory))
                {
                    Directory.CreateDirectory(parentDirectory);
                }

                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(WorkspaceFile));
                using (var streamWriter = new StreamWriter(filePath, false))
                {
                    var stlModel = ((STLModel3D)ObjectView.Objects3D.First(s => s != null && (s is STLModel3D) && (s as STLModel3D).Index == modelIndex));
                    var exportFile = new WorkspaceFile();
                    exportFile.Version = WorkspaceFile.VersionType.Version2;
                    exportFile.Objects.Add(stlModel.ExportToXMLFile());
                    serializer.Serialize(streamWriter, exportFile);
                }
            }
            catch (IOException exc)
            {
                //MessageBox.Show(exc.Message);
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.StackTrace);
            }
        }

        private static void StlModel_ExportingToAPFProgress(object sender, EventArgs e)
        {
            CurrentProgress++;
            ExportToAPF_Progress?.Invoke(CurrentProgress, MaxValue);
        }

        public static void SaveSTL_Binary(string fileName, long numberOfTriangles, List<Triangle> triangles)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
            {

                // write header
                var headerTextArray = ASCIIEncoding.Default.GetBytes("AtumStudio\r\n");
                byte[] data = new byte[80];
                headerTextArray.CopyTo(data, 0);
                bw.Write(data);

                //triangles amount
                bw.Write(Convert.ToInt32(numberOfTriangles));

                //triangles
                foreach (var triangle in triangles)
                {
                    //write normal
                    bw.Write(triangle.Normal.X);
                    bw.Write(triangle.Normal.Y);
                    bw.Write(triangle.Normal.Z);

                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        bw.Write(triangle.Vectors[vectorIndex].Position.X);
                        bw.Write(triangle.Vectors[vectorIndex].Position.Y);
                        bw.Write(triangle.Vectors[vectorIndex].Position.Z);
                    }

                    //export attribute
                    bw.Write(Convert.ToInt16(0));
                }
            }
        }
    }
}
