using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// An implementation of a shelf packing strategy with largest item first. In this strategy the packing area
    /// is divided into 'shelves' over the largest side and objects are fit in the shelf from big to small until no objects can fit anymore.
    /// </summary>
    public class ShelfPackLargestFirstPackingStrategy : IPackingStrategy
    {
        public PackingSolutions CreateSolutions(PackModelsRequest packModelsRequest)
        {
            PackingSolutions packingSolutions = new PackingSolutions();

            Stopwatch stopwatch = Stopwatch.StartNew();

            int theoreticalMaxCloneCount = packModelsRequest.DetermineMaximumCloneCountBasedOnSurfaceArea();
            int maxCloneCount = theoreticalMaxCloneCount;
            if (packModelsRequest.RequestedCloneCount.HasValue && packModelsRequest.RequestedCloneCount.Value > 0)
                maxCloneCount = Math.Min(packModelsRequest.RequestedCloneCount.Value, theoreticalMaxCloneCount);

            for (int cloneCount = maxCloneCount; cloneCount > 0; cloneCount--)
            {
                PackingSolution solution = CreateSolution(packModelsRequest, cloneCount);
                packingSolutions.Solutions.Add(solution);

                if (solution.CloneCount == maxCloneCount || solution.CloneCount >= cloneCount || !solution.FootprintCloneCount.Any(f=>f != solution.CloneCount))
                    break;
            }

            stopwatch.Stop();
            packingSolutions.TotalDurationMs = stopwatch.ElapsedMilliseconds;

            return packingSolutions;
        }

        public PackingSolution CreateSolution(PackModelsRequest packModelsRequest, int maxCloneCount)
        {
            PackingSolution packingSolution = new PackingSolution();
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Get model footprints based on area size
            packingSolution.Footprints = packModelsRequest.ModelFootprints.OrderByDescending(footprint => footprint.Area).Select(f=>f.Clone()).ToList();
            packingSolution.Footprints.ForEach(f => f.CloneCount = 0);
            packingSolution.FootprintCloneCount = new int[packingSolution.Footprints.Count];

            bool buildPlatformFull = false;
            Container buildPlatformContainer = Container.Create(packModelsRequest.BuildPlatform);
            Container remainingBuildPlatformArea = buildPlatformContainer;

            while (!buildPlatformFull)
            {
                var firstModelFootprintToPack = GetBestFitModelFootprint(packModelsRequest, maxCloneCount, packingSolution.Footprints, packingSolution.FootprintCloneCount, remainingBuildPlatformArea);

                if (firstModelFootprintToPack != null)
                {
                    // Create shelf
                    Container shelf = new Container()
                    {
                        PositionX = remainingBuildPlatformArea.PositionX,
                        PositionY = remainingBuildPlatformArea.PositionY,
                        SizeX = firstModelFootprintToPack.SizeX,
                        SizeY = packModelsRequest.BuildPlatform.SizeY
                    };
                    buildPlatformContainer.AddChild(shelf);
                    FillContainer(shelf, packModelsRequest, maxCloneCount, packingSolution.Footprints, packingSolution.FootprintCloneCount);

                    float newPositionX = shelf.PositionX + shelf.SizeX + packModelsRequest.Clearance;
                    remainingBuildPlatformArea = new Container()
                    {
                        PositionX = newPositionX,
                        SizeX = packModelsRequest.BuildPlatform.SizeX - newPositionX,
                        PositionY = 0,
                        SizeY = packModelsRequest.BuildPlatform.SizeY
                    };
                }

                ModelFootprint modelFootprint = GetBestFitModelFootprint(packModelsRequest, maxCloneCount, packingSolution.Footprints, packingSolution.FootprintCloneCount, remainingBuildPlatformArea);
                buildPlatformFull = modelFootprint == null;
            }

            var allContainers = buildPlatformContainer.GetAll();
            var allItems = allContainers.SelectMany(c => c.Items);
            packingSolution.PackedItems.AddRange(allItems);
            packingSolution.UnusedSpaces.AddRange(allContainers.Where(s => !s.Items.Any()));
            packingSolution.UnusedSpaces.Add(remainingBuildPlatformArea);
            packingSolution.CloneCount = packingSolution.FootprintCloneCount.Min();

            stopwatch.Stop();
            packingSolution.DurationMs = stopwatch.ElapsedMilliseconds;
            return packingSolution;
        }

        private static void FillContainer(Container container, PackModelsRequest packModelsRequest, int maxCloneCount, List<ModelFootprint> orderedModelFootprints, int[] modelsCloneCount)
        {
            ModelFootprint modelFootprint = GetBestFitModelFootprint(packModelsRequest, maxCloneCount, orderedModelFootprints, modelsCloneCount, container);
            if (modelFootprint != null)
            {
                int modelIndex = orderedModelFootprints.IndexOf(modelFootprint);
                modelsCloneCount[modelIndex]++;
                modelFootprint.CloneCount++;

                PackedItem item = PackedItem.Create(modelFootprint);
                item.CloneNumber = modelsCloneCount[modelIndex];
                item.ClearanceY = packModelsRequest.Clearance;
                item.ClearanceX = packModelsRequest.Clearance;
                item.PositionX = container.PositionX;
                item.PositionY = container.PositionY;
                container.Items.Add(item);

                Container spaceNextToModel = new Container()
                {
                    SizeX = container.SizeX - item.TotalSizeX,
                    SizeY = item.ModelFootprint.SizeY,
                    PositionX = item.PositionX + item.TotalSizeX,
                    PositionY = item.PositionY
                };

                if (spaceNextToModel.SizeX > packModelsRequest.Clearance)
                {
                    container.AddChild(spaceNextToModel);
                    FillContainer(spaceNextToModel, packModelsRequest, maxCloneCount, orderedModelFootprints, modelsCloneCount);
                }

                Container remainingSpaceAboveModel = new Container()
                {
                    SizeX = container.SizeX,
                    SizeY = container.SizeY - item.TotalSizeY,
                    PositionX = container.PositionX,
                    PositionY = container.PositionY + item.TotalSizeY
                };

                if (remainingSpaceAboveModel.SizeY > packModelsRequest.Clearance)
                {
                    container.AddChild(remainingSpaceAboveModel);
                    FillContainer(remainingSpaceAboveModel, packModelsRequest, maxCloneCount, orderedModelFootprints, modelsCloneCount);
                }
            }
        }

        private static ModelFootprint GetBestFitModelFootprint(PackModelsRequest packModelsRequest, int maxCloneCount, List<ModelFootprint> orderedModelFootprints, int[] modelsCloneCount, Container containerArea)
        {
            // Check best orientation for model footprints for this container
            orderedModelFootprints.DetermineOptimalModelsOrientation(containerArea, packModelsRequest.Clearance);
            // Get the first biggest model footprint which still needs to be packed
            var modelFootprintToPack = orderedModelFootprints.FirstOrDefault(footprint => modelsCloneCount[orderedModelFootprints.IndexOf(footprint)] < maxCloneCount && modelsCloneCount[orderedModelFootprints.IndexOf(footprint)] < footprint.RequestedCloneCount && containerArea.Fits(footprint));
            // If all models are packed with the max clone count to get an equal amount of models, get the next model that fits to fill up the space
            if (modelFootprintToPack == null)
            {
                modelFootprintToPack = orderedModelFootprints.FirstOrDefault(footprint => modelsCloneCount[orderedModelFootprints.IndexOf(footprint)] < footprint.RequestedCloneCount && containerArea.Fits(footprint));
            }

            return modelFootprintToPack;
        }

        private bool allModelsPlaced(int[] modelsCloneCount, int maxCloneCount)
        {
            return !modelsCloneCount.Any(c => c != maxCloneCount);
        }
    }
}
