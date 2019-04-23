using System;
using System.Xml;

namespace Fdbl.Toolkit.Xml {

    public class FdblXmlFunctions {

        #region Methods - Public (Static)

        public static string GetAttributeValue(XmlElement node, string attributeName) {

            if (node == null) throw new ArgumentNullException("xml node is null");
            if (attributeName == null) throw new ArgumentNullException("xml node attribute name is null");

            if (attributeName.Trim().Length == 0) throw new ArgumentException("xml node attribute name is blank");

            return GetAttributeValue(node.Attributes, attributeName);

        }

        public static string GetAttributeValue(XmlAttributeCollection attributeList, string attributeName) {

            if (attributeList == null) throw new ArgumentNullException("xml attribute collection is null");
            if (attributeName == null) throw new ArgumentNullException("xml node attribute name is null");

            if (attributeName.Trim().Length == 0) throw new ArgumentException("xml node attribute name is blank");

            string attrName = attributeName.ToLower();

            for (int ndx = 0; ndx < attributeList.Count; ndx++) {

                if (attributeList[ndx].Name.ToLower().Equals(attrName)) return GetAttributeValue(attributeList[ndx]);

            }

            throw new ArgumentException(string.Format("xml attribute ({0}) not found in node", attributeName));

        }

        public static string GetAttributeValue(XmlAttribute attribute) {

            if (attribute == null) throw new ArgumentNullException("xml attribute node is null");

            if (attribute.Value == null || attribute.Value.Trim().Length == 0) return string.Empty;

            return attribute.Value;

        }

        public static FdblXmlNodeList GetNodeList(XmlElement node, string nodeName) {

            if (node == null) throw new ArgumentNullException("xml node is null");
            if (nodeName == null) throw new ArgumentNullException("xml node name is null");

            if (nodeName.Trim().Length == 0) throw new ArgumentException("xml node name is blank");

            if (node.IsEmpty) return null;

            System.Xml.XmlNodeList nodeList = (System.Xml.XmlNodeList)MaskNull(nodeName.IndexOf("/") == -1 ? node.GetElementsByTagName(nodeName) : node.SelectNodes(nodeName));
            if (nodeList.Count == 0) return null;

            return new FdblXmlNodeList(nodeList);

        }

        public static string GetNodeValue(XmlElement node) {

            if (node == null) throw new ArgumentNullException("xml node is null");

            if (node.HasChildNodes) {

                if (node.ChildNodes.Count == 1) {

                    XmlNode cNode = node.FirstChild;

                    if (cNode.Value != null) return cNode.Value;
                    if (cNode.InnerText != null) return cNode.InnerText;
                    if (cNode.InnerXml != null) return cNode.InnerXml;

                } else {

                    if (node.Value != null) return node.Value;
                    if (node.InnerText != null) return node.InnerText;
                    if (node.InnerXml != null) return node.InnerXml;

                }

            } else {

                if (node.Value != null) return node.Value;
                if (node.InnerText != null) return node.InnerText;
                if (node.InnerXml != null) return node.InnerXml;

            }

            throw new ArgumentException("xml node is invalid");

        }

        public static object MaskNull(object obj) {
            try {
                if (obj.Equals(System.DBNull.Value)) return null;
                return obj;
            } catch {
                return null;
            }
        }

        public static void SetAttributeValue(XmlAttribute attribute, string value) {

            if (attribute == null) throw new ArgumentNullException("xml attribute node is null");
            if (value == null) throw new ArgumentNullException("xml attribute value is null");

            attribute.Value = value;

        }

        public static void SetAttributeValue(XmlElement node, string attributeName, string attributeValue) {

            if (node == null) throw new ArgumentNullException("xml node is null");
            if (attributeName == null) throw new ArgumentNullException("xml node attribute name is null");
            if (attributeValue == null) throw new ArgumentNullException("xml attribute value is null");

            if (attributeName.Trim().Length == 0) throw new ArgumentException("xml node attribute name is blank");

            node.SetAttribute(attributeName, attributeValue);

        }

        public static void SetNodeValue(XmlElement node, string value) {

            if (node == null) throw new ArgumentNullException("xml node is null");
            if (value == null) throw new ArgumentNullException("xml value is null");

            node.Value = value;

        }

        #endregion

        #region Constructors

        private FdblXmlFunctions() { }

        #endregion

    }

}