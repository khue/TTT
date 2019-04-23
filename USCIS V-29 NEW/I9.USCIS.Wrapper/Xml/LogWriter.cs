using System;
using System.IO;
using System.Xml;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Xml {

    internal class LogWriter {

        #region Constructors

        private LogWriter() { }

        #endregion

        #region Methods - Public (Static)

        public static void WriteEntry(WebService.ProcessIds pids, LogLevel logLevel, string classModule, string friendlyMessage, string message, Exception ex, Exception iex, string logFile) {

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
                    xtw.WriteRaw("<USCISLog>");

                } else sw.BaseStream.Seek(-11, SeekOrigin.End);

                xtw.WriteStartElement("Capture");

                xtw.WriteStartElement("Entity");

                if (pids != null) {

                    if (pids.TransactionId != WebService.Request.NullId) xtw.WriteAttributeString("TransactionId", Convert.ToString(pids.TransactionId));
                    if (pids.RequestId != WebService.Request.NullId) xtw.WriteAttributeString("RequestId", Convert.ToString(pids.RequestId));
                    if (pids.ResponseId != WebService.Request.NullId) xtw.WriteAttributeString("ResponseId", Convert.ToString(pids.ResponseId));
                    if (pids.QueueErrorId != WebService.Request.NullId) xtw.WriteAttributeString("QueueErrorId", Convert.ToString(pids.QueueErrorId));
                    if (pids.QueueFutureId != WebService.Request.NullId) xtw.WriteAttributeString("QueueFutureId", Convert.ToString(pids.QueueFutureId));
                    if (pids.ConsoleId != WebService.Request.NullId) xtw.WriteAttributeString("ConsoleId", Convert.ToString(pids.ConsoleId));
                    if (pids.CategoryId != USCISCategoryId.Unknown) xtw.WriteAttributeString("CategoryId", Convert.ToString((int)pids.CategoryId));
                    if (pids.SystemId != USCISSystemId.Unknown) xtw.WriteAttributeString("SystemId", Convert.ToString((int)pids.SystemId));

                }

                xtw.WriteAttributeString("DateTime", Convert.ToString(DateTime.Now));
                xtw.WriteAttributeString("Level", Convert.ToString(logLevel));
                xtw.WriteAttributeString("ClassModule", classModule);
                xtw.WriteAttributeString("FriendlyMessage", friendlyMessage);
                xtw.WriteAttributeString("Message", message);
                xtw.WriteRaw(FdblExceptions.GetDetails(ex));

                xtw.WriteEndElement();

                if (iex != null) {

                    xtw.WriteStartElement("Exception");
                    xtw.WriteRaw(FdblExceptions.GetDetails(iex));
                    xtw.WriteEndElement();

                }

                xtw.WriteEndElement();

                xtw.WriteRaw("\n</USCISLog>");

                xtw.Flush();
                xtw.Close();

            } catch { }

        }

        #endregion

    }

}