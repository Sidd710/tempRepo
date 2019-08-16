using System;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using Atum.Studio.Core.SoftwareUpdater;
using System.Diagnostics;
using Atum.DAL.Managers;
using System.IO;
using Atum.DAL.ApplicationSettings;

namespace Atum.Studio.Core.Managers
{
    public class SoftwareUpdateManager
    {
        public static event EventHandler NewUpdateAvailable;
        private static AutoUpdater _autoUpdater;

        internal static void Start()
        {
            var bHasError = false;
            _autoUpdater = new AutoUpdater();

      try
      {
          _autoUpdater.NewUpdateAvailable += autoUpdater_NewUpdateAvailable;
          _autoUpdater.Update();
      }
      catch (WebException exp)
      {
                Debug.WriteLine(exp.Message);
                MessageBox.Show("Can not find the specified resource");
          bHasError = true;
      }
      catch (XmlException exp)
      {
                Debug.WriteLine(exp.Message);
                bHasError = true;
          MessageBox.Show("Download the upgrade file error");
      }
      catch (NotSupportedException exp)
      {
                Debug.WriteLine(exp.Message);
                bHasError = true;
          MessageBox.Show("Upgrade address configuration error");
      }
      catch (ArgumentException exp)
      {
                Debug.WriteLine(exp.Message);
                bHasError = true;
          MessageBox.Show("Download the upgrade file error");
      }
      catch (Exception exp)
      {
                Debug.WriteLine(exp.Message);
                bHasError = true;
          MessageBox.Show("An error occurred during the upgrade process");
      }
      finally
      {
          if (bHasError == true)
          {
              try
              {
                  //autoUpdater.RollBack();
              }
              catch (Exception)
              {
                 //Log the message to your file or database
              }
          }
      }
        }

        static void autoUpdater_NewUpdateAvailable(object sender, EventArgs e)
        {
            NewUpdateAvailable?.Invoke(null, null);
        }

        internal static void ShowUpdateDialog()
        {
            _autoUpdater.ShowUpdateDialog();
        }

        internal static void UpdateApplicationWhenAvailable()
        {

            var updatePath = Settings.UpdatePath;
            if (!FileSystemManager.HasWriteAccess(updatePath))
            {
                updatePath = Settings.RoamingUpdatePath;
            }

            if (Directory.Exists(updatePath))
            {
                foreach (var file in Directory.GetFiles(updatePath))
                {
                    if (file.ToLower().Contains(".msi"))
                    {
                        var updateProcess = new Process();
                        updateProcess.StartInfo.FileName = file;
                        updateProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                        updateProcess.Start();
                        break;
                    }
                }
            }

            Environment.Exit(0);
        }



    }
}
