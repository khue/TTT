using System;
using System.IO;
using System.Xml;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Xml {

    internal class ProcessWriter {

        #region Constructors

        private ProcessWriter() { }

        #endregion

        #region Methods - Public (Static)

        public static void WriteEntry(int idTransaction, string xmlRequest, string xmlResponse, string logFile) {

            try {

                bool nf = !File.Exists(logFile);

                FileStream fs = new FileStream(logFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                XmlTextWriter xtw = new XmlTextWriter(sw);

                xtw.Formatting = Formatting.Indented;
                xtw.IndentChar = (char)0x9;
                xtw.Indentation = 2;

                if (nf) {

                    xtw.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
                    xtw.WriteRaw("<USCISProcess>");

                } else sw.BaseStream.Seek(-15, SeekOrigin.End);

                xtw.WriteStartElement("Transaction");

                xtw.WriteAttributeString("Id ", Convert.ToString(idTransaction));
                xtw.WriteAttributeString("Status ", "pending");

                xtw.WriteRaw("\n");
                xtw.WriteRaw(xmlRequest);
                xtw.WriteRaw("\n");
                xtw.WriteRaw(xmlResponse);
                xtw.WriteRaw("\n");

                xtw.WriteEndElement();

                xtw.WriteRaw("\n</USCISProcess>");

                xtw.Flush();
                xtw.Close();

            } catch { }

        }

        #endregion

    }

}