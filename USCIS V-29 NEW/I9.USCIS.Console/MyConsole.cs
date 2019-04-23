using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

using I9.USCIS.Wrapper;

namespace I9.USCIS.Console {

    internal class MyConsole {

        #region Fields

        public static readonly int NullId = -1;

        #endregion

        #region Members

        private MyConfiguration _MyConfig = null;

        #endregion

        #region Constructors

        internal MyConsole() {

            FdblConsole.WriteInitialization(System.Reflection.Assembly.GetCallingAssembly());

            _MyConfig = new MyConfiguration();

            FdblConsole.WriteMessage(string.Format("Console started on physical node {0}", Environment.MachineName));

        }

        #endregion

        #region Methods - Public

        public void Run(int idConsole) {

            try {
                
                FdblConsole.WriteMessage(string.Format("Fetching console record (id: {0})", idConsole));
                Items.SqlConsoleItem sci = _ProcessBegin(idConsole, Process.GetCurrentProcess().Id);

                sci.ConsoleId = idConsole;

                FdblConsole.WriteMessage("Processing console record");
                _Process(sci);

                FdblConsole.WriteMessage("Closing console record");
                _ProcessComplete(idConsole, Process.GetCurrentProcess().Id);

            } catch (Exception ex) {

                FdblConsole.WriteMessage("Resetting console record");
                //_ProcessReset(idConsole, Process.GetCurrentProcess().Id);
                _ProcessComplete(idConsole, Process.GetCurrentProcess().Id);

                FdblConsole.WriteMessage("Sending notification (failure)");
                _SendNotification(ex);

            } finally {

                FdblConsole.WriteMessage("Console is shutting down");

            }

        }

        #endregion

        #region Methods - Private (Processing)

        private Items.SqlConsoleItem _ProcessBegin(int idConsole, int idWin32) {

            Sql.spUSCISBE_Console_Begin spConsoleBegin = null;

            try {

                spConsoleBegin = new Sql.spUSCISBE_Console_Begin(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spConsoleBegin.StartDataReader(idConsole, idWin32);

                if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) throw new MyException(string.Format("spUSCIS_Console_Begin could not locate the record for id: {0}", idConsole));

                if (!spConsoleBegin.MoveNextDataReader(true)) throw new MyException("spUSCISBE_Console_Begin could not advance cursor");

                return new Items.SqlConsoleItem(spConsoleBegin);

            } finally {

                if (spConsoleBegin != null) spConsoleBegin.Dispose();

            }

        }

        private void _Process(Items.SqlConsoleItem sci) {

            Wrapper.EmploymentVerification ev = null;

            try {

                ev = new Wrapper.EmploymentVerification(sci.ConsoleId, sci.SystemIdEnum, sci.EmployeeId, sci.I9Id);

                if (sci.ActionIdEnum == USCISActionId.AutoProcessInitiateCase) ev.AutoProcessInitiateCase();
                else if (sci.ActionIdEnum == USCISActionId.AutoProcessSSAVerification) ev.AutoProcessSSAVerification(sci.CaseNumber);
                //else if (sci.ActionIdEnum == USCISActionId.InitiateCase) ev.InitiateCase();
                else if (sci.ActionIdEnum == USCISActionId.InitiateCase) ev.AutoProcessInitiateCase();
                else if (sci.ActionIdEnum == USCISActionId.SubmitSSAReferral) ev.SubmitSSAReferral(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.ResubmitSSAVerification) ev.ResubmitSSAVerification(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.SubmitSecondVerification) ev.SubmitSecondVerification(sci.CaseNumber, sci.Comments);
                else if (sci.ActionIdEnum == USCISActionId.SubmitDHSReferral) ev.SubmitDHSReferral(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.CloseCase) ev.CloseCase(sci.ClosureId, sci.CaseNumber, sci.CurrentlyEmployed);
                else if (sci.ActionIdEnum == USCISActionId.EmpConfirmPhoto) ev.EmpConfirmPhoto(sci.CaseNumber,true);
                else if (sci.ActionIdEnum == USCISActionId.EmpConfirmPhotoNoClose) ev.EmpConfirmPhoto(sci.CaseNumber, false);
                else if (sci.ActionIdEnum == USCISActionId.EmpUpdateSSALetterReceived) ev.EmpUpdateSSALetterReceived(sci.CaseNumber, sci.LetterTypeCode);
                else if (sci.ActionIdEnum == USCISActionId.EmpUpdateDHSLetterReceived) ev.EmpUpdateDHSLetterReceived(sci.CaseNumber, sci.LetterTypeCode);
                else if (sci.ActionIdEnum == USCISActionId.EmpSSAReVerify) ev.EmpSSAReVerify(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpDHSReVerify) ev.EmpDHSReVerify(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpCitDHSReVerify) ev.EmpCitDHSReVerify(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpRetrievePhoto) ev.EmpRetrievePhoto(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpDMVDHSReVerify) ev.EmpDMVDHSReVerify(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpRetrieveFAN) ev.EmpRetrieveFAN(sci.CaseNumber,sci.LetterTypeCode);
                else if (sci.ActionIdEnum == USCISActionId.EmpGetCaseDetails) ev.EmpGetCaseDetails(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpGetDuplicateCaseList) ev.EmpGetDuplicateCaseList(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpDupCaseContinueWithoutChanges) ev.EmpDupCaseContinueWithoutChanges(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpDupCaseContinueWithChanges) ev.EmpDupCaseContinueWithChanges(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpSaveSSATNCNotification) ev.EmpSaveSSATNCNotification(sci.CaseNumber);
                else if (sci.ActionIdEnum == USCISActionId.EmpSaveDHSTNCNotification) ev.EmpSaveDHSTNCNotification(sci.CaseNumber);

            } finally {

                if (ev != null) ev.Dispose();

            }

        }

        private void _ProcessComplete(int idConsole, int idHandle) {

            Sql.spUSCISBE_Console_Complete spConsoleComplete = null;

            try {

                spConsoleComplete = new Sql.spUSCISBE_Console_Complete(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spConsoleComplete.StartDataReader(idConsole, idHandle);

            } finally {

                if (spConsoleComplete != null) spConsoleComplete.Dispose();

            }

        }

        private void _ProcessReset(int idConsole, int idHandle) {

            Sql.spUSCISBE_Console_Reset spConsoleReset = null;

            try {

                spConsoleReset = new Sql.spUSCISBE_Console_Reset(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spConsoleReset.StartDataReader(idConsole, idHandle);

            } finally {

                if (spConsoleReset != null) spConsoleReset.Dispose();

            }

        }

        #endregion

        #region Methods - Private (Email)

        private void _SendNotification(Exception ex) {

            FdblSmtpRecord smtp = _MyConfig.FailureEmail;

            if (ex != null) smtp.SetMessage(string.Format("{0}\n\n{1}", smtp.Message, FdblExceptions.GetDetails(ex)));

            try { FdblSmtp.Send(smtp); } catch { }

            if (ex != null) smtp.ResetMessage();

        }

        #endregion

    }

}
