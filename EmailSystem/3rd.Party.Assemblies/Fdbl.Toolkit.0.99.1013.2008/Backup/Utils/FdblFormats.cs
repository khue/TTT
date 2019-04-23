using System;
using System.IO;

namespace Fdbl.Toolkit.Utils {

    public class FdblFormats {

        #region Fields

        public readonly static string CrLf = string.Format("{0}{1}", (char)13, (char)10);
        public readonly static string NewLine = string.Format("{0}", (char)13);

        public readonly static string Backspace = string.Format("{0}", (char)8);
        public readonly static string Tab = string.Format("{0}", (char)9);

        public readonly static string DoubleQuote = string.Format("{0}", (char)34);
        public readonly static string SingleQuote = string.Format("{0}", (char)39);

        public readonly static string DateFormat = "MM-dd-yyyy";
        public readonly static string DateFormatMedium = "dd-MMM-yyyy";

        public readonly static string TimeFormat12 = "hh:mm:ss tt";
        public readonly static string TimeFormat24 = "HH:mm:ss";

        private static string _SqlLookFor = string.Format("{0}", (char)39);
        private static string _SqlReplaceWith = string.Format("{0}{0}", (char)39);

        #endregion

        #region Methods - Public (Static)

        public static string ElapsedTime(string start, string stop) {
            return ElapsedTime(Convert.ToDateTime(start), Convert.ToDateTime(stop));
        }

        public static string ElapsedTime(DateTime start, DateTime stop) {

            TimeSpan ts = stop - start;

            string elapsed = string.Empty;

            if (ts.Days > 0) elapsed += string.Format("{0} day{1} ", ts.Days, (ts.Days == 1 ? string.Empty : "s"));
            if (ts.Days > 0 || ts.Hours > 0) elapsed += string.Format("{0} hour{1} ", ts.Hours, (ts.Hours == 1 ? string.Empty : "s"));
            if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0) elapsed += string.Format("{0} minute{1} ", ts.Minutes, (ts.Minutes == 1 ? string.Empty : "s"));
            if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0) elapsed += string.Format("{0} second{1} ", ts.Seconds, (ts.Seconds == 1 ? string.Empty : "s"));
            if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 || ts.Milliseconds > 0) elapsed += string.Format("{0} millisecond{1}", ts.Milliseconds, (ts.Milliseconds == 1 ? string.Empty : "s"));

            return elapsed;

        }

        public static string ForFtp(string path) {

            if (path == null || path.Trim().Length == 0) return string.Empty;

            string oSep1 = string.Format("{0}", Path.DirectorySeparatorChar);
            string oSep2 = @"\";
            string sep = "/";
            string dSep = string.Format("{0}{0}", sep);

            path = path.Trim().Replace(oSep1, sep).Replace(oSep2, sep);
            while (path.IndexOf(dSep) != -1) path = path.Replace(dSep, sep);
            if (!path.EndsWith(sep)) path += sep;

            return path;

        }

        public static string ForOS(string path) {

            if (path == null || path.Trim().Length == 0) return string.Empty;

            string oSep1 = "/";
            string oSep2 = @"\";
            string sep = string.Format("{0}", Path.DirectorySeparatorChar);
            string dSep = string.Format("{0}{0}", sep);

            path = path.Trim().Replace(oSep1, sep).Replace(oSep2, sep);
            while (path.IndexOf(dSep) != -1) path = path.Replace(dSep, sep);
            if (path.StartsWith(sep)) path = sep + path;
            if (!File.Exists(path) && !path.EndsWith(sep)) path += sep;

            return path;

        }

        public static string ForSql(string[] strArray) {
            return ForSql(strArray, false);
        }

        public static string ForSql(string[] strArray, bool canBeNull) {
            string data = string.Empty;

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                if (data.Length > 0) data += "; ";
                data += strArray[ndx];
            }

            return ForSql(data, canBeNull);
        }

        public static string ForSql(string str) {
            return ForSql(str, false);
        }

        public static string ForSql(string str, bool canBeNull) {

            if (str == null || str.Trim().Length == 0) return canBeNull ? "NULL" : string.Format("{0}{0}", FdblFormats.SingleQuote);

            return string.Format("{0}{1}{0}", FdblFormats.SingleQuote, Utils.FdblStrings.ReplaceSubstring(str, _SqlLookFor, _SqlReplaceWith));

        }

        public static string ForWeb(string path) {

            if (path == null || path.Trim().Length == 0) return string.Empty;

            string oSep1 = string.Format("{0}", Path.DirectorySeparatorChar);
            string oSep2 = @"\";
            string sep = "/";
            string dSep = string.Format("{0}{0}", sep);

            path = path.Trim().Replace(oSep1, sep).Replace(oSep2, sep);
            while (path.IndexOf(dSep) != -1) path = path.Replace(dSep, sep);
            if (path.StartsWith(sep)) path = sep + path;
            if (!path.EndsWith(sep)) path += sep;

            return path;
        }

        #endregion

        #region Contructors

        private FdblFormats() { }

        #endregion

    }
}
