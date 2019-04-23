using System;
using System.Collections.Generic;
using System.Text;

namespace I9.OnDemandEmail.Console.Items {

    internal class EmailOnDemandLog {

        #region Members

        private int _EmailOnDemandId = MyConsole.NullId;
        private int _EmailQueueId = MyConsole.NullId;

        private string _Message = null;
        private string _Details = null;

        private bool _Delivered = false;
        private bool _Issue = false;

        private bool _CatchException = false;

        #endregion

        #region Constructors

        internal EmailOnDemandLog() { }

        #endregion

        #region Properties - Public

        public int EmailOnDemandId {
            get { return _EmailOnDemandId; }
            set { _EmailOnDemandId = value; }
        }

        public int EmailQueueId {
            get { return _EmailQueueId; }
            set { _EmailQueueId = value; }
        }

        public string Message {
            get { return _Message; }
            set { _Message = value; }
        }

        public string Details {
            get { return _Details; }
            set { _Details = value; }
        }

        public bool Delivered {
            get { return _Delivered; }
            set { _Delivered = value; }
        }

        public bool Issue {
            get { return _Issue; }
            set { _Issue = value; }
        }

        public bool CatchException {
            get { return _CatchException; }
            set { _CatchException = value; }
        }

        #endregion

    }

}
