using System;

namespace I9.USCIS.Wrapper {

    internal class USCISException : Exception {

        #region Constructors

        public USCISException() : base() { }

        public USCISException(string message) : base(message) { }

        public USCISException(string message, Exception innerException) : base(message, innerException) { }

        #endregion

    }

}
