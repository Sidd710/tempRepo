using Atum.Studio.Core.Engines;
using Atum.Studio.Core.FontTessellation;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Models.Defaults
{
    internal class LensWarpCorrectionModel : STLModel3D
    {
        internal LensWarpCorrectionModel(string text) : base(TypeObject.Model, true)
        {
            this.Triangles = new TriangleInfoList();

            var width = 12.8 * 10;
            var height = 8 * 10;

            var cubeIndex = 1;
            for (var rowIndex = 0; rowIndex < 7; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < 7; columnIndex++)
                {
                    CreateMeasureCube(new Vector2(-83.6f + (columnIndex * 25.6f) + 12.8f, -(52) + (rowIndex * 16) + 8f), cubeIndex.ToString(), width, height);

                    cubeIndex++;
                }
            }

            //this.Triangles[0].AddRange((new Cube(192f, 120f, 0.2f)).AsSTLModel3D(new DAL.Materials.Material()).Triangles[0]);

            this.UpdateBoundries();
            this.SupportBasement = true;
            this.BindModel();
            this.UpdateBinding();
            this.Loaded = true;
            this._color = Color.FromArgb(ObjectView.NextObjectIndex, Color.Gray);



            ObjectView.AddModel(this);

        }

        void CreateMeasureCube(Vector2 startPoint, string text, double width, double height)
        {
            //outerbox
            var cubeModel = new STLModel3D(TypeObject.None, false);
            //cubeModel = (new Cube(12.8f, 8f, 5f)).AsSTLModel3D(Managers.MaterialManager.DefaultMaterial);

            var outerPath = new Polygon(new PolygonPoint(-(width / 2), -(height / 2)), new PolygonPoint((width / 2), -(height / 2)), new PolygonPoint((width / 2), (height / 2)), new PolygonPoint(-(width / 2), (height / 2)));
            var textAsPolygonSet = new PolygonSet();
            var letterPolygonsTop = FontTessellationEngine.ConvertStringToTrianglesWithOuterPath(text, FontStyle.Bold, outerPath, out textAsPolygonSet);
            outerPath = new Polygon(new PolygonPoint(-(width / 2), -(height / 2)), new PolygonPoint((width / 2), -(height / 2)), new PolygonPoint((width / 2), (height / 2)), new PolygonPoint(-(width / 2), (height / 2)));
            var letterPolygonsBottom = FontTessellationEngine.ConvertStringToTrianglesWithOuterPath(text, FontStyle.Bold, outerPath, out textAsPolygonSet);

            foreach (var t in letterPolygonsBottom[0])
            {
                t.Flip();
            }

            cubeModel.Triangles = letterPolygonsTop;
            foreach (var t in letterPolygonsTop[0])
            {
                t.Vectors[0].Position += new Vector3Class(0, 0, 5);
                t.Vectors[1].Position += new Vector3Class(0, 0, 5);
                t.Vectors[2].Position += new Vector3Class(0, 0, 5);
            }

            cubeModel.Triangles[0].AddRange(letterPolygonsBottom[0]);

            cubeModel._scaleFactorX = cubeModel._scaleFactorY = cubeModel._scaleFactorZ = 1;
            cubeModel.Scale(0.1f, 0.1f, 1f, Events.ScaleEventArgs.TypeAxis.ALL, true, true);

            CreateExtrudeTriangles(cubeModel, outerPath.Points);
            foreach (var polygon in outerPath.Holes)
            {
                CreateExtrudeTriangles(cubeModel, polygon.Points, true);

                foreach (var holePolygon in polygon.Holes)
                {
                    CreateExtrudeTriangles(cubeModel, holePolygon.Points, true);
                }
            }

            //center triangle
            cubeModel.UpdateBoundries();
            cubeModel.UpdateDefaultCenter();
            cubeModel.Triangles.UpdateWithMoveTranslation(new Vector3Class(startPoint.X, startPoint.Y, 0) - new Vector3Class(cubeModel.LeftPoint, cubeModel.BackPoint, -0.2f));

            //orient model properly
            cubeModel.HorizontalMirror(false, true);
            cubeModel.Rotate(0, 0, 180, Events.RotationEventArgs.TypeAxis.Z);
            cubeModel.UpdateTrianglesMinMaxZ();

            this.Triangles[0].AddRange(cubeModel.Triangles[0]);

        }

        void CreateExtrudeTriangles(STLModel3D cube, IList<TriangulationPoint> pathPolygonPoints, bool flipTriangles = false)
        {
            //create extrude triangles from polygons
            for (var pathOuterPointIndex = 0; pathOuterPointIndex < pathPolygonPoints.Count; pathOuterPointIndex++)
            {
                var startPoint = new Vector3Class((float)pathPolygonPoints[pathOuterPointIndex].X, (float)pathPolygonPoints[pathOuterPointIndex].Y, 0) / 10;
                var endPoint = new Vector3Class((float)pathPolygonPoints[0].X, (float)pathPolygonPoints[0].Y, 0) / 10;

                if (pathOuterPointIndex < pathPolygonPoints.Count - 1)
                {
                    endPoint = new Vector3Class((float)pathPolygonPoints[pathOuterPointIndex + 1].X, (float)pathPolygonPoints[pathOuterPointIndex + 1].Y, 0) / 10;
                }

                var outerPathTriangle = new Triangle();
                outerPathTriangle.Vectors[0].Position = startPoint;
                outerPathTriangle.Vectors[1].Position = endPoint;
                outerPathTriangle.Vectors[2].Position = endPoint + new Vector3Class(0, 0, 5);

                if (flipTriangles) { outerPathTriangle.Flip(); }
                cube.Triangles[0].Add(outerPathTriangle);

                outerPathTriangle = new Triangle();
                outerPathTriangle.Vectors[0].Position = startPoint + new Vector3Class(0, 0, 5);
                outerPathTriangle.Vectors[1].Position = startPoint;
                outerPathTriangle.Vectors[2].Position = endPoint + new Vector3Class(0, 0, 5);

                if (flipTriangles) { outerPathTriangle.Flip(); }
                cube.Triangles[0].Add(outerPathTriangle);
            }

        }

        internal static Bitmap CreateMeasurementGrid(DAL.Hardware.AtumPrinter selectedPrinter)
        {
            var bitmap = new Bitmap(selectedPrinter.ProjectorResolutionX, selectedPrinter.ProjectorResolutionY);
            var rowIndex = 0;
            var columnIndex = 0;
            var rowHeight = 80;
            var columnWidth = 128;
            var maxRows = 15;
            var maxColumns = 15;
            var currentAlphabeticIndex = 1;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            var whiteBrush = new SolidBrush(Color.White);
            var blackBrush = new SolidBrush(Color.Black);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Black);
                for (var y = 0; y < maxRows; y++)
                {

                    columnIndex = 0;
                    for (var x = 0; x < maxColumns; x++)
                    {
                        //if (projectionGrid[rowIndex, columnIndex].Active)
                        //{
                        if (x % 2 == 0)
                        {
                            if (y % 2 == 0)
                            {
                                g.FillRectangle(whiteBrush, x * columnWidth, ((y) * (rowHeight)), columnWidth, rowHeight);

                                g.DrawString(currentAlphabeticIndex.ToString(), new Font(FontFamily.GenericSansSerif, 52, FontStyle.Bold), blackBrush, x * columnWidth + (columnWidth / 2), y * rowHeight + (rowHeight / 2) + 5, sf);
                                currentAlphabeticIndex++;
                            }
                        }

                        rowIndex++;
                    }
                    columnIndex++;
                }
            }

            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            return bitmap;

        }

    }
}
