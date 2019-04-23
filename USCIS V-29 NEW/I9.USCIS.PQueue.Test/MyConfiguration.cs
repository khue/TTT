using System;
using System.Configuration;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.PQueue.Test {

    internal class MyConfiguration {

        #region Structures

        internal struct CfgFiles {
            internal string Password;
            internal string Config;
        }

        #endregion

        #region Members

        private CfgFiles _Files;
        private FdblSmtpRecord _SuccessEmail;
        private FdblSmtpRecord _FailureEmail;

        #endregion

        #region Constructors

        public MyConfiguration() {

            _Files = new CfgFiles();

            _Files.Password = FdblStrings.Trim(ConfigurationSettings.AppSettings["XmlFile.Password"]);
            _Files.Config = FdblStrings.Trim(ConfigurationSettings.AppSettings["XmlFile.USCIS"]);

            if (string.IsNullOrEmpty(_Files.Password)) throw new MyException("Invalid application configuration: missing the XmlFile.Password value");
            if (string.IsNullOrEmpty(_Files.Config)) throw new MyException("Invalid application configuration: missing the XmlFile.USCIS value");

            string smtp = FdblStrings.Trim(ConfigurationSettings.AppSettings["Smtp.Server"]);

            if (string.IsNullOrEmpty(smtp)) throw new MyException("Invalid application configuration: missing Smtp.Server value");

            _SuccessEmail = new FdblSmtpRecord();

            _SuccessEmail.SmtpServer = smtp;
            _SuccessEmail.SendFrom = FdblStrings.Trim(ConfigurationSettings.AppSettings["Success.Email.From"]);
            _SuccessEmail.SendTo = FdblStrings.Trim(ConfigurationSettings.AppSettings["Success.Email.SendTo"]);
            _SuccessEmail.Subject = FdblStrings.Trim(ConfigurationSettings.AppSettings["Success.Email.Subject"]);
            _SuccessEmail.Message = FdblStrings.Trim(ConfigurationSettings.AppSettings["Success.Email.Message"]);

            if (string.IsNullOrEmpty(_SuccessEmail.SendFrom)) throw new MyException("Invalid application configuration: missing Success.Email.From value");
            if (string.IsNullOrEmpty(_SuccessEmail.SendTo)) throw new MyException("Invalid application configuration: missing Success.Email.SendTo value");
            if (string.IsNullOrEmpty(_SuccessEmail.Subject)) throw new MyException("Invalid application configuration: missing Success.Email.Subject value");
            if (string.IsNullOrEmpty(_SuccessEmail.Message)) throw new MyException("Invalid application configuration: missing Success.Email.Message value");

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

        internal CfgFiles Files { get { return _Files; } }

        internal FdblSmtpRecord SuccessEmail { get { return _SuccessEmail; } }
        internal FdblSmtpRecord FailureEmail { get { return _FailureEmail; } }

        #endregion

    }

}