using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;

namespace coffee_shop_backend.Utility
{
    public class DataCryptUtility
    {
        private readonly int _keySize = 256;
        private readonly int _keyBlockSize = 128;
        private static string _keyStr = "TAK2rsCCZyDY4hfn";
        private byte[] _key;
        private byte[] _iv;
        IDataProtector _protector;

        public DataCryptUtility(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("AspNetCoreDataProtectionDemo.v1");
            var hash = Encoding.UTF8.GetBytes(SHA256Encrypt(_keyStr));
            var keyGenerator = new Rfc2898DeriveBytes(hash, hash, 1000);
            _key = keyGenerator.GetBytes(_keySize / 8);
            _iv = keyGenerator.GetBytes(_keyBlockSize / 8);
        }

        public string AesEncrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            var plainData = Encoding.UTF8.GetBytes(plainText);
            var cipherData = AesEncrypt(plainData);

            return Convert.ToBase64String(cipherData);
        }

        public byte[] AesEncrypt(byte[] plainData)
        {
            byte[] cipherData;
            using(Aes aes = Aes.Create())
            {
                aes.KeySize = _keySize;
                aes.BlockSize = _keyBlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Key = _key;
                aes.IV = _iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainData, 0, plainData.Length);
                        cs.FlushFinalBlock();
                    }
                    cipherData = ms.ToArray();
                }
            }
            return cipherData;
        }

        public string AesDecrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            var cipherData = Convert.FromBase64String(cipherText);
            var plainData = AesDecrypt(cipherData);
            return Encoding.UTF8.GetString(plainData);
        }

        public byte[] AesDecrypt(byte[] cipherData)
        {
            byte[] plainData;
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = _keySize;
                aes.BlockSize = _keyBlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Key = _key;
                aes.IV = _iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherData, 0, cipherData.Length);
                        cs.FlushFinalBlock();
                    }
                    plainData = ms.ToArray();
                }
            }
            return plainData;
        }


        public string SHA256Encrypt(string plainText)
        {
            using (var sha256 = SHA256.Create())
            {
                var plainData = Encoding.UTF8.GetBytes(plainText);
                var hash = sha256.ComputeHash(plainData);
                return Convert.ToBase64String(hash);
            }
        }
        public string Encrypt(string data)
        {
            return _protector.Protect(data);
        }

        public string Decrypt(string data)
        {
            return _protector.Unprotect(data);
        }

        public static byte[] GenerateKey()
        {
            var data = new byte[32];
            RandomNumberGenerator.Fill(data);
            return data;
        }
    }

    
}
