using System.IO;
using System.Security.Cryptography;

namespace Giant.Util
{
    public class RSAHelper
    {
        private static readonly int KeySize = 1024;

        private static string publicKey;
        private static string privateKey;
        private static RSACryptoServiceProvider RSA;

        public static string Encrypt(string data)
        {
            CheckKey();

            byte[] dataBytes = data.FromBase64String();
            byte[] encryptBytes = Encrypt(dataBytes);
            return encryptBytes.ToBase64String();
        }

        public static byte[] Encrypt(byte[] data)
        {
            CheckKey();

            return RSA.Encrypt(data, false);
        }

        public static byte[] Encrypt(byte[] data, string xmlPublicKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPublicKey);
            
            return provider.Encrypt(data, false);
        }

        public static byte[] Decrypt(byte[] data)
        {
            CheckKey();

            return RSA.Decrypt(data, false);
        }

        public static byte[] Decrypt(byte[] data, string xmlPrivateKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPrivateKey);
            return provider.Decrypt(data, false);
        }

        private static void CheckKey()
        {
            if (RSA == null)
            {
                LoadKey();
            }
        }

        private static void LoadKey()
        {
            string path = Directory.GetCurrentDirectory();
            string publicKeyPath = Path.Combine(path, "PublicKey.txt");
            string privateKeyPath = Path.Combine(path, "PrivateKey.txt");

            if (!File.Exists(privateKeyPath))
            {
                RSA = new RSACryptoServiceProvider(KeySize);
                publicKey = RSA.ToXmlString(false);
                privateKey = RSA.ToXmlString(true);

                File.WriteAllText(publicKeyPath, publicKey);
                File.WriteAllText(privateKeyPath, privateKey);
                return;
            }
            else
            {
                publicKey = File.ReadAllText(publicKeyPath);
                privateKey = File.ReadAllText(privateKeyPath);

                RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(privateKey);
            }
        }
    }
}
