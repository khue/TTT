using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Fdbl.Toolkit.Crypto {

    #region Enumberations

    [FlagsAttribute]
    public enum FdblSymmetricType : byte {
        Rijndael,
        DES,
        TDES,
        RC2
    }

    #endregion

    public class FdblSymmetric {

        #region Members

        private SymmetricAlgorithm _CryptoService;

        private string _Salt;
        private string _Key;
        private string _LegalKey;

        private byte[] _IV;

        #endregion

        #region Properteis - Public

        public byte[] IV {
            get { return _IV; }
            set { _IV = value; }
        }

        public string Key {
            get { return _Key; }
            set { _Key = value; }
        }

        public string LegalKey {
            get { return _LegalKey; }
        }

        public string Salt {
            get { return _Salt; }
            set { _Salt = value; }
        }

        #endregion

        #region Methods - Public

        public void DecryptFile(string inFile, string outFile) {

            if (inFile == null) throw new ArgumentNullException("infile is null");
            if (inFile.Trim().Length == 0) throw new ArgumentException("infile is blank");

            if (outFile == null) throw new ArgumentNullException("outfile is null");
            if (outFile.Trim().Length == 0) throw new ArgumentException("outfile is blank");

            if (!File.Exists(inFile)) throw new FileNotFoundException(string.Format("infile not found: {0}", inFile));
            if (File.Exists(outFile)) File.Delete(outFile);

            using (FileStream fin = new FileStream(inFile, FileMode.Open, FileAccess.Read)) {

                using (FileStream fout = new FileStream(outFile, FileMode.Create, FileAccess.Write)) {

                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform cTransform = _CryptoService.CreateDecryptor();

                    using (CryptoStream cs = new CryptoStream(fout, cTransform, CryptoStreamMode.Write)) {

                        while (rdlen < totlen) {

                            len = fin.Read(bin, 0, 100);
                            cs.Write(bin, 0, len);
                            rdlen += len;

                        }

                    }

                }

            }

        }

        public string DecryptString(string data) {

            if (data == null) throw new ArgumentNullException("string is null");
            if (data.Trim().Length == 0) return string.Empty;

            string rText = string.Empty;
            byte[] cText = Convert.FromBase64String(data);

            _CryptoService.IV = _IV;
            _CryptoService.Key = _GenerateLegalKey();

            ICryptoTransform cTransform = _CryptoService.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(cText, 0, cText.Length)) {

                using (CryptoStream cs = new CryptoStream(ms, cTransform, CryptoStreamMode.Read)) {

                    using (StreamReader sr = new StreamReader(cs)) {

                        rText = sr.ReadToEnd();

                    }

                }

            }

            return rText;

        }

        public void EncryptFile(string inFile, string outFile) {

            if (inFile == null) throw new ArgumentNullException("infile is null");
            if (inFile.Trim().Length == 0) throw new ArgumentException("infile is blank");

            if (outFile == null) throw new ArgumentNullException("outfile is null");
            if (outFile.Trim().Length == 0) throw new ArgumentException("outfile is blank");

            if (!File.Exists(inFile)) throw new FileNotFoundException(string.Format("infile not found: {0}", inFile));
            if (File.Exists(outFile)) File.Delete(outFile);

            using (FileStream fin = new FileStream(inFile, FileMode.Open, FileAccess.Read)) {

                using (FileStream fout = new FileStream(outFile, FileMode.Create, FileAccess.Write)) {

                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform cTransform = _CryptoService.CreateEncryptor();

                    using (CryptoStream cs = new CryptoStream(fout, cTransform, CryptoStreamMode.Write)) {

                        while (rdlen < totlen) {

                            len = fin.Read(bin, 0, 100);
                            cs.Write(bin, 0, len);
                            rdlen += len;

                        }

                    }

                }

            }

        }

        public string EncryptString(string data) {

            if (data == null) throw new ArgumentNullException("string is null");
            if (data.Trim().Length == 0) return string.Empty;

            string rText = string.Empty;
            byte[] pText = ASCIIEncoding.ASCII.GetBytes(data);

            _CryptoService.IV = _IV;
            _CryptoService.Key = _GenerateLegalKey();

            ICryptoTransform cTransform = _CryptoService.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream()) {

                using (CryptoStream cs = new CryptoStream(ms, cTransform, CryptoStreamMode.Write)) {

                    cs.Write(pText, 0, pText.Length);
                    cs.FlushFinalBlock();

                    ms.Flush();

                    rText = Convert.ToBase64String(ms.ToArray());

                }

            }

            return rText;

        }

        #endregion

        #region Constructors

        public FdblSymmetric(FdblSymmetricType provider) : this(provider, string.Empty, string.Empty) { }

        public FdblSymmetric(FdblSymmetricType provider, string key) : this(provider, key, string.Empty) { }

        public FdblSymmetric(FdblSymmetricType provider, string key, string salt) {

            _Key = key;
            _Salt = salt;

            _LegalKey = null;

            _SetProvider(provider);
            _SetDefaultIV(provider);

            _CryptoService.Mode = CipherMode.CBC;

        }

        public FdblSymmetric(FdblSymmetricType provider, string key, string salt, byte[] iv) {

            _Key = key;
            _Salt = salt;

            _LegalKey = null;

            _SetProvider(provider);
            _IV = iv;

            _CryptoService.Mode = CipherMode.CBC;

        }

        #endregion

        #region Methods - Private

        private byte[] _GenerateLegalKey() {

            _LegalKey = _Key;

            if (_CryptoService.LegalKeySizes.Length > 0) {

                int keySize = _LegalKey.Length * 8;
                int minSize = _CryptoService.LegalKeySizes[0].MinSize;
                int maxSize = _CryptoService.LegalKeySizes[0].MaxSize;
                int skipSize = _CryptoService.LegalKeySizes[0].SkipSize;

                if (keySize > maxSize) {

                    _LegalKey = _LegalKey.Substring(0, maxSize / 8);

                } else if (keySize < maxSize) {

                    int validSize = (keySize <= minSize) ? minSize : (keySize - keySize % skipSize) + skipSize;

                    if (keySize < validSize) _LegalKey = _LegalKey.PadRight(validSize / 8, '*');

                }

            }

            PasswordDeriveBytes pwd = new PasswordDeriveBytes(_LegalKey, ASCIIEncoding.ASCII.GetBytes(_Salt));

            return pwd.GetBytes(_LegalKey.Length);

        }

        private void _SetDefaultIV(FdblSymmetricType provider) {

            if (provider == FdblSymmetricType.DES) {

                _IV = new byte[] { 0xeb, 0x89, 0xf6, 0xc, 0xb5, 0x2f, 0xb8, 0xf3 };

            } else if (provider == FdblSymmetricType.RC2) {

                _IV = new byte[] { 0x37, 0xfa, 0xe2, 0xa, 0x23, 0x7a, 0xf0, 0xbc };

            } else if (provider == FdblSymmetricType.Rijndael) {

                _IV = new byte[] { 0xa4, 0x1d, 0x20, 0x3f, 0xa6, 0xde, 0xea, 0xbf, 0x8, 0x37, 0x8, 0x32, 0x15, 0x4b, 0xfa, 0xee };

            } else if (provider == FdblSymmetricType.TDES) {

                _IV = new byte[] { 0x32, 0xac, 0xdc, 0x6, 0x8, 0x1a, 0xb2, 0xf7 };

            } else throw new ArgumentException("invalid provider type");

        }

        private void _SetProvider(FdblSymmetricType provider) {

            if (provider == FdblSymmetricType.DES) {

                _CryptoService = new DESCryptoServiceProvider();

            } else if (provider == FdblSymmetricType.RC2) {

                _CryptoService = new RC2CryptoServiceProvider();

            } else if (provider == FdblSymmetricType.Rijndael) {

                _CryptoService = new RijndaelManaged();

            } else if (provider == FdblSymmetricType.TDES) {

                _CryptoService = new TripleDESCryptoServiceProvider();

            } else throw new ArgumentException("invalid provider type");

        }

        #endregion

    }

}