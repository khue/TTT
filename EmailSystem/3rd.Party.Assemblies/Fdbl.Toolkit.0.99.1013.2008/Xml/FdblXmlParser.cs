using System;
using System.Xml;

namespace Fdbl.Toolkit.Xml {

    #region Enumerations

    [FlagsAttribute]
    public enum ParserLoadFrom : byte {
        File,
        Buffer
    }

    #endregion

    public class FdblXmlParser {

        #region Memebers

        private XmlDocument _XmlDocument;
        private string _FileName;

        #endregion

        #region Methods - Public (Static)

        public static object MaskNull(object obj) {
            try {
                if (obj.Equals(System.DBNull.Value)) return null;
                return obj;
            } catch {
                return null;
            }
        }

        #endregion

        #region Constructors

        public FdblXmlParser(XmlDocument xmlDocument) {

            _XmlDocument = xmlDocument;

        }

        public FdblXmlParser(string fileOrBuffer, ParserLoadFrom loadFrom) {

            if (fileOrBuffer == null) throw new ArgumentNullException("file or buffer value is null");

            if (fileOrBuffer.ToCharArray().Length == 0) throw new ArgumentException("file or buffer value is blank");

            _XmlDocument = new XmlDocument();
            _FileName = string.Empty;

            if (loadFrom == ParserLoadFrom.File) {

                _FileName = fileOrBuffer;
                _XmlDocument.Load(_FileName);

            } else if (loadFrom == ParserLoadFrom.Buffer) {

                _XmlDocument.LoadXml(fileOrBuffer);

            }

        }

        #endregion

        #region Properties - Public

        public XmlDocument Document {
            get { return _XmlDocument; }
        }

        public string FileName {
            get { return _FileName; }
        }

        public string Xml {
            get { return _XmlDocument == null ? string.Empty : _XmlDocument.OuterXml; }
        }

        #endregion

        #region Methods - Public

        public FdblXmlNodeList GetNodeList(string nodeName) {

            XmlNodeList nodeList = (XmlNodeList)FdblXmlParser.MaskNull(nodeName.IndexOf("/") == -1 ? _XmlDocument.GetElementsByTagName(nodeName) : _XmlDocument.SelectNodes(nodeName));

            if (nodeList.Count == 0) return null;

            return new FdblXmlNodeList(nodeList);

        }

        #endregion

    }

}