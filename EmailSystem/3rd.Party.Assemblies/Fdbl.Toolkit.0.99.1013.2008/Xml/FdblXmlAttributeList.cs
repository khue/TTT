using System;
using System.Xml;

namespace Fdbl.Toolkit.Xml {

    public class FdblXmlAttributeList {

        #region Members

        private XmlAttributeCollection _List;
        private int _Index;

        #endregion

        #region Constructors

        public FdblXmlAttributeList() {
            _List = null;
            _Index = -1;
        }

        public FdblXmlAttributeList(XmlAttributeCollection attrList) {

            if (attrList == null) throw new ArgumentNullException("xml attribute list is null");

            _List = attrList;
            _Index = -1;

        }

        #endregion

        #region Properties - Public

        public int Count {
            get { return _List.Count; }
        }

        public int Index {
            get { return _Index; }
        }

        public XmlAttributeCollection List {
            get { return _List; }
        }

        #endregion

        #region Methods - Public

        public void Clear() {
            _List = null;
            _Index = -1;
        }

        public XmlAttribute GetAttribute(string attributeName) {

            if (attributeName == null) throw new ArgumentNullException("attributeName is null");
            if (attributeName.Trim().Length == 0) throw new ArgumentException("attributeName is blank");

            if (_List == null) throw new FdblClassMemberNullException("xml attribute list is null");

            attributeName = attributeName.ToLower();
            for (int ndx = 0; ndx < _List.Count; ndx++) {
                if (_List[ndx].Name.ToLower().Equals(attributeName)) return _List[ndx];
            }

            return null;

        }

        public XmlAttribute GetCurrentAttribute() {

            if (_List == null) throw new FdblClassMemberNullException("xml attribute list is null");

            return _GetAttribute(_Index);

        }

        public XmlAttribute GetFirstAttribute() {

            if (_List == null) throw new FdblClassMemberNullException("xml attribute list is null");

            _Index = 0;
            return _GetAttribute(_Index);

        }

        public XmlAttribute GetNextAttribute() {

            if (_List == null) throw new FdblClassMemberNullException("xml attribute list is null");

            return _GetAttribute(++_Index);

        }

        public XmlAttribute GetLastAttribute() {
            if (_List == null) throw new FdblClassMemberNullException("xml attribute list is null");

            _Index = _List.Count;
            return _GetAttribute(_Index);

        }

        public XmlAttribute GetPreviousAttribute() {

            if (_List == null) throw new FdblClassMemberNullException("xml attribute list is null");

            return _GetAttribute(--_Index);

        }

        public void SetAttributeList(XmlAttributeCollection attrList) {

            if (attrList == null) throw new ArgumentNullException("xml attribute list is null");

            _List = attrList;
            _Index = -1;

        }

        #endregion

        #region Methods - Private

        private XmlAttribute _GetAttribute(int index) {

            if (_List.Count < 1) return null;
            if (index == -1) return null;
            if (index == _List.Count) return null;

            XmlAttribute attribute = _List[index];

            if (attribute == null) return null;
            if (attribute.Value == null) return null;

            return attribute;

        }

        #endregion

    }

}