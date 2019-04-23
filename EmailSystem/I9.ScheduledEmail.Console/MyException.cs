using System;

namespace I9.ScheduledEmail.Console {

    internal class MyException : Exception {

        #region Constructors

        internal MyException() : base() { }

        internal MyException(string message) : base(message) { }

        internal MyException(string message, Exception innerException) : base(message, innerException) { }

        #endregion

    }

}
