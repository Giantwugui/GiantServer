using System.Security.Cryptography;

namespace Giant.Util
{
    public class AESCrypt
    {
        private readonly byte[] encryptKey;

        public string EncryptKey { get; private set; }

        public AESCrypt()
        {
            encryptKey = new byte[AESHelper.KeyLength128];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            provider.GetBytes(encryptKey);
            provider.Dispose();

            EncryptKey = encryptKey.ToBase64String();
        }

        public string Encrypt(string content)
        {
            return AESHelper.Encrypt(EncryptKey, content, CipherMode.ECB);
        }

        public byte[] Encrypt(byte[] content)
        {
            return AESHelper.Encrypt(encryptKey, content, CipherMode.ECB);
        }

        public string Decrypt(string content)
        {
            return AESHelper.Dencrypt(EncryptKey, content, CipherMode.ECB);
        }

        public byte[] Decrypt(byte[] content)
        {
            return AESHelper.Dencrypt(encryptKey, content, CipherMode.ECB);
        }
    }
}
