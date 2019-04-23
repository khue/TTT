using System;
using System.Collections;

namespace Fdbl.Toolkit.Collections {

    public class FdblArrayList : ArrayList {

        #region Members

        private int _Index;

        #endregion

        #region Constructors

        public FdblArrayList() : base() {
            _Index = -1;
        }

        public FdblArrayList(ICollection c) : base(c) {
            _Index = -1;
        }

        public FdblArrayList(int capacity) : base(capacity) {
            _Index = -1;
        }

        #endregion

        #region Methods - Public

        public virtual object GetFirstItem() {
            _Index = 0;
            return _GetItem(_Index);
        }

        public virtual object GetNextItem() {
            return _GetItem(++_Index);
        }

        public virtual object GetLastItem() {
            _Index = this.Count - 1;
            return _GetItem(_Index);
        }

        public virtual object GetPreviousItem() {
            return _GetItem(--_Index);
        }

        #endregion

        #region Methods - Private

        private object _GetItem(int index) {

            if (this.Count == 0) return null;
            if (index < 0) return null;
            if (index >= this.Count) return null;

            return this[index];

        }

        #endregion

    }

}
