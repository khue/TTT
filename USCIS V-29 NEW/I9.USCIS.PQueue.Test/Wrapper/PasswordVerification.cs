using System;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;

using I9.USCIS.Wrapper;
using I9.USCIS.Wrapper.WebService;

namespace I9.USCIS.PQueue.Test.Wrapper {
    
    internal class PasswordVerification {

        #region Members

        private IRequest _Request;

        #endregion

        #region Constructors

        internal PasswordVerification(USCISSystemId idSystem) {

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

        #endregion

        #region Methods - Public (Web Service - Server Processes)

        public bool UpdatePassword(string password) {

            return _Request.SetAccountPassword(password);

        }

        #endregion

    }
}
