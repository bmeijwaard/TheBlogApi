using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Data.Providers.Contracts;

namespace TheBlogApi.Data.Providers
{
    public class CryptoProvider : ICryptoProvider
    {
        private readonly EncryptionSettings _settings;
        private readonly SHA512CryptoServiceProvider _Sha512CryptoServiceProvider;
        public CryptoProvider(IOptions<EncryptionSettings> settings)
        {
            _Sha512CryptoServiceProvider = new SHA512CryptoServiceProvider();
            _settings = settings.Value;
        }

        public string EncryptPublic(string input)
        {
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            var passwordBytes = Encoding.UTF8.GetBytes(_settings.Password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = EncryptAES(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);
            return result;
        }

        public string DecryptPublic(string input)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(_settings.Password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = DecryptAES(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);
            return result;
        }


        public string EncryptPrivate(string input)
        {
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            var passwordBytes = Encoding.UTF8.GetBytes(_settings.PrivatePassword);
            passwordBytes = _Sha512CryptoServiceProvider.ComputeHash(passwordBytes);

            var bytesEncrypted = EncryptAES(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);
            return result;
        }

        public string DecryptPrivate(string input)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(_settings.PrivatePassword);
            passwordBytes = _Sha512CryptoServiceProvider.ComputeHash(passwordBytes);

            byte[] bytesDecrypted = DecryptAES(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);
            return result;
        }


        private byte[] EncryptAES(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = _settings.Salt;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = _settings.KeySize;
                    AES.BlockSize = _settings.BlockSize;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Padding = PaddingMode.PKCS7;
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private byte[] DecryptAES(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            byte[] saltBytes = _settings.Salt;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    {
                        AES.KeySize = _settings.KeySize;
                        AES.BlockSize = _settings.BlockSize;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Padding = PaddingMode.PKCS7;
                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
        }
    }
}
