using Atum.DAL.Compression.Zip;
using Atum.DAL.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System;
using System.Diagnostics;
using System.IO;

namespace Atum.Studio.Core.ModelCorrections
{
    internal static class ProjectorCalibrationMatrix
    {
        internal static void Calculate(STLModel3D model)
        {
            var selectedPrinter = Managers.PrinterManager.DefaultPrinter;

            var metadataFilePath = Path.Combine(DAL.ApplicationSettings.Settings.PrintersMetadataPath, selectedPrinter.ID + ".zip");
            if (File.Exists(metadataFilePath))
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var metadataZipFile = new ZipFile(new StreamReader(metadataFilePath).BaseStream);
                var zipEntry = metadataZipFile.GetEntry("ProjectorCalibrationMatrix.xml");
                if (zipEntry != null)
                {
                    var projectorTranslationMatrix = new Vector2[1920, 1200];
                    using (var textZipStream = metadataZipFile.GetInputStream(zipEntry))
                    {
                        using (StreamReader sr = new StreamReader(textZipStream))
                        {
                            var columnIndex = 0;
                            while (!sr.EndOfStream)
                            {
                                var readLine = sr.ReadLine();
                                var rowIndex = 0;
                                foreach (var rowValue in readLine.Split(';'))
                                {
                                    var xyValue = rowValue.Split(':');
                                    if (rowValue != string.Empty)
                                    {
                                        projectorTranslationMatrix[columnIndex, rowIndex] = new Vector2(float.Parse(xyValue[0]), float.Parse(xyValue[1]));
                                    }

                                    if (rowIndex == 1200)
                                    {
                                        break;
                                    }

                                    rowIndex++;
                                }

                                columnIndex++;
                            }
                        }
                    }

                    var leftx = 5000f;
                    var lefty = 5000f;
                    var correctedPoints = new TriangleConnectedPoints();
                    foreach (var key in model.Triangles.ConnectedPoints.Keys)
                    {
                        leftx = (key.X * 10) + 960;
                        var lowerLeftX = (int)Math.Floor(leftx);
                        var upperLeftX = lowerLeftX + 1;
                        lefty = (key.Y * 10) + 600 - 2;
                        var lowerLeftY = (int)Math.Floor(lefty);
                        var upperLeftY = lowerLeftY + 1;

                        var correctedPointLower = projectorTranslationMatrix[lowerLeftX, lowerLeftY];
                        var correctedPointUpper = projectorTranslationMatrix[upperLeftX, upperLeftY];

                        var delta = (correctedPointUpper - correctedPointLower) / 10;
                        var deltaX = delta.X * (leftx - lowerLeftX);
                        var deltaY = delta.Y * (lefty - lowerLeftY);
                        var deltaVector = new Vector3(deltaX, deltaY, 0);

                        var newKeyVector2 = correctedPointLower + deltaVector.Xy;
                        newKeyVector2 = ((newKeyVector2 - (new Vector2(960, 600))) / 10);
                        var newKey = new Vector3(newKeyVector2.X, newKeyVector2.Y, key.Z);
                        if (!correctedPoints.ContainsKey(newKey))
                        {
                            correctedPoints.Add(newKey, model.Triangles.ConnectedPoints[key]);
                        }
                        else
                        {
                            correctedPoints[newKey].AddRange(model.Triangles.ConnectedPoints[key]);
                        }
                    }

                    model.Triangles.ConnectedPoints.UpdateTrianglesFromKeys(model.Triangles, correctedPoints);
                }

                metadataZipFile.Close();

                LoggingManager.WriteToLog("Printer Manager", "Camera Calibration Values", string.Format("Processed in {0}ms", stopWatch.ElapsedMilliseconds));
            }
        }

    }
}
