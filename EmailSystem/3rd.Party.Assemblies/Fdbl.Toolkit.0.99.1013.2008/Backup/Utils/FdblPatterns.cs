using System;
using System.IO;
using System.Text;

namespace Fdbl.Toolkit.Utils {

    /**
     * This class was taken from the internal class PatternMatcher 
     * within the microsoft System.IO namespace.
     */
    public class FdblPatterns {

        #region Constants

        private const char ANSI_DOS_QM = '<';
        private const char ANSI_DOS_STAR = '>';
        private const char DOS_DOT = '"';
        private const int MATCHES_ARRAY_SIZE = 0x10;

        #endregion

        #region Method - Public (Static)

        public static bool IsMatch(string name, string filter) {

            if (name == null || name.Trim().Length == 0) return false;
            if (filter == null || filter.Trim().Length == 0) return false;

            if (filter.Equals("*.*")) return true;
            if (filter.Equals("*")) return true;

            int len = filter.Length;
            int ndx = filter.LastIndexOf('*');

            StringBuilder sb = new StringBuilder(filter);
            if (ndx == (len - 1)) sb.Append(".*");

            for (int pos = 0; pos < len; pos++) {

                if (sb[pos] == '*') {
                    sb[pos] = '>';
                } else if (sb[pos] == '?') {
                    sb[pos] = '<';
                } else if (sb[pos] == '.') {
                    sb[pos] = '"';
                } else {
                    if (sb[pos] == Path.DirectorySeparatorChar) throw new ArgumentException(string.Format("The matching filter can not contain the '{0}' character", Path.DirectorySeparatorChar));
                    sb[pos] = char.ToLower(sb[pos]);
                }
            }
            return FdblPatterns.IsStrictMatch(name.ToLower(), sb.ToString());
        }

        public static bool IsStrictMatch(string name, string filter) {

            if (name == null || name.Trim().Length == 0) return false;
            if (filter == null || filter.Trim().Length == 0) return false;

            if (filter.Equals("*.*")) return true;
            if (filter.Equals("*")) return true;

            int num9;
            char ch1 = '\0';
            char ch2 = '\0';
            int[] numArray1 = new int[0x10];
            int[] numArray2 = new int[0x10];
            bool flag1 = false;

            if ((filter[0] == '*') && (filter.IndexOf('*', 1) == -1)) {
                int fLen = filter.Length - 1;
                if ((name.Length >= fLen) && (string.Compare(filter, 1, name, name.Length - fLen, fLen, true) == 0)) return true;
            }

            numArray1[0] = 0;
            int num7 = 1;
            int num1 = 0;
            int num8 = filter.Length * 2;

            while (!flag1) {
                int num3;
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;

                if (num1 < name.Length) {
                    ch1 = name[num1];
                    num3 = 1;
                    num1++;
                } else {
                    flag1 = true;
                    if (numArray1[num7 - 1] == num8) break;
                }

                while (num4 < num7) {
                    int num2 = (numArray1[num4++] + 1) / 2;
                    num3 = 0;
                Label_00F7:
                    if (num2 != filter.Length) {
                        num2 += num3;
                        num9 = num2 * 2;
                        if (num2 == filter.Length) {
                            numArray2[num5++] = num8;
                        } else {
                            ch2 = filter[num2];
                            num3 = 1;
                            if (num5 >= 14) {
                                int num11 = numArray2.Length * 2;
                                int[] numArray3 = new int[num11];
                                Array.Copy(numArray2, numArray3, numArray2.Length);
                                numArray2 = numArray3;
                                numArray3 = new int[num11];
                                Array.Copy(numArray1, numArray3, numArray1.Length);
                                numArray1 = numArray3;
                            }
                            if (ch2 == '*') {
                                numArray2[num5++] = num9;
                                numArray2[num5++] = num9 + 1;
                                goto Label_00F7;
                            }
                            if (ch2 == '>') {
                                bool flag2 = false;
                                if (!flag1 && (ch1 == '.')) {
                                    int num13 = name.Length;
                                    for (int num12 = num1; num12 < num13; num12++) {
                                        char ch3 = name[num12];
                                        num3 = 1;
                                        if (ch3 == '.') {
                                            flag2 = true;
                                            break;
                                        }
                                    }
                                }
                                if ((flag1 || (ch1 != '.')) || flag2) {
                                    numArray2[num5++] = num9;
                                    numArray2[num5++] = num9 + 1;
                                    goto Label_00F7;
                                }
                                numArray2[num5++] = num9 + 1;
                                goto Label_00F7;
                            }
                            num9 += (num3 * 2);
                            if (ch2 == '<') {
                                if (flag1 || (ch1 == '.')) goto Label_00F7;
                                numArray2[num5++] = num9;
                            } else {
                                if (ch2 == '"') {
                                    if (flag1) goto Label_00F7;
                                    if (ch1 == '.') {
                                        numArray2[num5++] = num9;
                                        goto Label_0294;
                                    }
                                }
                                if (!flag1) {
                                    if (ch2 == '?' || ch2 == ch1) numArray2[num5++] = num9;
                                }
                            }
                        }
                    }
                Label_0294:
                    if ((num4 < num7) && (num6 < num5)) {
                        while (num6 < num5) {
                            int num14 = numArray1.Length;
                            while ((num4 < num14) && (numArray1[num4] < numArray2[num6])) {
                                num4++;
                            }
                            num6++;
                        }
                    }
                }
                if (num5 == 0) return false;

                int[] numArray4 = numArray1;
                numArray1 = numArray2;
                numArray2 = numArray4;
                num7 = num5;
            }
            num9 = numArray1[num7 - 1];
            return (num9 == num8);
        }

        #endregion

        #region Contructors

        private FdblPatterns() { }

        #endregion

    }

}
