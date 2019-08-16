using Atum.Studio.Core.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.ModelCorrections
{
    internal static class Trapezoid
    {
        internal static void Calculate(STLModel3D model)
        {
            model.UpdateBoundries();

            //properties of right part of trapezoid
            var sideBCAngleOffset = 0f;
            var sideBCAngle = 90d;
            var sideBDHeight = Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputC;
            if (Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputA != 70 || Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputB != 120 ||
                Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputC != 70 || Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputD != 120)
            {
                sideBCAngleOffset = (float)(Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputC, 2) - Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputF, 2) + Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputD, 2)) / (2 * Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputD);
                sideBCAngle = MathHelper.RadiansToDegrees(Math.Acos(-sideBCAngleOffset / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputF));
                sideBDHeight = (float)Math.Sqrt(Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputC, 2) - Math.Pow(sideBCAngleOffset, 2));
            }

            //properties of left part of trapezoid
            var sideABAngleOffset = 0f;
            var sideABAngle = 90d;
            var sideABHeight = Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputA;
            if (Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputA != 70 || Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputB != 120 ||
                Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputC != 70 || Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputD != 120)
            {
                sideABAngleOffset = (float)(Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputA, 2) - Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputE, 2) + Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputB, 2)) / (2 * Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputB);
                sideABAngle = MathHelper.RadiansToDegrees(Math.Acos(-sideABAngleOffset / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputE));
                sideABHeight = (float)Math.Sqrt(Math.Pow(Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputA, 2) - Math.Pow(sideABAngleOffset, 2));
            }
            model.UpdateBoundries();
            var moveYFactor = -model.FrontPoint;

            for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    model.Triangles[arrayIndex][triangleIndex].Vectors[0].Position += new Vector3(0, moveYFactor, 0);
                    model.Triangles[arrayIndex][triangleIndex].Vectors[1].Position += new Vector3(0, moveYFactor, 0);
                    model.Triangles[arrayIndex][triangleIndex].Vectors[2].Position += new Vector3(0, moveYFactor, 0);
                }
            }

            model.UpdateBoundries();

            var modelWidth = model.RightPoint - model.LeftPoint;

            float sideDHeightDifference = (float)(sideABHeight - sideBDHeight);
            if (sideDHeightDifference < 0) { sideDHeightDifference = -sideDHeightDifference; }
            if (sideABAngle <= 90)    //angle A forward /
            {
                var leftSideMaxCorrection = new Vector3((sideABAngleOffset), ((float)(sideABHeight - sideBDHeight)), 0);
                if (sideABHeight >= sideBDHeight) //highest point at right side
                {
                    leftSideMaxCorrection = new Vector3(leftSideMaxCorrection.X, 0, 0); //NO Y correction
                }

                //change model vectors
                for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            var orgPoint = model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;
                            if (orgPoint.X <= 0)
                            {
                                var vectorOffSetPercentageX = orgPoint.Y / (float)sideABHeight;
                                var vectorOffSetPercentageY = 1 - (-orgPoint.X / (modelWidth / 2));

                                var vectorOffSetPercentageYOffset = vectorOffSetPercentageY * (sideDHeightDifference / 2);

                                var correctionVectorY = (vectorOffSetPercentageX * vectorOffSetPercentageYOffset);
                                correctionVectorY = sideABAngle >= sideBDHeight ? -correctionVectorY : correctionVectorY;
                                var correctionVector = new Vector3(leftSideMaxCorrection.X * vectorOffSetPercentageX, correctionVectorY, 0);
                                model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += correctionVector;
                            }
                        }
                    }
                }
            }
            else if (sideABAngle > 90)    //angle A backward
            {
                var leftSideMaxCorrection = new Vector3(sideABAngleOffset, sideDHeightDifference, 0);
                if (sideABHeight >= sideBDHeight) //highest point at right side
                {
                    leftSideMaxCorrection = new Vector3(leftSideMaxCorrection.X, 0, 0); //NO Y correction
                }

                //change model vectors
                for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            var orgPoint = model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;
                            if (orgPoint.X <= 0)
                            {
                                var vectorOffSetPercentageX = orgPoint.Y / sideABHeight;
                                var vectorOffSetPercentageY = 1 - (-orgPoint.X / (modelWidth / 2));

                                var vectorOffSetPercentageYOffset = vectorOffSetPercentageY * (sideDHeightDifference / 2);
                                var correctionVectorY = (vectorOffSetPercentageX * vectorOffSetPercentageYOffset);
                                correctionVectorY = sideABAngle >= sideBDHeight ? correctionVectorY : -correctionVectorY;
                                var correctionVector = new Vector3(leftSideMaxCorrection.X * vectorOffSetPercentageX, correctionVectorY, 0);

                                model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += correctionVector;
                            }
                        }
                    }
                }
            }

            if (sideBCAngle <= 90)    //angle C forward \
            {
                var rightSideMaxCorrection = new Vector3(-sideBCAngleOffset, sideDHeightDifference, 0);

                if (sideABHeight < sideBDHeight) //highest point at right side
                {
                    rightSideMaxCorrection = new Vector3(rightSideMaxCorrection.X, 0, 0); //NO Y correction
                }

                //change model vectors
                for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            //var vectorOffSetPercentage = 0f;
                            var orgPoint = model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;

                            if (orgPoint.X > 0)
                            {
                                var vectorOffSetPercentageX = orgPoint.Y / (float)sideBDHeight;
                                var vectorOffSetPercentageY = ((modelWidth / 2) + orgPoint.X) / modelWidth;
                                var correctionVectorX = vectorOffSetPercentageX * rightSideMaxCorrection.X;
                                var correctionVectorY = (vectorOffSetPercentageY * vectorOffSetPercentageX * sideDHeightDifference);
                                correctionVectorY = sideABHeight < correctionVectorY ? -correctionVectorY : correctionVectorY;
                                model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += new Vector3(-correctionVectorX, correctionVectorY, 0);
                            }
                        }
                    }
                }
            }
            if (sideBCAngle > 90)    //angle C backward /
            {
                var rightSideMaxCorrection = new Vector3(-sideBCAngleOffset, sideDHeightDifference, 0);

                if (sideABHeight < sideBDHeight) //highest point at right side
                {
                    rightSideMaxCorrection = new Vector3(rightSideMaxCorrection.X, 0, 0); //NO Y correction
                }

                //change model vectors
                for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            var orgPoint = model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;
                            if (orgPoint.X > 0)
                            {
                                var vectorOffSetPercentageX = orgPoint.Y / (float)sideBDHeight;
                                var vectorOffSetPercentageY = ((modelWidth / 2) + orgPoint.X) / modelWidth;
                                var correctionVectorX = vectorOffSetPercentageX * rightSideMaxCorrection.X;
                                var correctionVectorY = (vectorOffSetPercentageY * vectorOffSetPercentageX * sideDHeightDifference);
                                correctionVectorY = sideABHeight < correctionVectorY ? correctionVectorY : -correctionVectorY;
                                model.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += new Vector3(-correctionVectorX, correctionVectorY, 0);

                            }
                        }
                    }
                }
            }

            model.Scale(144 / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionInputB, 1, 1, Events.ScaleEventArgs.TypeAxis.X);
            model.Scale(1, 90f / Math.Max(sideABHeight, sideBDHeight), 1, Events.ScaleEventArgs.TypeAxis.Y);
            model.UpdateDefaultCenter();

            //var rotate model with angle of side D

            if (model.SupportStructure != null)
            {
                foreach (var supportCone in model.SupportStructure)
                {
                    ModelCorrections.Trapezoid.Calculate(supportCone);
                }
            }

            if (model.Triangles.HorizontalSurfaces != null && model.Triangles.HorizontalSurfaces.SupportStructure != null)
            {
                foreach (var supportCone in model.Triangles.HorizontalSurfaces.SupportStructure)
                {
                    ModelCorrections.Trapezoid.Calculate(supportCone);
                }
            }

            if (model.Triangles.FlatSurfaces != null && model.Triangles.FlatSurfaces.SupportStructure != null)
            {
                foreach (var supportCone in model.Triangles.FlatSurfaces.SupportStructure)
                {
                    ModelCorrections.Trapezoid.Calculate(supportCone);
                }
            }

            if (model.SupportBasementStructure != null)
            {
                ModelCorrections.Trapezoid.Calculate(model.SupportBasementStructure);
            }
        }
    }

}
