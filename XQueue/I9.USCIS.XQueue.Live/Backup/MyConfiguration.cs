using System;
using System.Configuration;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.XQueue.Live {

    internal class MyConfiguration {

        #region Members

        private FdblSqlFactory _SqlFactory = null;
        private FdblSmtpRecord _FailureEmail;

        private string _LogFile = null;

        #endregion

        #region Constructors

        public MyConfiguration() {

            _LogFile = FdblStrings.Trim(ConfigurationSettings.AppSettings["Log.File"]);

            if (string.IsNullOrEmpty(_LogFile)) throw new MyException("Invalid application configuration: missing Log.File value");

            _SqlFactory = new FdblSqlFactory();

            _SqlFactory.Server = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Server"]);
            _SqlFactory.Database = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Database"]);
            _SqlFactory.UserAccount = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Account"]);
            _SqlFactory.AccountPassword = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Password"]);

            if (string.IsNullOrEmpty(_SqlFactory.Server)) throw new MyException("Invalid application configuration: missing Sql.Server value");
            if (string.IsNullOrEmpty(_SqlFactory.Database)) throw new MyException("Invalid application configuration: missing Sql.Database value");
            if (string.IsNullOrEmpty(_SqlFactory.UserAccount)) throw new MyException("Invalid application configuration: missing Sql.Account value");

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

        internal FdblSqlFactory SqlFactory { get { return _SqlFactory; } }
        internal FdblSmtpRecord FailureEmail { get { return _FailureEmail; } }
        internal string LogFile { get { return _LogFile; } }

        #endregion

    }

}