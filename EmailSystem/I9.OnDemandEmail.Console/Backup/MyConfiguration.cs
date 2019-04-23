using System;
using System.Configuration;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console {

    internal class MyConfiguration {

        #region Members

        private FdblSqlFactory _SqlFactory = null;
        private FdblSmtpRecord _FailureEmail = null;
        private FdblSmtpRecord _IssueEmail = null;

        #endregion

        #region Constructors

        internal MyConfiguration() {

            _SqlFactory = new FdblSqlFactory();
            _SqlFactory.Server = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Server"]);
            _SqlFactory.Database = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Database"]);
            _SqlFactory.UserAccount = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Account"]);
            _SqlFactory.AccountPassword = FdblStrings.Trim(ConfigurationSettings.AppSettings["Sql.Password"]);

            if (FdblStrings.IsBlank(_SqlFactory.Server)) throw new MyException("Invalid application configuration: missing Sql.Server value");
            if (FdblStrings.IsBlank(_SqlFactory.Database)) throw new MyException("Invalid application configuration: missing Sql.Database value");
            if (FdblStrings.IsBlank(_SqlFactory.UserAccount)) throw new MyException("Invalid application configuration: missing Sql.Account value");

            string smtp = FdblStrings.Trim(ConfigurationSettings.AppSettings["Smtp.Server"]);

            if (FdblStrings.IsBlank(smtp)) throw new MyException("Invalid application configuration: missing Smtp.Server value");

            _FailureEmail = new FdblSmtpRecord();
            _FailureEmail.SmtpServer = smtp;
            _FailureEmail.SendFrom = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.From"]);
            _FailureEmail.SendTo = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.SendTo"]);
            _FailureEmail.Subject = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.Subject"]);
            _FailureEmail.Message = FdblStrings.Trim(ConfigurationSettings.AppSettings["Failure.Email.Message"]);

            if (FdblStrings.IsBlank(_FailureEmail.SendFrom)) throw new MyException("Invalid application configuration: missing Failure.Email.From value");
            if (FdblStrings.IsBlank(_FailureEmail.SendTo)) throw new MyException("Invalid application configuration: missing Failure.Email.SendTo value");
            if (FdblStrings.IsBlank(_FailureEmail.Subject)) throw new MyException("Invalid application configuration: missing Failure.Email.Subject value");
            if (FdblStrings.IsBlank(_FailureEmail.Message)) throw new MyException("Invalid application configuration: missing Failure.Email.Message value");

            _IssueEmail = new FdblSmtpRecord();
            _IssueEmail.SmtpServer = smtp;
            _IssueEmail.SendFrom = FdblStrings.Trim(ConfigurationSettings.AppSettings["Issue.Email.From"]);
            _IssueEmail.SendTo = FdblStrings.Trim(ConfigurationSettings.AppSettings["Issue.Email.SendTo"]);
            _IssueEmail.Subject = FdblStrings.Trim(ConfigurationSettings.AppSettings["Issue.Email.Subject"]);
            _IssueEmail.Message = FdblStrings.Trim(ConfigurationSettings.AppSettings["Issue.Email.Message"]);

            if (FdblStrings.IsBlank(_IssueEmail.SendFrom)) throw new MyException("Invalid application configuration: missing Issue.Email.From value");
            if (FdblStrings.IsBlank(_IssueEmail.SendTo)) throw new MyException("Invalid application configuration: missing Issue.Email.SendTo value");
            if (FdblStrings.IsBlank(_IssueEmail.Subject)) throw new MyException("Invalid application configuration: missing Issue.Email.Subject value");
            if (FdblStrings.IsBlank(_IssueEmail.Message)) throw new MyException("Invalid application configuration: missing Issue.Email.Message value");

        }

        #endregion

        #region Properties - Internal

        internal FdblSmtpRecord FailureEmail { get { return _FailureEmail; } }
        internal FdblSmtpRecord IssueEmail { get { return _IssueEmail; } }

        internal FdblSqlFactory SqlFactory { get { return _SqlFactory; } }

        #endregion

    }

}
