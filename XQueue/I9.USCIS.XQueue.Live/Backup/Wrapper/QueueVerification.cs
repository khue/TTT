﻿using System;
using System.IO;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

using I9.USCIS.Wrapper;
using I9.USCIS.Wrapper.WebService;

namespace I9.USCIS.XQueue.Live.Wrapper {

    internal class QueueVerification {

        #region Members

        private IRequest _Request;
               
        private string _SqlConnection = null;
        private string _LogFile = null;

        private string _LastCaseNumber = null;
        private bool _LastCaseProcessed = false;

        #endregion

        #region Constructors

        internal QueueVerification(USCISSystemId idSystem, string sqlConnection, string logFile) {

            if (idSystem == USCISSystemId.Live) _Request = USCISFactory.GetLiveRequest();
            else if (idSystem == USCISSystemId.Test) _Request = USCISFactory.GetTestRequest();
            else throw new ArgumentException("The given uscis system id is not defined");

            if (string.IsNullOrEmpty(sqlConnection)) throw new ArgumentException("The given sql connection is null");

            _SqlConnection = sqlConnection;

            if (string.IsNullOrEmpty(logFile)) throw new ArgumentException("The given log file is null");

            _LogFile = logFile;

        }

        #endregion

        #region Methods - Public

        public void Dispose() {

            if (_Request != null) _Request.Dispose();

            System.GC.SuppressFinalize(this);

        }

        #endregion

        #region Methods - Public (Web Service - Server Processes)

        public bool ProcessNextCase() {

            _Request.EmployeeId = MyConsole.BypassId;
            _Request.I9Id = MyConsole.BypassId;
            _Request.CaseNumber = null;

            _Request.GetNextCase();

            if (_Request.WebServiceResponse == null) return false;
            if (_Request.WebServiceResponse.NumberOf == 0) return false;
            if (_Request.WebServiceResponse.ReturnStatus != 0) throw new MyException(string.Format("{0}: (Code: {1})", _Request.WebServiceResponse.ReturnStatusMsg, _Request.WebServiceResponse.ReturnStatus));

            for (int i = 0; i < _Request.WebServiceResponse.NumberOf; i++) {

                Items.CaseInfo ci = new Items.CaseInfo(_Request.WebServiceResponse.Cases[i]);

                if (ci.CaseNumber.Equals(_LastCaseNumber, StringComparison.OrdinalIgnoreCase) && _LastCaseProcessed == false ) throw new MyException(string.Format("Same case ({0}) as last time.  Queue might be stuck.", _LastCaseNumber));
                _LastCaseProcessed = false;
                _LastCaseNumber = ci.CaseNumber;

                _Request.EmployeeId = MyConsole.NullId;
                _Request.I9Id = MyConsole.NullId;
                _Request.CaseNumber = ci.CaseNumber;

                _Request.GetSystemIdentities();
                
                ci.TransactionId = _Request.TransactionId;
                ci.EmployeeId = _Request.EmployeeId;
                ci.I9Id = _Request.I9Id;

                ci.RequestId = _Request.Pids.RequestId;

                string tmpCode = ci.ResponseCode.ToLower();
                int tmpInt = 0;

                if (int.TryParse(tmpCode, out tmpInt)) tmpCode = tmpCode.PadLeft(3, '0');

                _Request.UpdateRequest(_Request.Pids.RequestId, _Request.Pids.TransactionId, _Request.EmployeeId, _Request.I9Id);

                try {

                    if (ci.TypeOfCase.Equals("1")) { // SSA Response

                        if (tmpCode.Equals("005")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_005, USCISProcessStatus.AwaitingUSCIS);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_005, _Request.EmployeeId, _Request.I9Id);
                            
                        } else if (tmpCode.Equals("008")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_008, USCISProcessStatus.ClosingCase);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_008, _Request.EmployeeId, _Request.I9Id);

                            bool isClosed = _Request.CloseCase(USCISClosureId.EAUTH);

                            if (!isClosed) _Request.UpdateTransaction(USCISCaseStatus.Code_008, USCISProcessStatus.CaseNeedsClose);

                        } else if (tmpCode.Equals("016")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_016, USCISProcessStatus.ClosingCase);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_016, _Request.EmployeeId, _Request.I9Id);

                            bool isClosed = _Request.CloseCase(USCISClosureId.EAUTH);

                            if (!isClosed) _Request.UpdateTransaction(USCISCaseStatus.Code_016, USCISProcessStatus.CaseNeedsClose);

                        } else if (tmpCode.Equals("028")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_028, USCISProcessStatus.CaseNeedsClose);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_028, _Request.EmployeeId, _Request.I9Id);

                        } else if (tmpCode.Equals("034")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_034, USCISProcessStatus.AwaitingUSCIS);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_034, _Request.EmployeeId, _Request.I9Id);

                        } else if (tmpCode.Equals("035")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_035, USCISProcessStatus.AwaitingClient);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_035, _Request.EmployeeId, _Request.I9Id);

                        }
                        else if (tmpCode.Equals("038"))
                        {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_038, USCISProcessStatus.AwaitingClient);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_038, _Request.EmployeeId, _Request.I9Id);


                        } else if (tmpCode.Equals("d")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_D, USCISProcessStatus.AwaitingUSCIS);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_D, _Request.EmployeeId, _Request.I9Id);
                        }

                    } else if (ci.TypeOfCase.Equals("2")) { // DHS Secondary Response

                        if (tmpCode.Equals("a")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_A, USCISProcessStatus.ClosingCase);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_A, _Request.EmployeeId, _Request.I9Id);

                            bool isClosed = _Request.CloseCase(USCISClosureId.EAUTH);

                            if (!isClosed) _Request.UpdateTransaction(USCISCaseStatus.Code_A, USCISProcessStatus.CaseNeedsClose);

                        } else if (tmpCode.Equals("u")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_U, USCISProcessStatus.AwaitingClient);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_U, _Request.EmployeeId, _Request.I9Id);

                        } else if (tmpCode.Equals("c")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_C, USCISProcessStatus.AwaitingUSCIS);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_C, _Request.EmployeeId, _Request.I9Id);
                        }

                    } else if (ci.TypeOfCase.Equals("3")) { // DHS Third Step Response

                        if (tmpCode.Equals("i")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_I, USCISProcessStatus.AwaitingUSCIS);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_I, _Request.EmployeeId, _Request.I9Id);

                        } else if (tmpCode.Equals("n")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_N, USCISProcessStatus.CaseNeedsClose);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_N, _Request.EmployeeId, _Request.I9Id);

                        } else if (tmpCode.Equals("o")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_O, USCISProcessStatus.ClosingCase);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_O, _Request.EmployeeId, _Request.I9Id);

                            bool isClosed = _Request.CloseCase(USCISClosureId.EAUTH);

                            if (!isClosed) _Request.UpdateTransaction(USCISCaseStatus.Code_O, USCISProcessStatus.CaseNeedsClose);

                        } else if (tmpCode.Equals("s")) {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_S, USCISProcessStatus.CaseNeedsClose);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_S, _Request.EmployeeId, _Request.I9Id);

                        }
                        else if (tmpCode.Equals("038"))
                        {

                            _Request.UpdateTransaction(USCISCaseStatus.Code_038, USCISProcessStatus.CaseNeedsClose);
                            CheckAndSendEmail((int)OnDemandEmailCodes.Code_038, _Request.EmployeeId, _Request.I9Id);
                        }

                    }

                    ci.Processed = true;
                    _LastCaseProcessed = true;
                } catch (Exception ex) {

                    ci.Issue = ex.Message;
                    ci.Details = FdblExceptions.GetDetails(ex);

                } finally {

                    _InsertResolvedCase(ci);

                    FdblConsole.WriteMessage(string.Format("Processed -> Employee: {0}  I9: {1}  Case: {2}", ci.EmployeeId, ci.I9Id, ci.CaseNumber));

                }

            }

            return true;

        }

        #endregion

        #region Methods - Private

        private void _InsertResolvedCase(Items.CaseInfo ci) {

            Sql.spUSCIS_ResolvedCase_Insert spResolvedCaseInsert = null;

            try {

                spResolvedCaseInsert = new Sql.spUSCIS_ResolvedCase_Insert(_SqlConnection);

                int spReturnCode = spResolvedCaseInsert.StartDataReader(ci);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new Exception(string.Format("spUSCIS_ResolvedCase_Insert returned: {0} (Sql returned: {1})", spReturnCode, spResolvedCaseInsert.SqlErrorCode));

            } catch (Exception ex) {

                File.AppendAllText(_LogFile, string.Format("{0}|{1}\n", ci.RawData, ex.Message));

            } finally {

                if (spResolvedCaseInsert != null) spResolvedCaseInsert.Dispose();

            }

        }

        #endregion

        #region Methods - Protected

        protected void CheckAndSendEmail(int idEmailTemplateType, int idEmployee, int idI9)
        {
            Sql.spUSCIS_OnDemandEmail_Insert spOnDemandEmailInsert = new Sql.spUSCIS_OnDemandEmail_Insert(_SqlConnection);

            try
            {
                int spReturnCode = spOnDemandEmailInsert.StartDataReader(idEmailTemplateType, idEmployee, idI9);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new Exception(string.Format("spUSCIS_OnDemandEmail_Insert returned: {0} (Sql returned: {1})", spReturnCode, spOnDemandEmailInsert.SqlErrorCode));

                //return true;

            }// end try

            catch (Exception ex)
            {

                File.AppendAllText(_LogFile, string.Format("{0}|{1}\n", "", ex.Message));
                //return false;
            }// end catch

            finally
            {

                if (spOnDemandEmailInsert != null) spOnDemandEmailInsert.Dispose();

            }// end finally

        }// end CheckAndSendEmail

        #endregion

    }

}