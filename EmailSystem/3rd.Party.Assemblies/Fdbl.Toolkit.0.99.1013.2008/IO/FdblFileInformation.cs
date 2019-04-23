using System;
using System.IO;

namespace Fdbl.Toolkit.IO {

    public class FdblFileInformation {

        #region Members

        private string _FullPath;
        private string _RelativePath;
        private string _FileName;
        private bool _IsDirectory;

        #endregion

        #region Properties - Public

        public string FileName {
            get { return _FileName; }
            set { _FileName = value == null ? string.Empty : value; }
        }

        public string FullFile {
            get { return string.Format("{0}{1}", _FullPath, _FileName); }
        }

        public string FullPath {
            get { return _FullPath; }
            set { _FullPath = value == null ? string.Empty : Utils.FdblFormats.ForOS(value); }
        }

        public bool IsDirectory {
            get { return _IsDirectory; }
            set { _IsDirectory = value; }
        }

        public string RelativeFile {
            get { return string.Format("{0}{1}", _RelativePath, _FileName); }
        }

        public string RelativePath {
            get { return _RelativePath; }
            set { _RelativePath = value == null ? string.Empty : Utils.FdblFormats.ForOS(value); }
        }

        #endregion

        #region Methods - Public

        public uint GetCRC() {

            return Crypto.FdblHash.CRCFile(string.Format("{0}{1}", _FullPath, _FileName));

        }

        public string GetMD5() {

            return Crypto.FdblHash.HashFile(string.Format("{0}{1}", _FullPath, _FileName), Crypto.FdblHashType.MD5);

        }

        public string GetSHA1() {

            return Crypto.FdblHash.HashFile(string.Format("{0}{1}", _FullPath, _FileName), Crypto.FdblHashType.SHA1);

        }

        public string GetSHA256() {

            return Crypto.FdblHash.HashFile(string.Format("{0}{1}", _FullPath, _FileName), Crypto.FdblHashType.SHA256);

        }

        public string GetSHA384() {

            return Crypto.FdblHash.HashFile(string.Format("{0}{1}", _FullPath, _FileName), Crypto.FdblHashType.SHA384);

        }

        public string GetSHA512() {

            return Crypto.FdblHash.HashFile(string.Format("{0}{1}", _FullPath, _FileName), Crypto.FdblHashType.SHA512);

        }

        #endregion

        #region Constructors

        public FdblFileInformation() {

            _FullPath = string.Empty;
            _RelativePath = string.Empty;
            _FileName = string.Empty;
            _IsDirectory = false;

        }

        public FdblFileInformation(string fileName) {

            if (File.Exists(fileName)) {

                _FullPath = Utils.FdblFormats.ForOS(Path.GetDirectoryName(fileName));
                _FileName = Path.GetFileName(fileName);
                _RelativePath = string.Empty;
                _IsDirectory = false;

            } else {

                _FullPath = Utils.FdblFormats.ForOS(fileName);
                _RelativePath = string.Empty;
                _FileName = string.Empty;
                _IsDirectory = true;

            }
        }

        #endregion

    }

}
