using System;
using System.Collections.Generic;
using System.Text;

namespace I9.ScheduledEmail.Console.Items {

    internal class JobDetail {

        #region Members

        private int _EmailJobId = -1;
        private int _EmailTemplateId = -1;

        private DateTime _DTStart = DateTime.Now;
        private DateTime _DTFinish = DateTime.Now;

        private int _Clients = 0;
        private int _Attachments = 0;
        private int _Delivered = 0;
        private int _Issues = 0;

        private bool _IsActive = false;

        #endregion

        #region Constructors

        internal JobDetail() { }

        #endregion

        #region Properties - Public

        public int EmailJobId {
            get { return _EmailJobId; }
            set { _EmailJobId = value; }
        }

        public int EmailTemplateId {
            get { return _EmailTemplateId; }
            set { _EmailTemplateId = value; }
        }

        public DateTime DTStart {
            get { return _DTStart; }
            set { _DTStart = value; }
        }

        public DateTime DTFinish {
            get { return _DTFinish; }
            set { _DTFinish = value; }
        }

        public int Clients {
            get { return _Clients; }
            set { _Clients = value; }
        }

        public int Attachments {
            get { return _Attachments; }
            set { _Attachments = value; }
        }

        public int Delivered {
            get { return _Delivered; }
            set { _Delivered = value; }
        }

        public int Issues {
            get { return _Issues; }
            set { _Issues = value; }
        }

        public bool IsActive {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        #endregion

        #region Methods - Public

        public void Reset() {

            _EmailTemplateId = -1;

            _DTStart = DateTime.Now;
            _DTFinish = DateTime.Now;

            _Clients = 0;
            _Attachments = 0;
            _Delivered = 0;
            _Issues = 0;

        }

        #endregion

    }

}