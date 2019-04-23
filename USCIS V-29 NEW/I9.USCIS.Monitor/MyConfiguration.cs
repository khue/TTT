using System;
using System.Configuration;
using System.IO;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Monitor {
    
    internal class MyConfiguration {

        #region Structures

        internal struct CfgMonitor {

            internal int Interval;
            internal int LogEntries;

        }

        internal struct CfgConsole {

            internal string Directory;
            internal string Name;
            internal int Allowed;

        }

        #endregion

        #region Members

        private CfgMonitor _Monitor;
        private CfgConsole _Console;

        private FdblSqlFactory _SqlFactory = null;
        private FdblSmtpRecord _FailureEmail = null;

        #endregion

        #region Constructors

        internal MyConfiguration() {

            _Monitor = new CfgMonitor();

            string tmpItem = FdblStrings.Trim(ConfigurationSettings.AppSettings["Monitor.Interval"]);
            if (string.IsNullOrEmpty(tmpItem)) throw new MyException("Invalid application configuration: missing Monitor.Interval value");
            if (!int.TryParse(tmpItem, out _Monitor.Interval)) throw new MyException("Invalid application configuration: invalid Monitor.Interval value");

            tmpItem = FdblStrings.Trim(ConfigurationSettings.AppSettings["Monitor.LogEntries"]);
            if (string.IsNullOrEmpty(tmpItem)) throw new MyException("Invalid application configuration: missing Monitor.LogEntries value");
            if (!int.TryParse(tmpItem, out _Monitor.LogEntries)) throw new MyException("Invalid application configuration: invalid Monitor.LogEntries value");

            _Console = new CfgConsole();

            _Console.Directory = FdblStrings.Trim(ConfigurationSettings.AppSettings["Console.Directory"]);
            _Console.Name = FdblStrings.Trim(ConfigurationSettings.AppSettings["Console.Name"]);

            if (string.IsNullOrEmpty(_Console.Directory)) throw new MyException("Invalid application configuration: missing Console.Directory value");
            if (string.IsNullOrEmpty(_Console.Name)) throw new MyException("Invalid application configuration: missing Console.Name value");

            tmpItem = FdblStrings.Trim(ConfigurationSettings.AppSettings["Console.Allowed"]);
            if (string.IsNullOrEmpty(tmpItem)) throw new MyException("Invalid application configuration: missing Console.Allowed value");
            if (!int.TryParse(tmpItem, out _Console.Allowed)) throw new MyException("Invalid application configuration: invalid Console.Allowed value");

            if (_Console.Allowed < 1 || _Console.Allowed > 50) _Console.Allowed = 5;

            _SqlFactory = new FdblSqlFactory();
            _SqlFactory.Server = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Server"]);
            _SqlFactory.Database = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Database"]);
            _SqlFactory.UserAccount = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.UserAccount"]);
            _SqlFactory.AccountPassword = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.AccountPassword"]);

            if (string.IsNullOrEmpty(_SqlFactory.Server)) throw new MyException("Invalid application configuration: missing Sql.Server value");
            if (string.IsNullOrEmpty(_SqlFactory.Database)) throw new MyException("Invalid application configuration: missing Sql.Database value");
            if (string.IsNullOrEmpty(_SqlFactory.UserAccount)) throw new MyException("Invalid application configuration: missing Sql.UserAccount value");

            string smtp = FdblStrings.Trim(ConfigurationSettings.AppSettings["Smtp.Server"]);

            if (string.IsNullOrEmpty(smtp)) throw new MyException("Invalid application configuration: missing Smtp.Server value");

            _FailureEmail = new FdblSmtpRecord();
            _FailureEmail.SmtpServer = smtp;
            _FailureEmail.SendFrom = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.From"]);
            _FailureEmail.SendTo = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.SendTo"]);
            _FailureEmail.Subject = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.Subject"]);
            _FailureEmail.Message = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.Message"]);

            if (string.IsNullOrEmpty(_FailureEmail.SendFrom)) throw new MyException("Invalid application configuration: missing Failure.Email.From value");
            if (string.IsNullOrEmpty(_FailureEmail.SendTo)) throw new MyException("Invalid application configuration: missing Failure.Email.SendTo value");
            if (string.IsNullOrEmpty(_FailureEmail.Subject)) throw new MyException("Invalid application configuration: missing Failure.Email.Subject value");
            if (string.IsNullOrEmpty(_FailureEmail.Message)) throw new MyException("Invalid application configuration: missing Failure.Email.Message value");

        }

        #endregion

        #region Properties - Public

        internal CfgMonitor Monitor {
            get { return _Monitor; }
        }

        internal CfgConsole Console {
            get { return _Console; }
        }

        internal FdblSqlFactory SqlFactory {
            get { return _SqlFactory; }
        }

        internal FdblSmtpRecord FailureEmail {
            get { return _FailureEmail; }
        }

        #endregion

    }
}
