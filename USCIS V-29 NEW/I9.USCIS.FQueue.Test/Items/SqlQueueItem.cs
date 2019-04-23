using System;
using System.Collections.Generic;
using System.Text;

namespace I9.USCIS.FQueue.Test.Items {
    
    internal class SqlQueueItem {

        #region Members

        int _USCISQueueFutureId = MyConsole.NullId;
        int _USCISTransactionId = MyConsole.NullId;
        int _EmployeeId = MyConsole.NullId;
        int _I9Id = MyConsole.NullId;

        #endregion

        #region Constructors

        public SqlQueueItem(Sql.spUSCISBE_QueueFuture_Get spQueueFutureGet) {

            if (spQueueFutureGet == null) throw new MyException("spUSCIS_QueueFuture_Get is null");

            _USCISQueueFutureId = Convert.ToInt32(spQueueFutureGet.GetDataReaderValue(0, MyConsole.NullId));
            _USCISTransactionId = Convert.ToInt32(spQueueFutureGet.GetDataReaderValue(1, MyConsole.NullId));
            _EmployeeId = Convert.ToInt32(spQueueFutureGet.GetDataReaderValue(2, MyConsole.NullId));
            _I9Id = Convert.ToInt32(spQueueFutureGet.GetDataReaderValue(3, MyConsole.NullId));

        }

        #endregion

        #region Properties - Public

        public int USCISQueueFutureId { get { return _USCISQueueFutureId; } }
        public int USCISTransactionId { get { return _USCISTransactionId; } }
        public int EmployeeId { get { return _EmployeeId; } }
        public int I9Id { get { return _I9Id; } }

        #endregion

    }
}
