using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Atum.DAL.OS
{
    public class OSProvider
    {
        public static bool IsWindows { get { return Environment.OSVersion.Platform != PlatformID.Unix; } }

        public static bool IsOSX
        {
            get
            {
                bool result = false;
                if (!IsWindows)
                {
                    var processStartInfo = new ProcessStartInfo("uname", "-s");
                        processStartInfo.RedirectStandardOutput = true;
                        processStartInfo.UseShellExecute = false;
                        string outputstring = string.Empty;

                        using (var process = Process.Start(processStartInfo))
                        {
                            process.Start();
                            outputstring = process.StandardOutput.ReadLine();
                            process.WaitForExit();
                        }

                        result = string.Compare(outputstring, "darwin", true) == 0;
                }

                return result;

            }
        }

        public static bool IsLinux
        {
            get
            {
                bool result = false;
                if (!IsWindows)
                {
                    var processStartInfo = new ProcessStartInfo("uname", "-s");
                    processStartInfo.RedirectStandardOutput = true;
                    processStartInfo.UseShellExecute = false;
                    string outputstring = string.Empty;

                    using (var process = Process.Start(processStartInfo))
                    {
                        process.Start();
                        outputstring = process.StandardOutput.ReadLine();
                        process.WaitForExit();
                    }

                    result = string.Compare(outputstring, "linux", true) == 0;
                }

                return result;

            }
        }
    }
}
