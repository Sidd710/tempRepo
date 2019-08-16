﻿
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;

namespace Atum.Studio.Installer.Extension
{
    [RunInstaller(true)]
    public partial class InstallerExtension : System.Configuration.Install.Installer
    {
        public InstallerExtension()
        {
        }

        [STAThread]
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            string targetPath = Context.Parameters["targetdir"].Replace(@"\\", @"\");
            var applicationSettingsPath = Path.Combine(targetPath, "Settings");
            var licensePath = Path.Combine(applicationSettingsPath, "Licenses.dat");
            var licenseFound = false;
            if (!Directory.Exists(applicationSettingsPath)){
                Directory.CreateDirectory(applicationSettingsPath);

                //when file does not exists create trial license
                DAL.Licenses.OnlineCatalogLicenses.CreateTrialLicense(licensePath);
            }
            else
            {
                licenseFound = File.Exists(DAL.ApplicationSettings.Settings.LocalLicenseFilePath);
            }

            

            

            //change permissions on license.dat so every user can change it
            try
            {
                if (File.Exists(licensePath))
                {
                    SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                    FileSystemAccessRule writerule = new FileSystemAccessRule(sid, FileSystemRights.Modify, AccessControlType.Allow);

                    // Get your file's ACL
                    FileSecurity fileSecurity = File.GetAccessControl(licensePath);

                    // Add the new rule to the ACL
                    fileSecurity.AddAccessRule(writerule);

                    // Set the ACL back to the file
                    File.SetAccessControl(licensePath, fileSecurity);
                }
            }
            catch(Exception exc)
            {

            }

            base.OnBeforeInstall(savedState);
        }
        
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

    }
    public class WindowWrapper : IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
    
}
