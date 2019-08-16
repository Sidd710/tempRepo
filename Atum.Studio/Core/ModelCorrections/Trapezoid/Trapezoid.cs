using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Models.Defaults;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Atum.Studio.Core.ModelCorrections
{
    internal static class Trapezoid
    {
        internal static void Calculate(STLModel3D model, DAL.Materials.Material selectedMaterial, Vector3Class modelTranslation)
        {
            model.UpdateBoundries();

            //properties of right part of trapezoid
            var sideA = PrintJobManager.SelectedPrinter.TrapeziumCorrectionInputA;
            var sideB = PrintJobManager.SelectedPrinter.TrapeziumCorrectionInputB;
            var sideC = PrintJobManager.SelectedPrinter.TrapeziumCorrectionInputC;
            var sideD = PrintJobManager.SelectedPrinter.TrapeziumCorrectionInputD;
            var sideE = PrintJobManager.SelectedPrinter.TrapeziumCorrectionInputE;
            var sideF = PrintJobManager.SelectedPrinter.TrapeziumCorrectionInputF;

            var sideBCAngleOffset = 0f;
            var sideBCAngle = 90d;

            var maxBuildSizeX = PrintJobManager.SelectedPrinter.MaxBuildSizeX;
            var maxBuildSizeY = PrintJobManager.SelectedPrinter.MaxBuildSizeY;

            var calibrationModelSizeX = BasicCorrectionModel.ModelSizeX;
            var calibrationModelSizeY = BasicCorrectionModel.ModelSizeY;

            var triangleBArea = Math.Sqrt(((sideC + sideB + sideF) / 2)
                * (((sideC + sideB + sideF) / 2) - sideC)
                * (((sideC + sideB + sideF) / 2) - sideB)
                * (((sideC + sideB + sideF) / 2) - sideF));

            var sideBDHeight = ((float)(triangleBArea * 2) / sideB);

            if (sideA != calibrationModelSizeY || sideB != calibrationModelSizeX ||
                sideC != calibrationModelSizeY || sideD != calibrationModelSizeX ||
                sideE != sideF)
            {
                sideBCAngle = MathHelper.RadiansToDegrees(Math.Acos((Math.Pow(sideC, 2) + Math.Pow(sideB, 2) - Math.Pow(sideF, 2)) / (2 * sideC * sideB)));
                sideBCAngleOffset = (float)Math.Sqrt(Math.Pow(sideC, 2) - Math.Pow(sideBDHeight, 2));

                if (sideBCAngle > 90)
                {
                    sideBCAngleOffset = -sideBCAngleOffset;
                }
            }

            //properties of left part of trapezoid
            var sideABAngleOffset = 0f;
            var sideABAngle = 90d;

            var triangleAArea = Math.Sqrt(((sideA + sideB + sideE) / 2)
                * (((sideA + sideB + sideE) / 2) - sideA)
                * (((sideA + sideB + sideE) / 2) - sideB)
                * (((sideA + sideB + sideE) / 2) - sideE));

            var sideABHeight = ((float)(triangleAArea * 2) / sideB);

            if (sideA != calibrationModelSizeY || sideB != calibrationModelSizeX ||
                sideC != calibrationModelSizeY || sideD != calibrationModelSizeX ||
                sideE != sideF)
            {
                sideABAngleOffset = (float)Math.Sqrt(Math.Pow(sideA, 2) - Math.Pow(sideABHeight, 2));
                sideABAngle = MathHelper.RadiansToDegrees(Math.Acos((Math.Pow(sideA, 2) + Math.Pow(sideB, 2) - Math.Pow(sideE, 2)) / (2 * sideA * sideB)));
                if (sideABAngle < 90)
                {
                    sideABAngleOffset = -sideABAngleOffset;
                }
            }

            Calculate(model, selectedMaterial, modelTranslation, maxBuildSizeX, maxBuildSizeY, sideABHeight, sideBDHeight, sideBCAngleOffset,
                sideABAngleOffset, sideABAngle, sideBCAngle, sideB, calibrationModelSizeX, calibrationModelSizeY);
        }

        internal static void Calculate(STLModel3D model, DAL.Materials.Material selectedMaterial, Vector3Class modelTranslation,
            float maxBuildSizeX, float maxBuildSizeY,
            float sideABHeight, float sideBDHeight, float sideBCAngleOffset, float sideABAngleOffset,
            double sideABAngle, double sideBCAngle,
            float sideB,
            float calibrationModelSizeX, float calibrationModelSizeY)
        {
            model.UpdateBoundries();
            var moveYFactor = -(maxBuildSizeY / 2);

            for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    model.Triangles[arrayIndex][triangleIndex].Vectors[0].Position += new Vector3Class(0, moveYFactor, 0);
                    model.Triangles[arrayIndex][triangleIndex].Vectors[1].Position += new Vector3Class(0, moveYFactor, 0);
                    model.Triangles[arrayIndex][triangleIndex].Vectors[2].Position += new Vector3Class(0, moveYFactor, 0);
                }
            }

            model.UpdateBoundries();

            float sideDHeightDifference = (float)(sideABHeight - sideBDHeight);
            if (sideDHeightDifference < 0) { sideDHeightDifference = -sideDHeightDifference; }
            //change model vectors

            if (float.IsNaN(sideABAngleOffset)) sideABAngleOffset = 0;
            if (float.IsNaN(sideBCAngleOffset)) sideBCAngleOffset = 0;

            var leftSideMaxCorrection = new Vector3((sideABAngleOffset), sideDHeightDifference, 0);
            var rightSideMaxCorrection = new Vector3((sideBCAngleOffset), 0, 0);
            if (sideABHeight >= sideBDHeight) //highest point at right side
            {
                leftSideMaxCorrection = new Vector3(leftSideMaxCorrection.X, 0, 0); //NO Y correction
                rightSideMaxCorrection = new Vector3(rightSideMaxCorrection.X, sideDHeightDifference, 0);
            }

            for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var correctionVector = new Vector3Class();

                        var orgPoint = model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;

                        var vectorOffSetPercentageABY = orgPoint.Y / (float)sideABHeight;
                        var vectorOffSetPercentageBDY = orgPoint.Y / (float)sideBDHeight;

                        var horizontalABOffsetPercentage = ((calibrationModelSizeX / 2) + orgPoint.X) / calibrationModelSizeX;
                        var horizontalBCOffsetPercentage = (1 - (orgPoint.X / (calibrationModelSizeX / 2))) / 2;

                        if (orgPoint.X < 0)
                        {
                            horizontalABOffsetPercentage = 1 - (0.5f / (calibrationModelSizeX / 2)) * ((calibrationModelSizeX / 2) - orgPoint.X);
                            horizontalBCOffsetPercentage = (-orgPoint.X + (calibrationModelSizeX / 2)) / calibrationModelSizeX;
                        }

                        var verticalOffsetPercentage = vectorOffSetPercentageABY * (1 - horizontalABOffsetPercentage);

                        if (sideABHeight < sideBDHeight)
                        {
                            verticalOffsetPercentage = vectorOffSetPercentageBDY * (1 - horizontalBCOffsetPercentage);
                        }

                        if (sideABAngle <= 90)    //angle A forward /
                        {
                            correctionVector = new Vector3Class(-vectorOffSetPercentageABY * rightSideMaxCorrection.X * horizontalABOffsetPercentage, 0, 0);
                        }
                        else if (sideABAngle > 90)    //angle A backward
                        {
                            correctionVector = new Vector3Class(vectorOffSetPercentageABY * leftSideMaxCorrection.X * horizontalABOffsetPercentage, 0, 0);
                        }

                        if (sideBCAngle <= 90)    //angle C forward \
                        {
                            correctionVector += new Vector3Class(-vectorOffSetPercentageBDY * leftSideMaxCorrection.X * horizontalBCOffsetPercentage, 0, 0);
                        }
                        else if (sideBCAngle > 90)    //angle C backward /
                        {
                            correctionVector += new Vector3Class(vectorOffSetPercentageBDY * rightSideMaxCorrection.X * horizontalBCOffsetPercentage, 0, 0);
                        }

                        //Y correction
                        if (sideABHeight < sideBDHeight && sideDHeightDifference != 0)
                        {
                            correctionVector += new Vector3Class(0, verticalOffsetPercentage * sideDHeightDifference, 0);
                        }
                        else if (sideDHeightDifference != 0)
                        {
                            correctionVector += new Vector3Class(0, verticalOffsetPercentage * sideDHeightDifference, 0);
                        }

                        model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += correctionVector;

                    }
                }
            }

            //move model back to normal position
            moveYFactor = (maxBuildSizeY / 2);
            for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    model.Triangles[arrayIndex][triangleIndex].Vectors[0].Position += new Vector3Class(0, moveYFactor, 0);
                    model.Triangles[arrayIndex][triangleIndex].Vectors[1].Position += new Vector3Class(0, moveYFactor, 0);
                    model.Triangles[arrayIndex][triangleIndex].Vectors[2].Position += new Vector3Class(0, moveYFactor, 0);
                }
            }

            //find highest vector and calc the scaling factor with original factor
            var scaleFactorY = maxBuildSizeY / sideABHeight;
            if (sideBDHeight > sideABHeight)
            {
                scaleFactorY = maxBuildSizeY / sideBDHeight;
            }

            var resolutionScalingFactor = maxBuildSizeX / 192f;

            var modelScaleFactorX = ((maxBuildSizeX / sideB) / BasicCorrectionModel.ModelDefaultCorrectionFactorX) / resolutionScalingFactor * (float)selectedMaterial.ShrinkFactor * model.ScaleFactorX;
            var modelScaleFactorY = ((scaleFactorY / BasicCorrectionModel.ModelDefaultCorrectionFactorY) / resolutionScalingFactor) * (float)selectedMaterial.ShrinkFactor * model.ScaleFactorY;
            model.Scale(modelScaleFactorX, 1, 1, Events.ScaleEventArgs.TypeAxis.X, false);
            model.Scale(1, modelScaleFactorY, 1, Events.ScaleEventArgs.TypeAxis.Y, false);
            model.Scale(1, 1, model.ScaleFactorZ * (float)selectedMaterial.ShrinkFactor, Events.ScaleEventArgs.TypeAxis.Z, false);

            //combine movetranslation with scaled vector
            if (model is SupportCone)
            {
                model.MoveTranslation += modelTranslation;
            }

            model.UpdateBoundries();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //update triangle normals
            foreach (var triangleArray in model.Triangles)
            {
                foreach (var triangle in triangleArray)
                {
                    triangle.CalcNormal();
                }
            }

            if (!(model is SupportCone))
            {
                if (model.SupportStructure != null)
                {
                    foreach (var supportCone in model.TotalObjectSupportCones)
                    {
                        Calculate(supportCone, selectedMaterial, new Vector3Class(), maxBuildSizeX, maxBuildSizeY, sideABHeight, sideBDHeight, sideBCAngleOffset, sideABAngleOffset, sideABAngle, sideBCAngle, sideB, calibrationModelSizeX, calibrationModelSizeY);
                    }
                }
            }

            if (model.SupportBasementStructure != null)
            {
                Calculate(model.SupportBasementStructure, selectedMaterial, modelTranslation, maxBuildSizeX, maxBuildSizeY, sideABHeight, sideBDHeight, sideBCAngleOffset, sideABAngleOffset, sideABAngle, sideBCAngle, sideB, calibrationModelSizeX, calibrationModelSizeY);
            }
        }
    }

}
