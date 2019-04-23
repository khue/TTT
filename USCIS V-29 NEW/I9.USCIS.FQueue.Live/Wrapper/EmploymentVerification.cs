using System;

using I9.USCIS.Wrapper;
using I9.USCIS.Wrapper.WebService;

namespace I9.USCIS.FQueue.Live.Wrapper {

    internal class EmploymentVerification {

        #region Members

        private IRequest _Request;

        #endregion

        #region Constructors

        internal EmploymentVerification(USCISSystemId idSystem) {

            if (idSystem == USCISSystemId.Live) _Request = USCISFactory.GetLiveRequest();
            else if (idSystem == USCISSystemId.Test) _Request = USCISFactory.GetTestRequest();
            else throw new ArgumentException("The given uscis system id is not defined");

        }

        #endregion

        #region Methods - Public

        public void Dispose() {
      
            if (_Request != null) _Request.Dispose();

            System.GC.SuppressFinalize(this);

        }

        public void SetEmployee(int idQueueFuture, int idTransaction, int idEmployee, int idI9) {

            if (idQueueFuture != MyConsole.NullId) _Request.QueueFutureId = idQueueFuture;
            if (idTransaction != MyConsole.NullId) _Request.TransactionId = idTransaction;
            _Request.EmployeeId = idEmployee;
            _Request.I9Id = idI9;

        }

        #endregion

        #region Methods - Public (Web Service - Server Processes)

        public bool AutoProcessCase() {

            if (!_Request.InitiateCase()) return false;

            if (_Request.WebServiceResponse == null) return true;

            _Request.InsertLink();

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Unknown) return false;
            if (cs == USCISCaseStatus.Code_005) return true;
            if (cs == USCISCaseStatus.Code_027) return true;
            if (cs == USCISCaseStatus.Code_028) return true;
            if (cs == USCISCaseStatus.Code_029) return true;
            if (cs == USCISCaseStatus.Code_031) return true;
            if (cs == USCISCaseStatus.Code_030) return true;
            if (cs == USCISCaseStatus.Code_036) return true;
            if (cs == USCISCaseStatus.Code_038) return true;
            if (cs == USCISCaseStatus.Code_U) return true;
            if (cs == USCISCaseStatus.Code_C) return true;
            if (cs == USCISCaseStatus.Code_I) return true;
            if (cs == USCISCaseStatus.Code_N) return true;
            if (cs == USCISCaseStatus.Code_S) return true;
            if (cs == USCISCaseStatus.Code_P) return true;
            if (cs == USCISCaseStatus.Code_X) return true;

            _Request.CaseNumber = _Request.WebServiceResponse.CaseNbr;

            if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;

            _Request.InsertLink();

            return true;

        }

        #endregion

    }

}