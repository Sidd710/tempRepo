namespace Atum.Studio.Core
{
    public class Enums
    {
        public enum SoftwareLevelType
        {
            Bronze = 0,
            Silver = 1,
            Gold = 2,
            Platinum = 10,
            Debug = 100,
        }

        #region Sceneview

        public enum SceneViewSelectedMoveTranslationAxisType
        {
            NoAxisSelected = 0,
            Hidden = 100,
            X = 240,
            Y = 241,
            Z = 242,
        }

        public enum SceneViewSelectedRotationAxisType
        {
            None = 0,
            Hidden = 100,
            X = 240,
            Y = 241,
            Z = 242
        }

        public enum SceneViewSelectedScaleAxisType
        {
            NoAxisSelected = 0,
            Hidden = 100,
            X = 240,
            Y = 241,
            Z = 242,
            All = 243
        }

        #endregion

        #region ToolStripActions
        public enum MainFormToolStripActionType
        {
            Unknown = 0,
            btnPanPressed = 1,
            btnZoomPressed = 2,
            btnOrbitPressed = 3,
            btnSelectPressed = 10,
            btnMovePressed = 11,
            btnRotatePressed = 12,
            btnScalePressed = 13,
            btnSimulationPressed = 14,
            btnLayFlatPressed = 16,

            btnManualSingleSupportCone = 25,
            btnManualGridSupportCone = 26,

            btnModelActionDuplicate = 40,
            btnModelActionMagsAI = 41,
            btnModelActionMagsAIManualSupport = 42,
            btnModelActionMagsAIGridSupport = 43,

        }

        #endregion
    }
}
