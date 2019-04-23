﻿using System;

namespace I9.USCIS.FQueue.Live {

    internal class MyException : Exception {

        #region Constructors

        public MyException() : base() { }

        public MyException(string message) : base(message) { }

        public MyException(string message, Exception innerException) : base(message, innerException) { }

        #endregion

    }

}
