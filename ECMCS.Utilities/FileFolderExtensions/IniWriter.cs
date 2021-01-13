using System.Runtime.InteropServices;

namespace ECMCS.Utilities.FileFolderExtensions
{
    public class IniWriter
    {
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string iniSection, string iniKey, string iniValue, string iniFilePath);

        public static void WriteValue(string iniSection, string iniKey, string iniValue, string iniFilePath)
        {
            WritePrivateProfileString(iniSection, iniKey, iniValue, iniFilePath);
        }
    }
}