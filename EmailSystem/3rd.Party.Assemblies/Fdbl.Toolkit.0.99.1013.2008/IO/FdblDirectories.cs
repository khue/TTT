using System;
using System.Collections;
using System.IO;

namespace Fdbl.Toolkit.IO {

    #region Enumerations

    [FlagsAttribute]
    public enum FdblCopyOptions : byte {
        None,
        Overwrite,
        Skip,
        SweepBefore
    }

    [FlagsAttribute]
    public enum FdblMoveOptions : byte {
        None,
        Overwrite,
        Skip,
        SweepBefore
    }

    #endregion

    public class FdblDirectories {

        #region Members (Static)

        private static readonly double KiloByte = 1024;

        #endregion

        #region Methods - Public (Static)

        public static void Copy(string source, string target, bool recurse, FdblCopyOptions options) {

            if (source == null) throw new ArgumentNullException("source directory is null");
            if (target == null) throw new ArgumentNullException("target directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");
            if (target.Trim().Length == 0) throw new ArgumentException("target directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());
            target = Utils.FdblFormats.ForOS(target.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            if (options == FdblCopyOptions.SweepBefore) Sweep(target);

            _CopyFile(source, target, options == FdblCopyOptions.Overwrite, true);

        }

        public static void CopySingle(string source, string target, FdblCopyOptions options) {

            if (source == null) throw new ArgumentNullException("source directory is null");
            if (target == null) throw new ArgumentNullException("target directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");
            if (target.Trim().Length == 0) throw new ArgumentException("target directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());
            target = Utils.FdblFormats.ForOS(target.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            if (options == FdblCopyOptions.SweepBefore) Sweep(target);

            _CopyFile(source, target, options == FdblCopyOptions.Overwrite, false);

        }

        public static bool Exists(string path) {

            if (File.Exists(path)) throw new ArgumentException(string.Format("path was a file: {0}", path));

            return Directory.Exists(path);

        }

        public static ArrayList GetDirectories(string source) {
            return GetDirectories(source, "*", false);
        }

        public static ArrayList GetDirectories(string source, bool recurse) {
            return GetDirectories(source, "*", recurse);
        }

        public static ArrayList GetDirectories(string source, string pattern) {
            return GetDirectories(source, pattern, false);
        }

        public static ArrayList GetDirectories(string source, string pattern, bool recurse) {

            if (source == null) throw new ArgumentNullException("source directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            if (pattern == null || pattern.Trim().Length == 0) pattern = "*";

            ArrayList dirs = new ArrayList();
            _GetDirectoryListing(source, source, pattern, recurse, dirs);

            return dirs;

        }

        public static ArrayList GetFiles(string source) {
            return GetFiles(source, "*", false);
        }

        public static ArrayList GetFiles(string source, bool recurse) {
            return GetFiles(source, "*", recurse);
        }

        public static ArrayList GetFiles(string source, string pattern) {
            return GetFiles(source, pattern, false);
        }

        public static ArrayList GetFiles(string source, string pattern, bool recurse) {

            if (source == null) throw new ArgumentNullException("source directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            if (pattern == null || pattern.Trim().Length == 0) pattern = "*";

            ArrayList dirs = new ArrayList();
            _GetFileListing(source, source, pattern, recurse, dirs);

            return dirs;

        }

        public static ArrayList GetListing(string source) {
            return GetListing(source, false);
        }

        public static ArrayList GetListing(string source, bool recurse) {

            if (source == null) throw new ArgumentNullException("source directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            ArrayList dirs = new ArrayList();
            _GetFullListing(source, source, recurse, dirs);

            return dirs;

        }

        public static int GetNumberOfFiles(string source) {
            return GetNumberOfFiles(source, "*", false);
        }

        public static int GetNumberOfFiles(string source, string pattern) {
            return GetNumberOfFiles(source, pattern, false);
        }

        public static int GetNumberOfFiles(string source, string pattern, bool recurse) {

            ArrayList files = GetFiles(source, pattern, recurse);

            return files.Count;

        }

        public static double GetDirectorySize(string source, bool recurse) {
            return GetDirectorySize(source, "*", recurse);
        }

        public static double GetDirectorySize(string source, string pattern) {
            return GetDirectorySize(source, pattern, false);
        }

        public static double GetDirectorySize(string source, string pattern, bool recurse) {

            if (source == null) throw new ArgumentNullException("source directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            if (pattern == null || pattern.Trim().Length == 0) pattern = "*";

            ulong dirTotal = _GetDirectorySize(source, source, pattern, recurse);
            double dirKB = (double)dirTotal / KiloByte;

            return Math.Round(dirKB, 3);

        }

        public static void Make(string path) {

            if (File.Exists(path)) throw new ArgumentException(string.Format("path was a file: {0}", path));

            if (Directory.Exists(path)) return;

            Directory.CreateDirectory(path);

        }

        public static void Move(string source, string target) {

            if (source == null) throw new ArgumentNullException("source directory is null");
            if (target == null) throw new ArgumentNullException("target directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");
            if (target.Trim().Length == 0) throw new ArgumentException("target directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());
            target = Utils.FdblFormats.ForOS(target.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));
            if (!Directory.Exists(target)) throw new IOException(string.Format("target directory exists: {0}", target));

            Directory.Move(source, target);

        }

        public static void Move(string source, string target, FdblMoveOptions options) {

            if (source == null) throw new ArgumentNullException("source directory is null");
            if (target == null) throw new ArgumentNullException("target directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");
            if (target.Trim().Length == 0) throw new ArgumentException("target directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());
            target = Utils.FdblFormats.ForOS(target.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            if (options == FdblMoveOptions.Skip && Directory.Exists(target)) return;
            if (options == FdblMoveOptions.SweepBefore) Sweep(target);

            _MoveFile(source, target, options == FdblMoveOptions.Overwrite, true);

        }

        public static void MoveSingle(string source, string target, FdblMoveOptions options) {

            if (source == null) throw new ArgumentNullException("source directory is null");
            if (target == null) throw new ArgumentNullException("target directory is null");

            if (source.Trim().Length == 0) throw new ArgumentException("source directory is blank");
            if (target.Trim().Length == 0) throw new ArgumentException("target directory is blank");

            source = Utils.FdblFormats.ForOS(source.Trim());
            target = Utils.FdblFormats.ForOS(target.Trim());

            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(string.Format("source directory does not exist: {0}", source));

            if (options == FdblMoveOptions.Skip && Directory.Exists(target)) return;
            if (options == FdblMoveOptions.SweepBefore) Sweep(target);

            _MoveFile(source, target, options == FdblMoveOptions.Overwrite, false);

        }

        public static void Remove(string path, bool recurse) {

            if (!Directory.Exists(path)) return;

            Directory.Delete(path, recurse);

        }

        public static void Sweep(string path) {
            Sweep(path, Array.CreateInstance(typeof(string), 0), false);
        }

        public static void Sweep(string path, bool recurse) {
            Sweep(path, Array.CreateInstance(typeof(string), 0), recurse);
        }

        public static void Sweep(string path, Array exclusions) {
            Sweep(path, exclusions, false);
        }

        public static void Sweep(string path, Array exclusions, bool recurse) {

            if (path == null || path.Trim().Length == 0) return;

            path = Utils.FdblFormats.ForOS(path.Trim());

            if (exclusions == null) exclusions = Array.CreateInstance(typeof(string), 0);

            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fi = di.GetFiles();
            int idx = -1;

            for (int ndx = 0; ndx < fi.Length; ndx++) {

                if (exclusions.Length > 0) idx = Array.BinarySearch(exclusions, fi[ndx].Name);
                if (idx < 0) fi[ndx].Delete();

            }

            if (!recurse) return;

            DirectoryInfo[] dirs = di.GetDirectories();

            for (int ndx = 0; ndx < dirs.Length; ndx++) {
                Sweep(dirs[ndx].FullName, exclusions, recurse);
            }

        }

        #endregion

        #region Contructors

        private FdblDirectories() { }

        #endregion

        #region Methods - Private (Static)

        private static void _CopyFile(string source, string target, bool overwrite, bool recurse) {

            if (source == null || source.Trim().Length == 0) return;
            if (target == null || target.Trim().Length == 0) return;

            source = Utils.FdblFormats.ForOS(source.Trim());
            target = Utils.FdblFormats.ForOS(target.Trim());

            if (!Directory.Exists(target)) Directory.CreateDirectory(target);

            string[] files = Directory.GetFileSystemEntries(source);

            for (int ndx = 0; ndx < files.Length; ndx++) {

                string fName = string.Format("{0}{1}", target, Path.GetFileName(files[ndx]));

                if (Directory.Exists(files[ndx])) {

                    if (recurse) _CopyFile(files[ndx], fName, overwrite, recurse);

                } else {

                    if (overwrite) {

                        if (File.Exists(fName)) File.Delete(fName);
                        File.Copy(files[ndx], fName, true);

                    } else {

                        if (!File.Exists(fName)) File.Copy(files[ndx], fName, false);

                    }

                }

            }

        }

        private static void _GetDirectoryListing(string root, string source, string pattern, bool recurse, ArrayList dirs) {

            if (source == null || source.Trim().Length == 0) return;

            source = Utils.FdblFormats.ForOS(source.Trim());
            string[] folders = Directory.GetDirectories(source, pattern);

            for (int ndx = 0; ndx < folders.Length; ndx++) {

                string f = folders[ndx];
                FdblFileInformation fi = new FdblFileInformation();

                fi.IsDirectory = true;
                fi.FullPath = Utils.FdblFormats.ForOS(f);
                fi.RelativePath = fi.FullPath.Substring(root.Length);
                fi.FileName = string.Empty;

                dirs.Add(fi);

                if (recurse) _GetDirectoryListing(root, f, pattern, recurse, dirs);
            }

        }

        private static ulong _GetDirectorySize(string root, string source, string pattern, bool recurse) {

            ulong dirTotal = 0;

            if (source == null || source.Trim().Length == 0) return dirTotal;

            source = Utils.FdblFormats.ForOS(source.Trim());
            string[] files = Directory.GetFiles(source, pattern);

            for (int ndx = 0; ndx < files.Length; ndx++) {

                FileInfo fi = new FileInfo(files[ndx]);
                dirTotal += (ulong)fi.Length;

            }

            if (!recurse) return dirTotal;

            string[] folders = Directory.GetDirectories(source);

            for (int ndx = 0; ndx < folders.Length; ndx++) {
                dirTotal += _GetDirectorySize(root, Utils.FdblFormats.ForOS(folders[ndx]), pattern, recurse);
            }

            return dirTotal;

        }


        private static void _GetFileListing(string root, string source, string pattern, bool recurse, ArrayList dirs) {

            if (source == null || source.Trim().Length == 0) return;

            source = Utils.FdblFormats.ForOS(source.Trim());
            string[] files = Directory.GetFileSystemEntries(source, pattern);

            for (int ndx = 0; ndx < files.Length; ndx++) {

                string f = files[ndx];
                FdblFileInformation fi = new FdblFileInformation();

                fi.IsDirectory = Directory.Exists(f);
                fi.FullPath = fi.IsDirectory ? Utils.FdblFormats.ForOS(f) : Path.GetDirectoryName(f);
                fi.RelativePath = fi.FullPath.Substring(root.Length);
                fi.FileName = fi.IsDirectory ? string.Empty : Path.GetFileName(f);

                if (!fi.IsDirectory) dirs.Add(fi);
            }

            if (!recurse) return;

            string[] folders = Directory.GetDirectories(source);

            for (int ndx = 0; ndx < folders.Length; ndx++) {
                _GetFileListing(root, Utils.FdblFormats.ForOS(folders[ndx]), pattern, recurse, dirs);
            }

        }

        private static void _GetFullListing(string root, string source, bool recurse, ArrayList dirs) {

            if (source == null || source.Trim().Length == 0) return;

            source = Utils.FdblFormats.ForOS(source.Trim());
            string[] files = Directory.GetFileSystemEntries(source);

            for (int ndx = 0; ndx < files.Length; ndx++) {

                string f = files[ndx];
                FdblFileInformation fi = new FdblFileInformation();

                fi.IsDirectory = Directory.Exists(f);
                fi.FullPath = fi.IsDirectory ? Utils.FdblFormats.ForOS(f) : Path.GetDirectoryName(f);
                fi.RelativePath = fi.FullPath.Substring(root.Length);
                fi.FileName = fi.IsDirectory ? string.Empty : Path.GetFileName(f);

                dirs.Add(fi);

                if (fi.IsDirectory && recurse) _GetFullListing(root, f, recurse, dirs);

            }

        }

        private static void _MoveFile(string source, string target, bool overwrite, bool recurse) {

            if (source == null || source.Trim().Length == 0) return;
            if (target == null || target.Trim().Length == 0) return;

            source = Utils.FdblFormats.ForOS(source.Trim());
            target = Utils.FdblFormats.ForOS(target.Trim());

            if (!Directory.Exists(target)) Directory.CreateDirectory(target);

            string[] files = Directory.GetFileSystemEntries(source);

            for (int ndx = 0; ndx < files.Length; ndx++) {

                string fName = string.Format("{0}{1}", target, Path.GetFileName(files[ndx]));

                if (Directory.Exists(files[ndx])) {

                    if (recurse) _MoveFile(files[ndx], fName, overwrite, recurse);

                } else {

                    if (overwrite && File.Exists(fName)) File.Delete(fName);
                    if (!File.Exists(fName)) File.Move(files[ndx], fName);

                }

            }

        }

        #endregion

    }

}
