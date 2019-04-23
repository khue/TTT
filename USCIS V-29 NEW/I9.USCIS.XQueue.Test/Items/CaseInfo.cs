using System;

using com.vis_dhs;

namespace I9.USCIS.XQueue.Test.Items {

    internal class CaseInfo {

        #region Members

        private int _TransactionId = MyConsole.NullId;
        private int _RequestId = MyConsole.NullId;

        private int _EmployeeId = MyConsole.NullId;
        private int _I9Id = MyConsole.NullId;

        private string _CaseNumber = null;
        private string _TypeOfCase = null;
        private string _UserField = null;

        private string _ResponseCode = null;
        private string _ResponseStatement = null;

        private DateTime _ResolveDate;

        private bool _Processed = false;

        private string _Issue = null;
        private string _Details = null;

        #endregion

        #region Constructors

        internal CaseInfo() { }

        internal CaseInfo(ICaseListArray uscisCase) {

            if (uscisCase == null) throw new ArgumentNullException("The ICaseListArray obejct is null");

            _CaseNumber = uscisCase.GetCaseNumber();
            _TypeOfCase = uscisCase.GetTypeOfCase();

            
            if (uscisCase.GetResolutionCode().Length > 0)
            {
                _ResponseCode = uscisCase.GetResolutionCode();
            }
            else
            {
                _ResponseCode = uscisCase.GetMessageCode();
            }

            _ResponseStatement = uscisCase.GetEligibility();

            _ResolveDate = Convert.ToDateTime(uscisCase.GetResolveDate());

        }

        #endregion

        #region Properties - Public

        public bool Processed {
            get { return _Processed; }
            set { _Processed = value; }
        }

        public string Issue {
            get { return _Issue; }
            set { _Issue = value; }
        }

        public string Details {
            get { return _Details; }
            set { _Details = value; }
        }

        public int TransactionId {
            get { return _TransactionId; }
            set { _TransactionId = value; } 
        }

        public int RequestId {
            get { return _RequestId; }
            set { _RequestId = value; }
        }

        public int EmployeeId {
            get { return _EmployeeId; }
            set { _EmployeeId = value; }
        }

        public int I9Id { 
            get { return _I9Id; } 
            set { _I9Id = value; } 
        }

        public string CaseNumber { get { return _CaseNumber; } }
        public string TypeOfCase { get { return _TypeOfCase; } }
        public string UserField { get { return _UserField; } }

        public string ResponseCode { get { return _ResponseCode; } }
        public string ResponseStatement { get { return _ResponseStatement; } }

        public DateTime ResolveDate { get { return _ResolveDate; } }

        public string RawData { get { return string.Format("USCISRequestId={0};USCISTransactionId={1};Processed={2};CaseNumber={3};TypeOfCase={4};ResolvedDate={5};ResponseCode={6};ResponseStatement={7};Issue={8}", _RequestId, _TransactionId, _Processed, _CaseNumber, _TypeOfCase, _ResolveDate, _ResponseCode, _ResponseStatement, _Issue); } }
        
        #endregion

    }

}
