using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.PInvoke
{
    internal class NATIVE
    {
        internal const int GCL_STYLE = -26;
        internal const int CS_DROPSHADOW = 0x20000;

        [DllImport("User32.dll")]
        internal static extern bool MoveWindow(IntPtr h, int x, int y, int width, int height, bool redraw);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        internal static extern int GetClassLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetClassLong")]
        internal static extern int SetClassLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        internal static extern bool HideCaret(IntPtr hWnd);

    }
}
