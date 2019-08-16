using OpenTK;
using System;
using System.Collections.Generic;

namespace Atum.Studio.Core.ModelCorrections.LensWarpCorrection
{
    public class LensCorrectionTransformationMatrix : IDisposable
    {
        public Vector2 TransformedUpperLeft { get; set; }
        public Vector2 TransformedLowerLeft { get; set; }
        public Vector2 TransformedUpperRight { get; set; }
        public Vector2 TransformedLowerRight { get; set; }

        public Vector2 OriginUpperLeft { get; set; }
        public Vector2 OriginLowerLeft { get; set; }
        public Vector2 OriginUpperRight { get; set; }
        public Vector2 OriginLowerRight { get; set; }

        private List<Vector2> _transformedUpperLinePoints;
        private List<Vector2> _transformedLowerLinePoints;
        private List<Vector2> _transformedLeftLinePoints;
        private List<Vector2> _tranformedRightLinePoints;

        private List<Vector2> _originUpperLinePoints;
        private List<Vector2> _originLowerLinePoints;
        private List<Vector2> _originLeftLinePoints;
        private List<Vector2> _originRightLinePoints;

        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        public Vector2[,] TransformationVectors = new Vector2[128, 80];
        public List<List<TransformationMatrixIntersectionData>> OriginIntersections;
        public List<List<TransformationMatrixIntersectionData>> TransformedIntersections;

        //public int SegmentsCount { get; set; }

        public int SegmentsVertical = 80;
        public int SegmentsHorizontal = 128;
        
        public LensCorrectionTransformationMatrix()
        {

        }

        public LensCorrectionTransformationMatrix(Vector2 transformedUpperLeft, Vector2 transformedUpperRight, Vector2 transformedLowerLeft, Vector2 transformedLowerRight, Vector2 originUpperLeft, Vector2 originUpperRight, Vector2 originLowerLeft, Vector2 originLowerRight, int segmentsCount, int matrixColumnIndex, int matrixRowIndex)
        {

            this.TransformedUpperLeft = transformedUpperLeft;
            this.TransformedLowerLeft = transformedLowerLeft;
            this.TransformedUpperRight = transformedUpperRight;
            this.TransformedLowerRight = transformedLowerRight;

            this.OriginUpperLeft = originUpperLeft;
            this.OriginLowerLeft = originLowerLeft;
            this.OriginUpperRight = originUpperRight;
            this.OriginLowerRight = originLowerRight;

            this.RowIndex = matrixRowIndex;
            this.ColumnIndex = matrixColumnIndex;

            this.OriginIntersections = new List<List<TransformationMatrixIntersectionData>>();
            this.TransformedIntersections = new List<List<TransformationMatrixIntersectionData>>();

        }
        
        internal void CalcTransformations()
        {

            this._transformedUpperLinePoints = GetLinePoints(this.TransformedUpperLeft, this.TransformedUpperRight, this.SegmentsHorizontal);
            this._transformedLowerLinePoints = GetLinePoints(this.TransformedLowerLeft, this.TransformedLowerRight, this.SegmentsHorizontal);
            this._transformedLeftLinePoints = GetLinePoints(this.TransformedUpperLeft, this.TransformedLowerLeft, this.SegmentsVertical);
            this._tranformedRightLinePoints = GetLinePoints(this.TransformedUpperRight, this.TransformedLowerRight, this.SegmentsVertical);

            this._originUpperLinePoints = GetLinePoints(this.OriginUpperLeft, this.OriginUpperRight, this.SegmentsHorizontal);
            this._originLowerLinePoints = GetLinePoints(this.OriginLowerLeft, this.OriginLowerRight, this.SegmentsHorizontal);
            this._originLeftLinePoints = GetLinePoints(this.OriginUpperLeft, this.OriginLowerLeft, this.SegmentsVertical);
            this._originRightLinePoints = GetLinePoints(this.OriginUpperRight, this.OriginLowerRight, this.SegmentsVertical);


            CalcOriginIntersections();
            CalcTransformedIntersections();
        }

        internal float[] GetXYPercentageByPoint(Vector2 point)
        {
            var result = new float[2];

            var verticalIntersectionLines = new Dictionary<Vector2, Vector2>();
            for (var lineIndex = 0; lineIndex < this._originUpperLinePoints.Count; lineIndex++)
            {
                verticalIntersectionLines.Add(this._originUpperLinePoints[lineIndex], this._originLowerLinePoints[lineIndex]);
            }

            var horizontalIntersectionLines = new Dictionary<Vector2, Vector2>();
            for (var lineIndex = 0; lineIndex < this._originUpperLinePoints.Count; lineIndex++)
            {
                horizontalIntersectionLines.Add(this._originLeftLinePoints[lineIndex], this._originRightLinePoints[lineIndex]);
            }

            var verticalIntersectionLineIndex = 0;
            foreach (var verticalIntersectionLine in verticalIntersectionLines)
            {
                var horizontalIntersectionLineIndex = 0;
                foreach (var horizontalIntersectionLine in horizontalIntersectionLines)
                {
                    var intersectionPoint = Helpers.MathHelper.GetIntersectionPoint(verticalIntersectionLine.Key, verticalIntersectionLine.Value, horizontalIntersectionLine.Key, horizontalIntersectionLine.Value);
                    var intersectionPointAsInt = new Vector2((int)intersectionPoint.X, (int)intersectionPoint.Y);

                    if ((int)point.X == intersectionPointAsInt.X && (int)point.Y == intersectionPointAsInt.Y)
                    {
                        return new float[2] { verticalIntersectionLineIndex, 100f - horizontalIntersectionLineIndex };
                    }

                    horizontalIntersectionLineIndex++;
                }

                verticalIntersectionLineIndex++;
            }

            return result;
        }

        internal Vector2 CalcDestinationPointByPercentage(Vector2 percentage)
        {
            //only add lines by percentage index
            var verticalIntersectionLines = new Dictionary<Vector2, Vector2>();
            var intersectionPoint = Helpers.MathHelper.GetIntersectionPoint(this._originUpperLinePoints[(int)percentage.X], this._originLowerLinePoints[(int)percentage.X], this._originLeftLinePoints[(int)percentage.Y], this._originRightLinePoints[(int)percentage.Y]);
            return intersectionPoint;
        }

        private void CalcOriginIntersections(bool destinationMatrix = false)
        {
            var verticalIntersectionLines = new Dictionary<Vector2, Vector2>();
            for (var lineIndex = 0; lineIndex < this._originUpperLinePoints.Count; lineIndex++)
            {
                verticalIntersectionLines.Add(this._originUpperLinePoints[lineIndex], this._originLowerLinePoints[lineIndex]);
            }

            var horizontalIntersectionLines = new Dictionary<Vector2, Vector2>();
            for (var lineIndex = 0; lineIndex < this._originRightLinePoints.Count; lineIndex++)
            {
                horizontalIntersectionLines.Add(this._originLeftLinePoints[lineIndex], this._originRightLinePoints[lineIndex]);
            }

            var verticalIntersectionLineIndex = 0;
            //  var deDupIntersectionPoints = new SortedList<double, SortedList<double, int>>();
            foreach (var verticalIntersectionLine in verticalIntersectionLines)
            {
                var horizontalIntersectionLineIndex = 0;
                var horizontalIntersectionPoints = new List<TransformationMatrixIntersectionData>();
                foreach (var horizontalIntersectionLine in horizontalIntersectionLines)
                {
                    var intersectionPoint = Helpers.MathHelper.GetIntersectionPoint(verticalIntersectionLine.Key, verticalIntersectionLine.Value, horizontalIntersectionLine.Key, horizontalIntersectionLine.Value);
                    var intersectionData = new TransformationMatrixIntersectionData(intersectionPoint);

                    //remove duplicate intersectionpoints AS INT
                    var intersectionX = Math.Round(intersectionData.IntersectionPoint.X, 1);
                    var intersectionY = Math.Round(intersectionData.IntersectionPoint.Y, 1);
                    horizontalIntersectionPoints.Add(intersectionData);
                    horizontalIntersectionLineIndex++;
                }

                this.OriginIntersections.Add(horizontalIntersectionPoints);

                verticalIntersectionLineIndex++;
            }
        }

        private void CalcTransformedIntersections()
        {
            var verticalIntersectionLines = new Dictionary<Vector2, Vector2>();
            for (var lineIndex = 0; lineIndex < this._transformedUpperLinePoints.Count; lineIndex++)
            {
                verticalIntersectionLines.Add(this._transformedUpperLinePoints[lineIndex], this._transformedLowerLinePoints[lineIndex]);
            }

            var horizontalIntersectionLines = new Dictionary<Vector2, Vector2>();
            for (var lineIndex = 0; lineIndex < this._tranformedRightLinePoints.Count; lineIndex++)
            {
                horizontalIntersectionLines.Add(this._transformedLeftLinePoints[lineIndex], this._tranformedRightLinePoints[lineIndex]);
            }

            var verticalIntersectionLineIndex = 0;
            //  var deDupIntersectionPoints = new SortedList<double, SortedList<double, int>>();
            foreach (var verticalIntersectionLine in verticalIntersectionLines)
            {
                var horizontalIntersectionLineIndex = 0;
                var horizontalIntersectionPoints = new List<TransformationMatrixIntersectionData>();
                foreach (var horizontalIntersectionLine in horizontalIntersectionLines)
                {
                    var intersectionPoint = Helpers.MathHelper.GetIntersectionPoint(verticalIntersectionLine.Key, verticalIntersectionLine.Value, horizontalIntersectionLine.Key, horizontalIntersectionLine.Value);
                    var intersectionData = new TransformationMatrixIntersectionData(intersectionPoint);

                    //remove duplicate intersectionpoints AS INT
                    var intersectionX = Math.Round(intersectionData.IntersectionPoint.X, 1);
                    var intersectionY = Math.Round(intersectionData.IntersectionPoint.Y, 1);
                    horizontalIntersectionPoints.Add(intersectionData);
                    horizontalIntersectionLineIndex++;
                }

                this.TransformedIntersections.Add(horizontalIntersectionPoints);

                verticalIntersectionLineIndex++;
            }
        }

        internal void Clear()
        {
            this._originLeftLinePoints.Clear();
            this._originLowerLinePoints.Clear();
            this._originRightLinePoints.Clear();
            this._originUpperLinePoints.Clear();
            this.OriginIntersections.Clear();
        }

        private List<Vector2> GetLinePoints(Vector2 sourcePoint, Vector2 destinationPoint, int segments)
        {
            var lineDistanceX = destinationPoint.X - sourcePoint.X;
            var lineDistanceY = destinationPoint.Y - sourcePoint.Y;

            if (lineDistanceX > 0) lineDistanceX++;
            if (lineDistanceY > 0) lineDistanceY++;

            var lineDistancePartX = lineDistanceX / (segments);
            var lineDistancePartY = lineDistanceY / (segments);

            var linePoints = new List<Vector2>();
            for (var i = 0; i < segments; i++)
            {
                var linePoint = new Vector2(sourcePoint.X + (lineDistancePartX * i), sourcePoint.Y + (lineDistancePartY * i));

                linePoints.Add(linePoint);
            }

            return linePoints;
        }

        public void Dispose()
        {

        }
    }

    public class TransformationMatrixDataItem
    {
        public TransformationMatrixIntersectionData SourceData { get; set; }
        public TransformationMatrixIntersectionData DestinationData { get; set; }

        public TransformationMatrixDataItem()
        {

        }

        public Vector2 CalcTranslationVector()
        {
            var translationVector = this.DestinationData.IntersectionPoint - this.SourceData.IntersectionPoint;
            return new Vector2(translationVector.X, translationVector.Y);
        }

    }

    public class TransformationMatrixIntersectionData
    {

        public Vector2 IntersectionPoint { get; set; }

        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        public TransformationMatrixIntersectionData()
        {

        }

        public TransformationMatrixIntersectionData(Vector2 intersectionPoint)
        {
            this.IntersectionPoint = intersectionPoint;
        }
    }
}
