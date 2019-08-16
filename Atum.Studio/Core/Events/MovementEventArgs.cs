using System;

namespace Atum.Studio.Core.Events
{
    internal class MovementEventArgs : EventArgs
    {
        internal Enums.SceneViewSelectedMoveTranslationAxisType MoveAxisType { get; set; }
        internal float MoveTranslationX { get; set; }
        internal float MoveTranslationY { get; set; }
        internal float MoveTranslationZ { get; set; }
    }
}
