using System;
using System.IO;

namespace Fdbl.Toolkit.Utils {

    public class FdblExceptions {

        #region Methods - Public (Static)

        public static string GenerateFileName() {
            return GenerateFileName(null);
        }

        public static string GenerateFileName(string key) {
            if (key == null || key.Trim().Length == 0) return string.Format("{0}.Log.{1}.txt", DateTime.Now.ToString(FdblFormats.DateFormat), DateTime.Now.ToString(FdblFormats.TimeFormat24));
            return string.Format("{0}.Log.{1}.{2}.txt", DateTime.Now.ToString(FdblFormats.DateFormat), key, DateTime.Now.ToString(FdblFormats.TimeFormat24));
        }

        public static string GetMessages(Exception ex) {

            if (ex == null) return string.Empty;

            if (ex.Message.ToLower().Equals("rethrow")) ex = ex.InnerException;

            string details = ex.Message;

            Exception iex = ex = ex.InnerException;
            while (iex != null) {
                details += string.Format("{0}{1}", FdblFormats.CrLf, iex.Message);
                iex = iex.InnerException;
            }

            return details;
        }

        public static string GetDetails(Exception ex) {

            if (ex == null) return string.Empty;

            if (ex.Message.ToLower().Equals("rethrow")) ex = ex.InnerException;

            string details = string.Format("{0}{1}{1}Exception Message Trace{1}", ex.Message, FdblFormats.CrLf);

            Exception iex = ex.InnerException;
            while (iex != null) {
                details += string.Format("   {0}{1}", iex.Message, FdblFormats.CrLf);
                iex = iex.InnerException;
            }

            details += string.Format("{0}Stack Trace{0}{1}{0}{0}Root Stack Trace{0}", FdblFormats.CrLf, ex.StackTrace);

            iex = ex.InnerException;
            while (iex != null) {
                details += iex.StackTrace;
                iex = iex.InnerException;
                if (iex != null) details += FdblFormats.CrLf;
            }
            details += FdblFormats.CrLf;

            return details;
        }

        public static void WriteToFile(string fileName, Exception ex) {
            if (fileName == null) return;
            if (fileName.Trim().Length == 0) return;

            WriteToFile(new StreamWriter(fileName, true), ex);
        }

        public static void WriteToFile(StreamWriter sw, Exception ex) {
            if (sw == null) return;
            if (ex == null) return;

            using (sw) {
                sw.Write(GetDetails(ex));
                sw.Flush();
            }
        }

        #endregion

        #region Contructors

        private FdblExceptions() { }

        #endregion

    }

}
