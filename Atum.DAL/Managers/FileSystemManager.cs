using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

namespace Atum.DAL.Managers
{
    public class FileSystemManager
    {

        public static bool HasWriteAccess(string directoryPath)
        {
            try
            {
                var tempDirName = new FileInfo(Path.GetTempFileName()).Name;
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
             //   DirectorySecurity dirAC = dirInfo.GetAccessControl(AccessControlSections.All);

                var temoDir = dirInfo.CreateSubdirectory(tempDirName);
                Directory.Delete(temoDir.FullName,true);
                return true;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return false;
            }
        }
    }
}
