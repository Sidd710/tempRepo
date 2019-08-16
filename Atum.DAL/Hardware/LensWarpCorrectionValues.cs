using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class LensWarpingCorrectionValues
    {
        public bool Enabled { get; set; }
        public List<List<float>> HorizontalValues { get; set; }
        public List<List<float>> VerticalValues { get; set; }

        [XmlIgnore]
        public Vector2[,] TransformationVectors { get; set; }

        public LensWarpingCorrectionValues()
        {
            this.Enabled = false;
        }

        public LensWarpingCorrectionValues(List<List<float>> totalRowValues, List<List<float>> totalColumnValues)
        {
            this.HorizontalValues = totalRowValues;
            this.VerticalValues = totalColumnValues;
        }

        public void CreateDefaultValues()
        {
            var defaultHorizontalValues = new List<float> { 12.8f, 12.8f, 12.8f, 12.8f, 12.8f, 12.8f, 12.8f, 12.8f };
            var defaultVerticalValues = new List<float> { 8f, 8f, 8f, 8f, 8f, 8f, 8f, 8f };
            this.HorizontalValues = new List<List<float>>();
            this.VerticalValues = new List<List<float>>();
            for (var valueIndex = 0; valueIndex < 8; valueIndex++)
            {
                this.HorizontalValues.Add(defaultHorizontalValues);
                this.VerticalValues.Add(defaultVerticalValues);
            }
        }

    }
}
