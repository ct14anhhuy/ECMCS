using ECMCS.App.Extension;
using ECMCS.Utilities.FileFolderExtensions;
using log4net;
using System;
using System.Diagnostics;
using System.Threading;
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
            log4net.Config.XmlConfigurator.Configure();
            ThreadContext.Properties["appName"] = "ECMCS.App";
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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

        #region Global errors

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogHelper.Error(e.Exception.Message);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            LogHelper.Error(ex.Message);
        }

        #endregion Global errors
    }
}