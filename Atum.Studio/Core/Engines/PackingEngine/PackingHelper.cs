using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    public static class PackingHelper
    {
        public static PackingSolutions CalculatePackingSolutions(PackModelsRequest packModelsRequest)
        {
            PackingEngine packingEngine = new PackingEngine();
            packingEngine.PackingStrategies.Add(new ShelfPackLargestFirstPackingStrategy());

            var solutions = packingEngine.AnalyzeSolutions(packModelsRequest);

            return solutions;
        }

        public static PackModelsRequest CreatePackModelsRequest(int? requestedCloneCount)
        {
            PackModelsRequest packModelsRequest = new PackModelsRequest();

            foreach (var object3D in ObjectView.Objects3D)
            {
                if (object3D is STLModel3D && !(object3D is GroundPane))
                {
                    var stlModel = object3D as STLModel3D;
                    var modelFootprint = ModelFootprint.FromModel(stlModel);
                    packModelsRequest.ModelFootprints.Add(modelFootprint);
                }
            }

            var selectedPrinter = PrintJobManager.SelectedPrinter;
            var printerGroundPaneDimensionX = (selectedPrinter.ProjectorResolutionX / 10) * selectedPrinter.TrapeziumCorrectionFactorX;
            var printerGroundPaneDimensionY = (selectedPrinter.ProjectorResolutionY / 10) * selectedPrinter.TrapeziumCorrectionFactorY;

            packModelsRequest.BuildPlatform = new Core.Engines.PackingEngine.Rectangle()
            {
                SizeX = printerGroundPaneDimensionX,
                SizeY = printerGroundPaneDimensionY
            };

            packModelsRequest.Clearance = UserProfileManager.UserProfile.Settings_Models_ClearanceBetweenClones;
            packModelsRequest.RequestedCloneCount = requestedCloneCount;

            return packModelsRequest;
        }

        public static void DetermineOptimalModelsOrientation(this IEnumerable<ModelFootprint> modelFootprints, Rectangle container, float clearance)
        {
            foreach (ModelFootprint modelFootprint in modelFootprints)
            {
                modelFootprint.DetermineOptimalModelOrientation(container, clearance);
            }
        }

        public static void DetermineOptimalModelOrientation(this ModelFootprint modelFootprint, Rectangle container, float clearance)
        {
            // Optimal orientation is the orientation that allows the most amount of clones of the model in total on the buildplate, with clearance
            // TODO: there may be a better way to calculate with clearance since the clearance is not needed on the edges of the buildplatform

            // Normal orientation
            int normalOrientationRows = DetermineMaxRepetitions(container.SizeY, modelFootprint.SizeY + 0.0f * clearance);
            int normalOrientationColumns = DetermineMaxRepetitions(container.SizeX, modelFootprint.SizeX + 0.0f * clearance);
            int normalOrientationCloneCount = normalOrientationRows * normalOrientationColumns;

            // Rotated 90 degrees
            int rotatedOrientationRows = DetermineMaxRepetitions(container.SizeY, modelFootprint.SizeX + 0.0f * clearance);
            int rotatedOrientationColumns = DetermineMaxRepetitions(container.SizeX, modelFootprint.SizeY + 0.0f * clearance);
            int rotatedOrientationCloneCount = rotatedOrientationRows * rotatedOrientationColumns;
            bool rotateModel = rotatedOrientationCloneCount > normalOrientationCloneCount;
            //The model may already be rotated and SizeX and SizeY may already return the rotated value, so only apply rotation by inverting current value if needed
            if (rotateModel)
                modelFootprint.RotateModel = !modelFootprint.RotateModel;
        }

        private static int DetermineMaxRepetitions(float platformSize, float modelBoundingSize)
        {
            return (int)Math.Floor(platformSize / modelBoundingSize);
        }
    }
}
