using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAISurfaceWithOverhangSupportCones : TriangleSurfaceInfo
    {
        //internal ExtrudedSurfaceModel ExtrudeModel { get; set; }
        internal SortedDictionary<float, List<List<IntPoint>>> VerticalSurfaceModelPolygons { get; set; }
        internal float MaxAngle { get; set; }

        internal List<Vector3> DebugPoints = new List<Vector3>();

        internal Dictionary<MagsAISurfaceIntersectionData, SupportCone> SupportCones = new Dictionary<MagsAISurfaceIntersectionData, SupportCone>();

        internal MagsAISurfaceWithOverhangSupportCones(TriangleSurfaceInfo surface)
        {
            foreach (var surfaceTriangleIndex in surface.Keys)
            {
                this.Add(surfaceTriangleIndex, false);
            }
        }

        internal void AddSupportCone(SupportCone supportCone, MagsAISurfaceIntersectionData magsAIIntersectionData)
        {
            foreach (var triangleIntersection in this.Keys)
            {
                if (triangleIntersection.ArrayIndex == supportCone.ModelIntersectedTriangle.ArrayIndex && triangleIntersection.TriangleIndex == supportCone.ModelIntersectedTriangle.TriangleIndex)
                {
                    this.SupportCones.Add(magsAIIntersectionData, supportCone);
                    break;
                }
            }
        }

        internal void FilterSupportCones()
        {
            //find all available overhangdistancefactors
            var availableOverhangDistanceFactors = new SortedList<float, bool>();
            foreach (var surfaceIntersectionData in this.SupportCones.Keys)
            {
                if (!availableOverhangDistanceFactors.ContainsKey(surfaceIntersectionData.OverhangDistance))
                {
                    availableOverhangDistanceFactors.Add(surfaceIntersectionData.OverhangDistance, false);
                }
            }

            var currentDistanceIndex = 0;
            //find all support cones that are lower

            while (currentDistanceIndex < availableOverhangDistanceFactors.Count - 2)
            {
                var currentDistanceOverhangFactor = availableOverhangDistanceFactors.ElementAt(currentDistanceIndex).Key;
                var currentSupportConesWithSameOverhangDistance = new Dictionary<MagsAISurfaceIntersectionData, SupportCone>();
                var currentSupportConesWithLargerOverhangDistance = new Dictionary<MagsAISurfaceIntersectionData, SupportCone>();
                foreach (var surfaceIntersectionData in this.SupportCones.Keys)
                {
                    if (surfaceIntersectionData.OverhangDistance > currentDistanceOverhangFactor && surfaceIntersectionData.Filter == typeOfAutoSupportFilter.None)
                    {
                        currentSupportConesWithLargerOverhangDistance.Add(surfaceIntersectionData, this.SupportCones[surfaceIntersectionData]);
                    }
                    else if (surfaceIntersectionData.OverhangDistance == currentDistanceOverhangFactor && surfaceIntersectionData.Filter == typeOfAutoSupportFilter.None)
                    {
                        currentSupportConesWithSameOverhangDistance.Add(surfaceIntersectionData, this.SupportCones[surfaceIntersectionData]);
                    }
                }

                //find all support cones with larger distance factor that are below current support cone and within range
                foreach (var currentSupportConeWithSameOverhangDistance in currentSupportConesWithSameOverhangDistance)
                {
                    var intersectedSupportCones = new Dictionary<MagsAISurfaceIntersectionData, SupportCone>();
                    foreach (var currentSupportConeWithLargerOverhangDistance in currentSupportConesWithLargerOverhangDistance)
                    {
                        if (currentSupportConeWithLargerOverhangDistance.Key.SliceHeight <= currentSupportConeWithSameOverhangDistance.Key.SliceHeight)
                        {
                            intersectedSupportCones.Add(currentSupportConeWithLargerOverhangDistance.Key, currentSupportConeWithLargerOverhangDistance.Value);
                        }
                    }

                    //calc support cone contour
                    var currentSupportConeWithSameOverhangDistanceContour = MagsAIEngine.ConvertSupportPointsToCircles(currentSupportConeWithSameOverhangDistance.Key.TopPoint, 5);

                    foreach (var intersectedSupportCone in intersectedSupportCones.Keys)
                    {
                        if (Clipper.PointInPolygon(intersectedSupportCone.TopPoint, currentSupportConeWithSameOverhangDistanceContour) != 0)
                        {
                            intersectedSupportCone.Filter = typeOfAutoSupportFilter.FilteredBySupportConeOverhangWithinRange; //using internal references to update this.supportcones.keys
                        }
                    }
                }
                currentDistanceIndex++;
            }
        }
        
    }
}
