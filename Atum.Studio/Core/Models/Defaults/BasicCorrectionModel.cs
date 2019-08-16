using Atum.DAL.Hardware;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Shapes.Custom;
using Atum.Studio.Core.Structs;
using OpenTK;
using System.Collections.Generic;

namespace Atum.Studio.Core.Models.Defaults
{
    internal class BasicCorrectionModel : STLModel3D
    {
        internal static float CUBESIZE = 10f;
        internal static float GroundPanelEdgeOffsetXY = 0f;
        internal static float SUPPORTBASEMENTHEIGHT = Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness;
        internal static float SupportBasementSize = 14f;

        public static float ModelSizeX
        {
            get
            {
                if (PrintJobManager.SelectedPrinter is AtumDLPStation5 || PrintJobManager.SelectedPrinter is LoctiteV10)
                {
                    switch (PrintJobManager.SelectedPrinter.PrinterXYResolution)
                    {
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                            return 160;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                            return 120; //120 //96
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                            return 80;
                    }
                }
                else
                {
                    switch (PrintJobManager.SelectedPrinter.PrinterXYResolution)
                    {
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                            return 160;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                            return 120; //120 //96
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                            return 80;
                    }
                }

                return 0;
            }
        }


        public static float ModelSizeY
        {
            get
            {
                if (PrintJobManager.SelectedPrinter is AtumDLPStation5 || PrintJobManager.SelectedPrinter is LoctiteV10)
                {
                    switch (PrintJobManager.SelectedPrinter.PrinterXYResolution)
                    {
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                            return 80;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                            return 60f;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                            return 40;
                    }
                }
                else
                {
                    switch (PrintJobManager.SelectedPrinter.PrinterXYResolution)
                    {
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                            return 100;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                            return 75;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                            return 50;
                    }
                }

                return 0;
            }
        }

        public static float ModelDefaultCorrectionFactorX
        {
            get
            {
                return 1.2f;
            }
        }

        public static float ModelDefaultCorrectionFactorY
        {
            get
            {
                if (PrintJobManager.SelectedPrinter is AtumDLPStation5 || PrintJobManager.SelectedPrinter is LoctiteV10)
                {
                    return 1.35f;
                }
                else
                {
                    return 1.2f;
                }
                
            }
        }

        internal BasicCorrectionModel()
          : base(STLModel3D.TypeObject.Model, true)
        {
            this.FileName = "Calibration";
            this.Triangles = new TriangleInfoList();
            GroundPane groundPane = ObjectView.GroundPane;
            float x = 75;
            float y = 45;

            if (PrintJobManager.SelectedPrinter is AtumDLPStation5 || PrintJobManager.SelectedPrinter is LoctiteV10)
            {
                switch (PrintJobManager.SelectedPrinter.PrinterXYResolution)
                {
                    case AtumPrinter.PrinterXYResolutionType.Micron100:
                        x = 75f;
                        y = 35f;
                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron75:
                        x = 55f;
                        y = 25f;
                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron50:
                        x = 35;
                        y = 15f;
                        break;
                }
            }
            else
            {
                switch (PrintJobManager.SelectedPrinter.PrinterXYResolution)
                {
                    case AtumPrinter.PrinterXYResolutionType.Micron75:
                        x = 55f; //55 //43
                        y = 32.5f; //32.5 //25
                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron50:
                        x = 35;
                        y = 20;
                        break;
                }
            }

            AtumBox atumBox1 = new AtumBox(CUBESIZE, CUBESIZE, CUBESIZE, true, false);
            atumBox1.Triangles.UpdateWithMoveTranslation(new Vector3Class(-x + CUBESIZE, y + CUBESIZE, SUPPORTBASEMENTHEIGHT));
            atumBox1.SupportBasement = true;

            this.Triangles[0].AddRange(atumBox1.Triangles[0]);
            this.Triangles[0].AddRange(atumBox1.SupportBasementStructure.Triangles[0]);

            AtumBox atumBox2 = new AtumBox(CUBESIZE, CUBESIZE, CUBESIZE, true, false);
            atumBox2.Rotate(0.0f, 0.0f, -270f, RotationEventArgs.TypeAxis.Z, updateFaceColor: false);
            atumBox2.Triangles.UpdateWithMoveTranslation(new Vector3Class(-x + CUBESIZE, -y + CUBESIZE, SUPPORTBASEMENTHEIGHT));
            atumBox2.SupportBasement = true;
            this.Triangles[0].AddRange(atumBox2.Triangles[0]);
            this.Triangles[0].AddRange(atumBox2.SupportBasementStructure.Triangles[0]);

            AtumBox atumBox3 = new AtumBox(CUBESIZE, CUBESIZE, CUBESIZE, true, false);
            atumBox3.Rotate(0.0f, 0.0f, 180f, RotationEventArgs.TypeAxis.Z, updateFaceColor: false);
            atumBox3.Triangles.UpdateWithMoveTranslation(new Vector3Class(x + CUBESIZE, -y + CUBESIZE, SUPPORTBASEMENTHEIGHT));
            atumBox3.SupportBasement = true;
            this.Triangles[0].AddRange(atumBox3.Triangles[0]);
            this.Triangles[0].AddRange(atumBox3.SupportBasementStructure.Triangles[0]);

            Triangle3D triangle3D = new Triangle3D(CUBESIZE, CUBESIZE, CUBESIZE, true, false);
            triangle3D.Rotate(0, 0, 180, RotationEventArgs.TypeAxis.Z, updateFaceColor: false);
            triangle3D.Triangles.UpdateWithMoveTranslation(new Vector3Class(x + CUBESIZE, y + CUBESIZE, SUPPORTBASEMENTHEIGHT));
            triangle3D.SupportBasement = true;
            this.Triangles[0].AddRange(triangle3D.Triangles[0]);
            this.Triangles[0].AddRange(triangle3D.SupportBasementStructure.Triangles[0]);
        }
    }
}
