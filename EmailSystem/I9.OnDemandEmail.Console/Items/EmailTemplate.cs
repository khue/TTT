using System;

namespace I9.OnDemandEmail.Console.Items {

    internal class EmailTemplate {

        #region Members
        
        private int _EmailTemplateId = MyConsole.NullId;
        private int _EmailTemplateTypeId = MyConsole.NullId;
        private int _CompanyId = MyConsole.NullId;

        private string _TemplateTypeName = null;
        private string _TemplateName = null;
        private string _SendFrom = null;
        private string _SendSender = null;
        private string _SendReplyTo = null;
		private string _Subject = null;
		private string _MessageText = null;
        private string _MessageHtml = null;

		private bool _AllowMultiple = false;

        #endregion

        #region Constructors

        internal EmailTemplate(Sql.spODE_EmailTemplate_Get spTemplateGet) {

            if (spTemplateGet == null) throw new ArgumentNullException("spODE_EmailTemplates_Get is null");

            _EmailTemplateId = Convert.ToInt32(spTemplateGet.GetDataReaderValue(0, -1));
            _EmailTemplateTypeId = Convert.ToInt32(spTemplateGet.GetDataReaderValue(1, -1));
            _CompanyId = Convert.ToInt32(spTemplateGet.GetDataReaderValue(2, -1));

            _TemplateTypeName = Convert.ToString(spTemplateGet.GetDataReaderValue(3, null));
            _TemplateName = Convert.ToString(spTemplateGet.GetDataReaderValue(4, null));
            _SendFrom = Convert.ToString(spTemplateGet.GetDataReaderValue(5, null));
            _Subject = Convert.ToString(spTemplateGet.GetDataReaderValue(6, null));
            _MessageText = Convert.ToString(spTemplateGet.GetDataReaderValue(7, null));
            _MessageHtml = Convert.ToString(spTemplateGet.GetDataReaderValue(8, null));
            

            _AllowMultiple = Convert.ToBoolean(spTemplateGet.GetDataReaderValue(9, false));
            _SendSender = Convert.ToString(spTemplateGet.GetDataReaderValue(10, null));
            _SendReplyTo = Convert.ToString(spTemplateGet.GetDataReaderValue(11, null));

        }

        internal EmailTemplate(Sql.spODE_EmailTemplatePassword_Get spTemplatesPasswordGet) {

            if (spTemplatesPasswordGet == null) throw new ArgumentNullException("spODE_EmailTemplatesPassword_Get is null");

            _EmailTemplateId = Convert.ToInt32(spTemplatesPasswordGet.GetDataReaderValue(0, -1));
            _EmailTemplateTypeId = Convert.ToInt32(spTemplatesPasswordGet.GetDataReaderValue(1, -1));
            _CompanyId = Convert.ToInt32(spTemplatesPasswordGet.GetDataReaderValue(2, -1));

            _TemplateTypeName = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(3, null));
            _TemplateName = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(4, null));
            _SendFrom = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(5, null));
            _Subject = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(6, null));
            _MessageText = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(7, null));
            _MessageHtml = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(8, null));

            _AllowMultiple = Convert.ToBoolean(spTemplatesPasswordGet.GetDataReaderValue(9, false));
            _SendSender = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(10, null));
            _SendReplyTo = Convert.ToString(spTemplatesPasswordGet.GetDataReaderValue(11, null));

        }

        #endregion

        #region Public Properties

        public int CompanyId { get { return _CompanyId; } }
        public int EmailTemplateId { get { return _EmailTemplateId; } }
        public int EmailTemplateTypeId { get { return _EmailTemplateTypeId; } }

        public string MessageHtml { get { return _MessageHtml; } }
        public string MessageText { get { return _MessageText; } }
        public string SendFrom { get { return _SendFrom; } }
        public string SendSender { get { return _SendSender; } }
        public string SendReplyTo {  get { return _SendReplyTo; } }
        public string Subject { get { return _Subject; } }
        public string TemplateName { get { return _TemplateName; } }

        public bool AllowMultiple { get { return _AllowMultiple; } }

        #endregion

        #region Public Properties (Virtual)

        public bool IsNewEmployeeAgentUser {

            get {

                if (string.IsNullOrEmpty(_TemplateTypeName)) return false;
                if (_TemplateTypeName.Equals("New Employee", StringComparison.OrdinalIgnoreCase)) return true;
                if (_TemplateTypeName.Equals("New Agent Email", StringComparison.OrdinalIgnoreCase)) return true;
                if (_TemplateTypeName.Equals("New User Email", StringComparison.OrdinalIgnoreCase)) return true;
                return false;

            }

        }

        public bool IsNewEmployeePassword {

            get {

                if (string.IsNullOrEmpty(_TemplateTypeName)) return false;
                if (_TemplateTypeName.Equals("New Employee Password", StringComparison.OrdinalIgnoreCase)) return true;

                return false;

            }

        }

        #endregion

    }

}
