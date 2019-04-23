using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;
using Fdbl.Toolkit.Xml;

namespace I9.ScheduledEmail.Console.Items {
    
    internal class EmailRecord {

        #region Members

        private int _EmailTemplateId = -1;
        private int _UserId = -1;
        private int _AgentId = -1;
        private int _EmployeeId = -1;
        private int _I9Id = -1;
        private int _FutureEmployeeId = -1;

        private string _FirstName = null;
        private string _LastName = null;

        private string _SentFrom = null;
        private string _SendTo = null;
        private string _CopyTo = null;
        private string _BlindCopyTo = null;
        private string _Subject = null;
        private string _MessageText = null;
        private string _MessageHtml = null;

        private bool _Delivered = false;
        private bool _CanDeliver = true;

        private bool _IsKeywordIssue = false;
        private bool _IsAddressIssue = false;

        private List<AddressIssue> _AddressIssues = new List<AddressIssue>();

        #endregion

        #region Constructors

        internal EmailRecord(EmailTemplate et, List<AddressRecord> al, XmlElement xes) {

            if (et == null) throw new ArgumentNullException("email template is null");
            if (al == null) throw new ArgumentNullException("address list is null");
            if (xes == null) throw new ArgumentNullException("xml element is null");

            if (al.Count == 0) throw new ArgumentException("address list is empty");

            _EmailTemplateId = et.EmailTemplateId;
            _SentFrom = et.SendFrom;
            _Subject = et.Subject;
            _MessageText = et.MessageText;
            _MessageHtml = et.MessageHtml;

            XmlDocument doc = new XmlDocument();

            doc.AppendChild(doc.ImportNode(xes, true));

            XmlElement xe = (XmlElement)doc.FirstChild;

            XmlAttributeCollection xac = xe.Attributes;
            if (xac == null || xac.Count == 0) throw new MyException("Could not find the record node attributes: record");

            string id = FdblXmlFunctions.GetAttributeValue(xac, "Id");
            string type = FdblXmlFunctions.GetAttributeValue(xac, "Type");

            try { _FirstName = FdblXmlFunctions.GetAttributeValue(xac, "FirstName"); } catch { _FirstName = null; }
            try { _LastName = FdblXmlFunctions.GetAttributeValue(xac, "LastName"); } catch { _LastName = null; }

            if (type.Equals("User", StringComparison.OrdinalIgnoreCase)) _UserId = Int32.Parse(id);
            else if (type.Equals("Agent", StringComparison.OrdinalIgnoreCase)) _AgentId = Int32.Parse(id);
            else if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase)) {

                _EmployeeId = Int32.Parse(id);

                try { id = FdblXmlFunctions.GetAttributeValue(xac, "CurrentI9Id"); } catch { id = null; }
                
                if (!string.IsNullOrEmpty(id)) _I9Id = Int32.Parse(id);

            } else if (type.Equals("FutureEmployee", StringComparison.OrdinalIgnoreCase)) _FutureEmployeeId = Int32.Parse(id);

            FdblXmlNodeList fxnl = FdblXmlFunctions.GetNodeList(xe, "Keyword");
            if (fxnl != null && fxnl.Count > 0) {

                xac = fxnl.GetFirstNode().Attributes;

                if (xac != null) {

                    for (int i = 0; i < xac.Count; i++) {

                        string lookFor = "{{" + xac[i].Name + "}}";
                        string replaceWith = xac[i].Value;

                        if (_Subject.IndexOf(lookFor, StringComparison.OrdinalIgnoreCase) != -1) _Subject = _Subject.Replace(lookFor, replaceWith);
                        if (!string.IsNullOrEmpty(_MessageText) && _MessageText.IndexOf(lookFor, StringComparison.OrdinalIgnoreCase) != -1) _MessageText = _MessageText.Replace(lookFor, replaceWith);
                        if (!string.IsNullOrEmpty(_MessageHtml) && _MessageHtml.IndexOf(lookFor, StringComparison.OrdinalIgnoreCase) != -1) _MessageHtml = _MessageHtml.Replace(lookFor, replaceWith);

                    }

                }

            }

            if ((_Subject.Contains("{{") && _Subject.Contains("}}")) ||
                (!string.IsNullOrEmpty(_MessageText) && _MessageText.Contains("{{") && _MessageText.Contains("}}")) ||
                (!string.IsNullOrEmpty(_MessageHtml) && _MessageHtml.Contains("{{") && _MessageHtml.Contains("}}"))) {
                
                _CanDeliver = false;
                _IsKeywordIssue = true;

            }

            xac = null;

            List<string> lst = new List<string>();

            fxnl = FdblXmlFunctions.GetNodeList(xe, "Address");
            if (fxnl != null && fxnl.Count > 0) {

                xac = fxnl.GetFirstNode().Attributes;

                if (xac != null) {

                    for (int i = 0; i < xac.Count; i++) {

                        lst.Add(xac[i].Name.ToLower());

                    }

                }

                fxnl = FdblXmlFunctions.GetNodeList(xe, "Flex");

                if (fxnl != null) {

                    for (int i = 0; i < fxnl.Count; i++) {

                        xe = fxnl.GetNode(i + 1);
                        XmlAttribute xa = null;

                        try {

                            xa = doc.CreateAttribute(xe.GetAttribute("Field"));
                            xa.Value = xe.GetAttribute("Value");

                        } catch {

                            xa = null;

                        }

                        if (xa != null && !lst.Contains(xa.Name.ToLower())) xac.Append(xa);

                    }

                }

                if (et.IsNewEmployee || et.IsNewEmployeePassword) {

                    int i = 0;

                    while (i < xac.Count) {

                        if (!xac[i].Name.Equals("EmployeeEmail", StringComparison.OrdinalIgnoreCase)) xac.RemoveAt(i);
                        else i++;

                    }

                }

                fxnl = FdblXmlFunctions.GetNodeList(xe, "Address");

                if (fxnl != null && fxnl.Count > 0) xac = fxnl.GetFirstNode().Attributes;

            }

            //if (xac == null || xac.Count == 0) _IsAddressIssue = true;

            _SendTo = string.Empty;
            _CopyTo = string.Empty;
            _BlindCopyTo = string.Empty;

            for (int i = 0; i < al.Count; i++) {

                AddressRecord ar = al[i];
                string address = string.Empty;

                if (!string.IsNullOrEmpty(ar.ExplicitValue)) address = ar.ExplicitValue;
                else if (!string.IsNullOrEmpty(ar.FieldName)) {

                    if (xac == null || xac.Count == 0) {

                        _IsAddressIssue = true;
                        _AddressIssues.Add(new AddressIssue(ar.FieldName));

                    } else {

                        try {

                            address = FdblXmlFunctions.GetAttributeValue(xac, ar.FieldName);

                        } catch (Exception ex) {

                            address = string.Empty;

                            _IsAddressIssue = true;
                            _AddressIssues.Add(new AddressIssue(ar.FieldName, ex.Message));

                        }

                    }

                }

                if (!string.IsNullOrEmpty(address)) {

                    if (ar.FieldType.Equals("to", StringComparison.OrdinalIgnoreCase)) {

                        if (_SendTo.Length > 0) _SendTo += ", ";

                        _SendTo += address;

                    } else if (ar.FieldType.Equals("cc", StringComparison.OrdinalIgnoreCase)) {

                        if (_CopyTo.Length > 0) _CopyTo += ", ";

                        _CopyTo += address;

                    } else if (ar.FieldType.Equals("bcc", StringComparison.OrdinalIgnoreCase)) {

                        if (_BlindCopyTo.Length > 0) _BlindCopyTo += ", ";

                        _BlindCopyTo += address;

                    }

                }

            }

            if (string.IsNullOrEmpty(_SendTo)) _SendTo = null;
            if (string.IsNullOrEmpty(_CopyTo)) _CopyTo = null;
            if (string.IsNullOrEmpty(_BlindCopyTo)) _BlindCopyTo = null;

            if (_SendTo == null && _CopyTo == null && _BlindCopyTo == null) _CanDeliver = false;

        }

        #endregion

        #region Properties

        public int EmailTemplateId { get { return _EmailTemplateId; } }
        public int UserId { get { return _UserId; } }
        public int AgentId { get { return _AgentId; } }
        public int EmployeeId { get { return _EmployeeId; } }
        public int I9Id { get { return _I9Id; } }
        public int FutureEmployeeId { get { return _FutureEmployeeId; } }

        public string FirstName { get { return _FirstName; } }
        public string LastName { get { return _LastName; } }

        public string BlindCopyTo { get { return _BlindCopyTo; } }
        public string CopyTo { get { return _CopyTo; } }
        public string MessageHtml { get { return _MessageHtml; } }
        public string MessageText { get { return _MessageText; } }
        public string SentFrom { get { return _SentFrom; } }
        public string SendTo { get { return _SendTo; } }
        public string Subject { get { return _Subject; } }
        
        public bool Delivered { 
            get { return _Delivered; } 
            set { _Delivered = value; } 
        }

        public bool CanDeliver { get { return _CanDeliver; } }
        public bool IsKeywordIssue { get { return _IsKeywordIssue; } }
        public bool IsAddressIssue { get { return _IsAddressIssue; } }

        public List<AddressIssue> AddressIssues { get { return _AddressIssues; } }

        #endregion

        #region Methods - Public

        public int GetId() {
            if (_UserId != -1) return _UserId;
            if (_AgentId != -1) return _AgentId;
            if (_EmployeeId != -1) return _EmployeeId;
            return -1;
        }

        #endregion

    }

}
