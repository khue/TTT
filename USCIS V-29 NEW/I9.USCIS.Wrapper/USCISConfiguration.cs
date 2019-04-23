using System;
using System.Configuration;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper {

    public class USCISConfiguration {

        #region Structures

        public struct CPSInfo {
            public int Threshold;
            public int CompanyId;
            public string UserId;
            public string UserPassword;
            public string SubmiterName;
            public string SubmiterPhone;
            public int AttemptMaxRetry;
            public int AttemptWaitTime;
        }

        public struct LogInfo {
            public string Capture;
            public string Process;
        }

        #endregion

        #region Members

        private FdblSqlFactory _SqlFactory = null;
        private FdblSqlFactory _SqlFactoryAlt = null;
        private FdblSmtpRecord _FailureEmail = null;

        private CPSInfo _CPSInfo;
        private LogInfo _LogInfo;

        #endregion

        #region Constructors

        internal USCISConfiguration(USCISSystemId idSystem) {

            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = "I9.USCIS.Wrapper.dll.config";

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            AppSettingsSection ass = config.AppSettings;


            _CPSInfo = new CPSInfo();

            string tmpThreshold = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.Threshold", idSystem)].Value);
            string tmpCompany = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.CompanyId", idSystem)].Value);
            _CPSInfo.UserId = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.UserId", idSystem)].Value);
            _CPSInfo.UserPassword = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.UserPassword", idSystem)].Value);
            _CPSInfo.SubmiterName = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.SubmiterName", idSystem)].Value);
            _CPSInfo.SubmiterPhone = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.SubmiterPhone", idSystem)].Value);
            string tmpMaxRetry = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.AttemptMaxRetry", idSystem)].Value);
            string tmpWaitTime = FdblStrings.Trim(ass.Settings[string.Format("{0}.Cps.AttemptWaitTime", idSystem)].Value);

            float pseWaitTime = 0;

            if (string.IsNullOrEmpty(tmpThreshold)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Cps.Threshold value", idSystem));
            if (string.IsNullOrEmpty(tmpCompany)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Cps.CompanyId value", idSystem));
            if (!int.TryParse(tmpThreshold, out _CPSInfo.Threshold)) throw new USCISException(string.Format("Invalid application configuration: invalid {0}.Cps.Threshold value", idSystem));
            if (!int.TryParse(tmpCompany, out _CPSInfo.CompanyId)) throw new USCISException(string.Format("Invalid application configuration: invalid {0}.Cps.CompanyId value", idSystem));
            if (string.IsNullOrEmpty(_CPSInfo.UserId)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Cps.UserId value", idSystem));
            if (string.IsNullOrEmpty(_CPSInfo.UserPassword)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Cps.UserPassword value", idSystem));
            if (string.IsNullOrEmpty(tmpMaxRetry)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Cps.AttemptMaxRetry value", idSystem));
            if (string.IsNullOrEmpty(tmpWaitTime)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Cps.AttemptWaitTime value", idSystem));
            if (!int.TryParse(tmpMaxRetry, out _CPSInfo.AttemptMaxRetry)) throw new USCISException(string.Format("Invalid application configuration: invalid {0}.Cps.AttemptMaxRetry value", idSystem));
            if (!float.TryParse(tmpWaitTime, out pseWaitTime)) throw new USCISException(string.Format("Invalid application configuration: invalid {0}.Cps.AttemptWaitTime value", idSystem));

           _CPSInfo.AttemptWaitTime = (int)(pseWaitTime * 1000);

            _LogInfo = new LogInfo();

            _LogInfo.Capture = FdblStrings.Trim(ass.Settings[string.Format("{0}.Logs.Capture", idSystem)].Value);
            _LogInfo.Process = FdblStrings.Trim(ass.Settings[string.Format("{0}.Logs.Process", idSystem)].Value);

            if (string.IsNullOrEmpty(_LogInfo.Capture)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Logs.Capture value", idSystem));
            if (string.IsNullOrEmpty(_LogInfo.Process)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Logs.Process value", idSystem));

            _SqlFactory = new FdblSqlFactory();
            _SqlFactoryAlt = new FdblSqlFactory();

            _SqlFactory.Server = FdblStrings.Trim(ass.Settings[string.Format("{0}.Sql.Server", idSystem)].Value);
            _SqlFactory.Database = FdblStrings.Trim(ass.Settings[string.Format("{0}.Sql.Database", idSystem)].Value);
            _SqlFactory.UserAccount = FdblStrings.Trim(ass.Settings[string.Format("{0}.Sql.Account", idSystem)].Value);
            _SqlFactory.AccountPassword = FdblStrings.Trim(ass.Settings[string.Format("{0}.Sql.Password", idSystem)].Value);
            _SqlFactoryAlt.Server = FdblStrings.Trim(ass.Settings[string.Format("{0}.AltSql.Server", idSystem)].Value);
            _SqlFactoryAlt.Database = FdblStrings.Trim(ass.Settings[string.Format("{0}.AltSql.Database", idSystem)].Value);
            _SqlFactoryAlt.UserAccount = FdblStrings.Trim(ass.Settings[string.Format("{0}.AltSql.Account", idSystem)].Value);
            _SqlFactoryAlt.AccountPassword = FdblStrings.Trim(ass.Settings[string.Format("{0}.AltSql.Password", idSystem)].Value);

            if (string.IsNullOrEmpty(_SqlFactory.Server)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Sql.Server value", idSystem));
            if (string.IsNullOrEmpty(_SqlFactory.Database)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Sql.Database value", idSystem));
            if (string.IsNullOrEmpty(_SqlFactory.UserAccount)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.Sql.Account value", idSystem));
            if (string.IsNullOrEmpty(_SqlFactoryAlt.Server)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.AltSql.Server value", idSystem));
            if (string.IsNullOrEmpty(_SqlFactoryAlt.Database)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.AltSql.Database value", idSystem));
            if (string.IsNullOrEmpty(_SqlFactoryAlt.UserAccount)) throw new USCISException(string.Format("Invalid application configuration: missing {0}.AltSql.Account value", idSystem));

            string smtp = FdblStrings.Trim(ass.Settings["Smtp.Server"].Value);

            if (string.IsNullOrEmpty(smtp)) throw new USCISException("Invalid application configuration: missing Smtp.Server value");

            _FailureEmail = new FdblSmtpRecord();

            _FailureEmail.SmtpServer = smtp;
            _FailureEmail.SendFrom = FdblStrings.Trim(ass.Settings["Failure.Email.From"].Value);
            _FailureEmail.SendTo = FdblStrings.Trim(ass.Settings["Failure.Email.SendTo"].Value);
            _FailureEmail.Subject = FdblStrings.Trim(ass.Settings["Failure.Email.Subject"].Value);
            _FailureEmail.Message = FdblStrings.Trim(ass.Settings["Failure.Email.Message"].Value);

            if (FdblStrings.IsBlank(_FailureEmail.SendFrom)) throw new USCISException("Invalid application configuration: missing Failure.Email.From value");
            if (FdblStrings.IsBlank(_FailureEmail.SendTo)) throw new USCISException("Invalid application configuration: missing Failure.Email.SendTo value");
            if (FdblStrings.IsBlank(_FailureEmail.Subject)) throw new USCISException("Invalid application configuration: missing Failure.Email.Subject value");
            if (FdblStrings.IsBlank(_FailureEmail.Message)) throw new USCISException("Invalid application configuration: missing Failure.Email.Message value");

        }

        #endregion

        #region Properties - Public

        public CPSInfo CPS { get { return _CPSInfo; } }
        public LogInfo Logs { get { return _LogInfo; } }
        public FdblSqlFactory SqlFactory { get { return _SqlFactory; } }
        public FdblSqlFactory SqlFactoryAlt { get { return _SqlFactoryAlt; } }
        public FdblSmtpRecord FailureEmail { get { return _FailureEmail; } }

        #endregion

    }

}