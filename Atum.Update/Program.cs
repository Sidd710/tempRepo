using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Atum.Studio.Update
{
    class Program
    {
        static void Main(string[] args)
        {
            var atumStudioClosed = false;
            var updaterPath = (new System.IO.FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName;
            var parentPath = (new System.IO.DirectoryInfo(updaterPath)).Parent.FullName;


            using (var logWriter = new System.IO.StreamWriter(System.IO.Path.Combine(updaterPath, "logging.txt"), true))
            {
                try
                {
                    while (!atumStudioClosed)
                    {
                        System.Threading.Thread.Sleep(1000);
                        var atumStudioActive = false;
                        foreach (var process in System.Diagnostics.Process.GetProcesses())
                        {
                            if (process.ProcessName.ToLower().StartsWith("atum.studio.exe") || process.ProcessName.ToLower().StartsWith("dds.studio.exe"))
                            {
                                process.Kill();
                                atumStudioActive = true;
                                break;
                            }
                        }

                        if (!atumStudioActive)
                        {
                            atumStudioClosed = true;
                        }

                    }

                    //do update
                    //get current exe path

                    foreach (var file in System.IO.Directory.GetFiles(updaterPath))
                    {
                        if (!file.ToLower().Contains("studio.update.exe") && !file.ToLower().Contains("logging.txt"))
                        {
                            var fileName = new System.IO.FileInfo(file);

                            try
                            {
                                System.IO.File.Copy(file, System.IO.Path.Combine(parentPath, fileName.Name), true);
                                logWriter.WriteLine(string.Format("{2}:{0} --> {1} OK", file, System.IO.Path.Combine(parentPath, fileName.Name), DateTime.Now));
                            }
                            catch (Exception exc)
                            {
                                logWriter.WriteLine(string.Format("{2}:{0} --> {1} Error ({2})", file, System.IO.Path.Combine(parentPath, fileName.Name), exc.Message, DateTime.Now));
                            }
                        }
                    }

                    foreach (var directory in System.IO.Directory.GetDirectories(updaterPath))
                    {
                        var directoryName = (new System.IO.DirectoryInfo(directory)).Name;
                        var localDirectoryPath = System.IO.Path.Combine(parentPath, directoryName);
                        if (!System.IO.Directory.Exists(localDirectoryPath)) {
                            System.IO.Directory.CreateDirectory(localDirectoryPath);
                            logWriter.WriteLine(string.Format("{1}:{0}", localDirectoryPath + "created", DateTime.Now));
                        }

                        foreach (var childDirectory in System.IO.Directory.GetDirectories(directory))
                        {
                            var childDirectoryName = (new System.IO.DirectoryInfo(childDirectory)).Name;
                            var localChildDirectoryPath = System.IO.Path.Combine(localDirectoryPath, childDirectoryName);

                            if (!System.IO.Directory.Exists(localChildDirectoryPath))
                            {
                                System.IO.Directory.CreateDirectory(localChildDirectoryPath);
                                logWriter.WriteLine(string.Format("{1}:{0}", localChildDirectoryPath + " created", DateTime.Now));
                            }

                            foreach (var childFile in System.IO.Directory.GetFiles(childDirectory))
                            {
                                    var childFileName = new System.IO.FileInfo(childFile);

                                    try
                                    {
                                        System.IO.File.Copy(childFile, System.IO.Path.Combine(localChildDirectoryPath, childFileName.Name), true);
                                        logWriter.WriteLine(string.Format("{2}:{0} --> {1} OK", childFile, System.IO.Path.Combine(localChildDirectoryPath, childFileName.Name), DateTime.Now));
                                    }
                                    catch (Exception exc)
                                    {
                                        logWriter.WriteLine(string.Format("{2}:{0} --> {1} Error ({2})", childFileName, System.IO.Path.Combine(localChildDirectoryPath, childFileName.Name), exc.Message, DateTime.Now));
                                    }
                                }
                            }
                        }
                        
                    
                }
                catch (Exception exc)
                {
                    logWriter.WriteLine(string.Format("Update exeption: " + exc.Message));
                }
            }

            if (System.IO.File.Exists(System.IO.Path.Combine(parentPath, "Atum.Studio.exe")))
            {
                System.Diagnostics.Process.Start(System.IO.Path.Combine(parentPath, "Atum.Studio.exe"));
            }
            else if (System.IO.File.Exists(System.IO.Path.Combine(parentPath, "DDS.Studio.exe")))
            {
                System.Diagnostics.Process.Start(System.IO.Path.Combine(parentPath, "DDS.Studio.exe"));
            }

                Environment.Exit(0);

        }
    }
}
