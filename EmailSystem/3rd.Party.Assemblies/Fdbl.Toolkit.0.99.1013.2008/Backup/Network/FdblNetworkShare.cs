using System;
using System.IO;

namespace Fdbl.Toolkit.Network {

    public class FdblNetworkShare {

        #region Members

        private string _RemoteServer;
        private string _RemotePath;
        private string _LocalDrive;
        private string _Remarks;
        private NetworkShareTypes _ShareType;
        private bool _IsFileSystem;

        #endregion

        #region Properties - Public

        public bool IsFileSystem {
            get { return _IsFileSystem; }
        }

        public string LocalDrive {
            get { return _LocalDrive; }
        }

        public string Remarks {
            get { return _Remarks; }
        }

        public string RemotePath {
            get { return _RemotePath; }
        }

        public string RemoteServer {
            get { return _RemoteServer; }
        }

        public NetworkShareTypes ShareType {
            get { return _ShareType; }
        }

        #endregion

        #region Methods - Public

        public string GetDetails() {
            return string.Format("Remote: {0} / Local Drive: {1} / Type: {2}", ToString(), _LocalDrive, Convert.ToString(_ShareType));
        }

        public DirectoryInfo GetDirectoryInfo() {

            if (!_IsFileSystem) return null;

            if (_RemoteServer.Trim().Length == 0 && _RemotePath.Trim().Length == 0) return null;

            if (_RemoteServer.Trim().Length == 0) return new DirectoryInfo(_RemotePath);

            return new DirectoryInfo(ToString());

        }

        public FdblNetworkDrive GetNetworkDrive() {

            FdblNetworkDrive drive = new FdblNetworkDrive();

            drive.RemoteServer = _RemoteServer;
            drive.RemotePath = _RemotePath;

            return drive;

        }

        public bool MatchesPath(string path) {

            if (path == null || path.Trim().Length == 0) return false;

            return path.ToLower().StartsWith(_RemotePath.ToLower());

        }

        public override string ToString() {
            if (_RemoteServer == null || _RemoteServer.Trim().Length == 0) return string.Format(@"\\{0}\{1}", Environment.MachineName, _RemotePath);
            return string.Format(@"\\{0}\{1}", _RemoteServer, _RemotePath);
        }

        #endregion

        #region Constructors

        public FdblNetworkShare(string remoteServer, string remotePath, string localDrive, string remarks, NetworkShareTypes shareType) {

            _RemoteServer = remoteServer == null ? string.Empty : remoteServer.Trim();
            _RemotePath = remotePath == null ? string.Empty : remotePath.Trim();
            _LocalDrive = localDrive == null ? string.Empty : localDrive.Trim();
            _Remarks = remarks == null ? string.Empty : Remarks.Trim();
            _ShareType = shareType;
            _IsFileSystem = GetIsFileSystem();

            if (_ShareType == NetworkShareTypes.Special && _RemotePath.Equals("IPC$")) _ShareType |= NetworkShareTypes.IPC;

        }

        #endregion

        #region Methods - Private

        private bool GetIsFileSystem() {
            if ((_ShareType & NetworkShareTypes.Device) != 0) return false;
            if ((_ShareType & NetworkShareTypes.IPC) != 0) return false;
            if ((_ShareType & NetworkShareTypes.Printer) != 0) return false;
            if ((_ShareType & NetworkShareTypes.Special) == 0) return true;
            if (_ShareType == NetworkShareTypes.Special && !(_RemotePath == null || _RemotePath.Trim().Length == 0)) return true;
            return false;
        }

        #endregion

    }

}