using System;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;

namespace I9.USCIS.Wrapper {

    public class USCISFactory {

        #region Constructors

        private USCISFactory() { }

        #endregion

        #region Methods - Public (Static - Request)

        public static WebService.Request_Live GetLiveRequest() { return new WebService.Request_Live(); }
        public static WebService.Request_Live GetLiveRequest(int idEmployee, int idI9) { return new WebService.Request_Live(idEmployee, idI9); }

        public static WebService.Request_Test GetTestRequest() { return new WebService.Request_Test(); }
        public static WebService.Request_Test GetTestRequest(int idEmployee, int idI9) { return new WebService.Request_Test(idEmployee, idI9); }

        #endregion

        #region Methods - Public (Static - Queue)

        public static bool QueueAsError(USCISSystemId idSystem, int idTransaction, USCISMethodId idMethod, int idEmployee, int idI9, string caseNumber, string comments, USCISActionId idAction, USCISClosureId idClosure, bool canBeProcessed) {

            if (idSystem == USCISSystemId.Unknown) throw new ArgumentException("invalid system id");
            if (idTransaction == WebService.Request.NullId) throw new ArgumentException("invalid transaction id");

            if (idEmployee == WebService.Request.NullId) return false;
            if (idI9 == WebService.Request.NullId) return false;

            USCISConfiguration config = null;
            WebService.ProcessIds pids = null;

            try {

                config = new USCISConfiguration(idSystem);

                pids = new WebService.ProcessIds(config.Logs.Capture);

                pids.SystemId = idSystem;
                pids.TransactionId = idTransaction;
                pids.EmployeeId = idEmployee;
                pids.I9Id = idI9;
                pids.CaseNumber = caseNumber;
                pids.CategoryId = USCISCategoryId.Standard;
                pids.MethodId = idMethod;
                pids.MethodName = "QueueAsError";

                Sql.spUSCIS_QueueError_Insert spQueueErrorInsert = new Sql.spUSCIS_QueueError_Insert(config.SqlFactory.GetConnectionString());

                try {

                    int spReturnCode = spQueueErrorInsert.StartDataReader(pids, idAction, idClosure, comments, canBeProcessed);

                    if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_QueueError_Insert returned: {0} (Sql returned: {1})", spReturnCode, spQueueErrorInsert.SqlErrorCode));

                } finally {

                    if (spQueueErrorInsert != null) spQueueErrorInsert.Dispose();

                }

                return true;

            } catch (Exception ex) {

                if (config != null) {

                    Xml.LogWriter.WriteEntry(pids, LogLevel.error, "USCISFactory::QueueAsError", null, ex.Message, ex, null, config.Logs.Capture);
                    Xml.ProcessWriter.WriteEntry(pids.TransactionId, WebService.Request.GetRequestXmlLog(pids, null), WebService.Request.GetResponseXmlLog(null), config.Logs.Process);

                }

                return false;

            }

        }

        public static bool QueueAsFuture(USCISSystemId idSystem, int idTransaction, int idEmployee, int idI9) {

            if (idSystem == USCISSystemId.Unknown) throw new ArgumentException("invalid system id");
            if (idTransaction == WebService.Request.NullId) throw new ArgumentException("invalid transaction id");

            if (idEmployee == WebService.Request.NullId) return false;
            if (idI9 == WebService.Request.NullId) return false;

            USCISConfiguration config = null;
            WebService.ProcessIds pids = null;

            try {

                config = new USCISConfiguration(idSystem);

                pids = new WebService.ProcessIds(config.Logs.Capture);

                pids.SystemId = idSystem;
                pids.TransactionId = idTransaction;
                pids.EmployeeId = idEmployee;
                pids.I9Id = idI9;
                pids.MethodName = "QueueAsFuture";

                Sql.spUSCIS_QueueFuture_Insert spQueueFutureInsert = new Sql.spUSCIS_QueueFuture_Insert(config.SqlFactory.GetConnectionString());

                try {

                    int spReturnCode = spQueueFutureInsert.StartDataReader(pids);

                    if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_QueueFuture_Insert returned: {0} (Sql returned: {1})", spReturnCode, spQueueFutureInsert.SqlErrorCode));

                } finally {

                    if (spQueueFutureInsert != null) spQueueFutureInsert.Dispose();

                }

                return true;

            } catch (Exception ex) {

                if (config != null) {

                    Xml.LogWriter.WriteEntry(pids, LogLevel.error, "USCISFactory::QueueAsFuture", null, ex.Message, ex, null, config.Logs.Capture);
                    Xml.ProcessWriter.WriteEntry(pids.TransactionId, WebService.Request.GetRequestXmlLog(pids, null), WebService.Request.GetResponseXmlLog(null), config.Logs.Process);

                }

                return false;

            }

        }

        #endregion

    }

}