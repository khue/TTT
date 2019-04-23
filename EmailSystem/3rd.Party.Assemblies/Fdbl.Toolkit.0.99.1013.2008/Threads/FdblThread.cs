using System;
using System.Threading;

namespace Fdbl.Toolkit.Threads {

    abstract public class FdblThread : IDisposable {

        #region Members

        private int _ThreadId;
        private bool _IsStopping;
        private bool _IsStopped;
        private bool _IsRunning;

        #endregion

        #region Properties - Public

        public bool IsRunning {
            get {
                lock (this) {
                    return _IsRunning;
                }
            }
            set {
                lock (this) {
                    _IsRunning = value;
                }
            }
        }

        public bool IsStopping {
            get {
                lock (this) {
                    return _IsStopping;
                }
            }
        }

        public bool IsStopped {
            get {
                lock (this) {
                    return _IsStopped;
                }
            }
            set {
                lock (this) {
                    _IsStopped = value;
                }
            }
        }

        public int ThreadId {
            get {
                lock (this) {
                    return _ThreadId;
                }
            }
        }

        #endregion

        #region Methods - Public (Abstract)

        public abstract void Run();

        #endregion

        #region Methods - Public

        public virtual void Dispose() {

            Stop();

            System.GC.SuppressFinalize(this);

        }

        public virtual string GetThreadDetails(int spacer) {

            return GetThreadInfo(spacer);

        }

        public virtual string GetThreadInfo(int spacer) {

            lock (this) {

                return string.Format("{0}Thread Id: {1}  Status: {2}", Utils.FdblStrings.Repeat(spacer), _ThreadId, GetStatus());

            }

        }

        protected virtual string GetStatus() {

            lock (this) {

                if (_IsStopping) return "Stopping";
                if (_IsStopped) return "Stopped";
                if (_IsRunning) return "Running";

                return "Unknown";

            }

        }

        public virtual void Stop() {

            lock (this) {

                _IsStopping = true;

            }

        }

        #endregion

        #region Constructors

        public FdblThread() : this(-1) { }

        public FdblThread(int threadId) {
            _ThreadId = threadId;
            _IsStopping = false;
            _IsStopped = false;
            _IsRunning = false;
        }

        #endregion

    }

}