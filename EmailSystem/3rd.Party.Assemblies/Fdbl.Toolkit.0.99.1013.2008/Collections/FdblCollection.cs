using System;
using System.Collections;

namespace Fdbl.Toolkit.Collections {

    public class FdblCollection : IDisposable, ICloneable {

        #region Members

        private Hashtable _Queue;
        private Hashtable _Matrix;
        private int _Index;

        #endregion

        #region Constructors

        public FdblCollection() {

            _Queue = new Hashtable();
            _Matrix = new Hashtable();
            _Index = 0;

        }

        #endregion

        #region Properties - Public

        public object CurrentItem {
            get {
                if (Size == 0) return null;
                return GetItem(_Index);
            }
        }

        public object CurrentKey {
            get {
                if (Size == 0) return string.Empty;
                if (!_Matrix.ContainsKey(_Index)) return string.Empty;
                return _Matrix[_Index];
            }
        }

        public int Size {
            get {
                if (_Queue == null) return 0;
                return _Queue.Count;
            }
        }

        #endregion

        #region Methods - Public

        public void Add(object data, string key, bool overwrite) {

            if (Utils.FdblStrings.IsBlank(key)) key = Convert.ToString(Size).PadLeft(8, '0');
            if (!overwrite && Contains(key)) throw new ArgumentException(string.Format("An item with the given key ({0}) already exists", key));

            _Queue.Add(key, data);
            _Matrix.Add(Size - 1, key);

        }

        public void Add(object data, string key) {
            Add(data, key, false);
        }

        public void Add(object data) {
            Add(data, null, false);
        }

        public void Clear() {

            if (_Queue != null) _Queue.Clear();
            if (_Matrix != null) _Matrix.Clear();
            _Index = 0;

        }

        public object Clone() {

            FdblCollection col = new FdblCollection();

            Array keyAry = Array.CreateInstance(typeof(object), Size);
            Array valAry = Array.CreateInstance(typeof(object), Size);

            _Queue.Keys.CopyTo(keyAry, 0);
            _Queue.Values.CopyTo(valAry, 0);

            Array.Sort(keyAry, valAry);

            for (int ndx = 0; ndx <= keyAry.Length - 1; ndx++) {
                col.Add(valAry.GetValue(ndx), (string)keyAry.GetValue(ndx));
            }

            return col;

        }

        public void Compact() {
            if (Size == 0) return;

            Hashtable orgQueue = (Hashtable)_Queue.Clone();
            Hashtable orgMatrix = (Hashtable)_Matrix.Clone();

            Array keyAry = Array.CreateInstance(typeof(object), Size);
            Array valAry = Array.CreateInstance(typeof(object), Size);

            _Queue.Keys.CopyTo(keyAry, 0);
            _Queue.Values.CopyTo(valAry, 0);

            Array.Sort(keyAry, valAry);

            _Queue.Clear();
            _Matrix.Clear();

            _Index = 0;

            for (int ndx = 0; ndx <= keyAry.Length - 1; ndx++) {
                Add(valAry.GetValue(ndx));
            }

        }

        public bool Contains(string key) {
            return _Queue.ContainsKey(key);
        }

        public virtual void Dispose() {

            if (_Queue != null) _Queue.Clear();
            if (_Matrix != null) _Matrix.Clear();

            System.GC.SuppressFinalize(this);

        }

        public int FindIndex(string key) {

            IDictionaryEnumerator myEnumerator = _Matrix.GetEnumerator();

            while (myEnumerator.MoveNext()) {
                if (myEnumerator.Value.Equals(key)) return (int)myEnumerator.Key + 1;
            }

            return -1;

        }

        public object GetFirstItem() {

            if (Size == 0) return null;

            _Index = 0;

            return GetItem(_Index);

        }

        public object GetItem(string key) {

            if (!Contains(key)) return null;

            return _Queue[key];

        }

        public object GetItem(int index) {

            if (!_Matrix.ContainsKey(index)) return null;

            return _Queue[_Matrix[index]];

        }

        public object GetLastItem() {

            if (Size == 0) return null;

            _Index = Size - 1;

            return GetItem(_Index);

        }

        public object GetNextItem() {

            if (Size == 0) return null;
            if (_Index + 1 > Size) return null;

            return GetItem(++_Index);

        }

        public object GetNthItem(int index) {

            if (Size == 0) return null;
            if (index < 1) return null;
            if (index > Size) return null;

            _Index = index - 1;

            return GetItem(_Index);

        }

        public object GetPreviousItem() {

            if (Size == 0) return null;
            if (_Index - 1 < 0) return null;

            return GetItem(--_Index);

        }

        public void Remove(string key) {

            if (key == null) throw new ArgumentNullException("key is null");
            if (key.Trim().Length == 0) throw new ArgumentException("key is blank");

            _Queue.Remove(key);

            if (Size == 0) {
                Clear();
                return;
            }

            Hashtable ht = new Hashtable();
            int max = Size;
            bool haveRemoved = false;

            for (int ndx = 0; ndx <= max; ndx++) {

                if (!haveRemoved) {

                    if (_Matrix[ndx].Equals(key)) {
                        haveRemoved = true;
                    } else {
                        ht.Add(ndx, _Matrix[ndx]);
                    }

                } else {

                    ht.Add(ndx - 1, _Matrix[ndx]);

                }

            }

            if (haveRemoved) {

                _Index--;
                _Matrix = ht;

            }

        }

        public void Remove(int key) {
            Remove((string)_Matrix[key]);
        }

        public Array ToArray() {

            if (Size == 0) return null;

            int tmpIndex = 0;
            int ndx = 0;

            try {
                tmpIndex = _Index;

                Array tmpArray = Array.CreateInstance(typeof(object), Size);

                object tmpObject = GetFirstItem();
                while (tmpObject != null) {
                    tmpArray.SetValue(tmpObject, ndx++);
                    tmpObject = GetNextItem();
                }

                return tmpArray;

            } finally {

                _Index = tmpIndex;

            }

        }

        #endregion

    }

}