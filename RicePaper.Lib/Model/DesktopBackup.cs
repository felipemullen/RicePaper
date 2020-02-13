using System;
using System.Collections.Generic;
using System.IO;
using AppKit;
using Foundation;

namespace RicePaper.Lib.Model
{
    public class DesktopBackupFile
    {
        #region Properties
        public string CacheLocation { get; private set; }
        public string ScreenId { get; private set; }
        public string OriginalLocation { get; private set; }
        public string FileName => $"{ScreenId}.screendata";
        public string BackupPath => Path.Combine(Util.DesktopBackupDirectory, FileName);

        public string[] FileContents
        {
            get
            {
                return new string[] {
                    OriginalLocation,
                    CacheLocation,
                    ScreenId
                };
            }
        }
        #endregion

        #region Initialization
        public DesktopBackupFile(string original, string cache, string id)
        {
            this.OriginalLocation = original;
            this.CacheLocation = cache;
            this.ScreenId = id;
        }

        public static DesktopBackupFile FromStringContents(string contents)
        {
            if (string.IsNullOrWhiteSpace(contents))
                return null;

            var data = contents.Split('\n');
            if (data.Length > 0)
            {
                return new DesktopBackupFile(data[0], data[1], data[2]);
            }

            return null;
        }
        #endregion

        #region Private Helpers
        public void WriteMetadata()
        {
            string filepath = BackupPath;
            if (File.Exists(filepath) == false)
            {
                File.WriteAllLines(filepath, FileContents);
            }
            else
            {
                // This is a bad desktop image, use file from previous backup
                if (OriginalLocation.Contains(Util.CACHE_DIR))
                {
                    string contents = File.ReadAllText(filepath);
                    var oldBackup = DesktopBackupFile.FromStringContents(contents);
                    this.CopyFrom(oldBackup);
                }

                File.WriteAllLines(filepath, FileContents);
            }
        }

        public void CopyFrom(DesktopBackupFile oldBackup)
        {
            this.OriginalLocation = oldBackup.OriginalLocation;
            this.ScreenId = oldBackup.ScreenId;
            this.CacheLocation = oldBackup.CacheLocation;
        }
        #endregion
    }

    public class DesktopBackup
    {
        private static Dictionary<string, DesktopBackupFile> _backups;

        public static Dictionary<string, DesktopBackupFile> Backups
        {
            get
            {
                if (_backups == null)
                {
                    _backups = new Dictionary<string, DesktopBackupFile>();

                    var files = Directory.EnumerateFiles(Util.DesktopBackupDirectory, "*.screendata");
                    foreach (var location in files)
                    {
                        string contents = File.ReadAllText(location);
                        var backupFile = DesktopBackupFile.FromStringContents(contents);
                        _backups[backupFile.ScreenId] = backupFile;
                    }
                }

                return _backups;
            }
        }

        public static Dictionary<string, DesktopBackupFile> BackupDesktops()
        {
            var backups = new Dictionary<string, DesktopBackupFile>();

            foreach (var screen in NSScreen.Screens)
            {
                try
                {
                    string id = Util.ScreenId(screen);
                    string localPath = Util.ScreenImagePath(screen);

                    FileInfo fileInfo = new FileInfo(localPath);
                    string newPath = Path.Combine(Util.DesktopBackupDirectory, $"{id}{fileInfo.Extension}");

                    var backup = new DesktopBackupFile(localPath, "", id);
                    backup.WriteMetadata();

                    backups.Add(id, backup);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }

            DesktopBackup._backups = backups;
            return backups;
        }

        public static void RestoreDesktops()
        {
            if (Backups != null && Backups.Count > 0)
            {
                foreach (var screen in NSScreen.Screens)
                {
                    var id = Util.ScreenId(screen);
                    if (Backups.ContainsKey(id))
                    {
                        var backupFile = _backups[id];
                        NSError errorContainer = new NSError();

                        var workspace = NSWorkspace.SharedWorkspace;
                        var options = workspace.DesktopImageOptions(screen);
                        if (File.Exists(backupFile.OriginalLocation))
                        {
                            var url = NSUrl.FromFilename(backupFile.OriginalLocation);
                            NSWorkspace.SharedWorkspace.SetDesktopImageUrl(url, screen, options, errorContainer);
                        }
                    }
                }
            }
        }

        public static void RemoveAllMetadata()
        {
            var files = Directory.EnumerateFiles(Util.DesktopBackupDirectory, "*.screendata");
            foreach (var location in files)
            {
                File.Delete(location);
            }
        }
    }
}
