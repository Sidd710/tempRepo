using Atum.DAL.ApplicationSettings;
using Atum.DAL.Managers;
using Atum.Studio.Core.Platform;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Atum.Studio.Core.Managers
{
    public class DisplayManager
    {
        public static GraphicsMode CurrentDisplayMode;

        public static void Start()
        {
            //remove obsolete file

            try
            {
                if (File.Exists(Settings.DisplayProfilesPath))
                {
                    File.Delete(Settings.DisplayProfilesPath);
                }
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            
            //get best graphicsmode
            List<GraphicsMode> modes = new List<GraphicsMode>();
            foreach (ColorFormat color in new ColorFormat[] { 32, 24, 16, 8 })
                foreach (int depth in new int[] { 24, 16 })
                    foreach (int stencil in new int[] { 8, 0 })
                        foreach (int samples in new int[] { 0, 2, 4, 6, 8, 16 })
                            foreach (bool stereo in new bool[] { false, true })
                            {
                                try
                                {
                                    GraphicsMode mode = new GraphicsMode(color, depth, stencil, samples, 0, 2, stereo);
                                    if (!modes.Contains(mode))
                                        modes.Add(mode);
                                }
                                catch
                                { }
                            }


            foreach (ColorFormat color in new ColorFormat[] { 32, 24, 16, 8 })
                foreach (int depth in new int[] { 24, 16 })
                    foreach (int stencil in new int[] { 8, 0 })
                        foreach (int samples in new int[] { 8, 6, 4, 2, 0 })
                        {
                            foreach (var mode in modes)
                                if (mode.ColorFormat == color && mode.Depth == depth && mode.Stencil == stencil && mode.Samples == samples)
                                {
                                    CurrentDisplayMode = mode;

                                    return ;

                                }
                        }
        }
        
        public static float GetResolutionScaleFactor()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                switch (graphics.DpiX){
                    case 96:
                        return 1;
                    case 120:
                        return 1.25f;
                    case 144:
                        return 1.5f;
                    case 168:
                        return 1.75f;
                    case 192:
                        return 2f;
                    default:
                        return 1f;
                }
            }
        }
    }
}
