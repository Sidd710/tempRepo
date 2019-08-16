using System;
using System.Collections.Generic;
using Atum.DAL.Compression.Zip;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Managers;
using System.Drawing;
using OpenTK;
using Atum.DAL.Hardware;
using Atum.DAL.Managers;
using Atum.DAL.ApplicationSettings;
using static Atum.Studio.Core.Helpers.ContourHelper;
using Atum.Studio.Core.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Events;
using System.Reflection;

namespace Atum.Studio.Core.Engines
{
    public class RenderEngine
    {

        private static ZipOutputStream _zipStream = null;
        private static FileStream _tempFileStream = null;

        public static long TotalProcessedSlices { get; set; }
        public static long TotalAmountSlices { get; set; }
        public static DAL.Print.PrintJob PrintJob { get; set; }

        public static bool _cancelRendering { get; set; }
        internal static event EventHandler RenderToPrintjobCanceled;
        internal static event EventHandler RenderToPrintjobCompleted;
        internal static event EventHandler RenderToCalibrationjobCompleted;

        private static List<STLModel3D> _createdClones = new List<STLModel3D>();
        private static List<STLModel3D> _originalModelsWithLinkedClones = new List<STLModel3D>();


        private static Bitmap _renderBitmap;
        private static Bitmap _renderBitmapXYSmoothing;
        private static SortedList<int, int> _emptyPNGs;

        private readonly static object _totalProcessedSlicesLock = new object();

        private static Stopwatch _stopWatch = new Stopwatch();
        internal static long TotatAmountofActivePixel { get; set; }


        internal RenderEngine()
        {
        }

        internal static void RenderAsync()
        {
            TotalProcessedSlices = 0;
            TotalAmountSlices = 1;
            TotatAmountofActivePixel = 0;

            _emptyPNGs = new SortedList<int, int>();
            var bwRenderEngine = new BackgroundWorker();
            bwRenderEngine.DoWork += bwRenderEngine_DoWork;
            bwRenderEngine.RunWorkerAsync();
        }

        static void bwRenderEngine_DoWork(object sender, DoWorkEventArgs e)
        {
            PreRender();

            if (!_cancelRendering)
            {
                Render();
            }
        }

        public static void PreRender()
        {
            _cancelRendering = false;
            PrintJob.PostRenderCompleted = false;
            PrintJob.SelectedPrinter = RenderEngine.PrintJob.SelectedPrinter;

            TotalProcessedSlices = 0;
            //copy org vectors to undopoints
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (object3d is STLModel3D)
                {
                    var stlModel = object3d as STLModel3D;
                    stlModel.CreateTriangleSnapshotPoints();
                }
            }


            // Create model clones used only for rendering
            CreateModelClones();

            PrintJob.ApplicationVersion = BrandingManager.ApplicationVersion;
            PrintJob.TotalPrintVolume = 0f;

            //add volume
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (object3d is STLModel3D && !(object3d is GroundPane))
                {
                    var stlModel = (STLModel3D)object3d;
                    stlModel.PreviousScaleFactorX = stlModel.ScaleFactorX;
                    stlModel.PreviousScaleFactorY = stlModel.ScaleFactorY;
                    stlModel.PreviousScaleFactorZ = stlModel.ScaleFactorZ;

                }
            }

            //combine movetranslation to triangles
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (object3d is STLModel3D && (!(object3d is GroundPane)))
                {
                    var stlModel = object3d as STLModel3D;
                    var stlModelXYmove = new Vector3Class(stlModel.MoveTranslation.X, stlModel.MoveTranslation.Y, 0);
                    stlModel.Triangles.UpdateWithMoveTranslation(stlModelXYmove);


                    foreach (var supportCone in stlModel.TotalObjectSupportCones)
                    {
                        if (supportCone != null)
                        {
                            if (supportCone is SupportConeV2)
                            {
                                var supportConeV2 = supportCone as SupportConeV2;
                                supportConeV2.Triangles.UpdateWithMoveTranslation(supportCone.MoveTranslation + stlModelXYmove);
                                // supportConeV2.CalcSlicesContours(PrintJob.Material, PrintJob.SelectedPrinter);
                            }
                            else
                            {
                                supportCone.Triangles.UpdateWithMoveTranslation(supportCone.MoveTranslation + stlModelXYmove);
                            }
                        }
                    }

                    //support basement
                    if (stlModel.SupportBasementStructure != null)
                    {
                        stlModel.SupportBasementStructure.Triangles.UpdateWithMoveTranslation(stlModelXYmove);
                    }
                }
            }

            ////add trapezium correction
            //render autosupport to scaled model

            var stopwatch = new Stopwatch();
            stopwatch.Start();


            //mirror must use movetranslation otherwise model intersections can occur
            if (!UserProfileManager.UserProfiles[0].Settings_PrintJob_MirrorObjects)
            {
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && !(object3d is GroundPane) && !(object3d is Models.Defaults.BasicCorrectionModel))
                    {
                        var stlModel = object3d as STLModel3D;
                        stlModel.HorizontalMirror(false, false);
                    }
                }
            }

            ////add trapezium correction and shrinkfactor
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (object3d is STLModel3D && (!(object3d is GroundPane)))
                {
                    var stlModel = (STLModel3D)object3d;
                    ModelCorrections.Trapezoid.Calculate(stlModel, PrintJob.Material, stlModel.MoveTranslation);
                    Debug.WriteLine("Calc Trapezoid: " + stopwatch.ElapsedMilliseconds + "ms");

                    stlModel.CalcSliceIndexes(PrintJob.Material, true);

                    foreach (var supportConeV2 in stlModel.TotalObjectSupportCones.OfType<SupportConeV2>())
                    {
                        supportConeV2.CalcSlicesContours(PrintJob.Material, PrintJob.SelectedPrinter);
                    }
                }
            }

            Debug.WriteLine("Total models pre processing: " + stopwatch.ElapsedMilliseconds + "ms");
        }

        internal static List<Task> _asyncTasks = new List<Task>();

        public static object Render()
        {
            _asyncTasks.Clear();

            TotalProcessedSlices = 0;
            TotatAmountofActivePixel = 0;
            //define zip and bitmap stream
            _emptyPNGs = new SortedList<int, int>();
            pixelValues = new byte[RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX * RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionY * 3];
            pixelBleedingValues = new byte[RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX * RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionY * 3];
            _renderBitmap = new Bitmap(RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX, RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionY, PixelFormat.Format24bppRgb);
            _renderBitmapXYSmoothing = (Bitmap)_renderBitmap.Clone();

            if (_zipStream == null || _zipStream.IsFinished)
            {
                //remove diacritics from filepath
                PrintJob.SlicesPath = Path.Combine(Settings.RenderEnginePrintjobsPath, Helpers.StringHelper.RemoveDiacritics(PrintJob.Name));
                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.BasePath);
                if (!hasLocalWriteAccess)
                {
                    PrintJob.SlicesPath = Path.Combine(Settings.RenderEngineRoamingPrintjobsPath, Helpers.StringHelper.RemoveDiacritics(PrintJob.Name));
                }

                if (!Directory.Exists(PrintJob.SlicesPath)) Directory.CreateDirectory(PrintJob.SlicesPath);
                _tempFileStream = new FileStream(Path.Combine(PrintJob.SlicesPath, "slices.zip"), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                _zipStream = new ZipOutputStream(_tempFileStream);

                //checksum file
                var checksumFilePath = Path.Combine(PrintJob.SlicesPath, "checksum.crc");
                if (File.Exists(checksumFilePath)) { File.Delete(checksumFilePath); }
                using (FileStream checksumFile = new FileStream(checksumFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {

                }
            }

            try
            {
                _stopWatch = new Stopwatch();
                _stopWatch.Start();


                //var sliceIndex = 0;
                var sliceCount = 0;

                //update triangles with min/max z
                var topPoint = 0f;
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && !(object3d is Core.Models.Defaults.BasicCorrectionModel))
                    {
                        var stlModel = (STLModel3D)object3d;
                        // if (UserProfileManager.UserProfiles[0].Settings_PrintJob_MirrorObjects) stlModel.Mirror. (false, false);
                        stlModel.UpdateTrianglesMinMaxZ();
                        if (stlModel.TopPoint > topPoint) { topPoint = stlModel.TopPoint; }
                    }
                }

                //
                //determine slicecount
                var initialLayersHeight = 0f;
                for (var initialLayerIndex = 1; initialLayerIndex < PrintJob.Material.InitialLayers + 1; initialLayerIndex++)
                {
                    initialLayersHeight = (float)PrintJob.Material.LT1 * initialLayerIndex;

                    sliceCount++;

                    if (initialLayersHeight > topPoint)
                    {
                        break;
                    }
                }

                if (initialLayersHeight < topPoint)
                {
                    var remainingLayersHeight = topPoint - initialLayersHeight;
                    var remainingLayersCount = (remainingLayersHeight / PrintJob.Material.LT2) + 1;

                    for (var remainingLayerIndex = 0; remainingLayerIndex < remainingLayersCount; remainingLayerIndex++)
                    {
                        sliceCount++;
                    }
                }


                var highestModelIndex = 0;
                TotalAmountSlices = 0;
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && (!(object3d is GroundPane)))
                    {
                        var stlModel = object3d as STLModel3D;
                        //  stlModel.Triangles.UpdateConnections();
                        stlModel.CalcSliceIndexes(PrintJob.Material, true);
                        if (stlModel.SliceIndexes.Count >= TotalAmountSlices)
                        {
                            TotalAmountSlices = stlModel.SliceIndexes.Count;
                            highestModelIndex = stlModel.Index;
                        }
                    }
                }

                var currentSliceIndex = 0;
                if (highestModelIndex > 0 && TotalAmountSlices > 0)
                {
                    SortedDictionary<float, List<TriangleConnectionInfo>> stlModelIndexes = null;
                    foreach (var object3d in ObjectView.Objects3D)
                    {
                        if (object3d is STLModel3D && (!(object3d is GroundPane)))
                        {
                            var stlModel = object3d as STLModel3D;
                            if (stlModel.Index == highestModelIndex)
                            {
                                stlModelIndexes = stlModel.SliceIndexes;
                                break;
                            }
                        }
                    }

                    foreach (var sliceHeight in stlModelIndexes.Keys)
                    {
                        if (!_cancelRendering)
                        {
                            var sliceIndexAsync = currentSliceIndex;
                            var sliceInfo = new Slices.Slice(sliceIndexAsync, new List<SlicePolyLine3D>(), new List<List<SlicePolyLine3D>>(), new List<SlicePolyLine3D>(), sliceHeight);

                            if (PerformanceSettingsManager.Settings.PrintJobGenerationMultiThreading && sliceInfo.SliceIndex > 0)
                            {
                                _asyncTasks.Add(Task.Run(() => RenderSliceThread(sliceInfo)));
                            }
                            else
                            {
                                RenderSliceThread(sliceInfo);
                            }
                        }

                        currentSliceIndex++;
                    }
                }

                //cleanjob image
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            return null;
        }

        private static void PostRenderAfterCanceling()
        {
            PostRender(false);
        }

        private static void CreateModelClones()
        {
            // Create a current list of models, because models will be added to the list during the iteration
            var models = ObjectView.Objects3D.OfType<STLModel3D>().ToList();
            foreach (var model in models)
            {
                if (model.LinkedClones != null && model.LinkedClones.Count > 0)
                {
                    _originalModelsWithLinkedClones.Add(model);
                    // model.UnbindModel();
                    ObjectView.Objects3D.Remove(model);
                }

                foreach (var linkedClone in model.LinkedClones)
                {
                    var clonedModel = model.Clone(true) as STLModel3D;
                    clonedModel.MoveTranslation = linkedClone.Translation;
                    if (linkedClone.Rotate)
                    {
                        clonedModel._rotationAngleZ = 0;
                        clonedModel.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z);
                    }
                    // clonedModel.BindModel();
                    _createdClones.Add(clonedModel);
                    ObjectView.Objects3D.Add(clonedModel);
                }
            }
        }

        private static void RemoveModelClones()
        {
            foreach (var modelClone in _createdClones)
            {
                ObjectView.Objects3D.Remove(modelClone);
            }
            _createdClones.Clear();

            foreach (var originalModel in _originalModelsWithLinkedClones)
            {
                ObjectView.Objects3D.Add(originalModel);
            }
            _originalModelsWithLinkedClones.Clear();
        }

        internal static void Cancel()
        {
            _cancelRendering = true;

            Task.WaitAll(_asyncTasks.ToArray());

            PostRenderAfterCanceling();
            TotalAmountSlices = TotalProcessedSlices;
        }

        private static int RenderSliceThread(object sliceInfoObject)
        {
            var sliceInfo = (Slices.Slice)sliceInfoObject;

            try
            {
                if (PerformanceSettingsManager.Settings.PrintJobGenerationMaxMemoryLimitEnabled)
                {
                    while ((Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) > Managers.PerformanceSettingsManager.Settings.PrintJobGenerationMaxMemory)
                    {
                        Thread.Sleep(1000);
                        GC.Collect();
                        GC.Collect();

                        LoggingManager.WriteToLog("Render Engine", "Memory management", "Waiting for free memory");
                    }
                }

                var modelsToSliceCount = 0;

                foreach (var object3d in ObjectView.Objects3D)
                {
                    var supportZPolys = new List<List<SlicePolyLine3D>>();

                    if (object3d is STLModel3D && (!(object3d is GroundPane)))
                    {
                        var stlModel = (STLModel3D)object3d;
                        modelsToSliceCount++;
                    }
                }

                Slices.Slice slice = new Slices.Slice(sliceInfo.SliceIndex, new List<SlicePolyLine3D>(), new List<List<SlicePolyLine3D>>(), new List<SlicePolyLine3D>(), sliceInfo.SliceHeight);
                slice.ModelPolyTrees = new List<PolyTree>[modelsToSliceCount];
                slice.ModelBleedingPolyTrees = new List<PolyTree>[modelsToSliceCount];
                slice.SupportPolyTrees = new List<PolyTree>[modelsToSliceCount];

                var currentModelIndex = 0;
                foreach (var object3d in ObjectView.Objects3D)
                {

                    var supportZPolys = new List<List<SlicePolyLine3D>>();

                    if (object3d is STLModel3D && (!(object3d is GroundPane)))
                    {
                        var stlModel = (STLModel3D)object3d;
                        if (!_cancelRendering)
                        {
                            foreach (var supportCone in stlModel.TotalObjectSupportCones)
                            {
                                if (supportCone != null)
                                {
                                    if (supportCone is SupportConeV2)
                                    {

                                    }
                                    else
                                    {
                                        if (!supportCone.Hidden)
                                        {
                                            supportZPolys.Add(GetZIntersectionsSupport(supportCone, sliceInfo.SliceHeight));
                                        }
                                    }
                                }
                            }

                            if (sliceInfo.SliceHeight <= (UserProfileManager.UserProfile.SupportEngine_Basement_Thickness * PrintJob.Material.ShrinkFactor) && stlModel.SupportBasement && stlModel.SupportBasementStructure != null)
                            {
                                foreach (var triangle in stlModel.SupportBasementStructure.Triangles[0])
                                {
                                    triangle.CalcMinMaxZ();
                                }

                                var supportBasementZPolys = GetZPolys(stlModel.SupportBasementStructure, stlModel.SupportBasementStructure.SliceIndexes, sliceInfo.SliceHeight);
                                supportZPolys.Add(GetZIntersections(supportBasementZPolys, sliceInfo.SliceHeight));
                            }
                        }

                        List<SlicePolyLine3D> modelZPolys = null;
                        List<SlicePolyLine3D> modelBleedingZPolys = null;
                        if (!_cancelRendering)
                        {
                            modelZPolys = GetZIntersections(GetZPolys(stlModel, stlModel.SliceIndexes, sliceInfo.SliceHeight), sliceInfo.SliceHeight);

                            //post processing plugins
                            foreach (var plugin in PluginManager.LoadedPlugins)
                            {
                                if (plugin.HasPostSliceMethod &&
                                    (plugin.PostSliceActionType & Plugins.PluginTypes.PostSliceActionType.Bleeding) == Plugins.PluginTypes.PostSliceActionType.Bleeding &&
                                    sliceInfo.SliceHeight > PrintJob.Material.BleedingOffset && PrintJob.Material.BleedingOffset > 0f
                                    )
                                {
                                    modelBleedingZPolys = plugin.PostSlice(stlModel, sliceInfo.SliceHeight + (float)PrintJob.Material.BleedingOffset, Plugins.PluginTypes.PostSliceActionType.Bleeding);
                                }
                            }
                        }

                        if (!_cancelRendering)
                        {
                            using (var modelWithSupportSlice = new Slices.Slice(sliceInfo.SliceIndex, modelZPolys, supportZPolys, modelBleedingZPolys, sliceInfo.SliceHeight))
                            {
                                modelWithSupportSlice.ConvertModelIntersectionsToPolyTrees(sliceInfo.SliceHeight, RenderEngine.PrintJob.SelectedPrinter,
                                    PrintJobManager.CurrentPrintJobSettings.Material, 
                                    PrintJobManager.CurrentPrintJobSettings.Material.SupportProfiles.First(),
                                     null, null);
                                modelWithSupportSlice.ConvertSupportIntersectionsToPolyTrees(sliceInfo.SliceHeight, RenderEngine.PrintJob.SelectedPrinter);

                                slice.ModelPolyTrees[currentModelIndex] = new List<PolyTree>();
                                slice.ModelPolyTrees[currentModelIndex].AddRange(modelWithSupportSlice.ModelPolyTrees[0]);

                                slice.SupportPolyTrees[currentModelIndex] = new List<PolyTree>();
                                slice.SupportPolyTrees[currentModelIndex].AddRange(modelWithSupportSlice.SupportPolyTrees[0]);

                                foreach (var supportCone in stlModel.TotalObjectSupportCones)
                                {
                                    if (supportCone is SupportConeV2)
                                    {
                                        var supportConeV2 = (SupportConeV2)supportCone;
                                        if (supportConeV2.SliceContours.ContainsKey(sliceInfo.SliceHeight))
                                        {
                                            slice.SupportPolyTrees[currentModelIndex].Add(supportConeV2.SliceContours[sliceInfo.SliceHeight]);

                                            foreach (var interlinkConnectionIndex in supportConeV2.InterlinkConnections)
                                            {
                                                if (interlinkConnectionIndex.Value != null)
                                                {
                                                    foreach (var interlinkConnection in interlinkConnectionIndex.Value)
                                                    {
                                                        if (interlinkConnection != null && interlinkConnection.SliceContours != null && interlinkConnection.SliceContours.ContainsKey(sliceInfo.SliceHeight))
                                                        {
                                                            slice.SupportPolyTrees[currentModelIndex].Add(interlinkConnection.SliceContours[sliceInfo.SliceHeight]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (modelBleedingZPolys != null && modelBleedingZPolys.Count > 0)
                                {
                                    if (sliceInfo.SliceHeight < 1f)
                                    {

                                    }

                                    modelWithSupportSlice.ConvertModelBleedingIntersectionsToPolyTrees(sliceInfo.SliceHeight, RenderEngine.PrintJob.SelectedPrinter);
                                    slice.ModelBleedingPolyTrees[currentModelIndex] = new List<PolyTree>();
                                    slice.ModelBleedingPolyTrees[currentModelIndex].AddRange(modelWithSupportSlice.ModelBleedingPolyTrees[0]);
                                }
                            }
                        }

                        currentModelIndex++;
                    }
                }

                //output to printjob
                if (!_cancelRendering)
                {
                    var validSlice = RenderSliceToPNG(slice);

                    if (PrintJob.Material.XYSmoothingEnabled)
                    {
                        var xySmoothingSlice = RenderSliceToXYSmootingPNG(slice);
                    }

                    
                    if (sliceInfo.SliceIndex % 50 == 0)
                    {
                        MemoryHelpers.ForceGCCleanup();
                    }

                    lock (_totalProcessedSlicesLock)
                    {
                        TotalProcessedSlices++;
                    }

                    if (TotalProcessedSlices == TotalAmountSlices)
                    {
                        #region Calculate Volume

                        var layerThickness = RenderEngine.PrintJob.Material.LT2;
                        var currentPixelVolume = Math.Pow(RenderEngine.PrintJob.SelectedPrinter.PrinterXYResolutionAsInt / 1000f, 2);
                        RenderEngine.PrintJob.TotalPrintVolume = (float)Math.Ceiling((decimal)(currentPixelVolume * (layerThickness / 1000f) * RenderEngine.TotatAmountofActivePixel));

                        #endregion

                        Debug.WriteLine("Total slices with white pixels: " + (TotalAmountSlices - _emptyPNGs.Count).ToString());
                        PostRender();

                        RenderToPrintjobCompleted?.Invoke(PrintJob, null);

                    }
                }
            }
            catch (Exception exc)
            {

            }

            return sliceInfo.SliceIndex;
        }

        #region Printjob output
        static byte[] pixelValues;
        static byte[] pixelBleedingValues;

        private static long CalculateWhitePixel(Slices.Slice slice)
        {
            var amountOfWhitePixels = RenderEngineHelper.ConvertPolyTreesToPixels(pixelValues, slice.ModelPolyTrees, PrintJob.SelectedPrinter.ProjectorResolutionX, PrintJob.SelectedPrinter.ProjectorResolutionY, true);
            amountOfWhitePixels += RenderEngineHelper.ConvertPolyTreesToPixels(pixelValues, slice.SupportPolyTrees, PrintJob.SelectedPrinter.ProjectorResolutionX, PrintJob.SelectedPrinter.ProjectorResolutionY, false);
            TotatAmountofActivePixel = TotatAmountofActivePixel + amountOfWhitePixels;
            return TotatAmountofActivePixel;
        }

        private static bool RenderSliceToXYSmootingPNG(Slices.Slice slice)
        {
            var result = false;

            //create pic object 
            Bitmap t = null;
            lock (_renderBitmapXYSmoothing)
            {
                t = (Bitmap)_renderBitmapXYSmoothing.Clone();
            }

            var g = Graphics.FromImage(t);

            foreach (var modelPolyTree in slice.ModelPolyTrees)
            {
                if (modelPolyTree.Count > 0)
                {
                    var clipperOffset = new ClipperOffset();
                    foreach (var polyNode in modelPolyTree[0]._allPolys)
                    {
                        clipperOffset.AddPath(polyNode.Contour, JoinType.jtMiter, EndType.etClosedPolygon);
                    }

                    var offsetResult = new PolyTree();
                    clipperOffset.Execute(ref offsetResult, -(CONTOURPOINT_TO_VECTORPOINT_FACTOR / 2));

                    var a = 0;
                    foreach (var polyNode in offsetResult._allPolys)
                    {
                        if (a == 0)
                        {
                            a++;
                            continue;
                        }
                        var pathPoints = new List<PointF>();
                        foreach (var polyNodePoint in polyNode.Contour)
                        {
                            pathPoints.Add(polyNodePoint.AsPointF());
                        }

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.DrawPolygon(new Pen(Color.White, 1), pathPoints.ToArray());
                    }
                }
            }

            using (var memStream = new MemoryStream())
            {
                t.Save(memStream, ImageFormat.Png);
                t.Dispose();
                memStream.Position = 0;

                lock (_zipStream)
                {
                    var entry = new ZipEntry(slice.SliceIndex + "-xys.png");
                    _zipStream.PutNextEntry(entry);

                    try
                    {
                        byte[] transferBuffer = new byte[1024];
                        int bytesRead;
                        do
                        {
                            bytesRead = memStream.Read(transferBuffer, 0, transferBuffer.Length);
                            _zipStream.Write(transferBuffer, 0, bytesRead);
                        }
                        while (bytesRead > 0);
                    }
                    finally
                    {
                        memStream.Close();
                    }

                    _zipStream.CloseEntry();

                    //LoggingManager.WriteToLog("RenderSliceThread", "Zip location", _);

                }
            }

            return result;

        }

        private static bool RenderSliceToPNG(Slices.Slice slice)
        {
            var result = false;

            //create pic object 
            Bitmap t = null;
            lock (_renderBitmap)
            {
                t = (Bitmap)_renderBitmap.Clone();
            }

            lock (pixelValues)
            {
                var pixelLength = pixelValues.Length;
                Array.Clear(pixelValues, 0, pixelLength);
                Array.Clear(pixelBleedingValues, 0, pixelLength);

                //render model points
                var amountOfWhitePixels = CalculateWhitePixel(slice);

                //render bleeding points
                if (PrintJob.Material.BleedingOffset > 0 && slice.SliceIndex > 5)
                {
                    if (slice.ModelBleedingPolyTrees != null && slice.ModelBleedingPolyTrees.Length > 0)
                    {
                        RenderEngineHelper.ConvertPolyTreesToPixels(pixelBleedingValues, slice.ModelBleedingPolyTrees, PrintJob.SelectedPrinter.ProjectorResolutionX, PrintJob.SelectedPrinter.ProjectorResolutionY, true);

                        var emptyBleedingModelPolyTrees = new List<PolyTree>[slice.ModelBleedingPolyTrees.Length];
                        for (var modelBleedingPolyTreeIndex = 0; modelBleedingPolyTreeIndex < slice.ModelBleedingPolyTrees.Length; modelBleedingPolyTreeIndex++)
                        {
                            if (slice.ModelBleedingPolyTrees[modelBleedingPolyTreeIndex] == null)
                            {
                                //process no bleeding information available
                                emptyBleedingModelPolyTrees[modelBleedingPolyTreeIndex] = slice.ModelPolyTrees[modelBleedingPolyTreeIndex];
                            }
                        }

                        //convert empty polygons to bleeding pixels
                        RenderEngineHelper.ConvertPolyTreesToPixels(pixelBleedingValues, emptyBleedingModelPolyTrees, PrintJob.SelectedPrinter.ProjectorResolutionX, PrintJob.SelectedPrinter.ProjectorResolutionY, true, 0);

                        //do bleeding merge model pixel active and bleeding not active then not active
                        for (var modelPixelIndex = 0; modelPixelIndex < pixelLength; modelPixelIndex += 3)
                        {
                            if (pixelValues[modelPixelIndex] == 255 && pixelBleedingValues[modelPixelIndex] == 0)
                            {
                                pixelValues[modelPixelIndex] = 0;
                                pixelValues[modelPixelIndex + 1] = 0;
                                pixelValues[modelPixelIndex + 2] = 0;

                                amountOfWhitePixels -= 1;
                            }
                        }
                    }
                    else
                    {
                        amountOfWhitePixels = 0;
                    }
                }

                //render support points

                if (amountOfWhitePixels > UserProfileManager.UserProfile.Settings_PrintJob_FirstSlice_MinAmountOfPixels)
                {
                    result = true;
                }
                else if (amountOfWhitePixels == 0 && slice.SliceIndex > 5)
                {
                    lock (_emptyPNGs)
                    {
                        _emptyPNGs.Add(slice.SliceIndex, 0);
                    }
                }

                //Get a reference to the images pixel data
                BitmapData picData = t.LockBits(new Rectangle(0, 0, PrintJob.SelectedPrinter.ProjectorResolutionX, PrintJob.SelectedPrinter.ProjectorResolutionY), ImageLockMode.WriteOnly, t.PixelFormat);
                IntPtr pixelStartAddress = picData.Scan0;

                //Copy the pixel data into the bitmap structure
                System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, pixelStartAddress, pixelValues.Length);
                t.UnlockBits(picData);
            }

            try
            {

                if (!_emptyPNGs.ContainsKey(slice.SliceIndex))
                {
                    using (var memStream = new MemoryStream())
                    {
                        //           t.Save("Slices\\" + slice.SliceIndex + ".png");
                        t.Save(memStream, ImageFormat.Png);
                        t.Dispose();
                        memStream.Position = 0;

                        lock (_zipStream)
                        {
                            var entry = new ZipEntry(slice.SliceIndex + ".png");
                            _zipStream.PutNextEntry(entry);

                            try
                            {
                                byte[] transferBuffer = new byte[1024];
                                int bytesRead;
                                do
                                {
                                    bytesRead = memStream.Read(transferBuffer, 0, transferBuffer.Length);
                                    _zipStream.Write(transferBuffer, 0, bytesRead);
                                }
                                while (bytesRead > 0);
                            }
                            finally
                            {
                                memStream.Close();
                            }

                            _zipStream.CloseEntry();

                            //LoggingManager.WriteToLog("RenderSliceThread", "Zip location", _);

                        }
                    }
                }

            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("RenderSliceThread", "Save image (exception)", exc.Message);
            }

            return result;

        }

        internal static void PostRender(bool saveData = true)
        {
            if (saveData)
            {
                if (PrintJob.SelectedPrinter is AtumV20Printer || PrintJob.SelectedPrinter is AtumV15Printer)
                {
                    PrintJob.SelectedPrinter = new AtumV15Printer()
                    {
                        CorrectionFactorX = PrintJob.SelectedPrinter.CorrectionFactorX,
                        CorrectionFactorY = PrintJob.SelectedPrinter.CorrectionFactorY,
                        Description = PrintJob.SelectedPrinter.Description,
                        DisplayName = PrintJob.SelectedPrinter.DisplayName,
                        PrinterXYResolution = PrintJob.SelectedPrinter.PrinterXYResolution,
                        Projectors = PrintJob.SelectedPrinter.Projectors,
                        Properties = PrintJob.SelectedPrinter.Properties,
                        TrapeziumCorrectionInputA = PrintJob.SelectedPrinter.TrapeziumCorrectionInputA,
                        TrapeziumCorrectionInputB = PrintJob.SelectedPrinter.TrapeziumCorrectionInputB,
                        TrapeziumCorrectionInputC = PrintJob.SelectedPrinter.TrapeziumCorrectionInputC,
                        TrapeziumCorrectionInputD = PrintJob.SelectedPrinter.TrapeziumCorrectionInputD,
                        TrapeziumCorrectionInputE = PrintJob.SelectedPrinter.TrapeziumCorrectionInputE,
                        TrapeziumCorrectionInputF = PrintJob.SelectedPrinter.TrapeziumCorrectionInputF,
                        TrapeziumCorrectionSideA = PrintJob.SelectedPrinter.TrapeziumCorrectionSideA,
                        TrapeziumCorrectionSideB = PrintJob.SelectedPrinter.TrapeziumCorrectionSideB,
                        TrapeziumCorrectionSideC = PrintJob.SelectedPrinter.TrapeziumCorrectionSideC,
                        TrapeziumCorrectionSideD = PrintJob.SelectedPrinter.TrapeziumCorrectionSideD,
                        TrapeziumCorrectionSideE = PrintJob.SelectedPrinter.TrapeziumCorrectionSideE,
                        TrapeziumCorrectionSideF = PrintJob.SelectedPrinter.TrapeziumCorrectionSideF,
                    };

                    PrintJob.SelectedPrinter.CreateProperties();
                    PrintJob.SelectedPrinter.CreateProjectors();
                    PrintJob.Option_TurnProjectorOff = PrintJob.Option_TurnProjectorOn = true;

                }
                else if (PrintJob.SelectedPrinter is AtumDLPStation5 || PrintJobManager.SelectedPrinter is LoctiteV10)
                {
                    PrintJob.SelectedPrinter = new AtumV15Printer()
                    {
                        CorrectionFactorX = PrintJob.SelectedPrinter.CorrectionFactorX,
                        CorrectionFactorY = PrintJob.SelectedPrinter.CorrectionFactorY,
                        Description = PrintJob.SelectedPrinter.Description,
                        DisplayName = PrintJob.SelectedPrinter.DisplayName,
                        PrinterXYResolution = PrintJob.SelectedPrinter.PrinterXYResolution,
                        Projectors = PrintJob.SelectedPrinter.Projectors,
                        Properties = PrintJob.SelectedPrinter.Properties,
                        TrapeziumCorrectionInputA = PrintJob.SelectedPrinter.TrapeziumCorrectionInputA,
                        TrapeziumCorrectionInputB = PrintJob.SelectedPrinter.TrapeziumCorrectionInputB,
                        TrapeziumCorrectionInputC = PrintJob.SelectedPrinter.TrapeziumCorrectionInputC,
                        TrapeziumCorrectionInputD = PrintJob.SelectedPrinter.TrapeziumCorrectionInputD,
                        TrapeziumCorrectionInputE = PrintJob.SelectedPrinter.TrapeziumCorrectionInputE,
                        TrapeziumCorrectionInputF = PrintJob.SelectedPrinter.TrapeziumCorrectionInputF,
                        TrapeziumCorrectionSideA = PrintJob.SelectedPrinter.TrapeziumCorrectionSideA,
                        TrapeziumCorrectionSideB = PrintJob.SelectedPrinter.TrapeziumCorrectionSideB,
                        TrapeziumCorrectionSideC = PrintJob.SelectedPrinter.TrapeziumCorrectionSideC,
                        TrapeziumCorrectionSideD = PrintJob.SelectedPrinter.TrapeziumCorrectionSideD,
                        TrapeziumCorrectionSideE = PrintJob.SelectedPrinter.TrapeziumCorrectionSideE,
                        TrapeziumCorrectionSideF = PrintJob.SelectedPrinter.TrapeziumCorrectionSideF,
                        ProjectorResolutionX = 1920,
                        ProjectorResolutionY = 1080,
                    };

                    PrintJob.Option_TurnProjectorOff = PrintJob.Option_TurnProjectorOn = false;
                }


                //var printjobFolderName = Helpers.StringHelper.RemoveDiacritics(PrintJob.Name);
                //var slicePath = Path.Combine(PrintJob.SlicesPath, printjobFolderName);
                if (!PrintJob.SlicesPath.Contains(".zip"))
                {
                    PrintJob.SlicesPath = Path.Combine(PrintJob.SlicesPath, "slices.zip");
                }

                Debug.WriteLine("Total slice time: " + _stopWatch.ElapsedMilliseconds);
                //                Debug.WriteLine(DateTime.Now);

                MemoryHelpers.ForceGCCleanup();

                //save printjob xml file
                var pathPrinterJobXml = Path.Combine((new FileInfo(PrintJob.SlicesPath).Directory.FullName), "printjob.apj");
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DAL.Print.PrintJob));
                using (var streamWriter = new StreamWriter(pathPrinterJobXml, false))
                {
                    serializer.Serialize(streamWriter, PrintJob);
                }
            }

            try
            {
                _zipStream.Flush();
                _zipStream.Close();
                _zipStream.Dispose();

                _tempFileStream.Close();
            }
            catch
            {

            }

            RemoveModelClones();

            //copy org vectors to undopoints
            for (var object3dIndex = ObjectView.Objects3D.Count - 1; object3dIndex > 0; object3dIndex--)
            {
                var object3d = ObjectView.Objects3D[object3dIndex];

                if (!(object3d is GroundPane))
                {
                    var stlModel = object3d as STLModel3D;
                    stlModel.RevertTriangleSnapshotPoints();
                    stlModel.ClearSliceIndexes();

                    if (stlModel.SupportBasementStructure != null)
                    {
                        stlModel.SupportBasementStructure.MoveTranslation = new Vector3Class();
                    }

                    stlModel.UpdateBoundries();
                    stlModel.UpdateBinding();
                }
            }

            PrintJob.PostRenderCompleted = true;
            _cancelRendering = false;
        }

        #endregion

        internal static List<SlicePolyLine3D> GetZIntersectionsSupport(SupportCone supportCone, float sliceHeight)
        {
            var lstlines = new List<SlicePolyLine3D>();

            if (supportCone.BottomPoint <= sliceHeight && supportCone.TopPoint >= sliceHeight)
            {
                foreach (var triangle in supportCone.Triangles[0])
                {
                    if (triangle.Bottom <= sliceHeight && triangle.Top >= sliceHeight)
                    {
                        var s3d = triangle.IntersectZPlane(sliceHeight);
                        if (s3d != null)
                        {
                            s3d.Normal = triangle.Normal;
                            lstlines.Add(s3d);
                        }
                    }
                }
            }

            return lstlines;
        }

        internal static List<SlicePolyLine3D> GetZIntersectionsBaseSTLModel3D(STLModel3D baseObject, float sliceHeight)
        {
            var lstlines = new List<SlicePolyLine3D>();

            if (baseObject.BottomPoint <= sliceHeight && baseObject.TopPoint >= sliceHeight)
            {
                foreach (var triangle in baseObject.Triangles[0])
                {
                    if (triangle.Bottom <= sliceHeight && triangle.Top >= sliceHeight)
                    {
                        var s3d = triangle.IntersectZPlane(sliceHeight);
                        if (s3d != null)
                        {
                            s3d.Normal = triangle.Normal;
                            lstlines.Add(s3d);
                        }
                    }
                }
            }

            return lstlines;
        }

        internal static List<SlicePolyLine3D> GetZIntersections(List<Triangle> triangles, float zcur)
        {
            try
            {
                SlicePolyLine3D s3d = null;
                var lstlines = new List<SlicePolyLine3D>();

                for (var triangleIndex = 0; triangleIndex < triangles.Count; triangleIndex++)
                {
                    var triangle = triangles[triangleIndex];
                    s3d = triangle.IntersectZPlane(zcur);
                    if (s3d != null)
                    {
                        s3d.Normal = triangle.Normal;
                        s3d.TriangleConnection = triangle.Index;
                        lstlines.Add(s3d);
                    }
                }
                return lstlines;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
        Return a list of polygons that intersect at this zlevel
        */
        internal static List<Triangle> GetZPolys(STLModel3D stlModel, SortedDictionary<float, List<TriangleConnectionInfo>> indexKeys, float sliceHeightAsFloat)
        {
            var sliceHeight = (float)Math.Round(sliceHeightAsFloat, 3);
            var lst = new List<Triangle>();
            //List<TriangleConnectionInfo> triangleConnections = null;
            var indexKey = sliceHeight;
            if (indexKeys != null && indexKeys.ContainsKey(sliceHeight))
            {

            }
            else
            {
                foreach (var sliceHeightKey in indexKeys.Keys)
                {
                    if (sliceHeightKey > sliceHeight)
                    {
                        sliceHeight = sliceHeightKey;
                        break;
                    }
                }
            }

            if (indexKeys != null && indexKeys.ContainsKey(sliceHeight))
            {
                foreach (var sliceIndex in indexKeys[sliceHeight])
                {
                    lst.Add(stlModel.Triangles[sliceIndex.ArrayIndex][sliceIndex.TriangleIndex]);
                }
            }


            return lst;
        }

    }
}
