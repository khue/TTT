using System;
using System.IO;

namespace Fdbl.Toolkit.Network {

    public class FdblNetworkDrive {

        #region Members

        private string _RemoteServer;
        private string _RemotePath;
        private string _UserAccount;
        private string _AccountPassword;
        private string _LocalDrive;
        private bool _PersistentConnection;
        private bool _ForceConnection;
        private bool _ForceDisconnection;
        private bool _SaveCredentials;
        private bool _PromptForCredentials;

        #endregion

        #region Properties - Public

        public string AccountPassword {
            get { return _AccountPassword; }
            set { _AccountPassword = (value == null || value.Trim().Length == 0) ? null : value; }
        }

        public string LocalDrive {
            get { return _LocalDrive; }
            set { _LocalDrive = (value == null || value.Trim().Length == 0) ? null : value; }
        }

        public bool ForceConnection {
            get { return _ForceConnection; }
            set { _ForceConnection = value; }
        }

        public bool ForceDisconnection {
            get { return _ForceDisconnection; }
            set { _ForceDisconnection = value; }
        }

        public bool PersistentConnection {
            get { return _PersistentConnection; }
            set { _PersistentConnection = value; }
        }

        public bool PromptForCredentials {
            get { return _PromptForCredentials; }
            set { _PromptForCredentials = value; }
        }

        public string RemotePath {
            get { return _RemotePath; }
            set { _RemotePath = (value == null || value.Trim().Length == 0) ? null : value; }
        }

        public string RemoteServer {
            get { return _RemoteServer; }
            set { _RemoteServer = (value == null || value.Trim().Length == 0) ? null : value; }
        }

        public bool SaveCredentials {
            get { return _SaveCredentials; }
            set { _SaveCredentials = value; }
        }

        public string UserAccount {
            get { return _UserAccount; }
            set { _UserAccount = (value == null || value.Trim().Length == 0) ? null : value; }
        }

        #endregion

        #region Methods - Public

        public string GetDisconnectDrive() {

            if (_LocalDrive == null || _LocalDrive.Trim().Length == 0) return ToString();

            return _LocalDrive;

        }

        public string GetDetails() {
            return GetDetails(false);
        }

        public string GetDetails(bool all) {

            if (!all) return string.Format("Remote: {0} / User Account: {1}", ToString(), _UserAccount);

            return string.Format("Remote: {0} / User Account: {1} / Account Password: {2}", ToString(), _UserAccount, _AccountPassword);

        }

        public DirectoryInfo GetDirectoryInfo() {

            if (_RemoteServer == null) throw new FdblClassMemberNullException("remote server is null");
            if (_RemotePath == null) throw new FdblClassMemberNullException("remote path is null");

            if (_RemotePath.Trim().Length == 0) throw new FdblClassMemberException("remote path is blank");

            if (_RemoteServer.Trim().Length == 0) return new DirectoryInfo(_RemotePath);

            return new DirectoryInfo(ToString());

        }

        public bool LocalPathMatches(string path) {

            if (path == null || path.Trim().Length == 0) return false;

            return path.ToLower().StartsWith(_RemotePath.ToLower());

        }

        public bool RemotePathMatches(string path) {

            if (path == null || path.Trim().Length == 0) return false;

            return _RemotePath.ToLower().StartsWith(path.ToLower());

        }

        public override string ToString() {

            if (_RemotePath == null) throw new FdblClassMemberNullException("remote path is null");

            if (_RemotePath.Trim().Length == 0) throw new FdblClassMemberException("remote path is blank");

            if (_RemoteServer.Trim().Length == 0) return string.Format(@"\\{0}\{1}", Environment.MachineName, _RemotePath);

            return string.Format(@"\\{0}\{1}", _RemoteServer, _RemotePath);

        }

        #endregion

        #region Constructors

        public FdblNetworkDrive() {

            _RemoteServer = null;
            _RemotePath = null;
            _UserAccount = null;
            _AccountPassword = null;
            _LocalDrive = null;
            _PersistentConnection = false;
            _ForceConnection = false;
            _ForceDisconnection = true;
            _SaveCredentials = false;
            _PromptForCredentials = false;

        }

        #endregion

    }

}