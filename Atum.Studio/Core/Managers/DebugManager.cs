using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Managers
{
    internal class DebugManager
    {
        internal static void CreateDebugPath()
        {
            if (!Directory.Exists(DAL.ApplicationSettings.Settings.RoamingDebugPath)){
                Directory.CreateDirectory(DAL.ApplicationSettings.Settings.RoamingDebugPath);
            }
        }

        internal static void SaveStringToTextFile(StringBuilder stringBuilder, string fileName)
        {
            CreateDebugPath();

            var savePath = Path.Combine(DAL.ApplicationSettings.Settings.RoamingDebugPath, fileName);
            using (var fileWriter = new StreamWriter(savePath, false))
            {
                fileWriter.WriteLine(stringBuilder.ToString());
            }
        }

        internal static void SaveImage(Bitmap bitmap, string fileName)
        {
            CreateDebugPath();

            var savePath = Path.Combine(DAL.ApplicationSettings.Settings.RoamingDebugPath, fileName);
            bitmap.Save(savePath);
        }

    }
}
