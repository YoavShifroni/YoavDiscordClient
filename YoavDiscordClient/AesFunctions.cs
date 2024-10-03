using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace YoavDiscordClient
{
    public static class AesFunctions
    {
        public static AesKeys AesKeys= new AesKeys();

  

        // Encrypts a string and returns the encrypted data as a base64 string
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            using (Aes aes = Aes.Create())
            {
                aes.Key = AesKeys.Key;
                aes.IV = AesKeys.Iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // Decrypts a base64 string and returns the original string
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));

            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = AesKeys.Key;
                aes.IV = AesKeys.Iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(buffer))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        
    }
}


