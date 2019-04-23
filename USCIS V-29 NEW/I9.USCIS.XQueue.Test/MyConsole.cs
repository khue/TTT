using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Utils;

using I9.USCIS.Wrapper;

namespace I9.USCIS.XQueue.Test {

    internal class MyConsole {

        #region Fields

        public static readonly int NullId = -1;
        public static readonly int BypassId = -99;

        #endregion

        #region Members

        private MyConfiguration _Config;

        #endregion

        #region Constructors

        internal MyConsole() {

            FdblConsole.WriteInitialization(System.Reflection.Assembly.GetCallingAssembly());

            _Config = new MyConfiguration();

            FdblConsole.WriteMessage(string.Format("Console started on physical node {0}", Environment.MachineName));

        }

        #endregion

        #region Methods - Public

        public void Run() {

            FdblConsole.WriteMessage("Checking for other instances of application");
            string app = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (FdblSystem.ApplicationAlreadyExecuting(app)) return;

            _Process(USCISSystemId.Test);

            FdblConsole.WriteMessage("Console is shutting down");

        }

        #endregion

        #region Methods - Private

        private void _Process(USCISSystemId idSystem) {

            Wrapper.QueueVerification qv = null;

            try {

                FdblConsole.WriteMessage(string.Format("Creating wrapper ({0} System)", idSystem));
                qv = new Wrapper.QueueVerification(idSystem, _Config.SqlFactory.GetConnectionString(), _Config.LogFile);

                for (; ; ) {

                    FdblConsole.WriteMessage("Fetch next case");

                    if (!qv.ProcessNextCase()) break;
                    
                    Thread.Sleep(25);

                }

            } catch (Exception ex) {

                FdblConsole.WriteMessage("Sending notification (failure)");

                _Config.FailureEmail.Message = string.Format("{0}\n\n{1}", _Config.FailureEmail.Message, FdblExceptions.GetDetails(ex));

                try { FdblSmtp.Send(_Config.FailureEmail); } catch { }

                _Config.FailureEmail.ResetMessage();

            } finally {

                if (qv != null) {

                    FdblConsole.WriteMessage(string.Format("Disposing wrapper ({0} System)", idSystem));
                    qv.Dispose();

                }

            }

        }

        #endregion

    }

}