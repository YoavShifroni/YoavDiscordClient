using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordServer
{
    public static class RsaFunctions
    {
        public static RSAParameters PublicKey;
        public static RSAParameters PrivateKey;

        static RsaFunctions()
        {

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                PublicKey = rsa.ExportParameters(false);
                PrivateKey = rsa.ExportParameters(true);
            }
        }

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
