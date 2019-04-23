using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Fdbl.Toolkit.Xml {

    public class FdblXslTransformer {

        #region Methods - Public (Static)

        public static void FileStream(string xmlInputFileName, string xslFileName, string xmlOutputFileName) {
            FileStream(xmlInputFileName, xslFileName, xmlOutputFileName, null, false);
        }

        public static void FileStream(string xmlInputFileName, string xslFileName, string xmlOutputFileName, XsltArgumentList xsltArgList) {
            FileStream(xmlInputFileName, xslFileName, xmlOutputFileName, xsltArgList, false);
        }

        public static void FileStream(string xmlInputFileName, string xslFileName, string xmlOutputFileName, bool prettyPrint) {
            FileStream(xmlInputFileName, xslFileName, xmlOutputFileName, null, prettyPrint);
        }

        public static void FileStream(string xmlInputFileName, string xslFileName, string xmlOutputFileName, XsltArgumentList xsltArgList, bool prettyPrint) {

            if (xmlInputFileName == null) throw new ArgumentNullException("xml input file name is null");
            if (xslFileName == null) throw new ArgumentNullException("xsl input file name is null");
            if (xmlOutputFileName == null) throw new ArgumentNullException("xml output file name is null");

            if (xmlInputFileName.Trim().Length == 0) throw new ArgumentException("xml input file name is blank");
            if (xslFileName.Trim().Length == 0) throw new ArgumentException("xsl input file name is blank");
            if (xmlOutputFileName.Trim().Length == 0) throw new ArgumentException("xml output file name is blank");

            if (!File.Exists(xmlInputFileName)) throw new FileNotFoundException("xml input file does not exist");
            if (!File.Exists(xslFileName)) throw new FileNotFoundException("xsl input file does not exist");

            Stream fout = null;
            XmlTextWriter writer = null;
            string tmpFile = string.Format("{0}{1}", xmlOutputFileName, (prettyPrint ? ".tmp" : string.Empty));
            bool failed = true;

            try {

                XPathDocument xml = new XPathDocument(xmlInputFileName);
                XslCompiledTransform xsl = new XslCompiledTransform();
                xsl.Load(xslFileName);
                fout = new FileStream(tmpFile, FileMode.Create, FileAccess.ReadWrite);
                xsl.Transform(xml, xsltArgList, fout);
                failed = false;

            } finally {

                if (fout != null) {
                    fout.Flush();
                    fout.Close();
                }

                if (failed && File.Exists(tmpFile)) File.Delete(tmpFile);

            }

            if (!prettyPrint) return;

            try {

                System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                writer = new XmlTextWriter(xmlOutputFileName, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                xml.Load(tmpFile);
                xml.WriteContentTo(writer);

            } finally {

                if (writer != null) {
                    writer.Flush();
                    writer.Close();
                }

                if (File.Exists(tmpFile)) File.Delete(tmpFile);
                if (failed && File.Exists(xmlOutputFileName)) File.Delete(xmlOutputFileName);

            }

        }

        public static void TextWriter(string xmlInputFileName, string xslFileName, string xmlOutputFileName) {
            TextWriter(xmlInputFileName, xslFileName, xmlOutputFileName, null);
        }

        public static void TextWriter(string xmlInputFileName, string xslFileName, string xmlOutputFileName, XsltArgumentList xsltArgList) {

            if (xmlInputFileName == null) throw new ArgumentNullException("xml input file name is null");
            if (xslFileName == null) throw new ArgumentNullException("xsl input file name is null");
            if (xmlOutputFileName == null) throw new ArgumentNullException("xml output file name is null");

            if (xmlInputFileName.Trim().Length == 0) throw new ArgumentException("xml input file name is blank");
            if (xslFileName.Trim().Length == 0) throw new ArgumentException("xsl input file name is blank");
            if (xmlOutputFileName.Trim().Length == 0) throw new ArgumentException("xml output file name is blank");

            if (!File.Exists(xmlInputFileName)) throw new FileNotFoundException("xml input file does not exist");
            if (!File.Exists(xslFileName)) throw new FileNotFoundException("xsl input file does not exist");

            XmlTextWriter writer = null;
            bool failed = true;

            try {

                XPathDocument xml = new XPathDocument(xmlInputFileName);
                XslCompiledTransform xsl = new XslCompiledTransform();
                xsl.Load(xslFileName);
                writer = new XmlTextWriter(xmlOutputFileName, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                xsl.Transform(xml, xsltArgList, writer);
                failed = false;

            } finally {

                if (writer != null) {
                    writer.Flush();
                    writer.Close();
                }

                if (failed && File.Exists(xmlOutputFileName)) File.Delete(xmlOutputFileName);

            }

        }

        #endregion

        #region Contructors

        private FdblXslTransformer() { }

        #endregion
    }

}