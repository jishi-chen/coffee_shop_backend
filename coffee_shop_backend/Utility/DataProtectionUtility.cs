using System.Security.Cryptography;
using System.Text;

namespace coffee_shop_backend.Utility
{
    public class DataProtectionUtility
    {
        private static bool _isLoaded = false;
        private static string _keyStr = string.Empty;
        private static string _ivStr = string.Empty;


        private readonly int _keySize = 256;
        private readonly int _keyBlockSize = 128;
        private readonly CipherMode _cipherMode = CipherMode.CBC;
        private byte[] _key;
        private byte[] _iv;

        private DataProtectionUtility()
        {
            this.Initialize();
        }

        public static DataProtectionUtility Create()
        {
            // 讀取資料庫中的 Key, IV 值
            if (!_isLoaded)
            {
                //var configs = new DBSystemConfiguration().GetConfigList("EncryptKey", "EncryptIV");
                _isLoaded = true;

                //if (configs.Any(obj => obj.Name == "EncryptKey"))
                //    _keyStr = configs.Where(obj => obj.Name == "EncryptKey").Select(obj => obj.Value).First();

                //if (configs.Any(obj => obj.Name == "EncryptIV"))
                //    _ivStr = configs.Where(obj => obj.Name == "EncryptIV").Select(obj => obj.Value).First();
            }

            return new DataProtectionUtility();
        }

        private void Initialize()
        {
            if (!string.IsNullOrEmpty(_keyStr) && !string.IsNullOrEmpty(_ivStr))
            {
                if (_key == null)
                    _key = Convert.FromBase64String(_keyStr);
                if (_iv == null)
                    _iv = Convert.FromBase64String(_ivStr);
            }
            else
            {
                var keyData = Encoding.UTF8.GetBytes(_keyStr);
                // calculate SHA hash
                var hash = SHA512.Create().ComputeHash(keyData);
                var keyGenerator = new Rfc2898DeriveBytes(hash, hash, 1000);

                if (_key == null)
                    _key = keyGenerator.GetBytes(_keySize / 8);
                if (_iv == null)
                    _iv = keyGenerator.GetBytes(_keyBlockSize / 8);
            }
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            var plainData = Encoding.UTF8.GetBytes(plainText);
            var cipherData = Encrypt(plainData);
            return Convert.ToBase64String(cipherData);
        }

        public byte[] Encrypt(byte[] plainData)
        {
            byte[] cipherData;

            using (MemoryStream ms = new MemoryStream())
            {
                using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
                {
                    AES.KeySize = _keySize;
                    AES.BlockSize = _keyBlockSize;
                    AES.Mode = _cipherMode;
                    AES.Key = _key;
                    AES.IV = _iv;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainData, 0, plainData.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    cipherData = ms.ToArray();
                }
            }

            return cipherData;
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            try
            {
                var cipherData = Convert.FromBase64String(cipherText);
                var plainData = Decrypt(cipherData);
                return Encoding.UTF8.GetString(plainData);
            }
            catch (FormatException)
            {
                return "";
            }
            catch (CryptographicException)
            {
                return "";
            }
        }

        public byte[] Decrypt(byte[] cipherData)
        {
            byte[] plainData;

            using (MemoryStream ms = new MemoryStream())
            {
                using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
                {
                    AES.KeySize = _keySize;
                    AES.BlockSize = _keyBlockSize;
                    AES.Mode = _cipherMode;
                    AES.Key = _key;
                    AES.IV = _iv;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherData, 0, cipherData.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    plainData = ms.ToArray();
                }
            }

            return plainData;
        }

        public static byte[] GenerateKey()
        {
            var data = new byte[32];
            new RNGCryptoServiceProvider().GetNonZeroBytes(data);
            return data;
        }
    }
}
