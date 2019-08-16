using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UpdateVersionInfo
{
    public static class VersionUpdater
    {
        public static void UpdateVersionInfo(string executableWithVersionInfo, string targetFilename)
        {
            // Run a git checkout <targetfilename> to revert to known state
            //Process process = Process.Start("git", $"checkout {targetFilename}");
            //process.WaitForExit();
            using (var logger = new StreamWriter(@"c:\temp\logging.txt"))
            {
                string fileContent = File.ReadAllText(executableWithVersionInfo, Encoding.Unicode);

                // Get version from .NET exe
                Assembly versionAssembly = Assembly.LoadFile(executableWithVersionInfo);
                var fileVersionAttribute = versionAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
                string fileVersion = fileVersionAttribute.Version;

                logger.WriteLine(fileVersion);

                string[] versionParts = fileVersion.Split('.');
                string major = versionParts[0];
                string minor = versionParts[1];
                string patch = versionParts[2];
                string revision = versionParts[3];

                logger.WriteLine("updateContent");

                string updatedContent = UpdateVersionInfo(fileContent, major, minor, patch, revision, string.Empty);

                File.WriteAllText(targetFilename, updatedContent, Encoding.Unicode);
            
            }
            
        }

   

        public static string UpdateVersionInfo(string content, string major, string minor, string patch, string revision, string postfix)
        {
            string updatedContent = content;

            updatedContent = updatedContent.Replace("FILEVERSION 1,0,0,0", $"FILEVERSION {major},{minor},{patch},{revision}");
            updatedContent = updatedContent.Replace("PRODUCTVERSION 1,0,0,0", $"PRODUCTVERSION {major},{minor},{patch},{revision}");
            updatedContent = updatedContent.Replace("PRODUCTVERSION 1,0,0,0", $"PRODUCTVERSION {major},{minor},{patch},{revision}");
            updatedContent = updatedContent.Replace("\"FileVersion\", \"1.0.0.0\"", $"\"FileVersion\", \"{major}.{minor}.{patch}.{revision}\"");
            updatedContent = updatedContent.Replace("\"ProductVersion\", \"1.0.0-DEV\"", $"\"ProductVersion\", \"{major}.{minor}.{patch}{postfix}\"");

            return updatedContent;
        }
    }
}
