using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace ECMCS.App.Extension
{
    public static class AppUpdate
    {
        public static void CheckUpdate()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment app = ApplicationDeployment.CurrentDeployment;
                if (app.CheckForUpdate())
                {
                    DialogResult dr = MessageBox.Show("Current version is not the latest version, press Yes button to update", "Update Version", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        try
                        {
                            int exitCode = 0;
                            app.Update();
                            Application.Restart();
                            Environment.Exit(exitCode);
                        }
                        catch (DeploymentDownloadException ex)
                        {
                            MessageBox.Show($"Could not update to the latest version, check the connection and try again {Environment.NewLine}{ex.Message}", "Update Failed");
                            return;
                        }
                    }
                    else
                    {
                        Application.ExitThread();
                    }
                }
            }
        }
    }
}