﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace YoavDiscordClient
{
    public static class AesFunctions
    {
        /// <summary>
        /// Static property that create the AES key and iv
        /// </summary>
        public static AesKeys AesKeys= new AesKeys();



        /// <summary>
        /// Encrypts a plain text string using the AES key and Iv, returning the result as a Base64-encoded string.
        /// </summary>
        /// <param name="plainText">The plain text string to encrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plainText"/> is null or empty.</exception>
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

        /// <summary>
        /// Decrypts a Base64-encoded encrypted string using the AES key and Iv, returning the original plain text.
        /// </summary>
        /// <param name="cipherText">The Base64-encoded string to decrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cipherText"/> is null or empty.</exception>
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


