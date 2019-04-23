using System;
using System.Xml;

namespace Fdbl.Toolkit.Xml {

    public class FdblXmlNodeList {

        #region Members

        private XmlNodeList _List;
        private int _Index;

        #endregion

        #region Constructors

        public FdblXmlNodeList() {
            _List = null;
            _Index = -1;
        }

        public FdblXmlNodeList(XmlNodeList nodeList) {

            if (nodeList == null) throw new ArgumentNullException("xml node list is null");

            _List = nodeList;
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

        public XmlNodeList List {
            get { return _List; }
        }

        #endregion

        #region Methods - Public

        public void Clear() {
            _List = null;
            _Index = -1;
        }

        public XmlElement GetCurrentNode() {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");

            return _GetNodeByIndex(_Index);

        }

        public XmlElement GetFirstNode() {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");

            _Index = 0;
            return _GetNodeByIndex(_Index);

        }

        public XmlElement GetNextNode() {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");

            return _GetNodeByIndex(++_Index);

        }

        public XmlElement GetNode(string nodeName) {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");
            if (nodeName == null) throw new ArgumentNullException("xml node name is null");

            if (nodeName.Trim().Length == 0) throw new ArgumentException("xml node name is blank");

            if (_List.Count < 1) return null;

            nodeName = nodeName.ToLower();
            for (int ndx = 0; ndx < _List.Count; ndx++) {
                if (_List[ndx].Name.ToLower().Equals(nodeName)) return (XmlElement)_List[ndx];
            }

            return null;

        }

        public XmlElement GetNode(int index) {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");

            if (index < 1) throw new IndexOutOfRangeException("index too small");
            if (index > _List.Count) throw new IndexOutOfRangeException("index too large");

            return GetNode(index, false);
        }

        public XmlElement GetNode(int index, bool updateInternalIndex) {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");

            if (index < 1) throw new IndexOutOfRangeException("index too small");
            if (index > _List.Count) throw new IndexOutOfRangeException("index too large");

            if (updateInternalIndex) _Index = (index - 1);

            return _GetNodeByIndex(index - 1);

        }

        public XmlElement GetLastNode() {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");

            _Index = _List.Count - 1;
            return _GetNodeByIndex(_Index);

        }

        public XmlElement GetPreviousNode() {

            if (_List == null) throw new FdblClassMemberNullException("xml node list is null");

            return _GetNodeByIndex(--_Index);

        }

        public void SetNodeList(XmlNodeList nodeList) {

            if (nodeList == null) throw new ArgumentNullException("xml node list is null");

            _List = nodeList;
            _Index = -1;

        }

        #endregion

        #region Methods - Private

        private XmlElement _GetNodeByIndex(int index) {

            if (_List.Count < 1) return null;
            if (index == -1) return null;
            if (index == _List.Count) return null;

            return (XmlElement)FdblXmlFunctions.MaskNull(_List.Item(index));
        }

        #endregion

    }

}