using System;
using System.Security.Cryptography;
using System.Text;

namespace I9.OnDemandEmail.Console.Common {

    internal class Crypto {

        #region Fields (Static)

        private static Random _Random = new Random();

        #endregion

        #region Constructors

        private Crypto() { }

        #endregion

        #region Methods - Public (Static)

        public static string ComputeSHA1Hash(string pwd, string salt) {

            HashAlgorithm alg = SHA1.Create();
            ASCIIEncoding ae = new ASCIIEncoding();

            byte[] hash = alg.ComputeHash(ae.GetBytes(pwd + salt));

            return BitConverter.ToString(hash).Replace("-", string.Empty);

        }

        public static string GetPasswordString(int keyLength) {

            string randKey = null;

            int intChar = 0;
            int haveUppercase = 0;
            int haveLowercase = 0;
            int haveNumber = 0;

            do {

                haveUppercase = 0;
                haveLowercase = 0;
                haveNumber = 0;

                randKey = string.Empty;

                while (randKey.Length < keyLength) {

                    intChar = _Random.Next(94) + 33;

                    if (intChar >= 65 && intChar <= 90) haveUppercase = 1;
                    else if (intChar >= 97 && intChar <= 122) haveLowercase = 1;
                    else if (intChar >= 48 && intChar <= 57) haveNumber = 1;
                    else intChar = 0;

                    if (intChar != 0) randKey += Convert.ToChar(intChar);

                }

            } while ((haveUppercase + haveLowercase + haveNumber) != 3);

            return randKey;

        }

        public static string GetSaltString(int keyLength) {

            string randKey = string.Empty;

            for (int i = 0; i < keyLength; i++) {

                randKey += Convert.ToChar(_Random.Next(94) + 33);

            }

            return randKey;

        }

        #endregion

    }

}