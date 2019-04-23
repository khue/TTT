using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Fdbl.Toolkit.Crypto {

    public class FdblGeneric {

        #region Constants

        private const string _BaseConversionMap = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/*";

        #endregion

        #region Fields (Static)

        private static Random _Random = new Random();

        private static string[] _NumberArray = new string[] {
      "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
    };

        #endregion

        #region Methods - Public (Static)

        public double FromBase(string str, int baseNumber) {

            if (baseNumber < 2) return -1;
            if (baseNumber > 64) return -1;

            int strCol = 0;
            int strPos = str.Length - 1;
            double tmpVal = 0;

            do {

                tmpVal += (_GetBaseNumber(str.Substring(strPos, 1)) * (Math.Pow(baseNumber, strCol)));
                strCol++;
                strPos--;

            } while (!(strPos == -1));

            return tmpVal;

        }

        public static string GetKey(int keyLength) {

            keyLength = Math.Min(Math.Max(1, keyLength), 2048);

            int max = (int)Math.Max(1, Math.Round((double)(keyLength / 20)));
            HashAlgorithm ha = (HashAlgorithm)CryptoConfig.CreateFromName("MD5");
            UnicodeEncoding ue = new UnicodeEncoding();
            string key = "";

            for (int ndx = 1; ndx <= max; ndx++) {

                DateTime dt = System.DateTime.Now;

                dt.AddDays(GetMultiplierKey() * _Random.Next(1825));
                dt.AddHours(GetMultiplierKey() * _Random.Next(744));
                dt.AddSeconds(GetMultiplierKey() * _Random.Next());

                key += BitConverter.ToString(ha.ComputeHash(ue.GetBytes(Convert.ToString(dt)))).Replace("-", "");

            }

            return key.Substring(((int)((key.Length - keyLength) * _Random.NextDouble()) + 1), keyLength).ToLower();

        }

        public static int GetMultiplierKey() {
            return _Random.NextDouble() > 0.5 ? 1 : -1;
        }

        public static string GetPasswordString(int keyLength) {

            string randKey = null;
            bool isValid = false;

            do {

                randKey = "";

                for (int i = 0; i < keyLength; i++) {

                    randKey += _BaseConversionMap.Substring(_Random.Next(62), 1);

                }

                string stripKey = Utils.FdblStrings.ReplaceSubstring(randKey, _NumberArray, Utils.FdblStrings.ToArray(""), false);

                isValid = (stripKey.Length > 0 && stripKey.Length != randKey.Length);

            } while (!isValid);

            return randKey;

        }

        public static string GetSaltString(int keyLength) {

            string randKey = "";

            for (int i = 0; i < keyLength; i++) {

                randKey += Convert.ToChar(_Random.Next(94) + 32);

            }

            return randKey;

        }

        public static string ToBase(long str, int baseNumber) {

            if (baseNumber < 2) return "";
            if (baseNumber > 64) return "";

            string tmpStr = "";
            bool isFirst = str < baseNumber;
            bool isGreater = false;

            do {

                isGreater = (str / baseNumber) > baseNumber;
                tmpStr = string.Format("{0}{1}", _GetBaseCharacter(str % baseNumber), tmpStr);
                str = (int)(str / baseNumber);
                if (!isGreater && !isFirst) tmpStr = string.Format("{0}{1}", _GetBaseCharacter(str), tmpStr);
                isFirst = false;

            } while (!isGreater);

            return tmpStr;

        }

        #endregion

        #region Constructors

        private FdblGeneric() { }

        #endregion

        #region Methods - Private (Static)

        private static string _GetBaseCharacter(long ndx) {

            if (ndx < 0) return Convert.ToString(ndx);
            if (ndx >= _BaseConversionMap.Length) return Convert.ToString(ndx);

            return _BaseConversionMap.Substring(Convert.ToInt32(ndx), 1);

        }

        private static double _GetBaseNumber(string str) {

            int ndx = _BaseConversionMap.IndexOf(str);
            if (ndx == -1) return double.Parse(str);

            return ndx;

        }

        #endregion

    }

}