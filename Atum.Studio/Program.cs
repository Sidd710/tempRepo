using System;
using System.Windows.Forms;
using OpenTK;
using System.Diagnostics;
using Atum.Studio.Core.Managers;
using System.Threading;
using System.Linq;
using Atum.Studio.Controls.NewGui.SplashControl;
using System.Collections.Generic;
using Atum.DAL.Managers;
using Atum.DAL.Licenses;

namespace Atum.Studio
{
    static class Program
    {
        public static SplashFrm splashForm = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                LoggingManager.Start();

                //DEBUG trial license
                //OnlineCatalogLicenses.CreateTrialLicense(@"D:\Coding\Workspaces\Atum\OperatorStation\Atum.Studio\bin\Debug\Settings\Licenses.dat");

                FontManager.LoadDefaultFonts();
                //ConnectivityManager.Start();
                MaterialManager.Start(true);
                UserProfileManager.Start();
                LicenseClientManager.Start();

                if (!UserProfileManager.UserProfile.Settings_Skip_Welcome_Screen_On_Next_Start || IsTrialLicenseExpired() || IsLicenseExpiring())
                {
                    Thread splashThread = new Thread(new ThreadStart(
                       delegate 
                       {
                               splashForm = new SplashFrm();
                           splashForm.ControlClosed += SplashForm_ControlClosed;
                           splashForm.RecentFilesOpenExistsProject += SplashForm_OpenExistsProject;
                           splashForm.RecentFilesOpenNewProject += SplashForm_OpenNewProject;
                           splashForm.RecentFilesSelectedRecentFileChanged += SplashForm_OpenExistsProject;
                           splashForm.UnLicensedProgramControlLicensedProgramControlOpenExistingProject += SplashForm_OpenExistsProject;
                           splashForm.UnLicensedProgramControlLicensedProgramControlOpenNewProject += SplashForm_OpenNewProject;
                           splashForm.SplashSkipWelcomClicked += SplashForm_OpenNewProject;

                               Application.Run(splashForm);
                           }
                       ));
                    
                    splashThread.SetApartmentState(ApartmentState.STA);
                    splashThread.Start();
                    splashThread.Join();


                }
                else
                {
                    var mainForm = InitializeMainForm();
                    Application.Run(mainForm);
                }
            }
            catch (Exception exc)
            {
                LicenseClientManager.Stop();
                MessageBox.Show(exc.StackTrace.Replace("Atum.Studio", "OperatorStation"));
                DAL.Managers.LoggingManager.WriteToLog("Fatal error happened", "Main", exc);
                Debug.WriteLine(exc.Message);
            }
        }

        private static bool IsLicenseExpiring()
        {
            if (LicenseClientManager.CurrentLicenses != null && LicenseClientManager.CurrentLicenses.Count > 0)
            {
                var license = LicenseClientManager.CurrentLicenses.First();
                if (license.LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.Trial || license.LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.StudioStandard)
                {
                    var daysLeft = (license.ExpirationDate - DateTime.Now).Days;
                    if (daysLeft < 60)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsLicenseExpired()
        {
            if (LicenseClientManager.CurrentLicenses != null && LicenseClientManager.CurrentLicenses.Count > 0)
            {
                var license = LicenseClientManager.CurrentLicenses.First();
                if (license.LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.Trial || license.LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.StudioStandard)
                {
                    var daysLeft = (license.ExpirationDate - DateTime.Now).Days;
                    if (daysLeft <= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsTrialLicenseExpired()
        {
            if (LicenseClientManager.CurrentLicenses != null && LicenseClientManager.CurrentLicenses.Count > 0)
            {
                var license = LicenseClientManager.CurrentLicenses.First();
                if (license.LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.Trial)
                {
                    var daysLeft = (license.ExpirationDate - DateTime.Now).Days;
                    if (daysLeft < 0)
                    {
                        return true ;
                    }
                }
            }

            return false;
        }

        private static void SplashForm_ControlClosed(object sender, EventArgs e)
        {
            splashForm.Close();

            if (IsTrialLicenseExpired() || IsLicenseExpired())
            {
                Environment.Exit(1);
            }
            else
            {
                Thread splashThread = new Thread(new ThreadStart(
                           delegate
                           {
                               var mainForm = InitializeMainForm();
                               Application.Run(mainForm);
                           }));
                splashThread.SetApartmentState(ApartmentState.STA);
                splashThread.Start();
            }
        }

        private static void SplashForm_OpenNewProject(object sender, EventArgs e)
        {
            splashForm.Close();

            Thread splashThread = new Thread(new ThreadStart(
           delegate
           {
               var mainForm = InitializeMainForm();
               Application.Run(mainForm);
           }));
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();
        }

        private static void SplashForm_OpenExistsProject(object sender, EventArgs e)
        {
            splashForm.Close();

            Thread splashThread = new Thread(new ThreadStart(
          delegate
          {
            
            var mainFormArguments = ((List<string>)sender).ToArray();
            var mainForm = InitializeMainForm(mainFormArguments);
            Application.Run(mainForm);
          }));

            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();
        }

        private static frmStudioMain InitializeMainForm()
        {
            var mainForm = new frmStudioMain();
            mainForm.FormClosed += MainForm_FormClosed;
            MainFormManager.Start(mainForm);
            MainFormManager.ProcesArguments(Environment.GetCommandLineArgs());

            var toolkitOptions = new ToolkitOptions();
            toolkitOptions.Backend = PlatformBackend.PreferNative;
            Toolkit.Init(toolkitOptions);

            return mainForm;
        }

        private static void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(1);
        }

        private static frmStudioMain InitializeMainForm(string[] fileNames = null)
        {
            var mainForm = new frmStudioMain();
            mainForm.FormClosed += MainForm_FormClosed;
            MainFormManager.Start(mainForm);
            if (fileNames != null && fileNames.Count() > 0)
            {
                MainFormManager.ProcesArguments(fileNames);
            }
            else
            {
                MainFormManager.ProcesArguments(Environment.GetCommandLineArgs());
            }

            var toolkitOptions = new ToolkitOptions();
            toolkitOptions.Backend = PlatformBackend.PreferNative;
            Toolkit.Init(toolkitOptions);

            return mainForm;
        }
    }


}


