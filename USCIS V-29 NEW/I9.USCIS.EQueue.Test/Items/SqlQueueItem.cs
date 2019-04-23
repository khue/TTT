using System;

namespace I9.USCIS.EQueue.Test.Items {

    internal class SqlQueueItem {

        #region Members

        private string _CaseNumber;
        private string _Comments;

        private int _USCISQueueErrorId = MyConsole.NullId;
        private int _USCISTransactionId = MyConsole.NullId;
        private int _USCISActionId = MyConsole.NullId;
        private int _USCISClosureId = MyConsole.NullId;

        private int _EmployeeId = MyConsole.NullId;
        private int _I9Id = MyConsole.NullId;

        #endregion

        #region Constructors

        public SqlQueueItem(Sql.spUSCISBE_QueueError_Get spQueueErrorGet) {

            if (spQueueErrorGet == null) throw new MyException("spUSCIS_QueueError_Get is null");

            _USCISQueueErrorId = Convert.ToInt32(spQueueErrorGet.GetDataReaderValue(0, MyConsole.NullId));
            _USCISTransactionId = Convert.ToInt32(spQueueErrorGet.GetDataReaderValue(1, MyConsole.NullId));
            _USCISActionId = Convert.ToInt32(spQueueErrorGet.GetDataReaderValue(2, MyConsole.NullId));
            _USCISClosureId = Convert.ToInt32(spQueueErrorGet.GetDataReaderValue(3, MyConsole.NullId));
            _EmployeeId = Convert.ToInt32(spQueueErrorGet.GetDataReaderValue(4, MyConsole.NullId));
            _I9Id = Convert.ToInt32(spQueueErrorGet.GetDataReaderValue(5, MyConsole.NullId));

            _CaseNumber = Convert.ToString(spQueueErrorGet.GetDataReaderValue(6, string.Empty));
            _Comments = Convert.ToString(spQueueErrorGet.GetDataReaderValue(7, string.Empty));

        }

        #endregion

        #region Properties - Public

        public string CaseNumber { get { return _CaseNumber; } }
        public string Comments { get { return _Comments; } }

        public int USCISQueueErrorId { get { return _USCISQueueErrorId; } }
        public int USCISTransactionId { get { return _USCISTransactionId; } }
        public int USCISActionId { get { return _USCISActionId; } }
        public int USCISClosureId { get { return _USCISClosureId; } }

        public int EmployeeId { get { return _EmployeeId; } }
        public int I9Id { get { return _I9Id; } }

        #endregion

    }

}