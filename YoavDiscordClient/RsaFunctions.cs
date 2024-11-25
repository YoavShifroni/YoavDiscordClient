using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    /// <summary>
    /// Provides RSA encryption and decryption functionality using public and private keys.
    /// </summary>
    public static class RsaFunctions
    {
        /// <summary>
        /// The RSA public key used for encryption.
        /// </summary>
        public static RSAParameters PublicKey;

        /// <summary>
        /// The RSA private key used for decryption.
        /// </summary>
        public static RSAParameters PrivateKey;

        /// <summary>
        /// Static constructor to generate RSA public and private keys with a key size of 2048 bits.
        /// </summary>
        static RsaFunctions()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                PublicKey = rsa.ExportParameters(false);
                PrivateKey = rsa.ExportParameters(true);
            }
        }

        /// <summary>
        /// Encrypts a plain text string using the RSA public key.
        /// </summary>
        /// <param name="plainText">The text to be encrypted.</param>
        /// <returns>A Base64-encoded string representing the encrypted text.</returns>
        public static string Encrypt(string plainText)
        {
            byte[] encrypted;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(PublicKey);
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
                encrypted = rsa.Encrypt(dataToEncrypt, true);
            }
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Decrypts a Base64-encoded string using the RSA private key.
        /// </summary>
        /// <param name="cipherText">The Base64-encoded encrypted text.</param>
        /// <returns>The decrypted plain text string.</returns>
        public static string Decrypt(string cipherText)
        {
            byte[] decrypted;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(PrivateKey);
                byte[] dataToDecode = Convert.FromBase64String(cipherText);
                decrypted = rsa.Decrypt(dataToDecode, true);
            }
            return Encoding.UTF8.GetString(decrypted);
        }
    }

}
