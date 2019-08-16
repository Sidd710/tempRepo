using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAISupportConeOutlinePoints: List<Vector3Class>
    {
        internal MagsAISupportConeOutlinePoints(List<Vector3Class> values)
        {
            this.Clear();
            this.AddRange(values);
        }

        internal List<PointF> AsDrawingPointF()
        {
            var k = new List<PointF>();
            k.Clear();
            foreach (var point in this)
            {
                var t = new PointF((point.X * 10), (point.Y) * 10);
                t.X += (1920 / 2);
                t.Y += (1200 / 2);
                k.Add(t);
            }

            if (k.Count > 0)
            {
                k.Add(k[0]);
            }

            return k;
        }

        internal List<IntPoint> AsIntPoint()
        {
            var k = new List<IntPoint>();
            k.Clear();
            foreach (var point in this)
            {
                var t = new IntPoint(point);
                k.Add(t);
            }

            return k;
        }
    }
}
