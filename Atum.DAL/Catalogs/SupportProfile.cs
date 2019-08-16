using Atum.DAL.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Materials
{
    [Serializable]
    public class SupportProfile
    {
        public bool Selected { get; set; }
        public bool Default { get; set; }

        public string DisplayName { get; set; }

        public float SupportTopRadius { get; set; }
        public float SupportTopHeight { get; set; }
        public float SupportMiddleRadius { get; set; }
        public float SupportBottomHeight { get; set; }
        public float SupportBottomRadius { get; set; }
        public float SupportBottomWidthCorrection { get; set; }

        public float ASegmentHeight { get; set; }
        public float ASegmentRadius { get; set; }
        public float BSegmentHeight { get; set; }
        public float BSegmentRadius { get; set; }
        public float CSegmentRadius { get; set; }
        public float DSegmentHeight { get; set; }
        public float DSegmentRadius { get; set; }

        public int SupportAmountOfSibblingLayers { get; set; }

        public List<float> SupportLowestPointsOffset { get; set; }
        public List<float> SupportLowestPointsDistanceOffset { get; set; }

        public List<float> SurfaceAngles { get; set; }
        public List<float> SurfaceAngleDistanceFactors { get; set; }

        public float SupportLowestPointsDistance { get; set; }

        public float SupportInfillDistance { get; set; }
        public float SupportOverhangDistance { get; set; }

        public float SupportIntermittedConnectionHeight { get; set; }
       

        public SupportProfile()
        {
        }

        public static SupportProfile CreateDefault()
        {

            var defaultSupportProfile = new SupportProfile();
            defaultSupportProfile.DisplayName = "Default";
            defaultSupportProfile.SupportAmountOfSibblingLayers = 40;
            defaultSupportProfile.SupportInfillDistance = 3;
            defaultSupportProfile.SupportOverhangDistance = 3;
            defaultSupportProfile.SupportLowestPointsDistance = 2.1f;
            defaultSupportProfile.ASegmentHeight = 0.2f;
            defaultSupportProfile.ASegmentRadius = 0.35f;
            defaultSupportProfile.BSegmentHeight = 1.0f;
            defaultSupportProfile.BSegmentRadius = 0.5f;
            defaultSupportProfile.CSegmentRadius = 0.5f;
            defaultSupportProfile.DSegmentHeight = 1.4f;
            defaultSupportProfile.DSegmentRadius = 0.5f;

            defaultSupportProfile.SupportTopRadius = 0.2f;
            defaultSupportProfile.SupportTopHeight = 4f;
            defaultSupportProfile.SupportMiddleRadius = 0.4f;
            defaultSupportProfile.SupportBottomHeight = 1f;
            defaultSupportProfile.SupportBottomRadius = 2f;

            defaultSupportProfile.SupportLowestPointsOffset = new List<float>() { 0.3f, 0.5f, 0.7f, 1f };
            defaultSupportProfile.SupportLowestPointsDistanceOffset = new List<float>() { 1f, 2f, 3f, 4f };

            defaultSupportProfile.SurfaceAngles = new List<float>();
            defaultSupportProfile.SurfaceAngles.Add(30);
            defaultSupportProfile.SurfaceAngles.Add(45);

            defaultSupportProfile.SurfaceAngleDistanceFactors = new List<float>();
            defaultSupportProfile.SurfaceAngleDistanceFactors.Add(1.1f);
            defaultSupportProfile.SurfaceAngleDistanceFactors.Add(1.4f);
            defaultSupportProfile.Selected = true;
            defaultSupportProfile.Default = true;

            defaultSupportProfile.SupportIntermittedConnectionHeight = 1f;

            return defaultSupportProfile;
        }
    }

    [Serializable]
    public class SupportProfileLayerBasedOverhang
    {
        public float OverhangInMM { get; set; }
        public int AmountOfLayers { get; set; }

        public SupportProfileLayerBasedOverhang()
        {
            this.OverhangInMM = 4f;
            this.AmountOfLayers = 4;
        }
    }

}
