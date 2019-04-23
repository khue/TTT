using System;
using System.Collections;

namespace Fdbl.Toolkit.Collections {

    public class FdblQueue {

        #region Members

        private ArrayList _Queue;

        #endregion

        #region Properties - Public

        public bool PendingItems {
            get {
                lock (this) {
                    return _Queue.Count > 0;
                }
            }
        }

        public int Size {
            get {
                lock (this) {
                    return _Queue.Count;
                }
            }
        }

        #endregion

        #region Methods - Public

        public ArrayList ConsumeAllItems() {
            lock (this) {
                ArrayList items = (ArrayList)_Queue.Clone();
                _Queue.Clear();
                return items;
            }
        }

        public string ConsumeItem() {
            lock (this) {
                if (_Queue.Count == 0) return null;
                string item = (string)_Queue[0];
                _Queue.Remove(0);
                return item;
            }
        }

        public void PublishItem(string message) {
            lock (this) {
                _Queue.Add(message);
            }
        }

        #endregion

        #region Constructors

        public FdblQueue() {
            _Queue = new ArrayList();
        }

        #endregion

    }

}