using System;
using System.Data;
using System.Data.SqlClient;

namespace Fdbl.Toolkit.Sql {

    #region Enumerations

    [FlagsAttribute]
    public enum FdblExecuteAction : byte {
        Add,
        Insert,
        Update,
        Delete
    }

    [FlagsAttribute]
    public enum FdblQueryResult : byte {
        Collection,
        Xml
    }

    [FlagsAttribute]
    public enum FdblTransactionAction : byte {
        Commit,
        Rollback
    }

    #endregion

    #region Structures

    public struct FdblSqlReturnCodes {

        public static readonly int Unknown = -99999;
        public static readonly int MultipleMatchFound = -7;
        public static readonly int DataFound = -6;
        public static readonly int MatchFound = -5;
        public static readonly int RelationFound = -4;
        public static readonly int NoData = -3;
        public static readonly int NoMatch = -2;
        public static readonly int NoRelation = -1;
        public static readonly int Success = 0;

    }

    #endregion

    public class FdblSql : IDisposable {

        #region Memebers

        private SqlConnection _SqlConnection;
        private SqlDataReader _SqlDataReader;
        private SqlCommand _SqlCommand;
        private DataTable _DataTable;
        private object _SqlErrorCode;
        private string _SqlConnectionInfo;
        private int _DataTableIndex;
        private bool _AutoConnect;

        #endregion

        #region Properties - Public

        public bool AutoConnect {
            get { return _AutoConnect; }
            set { _AutoConnect = value; }
        }

        public SqlCommand SqlCommand {
            get { return _SqlCommand; }
        }

        public SqlConnection SqlConnection {
            get { return _SqlConnection; }
        }

        public string SqlConnectionInfo {
            get { return _SqlConnectionInfo; }
        }

        public SqlDataReader SqlDataReader {
            get { return _SqlDataReader; }
        }

        public DataTable DataTable {
            get { return _DataTable; }
        }

        public object SqlErrorCode {
            get { return _SqlErrorCode; }
        }

        #endregion

        #region Methods - Public

        public virtual void ChangeConnection(string sqlConnectionInfo, bool force) {

            if (sqlConnectionInfo == null) throw new ArgumentNullException("sql connection info is null");
            if (sqlConnectionInfo.Trim().Length == 0) throw new ArgumentException("sql connection info is blank");

            if (!force) {

                if (_SqlConnection.State != ConnectionState.Closed) throw new Exception("sql connection is currently open");

            } else {

                CloseConnection();

            }

            _SqlConnection.ConnectionString = sqlConnectionInfo;

        }

        public virtual void CloseCommand() {
            CloseCommand(false);
        }

        public virtual void CloseCommand(bool force) {

            if (_SqlCommand == null) return;

            if (force) CloseDataReader();

            if (_SqlDataReader != null) throw new Exception("sql datareader member is active");

            _SqlCommand.Dispose();
            _SqlCommand = null;

        }

        public virtual void CloseConnection() {
            CloseConnection(false);
        }

        public virtual void CloseConnection(bool force) {

            CloseCommand(force);

            if (_SqlCommand != null) throw new Exception("sql command member is active");
            if (_SqlDataReader != null) throw new Exception("sql datareader member is active");

            if (_SqlConnection.State != ConnectionState.Closed) _SqlConnection.Close();

        }

        public virtual object CloseDataReader() {

            if (_SqlDataReader == null) return FdblSqlReturnCodes.Success;

            _SqlDataReader.Close();
            _SqlDataReader = null;

            if (_AutoConnect) {

                if (_SqlConnection.State != ConnectionState.Closed) _SqlConnection.Close();

            }

            return _GetReturnValue(FdblSqlReturnCodes.Success);

        }

        public virtual object CloseDataTable() {

            if (_DataTable == null) return FdblSqlReturnCodes.Success;

            _DataTable.Clear();
            _DataTable = null;

            if (_AutoConnect) {

                if (_SqlConnection.State != ConnectionState.Closed) _SqlConnection.Close();

            }

            return FdblSqlReturnCodes.Success;

        }

        public virtual void Dispose() {

            if (_DataTable != null) {

                _DataTable.Dispose();
                _DataTable = null;

            }

            if (_SqlDataReader != null) {

                _SqlDataReader.Close();
                _SqlDataReader = null;

            }

            if (_SqlCommand != null) {

                _SqlCommand.Dispose();
                _SqlCommand = null;

            }

            if (_SqlConnection != null) {

                _SqlConnection.Close();
                _SqlConnection.Dispose();
                _SqlConnection = null;

            }

            System.GC.SuppressFinalize(this);

        }

        public object GetDataReaderValue(int pos, object defaultReturn) {

            if (pos < 0 || pos >= _SqlDataReader.FieldCount) throw new ArgumentException("out of column bounds");

            if (_SqlDataReader.IsDBNull(pos)) return defaultReturn;

            return _SqlDataReader[pos];

        }

        public string GetDataReaderValueAsString(int pos) {

            if (pos < 0 || pos >= _SqlDataReader.FieldCount) throw new ArgumentException("out of column bounds");

            if (_SqlDataReader.IsDBNull(pos)) return string.Empty;

            return _SqlDataReader[pos].ToString();

        }

        public object GetDataTableValue(int pos, object defaultReturn) {

            if (pos < 0 || pos >= _DataTable.Columns.Count) throw new ArgumentException("out of column bounds");

            if (_DataTable.Rows[_DataTableIndex].IsNull(pos)) return defaultReturn;

            return _DataTable.Rows[_DataTableIndex][pos];

        }

        public string GetDataTableValueAsString(int pos) {

            if (pos < 0 || pos >= _DataTable.Columns.Count) throw new ArgumentException("out of column bounds");

            if (_DataTable.Rows[_DataTableIndex].IsNull(pos)) return string.Empty;

            return _DataTable.Rows[_DataTableIndex][pos].ToString();

        }

        public bool MoveNextDataReader() {
            return MoveNextDataReader(false);
        }

        public bool MoveNextDataReader(bool force) {

            if (_SqlDataReader == null) {

                if (force) return false;
                throw new FdblClassMemberNullException("sql datareader member is null");

            }

            if (_SqlDataReader.IsClosed) {

                if (force) return false;
                throw new Exception("sql datareader member not active");

            }

            return _SqlDataReader.Read();

        }

        public bool MoveNextDataTable() {
            return MoveNextDataReader(false);
        }

        public bool MoveNextDataTable(bool force) {

            if (_DataTable == null) {

                if (force) return false;
                throw new FdblClassMemberNullException("datatable member is null");

            }

            if (!_DataTable.IsInitialized) {

                if (force) return false;
                throw new Exception("datatable member not active");

            }

            _DataTableIndex++;

            if (_DataTableIndex == _DataTable.Rows.Count) return false;

            return true;

        }

        public virtual void OpenCommand() {
            OpenCommand(null, CommandType.Text, false);
        }

        public virtual void OpenCommand(bool force) {
            OpenCommand(null, CommandType.Text, force);
        }

        public virtual void OpenCommand(CommandType cmdType) {
            OpenCommand(null, cmdType, false);
        }

        public virtual void OpenCommand(CommandType cmdType, bool force) {
            OpenCommand(null, cmdType, false);
        }

        public virtual void OpenCommand(string cmdText, CommandType cmdType, bool force) {

            CloseCommand(force);

            _SqlCommand = new SqlCommand();
            _SqlCommand.Connection = _SqlConnection;
            _SqlCommand.CommandType = cmdType;

            if (cmdText != null) _SqlCommand.CommandText = cmdText;

        }

        public virtual void OpenConnection() {
            OpenConnection(false);
        }

        public virtual void OpenConnection(bool force) {

            CloseConnection(force);

            _SqlConnection.Open();

        }

        public virtual object OpenDataReader() {
            return OpenDataReader(false);
        }

        public virtual object OpenDataReader(bool force) {

            if (_AutoConnect) {

                if (_SqlConnection.State == ConnectionState.Broken) _SqlConnection.Close();
                if (_SqlConnection.State == ConnectionState.Closed) _SqlConnection.Open();

            }

            if (_SqlConnection.State == ConnectionState.Closed) throw new Exception("sql connection is not open");
            if (_SqlConnection.State == ConnectionState.Broken) throw new Exception("sql connection is broken");

            if (_SqlCommand == null) throw new FdblClassMemberNullException("sql command member is null");
            if (_SqlCommand.CommandText == null) throw new Exception("sql command text is null");
            if (_SqlCommand.CommandText.Trim().Length == 0) throw new Exception("sql command text is blank");

            if (force) CloseDataReader();

            if (_SqlDataReader != null) throw new Exception("sql datareader member already in use");

            _SqlDataReader = _SqlCommand.ExecuteReader();

            if (_SqlDataReader.HasRows) return FdblSqlReturnCodes.Success;

            _SqlDataReader.Close();
            _SqlDataReader = null;

            _SqlErrorCode = _GetSqlErrorValue(FdblSqlReturnCodes.Success);

            return _GetReturnValue(FdblSqlReturnCodes.Unknown);

        }

        public virtual object OpenDataTable() {
            return OpenDataTable(false);
        }

        public virtual object OpenDataTable(bool force) {

            if (_AutoConnect) {

                if (_SqlConnection.State == ConnectionState.Broken) _SqlConnection.Close();
                if (_SqlConnection.State == ConnectionState.Closed) _SqlConnection.Open();

            }

            if (_SqlConnection.State == ConnectionState.Closed) throw new Exception("sql connection is not open");
            if (_SqlConnection.State == ConnectionState.Broken) throw new Exception("sql connection is broken");

            if (_SqlCommand == null) throw new FdblClassMemberNullException("sql command member is null");
            if (_SqlCommand.CommandText == null) throw new Exception("sql command text is null");
            if (_SqlCommand.CommandText.Trim().Length == 0) throw new Exception("sql command text is blank");

            if (force) CloseDataReader();

            if (_SqlDataReader != null) throw new Exception("sql datareader member already in use");

            _SqlDataReader = _SqlCommand.ExecuteReader();

            
            if (_SqlDataReader.HasRows) {

                _DataTable = new DataTable("sqlData");
                _DataTable.Load(_SqlDataReader);
                _DataTableIndex = 0;

            }

            _SqlDataReader.Close();
            _SqlDataReader = null;

            _SqlErrorCode = _GetSqlErrorValue(FdblSqlReturnCodes.Success);

            return _GetReturnValue(FdblSqlReturnCodes.Unknown);

        }

        public virtual void ResetConnection() {

            CloseConnection();

            _SqlConnection = new SqlConnection(_SqlConnectionInfo);

        }

        public virtual void ResetCommand() {

            CloseDataReader();

            for (int ndx = 0; ndx < _SqlCommand.Parameters.Count; ndx++) {
                _SqlCommand.Parameters[ndx].Value = DBNull.Value;
            }

        }

        public virtual void ResetDataTableIndex() {

            _DataTableIndex = 0;

        }

        #endregion

        #region Constructors

        public FdblSql(string sqlConnectionInfo) {

            if (sqlConnectionInfo == null) throw new ArgumentNullException("sql connection info is null");
            if (sqlConnectionInfo.Trim().Length == 0) throw new ArgumentException("sql connection info is blank");

            _SqlConnectionInfo = sqlConnectionInfo;
            _SqlConnection = new SqlConnection(_SqlConnectionInfo);
            _AutoConnect = false;

        }

        #endregion

        #region Methods - Private

        private object _GetReturnValue(int defaultReturn) {

            object ret = null;

            if (_SqlCommand.Parameters.Contains("RETURN")) {
                ret = _SqlCommand.Parameters["RETURN"].Value;
            } else if (_SqlCommand.Parameters.Contains("RETURNVALUE")) {
                ret = _SqlCommand.Parameters["RETURNVALUE"].Value;
            } else if (_SqlCommand.Parameters.Contains("RETURN_VALUE")) {
                ret = _SqlCommand.Parameters["RETURN_VALUE"].Value;
            } else if (_SqlCommand.Parameters.Contains("@RETURN")) {
                ret = _SqlCommand.Parameters["@RETURN"].Value;
            } else if (_SqlCommand.Parameters.Contains("@RETURNVALUE")) {
                ret = _SqlCommand.Parameters["@RETURNVALUE"].Value;
            } else if (_SqlCommand.Parameters.Contains("@RETURN_VALUE")) {
                ret = _SqlCommand.Parameters["@RETURN_VALUE"].Value;
            }

            if (ret == null) return defaultReturn;

            return ret;

        }

        private object _GetSqlErrorValue(int defaultReturn) {

            object ret = null;

            if (_SqlCommand.Parameters.Contains("@SqlErrorCode")) {
                ret = _SqlCommand.Parameters["@SqlErrorCode"].Value;
            } else if (_SqlCommand.Parameters.Contains("@Sql_ErrorCode")) {
                ret = _SqlCommand.Parameters["@Sql_ErrorCode"].Value;
            } else if (_SqlCommand.Parameters.Contains("@Sql_Error_Code")) {
                ret = _SqlCommand.Parameters["@Sql_Error_Code"].Value;
            }

            if (ret == null) return defaultReturn;

            return ret;

        }

        #endregion

    }

}