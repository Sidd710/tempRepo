using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Helpers
{
    class STLModelHelper
    {
        
        
        internal enum TypeOfSurface
        {
            Unknown = 0,
            Horizontal = 1,
            Flat = 2
        }

        internal static TypeOfSurface GetSurfaceType(STLModel3D stlModel,  TriangleSurfaceInfo currentSurface)
        {
            if (stlModel != null)
            {
                foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
                {
                    if (surface == currentSurface)
                    {
                        return TypeOfSurface.Horizontal;
                    }
                }

                foreach (var surface in stlModel.Triangles.FlatSurfaces)
                {
                    if (surface == currentSurface)
                    {
                        return TypeOfSurface.Flat;
                    }
                }
            }

            return TypeOfSurface.Unknown;
        }
    }
}
