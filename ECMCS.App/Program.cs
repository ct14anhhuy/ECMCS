using ECMCS.App.Extension;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ECMCS.App
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (PriorProcess() != null)
            {
                MessageBox.Show("Another instance of ECM app is already running.");
                return;
            }
            AppUpdate.CheckUpdate();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        private static Process PriorProcess()
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process proc in procs)
            {
                if (proc.Id != curr.Id && proc.MainModule.FileName == curr.MainModule.FileName)
                {
                    return proc;
                }
            }
            return null;
        }
    }
}