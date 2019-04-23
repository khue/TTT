using System;
using System.Data;
using System.Data.SqlClient;

namespace Fdbl.Toolkit.Sql {

    public class FdblSqlTransaction : FdblSqlStandard {

        #region Members

        private SqlTransaction _SqlTransaction;
        private string _TransactionKey;

        #endregion

        #region Methods - Public

        public void CloseTransaction(FdblTransactionAction action) {

            if (_SqlTransaction == null) throw new FdblClassMemberException("sql transaction does not exist");

            if (action == FdblTransactionAction.Commit) {

                _SqlTransaction.Commit();

            } else if (action == FdblTransactionAction.Rollback) {

                _SqlTransaction.Rollback(_TransactionKey);

            }

            _SqlTransaction.Dispose();
            _SqlTransaction = null;

            ResetConnection();

        }

        public void OpenTransaction() {
            OpenTransaction(IsolationLevel.ReadCommitted);
        }

        public void OpenTransaction(IsolationLevel iso) {

            if (_SqlTransaction != null) throw new FdblClassMemberException("sql transaction already exists");

            _SqlTransaction = SqlConnection.BeginTransaction(iso, _TransactionKey);

            SqlCommand.Transaction = _SqlTransaction;

        }

        public void CreateSavePoint(string savePointName) {

            if (savePointName == null) throw new ArgumentNullException("sql transaction save point name is null");
            if (savePointName.Trim().Length == 0) throw new ArgumentException("sql transaction save point name is blank");

            if (_SqlTransaction == null) throw new FdblClassMemberNullException("sql transaction does not exist");

            _SqlTransaction.Save(savePointName);

        }

        public override void Dispose() {

            try {

                CloseConnection();

                if (_SqlTransaction != null) _SqlTransaction.Dispose();

            } finally {

                base.Dispose();

            }

        }

        public void RollbackToSavePoint(string savePointName) {

            if (savePointName == null) throw new ArgumentNullException("sql transaction save point name is null");
            if (savePointName.Trim().Length == 0) throw new ArgumentException("sql transaction save point name is blank");

            if (_SqlTransaction == null) throw new FdblClassMemberNullException("sql transaction does not exist");

            _SqlTransaction.Rollback(savePointName);

        }

        #endregion

        #region Constructors

        public FdblSqlTransaction(string sqlConnectionInfo)
            : base(sqlConnectionInfo) {

            _SqlTransaction = null;
            _TransactionKey = Crypto.FdblGeneric.GetKey(30);

        }

        #endregion

    }

}