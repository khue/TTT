using System;

namespace Fdbl.Toolkit.Sql {

    public class FdblSqlFactory {

        #region Members

        private string _Server;
        private string _Database;
        private string _UserAccount;
        private string _AccountPassword;

        #endregion

        #region Properties - Public

        public string AccountPassword {
            get { return _AccountPassword; }
            set { _AccountPassword = value; }
        }

        public string Database {
            get { return _Database; }
            set { _Database = value; }
        }

        public string Server {
            get { return _Server; }
            set { _Server = value; }
        }

        public string UserAccount {
            get { return _UserAccount; }
            set { _UserAccount = value; }
        }

        #endregion

        #region Methods - Public

        public void Clear() {
            _Server = null;
            _Database = null;
            _UserAccount = null;
            _AccountPassword = null;
        }

        public string GetConnectionString() {

            _ValidateConnection();

            string pwd = (_AccountPassword == null ? string.Empty : string.Format("pwd={0};", _AccountPassword));

            return string.Format("server={0};database={1};uid={2};{3}", _Server, _Database, _UserAccount, pwd);
        }

        public FdblSql GetFdblSql() {

            return new FdblSql(GetConnectionString());

        }

        public FdblSqlStandard GetFdblSqlStandard() {

            return new FdblSqlStandard(GetConnectionString());

        }

        public FdblSqlTransaction GetFdblSqlTransaction() {

            return new FdblSqlTransaction(GetConnectionString());

        }


        #endregion

        #region Constructors

        public FdblSqlFactory() { }

        public FdblSqlFactory(string server, string database, string userAccount) {

            if (server == null) throw new ArgumentNullException("server is null");
            if (database == null) throw new ArgumentNullException("database is null");
            if (userAccount == null) throw new ArgumentNullException("user account is null");

            if (server.Trim().Length == 0) throw new ArgumentException("server is blank");
            if (database.Trim().Length == 0) throw new ArgumentException("database is blank");
            if (userAccount.Trim().Length == 0) throw new ArgumentException("user account is blank");

            _Server = server;
            _Database = database;
            _UserAccount = userAccount;
            _AccountPassword = null;

        }

        public FdblSqlFactory(string server, string database, string userAccount, string accountPassword) {

            if (server == null) throw new ArgumentNullException("server is null");
            if (database == null) throw new ArgumentNullException("database is null");
            if (userAccount == null) throw new ArgumentNullException("user account is null");
            if (accountPassword == null) throw new ArgumentNullException("account password is null");

            if (server.Trim().Length == 0) throw new ArgumentException("server is blank");
            if (database.Trim().Length == 0) throw new ArgumentException("database is blank");
            if (userAccount.Trim().Length == 0) throw new ArgumentException("user account is blank");
            if (accountPassword.Trim().Length == 0) throw new ArgumentException("account password is blank");

            _Server = server;
            _Database = database;
            _UserAccount = userAccount;
            _AccountPassword = accountPassword;

        }

        #endregion

        #region Methods - Private

        private void _ValidateConnection() {

            if (_Server == null) throw new FdblClassMemberNullException("server is null");
            if (_Database == null) throw new FdblClassMemberNullException("database is null");
            if (_UserAccount == null) throw new FdblClassMemberNullException("user name is null");

            if (_Server.Trim().Length == 0) throw new FdblClassMemberException("server is blank");
            if (_Database.Trim().Length == 0) throw new FdblClassMemberException("database is blank");
            if (_UserAccount.Trim().Length == 0) throw new FdblClassMemberException("user name is blank");

        }

        #endregion

    }

}