using System;
using System.IO;
using System.Security.Cryptography;

namespace Giant.Share
{
    class RSAProvider
    {
        private readonly int KeySize = 1024;
        private readonly RSACryptoServiceProvider SACryptoServiceProvider;

        public byte[] PublicKey { get; private set; }
        public byte[] PrivateKey { get; private set; }

        public RSAProvider()
        {
            SACryptoServiceProvider = new RSACryptoServiceProvider(KeySize);
            PrivateKey = SACryptoServiceProvider.ExportCspBlob(true);
            PublicKey = SACryptoServiceProvider.ExportCspBlob(false);
        }
    }


    public class RSAHelper
    {
        private static RSA RSA;
        private static byte[] privateKey;
        private static byte[] publicKey;

        public static string PublicKey { get; private set; }
        public static string PrivateKey { get; private set; }

        static RSAHelper()
        {
            LoadKey();
            RSA = RSA.Create();
        }

        public static byte[] Encrypt(byte[] data)
        {
            return RSA.Encrypt(data, RSAEncryptionPadding.Pkcs1);
        }

        public static byte[] Decrypt(byte[] data)
        {
            return RSA.Decrypt(data, RSAEncryptionPadding.Pkcs1);
        }

        private static void LoadKey()
        {
            string path = Directory.GetCurrentDirectory();
            string publicKeyPath = Path.Combine(path, "PublicKey.txt");
            string privateKeyPath = Path.Combine(path, "PrivateKey.txt");

            if (!File.Exists(publicKeyPath) || !File.Exists(privateKeyPath))
            {
                RSAProvider provider = new RSAProvider();
                privateKey = provider.PrivateKey;
                publicKey = provider.PublicKey;

                PublicKey = Convert.ToBase64String(publicKey);
                PrivateKey = Convert.ToBase64String(privateKey);

                File.WriteAllText(publicKeyPath, PublicKey);
                File.WriteAllText(privateKeyPath, PrivateKey);
                return;
            }
            else
            {
                PublicKey = File.ReadAllText(publicKeyPath);
                PrivateKey = File.ReadAllText(privateKeyPath);

                publicKey = Convert.FromBase64String(PublicKey);
                privateKey = Convert.FromBase64String(PrivateKey);
            }
        }
    }
}
