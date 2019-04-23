using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;
using Fdbl.Toolkit.Xml;

using I9.USCIS.Wrapper;

namespace I9.USCIS.PQueue.Test {

    internal class MyConsole {

        #region Fields

        public static readonly int NullId = -1;

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

            _Process(USCISSystemId.Test);

            FdblConsole.WriteMessage("Console is shutting down");

        }

        #endregion

        #region Methods - Private

        private void _Process(USCISSystemId idSystem) {

            Wrapper.PasswordVerification pv = null;

            try {

                FdblConsole.WriteMessage(string.Format("Creating wrapper ({0} System)", idSystem));
                pv = new Wrapper.PasswordVerification(idSystem);

                FdblConsole.WriteMessage("  Getting monthly password");
                string pwd = _GetMonthlyPassword(idSystem);

                FdblConsole.WriteMessage("  Updating password");
                if (!pv.UpdatePassword(pwd)) throw new MyException(string.Format("Could not update the {0} USCIS account password", idSystem));

                FdblConsole.WriteMessage("  Updating wrapper configuration file");
                _UpdateWrapperConfiguration(idSystem, pwd);

                FdblConsole.WriteMessage("  Sending notification email (success)");
                
                _Config.SuccessEmail.SetMessage(string.Format(_Config.SuccessEmail.Message, idSystem));
                
                try { FdblSmtp.Send(_Config.SuccessEmail); } catch { }
                
                _Config.SuccessEmail.ResetMessage();

            } catch (Exception ex) {

                FdblConsole.WriteMessage("  Sending notification email (failure)");

                _Config.FailureEmail.Message = string.Format("{0}\n\n{1}", _Config.FailureEmail.Message, FdblExceptions.GetDetails(ex));
                
                try { FdblSmtp.Send(_Config.FailureEmail); } catch { }

                _Config.FailureEmail.ResetMessage();

            } finally {

                if (pv != null) {

                    FdblConsole.WriteMessage(string.Format("Disposing wrapper ({0} System)", idSystem));
                    pv.Dispose();

                }

            }

        }

        #endregion

        #region Methods - Private (Xml)

        private string _GetMonthlyPassword(USCISSystemId idSystem) {

            FdblXmlParser parser = new FdblXmlParser(_Config.Files.Password, ParserLoadFrom.File);
            FdblXmlNodeList nodes = parser.GetNodeList(string.Format("/USCISPasswords/{0}/Month[@Id='{1:00}']", idSystem, DateTime.Today.Month));

            if (nodes == null) throw new MyException(string.Format("Invalid configuration file: missing USCISPasswords/{0}/Month[{1:00}] node", idSystem, DateTime.Today.Month));

            string password = FdblXmlFunctions.GetAttributeValue(nodes.GetFirstNode(), "Password");

            if (string.IsNullOrEmpty(password)) throw new MyException(string.Format("Invalid configuration file: missing USCISPasswords/{0}/Month[{1:00}] value", idSystem, DateTime.Today.Month));

            return password;

        }

        private void _UpdateWrapperConfiguration(USCISSystemId idSystem, string password) {

            FdblXmlParser parser = new FdblXmlParser(_Config.Files.Config, ParserLoadFrom.File);
            FdblXmlNodeList nodes = parser.GetNodeList(string.Format("configuration/appSettings/add[@key='{0}.Cps.UserPassword']", idSystem));

            if (nodes == null) throw new MyException(string.Format("Invalid configuration file: missing configuration/appSettings/add[{0}.Cps.UserPassword] node", idSystem));

            nodes.GetFirstNode().SetAttribute("value", password);

            parser.Document.Save(_Config.Files.Config);

        }

        #endregion

    }

}