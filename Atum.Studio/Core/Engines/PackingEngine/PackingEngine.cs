using Atum.DAL.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// Engine to pack a number of modelclones onto a build plate using different packing strategies to get the most fit clones solution
    /// </summary>
    public class PackingEngine
    {
        public List<IPackingStrategy> PackingStrategies { get; set; }

        public PackingEngine()
        {
            PackingStrategies = new List<IPackingStrategy>();
        }

        public PackingSolutions AnalyzeSolutions(PackModelsRequest packModelsRequest)
        {
            PackingSolutions allPackingSolutions = new PackingSolutions();
            Stopwatch stopwatch = Stopwatch.StartNew();

            getSolutions(packModelsRequest, allPackingSolutions, stopwatch);

            determineBestSolution(packModelsRequest, allPackingSolutions, stopwatch);

            stopwatch.Stop();

            allPackingSolutions.TotalDurationMs = stopwatch.ElapsedMilliseconds;
            LoggingManager.WriteToLog("PackingEngine", "Completed in", stopwatch.ElapsedMilliseconds + "ms");

            return allPackingSolutions;
        }

        /// <summary>
        /// Run all packingstrategies to get their solutions
        /// </summary>
        /// <param name="packModelsRequest"></param>
        /// <param name="allPackingSolutions"></param>
        /// <param name="stopwatch"></param>
        private void getSolutions(PackModelsRequest packModelsRequest, PackingSolutions allPackingSolutions, Stopwatch stopwatch)
        {
            foreach (IPackingStrategy packingStrategy in PackingStrategies)
            {
                getSolutions(packModelsRequest, allPackingSolutions, packingStrategy);
            }

            LoggingManager.WriteToLog("PackingEngine", "Get solutions for strategies", stopwatch.ElapsedMilliseconds + "ms");
        }

        private void determineBestSolution(PackModelsRequest packModelsRequest, PackingSolutions allPackingSolutions, Stopwatch stopwatch)
        {
            // TODO: implement determine best solution based on number of clones and biggest unused area left

            var packingSolutionsWithRequestedAmountOfClones = new List<PackingSolution>();
            if (packModelsRequest.PackingType == TypeOfPacking.DefinedNumbersOfClones)
            {
                foreach (var packageSolution in allPackingSolutions.Solutions)
                {
                    var allModelRequestsAreValid = true;
                    foreach (var modelRequest in packModelsRequest.ModelFootprints)
                    {
                        if (modelRequest.RequestedCloneCount < 5000)
                        {
                            var amountOfPackedModels = packageSolution.PackedItems.Count(s => s.ModelFootprint.Model == modelRequest.Model);
                            if (modelRequest.RequestedCloneCount >= amountOfPackedModels && amountOfPackedModels >= modelRequest.RequestedCloneCount - 1)
                            {

                            }
                            else
                            {
                                allModelRequestsAreValid = false;
                                break;
                            }
                        }
                    }

                    if (allModelRequestsAreValid)
                    {
                        packingSolutionsWithRequestedAmountOfClones.Add(packageSolution);
                    }
                }

                if (packingSolutionsWithRequestedAmountOfClones.Count > 0)
                {
                    //best solution is where there are as much as possible models
                    allPackingSolutions.BestSolution = packingSolutionsWithRequestedAmountOfClones.OrderByDescending(s => s.CloneCount).FirstOrDefault();
                }
                else
                {
                    allPackingSolutions.BestSolution = null;
                }
            }
            else
            {
                allPackingSolutions.BestSolution = allPackingSolutions.Solutions.OrderByDescending(s => s.CloneCount).FirstOrDefault();
            }


            LoggingManager.WriteToLog("PackingEngine", "Determine best solution", stopwatch.ElapsedMilliseconds + "ms");
        }

        private static void getSolutions(PackModelsRequest packModelsRequest, PackingSolutions allPackingSolutions, IPackingStrategy packingStrategy)
        {
            try
            {
                PackingSolutions packingSolutions = packingStrategy.CreateSolutions(packModelsRequest);
                allPackingSolutions.Solutions.AddRange(packingSolutions.Solutions);
            }
            catch (Exception ex)
            {
                LoggingManager.WriteToLog("PackingEngine", "Get solutions for strategy - " + packingStrategy.GetType().Name, ex);
            }
        }
    }
}
