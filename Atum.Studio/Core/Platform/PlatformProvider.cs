using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;

namespace Atum.Studio.Core.Platform
{
    internal class PlatformProvider
    {

        internal enum PresentationModeType
        {
            off = 0,
            on = 1
        }

        internal static void ChangePresentationMode(PresentationModeType presentationMode)
        {
            if (IsNotebook())
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = @"PresentationSettings.exe";
                process.StartInfo.Arguments = presentationMode == PresentationModeType.on ? "/start" : "/stop";
                process.StartInfo.UseShellExecute = true;
                if (System.IO.File.Exists(Environment.SystemDirectory + "\\PresentationSettings.exe"))
                {
                    process.StartInfo.WorkingDirectory = Environment.SystemDirectory + "\\PresentationSettings.exe";
                    process.Start();
                }
                else if (System.IO.File.Exists(@"c:\windows\sysnative\presentationsettings.exe"))
                {
                    process.StartInfo.WorkingDirectory = @"c:\windows\sysnative";
                    process.Start();
                }
            }
        }

        private static bool IsNotebook()
        {
            var isNotebook = false;

            //check wmi battery information. If present then notebook
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"\\.\root\cimv2"), new SelectQuery("select * from win32_battery")))
                {
                    ManagementObjectCollection batteryCollection = searcher.Get();
                    if (batteryCollection.Count > 0)
                    {
                        isNotebook = true;
                    }
                }
            }
            catch
            {
            }

            return isNotebook;
        }


        internal static void HideTaskbar(bool hideTaskbar)
        {
            if (hideTaskbar)
            {
                TurnOffTaskBar();
            }
        }

        private static void TurnOffTaskBar()
        {
            var taskBarName = "Shell_SecondaryTrayWnd";

            var hwnd = FindWindow(taskBarName, "");
            if (hwnd == null)
            {
                return;
            }
            ShowWindow(hwnd, SW_HIDE);
            ShowWindow(FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, null), SW_HIDE);
        }

        private static void TurnOnTaskBar()
        {

            //var taskBarName = "Shell_TrayWnd";
            var taskBarName = "Shell_SecondaryTrayWnd";
            var hwnd = FindWindow(taskBarName, "");
            if (hwnd == null)
            {
                return;
            }
            ShowWindow(hwnd, SW_SHOW);
            ShowWindow(FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, null), SW_SHOW);


        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int command);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;
    }
}

