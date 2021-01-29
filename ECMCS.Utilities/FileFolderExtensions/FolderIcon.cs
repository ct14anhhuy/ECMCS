using System.IO;

namespace ECMCS.Utilities.FileFolderExtensions
{
    public class FolderIcon
    {
        private string _folderPath = "";
        private string _iniPath = "";

        public FolderIcon(string folderPath)
        {
            FolderPath = folderPath;
        }

        public void CreateFolderIcon(string iconFilePath, string infoTip)
        {
            if (CreateFolder())
            {
                CreateDesktopIniFile(iconFilePath, infoTip);
                SetIniFileAttributes();
                SetFolderAttributes();
            }
        }

        public void CreateFolderIcon(string targetFolderPath, string iconFilePath, string infoTip)
        {
            FolderPath = targetFolderPath;
            CreateFolderIcon(iconFilePath, infoTip);
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
                if (!_folderPath.EndsWith("\\"))
                {
                    _folderPath += "\\";
                }
            }
        }

        public string IniPath
        {
            get { return _iniPath; }
            set { _iniPath = value; }
        }

        private bool CreateFolder()
        {
            if (FolderPath.Length == 0)
            {
                return false;
            }
            if (Directory.Exists(FolderPath))
            {
                return true;
            }
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(FolderPath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool CreateDesktopIniFile(string iconFilePath, bool getIconFromDLL, int iconIndex, string infoTip)
        {
            if (!Directory.Exists(FolderPath))
            {
                return false;
            }
            if (!File.Exists(iconFilePath))
            {
                return false;
            }
            if (!getIconFromDLL)
            {
                iconIndex = 0;
            }
            IniPath = FolderPath + "desktop.ini";
            IniWriter.WriteValue(".ShellClassInfo", "IconFile", iconFilePath, IniPath);
            IniWriter.WriteValue(".ShellClassInfo", "IconIndex", iconIndex.ToString(), IniPath);
            IniWriter.WriteValue(".ShellClassInfo", "InfoTip", infoTip, IniPath);

            return true;
        }

        private void CreateDesktopIniFile(string iconFilePath, string infoTip)
        {
            CreateDesktopIniFile(iconFilePath, false, 0, infoTip);
        }

        private bool SetIniFileAttributes()
        {
            if (!File.Exists(IniPath))
            {
                return false;
            }
            if ((File.GetAttributes(IniPath) & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                File.SetAttributes(IniPath, File.GetAttributes(IniPath) | FileAttributes.Hidden);
            }
            if ((File.GetAttributes(IniPath) & FileAttributes.System) != FileAttributes.System)
            {
                File.SetAttributes(IniPath, File.GetAttributes(IniPath) | FileAttributes.System);
            }
            return true;
        }

        private bool SetFolderAttributes()
        {
            if (!Directory.Exists(FolderPath))
            {
                return false;
            }
            if ((File.GetAttributes(FolderPath) & FileAttributes.System) != FileAttributes.System)
            {
                File.SetAttributes(FolderPath, File.GetAttributes(FolderPath) | FileAttributes.System);
            }
            return true;
        }
    }
}