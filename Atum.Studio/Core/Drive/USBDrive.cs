using System;

namespace Atum.Studio.Core.Drive
{
    internal class USBDrive
    {
        internal string DriveLetter { get; set; }
        internal string Label { get; set; }
        internal string DriveFormat { get; set; }
        internal System.IO.DriveInfo DriveObject { get; set; }

        internal USBDrive(System.IO.DriveInfo drive)
        {
            this.DriveObject = drive;
            this.DriveLetter = drive.Name;
            this.Label = drive.VolumeLabel;
            this.DriveFormat = drive.DriveFormat;
        }

        public override string ToString()
        {
            return this.DriveLetter + (!String.IsNullOrEmpty(this.Label) ? " (" + this.Label + ")": string.Empty);
        }
    }
}
