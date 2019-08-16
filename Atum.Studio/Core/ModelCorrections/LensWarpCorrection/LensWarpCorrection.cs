using Atum.DAL.Hardware;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.ModelCorrections.LensWarpCorrection
{
    internal class LensWarpCorrection
    {
        private const int DEBUGPIXELOFFSETX = 400;
        private const int DEBUGPIXELOFFSETY = 800;
        private const int DEBUGBITMAPWIDTH = 2720;
        private const int DEBUGBITMAPHEIGHT = 2000;

        public static void CalculateTransformationMatrix(AtumPrinter selectedPrinter)
        {
            if (selectedPrinter.LensWarpingCorrection.TransformationVectors == null)
            {
                var measureBlockSizeX = 12.8f;
                var measureBlockSizeY = 8f;
                var fitDataMatrixWidth = selectedPrinter.ProjectorResolutionX;
                var fitDataMatrixHeight = selectedPrinter.ProjectorResolutionY;

                // Create the test points.
                var rowMeasurePoints = new List<float> { 0, 319, 575, 831, 1087, 1343, 1599, 1919 };
                var columnMeasurePoints = new List<float> { 0, 200, 360, 520, 680, 840, 1000, 1199 };

                var measurementsXMatrix = CalcFitMatrix(selectedPrinter.LensWarpingCorrection.HorizontalValues, measureBlockSizeX, rowMeasurePoints, columnMeasurePoints, fitDataMatrixWidth, fitDataMatrixHeight, "X");
                var measurementsXMatrixPixelSizeInPercentages = ConvertFixMatrixValuesToPercentages(measurementsXMatrix, measureBlockSizeX, fitDataMatrixWidth, fitDataMatrixHeight);
                var measurementsYMatrix = CalcFitMatrix(selectedPrinter.LensWarpingCorrection.VerticalValues, measureBlockSizeY, rowMeasurePoints, columnMeasurePoints, fitDataMatrixWidth, fitDataMatrixHeight, "Y");
                var measurementsYMatrixPixelSizeInPercentages = ConvertFixMatrixValuesToPercentages(measurementsYMatrix, measureBlockSizeY, fitDataMatrixWidth, fitDataMatrixHeight);

                //PlotFitData(measurementsXMatrix, measurementsYMatric);

                //dispose opbjects
                measurementsXMatrix = null;
                measurementsYMatrix = null;

                var matrixIntersectionPoints = CalcIntersectionPoints(measurementsXMatrixPixelSizeInPercentages, measurementsYMatrixPixelSizeInPercentages, fitDataMatrixWidth, fitDataMatrixHeight, "intersections");
                measurementsXMatrixPixelSizeInPercentages = null;
                measurementsYMatrixPixelSizeInPercentages = null;

                var matrixTransformationVectors = CalcTransformationVectors(matrixIntersectionPoints, fitDataMatrixWidth, fitDataMatrixHeight, "transformation_vectors");
                //DrawValidation(matrixTransformationVectors, fitDataMatrixWidth, fitDataMatrixHeight);
                selectedPrinter.LensWarpingCorrection.TransformationVectors = matrixTransformationVectors;
            }
        }

        private static void DrawValidation(Vector2[,] translationMatrix, int fitDataMatrixWidth, int fitDataMatrixHeight, string fileName = null)
        {

            using (var bitmap = new Bitmap(fitDataMatrixWidth, fitDataMatrixHeight))
            {
                var g = Graphics.FromImage(bitmap);

                for (var sourceColumnIndex = 0; sourceColumnIndex < fitDataMatrixWidth - 1; sourceColumnIndex++)
                {
                    for (var sourceRowIndex = 0; sourceRowIndex < fitDataMatrixHeight; sourceRowIndex++)
                    {
                        var destinationPoint = translationMatrix[sourceColumnIndex, sourceRowIndex];
                        if (fileName != null) g.DrawLine(new Pen(Color.Red, 1), destinationPoint.X, (int)Math.Round(destinationPoint.Y, 0), destinationPoint.X + 0.5f, (int)Math.Round(destinationPoint.Y, 0));
                    }
                }

                if (fileName != null) DebugManager.SaveImage(bitmap, fileName + ".png");
            }
        }

        private static Vector2[,] CalcTransformationVectors(Vector2[,] intersectionPoints, int fitDataMatrixWidth, int fitDataMatrixHeight, string fileName = null)
        {
            var matrixTransformationVectors = new Vector2[fitDataMatrixWidth, fitDataMatrixHeight];
            try
            {
                //var matrixTransformationIntersections = new Vector2[fitDataMatrixWidth, fitDataMatrixHeight];

                var drawYellowPen = new Pen(Color.GreenYellow, 1);

                //create transformation matrix of full bitmap
                Bitmap bitmap = new Bitmap(1, 1);
                Graphics g = Graphics.FromImage(bitmap);
                if (fileName != null)
                {
                    bitmap = new Bitmap(fitDataMatrixWidth, fitDataMatrixHeight);

                    g = Graphics.FromImage(bitmap);
                }

                //var transformedNearestYPoints = new List<List<TransformationMatrixIntersectionData>>();
                try
                {
                    //for (var i = 0; i < fitDataMatrixHeight; i++)
                    //{
                    //    transformedNearestYPoints.Add(new List<TransformationMatrixIntersectionData>());
                    //}
                    for (var rowIndex = 0; rowIndex < 15; rowIndex++)
                    {
                        for (var columnIndex = 0; columnIndex < 15; columnIndex++)
                        {
                            var transformationMatrix = new LensCorrectionTransformationMatrix(intersectionPoints[columnIndex, rowIndex], intersectionPoints[columnIndex + 1, rowIndex], intersectionPoints[columnIndex, rowIndex + 1], intersectionPoints[columnIndex + 1, rowIndex + 1],
                                new Vector2(columnIndex * 128, rowIndex * 80), new Vector2(((columnIndex + 1) * 128) - 1, rowIndex * 80), new Vector2(((columnIndex) * 128), ((rowIndex + 1) * 80) - 1), new Vector2(((columnIndex + 1) * 128) - 1, ((rowIndex + 1) * 80) - 1), 40, 0, 15);

                            transformationMatrix.CalcTransformations();

                            for (var transformedColumnIndex = 0; transformedColumnIndex < 128; transformedColumnIndex++)
                            {
                                for (var transformedRowIndex = 0; transformedRowIndex < 80; transformedRowIndex++)
                                {

                                    var transformedPointIntersection = transformationMatrix.TransformedIntersections[transformedColumnIndex][transformedRowIndex].IntersectionPoint;
                                    var originColumnIndex = (columnIndex * 128) + transformedColumnIndex;
                                    var originRowIndex = (rowIndex * 80) + transformedRowIndex;
                                    if (originColumnIndex > fitDataMatrixWidth - 1)
                                    {
                                        originColumnIndex = fitDataMatrixWidth - 1;
                                    }

                                    if (transformedPointIntersection != new Vector2())
                                    {
                                        if (matrixTransformationVectors[originColumnIndex, originRowIndex] == new Vector2())
                                        {
                                            //add to y point search indexes
                                            var transformedIntersectionPoint = transformationMatrix.TransformedIntersections[transformedColumnIndex][transformedRowIndex];
                                            transformedIntersectionPoint.ColumnIndex = originColumnIndex;
                                            transformedIntersectionPoint.RowIndex = originRowIndex;

                                            var intersectionPointY = (int)Math.Floor(transformedIntersectionPoint.IntersectionPoint.Y);
                                            if (fitDataMatrixHeight <= intersectionPointY)
                                            {
                                                intersectionPointY = fitDataMatrixHeight - 1;
                                            }
                                            // transformedNearestYPoints[intersectionPointY].Add(transformedIntersectionPoint);

                                            intersectionPointY = (int)Math.Ceiling(transformedIntersectionPoint.IntersectionPoint.Y);
                                            if (fitDataMatrixHeight <= intersectionPointY)
                                            {
                                                intersectionPointY = fitDataMatrixHeight - 1;
                                            }
                                            //   transformedNearestYPoints[intersectionPointY].Add(transformedIntersectionPoint);

                                            matrixTransformationVectors[originColumnIndex, originRowIndex] = transformedIntersectionPoint.IntersectionPoint;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {

                }

                //    matrixTransformationVectors = matrixTransformationIntersections;

                if (fileName != null) DebugManager.SaveImage(bitmap, fileName + ".png");
            }
            catch (Exception exc)
            {

            }

            return matrixTransformationVectors;
        }

        private static Vector2[,] CalcIntersectionPoints(float[,] measurementsXMatrixPixelSizeInPercentages, float[,] measurementsYMatrixPixelSizeInPercentages, int fitDataMatrixWidth, int fitDataMatrixHeight, string fileName = null)
        {
            var intersectionPoints = new Vector2[16, 16];
            var result = new float[fitDataMatrixWidth, fitDataMatrixHeight];

            var drawMagentaPen = new Pen(Color.Magenta, 1);
            var drawGrayPen = new Pen(Color.Gray, 1);
            var drawPinkPen = new Pen(Color.Pink, 1);
            var drawGreenPen = new Pen(Color.GreenYellow, 1);

            Bitmap bitmap = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bitmap);
            if (fileName != null)
            {
                bitmap = new Bitmap(DEBUGBITMAPWIDTH, DEBUGBITMAPHEIGHT);
                g = Graphics.FromImage(bitmap);
            }


            //get column (Y) values
            var columnsFitData = new List<List<Vector2>>();
            var columnMeasurePoints = new List<float> { 0, 80, 160, 240, 320, 400, 480, 560, 640, 720, 800, 880, 960, 1040, 1120, 1200 };
            foreach (var columnMeasurePoint in columnMeasurePoints)
            {
                var columnFitData = new List<Vector2>();

                //columns
                for (var fitDataMatrixIndex = fitDataMatrixWidth - 1; fitDataMatrixIndex > 0; fitDataMatrixIndex--)
                {
                    var columnValues = new List<float>();
                    for (var rowIndex = 0; rowIndex < fitDataMatrixHeight; rowIndex++)
                    {
                        columnValues.Add(measurementsYMatrixPixelSizeInPercentages[fitDataMatrixIndex, rowIndex]);
                    }

                    var topPointY = 0f;
                    var currenYPixelPosition = 0;
                    while (topPointY < columnMeasurePoint)
                    {

                        if (currenYPixelPosition >= 1199)
                        {
                            topPointY += (columnValues[columnValues.Count - 1]);
                            currenYPixelPosition++;
                        }
                        else
                        {
                            topPointY += (columnValues[currenYPixelPosition]);
                            currenYPixelPosition++;
                        }
                    }


                    if (fileName != null) g.DrawLine(drawPinkPen, fitDataMatrixIndex + DEBUGPIXELOFFSETX, currenYPixelPosition, fitDataMatrixIndex + DEBUGPIXELOFFSETX, currenYPixelPosition + 0.5f);
                    if (fileName != null) g.DrawLine(drawGrayPen, fitDataMatrixIndex + DEBUGPIXELOFFSETX, columnMeasurePoint, fitDataMatrixIndex + DEBUGPIXELOFFSETX, columnMeasurePoint + 0.5f);
                    columnFitData.Add(new Vector2(fitDataMatrixIndex, currenYPixelPosition));
                }

                columnsFitData.Add(columnFitData);
            }

            //rows
            var rowMeasurePoints = new List<float> { 0, 128, 256, 384, 512, 640, 768, 896, 1024, 1152, 1280, 1408, 1536, 1664, 1792, 1920 };
            var rowsFitData = new List<List<Vector2>>();

            //get row (X) values from fitdata
            foreach (var rowMeasurePoint in rowMeasurePoints)
            {
                var rowFitData = new List<Vector2>();
                for (var rowIndex = fitDataMatrixHeight - 1; rowIndex > 0; rowIndex--)
                {
                    var rowValues = new List<float>();
                    for (var columnIndex = 0; columnIndex < fitDataMatrixWidth; columnIndex++)
                    {
                        rowValues.Add(measurementsXMatrixPixelSizeInPercentages[columnIndex, rowIndex]);
                    }

                    if (rowMeasurePoint < fitDataMatrixWidth / 2)
                    {
                        var leftPointX = (fitDataMatrixWidth / 2) - rowMeasurePoint;
                        var currentLeftXPixelPosition = fitDataMatrixWidth / 2;

                        while (leftPointX > 0)
                        {
                            if (currentLeftXPixelPosition <= -1)
                            {
                                leftPointX -= (rowValues[0]);
                            }
                            else
                            {
                                leftPointX -= (rowValues[currentLeftXPixelPosition]);
                            }
                            currentLeftXPixelPosition--;
                        }


                        var columnValuesInPercentages = new List<float>();
                        for (var scaledRowIndex = 0; scaledRowIndex < fitDataMatrixHeight; scaledRowIndex++)
                        {
                            columnValuesInPercentages.Add(measurementsYMatrixPixelSizeInPercentages[currentLeftXPixelPosition, scaledRowIndex]);
                        }

                        //find scaledRowValue
                        var scaledRowIndexByUsingPercentages = 0f;
                        var scaledColumnValueIndex = 0;
                        while (rowIndex > scaledRowIndexByUsingPercentages)
                        {

                            if (scaledColumnValueIndex >= 1199)
                            {
                                scaledRowIndexByUsingPercentages += columnValuesInPercentages[columnValuesInPercentages.Count - 1];
                            }
                            else
                            {
                                scaledRowIndexByUsingPercentages += columnValuesInPercentages[scaledColumnValueIndex];
                            }
                            scaledColumnValueIndex++;
                        }

                        //currentLeftXPixelPosition = 1920 - currentLeftXPixelPosition;
                        if (fileName != null) g.DrawLine(drawMagentaPen, currentLeftXPixelPosition + DEBUGPIXELOFFSETX, scaledColumnValueIndex, currentLeftXPixelPosition + 0.5f + DEBUGPIXELOFFSETX, scaledColumnValueIndex);
                        rowFitData.Add(new Vector2(currentLeftXPixelPosition, scaledColumnValueIndex));
                    }
                    else if (rowMeasurePoint >= (fitDataMatrixWidth / 2))
                    {
                        var rightPointX = (fitDataMatrixWidth / 2f);
                        var currentRightXPixelPosition = (fitDataMatrixWidth / 2) - 1;
                        while (rightPointX < rowMeasurePoint)
                        {
                            rightPointX += (rowValues[currentRightXPixelPosition]);
                            currentRightXPixelPosition++;
                        }

                        var columnValuesInPercentages = new List<float>();
                        for (var scaledRowIndex = 0; scaledRowIndex < fitDataMatrixHeight; scaledRowIndex++)
                        {
                            columnValuesInPercentages.Add(measurementsYMatrixPixelSizeInPercentages[currentRightXPixelPosition, scaledRowIndex]);
                        }

                        //find scaledRowValue
                        var scaledRowIndexByUsingPercentages = 0f;
                        var scaledColumnValueIndex = 0;
                        while (rowIndex > scaledRowIndexByUsingPercentages)
                        {
                            if (scaledColumnValueIndex >= 1199)
                            {
                                scaledRowIndexByUsingPercentages += columnValuesInPercentages[columnValuesInPercentages.Count - 1];
                            }
                            else
                            {
                                scaledRowIndexByUsingPercentages += columnValuesInPercentages[scaledColumnValueIndex];
                            }
                            scaledColumnValueIndex++;
                        }

                        //currentRightXPixelPosition = 1920 - currentRightXPixelPosition;
                        if (fileName != null) g.DrawLine(drawMagentaPen, currentRightXPixelPosition + DEBUGPIXELOFFSETX, scaledColumnValueIndex, currentRightXPixelPosition + 0.5f + DEBUGPIXELOFFSETX, scaledColumnValueIndex);
                        rowFitData.Add(new Vector2(currentRightXPixelPosition, scaledColumnValueIndex));
                    }
                }

                rowsFitData.Add(rowFitData);
            }

            //add 1 px to first/last row column to make sure that there always and intersection is available
            foreach (var rowFitData in rowsFitData)
            {
                rowFitData[0] += new Vector2(0, 5);
                rowFitData[rowFitData.Count - 1] -= new Vector2(0, 5);
            }

            foreach (var columnFitData in columnsFitData)
            {
                columnFitData[0] += new Vector2(5, 0);
                columnFitData[columnFitData.Count - 1] -= new Vector2(5, 0);
            }

            //filter min/max column values to speed up intersection lookup
            var columnMinMaxValues = new List<LensCorrectionTransformationMatrixColumnMinMax>();
            for (var columnIndex = 0; columnIndex < columnsFitData.Count; columnIndex++)
            {
                var minValue = float.MaxValue;
                var maxValue = float.MinValue;
                for (var columnFitDataIndex = 0; columnFitDataIndex < columnsFitData[columnIndex].Count; columnFitDataIndex++)
                {
                    minValue = Math.Min(minValue, columnsFitData[columnIndex][columnFitDataIndex].Y);
                    maxValue = Math.Max(maxValue, columnsFitData[columnIndex][columnFitDataIndex].Y);
                }

                columnMinMaxValues.Add(new LensCorrectionTransformationMatrixColumnMinMax() { Min = minValue - 5, Max = maxValue + 5 });
            }

            for (var rowIndex = 0; rowIndex < rowsFitData.Count; rowIndex++)
            {
                for (var rowFitDataIndex = 0; rowFitDataIndex < rowsFitData[rowIndex].Count - 1; rowFitDataIndex++)
                {
                    var currentRowPoint = rowsFitData[rowIndex][rowFitDataIndex];
                    var nextRowPoint = rowsFitData[rowIndex][rowFitDataIndex + 1];

                    //check if column exists within this range
                    var intersectionPoint = new Vector2();
                    for (var columnMinMaxIndex = 0; columnMinMaxIndex < columnMinMaxValues.Count; columnMinMaxIndex++)
                    {

                        if (columnMinMaxValues[columnMinMaxIndex].Min < currentRowPoint.Y && columnMinMaxValues[columnMinMaxIndex].Max > currentRowPoint.Y)
                        {
                            //find intersection data
                            for (var columnFitDataIndex = 0; columnFitDataIndex < columnsFitData[columnMinMaxIndex].Count - 1; columnFitDataIndex++)
                            {
                                var currentColumnPoint = columnsFitData[columnMinMaxIndex][columnFitDataIndex];
                                var nextColumnPoint = columnsFitData[columnMinMaxIndex][columnFitDataIndex + 1];

                                var hasIntersectionPoint = Helpers.VectorHelper.LineSegmentsIntersect(currentRowPoint, nextRowPoint, currentColumnPoint, nextColumnPoint, out intersectionPoint);

                                if (hasIntersectionPoint)
                                {
                                    intersectionPoints[rowIndex, -(columnMinMaxIndex - 15)] = intersectionPoint;
                                    if (fileName != null) g.DrawLine(drawGreenPen, intersectionPoint.X + DEBUGPIXELOFFSETX, intersectionPoint.Y, intersectionPoint.X + 0.5f + DEBUGPIXELOFFSETX, intersectionPoint.Y);
                                    break;
                                }

                            }
                        }
                    }
                }
            }

            if (fileName != null) DebugManager.SaveImage(bitmap, fileName + ".png");


            return intersectionPoints;
        }

        private static float[,] ConvertFixMatrixValuesToPercentages(float[,] fitDataMatrix, float compareValue, int fitDataMatrixWidth, int fitDataMatrixHeight)
        {
            var measurementMatrixInPercentages = new float[fitDataMatrixWidth, fitDataMatrixHeight];

            for (var columnIndex = 0; columnIndex < fitDataMatrixWidth; columnIndex++)
            {
                for (var rowIndex = 0; rowIndex < fitDataMatrixHeight; rowIndex++)
                {
                    if (fitDataMatrix[columnIndex, rowIndex] > compareValue)
                    {
                        var xPercentageOffset = fitDataMatrix[columnIndex, rowIndex] / compareValue;
                        measurementMatrixInPercentages[columnIndex, rowIndex] = xPercentageOffset;
                    }
                    else if (fitDataMatrix[columnIndex, rowIndex] < compareValue)
                    {
                        var xPercentageOffset = fitDataMatrix[columnIndex, rowIndex] / compareValue;
                        measurementMatrixInPercentages[columnIndex, rowIndex] = xPercentageOffset;
                    }
                    else if (fitDataMatrix[columnIndex, rowIndex] == 1)
                    {
                        measurementMatrixInPercentages[columnIndex, rowIndex] = 1;
                    }
                }
            }

            return measurementMatrixInPercentages;
        }

        private static float[,] CalcFitMatrix(List<List<float>> measuredValues, float compareValue, List<float> rowMeasurementPoints, List<float> columnMeasurementPoints, int fitDataMatrixWidth, int fitDataMatrixHeight, string fileName)
        {

            var currentRowIndex = 0;
            CubicSpline fitDataCurve = new CubicSpline();

            var fitDataMatrix = new float[fitDataMatrixWidth, fitDataMatrixHeight];
            float stepSize = (rowMeasurementPoints[rowMeasurementPoints.Count - 1] - rowMeasurementPoints[0]) / (fitDataMatrixWidth - 1);

            float[] columnIndexes = new float[fitDataMatrixWidth];
            for (int i = 0; i < fitDataMatrixWidth; i++)
            {
                columnIndexes[i] = rowMeasurementPoints[0] + i * stepSize;
            }

            var rowIndexes = new float[fitDataMatrixHeight];
            for (var rowIndex = 0; rowIndex < fitDataMatrixHeight; rowIndex++)
            {
                rowIndexes[rowIndex] = rowIndex;
            }

            foreach (var rowMeasuredSize in measuredValues)
            {
                fitDataCurve = new CubicSpline();
                float[] rowFitData = fitDataCurve.FitAndEval(rowMeasurementPoints.ToArray(), rowMeasuredSize.ToArray(), columnIndexes);
                var rowNumber = (int)columnMeasurementPoints[currentRowIndex];

                var yIndex = 0;
                foreach (var rowFitDataValue in rowFitData)
                {
                    fitDataMatrix[yIndex, rowNumber] = rowFitDataValue;
                    yIndex++;
                }

        //        string path = @"spline-wikipedia.png";
          //      PlotSplineSolution("test", rowMeasurementPoints.ToArray(), rowMeasuredSize.ToArray(), columnIndexes, rowFitData, path);
                currentRowIndex++;
            }

            //process all column pixels
            var columnMeasureSizesAsFloat = new List<float>();
            foreach (var yAxisMeasurePoint in columnMeasurementPoints)
            {
                columnMeasureSizesAsFloat.Add(yAxisMeasurePoint);
            }

            for (var columnIndex = 0; columnIndex < fitDataMatrixWidth; columnIndex++)
            {
                fitDataCurve = new CubicSpline();

                var columnFitData = new List<float>();

                for (var columnFitDataIndex = 0; columnFitDataIndex < columnMeasurementPoints.Count; columnFitDataIndex++)
                {
                    columnFitData.Add(fitDataMatrix[columnIndex, (int)columnMeasurementPoints[columnFitDataIndex]]);
                }

                float[] columnValues = fitDataCurve.FitAndEval(columnMeasureSizesAsFloat.ToArray(), columnFitData.ToArray(), rowIndexes);

                var measureMatrixRowIndex = 0;
                foreach (var columnValue in columnValues)
                {
                    fitDataMatrix[columnIndex, measureMatrixRowIndex] = columnValue;

                    measureMatrixRowIndex++;

                }
            }

            if (fileName != null)
            {
                //export matrix to csv
                var stringBuilder = new StringBuilder();
                for (var rowIndex = 0; rowIndex < fitDataMatrixHeight; rowIndex++)
                {
                    for (var columnIndex = 0; columnIndex < fitDataMatrixWidth; columnIndex++)
                    {
                        stringBuilder.Append(fitDataMatrix[columnIndex, rowIndex] + ";");
                    }
                }

                DebugManager.SaveStringToTextFile(stringBuilder, fileName + ".txt");

                using (var heatmap = new Bitmap(fitDataMatrixWidth, fitDataMatrixHeight))
                {
                    var g = Graphics.FromImage(heatmap);

                    var lowestOffsetX = compareValue;
                    var highestOffsetX = compareValue;
                    for (var columnIndex = 0; columnIndex < fitDataMatrixWidth; columnIndex++)
                    {
                        for (var rowIndex = 0; rowIndex < fitDataMatrixHeight; rowIndex++)
                        {
                            if (fitDataMatrix[columnIndex, rowIndex] < lowestOffsetX)
                            {
                                lowestOffsetX = fitDataMatrix[columnIndex, rowIndex];
                            }
                            if (fitDataMatrix[columnIndex, rowIndex] > highestOffsetX)
                            {
                                highestOffsetX = fitDataMatrix[columnIndex, rowIndex];
                            }
                        }
                    }

                    var totalLowestOffset = compareValue - lowestOffsetX;
                    var totalHighestOffset = highestOffsetX - compareValue;

                    for (var columnIndex = 0; columnIndex < fitDataMatrixWidth; columnIndex++)
                    {
                        for (var rowIndex = 0; rowIndex < fitDataMatrixHeight; rowIndex++)
                        {
                            if (fitDataMatrix[columnIndex, rowIndex] > compareValue)
                            {
                                var xPercentageOffset = (fitDataMatrix[columnIndex, rowIndex] - compareValue) / totalHighestOffset;
                                var bluep = ((int)(xPercentageOffset * (255)));
                                var greenp = 255 - (int)(xPercentageOffset * (255));
                                g.DrawLine(new Pen(Color.FromArgb(255, 0, greenp, bluep)), columnIndex, rowIndex, columnIndex + 1, rowIndex);
                            }
                            else if (fitDataMatrix[columnIndex, rowIndex] < compareValue)
                            {
                                var xPercentageOffset = (compareValue - fitDataMatrix[columnIndex, rowIndex]) / totalLowestOffset;
                                var redp = ((int)(xPercentageOffset * (255)));
                                var greenp = 255 - (int)(xPercentageOffset * (255));
                                g.DrawLine(new Pen(Color.FromArgb(255, redp, greenp, 0)), columnIndex, rowIndex, columnIndex + 1, rowIndex);
                            }
                            else if (fitDataMatrix[columnIndex, rowIndex] == 1)
                            {
                                g.DrawLine(new Pen(Color.Green), columnIndex, rowIndex, columnIndex + 1, rowIndex);
                            }
                        }
                    }

                    if (fileName != null) DebugManager.SaveImage(heatmap, fileName + ".png");
                }
            }

            return fitDataMatrix;
        }


        internal static void CalculateByModel(STLModel3D model, AtumPrinter selectedPrinter)
        {
            //convert polynode to pixel values
            for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var pixelPoint = Helpers.VectorHelper.GetPixelPointFromModelPoint(model.Triangles[arrayIndex][triangleIndex].Points[vectorIndex], selectedPrinter);
                        var pixelPointX = (int)Math.Round(pixelPoint.X, 0);
                        var pixelPointY = (int)Math.Round(pixelPoint.Y, 0);

                        //transform pixelPoint
                        if (pixelPointX >= selectedPrinter.ProjectorResolutionX)
                        {
                            pixelPointX = selectedPrinter.ProjectorResolutionX - 1;
                        }

                        if (pixelPointY >= selectedPrinter.ProjectorResolutionY)
                        {
                            pixelPointY = selectedPrinter.ProjectorResolutionY - 1;
                        }
                        var transformedPixelPoint = selectedPrinter.LensWarpingCorrection.TransformationVectors[pixelPointX, pixelPointY];

                        pixelPoint = new Vector3Class(transformedPixelPoint.X, transformedPixelPoint.Y, pixelPoint.Z);

                        //pixelPoint to 3d space point
                        var modelPoint = Helpers.VectorHelper.GetModelPointFromPixelPoint(pixelPoint, selectedPrinter);
                        model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position = modelPoint;

                    }
                }
            }

        }

        #region PlotSplineSolution

            private static void PlotSplineSolution(string title, float[] x, float[] y, float[] xs, float[] ys, string path, float[] qPrime = null)
        {
            var chart = new Chart();
            chart.Size = new Size(1920, 1200);
            chart.Titles.Add(title);
            chart.Legends.Add(new Legend("Legend"));

            ChartArea ca = new ChartArea("DefaultChartArea");
            ca.AxisX.Title = "X";
            ca.AxisY.Title = "Y";
            chart.ChartAreas.Add(ca);

            Series s2 = CreateSeries(chart, "Spline", CreateDataPoints(xs, ys), Color.Blue, MarkerStyle.None);
            Series s3 = CreateSeries(chart, "Original", CreateDataPoints(x, y), Color.Green, MarkerStyle.Diamond);

            chart.Series.Add(s3);
            chart.Series.Add(s2);

            if (qPrime != null)
            {
                Series s4 = CreateSeries(chart, "Slope", CreateDataPoints(xs, qPrime), Color.Red, MarkerStyle.None);
                chart.Series.Add(s4);
            }

            ca.RecalculateAxesScale();
            ca.AxisX.Minimum = Math.Floor(ca.AxisX.Minimum);
            ca.AxisX.Maximum = Math.Ceiling(ca.AxisX.Maximum);
            int nIntervals = (x.Length - 1);
            nIntervals = Math.Max(4, nIntervals);
            ca.AxisX.Interval = (ca.AxisX.Maximum - ca.AxisX.Minimum) / nIntervals;

            // Save
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                chart.SaveImage(fs, ChartImageFormat.Png);
            }
        }

        private static List<DataPoint> CreateDataPoints(float[] x, float[] y)
        {
            Debug.Assert(x.Length == y.Length);
            List<DataPoint> points = new List<DataPoint>();

            for (int i = 0; i < x.Length; i++)
            {
                points.Add(new DataPoint(x[i], y[i]));
            }

            return points;
        }

        private static Series CreateSeries(Chart chart, string seriesName, IEnumerable<DataPoint> points, Color color, MarkerStyle markerStyle = MarkerStyle.None)
        {
            var s = new Series()
            {
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Double,
                Legend = chart.Legends[0].Name,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line,
                Name = seriesName,
                ChartArea = chart.ChartAreas[0].Name,
                MarkerStyle = markerStyle,
                Color = color,
                MarkerSize = 8
            };

            foreach (var p in points)
            {
                s.Points.Add(p);
            }

            return s;
        }

        private static string ToString<T>(T[] array, string format = "")
        {
            var s = new StringBuilder();
            string formatString = "{0" + format + "}";

            for (int i = 0; i < array.Length; i++)
            {
                if (i < array.Length - 1)
                {
                    s.AppendFormat(formatString + ", ", array[i]);
                }
                else
                {
                    s.AppendFormat(formatString, array[i]);
                }
            }

            return s.ToString();
        }

        #endregion
    }
}
