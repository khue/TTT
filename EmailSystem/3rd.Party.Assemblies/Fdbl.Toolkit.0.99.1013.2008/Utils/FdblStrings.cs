using System;
using System.Security.Cryptography;
using System.Text;

namespace Fdbl.Toolkit.Utils {

    public class FdblStrings {

        #region Methods - Public (Static)

        public static bool Contains(string lookIn, int lookFor) {
            return Contains(lookIn, Convert.ToString((char)lookFor), true);
        }

        public static bool Contains(string lookIn, int lookFor, bool isCaseSensitive) {
            return Contains(lookIn, Convert.ToString((char)lookFor), isCaseSensitive);
        }

        public static bool Contains(string lookIn, char lookFor) {
            return Contains(lookIn, Convert.ToString(lookFor), true);
        }

        public static bool Contains(string lookIn, char lookFor, bool isCaseSensitive) {
            return Contains(lookIn, Convert.ToString(lookFor), isCaseSensitive);
        }

        public static bool Contains(string lookIn, string lookFor) {
            return Contains(lookIn, lookFor, true);
        }

        public static bool Contains(string lookIn, string lookFor, bool isCaseSensitive) {

            if (!isCaseSensitive) {
                lookIn = lookIn.ToLower();
                lookFor = lookFor.ToLower();
            }

            return lookIn.IndexOf(lookFor) != -1;
        }

        public static string ConvertFromBase64(string str) {

            if (str == null) throw new ArgumentNullException("string is null");
            if (str.Trim().Length == 0) throw new ArgumentException("string is blank");

            UnicodeEncoding ue = new UnicodeEncoding();

            return ue.GetString(Convert.FromBase64String(str));

        }

        public static string ConvertToBase64(string str) {

            if (str == null) throw new ArgumentNullException("string is null");
            if (str.Trim().Length == 0) throw new ArgumentException("string is blank");

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));

        }

        public static string[] Explode(string str) {
            return Explode(str, " ,;", false);
        }

        public static string[] Explode(string str, string separator) {
            return Explode(str, separator, false);
        }

        public static string[] Explode(string str, string separator, bool removeNulls) {

            if (str == null || str.Length == 0) return ToArray(string.Empty);
            if (separator == null || separator.Length == 0) separator = " ,;";

            string[] strArray = str.Split(separator.ToCharArray());

            if (!removeNulls) return strArray;

            int idx = 0;

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                if (strArray[ndx] != null && strArray[ndx].Trim().Length > 0) idx++;
            }

            string[] zipped = new string[idx];

            idx = 0;
            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                if (strArray[ndx] != null && strArray[ndx].Trim().Length > 0) zipped[idx++] = strArray[ndx];
            }

            return zipped;
        }

        public static string Implode(string[] strArray) {
            return Implode(strArray, " ");
        }

        public static string Implode(string[] strArray, int separator) {
            return Implode(strArray, Convert.ToString((char)separator));
        }

        public static string Implode(string[] strArray, char separator) {
            return Implode(strArray, Convert.ToString(separator));
        }

        public static string Implode(string[] strArray, string separator) {
            string joined = string.Empty;

            if (separator == null || separator.Trim().Length == 0) separator = " ";

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                if (joined.Length > 0) joined += separator;
                joined += strArray[ndx];
            }

            return joined;
        }

        public static int IndexOf(string lookIn, int lookFor) {
            return IndexOf(lookIn, Convert.ToString((char)lookFor), 0, true);
        }

        public static int IndexOf(string lookIn, int lookFor, int startIndex) {
            return IndexOf(lookIn, Convert.ToString((char)lookFor), startIndex, true);
        }

        public static int IndexOf(string lookIn, int lookFor, int startIndex, bool isCaseSensitive) {
            return IndexOf(lookIn, Convert.ToString((char)lookFor), startIndex, isCaseSensitive);
        }

        public static int IndexOf(string lookIn, char lookFor) {
            return IndexOf(lookIn, Convert.ToString(lookFor), 0, true);
        }

        public static int IndexOf(string lookIn, char lookFor, int startIndex) {
            return IndexOf(lookIn, Convert.ToString(lookFor), startIndex, true);
        }

        public static int IndexOf(string lookIn, char lookFor, int startIndex, bool isCaseSensitive) {
            return IndexOf(lookIn, Convert.ToString(lookFor), startIndex, isCaseSensitive);
        }

        public static int IndexOf(string lookIn, string lookFor) {
            return IndexOf(lookIn, lookFor, 0, true);
        }

        public static int IndexOf(string lookIn, string lookFor, int startIndex) {
            return IndexOf(lookIn, lookFor, startIndex, true);
        }

        public static int IndexOf(string lookIn, string lookFor, int startIndex, bool isCaseSensitive) {
            if (!isCaseSensitive) {
                lookIn = lookIn.ToLower();
                lookFor = lookFor.ToLower();
            }
            return lookIn.IndexOf(lookFor, startIndex);
        }

        public static bool IsBlank(string str) {
            if (str == null || str.Trim().Length == 0 || str == string.Empty) return true;
            return false;
        }

        public static string Left(string str, string lookFor) {

            if (str == null || str.Length == 0) return string.Empty;
            if (lookFor == null || lookFor.Length == 0) return string.Empty;

            return Left(str, str.IndexOf(lookFor));
        }

        public static string[] Left(string[] strArray, string lookFor) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (lookFor == null || lookFor.Length == 0) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = Left(strArray[ndx], strArray[ndx].IndexOf(lookFor));
            }

            return strArray;
        }

        public static string Left(string str, int length) {

            if (str == null || str.Length == 0) return string.Empty;
            if (length < 1) return string.Empty;

            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static string[] Left(string[] strArray, int length) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (length < 1) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = Left(strArray[ndx], length);
            }

            return strArray;
        }

        public static string LeftBack(string str, string lookFor) {

            if (str == null || str.Length == 0) return string.Empty;
            if (lookFor == null || lookFor.Length == 0) return string.Empty;

            int ndx = str.LastIndexOf(lookFor);
            if (ndx == -1) return string.Empty;

            return LeftBack(str, str.Length - (ndx + lookFor.Length - 1));
        }

        public static string[] LeftBack(string[] strArray, string lookFor) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (lookFor == null || lookFor.Length == 0) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                int idx = strArray[ndx].LastIndexOf(lookFor);
                strArray[ndx] = idx == -1 ? string.Empty : LeftBack(strArray[ndx], strArray[ndx].Length - (idx + lookFor.Length) - 1);
            }

            return strArray;
        }

        public static string LeftBack(string str, int length) {

            if (str == null || str.Length == 0) return string.Empty;
            if (length < 1) return string.Empty;
            if (length >= str.Length) return str;

            return str.Substring(0, str.Length - length);
        }

        public static string[] LeftBack(string[] strArray, int length) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (length < 1) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = LeftBack(strArray[ndx], length);
            }

            return strArray;
        }

        public static string Repeat(int howMany) {
            return Repeat(' ', howMany);
        }

        public static string Repeat(int repeatingCharacter, int howMany) {
            return Repeat(Convert.ToChar(repeatingCharacter), howMany);
        }

        public static string Repeat(char repeatingCharacter, int howMany) {

            if (howMany < 1) return string.Empty;

            return string.Empty.PadLeft(howMany, repeatingCharacter);
        }

        public static string Replace(string source, string lookFor, string replaceWith) {
            return Replace(source, ToArray(lookFor), ToArray(replaceWith), true);
        }

        public static string Replace(string source, string lookFor, string replaceWith, bool isCaseSensitive) {
            return Replace(source, ToArray(lookFor), ToArray(replaceWith), isCaseSensitive);
        }

        public static string Replace(string source, string[] lookFor, string[] replaceWith) {
            return Replace(source, lookFor, replaceWith, true);
        }

        public static string Replace(string source, string[] lookFor, string[] replaceWith, bool isCaseSensitive) {

            if (lookFor.Length == 0) return string.Empty;

            string[] balancedReplaceWith = _BalanceArray(lookFor, replaceWith);

            for (int ndx = 0; ndx < lookFor.Length; ndx++) {
                if (isCaseSensitive)
                    source = source.Equals(lookFor[ndx]) ? balancedReplaceWith[ndx] : source;
                else
                    source = source.ToLower().Equals(lookFor[ndx].ToLower()) ? balancedReplaceWith[ndx] : source;
            }

            return source;
        }

        public static string[] Replace(string[] source, string lookFor, string replaceWith) {
            return Replace(source, ToArray(lookFor), ToArray(replaceWith), true);
        }

        public static string[] Replace(string[] source, string lookFor, string replaceWith, bool isCaseSensitive) {
            return Replace(source, ToArray(lookFor), ToArray(replaceWith), isCaseSensitive);
        }

        public static string[] Replace(string[] source, string[] lookFor, string[] replaceWith) {
            return Replace(source, lookFor, replaceWith, true);
        }

        public static string[] Replace(string[] source, string[] lookFor, string[] replaceWith, bool isCaseSensitive) {

            if (source.Length == 0) return ToArray(string.Empty);
            if (lookFor.Length == 0) return ToArray(string.Empty);

            string[] balancedReplaceWith = _BalanceArray(lookFor, replaceWith);

            for (int outer = 0; outer < source.Length; outer++) {
                for (int inner = 0; inner < lookFor.Length; inner++) {
                    if (isCaseSensitive)
                        source[outer] = source[outer].Equals(lookFor[inner]) ? balancedReplaceWith[inner] : source[outer];
                    else
                        source[outer] = source[outer].ToLower().Equals(lookFor[inner].ToLower()) ? balancedReplaceWith[inner] : source[outer];
                }
            }

            return source;
        }

        public static string ReplaceSubstring(string str, string lookFor, string replaceWith, bool isCaseSensitive) {

            if (str == null || str.Length == 0) return string.Empty;
            if (lookFor == null || lookFor.Length == 0) return str;
            if (replaceWith == null) return str;

            if (isCaseSensitive) return str.Replace(lookFor, replaceWith);

            int ndx = 0;
            int idx = -1;

            while ((idx = IndexOf(str, lookFor, ndx, false)) != -1) {
                str = string.Format("{0}{1}{2}", str.Substring(0, idx), replaceWith, str.Substring(idx + lookFor.Length));
            }

            return str;
        }

        public static string ReplaceSubstring(string str, string lookFor, string replaceWith) {
            return ReplaceSubstring(str, lookFor, replaceWith, true);
        }

        public static string[] ReplaceSubstring(string[] strArray, string lookFor, string replaceWith, bool isCaseSensitive) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (lookFor == null || lookFor.Length == 0) return strArray;
            if (replaceWith == null || replaceWith.Length == 0) return strArray;

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = ReplaceSubstring(strArray[ndx], lookFor, replaceWith, isCaseSensitive);
            }

            return strArray;
        }

        public static string[] ReplaceSubstring(string[] strArray, string lookFor, string replaceWith) {
            return ReplaceSubstring(strArray, lookFor, replaceWith, true);
        }

        public static string ReplaceSubstring(string str, string[] lookFor, string[] replaceWith, bool isCaseSensitive) {

            if (str == null || str.Length == 0) return string.Empty;
            if (lookFor == null || lookFor.Length == 0) return str;

            string[] tmpReplaceWith = _BalanceArray(lookFor, replaceWith);

            for (int ndx = 0; ndx < lookFor.Length; ndx++) {
                str = ReplaceSubstring(str, lookFor[ndx], tmpReplaceWith[ndx], isCaseSensitive);
            }

            return str;
        }

        public static string ReplaceSubstring(string str, string[] lookFor, string[] replaceWith) {
            return ReplaceSubstring(str, lookFor, replaceWith, true);
        }

        public static string[] ReplaceSubstring(string[] strArray, string[] lookFor, string[] replaceWith, bool isCaseSensitive) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (lookFor == null || lookFor.Length == 0) return strArray;

            string[] tmpReplaceWith = _BalanceArray(lookFor, replaceWith);

            for (int ndx1 = 0; ndx1 < lookFor.Length; ndx1++) {
                for (int ndx2 = 0; ndx2 < strArray.Length; ndx2++) {
                    strArray[ndx2] = ReplaceSubstring(strArray[ndx2], lookFor[ndx1], replaceWith[ndx1], isCaseSensitive);
                }
            }

            return strArray;
        }

        public static string[] ReplaceSubstring(string[] strArray, string[] lookFor, string[] replaceWith) {
            return ReplaceSubstring(strArray, lookFor, replaceWith, true);
        }

        public static string Right(string str, string lookFor) {

            if (str == null || str.Length == 0) return string.Empty;
            if (lookFor == null || lookFor.Length == 0) return string.Empty;

            int ndx = str.IndexOf(lookFor);

            if (ndx == -1) return string.Empty;

            return Right(str, str.Length - (ndx + lookFor.Length));

        }

        public static string[] Right(string[] strArray, string lookFor) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (lookFor == null || lookFor.Length == 0) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                int idx = strArray[ndx].IndexOf(lookFor);
                strArray[ndx] = idx == -1 ? string.Empty : Right(strArray[ndx], strArray[ndx].Length - (idx + lookFor.Length));
            }

            return strArray;
        }

        public static string Right(string str, int length) {

            if (str == null || str.Length == 0) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > str.Length) return str;

            return str.Substring(str.Length - length);
        }

        public static string[] Right(string[] strArray, int length) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (length < 1) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = Right(strArray[ndx], length);
            }

            return strArray;
        }

        public static string RightBack(string str, string lookFor) {

            if (str == null || str.Length == 0) return string.Empty;
            if (lookFor == null || lookFor.Length == 0) return string.Empty;

            return RightBack(str, str.LastIndexOf(lookFor) + 1);
        }

        public static string[] RightBack(string[] strArray, string lookFor) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (lookFor == null || lookFor.Length == 0) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = RightBack(strArray[ndx], strArray[ndx].LastIndexOf(lookFor) + 1);
            }

            return strArray;
        }

        public static string RightBack(string str, int length) {

            if (str == null || str.Length == 0) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > str.Length) return str;

            return str.Substring(length);
        }

        public static string[] RightBack(string[] strArray, int length) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (length < 1) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = RightBack(strArray[ndx], length);
            }

            return strArray;
        }

        public static string RemoveLeft(string str, int count) {

            if (str == null || str.Length == 0) return string.Empty;
            if (count > str.Length) return string.Empty;
            if (count < 1) return str;

            return str.Substring(count);

        }

        public static string[] RemoveLeft(string[] strArray, int count) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (count < 1) return strArray;

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = RemoveLeft(strArray[ndx], count);
            }

            return strArray;
        }

        public static string RemoveRight(string str, int count) {

            if (str == null || str.Length == 0) return string.Empty;
            if (count > str.Length) return string.Empty;
            if (count < 1) return str;

            return str.Substring(0, str.Length - count);

        }

        public static string[] RemoveRight(string[] strArray, int count) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);
            if (count < 1) return strArray;

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = RemoveRight(strArray[ndx], count);
            }

            return strArray;
        }

        public static string[] ToArray(string str) {
            string[] ary = { str };

            return ary;
        }

        public static string ToLower(string str) {

            if (str == null || str.Length == 0) return string.Empty;

            return str.ToLower();
        }

        public static string[] ToLower(string[] strArray) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = ToLower(strArray[ndx]);
            }

            return strArray;
        }

        public static string ToProper(string str) {

            if (str == null || str.Length == 0) return string.Empty;

            string[] parts = Explode(str.ToLower(), " ");

            for (int ndx = 0; ndx < parts.Length; ndx++) {
                if (parts[ndx].Length > 0) parts[ndx] = Left(parts[ndx], 1).ToUpper() + Right(parts[ndx], parts[ndx].Length - 1);
            }

            return Implode(parts, string.Empty);
        }

        public static string[] ToProper(string[] strArray) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = ToProper(strArray[ndx]);
            }

            return strArray;
        }

        public static string ToUpper(string str) {

            if (str == null || str.Length == 0) return string.Empty;

            return str.ToUpper();
        }

        public static string[] ToUpper(string[] strArray) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = ToUpper(strArray[ndx]);
            }

            return strArray;
        }

        public static string Trim(string str) {

            if (str == null || str.Trim().Length == 0) return string.Empty;

            str = str.Trim().Replace("  ", " ");

            return str;
        }

        public static string[] Trim(string[] strArray) {
            return Trim(strArray, false);
        }

        public static string[] Trim(string[] strArray, bool removeNulls) {

            if (strArray == null || strArray.Length == 0) return ToArray(string.Empty);

            int idx = 0;
            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                strArray[ndx] = Trim(strArray[ndx]);
                if (strArray[ndx].Length > 0) idx++;
            }

            if (!removeNulls) return strArray;

            string[] zipped = new string[idx];
            idx = 0;

            for (int ndx = 0; ndx < strArray.Length; ndx++) {
                if (strArray[ndx].Length > 0) zipped[idx++] = strArray[ndx];
            }

            return zipped;
        }

        public static string[] Unique(string[] lookIn, bool removeNulls) {

            if (lookIn == null || lookIn.Length == 0) return ToArray(string.Empty);

            string list = string.Empty;
            int idx = 0;

            for (int ndx = 0; ndx < lookIn.Length; ndx++) {
                if (list.IndexOf("{»" + lookIn[ndx] + "«}") == -1) {
                    list += "{»" + lookIn[ndx] + "«}";
                    idx++;
                } else {
                    lookIn[ndx] = string.Empty;
                }
            }

            if (!removeNulls) return lookIn;

            string[] zipped = new string[idx];
            idx = 0;

            for (int ndx = 0; ndx < lookIn.Length; ndx++) {
                if (lookIn[ndx].Length > 0) zipped[idx++] = lookIn[ndx];
            }

            return zipped;
        }

        public static string Word(string lookIn, string lookFor, int position) {

            if (lookIn == null || lookIn.Length == 0) return string.Empty;
            if (lookFor == null || lookFor.Length == 0) return lookIn;
            if (position == 0) return lookIn;

            string[] strArray = lookIn.Split(lookFor.ToCharArray());

            position = position < 0 ? strArray.Length + position : position - 1;

            if (position < 0 || position >= strArray.Length) return string.Empty;

            return strArray[position];
        }

        public static string[] Word(string[] lookIn, string lookFor, int position) {

            if (lookIn == null || lookIn.Length == 0) return ToArray(string.Empty);
            if (lookFor == null || lookFor.Length == 0) return lookIn;
            if (position == 0) return lookIn;

            for (int ndx = 0; ndx < lookIn.Length; ndx++) {
                lookIn[ndx] = Word(lookIn[ndx], lookFor, position);
            }

            return lookIn;
        }

        #endregion

        #region Contructors

        private FdblStrings() { }

        #endregion

        #region Methods - Private (Static)

        private static string[] _BalanceArray(string[] master, string[] slave) {
            string[] tmpSlave;

            if (slave.Length > master.Length) {

                tmpSlave = new string[master.Length];
                Array.Copy(slave, tmpSlave, master.Length);

            } else if (slave.Length < master.Length) {

                tmpSlave = new string[master.Length];
                for (int ndx = 0; ndx < tmpSlave.Length; ndx++) {
                    tmpSlave[ndx] = string.Empty;
                }
                Array.Copy(slave, tmpSlave, slave.Length);

            } else {

                tmpSlave = new string[slave.Length];
                Array.Copy(slave, tmpSlave, slave.Length);

            }

            return tmpSlave;
        }

        #endregion

    }

}
