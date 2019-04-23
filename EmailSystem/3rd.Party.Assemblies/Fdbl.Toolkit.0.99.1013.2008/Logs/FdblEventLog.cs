using System;
using System.Diagnostics;

namespace Fdbl.Toolkit.Logs {

    #region Enumerations

    [FlagsAttribute]
    public enum FdblEventLogLevel : byte {
        Error,
        FailureAudit,
        Information,
        Warning,
        SuccessAudit
    }

    #endregion

    public class FdblEventLog {

        #region Members

        private string _LogName;
        private string _LogSource;
        private string _MachineName;

        #endregion

        #region Methods - Public (Static)

        public static void CreateLog(string logName, string logSource) {

            if (logName == null) throw new ArgumentNullException("log name is null");
            if (logSource == null) throw new ArgumentNullException("log source is null");

            if (logName.Trim().Length == 0) throw new ArgumentException("log name is blank");
            if (logSource.Trim().Length == 0) throw new ArgumentException("log source is blank");

            if (!EventLog.Exists(logName)) {
                if (EventLog.SourceExists(logSource)) EventLog.DeleteEventSource(logSource);
                EventLog.CreateEventSource(logSource, logName);
            }

        }

        public static void CreateLog(string logName, string logSource, string machineName) {

            if (logName == null) throw new ArgumentNullException("log name is null");
            if (logSource == null) throw new ArgumentNullException("log source is null");
            if (machineName == null) throw new ArgumentNullException("machine name is null");

            if (logName.Trim().Length == 0) throw new ArgumentException("log name is blank");
            if (logSource.Trim().Length == 0) throw new ArgumentException("log source is blank");
            if (machineName.Trim().Length == 0) throw new ArgumentException("machine name is blank");

            if (!EventLog.Exists(logName, machineName)) {
                if (EventLog.SourceExists(logSource, machineName)) EventLog.DeleteEventSource(logSource, machineName);
                EventLog.CreateEventSource(new EventSourceCreationData(logSource, logName));
            }

        }

        public static void DeleteLog(string logName, string logSource) {

            if (logName == null) throw new ArgumentNullException("log name is null");
            if (logSource == null) throw new ArgumentNullException("log source is null");

            if (logName.Trim().Length == 0) throw new ArgumentException("log name is blank");
            if (logSource.Trim().Length == 0) throw new ArgumentException("log source is blank");

            if (EventLog.Exists(logName)) {
                EventLog.Delete(logName);
                if (EventLog.SourceExists(logSource)) EventLog.DeleteEventSource(logSource);
            }

        }

        public static bool DoesLogExist(string logName) {

            if (logName == null) throw new ArgumentNullException("log name is null");

            if (logName.Trim().Length == 0) throw new ArgumentException("log name is blank");

            return EventLog.Exists(logName);

        }

        public static bool DoesLogExist(string logName, string machineName) {

            if (logName == null) throw new ArgumentNullException("log name is null");
            if (machineName == null) throw new ArgumentNullException("machine name is null");

            if (logName.Trim().Length == 0) throw new ArgumentException("log name is blank");
            if (machineName.Trim().Length == 0) throw new ArgumentException("machine name is blank");

            return EventLog.Exists(logName, machineName);

        }

        public static bool DoesSourceExist(string logSource) {

            if (logSource == null) throw new ArgumentNullException("log source is null");

            if (logSource.Trim().Length == 0) throw new ArgumentException("log source is blank");

            return EventLog.SourceExists(logSource);

        }

        public static bool DoesSourceExist(string logSource, string machineName) {

            if (logSource == null) throw new ArgumentNullException("log source is null");
            if (machineName == null) throw new ArgumentNullException("machine name is null");

            if (logSource.Trim().Length == 0) throw new ArgumentException("log source is blank");
            if (machineName.Trim().Length == 0) throw new ArgumentException("machine name is blank");

            return EventLog.SourceExists(logSource, machineName);

        }

        #endregion

        #region Methods - Public

        public void Clear() {
            EventLog ev = _GetLog();
            try {
                ev.Clear();
            } finally {
                ev.Close();
            }
        }

        public void Write(string Message) {
            EventLog ev = _GetLog();
            try {
                ev.WriteEntry(Message);
            } finally {
                ev.Close();
            }
        }

        public void Write(string Message, FdblEventLogLevel MessageType) {
            EventLog ev = _GetLog();
            try {
                ev.WriteEntry(Message, (EventLogEntryType)MessageType);
            } finally {
                ev.Close();
            }
        }

        public void Write(string Message, FdblEventLogLevel MessageType, int EventId) {
            EventLog ev = _GetLog();
            try {
                ev.WriteEntry(Message, (EventLogEntryType)MessageType, EventId);
            } finally {
                ev.Close();
            }
        }

        public void Write(string Message, FdblEventLogLevel MessageType, int EventId, short Category) {
            EventLog ev = _GetLog();
            try {
                ev.WriteEntry(Message, (EventLogEntryType)MessageType, EventId, Category);
            } finally {
                ev.Close();
            }
        }

        #endregion

        #region Constructors

        public FdblEventLog() : this("Application", System.Environment.MachineName, null) { }

        public FdblEventLog(string logName) : this(logName, System.Environment.MachineName, null) { }

        public FdblEventLog(string logName, string machineName) : this(logName, machineName, null) { }

        public FdblEventLog(string logName, string machineName, string logSource) {
            _LogName = logName;
            _MachineName = machineName;
            _LogSource = logSource;
        }

        #endregion

        #region Methods - Private

        private EventLog _GetLog() {
            if (_LogSource != null && _LogSource.Trim().Length != 0) return new EventLog(_LogName, _MachineName, _LogSource);
            if (_MachineName != null && _MachineName.Trim().Length != 0) return new EventLog(_LogName, _MachineName);
            return new EventLog(_LogName);
        }

        #endregion

    }

}