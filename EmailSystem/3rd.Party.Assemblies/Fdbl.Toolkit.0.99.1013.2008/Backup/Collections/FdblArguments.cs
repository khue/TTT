using System;

namespace Fdbl.Toolkit.Collections {

    public class FdblArguments : FdblCollection {

        #region Constructors

        public FdblArguments(string[] args)
            : base() {

            if (args == null) throw new ArgumentNullException("argument array is null");
            if (args.Length == 0) throw new ArgumentException("argument array is empty");

            string arg1 = null;
            string arg2 = null;

            for (int ndx = 0; ndx < args.Length; ndx++) {

                arg1 = Utils.FdblStrings.Left(args[ndx], "=").ToLower();
                arg2 = args[ndx].Substring(arg1.Length + 1);

                Add(arg2, arg1);

            }

        }

        #endregion

        #region Methods - Public

        public bool IsValid(string key) {

            if (key == null) throw new ArgumentNullException("key is null");
            if (key.Trim().Length == 0) throw new ArgumentException("key is blank");

            key = key.Trim().ToLower();

            object data = GetItem(key);

            if (data == null) return false;
            if (Convert.ToString(data).Trim().Length == 0) return false;

            return true;

        }

        #endregion

    }

}