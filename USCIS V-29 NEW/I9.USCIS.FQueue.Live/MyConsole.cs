using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

using I9.USCIS.Wrapper;

namespace I9.USCIS.FQueue.Live {

    internal class MyConsole {

        #region Fields

        public static readonly int NullId = -1;

        #endregion

        #region Members

        private MyConfiguration _MyConfig = null;

        #endregion

        #region Constructors

        internal MyConsole() {

            FdblConsole.WriteInitialization(System.Reflection.Assembly.GetCallingAssembly());

            _MyConfig = new MyConfiguration();

            FdblConsole.WriteMessage(string.Format("Console started on physical node {0}", Environment.MachineName));

        }

        #endregion

        #region Methods - Public

        public void Run() {

            FdblConsole.WriteMessage("Checking for other instances of application");
            string app = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (FdblSystem.ApplicationAlreadyExecuting(app)) return;

            _Process(USCISSystemId.Live);

            FdblConsole.WriteMessage("Console is shutting down");

        }

        #endregion

        #region Methods - Private (Processing)

        private void _Process(USCISSystemId idSystem) {

            Wrapper.EmploymentVerification ev = null;

            Sql.spUSCISBE_QueueFuture_Get spQueueFutureGet = null;
            Sql.spUSCISBE_QueueFuture_Begin spQueueFutureBegin = null;
            Sql.spUSCISBE_QueueFuture_Complete spQueueFutureComplete = null;
            Sql.spUSCISBE_QueueFuture_Reset spQueueFutureReset = null;

            try {

                int idWin32 = Process.GetCurrentProcess().Id;

                spQueueFutureGet = new Sql.spUSCISBE_QueueFuture_Get(_MyConfig.SqlFactory.GetConnectionString());
                spQueueFutureBegin = new Sql.spUSCISBE_QueueFuture_Begin(_MyConfig.SqlFactory.GetConnectionString());
                spQueueFutureComplete = new Sql.spUSCISBE_QueueFuture_Complete(_MyConfig.SqlFactory.GetConnectionString());
                spQueueFutureReset = new Sql.spUSCISBE_QueueFuture_Reset(_MyConfig.SqlFactory.GetConnectionString());

                FdblConsole.WriteMessage(string.Format("Fetching records ({0} System)", idSystem));
                int spReturnCode = spQueueFutureGet.StartDataReader(idSystem);

                if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) return;

                FdblConsole.WriteMessage(string.Format("Creating wrapper ({0} System)", idSystem));
                ev = new Wrapper.EmploymentVerification(idSystem);
                
                while (spQueueFutureGet.MoveNextDataReader(true)) {

                    Items.SqlQueueItem sqi = new Items.SqlQueueItem(spQueueFutureGet);

                    FdblConsole.WriteMessage(string.Format("Processing record: {0} (Employee: {1} / I9: {2})", sqi.USCISQueueFutureId, sqi.EmployeeId, sqi.I9Id));

                    spReturnCode = spQueueFutureBegin.StartDataReader(sqi.USCISQueueFutureId, idWin32);

                    if (spReturnCode == FdblSqlReturnCodes.Success) {

                        ev.SetEmployee(sqi.USCISQueueFutureId, sqi.USCISTransactionId, sqi.EmployeeId, sqi.I9Id);
                        ev.AutoProcessCase();

                        spReturnCode = spQueueFutureComplete.StartDataReader(sqi.USCISQueueFutureId, idWin32);

                    } else {

                        spReturnCode = spQueueFutureReset.StartDataReader(sqi.USCISQueueFutureId, idWin32);

                    }

                    Thread.Sleep(25);

                }

            } catch (Exception ex) {

                FdblConsole.WriteMessage("Sending notification (failure)");

                FdblSmtpRecord smtp = _MyConfig.FailureEmail;

                smtp.SetMessage(string.Format("{0}\n\n{1}", smtp.Message, FdblExceptions.GetDetails(ex)));

                try { FdblSmtp.Send(smtp); } catch { }

            } finally {

                if (ev != null) {

                    FdblConsole.WriteMessage(string.Format("Disposing wrapper ({0} System)", idSystem));
                    ev.Dispose();

                }

                if (spQueueFutureReset != null) spQueueFutureReset.Dispose();
                if (spQueueFutureComplete != null) spQueueFutureComplete.Dispose();
                if (spQueueFutureBegin != null) spQueueFutureBegin.Dispose();
                if (spQueueFutureGet != null) spQueueFutureGet.Dispose();

            }

        }

        #endregion

    }

}
