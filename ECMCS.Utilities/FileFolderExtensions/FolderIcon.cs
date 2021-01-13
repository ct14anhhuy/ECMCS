using System.IO;

namespace ECMCS.Utilities.FileFolderExtensions
{
    public class FolderIcon
    {
        private string _folderPath = "";
        private string _iniPath = "";

        public FolderIcon(string folderPath)
        {
            this.FolderPath = folderPath;
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
            this.FolderPath = targetFolderPath;
            this.CreateFolderIcon(iconFilePath, infoTip);
        }

        public string FolderPath
        {
            get { return this._folderPath; }
            set
            {
                _folderPath = value;
                if (!this._folderPath.EndsWith("\\"))
                {
                    this._folderPath += "\\";
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
            if (this.FolderPath.Length == 0)
            {
                return false;
            }
            if (Directory.Exists(this.FolderPath))
            {
                return true;
            }
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(this.FolderPath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool CreateDesktopIniFile(string iconFilePath, bool getIconFromDLL, int iconIndex, string infoTip)
        {
            if (!Directory.Exists(this.FolderPath))
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
            this.IniPath = this.FolderPath + "desktop.ini";
            IniWriter.WriteValue(".ShellClassInfo", "IconFile", iconFilePath, this.IniPath);
            IniWriter.WriteValue(".ShellClassInfo", "IconIndex", iconIndex.ToString(), this.IniPath);
            IniWriter.WriteValue(".ShellClassInfo", "InfoTip", infoTip, this.IniPath);

            return true;
        }

        private void CreateDesktopIniFile(string iconFilePath, string infoTip)
        {
            this.CreateDesktopIniFile(iconFilePath, false, 0, infoTip);
        }

        private bool SetIniFileAttributes()
        {
            if (!File.Exists(this.IniPath))
            {
                return false;
            }
            if ((File.GetAttributes(this.IniPath) & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                File.SetAttributes(this.IniPath, File.GetAttributes(this.IniPath) | FileAttributes.Hidden);
            }
            if ((File.GetAttributes(this.IniPath) & FileAttributes.System) != FileAttributes.System)
            {
                File.SetAttributes(this.IniPath, File.GetAttributes(this.IniPath) | FileAttributes.System);
            }
            return true;
        }

        private bool SetFolderAttributes()
        {
            if (!Directory.Exists(this.FolderPath))
            {
                return false;
            }
            if ((File.GetAttributes(this.FolderPath) & FileAttributes.System) != FileAttributes.System)
            {
                File.SetAttributes(this.FolderPath, File.GetAttributes(this.FolderPath) | FileAttributes.System);
            }
            return true;
        }
    }
}