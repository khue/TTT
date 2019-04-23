using System;
using System.IO;
using System.IO.Compression;

namespace Fdbl.Toolkit.Compression {

    public class FdblCompress {

        #region Constructors

        private FdblCompress() { }

        #endregion

        #region Methods - Public

        public static string CompressFile(string fnameSource, string pathSource, string pathTarget) {

            if (fnameSource == null) throw new ArgumentNullException("The source file name is null");
            if (pathSource == null) throw new ArgumentNullException("The source path name is null");
            if (pathTarget == null) throw new ArgumentNullException("The target path name is null");

            if (fnameSource.Trim().Length == 0) throw new ArgumentException("The source file name is blank");
            if (pathSource.Trim().Length == 0) throw new ArgumentException("The source path name is null");
            if (pathTarget.Trim().Length == 0) throw new ArgumentException("The target path name is null");

            FileStream fsSource = null;
            FileStream fsTarget = null;
            GZipStream gzTarget = null;

            try {

                if (!pathSource.EndsWith(Convert.ToString(Path.DirectorySeparatorChar))) pathSource += Path.DirectorySeparatorChar;
                if (!pathTarget.EndsWith(Convert.ToString(Path.DirectorySeparatorChar))) pathTarget += Path.DirectorySeparatorChar;

                if (!File.Exists(string.Format("{0}{1}", pathSource, fnameSource))) throw new FileNotFoundException(string.Format("Could not locate file: {0}{1}", pathSource, fnameSource));

                if (!Directory.Exists(pathTarget)) Directory.CreateDirectory(pathTarget);

                fsSource = File.OpenRead(string.Format("{0}{1}", pathSource, fnameSource));

                if (fsSource.Length == 0) return string.Empty;

                fsTarget = File.Create(string.Format("{0}{1}.gz", pathTarget, fnameSource));
                gzTarget = new GZipStream(fsTarget, CompressionMode.Compress);

                byte[] buffer = new byte[fsSource.Length];

                fsSource.Read(buffer, 0, buffer.Length);
                gzTarget.Write(buffer, 0, buffer.Length);

                gzTarget.Flush();

                return string.Format("{0}{1}.gz", pathTarget, fnameSource);


            } finally {

                if (gzTarget != null) gzTarget.Close();
                if (fsTarget != null) fsTarget.Close();
                if (fsSource != null) fsSource.Close();

            }

        }

        public static string CompressString(string source) {

            return string.Empty;

        }

        public static string DecompressFile(string fnameSource, string pathSource, string pathTarget) {

            return string.Empty;

        }

        public static string DecompressString(string source) {

            return string.Empty;

        }

        #endregion

    }
}
