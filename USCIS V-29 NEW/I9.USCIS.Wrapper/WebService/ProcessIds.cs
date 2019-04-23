using System;

namespace I9.USCIS.Wrapper.WebService {

    public class ProcessIds {

        #region Members

        private string _CaptureLog = null;

        private USCISSystemId _SystemId = USCISSystemId.Unknown;
        private USCISCategoryId _CategoryId = USCISCategoryId.Unknown;

        private int _TransactionId = Request.NullId;
        private int _EmployeeId = Request.NullId;
        private int _I9Id = Request.NullId;

        private string _CaseNumber = null;
        
        private int _RequestId = Request.NullId;
        private int _ResponseId = Request.NullId;

        private int _ConsoleId = Request.NullId;
        private int _QueueErrorId = Request.NullId;
        private int _QueueFutureId = Request.NullId;

        private USCISMethodId _MethodId = USCISMethodId.Unknown;
        private string _MethodName = null;

        private bool _NextCategoryCallIsInternal = false;

        #endregion

        #region Constructors

        public ProcessIds(string captureLog) {

            _CaptureLog = captureLog;

        }

        #endregion

        #region Properties - Public

        public string CaptureLog {
            get { return _CaptureLog; }
            set { _CaptureLog = value; }
        }

        public USCISSystemId SystemId {
            get { return _SystemId; }
            set { _SystemId = value; }
        }

        public USCISCategoryId CategoryId {

            get {

                if (_NextCategoryCallIsInternal) {

                    _NextCategoryCallIsInternal = false;
                    return USCISCategoryId.Internal;

                } else return _CategoryId;

            }

            set { _CategoryId = value; }

        }

        public int TransactionId {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }

        public int EmployeeId {
            get { return _EmployeeId; }
            set { _EmployeeId = value; }
        }

        public int I9Id {
            get { return _I9Id; }
            set { _I9Id = value; }
        }

        public string CaseNumber {
            get { return _CaseNumber; }
            set { _CaseNumber = value; }
        }

      
        public int ConsoleId {
            get { return _ConsoleId; }
            set { _ConsoleId = value; }
        }

        public int QueueErrorId {
            get { return _QueueErrorId; }
            set { _QueueErrorId = value; }
        }

        public int QueueFutureId {
            get { return _QueueFutureId; }
            set { _QueueFutureId = value; }
        }

        public int RequestId {
            get { return _RequestId; }
            set { _RequestId = value; }
        }

        public int ResponseId {
            get { return _ResponseId; }
            set { _ResponseId = value; }
        }

        public USCISMethodId MethodId {
            get { return _MethodId; }
            set { _MethodId = value; }
        }

        public string MethodName {
            get { return _MethodName; }
            set { _MethodName = value; }
        }

        public bool NextCategoryCallIsInternal {
            get { return _NextCategoryCallIsInternal; }
            set { _NextCategoryCallIsInternal = value; }
        }

        #endregion

    }

}