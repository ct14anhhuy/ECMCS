using Microsoft.Win32;
using System;

namespace ECMCS.Utilities
{
    public static class ProtocolHelper
    {
        public static void Create(string protocolName, string appPath)
        {
            try
            {
                var regKey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Classes", true);
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(protocolName);
                if (key == null)
                {
                    key = regKey.CreateSubKey(protocolName);
                    key.SetValue("URL Protocol", protocolName);
                    key.CreateSubKey(@"shell\open\command").SetValue("", "\"" + appPath + "\" \"%1\"");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}