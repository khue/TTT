using System;
using System.Configuration;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Debug {

    internal class MyConfiguration {

        #region Members

        private FdblSqlFactory _SqlFactory = null;

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

        }

        #endregion

        #region Properties - Internal

        internal FdblSqlFactory SqlFactory {
            get { return _SqlFactory; }
        }

        #endregion

    }

}
