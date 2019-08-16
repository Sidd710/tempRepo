using System;
using System.Collections.Generic;
using System.Text;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.FontTessellation;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Atum.Studio.Core.Engines
{
	public class FontTessellationEngine
	{

        public static TriangleInfoList ConvertStringToTrianglesWithOuterPath(string text, FontStyle fontStyle, Polygon outerPath, out PolygonSet textAsPolygonSet, float glyphFontSize = 50f)
        {

            FontFamily fontFamily = FontFamily.GenericSansSerif;
            textAsPolygonSet = new PolygonSet();

            var triangles = new TriangleInfoList();
            try
            {
                var textAsPolygons = GeneratePolygonsFromGlyph(fontFamily, fontStyle, text, glyphFontSize, true);
                textAsPolygonSet = CreateSetFromList(textAsPolygons);
                var polygonSetWithOuterPath = new PolygonSet();
                foreach (var t in textAsPolygonSet.Polygons)
                {
                    foreach (var hole in t.Holes)
                    {
                        polygonSetWithOuterPath.Add(hole);
                    }
                    //t.Holes.Clear();
                    outerPath.Holes.Add(t);
                }
                polygonSetWithOuterPath.Add(outerPath);


                P2T.Triangulate(polygonSetWithOuterPath);

                foreach (var polygon in polygonSetWithOuterPath.Polygons)
                {

                    foreach (var polyTriangle in polygon.Triangles)
                    {
                        var triangle = new Triangle();
                        var triangleIndex = 0;
                        foreach (var trianglePoint in polyTriangle.Points)
                        {
                            triangle.Vectors[triangleIndex].Position.X = (float)trianglePoint.X;
                            triangle.Vectors[triangleIndex].Position.Y = (float)trianglePoint.Y;

                            triangleIndex++;
                        }

                        triangles[0].Add(triangle);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

            return triangles;
        }

        public static TriangleInfoList ConvertStringToTriangles (string text, FontStyle fontStyle, float glyphFontSize = 10f)
		{

			FontFamily fontFamily = FontFamily.GenericSansSerif;

			var triangles = new TriangleInfoList ();
			try {
				var textAsPolygons = GeneratePolygonsFromGlyph (fontFamily, fontStyle, text, glyphFontSize, false);
				var polygonSet = CreateSetFromList (textAsPolygons);
				P2T.Triangulate (polygonSet);

				foreach (var polygon in polygonSet.Polygons) {

					foreach (var polyTriangle in polygon.Triangles) {
						var triangle = new Triangle ();
						var triangleIndex = 0;
						foreach (var trianglePoint in polyTriangle.Points) {
							triangle.Vectors [triangleIndex].Position.X = (float)trianglePoint.X;
							triangle.Vectors [triangleIndex].Position.Y = (float)trianglePoint.Y;

							triangleIndex++;
						}

						triangles[0].Add (triangle);
					}
				}
			} catch (Exception exc) {
				Debug.WriteLine (exc);
			}

			return triangles;
		}

		static PolygonSet CreateSetFromList (IList<Polygon> source)
		{
			// First we need to reorganize the polygons
			var root = new PolygonHierachy (source [0]);

			for (var i = 1; i < source.Count; ++i) {
				ProcessLevel (source [i], ref root);
			}

			// Generate the set from the hierachy
			var set = new PolygonSet ();
			ProcessSetLevel (set, root);

			return set;
		}

		class PolygonHierachy
		{
			public Polygon Current;
			public List<PolygonHierachy> Childs;
			public PolygonHierachy Next;

			public PolygonHierachy (Polygon current)
			{
				Current = current;
				Childs = new List<PolygonHierachy> ();
				Next = null;
			}
		}

		static bool CheckIfInside (
			IList<TriangulationPoint> polygonToTest,
			IList<TriangulationPoint> containingPolygon)
		{
			var t = 0;
			for (var i = 0; i < polygonToTest.Count; ++i) {
				if (PointInPolygon (polygonToTest [i], containingPolygon))
					t++;
			}

			return ((float)t) >= (polygonToTest.Count * .1f) ? true : false;
		}

		static bool PointInPolygon (TriangulationPoint p, IList<TriangulationPoint> poly)
		{
			PolygonPoint p1, p2;
			var inside = false;
			var oldPoint = new PolygonPoint (poly [poly.Count - 1].X, poly [poly.Count - 1].Y);

			for (var i = 0; i < poly.Count; i++) {
				var newPoint = new PolygonPoint (poly [i].X, poly [i].Y);
				if (newPoint.X > oldPoint.X) {
					p1 = oldPoint;
					p2 = newPoint;
				} else {
					p1 = newPoint;
					p2 = oldPoint;
				}
				if ((newPoint.X < p.X) == (p.X <= oldPoint.X) && ((long)p.Y - (long)p1.Y) * (long)(p2.X - p1.X)
					< ((long)p2.Y - (long)p1.Y) * (long)(p.X - p1.X)) {
					inside = !inside;
				}
				oldPoint = newPoint;
			}
			return inside;
		}

		static void ProcessLevel (Polygon poly, ref PolygonHierachy localRoot)
		{
			if (localRoot == null) {
				localRoot = new PolygonHierachy (poly);
				return;
			}

			// Check if source is the new root
			if (CheckIfInside (localRoot.Current.Points, poly.Points)) {
				var nroot = new PolygonHierachy (poly);
				var tmp = localRoot;
				while (tmp != null) {
					var cur = tmp;
					tmp = tmp.Next;
					cur.Next = null;
					nroot.Childs.Add (cur);
				}

				localRoot = nroot;
				return;
			}



			// Check if source is not in the local root
			if (!CheckIfInside (poly.Points, localRoot.Current.Points)) {
				ProcessLevel (poly, ref localRoot.Next);
				return;
			}

			// Now process the childs
			for (var i = 0; i < localRoot.Childs.Count; ++i) {
				if (!CheckIfInside (poly.Points, localRoot.Childs [i].Current.Points))
					continue;

				// Process to the child level
				var childRoot = localRoot.Childs [i];
				ProcessLevel (poly, ref childRoot);
				localRoot.Childs [i] = childRoot;
				return;
			}

			// Else -> new child
			var newChildList = new List<PolygonHierachy> ();
			var newPoly = new PolygonHierachy (poly);
			newChildList.Add (newPoly);
			for (var i = 0; i < localRoot.Childs.Count; ++i) {
				if (CheckIfInside (localRoot.Childs [i].Current.Points, poly.Points)) {
					newPoly.Childs.Add (localRoot.Childs [i]);
				} else {
					newChildList.Add (localRoot.Childs [i]);
				}
			}

			localRoot.Childs = newChildList; //.Childs.Add(new PolygonHierachy(poly));
		}

		static void ProcessSetLevel (PolygonSet set, PolygonHierachy current)
		{
			while (current != null) {
				var poly = current.Current;
				foreach (var child in current.Childs) {
					poly.AddHole (child.Current);
					foreach (var grandchild in child.Childs)
						ProcessSetLevel (set, grandchild);
				}
				set.Add (poly);
				current = current.Next;
			}
		}

		/// <summary>
		/// Construct the "raw" (no hole, unsorted) polygon list from the GDI+ paths
		/// </summary>
		/// <param name="fx">Graphics for debug output</param>
		/// <param name="fontFamily">Font family</param>
		/// <param name="style">Font style</param>
		/// <param name="glyph">String containing the glyph to write</param>
		/// <returns>List of polygon</returns>
		static List<Polygon> GeneratePolygonsFromGlyph (
			FontFamily fontFamily, FontStyle style,
			string glyph, float glyphFontSize, bool useCenterPosition)
		{
			var polygons = new List<Polygon> (); 
			try {
				PointF[] pts = null;
				byte[] ptsType = null;

				using (var path = new GraphicsPath()) {
                    var stringFormat = StringFormat.GenericDefault;
                    if (useCenterPosition)
                    {
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                    }

                    path.AddString (glyph, fontFamily, (int)style, glyphFontSize,
					                          new PointF (0, 0), stringFormat);

					path.Flatten ();

					if (path.PointCount == 0)
						return new List<Polygon> ();

					pts = path.PathPoints;
					ptsType = path.PathTypes;
				}

            
				List<PolygonPoint> points = null;
				var start = -1;

				for (var i = 0; i < pts.Length; i++) {
					var pointType = ptsType [i] & 0x07;
					if (pointType == 0) {
						points = new List<PolygonPoint> { new PolygonPoint(pts[i].X, pts[i].Y) };
						start = i;
						continue;
					}

					if (!DAL.OS.OSProvider.IsWindows) {
						//mono has no 0x80 endpoint
						if (i < ptsType.Length && ptsType [i + 1] == 0) {
							//- Last point in the polygon
							if (pts [i] != pts [start]) {
								points.Add (new PolygonPoint (pts [i].X, pts [i].Y));
							}
							polygons.Add (new Polygon (points));

							points = null;
						} else {
							points.Add (new PolygonPoint (pts [i].X, pts [i].Y));
						}

					} else {
						//windows
						if ((ptsType [i] & 0x80) != 0) {
							//- Last point in the polygon
							if (pts [i] != pts [start]) {
								points.Add (new PolygonPoint (pts [i].X, pts [i].Y));
							}
							polygons.Add (new Polygon (points));

							points = null;
						} else {
							points.Add (new PolygonPoint (pts [i].X, pts [i].Y));
						}
					}
				}
			} catch (Exception exc) {
				Debug.WriteLine (exc.Message);
			}

			return polygons;
		}
	}
}
