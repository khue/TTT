using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Monitor {

    public partial class MyService : ServiceBase {

        #region Fields

        public static readonly int NullId = -1;

        #endregion

        #region Members

        private MyConfiguration _Config = null;

        private Thread _Thread;

        #endregion

        #region Constructors

        public MyService() {

            InitializeComponent();

        }

        #endregion

        #region Events - Protected

        protected override void OnStart(string[] args) {

            try {

                _ServiceLog.WriteEntry("Initializing Service", EventLogEntryType.Information, 1000);

                _Config = new MyConfiguration();

                _ServiceLog.WriteEntry(string.Format("Starting Service\nPolling Interval: {0} seconds", _Config.Monitor.Interval), EventLogEntryType.Information, 1000);

                _ServiceTimer.Interval = (_Config.Monitor.Interval * 1000);
                _InitTimer.Enabled = true;

            } catch (Exception ex) {

                _ServiceLog.WriteEntry(FdblExceptions.GetDetails(ex), EventLogEntryType.Error, 1000);

            }

        }

        protected override void OnStop() {

            try {

                _ServiceLog.WriteEntry("Stopping Service", EventLogEntryType.Information, 2000);

                if (_Thread != null) {

                    if (_Thread.ThreadState == System.Threading.ThreadState.Running) {

                        _ServiceLog.WriteEntry("Waiting for manager thread to stop", EventLogEntryType.Information, 2000);
                        _Thread.Join();

                    }

                }

                _ServiceLog.WriteEntry("Service has been stopped", EventLogEntryType.Information, 2000);

            } catch (Exception ex) {

                _ServiceLog.WriteEntry(FdblExceptions.GetDetails(ex), EventLogEntryType.Error, 2000);

            }

        }

        #endregion

        #region Events - Private

        private void _InitTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {

            _InitTimer.Enabled = false;
            _InitTimer.Dispose();

            _ServiceTimer_Elapsed(null, null);

        }

        private void _ServiceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {

            _ServiceTimer.Enabled = false;

            _Thread = new Thread(new ThreadStart(DoRequestManager));
            _Thread.IsBackground = true;
            _Thread.Start();

        }

        #endregion

        #region Methods - Private

        private void DoRequestManager() {

            try {

                if (_ServiceLog.Entries.Count > _Config.Monitor.LogEntries) {

                    _ServiceLog.Clear();
                    _ServiceLog.WriteEntry("Purged log", EventLogEntryType.Information, 3100);

                }

                if (FdblSystem.ApplicationExceeds(_Config.Console.Name, _Config.Console.Allowed)) {

                    _ServiceLog.WriteEntry("Maximum allowed containers currently processing", EventLogEntryType.Information, 4000);
                    return;

                }

                Sql.spUSCISBE_Monitor_Get spMonitorGet = null;

                try {

                    spMonitorGet = new Sql.spUSCISBE_Monitor_Get(_Config.SqlFactory.GetConnectionString());

                    int spReturnCode = spMonitorGet.StartDataReader();

                    if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) return;

                    while (spMonitorGet.MoveNextDataReader(true)) {

                        if (FdblSystem.ApplicationExceeds(_Config.Console.Name, _Config.Console.Allowed)) {

                            _ServiceLog.WriteEntry("Maximum allowed consoles currently processing", EventLogEntryType.Information, 4000);
                            break;

                        }

                        int idConsole = Convert.ToInt32(spMonitorGet.GetDataReaderValue(0, -1));

                        if (idConsole != -1) {

                            _ServiceLog.WriteEntry(string.Format("Starting console for id: {0}", idConsole), EventLogEntryType.Information, 5000);

                            Process p = new Process();

                            p.StartInfo.WorkingDirectory = _Config.Console.Directory;
                            p.StartInfo.FileName = _Config.Console.Name;
                            p.StartInfo.Arguments = string.Format("/ConsoleId:{0}", idConsole);
                            p.StartInfo.CreateNoWindow = true;
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                            p.Start();

                            Thread.Sleep(25);

                        }

                    }

                } finally {

                    if (spMonitorGet != null) spMonitorGet.Dispose();

                }

            } catch (Exception ex) {

                _ServiceLog.WriteEntry(FdblExceptions.GetDetails(ex), EventLogEntryType.Error, 3000);

                FdblSmtpRecord smtp = new FdblSmtpRecord();

                smtp.SmtpServer = Program.SmtpServer;
                smtp.SendFrom = Program.SendFrom;
                smtp.SendTo = Program.SendTo;
                smtp.Subject = Program.Subject;
                smtp.Message = string.Format("{0}\n\n{1}", Program.Message, FdblExceptions.GetDetails(ex));

                try { FdblSmtp.Send(smtp); } catch { }

            } finally {

                System.GC.Collect();

                _ServiceTimer.Enabled = true;

            }

        }

        #endregion

    }

}