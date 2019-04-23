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

namespace I9.USCIS.EQueue.Live {

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

            Sql.spUSCISBE_QueueError_Get spQueueErrorGet = null;
            Sql.spUSCISBE_QueueError_Begin spQueueErrorBegin = null;
            Sql.spUSCISBE_QueueError_Complete spQueueErrorComplete = null;
            Sql.spUSCISBE_QueueError_Reset spQueueErrorReset = null;

            try {

                int idWin32 = Process.GetCurrentProcess().Id;

                spQueueErrorGet = new Sql.spUSCISBE_QueueError_Get(_MyConfig.SqlFactory.GetConnectionString());
                spQueueErrorBegin = new Sql.spUSCISBE_QueueError_Begin(_MyConfig.SqlFactory.GetConnectionString());
                spQueueErrorComplete = new Sql.spUSCISBE_QueueError_Complete(_MyConfig.SqlFactory.GetConnectionString());
                spQueueErrorReset = new Sql.spUSCISBE_QueueError_Reset(_MyConfig.SqlFactory.GetConnectionString());

                FdblConsole.WriteMessage(string.Format("Fetching records ({0} System)", idSystem));
                int spReturnCode = spQueueErrorGet.StartDataReader(idSystem);

                if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) return;

                FdblConsole.WriteMessage(string.Format("Creating wrapper ({0} System)", idSystem));
                ev = new Wrapper.EmploymentVerification(idSystem);

                while (spQueueErrorGet.MoveNextDataReader(true)) {

                    Items.SqlQueueItem sqi = new Items.SqlQueueItem(spQueueErrorGet);

                    FdblConsole.WriteMessage(string.Format("Processing record: {0} (Employee: {1} / I9: {2})", sqi.USCISQueueErrorId, sqi.EmployeeId, sqi.I9Id));

                    spReturnCode = spQueueErrorBegin.StartDataReader(sqi.USCISQueueErrorId, idWin32);

                    if (spReturnCode == FdblSqlReturnCodes.Success) {

                        ev.SetEmployee(sqi.USCISQueueErrorId, sqi.USCISTransactionId, sqi.EmployeeId, sqi.I9Id);

                        if (sqi.USCISActionId == (int)USCISActionId.AutoProcessInitiateCase) ev.AutoProcessInitiateCase();
                        else if (sqi.USCISActionId == (int)USCISActionId.AutoProcessSSAVerification) ev.AutoProcessSSAVerification(sqi.CaseNumber);
                        else if (sqi.USCISActionId == (int)USCISActionId.InitiateCase) ev.InitiateCase();
                        else if (sqi.USCISActionId == (int)USCISActionId.SubmitSSAReferral) ev.SubmitSSAReferral(sqi.CaseNumber);
                        else if (sqi.USCISActionId == (int)USCISActionId.ResubmitSSAVerification) ev.ResubmitSSAVerification(sqi.CaseNumber);
                        else if (sqi.USCISActionId == (int)USCISActionId.SubmitSecondVerification) ev.SubmitSecondVerification(sqi.CaseNumber, sqi.Comments);
                        else if (sqi.USCISActionId == (int)USCISActionId.SubmitDHSReferral) ev.SubmitDHSReferral(sqi.CaseNumber);
                        else if (sqi.USCISActionId == (int)USCISActionId.CloseCase) ev.CloseCase(sqi.USCISClosureId, sqi.CaseNumber);

                        spReturnCode = spQueueErrorComplete.StartDataReader(sqi.USCISQueueErrorId, idWin32);

                    } else {

                        spReturnCode = spQueueErrorReset.StartDataReader(sqi.USCISQueueErrorId, idWin32);

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

                if (spQueueErrorReset != null) spQueueErrorReset.Dispose();
                if (spQueueErrorComplete != null) spQueueErrorComplete.Dispose();
                if (spQueueErrorBegin != null) spQueueErrorBegin.Dispose();
                if (spQueueErrorGet != null) spQueueErrorGet.Dispose();

            }

        }

        #endregion

    }

}
