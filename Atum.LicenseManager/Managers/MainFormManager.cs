using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Atum.LicenseManager.Managers
{
    public class MainFormManager
    {
        public static void Start()
        {
            
        }

        public static void ProcesArguments(string[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                foreach (var arg in arguments)
                {
                    if (!string.IsNullOrEmpty(arg))
                    {
                        if (arg.ToLower() == "verbose=true")
                        {
                            DAL.ApplicationSettings.Settings.VerboseMode = true;
                        }
                    }
                }
            }
        }
    }
}
